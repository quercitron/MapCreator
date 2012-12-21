using System.Collections.Generic;
using System.Linq;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class DefineWaterTypesBuilderComponent : IMapBuilderComponent
    {
        public void Build(IMap map)
        {
            foreach (var polygon in map.Polygons)
            {
                if (!polygon.IsInside)
                {
                    polygon.IsOcean = true;

                    var queue = new Queue<Polygon>();
                    queue.Enqueue(polygon);

                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();
                        foreach (
                            var neighbor in current.Polygons.Where(neighbor => !neighbor.IsLand && !neighbor.IsOcean))
                        {
                            neighbor.IsOcean = true;
                            queue.Enqueue(neighbor);
                        }
                    }

                    break;
                }
            }
        }
    }
}
