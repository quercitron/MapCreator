using System;

namespace PerlinNoiseGeneration
{
    public class NormNoiseDecorator : NoiseDecorator
    {
        public NormNoiseDecorator(INoiseGenerator generator)
            : base(generator)
        {
        }

        public override double[,] GenerateNoise(int width, int height, int seed, double baseFrequency, int count)
        {
            var result = base.GenerateNoise(width, height, seed, baseFrequency, count);

            Normalize(result);

            return result;
        }

        private static void Normalize(double[,] noise)
        {
            double minNoise = 1;
            double maxNoise = 0;
            foreach (var value in noise)
            {
                minNoise = Math.Min(minNoise, value);
                maxNoise = Math.Max(maxNoise, value);
            }

            for (int i = 0; i < noise.GetLength(0); i++)
            {
                for (int j = 0; j < noise.GetLength(1); j++)
                {
                    noise[i, j] = (noise[i, j] - minNoise) / (maxNoise - minNoise);
                }
            }
        }
    }
}
