using System;

namespace PerlinNoiseGeneration
{
    public abstract class BaseNoiseGenerator : INoiseGenerator
    {
        public abstract double[,] GenerateNoise(int width, int height, int seed, double frequency);

        public double[,] GenerateNormNoise(int width, int height, int seed, double frequency)
        {
            var result = GenerateNoise(width, height, seed, frequency);

            Normalize(result);

            return result;
        }

        public double[,] GeneratePolarNoise(int width, int height, int seed, double frequency)
        {
            var result = GenerateNoise(width, height, seed, frequency);

            Polarize(result);

            return result;
        }

        private static void Polarize(double[,] noise)
        {
            Normalize(noise);

            double l = 1.5;
            double d = 0.5;

            for (int i = 0; i < noise.GetLength(0); i++)
            {
                for (int j = 0; j < noise.GetLength(1); j++)
                {
                    if (noise[i, j] < d)
                    {
                        noise[i, j] /= l;
                    }
                    else
                    {
                        noise[i, j] = (l - d) / (1 - d) / l * noise[i, j] + d * (l - 1) / l / (d - 1);
                    }
                }
            }            
        }

        private static void Normalize(double[,] noise)
        {
            double minNoise = 1;
            double maxNoise = 0;
            foreach (double d in noise)
            {
                minNoise = Math.Min(minNoise, d);
                maxNoise = Math.Max(maxNoise, d);
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