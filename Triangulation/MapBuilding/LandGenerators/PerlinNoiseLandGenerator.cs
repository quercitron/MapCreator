using GeneralAlgorithms.GeometryBase;

using PerlinNoiseGeneration;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding.LandGenerators
{
    internal class PerlinNoiseLandGenerator : IMapBuilderComponent
    {
        private readonly double K = 0.4;

        private readonly double BaseK = 0.45;

        private readonly int NoiseFrequency = 8;

        public PerlinNoiseLandGenerator(int seed, PerlinNoiseGenerator perlinNoiseGenerator)
        {
            this.m_PerlinNoiseGenerator = perlinNoiseGenerator;
            this.Seed = seed;
        }

        public int Seed { get; set; }

        private readonly PerlinNoiseGenerator m_PerlinNoiseGenerator;

        public void Build(IMap map, MapSettings settings)
        {
            var noise = this.m_PerlinNoiseGenerator.GenerateNoise((int)map.Width, (int)map.Height, Seed, NoiseFrequency);

            var mapCenter = new Point2D(map.Width / 2, map.Height / 2);

            double l = 0;
            double r = 2;

            while (r - l > 1e-8)
            {
                double m = (l + r) / 2;
                int count = 0;
                foreach (var polygon in map.Polygons)
                {
                    var center = polygon.Center;

                    var dist = Geometry.SpecificDist(mapCenter, center, map.Width, map.Height);

                    if (polygon.IsInside && map.ContainsPointInside(center))
                    {
                        var noiseValue = noise[(int)center.X, (int)center.Y];

                        if (noiseValue > (this.BaseK + (dist > m ? (1 - this.BaseK) * (dist / m - 1) : 0)))
                        {
                            count++;
                        }
                    }
                }

                if (count > K * map.Polygons.Count)
                {
                    r = m;
                }
                else
                {
                    l = m;
                }
            }

            foreach (var polygon in map.Polygons)
            {
                var center = polygon.Center;

                var dist = Geometry.SpecificDist(mapCenter, center, map.Width, map.Height);

                if (polygon.IsInside && map.ContainsPointInside(center))
                {
                    var noiseValue = noise[(int)center.X, (int)center.Y];
                    /*polygon.IsLand = noise[(int) center.X, (int) center.Y] * (map.Width / (4 * dist + map.Width)) > 0.25;*/
                    polygon.IsLand = noiseValue > (this.BaseK + (dist > l ? (1 - this.BaseK) * (dist / l - 1) : 0));
                }
                else
                {
                    polygon.IsLand = false;
                }
            }
        }
    }
}
