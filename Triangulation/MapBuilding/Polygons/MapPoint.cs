using GeneralAlgorithms.GeometryBase;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding.Polygons
{
    class MapPoint : Point2D
    {
        public MapPoint(double x, double y)
            : base(x, y)
        {
        }

        public MapPoint(Point2D point)
            : base(point)
        {
        }

        public Polygon Polygon { get; set; }
    }
}
