using PerlinNoiseGeneration;

using Triangulation.Dividing;
using Triangulation.MapBuilding;
using Triangulation.MapObjects;

namespace Triangulation
{
    internal class MapFactory : IMapFactory
    {
        private readonly Structure m_Structure;

        public MapFactory(Structure structure)
        {
            m_Structure = structure;
        }

        public IMap CreateMap(int seed, MapSettings settings)
        {
            var map = new Map(m_Structure.Width, m_Structure.Height);

            new FormPolygons(m_Structure, new NoiseLineGenerator()).Build(map, settings);

            new CalculateDistanceFromEdgeBuilderComponent().Build(map, settings);

            new PerlinNoiseLandGenerator(seed, new PerlinNoiseGenerator()).Build(map, settings);

            new DefineOcean().Build(map, settings);

            new AssignCoastBuilderComponent().Build(map, settings);

            new PerlinNoiseElevation(new PerlinNoiseGenerator()).Build(map, settings);
            //new DistanceFromWaterElevation(new PerlinNoiseGenerator()).Build(map, settings);

            new AddRiversBuilderComponent().Build(map, settings);

            new MoistureGenerator().Build(map, settings);

            new SetTerranTypeFromSite().Build(map, settings);

            return map;
        }
    }
}
