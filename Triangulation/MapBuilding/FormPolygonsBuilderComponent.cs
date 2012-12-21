using System.Collections.Generic;
using System.Linq;

using Triangulation.Algorithm.GeometryBase;
using Triangulation.Dividing;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class FormPolygonsBuilderComponent : IMapBuilderComponent
    {
        public FormPolygonsBuilderComponent(Structure structure, INoiseLineGenerator noiseLineGenerator)
        {
            this.m_Structure = structure;
            m_NoiseLineGenerator = noiseLineGenerator;
        }

        private readonly Structure m_Structure;

        private readonly INoiseLineGenerator m_NoiseLineGenerator;

        public void Build(IMap map)
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
                    /*var points = m_NoiseLineGenerator.Generate(border.Corners[0], border.Polygons[0].BasePoint,
                                                             border.Corners[1], border.Polygons[1].BasePoint, 2, true);*/
                    var points = new List<Point2D> { border.Corners[0], border.Corners[1] };
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
    }
}
