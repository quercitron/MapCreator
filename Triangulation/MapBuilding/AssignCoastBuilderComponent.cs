using System.Linq;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class AssignCoastBuilderComponent : IMapBuilderComponent
    {
        public void Build(IMap map, MapSettings settings)
        {
            foreach (var border in map.Borders)
            {
                if (border.Polygons[0].IsLand != border.Polygons[1].IsLand)
                {
                    if (border.Polygons[0].IsOcean || border.Polygons[1].IsOcean)
                    {
                        border.IsOceanCoast = true;
                        for (int i = 0; i < 2; i++)
                        {
                            border.Corners[i].IsOceanCoast = true;
                            border.Polygons[i].IsOceanCoast = true;
                        }
                    }
                    else
                    {
                        border.IsLakeCoast = true;
                        for (int i = 0; i < 2; i++)
                        {
                            border.Corners[i].IsLakeCoast = true;
                            border.Polygons[i].IsLakeCoast = true;
                        }
                    }
                }
            }

            foreach (var corner in map.Corners)
            {
                corner.IsLand = corner.Polygons.Any(p => p.IsLand);
                corner.IsWater = corner.Polygons.Any(p => !p.IsLand);
                corner.IsOcean = corner.Polygons.Any(p => p.IsOcean);
            }

            foreach (var corner in map.Corners)
            {
                if (corner.IsWater && !corner.IsOcean)
                {
                    corner.IsLake = true;
                }
            }
        }
    }
}
