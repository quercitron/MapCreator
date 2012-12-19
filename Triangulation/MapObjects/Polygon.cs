using System.Collections.Generic;
using System.Linq;

namespace Triangulation.MapObjects
{
    public class Polygon
    {
        public Polygon(Point2D basePoint)
        {
            BasePoint = basePoint;
            Init();
        }

        private void Init()
        {
            Polygons = new List<Polygon>();
            Corners = new List<Corner>();
            Borders = new List<Border>();
            CornersToDraw = new List<Point2D>();
        }

        public List<Polygon> Polygons { get; set; }
        public List<Corner> Corners { get; set; }
        public List<Border> Borders { get; set; }

        public List<Point2D> CornersToDraw { get; set; }

        public Point2D BasePoint { get; private set; }

        public bool IsLand { get; set; }

        public bool IsInside { get; set; }

        public Point2D Center
        {
            get
            {
                return new Point2D(Corners.Average(p => p.X), Corners.Average(p => p.Y));
            }
        }

        public bool IsOceanCoast { get; set; }
        public bool IsLakeCoast { get; set; }

        public bool IsOcean { get; set; }
        public bool IsLake
        {
            get { return !(IsLand || IsOcean); }
        }

        public int DistanceFromEdge { get; set; }

        public bool InSkeleton { get; set; }
        public int DistanceFromSkeleton { get; set; }

        public double Elevation { get; set; }

        public double DistanceForMoisture { get; set; }

        public bool IsWater
        {
            get { return IsOcean || IsLake; }          
        }
    }
}
