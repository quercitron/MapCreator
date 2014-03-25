using System;

using GeneralAlgorithms.GeometryBase;

using PerlinNoiseGeneration;

using Triangulation.MapObjects;

namespace Triangulation.MapBuilding.LandGenerators
{
    class ImprovedPerlinNoiseLandGenerator : IMapBuilderComponent
    {
        public ImprovedPerlinNoiseLandGenerator(int seed, PerlinNoiseGeneratorOld perlinNoiseGenerator)
        {
            this.m_PerlinNoiseGenerator = perlinNoiseGenerator;
            Seed = seed;
        }

        public int Seed { get; set; }

        private readonly PerlinNoiseGeneratorOld m_PerlinNoiseGenerator;

        private readonly double m_Indent = 0.3;

        public void Build(IMap map, MapSettings settings)
        {
            var noise = this.m_PerlinNoiseGenerator.GenerateNoise((int) map.Width, (int) map.Height, Seed, 8);

            CreateIndent(noise, map.Width, map.Height, m_Indent);

            double l = 0;
            double r = 1;
            while (r - l > 1e-8)
            {
                var m = (l + r)/2;
                int land = 0;
                int total = 0;
                foreach (var polygon in map.Polygons)
                {
                    var center = polygon.Center;
                    if (polygon.IsInside && map.ContainsPointInside(center))
                    {
                        total++;
                        var noiseValue = noise[(int) center.X, (int) center.Y];

                        if (noiseValue > m)
                        {
                            land++;
                        }
                    }
                }

                if (land > 0.4*total)
                {
                    l = m;
                }
                else
                {
                    r = m;
                }
            }

            foreach (var polygon in map.Polygons)
            {
                var center = polygon.Center;
                if (polygon.IsInside && map.ContainsPointInside(center))
                {
                    var noiseValue = noise[(int)center.X, (int)center.Y];

                    if (noiseValue > l)
                    {
                        polygon.IsLand = true;
                    }
                }
            }
        }

        private void CreateIndent(double[,] noise, double width, double height, double indent)
        {
            var mapCenter = new Point2D(width/2, height/2);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var point = new Point2D(i, j);
                    var dist = Geometry.SpecificDist(mapCenter, point, width, height);
                    if (dist > 1 - indent)
                    {
                        double overhead = (dist - (1 - indent)) / indent;
                        noise[i, j] = Math.Max(0, noise[i, j]*(1 - overhead));
                    }
                }
            }
        }
    }
}
