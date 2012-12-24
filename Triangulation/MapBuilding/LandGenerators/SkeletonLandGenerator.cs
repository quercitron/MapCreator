using System;
using System.Collections.Generic;
using System.Linq;
using Triangulation.Algorithm.GeometryBase;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding.LandGenerators
{
    internal class SkeletonLandGenerator : IMapBuilderComponent
    {
        public SkeletonLandGenerator(int seed)
        {
            m_Random = new Random(seed);
        }

        // Change random
        private readonly Random m_Random;

        public void Build(IMap map, MapSettings settings)
        {
            AddIslands(map);
        }

        private void AddIslands(IMap map)
        {
            int totalLength = (int)Math.Sqrt(map.Polygons.Count) / 2;
            int width = totalLength / 4;

            int[] sizes = new[] { 1, 2, 2, 3, 4, 5, 8, 8, 8, 8 };

            foreach (int size in sizes)
            {
                AddIsland(map, totalLength / size, width / size);
            }
        }

        private void AddIsland(IMap map, int totalLength, int width)
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
    }
}
