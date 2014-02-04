using System;

namespace GeneralAlgorithms.GeometryBase
{
    public static class Geometry
    {
        private static readonly Random m_Rnd = new Random();

        public static double Scal(Point2D a, Point2D b, Point2D c)
        {
            return (b.X - a.X)*(c.X - a.X) + (b.Y - a.Y)*(c.Y - a.Y);
        }

        private static double Scal(Point2D a, Point2D b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static double Vect(Point2D a, Point2D b, Point2D c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        public static double Dist(Point2D a, Point2D b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        public static Point2D Interpolate(Point2D a, Point2D b, double k)
        {
            return (1 - k) * a + k * b;
        }

        public static double Cos(Point2D a, Point2D b)
        {
            return Scal(a, b) / a.Length / b.Length;
        }  
      
        public static Point2D GetRandomUnitVector()
        {
            double x = m_Rnd.NextDouble() - 0.5;
            double y = m_Rnd.NextDouble() - 0.5;
            return new Point2D(x, y) / Math.Sqrt(x * x + y * y);
        }

        // TODO: Finish
        public static bool SegmentsIntersects(Point2D a, Point2D b, Point2D c, Point2D d)
        {
            bool intersect1 = (Vect(a, b, c) * Vect(a, b, d) < 0);
            bool intersect2 = (Vect(c, d, a) * Vect(c, d, b) < 0);
            return intersect1 && intersect2;
        }

        public static double SpecificDist(Point2D center, Point2D p, double width, double height)
        {
            double dx = (p.X - center.X) / (width / 2);
            double dy = (p.Y - center.Y) / (height / 2);
            return Math.Sqrt(dx*dx + dy*dy);
        }
    }
}
