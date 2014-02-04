using System.Collections.Generic;
using System.Linq;

using GeneralAlgorithms.GeometryBase;

namespace Triangulation.MapObjects
{
    public class Corner : Point2D
    {
        public Corner(double x, double y)
            : base(x, y)
        {
            Init();
        }

        public Corner(Point2D point)
            : base(point.X, point.Y)
        {
            Init();
        }

        public Polygon[] Polygons { get; private set; }
        public List<Border> Borders { get; private set; }
        public Corner[] Corners { get; private set; }

        private void Init()
        {
            Polygons = new Polygon[3];
            Borders = new List<Border>(3);
            Corners = new Corner[3];
        }

        public bool IsOceanCoast { get; set; }
        public bool IsLakeCoast { get; set; }

        public bool IsLand { get; set; }

        // TODO: think about property
        public bool IsWater
        {
            get
            {
                if (IsLake || IsOcean || IsRiver)
                {
                    return true;
                }

                return Polygons.Any(p => !p.IsLand);
            }
        }

        public double Elevation { get; set; }

        public double DistanceForMoisture { get; set; }

        public bool IsOcean { get; set; }
        public bool IsLake { get; set; }
        public bool IsRiver { get; set; }

        public double Moisture { get; set; }

        // TODO: think about name
        public double DistanceFromCoast { get; set; }

        public Lake Lake { get; set; }
    }
}