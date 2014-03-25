namespace PerlinNoiseGeneration
{
    public abstract class NoiseDecorator : INoiseGenerator
    {
        protected NoiseDecorator(INoiseGenerator generator)
        {
            this.m_Generator = generator;
        }

        private readonly INoiseGenerator m_Generator;

        public virtual double[,] GenerateNoise(int width, int height, int seed, double baseFrequency, int count)
        {
            return m_Generator.GenerateNoise(width, height, seed, baseFrequency, count);
        }
    }
}
