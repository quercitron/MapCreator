namespace PerlinNoiseGeneration
{
    public class NoiseComponent
    {
        public NoiseComponent(double amplitude, double frequency)
        {
            Amplitude = amplitude;
            Frequency = frequency;
        }

        public double Amplitude { get; private set; }

        public double Frequency { get; private set; }
    }
}
