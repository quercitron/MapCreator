using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Triangulation.Algorithm.GeometryBase;

namespace Triangulation.Dividing
{
    public class Structure
    {
        private static readonly Random Rnd = new Random();
        private readonly Queue<Triangle> m_TrianglesToCheck = new Queue<Triangle>();

        public EventHandler StructureChanged;
        private TriangleHash m_Hash;

        public Structure(double maxX, double maxY)
        {
            Triangles = new List<Triangle>();
            Points = new List<StructurePoint>();

            Width = maxX;
            Height = maxY;

            CreateBoard(maxX, maxY);
        }

        public List<Triangle> Triangles { get; set; }

        public List<StructurePoint> Points { get; set; }

        public long LastActionLength { get; private set; }

        public long LandCreation { get; private set; }

        public long MapBuilding { get; private set; }        

        public double Width { get; private set; }

        public double Height { get; private set; }

        public void AddPoint(Point2D point)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            AddNewPoint(point);

            stopwatch.Stop();

            LastActionLength = stopwatch.ElapsedMilliseconds;

            if (StructureChanged != null)
            {
                StructureChanged(this, new EventArgs());
            }
        }

        public void AddPointRange(IEnumerable<Point2D> points)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            foreach (Point2D point in points)
            {
                AddNewPoint(point);
            }

            stopwatch.Stop();

            LastActionLength = stopwatch.ElapsedMilliseconds;

            if (StructureChanged != null)
            {
                StructureChanged(this, new EventArgs());
            }
        }

        private void AddNewPoint(Point2D newPoint)
        {
            var point = new StructurePoint(newPoint);

            Points.Add(point);

            Triangle triangle = SearchTriangle(point);

            var newTriangles = new Triangle[3];

            for (int i = 0; i < 2; i++)
            {
                newTriangles[i] = new Triangle();
            }
            newTriangles[2] = triangle;

            for (int i = 0; i < 3; i++)
            {
                Edge edge = triangle.Edge(i);
                newTriangles[i].Points = new[] {edge.First, edge.Second, point};
                newTriangles[i].Triangles = new[] {
                                                      newTriangles[(i + 1) % 3],
                                                      newTriangles[(i + 2) % 3],
                                                      triangle.Triangles[i]
                                                  };
                if (triangle.Triangles[i] != null)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (triangle.Triangles[i].Triangles[j] == triangle)
                        {
                            triangle.Triangles[i].Triangles[j] = newTriangles[i];
                            break;
                        }
                    }
                }
            }

            /*newTriangles[2].Id = triangle.Id;
            Triangles[triangle.Id] = newTriangles[2];
            m_TrianglesToCheck.Enqueue(newTriangles[2]);*/

            for (int i = 0; i < 2; i++)
            {
                AddTriangle(newTriangles[i]);
                m_TrianglesToCheck.Enqueue(newTriangles[i]);
            }
            m_TrianglesToCheck.Enqueue(newTriangles[2]);

            CheckTriangles();
        }

        private void CheckTriangles()
        {
            while (m_TrianglesToCheck.Count > 0)
            {
                Triangle current = m_TrianglesToCheck.Dequeue();

                for (int i = 0; i < 3; i++)
                {
                    Triangle neighbor = current.Triangles[i];

                    if (neighbor != null)
                    {
                        StructurePoint opPoint = null;
                        int opId = 0;
                        for (int j = 0; j < 3; j++)
                        {
                            if (neighbor.Triangles[j] == current)
                            {
                                opPoint = neighbor.Points[j];
                                opId = j;
                                break;
                            }
                        }

                        if (
                            !Ok(current.Points[i], current.Points[(i + 1) % 3], opPoint,
                                current.Points[(i + 2) % 3]))
                        {
                            Triangle tempTriangle = current.Triangles[(i + 2) % 3];

                            current.Points[(i + 1) % 3] = opPoint;
                            neighbor.Points[(opId + 1) % 3] = current.Points[i];

                            current.SetNeighbor(i, neighbor.Triangles[(opId + 2) % 3]);
                            current.SetNeighbor((i + 2) % 3, neighbor);

                            neighbor.SetNeighbor(opId, tempTriangle);

                            m_TrianglesToCheck.Enqueue(current);
                            m_TrianglesToCheck.Enqueue(neighbor);

                            break;
                        }
                    }
                }
            }
        }

        private bool Ok(Point2D a, Point2D b, Point2D c, Point2D d)
        {
            return Geometry.Vect(a, b, d) * Geometry.Scal(c, d, b) + Geometry.Vect(c, d, b) * Geometry.Scal(a, b, d) >= 0;
        }

        private Triangle SearchTriangle(Point2D point)
        {
            /*Triangle current = Triangles[Rnd.Next(Triangles.Count)];*/

            Triangle current = m_Hash.GetTriangle(point);

            while (!current.Contain(point))
            {
                current = GetNext(current, point);
            }

            m_Hash.AddTriangle(point, current);

            return current;
        }

        private Triangle GetNext(Triangle triangle, Point2D target)
        {
            for (int i = 0; i < 3; i++)
            {
                if (triangle.Triangles[i] != null)
                {
                    Edge edge = triangle.Edge(i);

                    if (Geometry.Vect(edge, triangle.Points[i]) * Geometry.Vect(edge, target) <= 0)
                    {
                        return triangle.Triangles[i];
                    }
                }
            }

            throw new ApplicationException("Coudn't find next Triangle.");
        }

        private void CreateBoard(double maxX, double maxY)
        {
            const int border = 10000;
            var lb = new StructurePoint(-border, -border) {IsDummy = true};
            var lt = new StructurePoint(-border, maxY + border) {IsDummy = true};
            var rb = new StructurePoint(maxX + border, -border) {IsDummy = true};
            var rt = new StructurePoint(maxX + border, maxY + border) {IsDummy = true};

            Points.AddRange(new[] {lb, lt, rb, rt});

            var top = new Triangle();
            var bottom = new Triangle();

            top.Points = new[] {lb, rb, rt};
            top.Triangles = new[] {null, bottom, null};

            bottom.Points = new[] {rt, lt, lb};
            bottom.Triangles = new[] {null, top, null};

            AddTriangle(top);
            AddTriangle(bottom);

            m_Hash = new TriangleHash(maxX, maxY, top, 2);
        }

        private void AddTriangle(Triangle triangle)
        {
            triangle.Id = Triangles.Count();
            Triangles.Add(triangle);
        }
    }
}