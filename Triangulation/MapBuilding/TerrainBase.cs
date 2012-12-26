namespace Triangulation.MapBuilding
{
    internal class TerrainBase
    {
        public TerrainBase(double[,] nosise, double zeroLevel)
        {
            this.Nosise = nosise;
            this.ZeroLevel = zeroLevel;
        }

        public double[,] Nosise { get; private set; }
        public double ZeroLevel { get; private set; }
    }
}
