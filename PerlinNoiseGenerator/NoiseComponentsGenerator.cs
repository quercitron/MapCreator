using System.Collections.Generic;

namespace PerlinNoiseGeneration
{
    public interface INoiseComponentsGenerator
    {
        IEnumerable<NoiseComponent> GetComponents(int count);

        IEnumerable<NoiseComponent> GetComponents(double baseFrequency, int count);
    }
}
