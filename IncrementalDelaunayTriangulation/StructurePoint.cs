using GeneralAlgorithms.GeometryBase;

namespace IncrementalDelaunayTriangulation
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

        public StructurePoint(StructurePoint point)
            : base(point)
        {
            IsDummy = point.IsDummy;
        }

        public bool IsDummy { get; set; }

        public StructurePoint Copy()
        {
            return new StructurePoint(this);
        }
    }
}
