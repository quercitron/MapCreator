using PerlinNoiseGeneration;

using Triangulation.Dividing;
using Triangulation.MapBuilding;
using Triangulation.MapObjects;

namespace Triangulation
{
    public class MapFactory : IMapFactory
    {
        private readonly Structure m_Structure;

        public MapFactory(Structure structure)
        {
            m_Structure = structure;
        }

        public IMap CreateMap(int seed)
        {
            var map = new Map(m_Structure.Width, m_Structure.Height);

            new FormPolygonsBuilderComponent(m_Structure, new NoiseLineGenerator()).Build(map);

            new CalculateDistanceFromEdgeBuilderComponent().Build(map);

            new PerlinNoiseLandGeneratorBuilderComponent(seed, new PerlinNoiseGenerator()).Build(map);

            new DefineWaterTypesBuilderComponent().Build(map);

            new AssignCoastBuilderComponent().Build(map);

            new CalculateElevationBuilderComponent(new PerlinNoiseGenerator()).Build(map);

            new AddRiversBuilderComponent().Build(map);

            new CalculateMoistureBuilderComponent().Build(map);

            return map;
        }
    }
}
