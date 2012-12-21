using Triangulation.MapObjects;

namespace Triangulation
{
    public interface IMapFactory
    {
        IMap CreateMap(int seed);
    }
}