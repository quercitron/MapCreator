using System.Collections.Generic;

using GeneralAlgorithms.GeometryBase;

namespace Triangulation.MapObjects
{
    public class Border
    {
        public Border()
        {
            Init();
        }

        private void Init()
        {
            Polygons = new Polygon[2];
            Corners = new Corner[2];
        }

        public Polygon[] Polygons { get; private set; }
        public Corner[] Corners { get; private set; }

        public bool IsOceanCoast { get; set; }
        public bool IsLakeCoast { get; set; }

        public bool BorderToDrawCreated { get; set; }

        public double RiverCapacity { get; set; }

        public Point2D Center
        {
            get { return (Corners[0] + Corners[1]) / 2; }
        }

        public List<Point2D> BorderToDraw { get; set; }
        public Polygon BasePolygon { get; set; }

        public bool IsCoast
        {
            get { return IsLakeCoast || IsOceanCoast; }
        }

        public Corner OtherEnd(Corner current)
        {
            if (Corners[0] == current)
            {
                return Corners[1];
            }
            return Corners[0];
        }

        public double Weight { get; set; }

        public bool IsLake
        {
            get { return Polygons[0].IsLake || Polygons[1].IsLake; }
        }
    }
}