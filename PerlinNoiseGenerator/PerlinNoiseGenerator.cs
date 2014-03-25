using System;

using GeneralAlgorithms.RandomGenerator;

namespace PerlinNoiseGeneration
{
    public class PerlinNoiseGenerator : INoiseGenerator
    {
        private readonly INoiseComponentsGenerator _noiseComponentsGenerator;

        private readonly IRandomGeneratorFactory _randomGeneratorFactory;

        public PerlinNoiseGenerator(INoiseComponentsGenerator noiseComponentsGenerator, IRandomGeneratorFactory randomGeneratorFactory)
        {
            _noiseComponentsGenerator = noiseComponentsGenerator;
            _randomGeneratorFactory = randomGeneratorFactory;
        }

        public double[,] GenerateNoise(int width, int height, int seed, double baseFrequency, int count)
        {
            var result = new double[width,height];
            var randomGenerator = _randomGeneratorFactory.CreatreGenerator(seed);

            var noiseComponents = _noiseComponentsGenerator.GetComponents(baseFrequency, count);

            foreach (var component in noiseComponents)
            {
                var freqX = 2.0 * width / (width + height) * component.Frequency;
                var freqY = 2.0 * height / (width + height) * component.Frequency;

                int n = (int)Math.Ceiling(freqX);
                int m = (int)Math.Ceiling(freqY);

                var abc = new double[n + 1,m + 1];
                for (int i = 0; i < n + 1; i++)
                {
                    for (int j = 0; j < m + 1; j++)
                    {
                        abc[i, j] = randomGenerator.NextDouble() * component.Amplitude;
                    }
                }

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var aX = x * freqX / width;
                        var i = (int)aX;
                        var shiftX = aX - i;

                        var aY = y * freqY / height;
                        var j = (int)aY;
                        var shiftY = aY - j;

                        var val = (1 - shiftX) * (1 - shiftY) * abc[i, j]
                                  + shiftX * (1 - shiftY) * abc[i + 1, j]
                                  + (1 - shiftX) * shiftY * abc[i, j + 1]
                                  + shiftX * shiftY * abc[i + 1, j + 1];
                        // reflected, very funny
                        /*var val = (1 - shiftX) * (1 - shiftY) * abc[i + 1, j + 1]
                                  + shiftX * (1 - shiftY) * abc[i, j + 1]
                                  + (1 - shiftX) * shiftY * abc[i + 1, j]
                                  + shiftX * shiftY * abc[i, j];*/

                        result[x, y] += val;
                    }
                }
            }

            return result;
        }

        private static double InterpolateLinear(double a, double b, double x)
        {
            return a + (b - a) * x;
        }
    }
}
