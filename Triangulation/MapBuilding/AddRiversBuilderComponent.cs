using System.Linq;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class AddRiversBuilderComponent : IMapBuilderComponent
    {
        private readonly double AreaPart = 0.1;

        public void Build(IMap map, MapSettings settings)
        {
            var minStreamHeight = GetMinStreamHeight(map, AreaPart);

            for (int i = 0; i < 20; i++)
            {
                AddRiver(map, minStreamHeight);
            }
        }

        private void AddRiver(IMap map, double minStreamHeight)
        {
            if (map.Corners.Any(c => c.IsLand && c.Elevation > minStreamHeight))
            {
                Corner current;
                do
                {
                    current = map.GetRandomCorner();
                } while (!(current.IsLand && current.Elevation > minStreamHeight));

                current.IsRiver = true;

                double flow = 0;
                while (!current.IsOcean)
                {
                    Border minEdge = null;
                    Corner minNext = current;
                    foreach (var border in current.Borders)
                    {
                        var newCorner = border.OtherEnd(current);
                        if (newCorner.Elevation < minNext.Elevation)
                        {
                            minEdge = border;
                            minNext = newCorner;
                        }
                    }

                    if (minEdge == null)
                    {
                        break;
                    }

                    flow += 1;
                    minEdge.RiverCapacity += flow;
                    minNext.IsRiver = true;

                    current = minNext;
                }
            }
        }

        private static double GetMinStreamHeight(IMap map, double areaPart)
        {
            double l = 0;
            double r = 1;

            while (r - l > 1e-7)
            {
                double m = (l + r) / 2;

                if (areaPart * map.Corners.Count > map.Corners.Count(c => c.IsLand && c.Elevation >= m))
                {
                    r = m;
                }
                else
                {
                    l = m;
                }
            }

            return l;
        }
    }
}
