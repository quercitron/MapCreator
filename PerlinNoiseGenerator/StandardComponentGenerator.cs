using System;
using System.Collections.Generic;

namespace PerlinNoiseGeneration
{
    public class StandardComponentGenerator : INoiseComponentsGenerator
    {
        private readonly double _baseAmplitude;

        private readonly double _baseFrequency;

        public StandardComponentGenerator()
            : this(0.5, 1)
        {
        }

        public StandardComponentGenerator(double baseAmplitude, double baseFrequency)
        {
            _baseAmplitude = baseAmplitude;
            _baseFrequency = baseFrequency;
        }

        public IEnumerable<NoiseComponent> GetComponents(int count)
        {
            return GetComponents(_baseFrequency, count);
        }

        public IEnumerable<NoiseComponent> GetComponents(double baseFrequency, int count)
        {
            var amplitude = _baseAmplitude;
            var frequency = baseFrequency;
            for (int i = 0; i < count; i++)
            {
                yield return new NoiseComponent(amplitude, frequency);
                amplitude /= 2;
                frequency *= 2;
            }
        }
    }
}
