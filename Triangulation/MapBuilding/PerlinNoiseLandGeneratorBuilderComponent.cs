using PerlinNoiseGeneration;

using Triangulation.Algorithm.GeometryBase;
using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal class PerlinNoiseLandGeneratorBuilderComponent : IMapBuilderComponent
    {
        private readonly double K = 0.4;

        private readonly double BaseK = 0.45;

        public PerlinNoiseLandGeneratorBuilderComponent(int seed, PerlinNoiseGenerator noiseGenerator)
        {
            m_NoiseGenerator = noiseGenerator;
            this.Seed = seed;
        }

        public int Seed { get; set; }

        private PerlinNoiseGenerator m_NoiseGenerator;

        public void Build(IMap map)
        {
            var noise = m_NoiseGenerator.GenerateNoise((int)map.Width, (int)map.Height, Seed, 8);

            var mapCenter = new Point2D(map.Width / 2, map.Height / 2);

            double l = 0;
            double r = 100000;

            while (r - l > 1e-5)
            {
                double m = (l + r) / 2;
                int count = 0;
                foreach (var polygon in map.Polygons)
                {
                    var center = polygon.Center;

                    var dist = center.Dist(mapCenter);

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

                var dist = center.Dist(mapCenter);

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
