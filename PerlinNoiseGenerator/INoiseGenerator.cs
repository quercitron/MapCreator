namespace PerlinNoiseGeneration
{
    public interface INoiseGenerator
    {
        double[,] GenerateNoise(int width, int height, int seed, double baseFrequency, int count);

        //double[,] GenerateNormNoise(int width, int height, int seed, double frequency);

        //double[,] GeneratePolarNoise(int width, int height, int seed, double frequency);
    }
}