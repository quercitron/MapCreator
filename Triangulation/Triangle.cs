using System.Linq;
using Triangulation.MapObjects;

namespace Triangulation
{
    public class Triangle
    {        
        public StructurePoint[] Points { get; set; }

        public Triangle[] Triangles { get; set; }

        public int Id { get; set; }

        public Edge Edge(int i)
        {
            return new Edge(Points[(i + 1) % 3], Points[(i + 2) % 3]);
        }

        public Corner Corner { get; set; }

        public Point2D Center
        {
            get
            {
                double a = (Points[1].X * Points[1].X - Points[0].X * Points[0].X + Points[1].Y * Points[1].Y - Points[0].Y * Points[0].Y) / 2;
                double b = (Points[2].X * Points[2].X - Points[0].X * Points[0].X + Points[2].Y * Points[2].Y - Points[0].Y * Points[0].Y) / 2;
                double d = Geometry.Vect(Points[0], Points[1], Points[2]);

                double x = ((Points[2].Y - Points[0].Y) * a - (Points[1].Y - Points[0].Y) * b) / d;
                double y = (-(Points[2].X - Points[0].X) * a + (Points[1].X - Points[0].X) * b) / d;

                return new Point2D(x, y);
            }
        }

        public bool Contain(Point2D point)
        {
            double[] vects = new double[3];

            for (int i = 0; i < 3; i++)
            {
                vects[i] = Geometry.Vect(Points[i], Points[(i + 1) % 3], point);
            }

            return vects.All(s => s >= 0);
        }

        public void SetNeighbor(int index, Triangle newTriangle)
        {
            if (newTriangle != null)
            {
                int last = 0;
                Edge edge = this.Edge(index);
                for (int i = 0; i < 3; i++)
                {                    
                    if (edge.First != newTriangle.Points[i] && edge.Second != newTriangle.Points[i])
                    {
                        last = i;
                        break;
                    }
                }

                newTriangle.Triangles[last] = this;
            }

            Triangles[index] = newTriangle;
        }
    }
}
