using System;
using System.Collections.Generic;
using System.Linq;

using PerlinNoiseGeneration;

using Triangulation.Algorithm.GeometryBase;
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
                if (corner.IsOceanCoast)
                {
                    corner.DistanceFromCoast = 0;
                    queue.Enqueue(corner);
                }
                else
                {
                    corner.DistanceFromCoast = 1e9;
                }
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var corner in current.Corners.Where(c => c != null))
                {
                    var dist = current.Dist(corner);
                    if (corner.DistanceFromCoast > current.DistanceFromCoast + dist)
                    {
                        corner.DistanceFromCoast = current.DistanceFromCoast + dist;

                        queue.Enqueue(corner);

                        if (corner.IsLake)
                        {
                            double minNoise = noise[(int)corner.X, (int)corner.Y];
                            var lakeCorners = new List<Corner> { corner };

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
                                        c.DistanceFromCoast = lakeCorner.DistanceFromCoast +
                                                              this.SmallStep * Geometry.Dist(lakeCorner, c) / map.Diagonal;
                                        lakeQueue.Enqueue(c);
                                        queue.Enqueue(c);

                                        minNoise = Math.Min(minNoise, noise[(int)c.X, (int)c.Y]);
                                        lakeCorners.Add(c);
                                    }
                                }
                            }

                            foreach (var lakeCorner in lakeCorners)
                            {
                                noise[(int)lakeCorner.X, (int)lakeCorner.Y] = minNoise;
                            }
                        }
                    }
                }
            }

            var maxDistance = map.Corners.Where(c => c.IsLand).Max(c => c.DistanceFromCoast);
            foreach (var corner in map.Corners)
            {
                double noiseInCorner;
                if (map.ContainsPointInside(corner))
                {
                    noiseInCorner = noise[(int)corner.X, (int)corner.Y];
                }
                else
                {
                    var point = new Point2D(corner);
                    if (point.X < 0)
                    {
                        point.X = 0;
                    }
                    if (point.X >= map.Width - 1)
                    {
                        point.X = map.Width - 1;
                    }
                    if (point.Y < 0)
                    {
                        point.Y = 0;
                    }
                    if (point.Y >= map.Height - 1)
                    {
                        point.Y = map.Height - 1;
                    }
                    noiseInCorner = noise[(int)point.X, (int)point.Y];
                }

                if (corner.IsOcean)
                {
                    corner.Elevation = -Math.Min(corner.DistanceFromCoast / maxDistance, noiseInCorner);
                }
                else
                {
                    corner.Elevation = Math.Min(corner.DistanceFromCoast / maxDistance, noiseInCorner);
                }
            }

            this.NormalizeCornerElevation(map);
        }
    }
}
