namespace Triangulation.MapBuilding.Polygons
{
    class MapEdge
    {
        public MapEdge(MapPoint first, MapPoint second)
        {
            First = first;
            Second = second;
        }

        public MapPoint First { get; set; }

        public MapPoint Second { get; set; }
    }
}
