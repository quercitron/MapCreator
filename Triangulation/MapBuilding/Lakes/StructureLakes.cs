using System.Linq;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding.Lakes
{
    internal class StructureLakes : IMapBuilderComponent
    {
        public void Build(IMap map, MapSettings settings)
        {
            foreach (var corner in map.Corners.Where(c => c.IsLake))
            {
                if (corner.Lake == null)
                {
                    var lake = new Lake();                    
                    lake.Form(corner);
                }
            }
        }
    }
}
