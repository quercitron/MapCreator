using System.Collections.Generic;

using Triangulation.Algorithm.PriorityQueue;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal abstract class BaseElevationGenerator : IMapBuilderComponent
    {
        protected readonly double SmallStep = 1e-6;

        protected void NormalizeCornerElevation(IMap map)
        {
            var maxElevation = map.GetMaxCornerElevation;
            if (maxElevation > 0)
            {
                foreach (var corner in map.Corners)
                {
                    corner.Elevation /= maxElevation;
                    corner.Elevation *= corner.Elevation;
                }
            }
        }

        public abstract void Build(IMap map, MapSettings settings);
    }
}