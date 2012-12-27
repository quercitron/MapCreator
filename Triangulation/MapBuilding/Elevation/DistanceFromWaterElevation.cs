using System;
using System.Collections.Generic;
using System.Linq;

using PerlinNoiseGeneration;

using Triangulation.Algorithm.GeometryBase;
using Triangulation.Algorithm.PriorityQueue;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class DistanceFromWaterElevation : BaseElevationGenerator
    {
        public DistanceFromWaterElevation(INoiseGenerator noiseGenerator)
        {
            this.m_NoiseGenerator = noiseGenerator;
        }

        private readonly INoiseGenerator m_NoiseGenerator;

        // TODO: Change random
        private Random m_Random = new Random();

        public override void Build(IMap map, MapSettings settings)
        {
            Comparison<Corner> comparison = (a, b) => -a.Elevation.CompareTo(b.Elevation);
            var queue = new PriorityQueue<Corner>(comparison);

            foreach (var corner in map.Corners)
            {
                if (!corner.IsOcean || corner.IsOceanCoast)
                {
                    if (corner.IsOceanCoast)
                    {
                        corner.Elevation = 0;
                        queue.Enqueue(corner);
                    }
                    else
                    {
                        corner.Elevation = 1e10;
                    }
                }
                else
                {
                    corner.Elevation = 0;
                }
            }

            /*while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var corner in current.Corners)
                {
                    // TODO: check why can corner be null
                    if (corner != null && corner.IsLand && corner.Elevation < 0)
                    {
                        corner.Elevation = current.Elevation + Geometry.Dist(current, corner) ;
                        queue.Enqueue(corner);
                    }
                }
            }*/

            var noise = new PolarizeNoiseDecorator(new NormNoiseDecorator(m_NoiseGenerator)).GenerateNoise((int)map.Width, (int)map.Height, m_Random.Next(), 4);

            /*foreach (var corner in map.Corners)
            {
                if (corner.IsLand)
                {
                    corner.Elevation = (corner.Elevation + 1) * noise[(int) corner.X, (int) corner.Y];
                }
            }

            NormalizeElevation(map);*/

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var corner in current.Corners.Where(map.ContainsPointInside))
                {
                    var midPoint = (current + corner) / 2;
                    var newDist = current.Elevation + Geometry.Dist(current, corner) * noise[(int)midPoint.X, (int)midPoint.Y];
                    if (corner.Elevation > newDist)
                    {
                        corner.Elevation = newDist;
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
                                    if (c.IsLake && c.Elevation > lakeCorner.Elevation + bigStep)
                                    {
                                        c.Elevation = lakeCorner.Elevation + this.SmallStep;
                                        lakeQueue.Enqueue(c);
                                        queue.Enqueue(c);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            this.NormalizeCornerElevation(map);

            //TODO: obsolete?
            /*foreach (var polygon in map.Polygons)
            {
                polygon.Elevation = polygon.Corners.Average(p => p.Elevation);
                //polygon.Elevation = polygon.IsLand ? polygon.Corners.Average(p => p.Elevation) : 0;
            } */
        }
    }
}
