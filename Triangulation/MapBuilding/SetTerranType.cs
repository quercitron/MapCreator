using System.Linq;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class SetTerranType : IMapBuilderComponent
    {
        public void Build(IMap map, MapSettings settings)
        {
            var moistures = map.Polygons.Where(p => p.IsLand).Select(p => p.DistanceForMoisture).OrderBy(p => p).ToArray();
            var elevation = map.Polygons.Where(p => p.IsLand).Select(p => p.Elevation).OrderBy(p => p).ToArray();

            int N = 6;
            int M = 4;
            double[] moistureLevels = new double[N];
            double[] elevationLevels = new double[M];

            if (moistures.Length > 0 && elevation.Length > 0)
            {
                for (int i = 0; i < N; i++)
                {
                    moistureLevels[i] = moistures[(i*moistures.Length)/N];
                }
                for (int i = 0; i < M; i++)
                {
                    elevationLevels[i] = elevation[(i*elevation.Length)/M];
                }

                foreach (var polygon in map.Polygons.Where(p => p.IsLand))
                {
                    int ml = 0;
                    for (int i = 0; i < N; i++)
                    {
                        if (polygon.DistanceForMoisture > moistureLevels[i])
                        {
                            ml = i;
                        }
                    }

                    int el = 0;
                    for (int i = 0; i < M; i++)
                    {
                        if (polygon.Elevation > elevationLevels[i])
                        {
                            el = i;
                        }
                    }

                    TerrainType terranType = TerrainType.Grassland;

                    // TODO: change
                    ml = N - ml - 1;

                    if (el == 0)
                    {
                        switch (ml)
                        {
                            case 0:
                                terranType = TerrainType.SubtropicalDesert;
                                break;
                            case 1:
                                terranType = TerrainType.Grassland;
                                break;
                            case 2:
                            case 3:
                                terranType = TerrainType.TropicalSeasonalForest;
                                break;
                            case 4:
                            case 5:
                                terranType = TerrainType.TropicalRainForest;
                                break;
                        }
                    }

                    if (el == 1)
                    {
                        switch (ml)
                        {
                            case 0:
                                terranType = TerrainType.TemperatureDesert;
                                break;
                            case 1:
                            case 2:
                                terranType = TerrainType.Grassland;
                                break;
                            case 3:
                            case 4:
                                terranType = TerrainType.TemperatureDeciduousForest;
                                break;
                            case 5:
                                terranType = TerrainType.TemperateRainForest;
                                break;
                        }
                    }

                    if (el == 2)
                    {
                        switch (ml)
                        {
                            case 0:
                            case 1:
                                terranType = TerrainType.TemperatureDesert;
                                break;
                            case 2:
                            case 3:
                                terranType = TerrainType.Shrubland;
                                break;
                            case 4:
                            case 5:
                                terranType = TerrainType.Taiga;
                                break;
                        }
                    }

                    if (el == 3)
                    {
                        switch (ml)
                        {
                            case 0:
                                terranType = TerrainType.Scorched;
                                break;
                            case 1:
                                terranType = TerrainType.Bare;
                                break;
                            case 2:
                                terranType = TerrainType.Tundra;
                                break;
                            case 3:
                            case 4:
                            case 5:
                                terranType = TerrainType.Snow;
                                break;
                        }
                    }

                    polygon.Type = terranType;
                }
            }
        }
    }
}
