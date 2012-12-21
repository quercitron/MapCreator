using System.Drawing;

using Triangulation.MapObjects;

namespace Triangulation.MapPainter
{
    internal interface IMapPainter
    {
        Bitmap DrawMap(IMap map, DrawSettings settings);
    }
}
