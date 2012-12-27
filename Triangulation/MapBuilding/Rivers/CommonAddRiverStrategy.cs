using System.Collections.Generic;

using Triangulation.Algorithm.GeometryBase;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class CommonAddRiverStrategy : IAddRiverStrategy
    {
        private readonly double SmallStep = 1e-8;

        public void AddRiver(Corner riverSource, bool onlyErosion)
        {
            var visited = new HashSet<Corner> { riverSource };

            if (!onlyErosion)
            {
                riverSource.IsRiver = true;
            }
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
                        if (riverSource.IsLake && newCorner.IsLake && border.Polygons[0].IsLand && border.Polygons[1].IsLand)
                        {
                            continue;
                        }

                        if (nextCorner == null || newCorner.Elevation < nextCorner.Elevation)
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
                            if (riverSource.IsLake && newCorner.IsLake && border.Polygons[0].IsLand && border.Polygons[1].IsLand)
                            {
                                continue;
                            }
                            if (nextCorner == null ||
                                Geometry.Cos(direction, nextCorner - riverSource) < Geometry.Cos(direction, newCorner - riverSource))
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

                    nextCorner.Elevation = riverSource.Elevation - SmallStep;
                }

                if (!onlyErosion)
                {
                    // TODO: fix eps
                    if (nextEdge.RiverCapacity < 1e-7)
                    {
                        flow += 1;
                    }
                    nextEdge.RiverCapacity += flow;
                    nextCorner.IsRiver = true;
                }

                previous = riverSource;
                riverSource = nextCorner;
                visited.Add(riverSource);
            }
        }
    }
}
