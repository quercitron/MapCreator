using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    public interface IMapBuilderComponent
    {
        void Build(IMap map, MapSettings settings);
    }
}
