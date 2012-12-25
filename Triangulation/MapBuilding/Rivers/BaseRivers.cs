using System.Collections.Generic;
using System.Linq;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal abstract class BaseRivers : IMapBuilderComponent
    {
        protected double EstimateCorners(IMap map, IList<Corner> corners)
        {
            double result = 0;
            /*for (int i = 0; i < corners.Count; i++)
            {
                double minDist = 1e9;
                for (int j = 0; j < i; j++)
                {
                    minDist = Math.Min(minDist, Geometry.Dist(corners[i], corners[j]));
                }
                result += minDist * minDist;
            }*/

            int N = 10, M = 10;
            int[,] areas = new int[N,M];
            int[,] e = new int[N,M];

            foreach (var corner in map.Corners.Where(c => c.IsLand))
            {
                int x = (int)((N * corner.X) / map.Width);
                int y = (int)((M * corner.Y) / map.Height);
                e[x, y]++;
            }

            foreach (var corner in corners)
            {
                int x = (int)((N * corner.X) / map.Width);
                int y = (int)((M * corner.Y) / map.Height);
                areas[x, y]++;
            }

            int cornerCount = corners.Count;
            int landCount = map.Corners.Where(c => c.IsLand).Count();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    double d = (double)cornerCount * e[i, j] / landCount - areas[i, j];
                    result += d * d;
                }
            }

            return result;
        }

        protected Corner GetRandomLandCornerAboveTheHeight(IMap map, double minStreamHeight)
        {
            Corner current;
            do
            {
                current = map.GetRandomCorner();
            }
            while (!(current.IsLand && current.Elevation > minStreamHeight));
            return current;
        }

        protected static double GetMinStreamHeight(IMap map, double areaPart)
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

        public abstract void Build(IMap map, MapSettings settings);
    }
}