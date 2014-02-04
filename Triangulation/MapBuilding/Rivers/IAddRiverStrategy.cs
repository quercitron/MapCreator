using Triangulation.MapObjects;

namespace Triangulation.MapBuilding.Rivers
{
    internal interface IAddRiverStrategy
    {
        void AddRiver(Corner riverSource, bool onlyErosion);
    }
}
