using System;
using System.Collections.Generic;

namespace Triangulation
{
    static class NoiseLineGenerator
    {
        private static readonly RandomImproved RndGen = new RandomImproved();

        public static List<Point2D> Generate(Point2D a, Point2D b, Point2D c, Point2D d, double minLength, bool makeSmooth)
        {
            Point2D left, right;

            if (makeSmooth)
            {
                const int k = 1;

                var middle = (a + c) / 2;
                var smoothMultiplier = Math.Min(1, Geometry.Dist(a, c) / Geometry.Dist(b, middle)) * k;
                left = middle + (b - middle) * smoothMultiplier;
                smoothMultiplier = Math.Min(1, Geometry.Dist(a, c) / Geometry.Dist(d, middle)) * k;
                right = middle + (d - middle) * smoothMultiplier;
            }
            else
            {
                left = b;
                right = d;
            }

            var result = new List<Point2D>();

            result.Add(a);
            Subdivide(a, left, c, right, minLength, result);
            result.Add(c);

            return result;
        }

        private static void Subdivide(Point2D a, Point2D b, Point2D c, Point2D d, double minLength, ICollection<Point2D> currentResult)
        {
            if (a.Dist(c) < minLength || b.Dist(d) < minLength)
            {
                return;
            }

            double k = 0.2;
            var p = RndGen.NextDouble(k, 1 - k);
            var q = RndGen.NextDouble(k, 1 - k);

            var e = Geometry.Interpolate(a, d, p);
            var f = Geometry.Interpolate(b, c, p);

            var g = Geometry.Interpolate(a, b, q);
            var i = Geometry.Interpolate(d, c, q);

            var h = Geometry.Interpolate(e, f, q);

            double k2 = k / (1 - k);

            var s = 1 + RndGen.NextDouble(-0.4, 0.4);
            var t = 1 + RndGen.NextDouble(-0.4, 0.4);
            var m = 1 + RndGen.NextDouble(-0.4, 0.4);
            var n = 1 + RndGen.NextDouble(-0.4, 0.4);

            Subdivide(a, Geometry.Interpolate(g, a, s), h, Geometry.Interpolate(e, a, t), minLength, currentResult);
            currentResult.Add(h);
            Subdivide(h, Geometry.Interpolate(f, c, s), c, Geometry.Interpolate(i, c, t), minLength, currentResult);
        }
    }
}
