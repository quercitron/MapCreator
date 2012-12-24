using Triangulation.MapObjects;

namespace Triangulation
{
    internal interface IMapFactory
    {
        IMap CreateMap(int seed, MapSettings settings);
    }
}