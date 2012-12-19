using System.Linq;

namespace Triangulation.MapObjects
{
    public class Map : MapBase
    {
        public Map(double width, double height)
            : base(width, height)
        {
        }

        public int MaxDistanceFromEdge { get; set; }

        public int AvarageDistanceFromEdge
        {
            get { return Polygons.Sum(p => p.DistanceFromEdge) / Polygons.Count; }
        }
    }
}