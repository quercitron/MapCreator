using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using MapGenerator.Utils;
using Triangulation.Algorithm.GeometryBase;
using Triangulation.Dividing;
using Triangulation.MapObjects;
using Triangulation.MapPainter;

namespace Triangulation
{
    public class MapCreatorModel
    {
        public MapCreatorModel(double maxWidth, double maxHeight)
        {
            this.MaxWidth = maxWidth;
            this.MaxHeight = maxHeight;
            
            m_DefaultSettings = new MapSettings
            {
                LandPart = 0.4
            };

            this.CreateNewStructure();
        }

        private Structure m_Structure;

        private IMap m_Map;

        private readonly IMapFactory m_MapFactory = new MapFactory();

        private readonly IMapPainter m_MapPainter = new CommonMapPainter();

        private Saver m_Saver = new Saver("Images");

        public DrawSettings DrawSettings { get; set; }

        private readonly MapSettings m_DefaultSettings;

        // TODO: change random
        private readonly Random m_Random = new Random();

        public Bitmap Bitmap { get; set; }

        private void CreateNewStructure()
        {
            m_Structure = new Structure(MaxWidth, MaxHeight);

            //var mapFactory = new MapFactory(m_Structure);
            //m_Map = mapFactory.CreateMap((int)numericUpDownSeed.Value, m_DefaultSettings);

            m_Structure.StructureChanged += OnStrucureChanged;
        }

        public double MaxWidth { get; set; }

        public double MaxHeight { get; set; }

        public EventHandler ImageUpdated; 

        private void OnStrucureChanged(object sender, EventArgs e)
        {
            this.UpdateMap();
        }

        public void AddPoint(Point2D point)
        {

            m_Structure.AddPoint(point);
        }

        public void AddRangeOfPoints(int total)
        {
            var points = new List<Point2D>(total);
            for (int i = 0; i < total; i++)
            {
                points.Add(new Point2D(m_Random.NextDouble() * MaxWidth, m_Random.NextDouble() * MaxHeight));
            }
            m_Structure.AddPointRange(points);
        }

        public void Reset()
        {
            CreateNewStructure();
        }

        public void Redraw()
        {
            int currentCount = m_Structure.Points.Count;

            CreateNewStructure();

            AddRangeOfPoints(currentCount - 4);
        }

        public void SaveMapImage()
        {
            m_Saver.SaveBitmap(Bitmap, Seed);
        }

        private void UpdateMap()
        {
            var thread = new Thread(() => m_Map = m_MapFactory.CreateMap(m_Structure, Seed, m_DefaultSettings), 1024 * 1024 * 100);

            thread.Start();
            thread.Join();

            this.UpdateImage();
        }

        public void UpdateImage()
        {
            if (m_Map != null)
            {
                Bitmap = m_MapPainter.DrawMap(m_Map, DrawSettings);

                if (this.ImageUpdated != null)
                {
                    this.ImageUpdated(this, null);
                }
            }
        }

        public int Seed { get; set; }
    }
}
