using Triangulation.MapObjects;

namespace Triangulation.MapBuilding.Polygons
{
    class MapTriangle
    {
        public MapTriangle(int id)
        {
            Id = id;
            Points = new MapPoint[3];
            Triangles = new MapTriangle[3];
        }

        public int Id { get; private set; }

        public MapPoint[] Points { get; set; }

        public MapTriangle[] Triangles { get; set; }

        public Corner Corner { get; set; }

        public MapEdge Edge(int i)
        {
            return new MapEdge(Points[(i + 1) % 3], Points[(i + 2) % 3]);
        }
    }
}
