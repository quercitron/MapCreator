using System.Collections.Generic;
using System.Linq;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class DefineOcean : IMapBuilderComponent
    {
        public void Build(IMap map, MapSettings settings)
        {
            foreach (var polygon in map.Polygons)
            {
                if (!polygon.IsInside && !polygon.IsOcean)
                {
                    polygon.IsOcean = true;

                    var queue = new Queue<Polygon>();
                    queue.Enqueue(polygon);

                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();
                        foreach (var neighbor in current.Polygons.Where(neighbor => !neighbor.IsLand && !neighbor.IsOcean))
                        {
                            neighbor.IsOcean = true;
                            queue.Enqueue(neighbor);
                        }
                    }

                    break;
                }
            }

            foreach (var corner in map.Corners)
            {
                corner.IsLand = corner.Polygons.Any(p => p.IsLand);
                corner.IsOcean = corner.Polygons.Any(p => p.IsOcean);
            }
        }
    }
}
