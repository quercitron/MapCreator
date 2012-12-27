/*
using System;
using System.Collections.Generic;

using Triangulation.Algorithm.GeometryBase;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class ImprovedRiverStrategy : IAddRiverStrategy
    {
        private Corner[] m_River;

        private HashSet<Corner> m_Visited;

        public void AddRiver(Corner current, bool onlyErosion)
        {
            // TODO: remove magic const
            m_River = new Corner[10000];
            m_Visited = new HashSet<Corner>();
            AddNextRiverPart(current, onlyErosion, 0);
        }

        private bool AddNextRiverPart(Corner current, bool onlyErosion, int partNo)
        {
            m_Visited.Add(current);
            m_River[partNo] = current;

            if (!onlyErosion)
            {
                current.IsRiver = true;
            }

            Border nextEdge = null;
            Corner nextCorner = null;
            foreach (var border in current.Borders)
            {
                var newCorner = border.OtherEnd(current);
                if (!m_Visited.Contains(newCorner))
                {
                    if (current.IsLake && newCorner.IsLake && border.Polygons[0].IsLand && border.Polygons[1].IsLand)
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
                return false;
            }

            if (nextCorner.Elevation > current.Elevation)
            {
                nextEdge = null;
                nextCorner = null;

                Point2D direction;
                if (previous != null)
                {
                    direction = current - previous;
                }
                else
                {
                    direction = Geometry.GetRandomUnitVector();
                }

                foreach (var border in current.Borders)
                {
                    var newCorner = border.OtherEnd(current);
                    if (!visited.Contains(newCorner))
                    {
                        if (current.IsLake && newCorner.IsLake && border.Polygons[0].IsLand && border.Polygons[1].IsLand)
                        {
                            continue;
                        }
                        if (nextCorner == null ||
                            Geometry.Cos(direction, nextCorner - current) < Geometry.Cos(direction, newCorner - current))
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

                nextCorner.Elevation = current.Elevation - SmallStep;
            }
        }
    }
}
*/
