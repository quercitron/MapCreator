namespace PerlinNoiseGeneration
{
    public class SlowedHeightIncreaseNoiseDecorator : NoiseDecorator
    {
        public SlowedHeightIncreaseNoiseDecorator(INoiseGenerator generator)
            : base(generator)
        {
        }

        public override double[,] GenerateNoise(int width, int height, int seed, double frequency)
        {
            var result = base.GenerateNoise(width, height, seed, frequency);

            Polarize(result);

            return result;
        }

        private static void Polarize(double[,] noise)
        {
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
    }
}
