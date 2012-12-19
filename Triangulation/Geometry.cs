﻿using System;

namespace Triangulation
{
    public static class Geometry
    {
        private static readonly Random Rnd = new Random();

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

        public static double Vect(Edge edge, Point2D point)
        {
            return Vect(edge.First, edge.Second, point);
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
            double x = Rnd.NextDouble() - 0.5;
            double y = Rnd.NextDouble() - 0.5;
            return new Point2D(x, y) / Math.Sqrt(x * x + y * y);
        }
    }
}
