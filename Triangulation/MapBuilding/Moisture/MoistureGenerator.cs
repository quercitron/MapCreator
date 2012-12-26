using System;
using System.Collections.Generic;
using System.Linq;

using Triangulation.Algorithm.GeometryBase;
using Triangulation.Algorithm.PriorityQueue;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class MoistureGenerator : IMapBuilderComponent
    {
        private readonly double m_MaxDist = 1e9;

        public void Build(IMap map, MapSettings settings)
        {
            var queue = new PriorityQueue<Corner>((a, b) => -a.DistanceForMoisture.CompareTo(b.DistanceForMoisture));

            foreach (var corner in map.Corners)
            {
                if (corner.IsLake || corner.IsRiver/* || corner.IsOcean*/)
                {
                    queue.Enqueue(corner);
                }
                corner.DistanceForMoisture = corner.IsWater ? 0 : this.m_MaxDist;
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var corner in current.Corners)
                {
                    // TODO: Chack how can it be
                    if (corner != null)
                    {
                        // TODO: improve mechanic
                        double k = corner.Elevation > current.Elevation ? 0.4 : 1;
                        var newDist = current.DistanceForMoisture + Geometry.Dist(current, corner) * k;
                        if (corner.DistanceForMoisture > newDist)
                        {
                            corner.DistanceForMoisture = newDist;
                            queue.Enqueue(corner);
                        }
                    }
                }
            }

            SetAndNormalizeMoisture(map);

            foreach (var polygon in map.Polygons)
            {
                if (polygon.IsWater)
                {
                    polygon.Moisture = 0;
                }
                else
                {
                    polygon.Moisture = polygon.Corners.Average(c => c.Moisture);
                }
            }
        }

        private void SetAndNormalizeMoisture(IMap map)
        {
            var landCorners = map.Corners.Where(c => c.IsLand && c.DistanceForMoisture < m_MaxDist).ToList();
            if (landCorners.Count > 0)
            {
                var avgDistance = landCorners.Average(c => c.DistanceForMoisture);

                foreach (var corner in map.Corners)
                {
                    if (corner.IsLand)
                    {
                        if (corner.DistanceForMoisture < m_MaxDist)
                        {
                            corner.Moisture = Math.Max(0, 1 - 0.3 * corner.DistanceForMoisture / avgDistance);
                        }
                        else
                        {
                            corner.Moisture = 0;
                        }
                    }
                    else
                    {
                        corner.Moisture = 1;
                    }
                }
            }
        }
    }
}
