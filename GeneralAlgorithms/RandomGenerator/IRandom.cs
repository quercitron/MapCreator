namespace GeneralAlgorithms.RandomGenerator
{
    public interface IRandomGenerator
    {
        int Next();

        int Next(int maxValue);

        int Next(int minValue, int maxValue);

        double NextDouble();

        double NextDouble(double maxValue);

        double NextDouble(double minValue, double maxValue);
    }
}
