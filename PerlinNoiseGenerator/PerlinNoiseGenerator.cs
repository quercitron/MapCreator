using System;

namespace PerlinNoiseGeneration
{
    public class PerlinNoiseGenerator
    {        
        public double[,] GenerateNoise(int width, int height, int seed, double frequency)
        {            
            var result = new double[width,height];            

            double amplitude = 1;

            int numberOfOctaves = Math.Max((int) (Math.Log(Math.Max(width, height) / frequency) / Math.Log(2)), 2);

            for (int i = 0; i < numberOfOctaves; i++)
            {
                amplitude /= 2;                

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        result[x, y] += InterpolatedNoise2D(x * frequency / width, y * frequency / height, seed) * amplitude;
                    }
                }

                frequency *= 2;
            }

            return result;
        }

        private static double Noise2D(int x, int y, int seed)
        {
            long n = x + (long)y * 57 + seed;
            n = (n << 13) ^ n;
            return (1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }

        private static double Noise2DByRandomClass(int x, int y)
        {
            return new Random(x + 10000 * y).NextDouble();            
        }

        private static double InterpolatedNoise2D(double x, double y, int seed)
        {
            int integerX = (int) x;
            double fractionalX = x - integerX;
            int integerY = (int) y;
            double fractionalY = y - integerY;

            double v1 = SmoothedNoise2D(integerX, integerY, seed);
            double v2 = SmoothedNoise2D(integerX + 1, integerY, seed);
            double v3 = SmoothedNoise2D(integerX, integerY + 1, seed);
            double v4 = SmoothedNoise2D(integerX + 1, integerY + 1, seed);

            double i1 = InterpolateLinear(v1, v2, fractionalX);
            double i2 = InterpolateLinear(v3, v4, fractionalX);
            return InterpolateLinear(i1, i2, fractionalY);
        }

        private static double InterpolateLinear(double a, double b, double x)
        {
            return a + (b - a) * x;
        }

        private static double SmoothedNoise2D(int x, int y, int seed)
        {
            return Math.Abs(Noise2D(x, y, seed));
        }

        public double[,] GenerateNormNoise(int width, int height, int seed, int frequency)
        {
            var result = GenerateNoise(width, height, seed, frequency);

            Normalize(result);

            return result;
        }

        public double[,] GeneratePolarNoise(int width, int height, int seed, int frequency)
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