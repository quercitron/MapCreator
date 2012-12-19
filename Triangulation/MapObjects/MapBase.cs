using System;
using System.Collections.Generic;
using System.Linq;

namespace Triangulation.MapObjects
{
    public class MapBase
    {
        private static readonly Random Rnd = new Random();

        public MapBase(double width, double height)
        {
            Width = width;
            Height = height;

            Polygons = new List<Polygon>();
            Corners = new List<Corner>();
            Borders = new List<Border>();
        }

        public List<Polygon> Polygons { get; private set; }
        public List<Corner> Corners { get; private set; }
        public List<Border> Borders { get; private set; }

        public double Width { get; private set; }
        public double Height { get; private set; }

        public double GetMaxElevation
        {
            get { return Corners.Max(p => p.Elevation); }
        }

        public bool ContainsPointInside(Point2D point)
        {
            return 0 <= point.X && point.X <= Width && 0 <= point.Y && point.Y <= Height;
        }

        public Polygon GetRandomPolygon()
        {
            return Polygons[Rnd.Next(Polygons.Count)];
        }

        public Corner GetRandomCorner()
        {
            return Corners[Rnd.Next(Corners.Count)];
        }
    }
}