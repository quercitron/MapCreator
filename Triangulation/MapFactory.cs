using System;
using System.Collections.Generic;
using System.Linq;

using PerlinNoiseGeneration;

using Triangulation.Algorithm.PriorityQueue;
using Triangulation.MapObjects;

namespace Triangulation
{
    public class MapFactory
    {
        private readonly Structure m_Structure;

        private readonly Random m_Random = new Random();
        private readonly PerlinNoiseGenerator m_NoiseGenerator = new PerlinNoiseGenerator();

        public MapFactory(Structure structure)
        {
            m_Structure = structure;
        }

        public Map CreateMap(int seed = 0)
        {
            var map = new Map(m_Structure.Width, m_Structure.Height);

            FormPolygons(map);

            CalculateDistanceFromEdge(map);

            PerlinNoiseLandGenerator(map, seed);

            DefineWaterTypes(map);

            AssignCoast(map);

            CalculateElevation(map);

            AddRivers(map);

            CalculateMoisture(map);

            return map;
        }

        private void CalculateMoisture(Map map)
        {
            var queue = new PriorityQueue<Corner>((a, b) => -a.DistanceForMoisture.CompareTo(b.DistanceForMoisture));

            foreach (var corner in map.Corners)
            {
                if ( corner.IsLake || corner.IsRiver)
                {
                    corner.DistanceForMoisture = 0;
                    queue.Enqueue(corner);
                }
                else
                {
                    if (corner.IsOcean)
                    {
                        corner.DistanceForMoisture = 1e9;
                    }
                    else
                    {
                        corner.DistanceForMoisture = 1e9;
                    }
                }
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var corner in current.Corners)
                {
                    // TODO: Chack how can it be
                    if (corner != null)
                    {
                        var newDist = current.DistanceForMoisture + Geometry.Dist(current, corner);
                        if (corner.DistanceForMoisture > newDist)
                        {
                            corner.DistanceForMoisture = newDist;
                            queue.Enqueue(corner);
                        }
                    }
                }
            }

            foreach (var polygon in map.Polygons)
            {
                if (polygon.IsWater)
                {
                    polygon.DistanceForMoisture = 0;
                }
                else
                {
                    polygon.DistanceForMoisture = polygon.Corners.Average(c => c.DistanceForMoisture);
                }
            }
        }

        private void AddRivers(Map map)
        {
            const double areaPart = 0.1;
            var minStreamHeight = GetMinStreamHeight(map, areaPart);

            for (int i = 0; i < 20; i++)
            {                
                AddRiver(map, minStreamHeight);
            }
        }

        private void AddRiver(Map map, double minStreamHeight)
        {            
            if (map.Corners.Any(c => c.IsLand && c.Elevation > minStreamHeight))
            {
                Corner current;
                do
                {
                    current = map.GetRandomCorner();
                } while (!(current.IsLand && current.Elevation > minStreamHeight));

                current.IsRiver = true;

                double flow = 0;
                while (!current.IsOcean)
                {
                    Border minEdge = null;
                    Corner minNext = current;
                    foreach (var border in current.Borders)
                    {
                        var newCorner = border.OtherEnd(current);
                        if (newCorner.Elevation < minNext.Elevation)
                        {
                            minEdge = border;
                            minNext = newCorner;
                        }
                    }

                    if (minEdge == null)
                    {
                        break;
                    }

                    flow += 1;
                    minEdge.RiverCapacity += flow;
                    minNext.IsRiver = true;

                    current = minNext;
                }
            }
        }

        private static double GetMinStreamHeight(Map map, double areaPart)
        {
            double l = 0;
            double r = 1;

            while (r - l > 1e-7)
            {
                double m = (l + r) / 2;

                if (areaPart * map.Corners.Count > map.Corners.Count(c => c.IsLand && c.Elevation >= m))
                {
                    r = m;
                }
                else
                {
                    l = m;
                }
            }

            return l;
        }

        private void FormPolygons(Map map)
        {
            foreach (StructurePoint point in m_Structure.Points)
            {
                var polygon = new Polygon(point);
                point.Polygon = polygon;
                map.Polygons.Add(polygon);
            }

            foreach (Triangle triangle in m_Structure.Triangles)
            {
                var corner = new Corner(triangle.Center);
                triangle.Corner = corner;
                map.Corners.Add(corner);
            }

            foreach (Triangle triangle in m_Structure.Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    triangle.Corner.Polygons[i] = triangle.Points[i].Polygon;
                    triangle.Points[i].Polygon.Corners.Add(triangle.Corner);
                }

                for (int i = 0; i < 3; i++)
                {
                    Edge edge = triangle.Edge(i);
                    Triangle current = triangle.Triangles[i];

                    if (current != null)
                    {
                        triangle.Corner.Corners[i] = current.Corner;

                        if (triangle.Id < current.Id)
                        {
                            var border = new Border();
                            border.Corners[0] = current.Corner;
                            border.Corners[1] = triangle.Corner;
                            border.Polygons[0] = edge.First.Polygon;
                            border.Polygons[1] = edge.Second.Polygon;

                            border.BasePolygon = edge.Second.Polygon;

                            map.Borders.Add(border);

                            triangle.Corner.Borders.Add(border);
                            current.Corner.Borders.Add(border);

                            edge.First.Polygon.Borders.Add(border);
                            edge.Second.Polygon.Borders.Add(border);

                            edge.First.Polygon.Polygons.Add(edge.Second.Polygon);
                            edge.Second.Polygon.Polygons.Add(edge.First.Polygon);
                        }
                    }
                    else
                    {
                        edge.First.Polygon.Polygons.Add(edge.Second.Polygon);
                        edge.Second.Polygon.Polygons.Add(edge.First.Polygon);
                    }
                }
            }

            foreach (Polygon polygon in map.Polygons)
            {
                Point2D center = polygon.Center;
                polygon.Corners.Sort((a, b) => Geometry.Vect(center, a, b).CompareTo(0));
                polygon.Borders.Sort((a, b) => Geometry.Vect(center, a.Center, b.Center).CompareTo(0));
                polygon.IsInside = polygon.Corners.All(map.ContainsPointInside);
            }

            foreach (var polygon in map.Polygons)
            {
                foreach (var border in polygon.Borders.Where(b => !b.BorderToDrawCreated))
                {
                    var points = NoiseLineGenerator.Generate(border.Corners[0], border.Polygons[0].BasePoint,
                                                             border.Corners[1], border.Polygons[1].BasePoint, 2, true);
                    /*border.Polygons[0].CornersToDraw.AddRange(points);
                    border.Polygons[1].CornersToDraw.AddRange(points);*/
                    border.BorderToDraw = points;
                    border.BorderToDrawCreated = true;
                }
            }

            foreach (var polygon in map.Polygons)
            {
                foreach (var border in polygon.Borders)
                {
                    if (border.BasePolygon == polygon)
                    {
                        polygon.CornersToDraw.AddRange(border.BorderToDraw);
                    }
                    else
                    {
                        var points = new List<Point2D>(border.BorderToDraw);
                        points.Reverse();
                        polygon.CornersToDraw.AddRange(points);
                    }
                }
            }

            /*foreach (var polygon in map.Polygons)
            {
                var center = polygon.BasePoint;
                polygon.CornersToDraw.Sort((a, b) => Geometry.Vect(center, a, b).CompareTo(0));
            }*/
        }

        private void CalculateDistanceFromEdge(Map map)
        {
            var queue = new Queue<Polygon>();
            foreach (var polygon in map.Polygons)
            {
                if (polygon.IsInside)
                {
                    polygon.DistanceFromEdge = -1;
                }
                else
                {
                    polygon.DistanceFromEdge = 0;
                    queue.Enqueue(polygon);
                }
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var polygon in current.Polygons)
                {
                    if (polygon.DistanceFromEdge < 0)
                    {
                        polygon.DistanceFromEdge = current.DistanceFromEdge + 1;
                        map.MaxDistanceFromEdge = Math.Max(map.MaxDistanceFromEdge, current.DistanceFromEdge + 1);
                        queue.Enqueue(polygon);
                    }
                }
            }
        }

        private void PerlinNoiseLandGenerator(Map map, int seed)
        {
            var noise = m_NoiseGenerator.GenerateNoise((int)map.Width, (int)map.Height, seed, 8);

            var mapCenter = new Point2D(map.Width / 2, map.Height / 2);

            double k = 0.4;

            double baseK = 0.45;

            double l = 0;
            double r = 100000;

            while (r - l > 1e-5)
            {
                double m = (l + r) / 2;
                int count = 0;
                foreach (var polygon in map.Polygons)
                {
                    var center = polygon.Center;

                    var dist = center.Dist(mapCenter);

                    if (polygon.IsInside && map.ContainsPointInside(center))
                    {
                        var noiseValue = noise[(int) center.X, (int) center.Y];

                        if (noiseValue > (baseK + (dist > m ? (1 - baseK) * (dist / m - 1) : 0)))
                        {
                            count++;
                        }
                    }
                }

                if (count > k * map.Polygons.Count)
                {
                    r = m;
                }
                else
                {
                    l = m;
                }
            }

            foreach (var polygon in map.Polygons)
            {
                var center = polygon.Center;

                var dist = center.Dist(mapCenter);

                if (polygon.IsInside && map.ContainsPointInside(center))
                {
                    var noiseValue = noise[(int)center.X, (int)center.Y];
                    /*polygon.IsLand = noise[(int) center.X, (int) center.Y] * (map.Width / (4 * dist + map.Width)) > 0.25;*/
                    polygon.IsLand = noiseValue > (baseK + (dist > l ? (1 - baseK) * (dist / l - 1) : 0));
                }
                else
                {
                    polygon.IsLand = false;
                }
            }
        }

        private void CreateLand(Map map)
        {
            AddIslands(map);           
        }

        private void AddIslands(Map map)
        {
            int totalLength = (int) Math.Sqrt(map.Polygons.Count) / 2;
            int width = totalLength / 4;

            int[] sizes = new[] {1, 2, 2, 3, 4, 5, 8, 8, 8, 8};

            foreach (int size in sizes)
            {
                AddIsland(map, totalLength / size, width / size);
            }
        }

        private void AddIsland(Map map, int totalLength, int width)
        {
            if (map.Polygons.Any(p => p.IsInside && p.DistanceFromEdge >= width))
            {
                Queue<Polygon> queue = new Queue<Polygon>();

                var direction = Geometry.GetRandomUnitVector();

                Polygon current;
                do
                {
                    current = map.GetRandomPolygon();
                } while (!(current.IsInside && current.DistanceFromEdge >= width));

                CreateSkeleton(width, direction, current, totalLength, queue, m_Random.Next(2) == 0);

                while (queue.Count > 0)
                {
                    current = queue.Dequeue();
                    if (current.DistanceFromSkeleton < width)
                    {
                        foreach (var polygon in current.Polygons)
                        {
                            if (polygon.IsInside && !polygon.InSkeleton && polygon.DistanceFromEdge > 0)
                            {
                                polygon.InSkeleton = true;
                                polygon.IsLand = true;

                                polygon.DistanceFromSkeleton = current.DistanceFromSkeleton + 1;
                                queue.Enqueue(polygon);
                            }
                        }
                    }
                }
            }

            foreach (var polygon in map.Polygons)
            {
                polygon.InSkeleton = false;
            }
        }

        private void CreateSkeleton(int width, Point2D direction, Polygon current, int totalLength, Queue<Polygon> queue,
                                    bool addAtMiddle)
        {
            current.InSkeleton = true;
            current.DistanceFromSkeleton = 0;
            current.IsLand = true;

            Polygon middlePolygon = null;
            Point2D newBaseDirection = null;

            for (int i = 0; i < totalLength - 1; i++)
            {
                Polygon nextPolygon = null;
                double cos = 0;
                foreach (var polygon in current.Polygons)
                {
                    if (polygon.IsInside && !polygon.InSkeleton && polygon.DistanceFromEdge > width)
                    {
                        var newCos = Geometry.Cos(direction, polygon.Center - current.Center);
                        if (nextPolygon == null || cos < newCos)
                        {
                            nextPolygon = polygon;
                            cos = newCos;
                        }
                    }
                }
                if (nextPolygon != null)
                {
                    nextPolygon.InSkeleton = true;
                    nextPolygon.DistanceFromSkeleton = 0;
                    nextPolygon.IsLand = true;
                    queue.Enqueue(nextPolygon);
                    direction = nextPolygon.Center - current.Center;
                    current = nextPolygon;
                    if (i == totalLength / 2)
                    {
                        middlePolygon = current;
                        newBaseDirection = new Point2D(direction.Y, -direction.X);
                        if (m_Random.Next(2) == 0)
                        {
                            newBaseDirection = -newBaseDirection;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            if (addAtMiddle && middlePolygon != null)
            {
                CreateSkeleton(width, newBaseDirection, middlePolygon, totalLength / 2, queue, m_Random.Next(2) == 0);
            }
        }

        private void DefineWaterTypes(Map map)
        {
            foreach (var polygon in map.Polygons)
            {
                if (!polygon.IsInside)
                {
                    polygon.IsOcean = true;

                    var queue = new Queue<Polygon>();
                    queue.Enqueue(polygon);

                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();
                        foreach (
                            var neighbor in current.Polygons.Where(neighbor => !neighbor.IsLand && !neighbor.IsOcean))
                        {
                            neighbor.IsOcean = true;
                            queue.Enqueue(neighbor);
                        }
                    }

                    break;
                }
            }
        }

        private void AssignCoast(Map map)
        {
            foreach (var border in map.Borders)
            {
                if (border.Polygons[0].IsLand != border.Polygons[1].IsLand)
                {
                    if (border.Polygons[0].IsOcean || border.Polygons[1].IsOcean)
                    {
                        border.IsOceanCoast = true;
                        for (int i = 0; i < 2; i++)
                        {
                            border.Corners[i].IsOceanCoast = true;
                            border.Polygons[i].IsOceanCoast = true;
                        }
                    }
                    else
                    {
                        border.IsLakeCoast = true;
                        for (int i = 0; i < 2; i++)
                        {
                            border.Corners[i].IsLakeCoast = true;
                            border.Polygons[i].IsLakeCoast = true;
                        }
                    }
                }
            }

            foreach (var corner in map.Corners)
            {
                corner.IsLand = corner.Polygons.Any(p => p.IsLand);
                corner.IsWater = corner.Polygons.Any(p => !p.IsLand);
                corner.IsOcean = corner.Polygons.Any(p => p.IsOcean);
            }

            foreach (var corner in map.Corners)
            {
                if (corner.IsWater && !corner.IsOcean)
                {
                    corner.IsLake = true;
                }
            }
        }

        private void CalculateElevation(Map map)
        {
            Comparison<Corner> comparison = (a, b) => -a.Elevation.CompareTo(b.Elevation);
            var queue = new PriorityQueue<Corner>(comparison);

            foreach (var corner in map.Corners)
            {
                if (!corner.IsOcean || corner.IsOceanCoast)
                {
                    if (corner.IsOceanCoast)
                    {
                        corner.Elevation = 0;
                        queue.Enqueue(corner);
                    }
                    else
                    {
                        corner.Elevation = 1e10;
                    }
                }
                else
                {
                    corner.Elevation = 0;
                }
            }

            /*while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var corner in current.Corners)
                {
                    // TODO: check why can corner be null
                    if (corner != null && corner.IsLand && corner.Elevation < 0)
                    {
                        corner.Elevation = current.Elevation + Geometry.Dist(current, corner) ;
                        queue.Enqueue(corner);
                    }
                }
            }*/

            var noise = m_NoiseGenerator.GeneratePolarNoise((int)map.Width, (int)map.Height, m_Random.Next(), 4);

            /*foreach (var corner in map.Corners)
            {
                if (corner.IsLand)
                {
                    corner.Elevation = (corner.Elevation + 1) * noise[(int) corner.X, (int) corner.Y];
                }
            }

            NormalizeElevation(map);*/

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var corner in current.Corners.Where(map.ContainsPointInside))
                {
                    var midPoint = (current + corner) / 2;
                    var newDist = current.Elevation + Geometry.Dist(current, corner) * noise[(int) midPoint.X, (int) midPoint.Y];
                    if (corner.Elevation > newDist)
                    {
                        corner.Elevation = newDist;
                        queue.Enqueue(corner);

                        if (corner.IsLake)
                        {
                            const double smallStep = 1e-6;
                            double bigStep = 2 * map.Corners.Count * smallStep;

                            var lakeQueue = new Queue<Corner>();
                            lakeQueue.Enqueue(corner);
                            while (lakeQueue.Count > 0)
                            {
                                var lakeCorner = lakeQueue.Dequeue();
                                foreach (var c in lakeCorner.Corners)
                                {
                                    if (c.IsLake && c.Elevation > lakeCorner.Elevation + bigStep)
                                    {
                                        c.Elevation = lakeCorner.Elevation + smallStep;
                                        lakeQueue.Enqueue(c);
                                        queue.Enqueue(c);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            

            NormalizeElevation(map);            

            foreach (var polygon in map.Polygons)
            {
                polygon.Elevation = polygon.IsLand ? polygon.Corners.Average(p => p.Elevation) : 0;
            }            
        }

        private static void NormalizeElevation(Map map)
        {
            var maxElevation = map.GetMaxElevation;
            if (maxElevation > 0)
            {
                foreach (var corner in map.Corners)
                {
                    corner.Elevation /= maxElevation;
                    corner.Elevation *= corner.Elevation;
                }
            }
        }
    }
}
