using System;
using System.Collections.Generic;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class CalculateDistanceFromEdgeBuilderComponent : IMapBuilderComponent
    {
        public void Build(IMap map)
        {
            var queue = new Queue<Polygon>();
            foreach (var polygon in map.Polygons)
            {
                if (polygon.IsInside)
                {
                    polygon.DistanceFromEdge = -1;
                }
                else
                {
                    polygon.DistanceFromEdge = 0;
                    queue.Enqueue(polygon);
                }
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var polygon in current.Polygons)
                {
                    if (polygon.DistanceFromEdge < 0)
                    {
                        polygon.DistanceFromEdge = current.DistanceFromEdge + 1;
                        map.MaxDistanceFromEdge = Math.Max(map.MaxDistanceFromEdge, current.DistanceFromEdge + 1);
                        queue.Enqueue(polygon);
                    }
                }
            }
        }
    }
}
