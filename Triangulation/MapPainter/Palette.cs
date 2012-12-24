using System.Drawing;
using Triangulation.MapObjects;

namespace Triangulation.MapPainter
{
    internal class Palette
    {
        public Color OceanColor
        {
            get
            {
                return Color.RoyalBlue;
            }
        }

        public Color LakeColor
        {
            get
            {
                return Color.CornflowerBlue;
            }
        }

        public Color RiverColor
        {
            get
            {
                return Color.CornflowerBlue;
            }
        }

        public Color GetPolygonColor(Polygon polygon)
        {
            Color result;
            switch (polygon.Type)
            {
                case TerrainType.Grassland:
                    result = Color.FromArgb(196, 212, 170);
                    break;
                case TerrainType.Bare:
                    result = Color.FromArgb(187, 187, 187);
                    break;
                case TerrainType.Lake:
                    result = Color.CornflowerBlue;
                    break;
                case TerrainType.Ocean:
                    result = Color.RoyalBlue;
                    break;
                case TerrainType.Scorched:
                    result = Color.FromArgb(153, 153, 153);
                    break;
                case TerrainType.Shrubland:
                    result = Color.FromArgb(196,204,187);
                    break;
                case TerrainType.Snow:
                    result = Color.White;// Color.FromArgb(248, 248, 248);
                    break;
                case TerrainType.SubtropicalDesert:
                    result = Color.FromArgb(233, 221, 199);
                    break;
                case TerrainType.Tundra:
                    result = Color.FromArgb(221, 221, 187);
                    break;
                case TerrainType.Taiga:
                    result = Color.FromArgb(204, 212, 187);
                    break;
                case TerrainType.TemperatureDesert:
                    result = Color.FromArgb(228, 232, 202);
                    break;
                case TerrainType.TemperateRainForest:
                    result = Color.FromArgb(164, 196, 168);
                    break;
                case TerrainType.TemperatureDeciduousForest:
                    result = Color.FromArgb(180, 201, 169);
                    break;
                case TerrainType.TropicalRainForest:
                    result = Color.FromArgb(156, 187, 169);
                    break;
                case TerrainType.TropicalSeasonalForest:
                    result = Color.FromArgb(169, 204, 164);
                    break;
                default:
                    result = Color.Red;
                    break;
            }

            return result;
        }
    }
}
