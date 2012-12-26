using System;
using System.Linq;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal abstract class BaseElevationGenerator : IMapBuilderComponent
    {
        protected readonly double SmallStep = 1e-7;

        // TODO : it is not common normalize
        protected void NormalizeCornerElevation(IMap map)
        {
            // TODO: check why max and min elivations are so low
            var maxElevation = map.GetMaxCornerElevation;
            if (maxElevation > 0)
            {
                foreach (var corner in map.Corners.Where(c => c.Elevation >= 0))
                {
                    corner.Elevation /= maxElevation;
                    corner.Elevation *= corner.Elevation;
                }
            }

            var minElevation = -map.Corners.Min(c => c.Elevation);
            if (minElevation > 0)
            {
                foreach (var corner in map.Corners.Where(c => c.Elevation < 0))
                {
                    corner.Elevation /= minElevation;
                    corner.Elevation *= -corner.Elevation;
                }
            }

            // TODO: Remove
            if (map.Corners.Any(c => Math.Abs(c.Elevation) > 1))
            {
                throw new ApplicationException("TTT");
            }
        }

        public abstract void Build(IMap map, MapSettings settings);
    }
}