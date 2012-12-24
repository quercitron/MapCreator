using PerlinNoiseGeneration;

using Triangulation.Dividing;
using Triangulation.MapBuilding;
using Triangulation.MapBuilding.LandGenerators;
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

            new FormPolygonsBuilderComponent(m_Structure, new NoiseLineGenerator()).Build(map, settings);

            new CalculateDistanceFromEdgeBuilderComponent().Build(map, settings);

            new PerlinNoiseLandGenerator(seed, new PerlinNoiseGenerator()).Build(map, settings);

            new DefineWaterTypesBuilderComponent().Build(map, settings);

            new AssignCoastBuilderComponent().Build(map, settings);

            new CalculateElevationBuilderComponent(new PerlinNoiseGenerator()).Build(map, settings);

            new AddRiversBuilderComponent().Build(map, settings);

            new MoistureGenerator().Build(map, settings);

            new SetTerranType().Build(map, settings);

            return map;
        }
    }
}
