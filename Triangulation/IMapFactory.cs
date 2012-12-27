using Triangulation.Dividing;
using Triangulation.MapObjects;

namespace Triangulation
{
    internal interface IMapFactory
    {
        IMap CreateMap(Structure structure, int seed, MapSettings settings);
    }
}