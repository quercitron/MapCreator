using Triangulation.MapObjects;

namespace Triangulation
{
    public class StructurePoint : Point2D
    {
        public StructurePoint(double x, double y)
            : base(x, y)
        {
        }

        public StructurePoint(Point2D point)
            : base(point.X, point.Y)
        {
        }

        public bool IsDummy { get; set; }

        public Polygon Polygon { get; set; }
    }
}
