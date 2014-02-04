using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

using GeneralAlgorithms.GeometryBase;

using IncrementalDelaunayTriangulation;

using MapGenerator.Utils;

using Triangulation.MapObjects;
using Triangulation.MapPainter;

namespace Triangulation
{
    public class MapCreatorModel
    {
        public MapCreatorModel(double maxWidth, double maxHeight)
        {
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
            
            m_DefaultSettings = new MapSettings
            {
                LandPart = 0.4
            };

            CreateNewStructure();
        }

        public void AddPoint(Point2D point)
        {
            m_Structure.AddPoint(point);

            UpdateMap();
        }

        public void AddRangeOfPoints(int total)
        {
            var points = new List<Point2D>(total);
            for (int i = 0; i < total; i++)
            {
                points.Add(new Point2D(m_Random.NextDouble() * MaxWidth, m_Random.NextDouble() * MaxHeight));
            }
            m_Structure.AddPointRange(points);

            UpdateMap();
        }

        public void Redraw()
        {
            int currentCount = m_Structure.Points.Count;

            CreateNewStructure();

            AddRangeOfPoints(currentCount - 4);

            UpdateMap();
        }

        public void Reset()
        {
            CreateNewStructure();
        }

        public void SaveMapImage()
        {
            _imageSaver.SaveBitmap(Bitmap, Seed);
        }

        public void UpdateImage()
        {
            if (m_Map != null)
            {
                Bitmap = m_MapPainter.DrawMap(m_Map, DrawSettings);

                if (ImageUpdated != null)
                {
                    ImageUpdated(this, null);
                }
            }
        }

        public DrawSettings DrawSettings { get; set; }

        public Bitmap Bitmap { get; private set; }

        public double MaxWidth { get; set; }

        public double MaxHeight { get; set; }

        public EventHandler ImageUpdated;

        public int Seed { get; set; }

        private Structure m_Structure;

        private IMap m_Map;

        private readonly IMapFactory m_MapFactory = new MapFactory();

        private readonly IMapPainter m_MapPainter = new CommonMapPainter();

        private ImageSaver _imageSaver = new ImageSaver("Images");

        private readonly MapSettings m_DefaultSettings;

        // TODO: change random

        private readonly Random m_Random = new Random();

        private void CreateNewStructure()
        {
            m_Structure = new Structure(MaxWidth, MaxHeight);

            //var mapFactory = new MapFactory(m_Structure);
            //m_Map = mapFactory.CreateMap((int)numericUpDownSeed.Value, m_DefaultSettings);

            //m_Structure.StructureChanged += OnStrucureChanged;
        }

        private void OnStrucureChanged(object sender, EventArgs e)
        {
            UpdateMap();
        }

        private void UpdateMap()
        {
            var thread = new Thread(() => m_Map = m_MapFactory.CreateMap(m_Structure, Seed, m_DefaultSettings), 1024 * 1024 * 100);

            thread.Start();
            thread.Join();

            UpdateImage();
        }
    }
}
