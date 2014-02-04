using System.Collections.Generic;
using System.IO;
using System.Linq;

using GeneralAlgorithms.GeometryBase;
using GeneralAlgorithms.Sorting;

using IncrementalDelaunayTriangulation;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding.Polygons
{
    internal class FormPolygons : IMapBuilderComponent
    {
        public FormPolygons(Structure structure, INoiseLineGenerator noiseLineGenerator)
        {
            m_Structure = structure;
            m_NoiseLineGenerator = noiseLineGenerator;
        }

        private readonly Structure m_Structure;

        private readonly INoiseLineGenerator m_NoiseLineGenerator;

        public void Build(IMap map, MapSettings settings)
        {
            var pointsDict = new Dictionary<StructurePoint, MapPoint>();
            var trianglesDict = new Dictionary<Triangle, MapTriangle>();

            var points = new List<MapPoint>();
            foreach (var point in m_Structure.Points)
            {
                var mapPoint = new MapPoint(point);
                points.Add(mapPoint);

                var polygon = new Polygon(mapPoint);
                map.Polygons.Add(polygon);

                mapPoint.Polygon = polygon;
                pointsDict[point] = mapPoint;
            }

            var triangles = new List<MapTriangle>();
            foreach (var triangle in m_Structure.Triangles)
            {
                var mapTriangle = new MapTriangle(triangle.Id);
                triangles.Add(mapTriangle);

                var corner = new Corner(triangle.Center);
                map.Corners.Add(corner);

                mapTriangle.Corner = corner;
                trianglesDict[triangle] = mapTriangle;
            }

            foreach (var triangle in m_Structure.Triangles)
            {
                var mapTriangle = trianglesDict[triangle];

                for (int i = 0; i < 3; i++)
                {
                    var point = triangle.Points[i];
                    if (point != null)
                    {
                        mapTriangle.Points[i] = pointsDict[point];
                    }

                    var neighborTriangle = triangle.Triangles[i];
                    if (neighborTriangle != null)
                    {
                        mapTriangle.Triangles[i] = trianglesDict[neighborTriangle];
                    }
                }
            }

            foreach (var triangle in triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    triangle.Corner.Polygons[i] = triangle.Points[i].Polygon;
                    triangle.Points[i].Polygon.Corners.Add(triangle.Corner);
                }

                for (int i = 0; i < 3; i++)
                {
                    var edge = triangle.Edge(i);
                    var current = triangle.Triangles[i];

                    if (current != null)
                    {
                        triangle.Corner.Corners[i] = current.Corner;

                        // TODO: Remove check
                        if (triangle.Id == current.Id)
                        {
                            throw new InvalidDataException("Ids are equal");
                        }

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
                polygon.Corners.QuickSort((a, b) => Geometry.Vect(center, a, b).CompareTo(0));
                polygon.Borders.QuickSort((a, b) => Geometry.Vect(center, a.Center, b.Center).CompareTo(0));
                polygon.IsInside = polygon.Corners.All(map.ContainsPointInside);
            }

            foreach (var polygon in map.Polygons)
            {
                foreach (var border in polygon.Borders.Where(b => !b.BorderToDrawCreated))
                {
                    var borderPoints = m_NoiseLineGenerator.Generate(border.Corners[0], border.Polygons[0].BasePoint,
                                                             border.Corners[1], border.Polygons[1].BasePoint, 2, true);
                    /*var points = new List<Point2D> { border.Corners[0], border.Corners[1] };*/
                    /*border.Polygons[0].CornersToDraw.AddRange(points);
                    border.Polygons[1].CornersToDraw.AddRange(points);*/
                    border.BorderToDraw = borderPoints;
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
                        var borderPoints = new List<Point2D>(border.BorderToDraw);
                        borderPoints.Reverse();
                        polygon.CornersToDraw.AddRange(borderPoints);
                    }
                }
            }

            /*foreach (var polygon in map.Polygons)
            {
                var center = polygon.BasePoint;
                polygon.CornersToDraw.Sort((a, b) => Geometry.Vect(center, a, b).CompareTo(0));
            }*/
        }
    }
}
