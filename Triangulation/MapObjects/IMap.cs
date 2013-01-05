using System.Collections.Generic;

using Triangulation.Algorithm.GeometryBase;

namespace Triangulation.MapObjects
{
    public interface IMap
    {
        List<Polygon> Polygons { get; }

        List<Corner> Corners { get; }

        List<Border> Borders { get; }

        double Width { get; }

        double Height { get; }

        int MaxDistanceFromEdge { get; set; }

        double GetMaxCornerElevation { get; }

        double Diagonal { get; }

        bool ContainsPointInside(Point2D point);

        Corner GetRandomCorner();

        Polygon GetRandomPolygon();

        string CreationReport { get; set; }
    }
}