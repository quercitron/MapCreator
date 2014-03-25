using IncrementalDelaunayTriangulation;

using PerlinNoiseGeneration;

using Triangulation.MapBuilding;
using Triangulation.MapBuilding.Elevation;
using Triangulation.MapBuilding.Lakes;
using Triangulation.MapBuilding.LandGenerators;
using Triangulation.MapBuilding.Moisture;
using Triangulation.MapBuilding.Polygons;
using Triangulation.MapBuilding.Rivers;
using Triangulation.MapBuilding.TerranType;
using Triangulation.MapObjects;

namespace Triangulation
{
    internal class MapFactory : IMapFactory
    {
        public IMap CreateMap(Structure structure, int seed, MapSettings settings)
        {
            var map = new Map(structure.Width, structure.Height);

            new FormPolygons(structure, new NoiseLineGenerator()).Build(map, settings);

            new CalculateDistanceFromEdgeBuilderComponent().Build(map, settings);

            new PerlinNoiseLandGenerator(seed, new PerlinNoiseGeneratorOld()).Build(map, settings);

            new DefineOcean().Build(map, settings);

            new AssignCoastBuilderComponent().Build(map, settings);

            new StructureLakes().Build(map, settings);

            new PerlinNoiseElevation(new PerlinNoiseGeneratorOld()).Build(map, settings);
            //new DistanceFromWaterElevation(new PerlinNoiseGenerator()).Build(map, settings);

            new AddRiversBuilderComponent(new CommonAddRiverStrategy()).Build(map, settings);

            new MoistureGenerator().Build(map, settings);

            new SetTerranTypeFromSite().Build(map, settings);

            return map;
        }
    }
}
