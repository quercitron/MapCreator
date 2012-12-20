using System;

namespace Triangulation.Algorithm.RandomGenerator
{
    class DefaultRandomGenerator : BaseRandomGenerator
    {
        public DefaultRandomGenerator()
        {
            m_DefaultBaseGenerator = new Random();
        }

        private readonly Random m_DefaultBaseGenerator;

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
