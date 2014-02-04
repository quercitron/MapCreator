namespace GeneralAlgorithms.RandomGenerator
{
    abstract class BaseRandomGenerator : IRandomGenerator
    {
        public abstract int Next();

        public abstract int Next(int maxValue);

        public int Next(int minValue, int maxValue)
        {
            return minValue + Next(maxValue - minValue);
        }

        public abstract double NextDouble();

        public double NextDouble(double maxValue)
        {
            return NextDouble() * maxValue;
        }

        public double NextDouble(double minValue, double maxValue)
        {
            return minValue + NextDouble() * (maxValue - minValue);
        }
    }
}
