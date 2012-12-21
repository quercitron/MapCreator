using System.Collections.Generic;

using Triangulation.Algorithm.GeometryBase;

namespace Triangulation
{
    internal interface INoiseLineGenerator
    {
        List<Point2D> Generate(Point2D a, Point2D b, Point2D c, Point2D d, double minLength, bool makeSmooth);
    }
}