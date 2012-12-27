namespace PerlinNoiseGeneration
{
    public abstract class NoiseDecorator : INoiseGenerator
    {
        protected NoiseDecorator(INoiseGenerator generator)
        {
            this.m_Generator = generator;
        }

        private INoiseGenerator m_Generator;

        public virtual double[,] GenerateNoise(int width, int height, int seed, double frequency)
        {
            return m_Generator.GenerateNoise(width, height, seed, frequency);
        }
    }
}
