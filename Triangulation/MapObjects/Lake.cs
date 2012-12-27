using System.Collections.Generic;
using System.Linq;

namespace Triangulation.MapObjects
{
    public class Lake
    {
        public Lake()
        {
            m_Corners = new HashSet<Corner>();
            m_Polygons = new HashSet<Polygon>();
        }

        private HashSet<Corner> m_Corners;

        public void Form(Corner corner)
        {
            Add(corner);
            foreach (var next in corner.Corners.Where(c => c!= null && !this.m_Corners.Contains(c)))
            {
                this.Form(next);
            }
        }

        public void Add(Corner corner)
        {
            m_Corners.Add(corner);
            corner.Lake = this;

            foreach (var polygon in corner.Polygons.Where(p => p.IsLake))
            {
                m_Polygons.Add(polygon);
            }
        }

        public List<Corner> Corners
        {
            get
            {
                return m_Corners.ToList();
            }
        }

        public bool Contains(Corner corner)
        {
            return m_Corners.Contains(corner);
        }

        private HashSet<Polygon> m_Polygons;

        public void Add(Polygon polygon)
        {
            m_Polygons.Add(polygon);

            foreach (var corner in polygon.Corners.Where(c => c.IsLake))
            {
                m_Corners.Add(corner);
                corner.Lake = this;
            }
        }

        public List<Polygon> Polygons
        {
            get
            {
                return m_Polygons.ToList();
            }
        }

        public bool Contains(Polygon polygon)
        {
            return m_Polygons.Contains(polygon);
        }
    }
}
