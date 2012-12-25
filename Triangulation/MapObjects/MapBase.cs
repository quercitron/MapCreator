using System;
using System.Collections.Generic;
using System.Linq;

using Triangulation.Algorithm.GeometryBase;

namespace Triangulation.MapObjects
{
    abstract internal class MapBase : IMap
    {
        private static readonly Random m_Rnd = new Random();

        protected MapBase(double width, double height)
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

        public double GetMaxCornerElevation
        {
            get { return Corners.Max(p => p.Elevation); }
        }

        public int MaxDistanceFromEdge { get; set; }

        public bool ContainsPointInside(Point2D point)
        {
            return 0 <= point.X && point.X <= Width && 0 <= point.Y && point.Y <= Height;
        }

        public Polygon GetRandomPolygon()
        {
            return Polygons[m_Rnd.Next(Polygons.Count)];
        }

        public Corner GetRandomCorner()
        {
            return Corners[m_Rnd.Next(Corners.Count)];
        }
    }
}