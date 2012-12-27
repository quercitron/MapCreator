using Triangulation.MapObjects;

namespace Triangulation.MapBuilding
{
    internal interface IAddRiverStrategy
    {
        void AddRiver(Corner riverSource, bool onlyErosion);
    }
}
