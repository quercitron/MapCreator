using System.Linq;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class SetTerranTypeFromSite : IMapBuilderComponent
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
                    moistureLevels[i] = (double)i / N; //moistures[(i * moistures.Length) / N];
                }
                for (int i = 0; i < M; i++)
                {
                    elevationLevels[i] = (double)i / M; //elevation[(i * elevation.Length) / M];
                }

                foreach (var polygon in map.Polygons)
                {
                    TerrainType terranType = TerrainType.Grassland;

                    if (polygon.IsLand)
                    {
                        int ml = 0;
                        for (int i = 0; i < N; i++)
                        {
                            if (polygon.Moisture > moistureLevels[i])
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
                    }
                    else
                    {
                        if (polygon.IsLake)
                        {
                            terranType = TerrainType.Lake;
                        }
                        if (polygon.IsOcean)
                        {
                            terranType = TerrainType.Ocean;
                        }
                    }

                    polygon.Type = terranType;
                }
            }
        }
    }
}
