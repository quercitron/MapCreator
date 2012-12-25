using System;
using System.Collections.Generic;
using System.Linq;

using Triangulation.Algorithm.GeometryBase;
using Triangulation.Algorithm.PriorityQueue;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class AddRiversBuilderComponent : BaseRivers
    {
        private readonly double BigRiversAreaPart = 0.1;
        private readonly double SmallRiversAreaPart = 0.5;

        public override void Build(IMap map, MapSettings settings)
        {
            if (map.Corners.Any(c => c.IsLand))
            {
                var minStreamHeightForBigRivers = GetMinStreamHeight(map, this.BigRiversAreaPart);
                var minStreamHeightForRandomRivers = GetMinStreamHeight(map, this.SmallRiversAreaPart);

                List<Corner> riverSources = null;

                for (int k = 0; k < 50; k++)
                {
                    List<Corner> newSources = new List<Corner>();

                    for (int i = 0; i < 20; i++)
                    {
                        newSources.Add(this.GetRandomLandCornerAboveTheHeight(map, minStreamHeightForBigRivers));
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        newSources.Add(this.GetRandomLandCornerAboveTheHeight(map, minStreamHeightForRandomRivers));
                    }

                    if (riverSources == null || this.EstimateCorners(map, newSources) < this.EstimateCorners(map, riverSources))
                    {
                        riverSources = newSources;
                    }
                }

                if (riverSources != null)
                {
                    foreach (var source in riverSources)
                    {
                        this.AddRiver(source);
                    }
                }
            }
        }

        private void AddRiver(Corner riverSource)
        {
            var visited = new HashSet<Corner> { riverSource };

            riverSource.IsRiver = true;
            Point2D previous = null;

            double flow = 0;
            while (!riverSource.IsOcean)
            {
                Border nextEdge = null;
                Corner nextCorner = null;
                foreach (var border in riverSource.Borders)
                {
                    var newCorner = border.OtherEnd(riverSource);
                    if (!visited.Contains(newCorner))
                    {
                        if (nextCorner == null || newCorner.Elevation > nextCorner.Elevation)
                        {
                            nextEdge = border;
                            nextCorner = newCorner;
                        }
                    }
                }

                if (nextEdge == null)
                {
                    return;
                }

                if (nextCorner.Elevation > riverSource.Elevation)
                {
                    nextEdge = null;
                    nextCorner = null;

                    Point2D direction;
                    if (previous != null)
                    {
                        direction = riverSource - previous;
                    }
                    else
                    {
                        direction = Geometry.GetRandomUnitVector();
                    }

                    foreach (var border in riverSource.Borders)
                    {
                        var newCorner = border.OtherEnd(riverSource);
                        if (!visited.Contains(newCorner))
                        {
                            if (nextCorner == null || Geometry.Cos(direction, nextCorner) > Geometry.Cos(direction, newCorner))
                            {
                                nextEdge = border;
                                nextCorner = newCorner;
                            }
                        }
                    }

                    if (nextCorner == null)
                    {
                        return;
                    }

                    nextCorner.Elevation = riverSource.Elevation;
                }

                flow += 1;
                nextEdge.RiverCapacity += flow;
                nextCorner.IsRiver = true;

                previous = riverSource;
                riverSource = nextCorner;
                visited.Add(riverSource);
            }
        }
    }
}
