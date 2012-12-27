using System.Collections.Generic;
using System.Linq;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class AddRiversBuilderComponent : BaseRivers
    {
        public AddRiversBuilderComponent(IAddRiverStrategy addRiverStrategy)
        {
            this.m_AddRiverStrategy = addRiverStrategy;
        }

        private IAddRiverStrategy m_AddRiverStrategy;

        private readonly double BigRiversAreaPart = 0.1;
        private readonly double SmallRiversAreaPart = 0.5;

        public override void Build(IMap map, MapSettings settings)
        {
            if (map.Corners.Any(c => c.IsLand))
            {
                var minStreamHeightForBigRivers = GetMinStreamHeight(map, this.BigRiversAreaPart);
                var minStreamHeightForRandomRivers = GetMinStreamHeight(map, this.SmallRiversAreaPart);

                List<Corner> riverSources = null;

                for (int k = 0; k < 5000; k++)
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

                    // TODO: maybe remove this strange thing
                    foreach (var source in riverSources)
                    {
                        m_AddRiverStrategy.AddRiver(source, true);
                    }
                }

                if (riverSources != null)
                {
                    // TODO: set parameter?
                    for (int k = 0; k < 20; k++)
                    {
                        foreach (var source in riverSources)
                        {
                            m_AddRiverStrategy.AddRiver(source, true);
                        }
                    }

                    foreach (var source in riverSources)
                    {
                        m_AddRiverStrategy.AddRiver(source, false);
                    }
                }
            }
        }

        private void ResetRivers(IMap map)
        {
            foreach (var corner in map.Corners)
            {
                corner.IsRiver = false;
            }

            foreach (var border in map.Borders)
            {
                border.RiverCapacity = 0;
            }
        }
    }
}
