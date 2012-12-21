using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal interface IMapBuilderComponent
    {
        void Build(IMap map);
    }
}
