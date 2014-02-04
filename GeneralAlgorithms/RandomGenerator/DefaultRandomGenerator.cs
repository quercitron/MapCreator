using System;

namespace GeneralAlgorithms.RandomGenerator
{
    class DefaultRandomGenerator : BaseRandomGenerator
    {
        private readonly Random m_DefaultBaseGenerator;

        public DefaultRandomGenerator()
        {
            m_DefaultBaseGenerator = new Random();
        }

        public override int Next()
        {
            return m_DefaultBaseGenerator.Next();
        }

        public override int Next(int maxValue)
        {
            return m_DefaultBaseGenerator.Next(maxValue);
        }

        public override double NextDouble()
        {
            return m_DefaultBaseGenerator.NextDouble();
        }
    }
}
