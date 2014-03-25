namespace GeneralAlgorithms.RandomGenerator
{
    public interface IRandomGeneratorFactory
    {
        IRandomGenerator CreatreGenerator();

        IRandomGenerator CreatreGenerator(int seed);
    }
}
