using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GeneralAlgorithms.GeometryBase;

namespace IncrementalDelaunayTriangulation
{
    public class Structure
    {
        public Structure(double maxX, double maxY)
        {
            _triangles = new List<Triangle>();
            _points = new List<StructurePoint>();

            Width = maxX;
            Height = maxY;

            InitializeBoard(maxX, maxY);
        }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public ReadOnlyCollection<Triangle> Triangles
        {
            get { return _triangles.AsReadOnly(); }
        }

        public ReadOnlyCollection<StructurePoint> Points
        {
            get { return _points.AsReadOnly(); }
        }

        public void AddPoint(Point2D point)
        {
            AddNewPoint(point);
        }

        public void AddPointRange(IEnumerable<Point2D> points)
        {
            foreach (var point in points)
            {
                AddNewPoint(point);
            }
        }

        private static readonly Random Rnd = new Random();

        private readonly Queue<Triangle> m_TrianglesToCheck = new Queue<Triangle>();

        private TriangleHash m_Hash;

        private readonly List<Triangle> _triangles;

        private List<StructurePoint> _points;

        private void AddNewPoint(Point2D newPoint)
        {
            var point = new StructurePoint(newPoint);

            _points.Add(point);

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

                        if (!Ok(current.Points[i], current.Points[(i + 1) % 3], opPoint, current.Points[(i + 2) % 3]))
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
            var current = m_Hash.GetTriangle(point);

            while (!current.ContainsPoint(point))
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

                    if (Vect(edge, triangle.Points[i]) * Vect(edge, target) <= 0)
                    {
                        return triangle.Triangles[i];
                    }
                }
            }

            throw new ApplicationException("Coudn't find next Triangle.");
        }

        private void InitializeBoard(double maxX, double maxY)
        {
            const int border = 10000;
            var lb = new StructurePoint(-border, -border) {IsDummy = true};
            var lt = new StructurePoint(-border, maxY + border) {IsDummy = true};
            var rb = new StructurePoint(maxX + border, -border) {IsDummy = true};
            var rt = new StructurePoint(maxX + border, maxY + border) {IsDummy = true};

            _points.AddRange(new[] {lb, lt, rb, rt});

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
            triangle.Id = _triangles.Count;
            _triangles.Add(triangle);
        }

        private double Vect(Edge edge, Point2D point)
        {
            return Geometry.Vect(edge.First, edge.Second, point);
        }
    }
}