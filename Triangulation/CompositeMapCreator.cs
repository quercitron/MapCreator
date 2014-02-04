using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using IncrementalDelaunayTriangulation;

using Triangulation.MapBuilding;
using Triangulation.MapObjects;

namespace Triangulation
{
    public class CompositeMapCreator : IMapFactory
    {
        public CompositeMapCreator()
        {
            m_ComponentsRegistry = new List<IMapBuilderComponent>();
        }

        private readonly List<IMapBuilderComponent> m_ComponentsRegistry;

        public IMap CreateMap(Structure structure, int seed, MapSettings settings)
        {
            var map = new Map(structure.Width, structure.Height);

            var report = new StringBuilder();
            var stopwatch = new Stopwatch();

            foreach (var component in m_ComponentsRegistry)
            {
                stopwatch.Reset();
                stopwatch.Start();
                component.Build(map, settings);
                stopwatch.Stop();
                report.AppendLine(string.Format("{0}: {1} ms", component.GetType().Name, stopwatch.ElapsedMilliseconds));                
            }

            map.CreationReport = report.ToString();

            return map;
        }

        public void Register(IMapBuilderComponent component)
        {
            m_ComponentsRegistry.Add(component);
        }
    }
}
