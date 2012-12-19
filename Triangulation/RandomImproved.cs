using System;

namespace Triangulation
{
    public class RandomImproved : Random
    {
        public double NextDouble(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("MinValue must be less or equal to MaxValue");
            }

            return minValue + base.NextDouble() * (maxValue - minValue);
        }
    }
}
