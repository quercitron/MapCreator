namespace Triangulation.Dividing
{
    public class Edge
    {
        public Edge(StructurePoint first, StructurePoint second)
        {
            First = first;
            Second = second;
        }

        public StructurePoint First { get; set; }
        public StructurePoint Second { get; set; }
    }
}
