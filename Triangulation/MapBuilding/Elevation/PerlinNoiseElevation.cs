using System;
using System.Collections.Generic;
using System.Linq;

using PerlinNoiseGeneration;

using Triangulation.Algorithm.PriorityQueue;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class PerlinNoiseElevation : BaseElevationGenerator
    {
        public PerlinNoiseElevation(INoiseGenerator noiseGenerator)
        {
            this.m_NoiseGenerator = noiseGenerator;
        }

        private INoiseGenerator m_NoiseGenerator;

        public override void Build(IMap map, MapSettings settings)
        {
            // TODO: change parameters
            var noise = m_NoiseGenerator.GenerateNoise((int)map.Width, (int)map.Height, new Random().Next(), 10);

            PriorityQueue<Corner> queue = new PriorityQueue<Corner>((a, b) => -a.DistanceFromCoast.CompareTo(b.DistanceFromCoast));

            foreach (var corner in map.Corners)
            {
                if (corner.IsOcean)
                {
                    corner.DistanceFromCoast = 0;
                    if (corner.IsOceanCoast)
                    {
                        queue.Enqueue(corner);
                    }
                }
                else
                {
                    corner.DistanceFromCoast = 1e9;
                }
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var corner in current.Corners)
                {
                    var dist = current.Dist(corner);
                    if (corner.DistanceFromCoast > current.DistanceFromCoast + dist)
                    {
                        corner.DistanceFromCoast = current.DistanceFromCoast + dist;

                        queue.Enqueue(corner);

                        if (corner.IsLake)
                        {
                            double bigStep = 2 * map.Corners.Count * this.SmallStep;

                            var lakeQueue = new Queue<Corner>();
                            lakeQueue.Enqueue(corner);
                            while (lakeQueue.Count > 0)
                            {
                                var lakeCorner = lakeQueue.Dequeue();
                                foreach (var c in lakeCorner.Corners)
                                {
                                    if (c.IsLake && c.DistanceFromCoast > lakeCorner.DistanceFromCoast + bigStep)
                                    {
                                        c.DistanceFromCoast = lakeCorner.DistanceFromCoast + this.SmallStep;
                                        lakeQueue.Enqueue(c);
                                        queue.Enqueue(c);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var maxDistance = map.Corners.Max(c => c.DistanceFromCoast);
            foreach (var corner in map.Corners)
            {
                if (corner.IsOcean)
                {
                    corner.Elevation = 0;
                }
                else
                {
                    corner.Elevation = Math.Min(corner.DistanceFromCoast / maxDistance, noise[(int)corner.X, (int)corner.Y]);
                }
            }


            this.NormalizeCornerElevation(map);
        }
    }
}
