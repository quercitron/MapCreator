namespace GeneralAlgorithms.RandomGenerator.MersenneTwisterGenerator
{
    public class MersenneTwisterRandomGeneratorFactory : IRandomGeneratorFactory
    {
        public IRandomGenerator CreatreGenerator()
        {
            return new MersenneTwisterRandomGenerator();
        }

        public IRandomGenerator CreatreGenerator(int seed)
        {
            return new MersenneTwisterRandomGenerator(seed);
        }
    }
}
