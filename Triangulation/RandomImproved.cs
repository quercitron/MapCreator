using System;

using GeneralAlgorithms.RandomGenerator;

namespace Triangulation
{
    public class RandomImproved : Random, IRandomGenerator
    {
        public double NextDouble(double maxValue)
        {
            return NextDouble() * maxValue;
        }

        public double NextDouble(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("MinValue must be less or equal to MaxValue");
            }

            return minValue + NextDouble() * (maxValue - minValue);
        }
    }
}
