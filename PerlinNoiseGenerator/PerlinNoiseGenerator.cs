using System;

namespace PerlinNoiseGeneration
{
    public class PerlinNoiseGenerator : BaseNoiseGenerator 
    {        
        public override double[,] GenerateNoise(int width, int height, int seed, double frequency)
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
    }
}