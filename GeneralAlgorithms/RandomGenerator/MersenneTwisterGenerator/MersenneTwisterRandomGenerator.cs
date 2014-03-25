using NPack;

namespace GeneralAlgorithms.RandomGenerator.MersenneTwisterGenerator
{
    public class MersenneTwisterRandomGenerator : MersenneTwister, IRandomGenerator
    {
        public MersenneTwisterRandomGenerator()
        {
        }

        public MersenneTwisterRandomGenerator(int seed)
            : base(seed)
        {
        }

        public double NextDouble(double maxValue)
        {
            return maxValue * base.NextDouble();
        }

        public double NextDouble(double minValue, double maxValue)
        {
            return minValue + (maxValue - minValue) * base.NextDouble();
        }
    }
}
