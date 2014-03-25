namespace GeneralAlgorithms.RandomGenerator.DefaultGenerator
{
    public class DefaultRandomGeneratorFactory : IRandomGeneratorFactory
    {
        public IRandomGenerator CreatreGenerator()
        {
            return new DefaultRandomGenerator();
        }

        public IRandomGenerator CreatreGenerator(int seed)
        {
            return new DefaultRandomGenerator(seed);
        }
    }
}
