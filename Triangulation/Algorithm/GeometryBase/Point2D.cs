using System;
using System.Drawing;

namespace Triangulation.Algorithm.GeometryBase
{
    public class Point2D
    {
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point2D(Point2D point)
            : this(point.X, point.Y)
        {
        }

        public double X { get; set; }
        public double Y { get; set; }

        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }           
        }

        public static explicit operator Point(Point2D point2D)
        {
            return new Point((int) point2D.X, (int) point2D.Y);
        }

        public static implicit operator Point2D(Point point)
        {
            return new Point2D(point.X, point.Y);
        }

        public static explicit operator PointF(Point2D point2D)
        {
            return new PointF((float) point2D.X, (float)point2D.Y);
        }

        public static implicit operator Point2D(PointF point)
        {
            return new Point2D(point.X, point.Y);
        }

        public double Dist(Point2D point)
        {
            return Geometry.Dist(this, point);
        }

        public static Point2D operator +(Point2D a, Point2D b)
        {
            return new Point2D(a.X + b.X, a.Y + b.Y);
        }

        public static Point2D operator -(Point2D a, Point2D b)
        {
            return new Point2D(a.X - b.X, a.Y - b.Y);
        }

        public static Point2D operator -(Point2D a)
        {
            return new Point2D(-a.X, -a.Y);
        }

        public static Point2D operator *(double k, Point2D p)
        {
            return new Point2D(k * p.X, k * p.Y);
        }

        public static Point2D operator *(Point2D p, double k)
        {
            return new Point2D(k * p.X, k * p.Y);
        }

        public static Point2D operator /(Point2D p, double k)
        {
            return new Point2D(p.X / k, p.Y / k);
        }
    }
}