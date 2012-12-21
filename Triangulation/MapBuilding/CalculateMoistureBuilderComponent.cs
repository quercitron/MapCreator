using System.Linq;

using Triangulation.Algorithm.GeometryBase;
using Triangulation.Algorithm.PriorityQueue;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class CalculateMoistureBuilderComponent : IMapBuilderComponent
    {
        public void Build(IMap map)
        {
            var queue = new PriorityQueue<Corner>((a, b) => -a.DistanceForMoisture.CompareTo(b.DistanceForMoisture));

            foreach (var corner in map.Corners)
            {
                if (corner.IsLake || corner.IsRiver)
                {
                    corner.DistanceForMoisture = 0;
                    queue.Enqueue(corner);
                }
                else
                {
                    if (corner.IsOcean)
                    {
                        corner.DistanceForMoisture = 1e9;
                    }
                    else
                    {
                        corner.DistanceForMoisture = 1e9;
                    }
                }
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var corner in current.Corners)
                {
                    // TODO: Chack how can it be
                    if (corner != null)
                    {
                        var newDist = current.DistanceForMoisture + Geometry.Dist(current, corner);
                        if (corner.DistanceForMoisture > newDist)
                        {
                            corner.DistanceForMoisture = newDist;
                            queue.Enqueue(corner);
                        }
                    }
                }
            }

            foreach (var polygon in map.Polygons)
            {
                if (polygon.IsWater)
                {
                    polygon.DistanceForMoisture = 0;
                }
                else
                {
                    polygon.DistanceForMoisture = polygon.Corners.Average(c => c.DistanceForMoisture);
                }
            }
        }
    }
}
