using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using PerlinNoiseGeneration;

using Triangulation.Algorithm.GeometryBase;
using Triangulation.Dividing;
using Triangulation.MapObjects;
using Triangulation.MapPainter;

namespace Triangulation
{
    public partial class MainForm : Form
    {
        private Structure m_Structure;

        private IMap m_Map;

        private Bitmap m_Bitmap;

        private readonly Random m_Random = new Random();
        private const int AddRangeCount = 10;

        private const int MaxWidth = 1200;
        private const int MaxHeight = 900;

        public MainForm()
        {
            InitializeComponent();

            AddCheckboxEvents();

            panel1.Width = MaxWidth;
            panel1.Height = MaxHeight;

            InitMap();
        }

        private void AddCheckboxEvents()
        {
            foreach (var checkbox in this.GetAllControls(this, typeof (CheckBox)))
            {
                ((CheckBox) checkbox).CheckedChanged += RedrawMap;
            }

            /*checkBoxLinealBorders.CheckedChanged += RedrawMap;
            checkBoxNoiseBorders.CheckedChanged += RedrawMap;
            checkBoxPoints.CheckedChanged += RedrawMap;
            checkBoxTriangles.CheckedChanged += RedrawMap;
            checkBoxPolygons.CheckedChanged += RedrawMap;
            checkBoxShowTimes.CheckedChanged += RedrawMap;
            checkBoxApplyNoise.CheckedChanged += RedrawMap;
            checkBoxCoast.CheckedChanged += RedrawMap;*/
        }

        public IEnumerable<Control> GetAllControls(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => this.GetAllControls(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private void InitMap()
        {
            CreateNewStructure();

            DrawMap();
            this.Refresh();
        }

        private void CreateNewStructure()
        {
            m_Structure = new Structure(MaxWidth, MaxHeight);

            var mapFactory = new MapFactory(m_Structure);
            m_Map = mapFactory.CreateMap((int)numericUpDown1.Value);

            m_Structure.StructureChanged += OnStrucureChanged;
        }

        private void OnStrucureChanged(object sender, EventArgs e)
        {
            var mapFactory = new MapFactory(m_Structure);
            m_Map = mapFactory.CreateMap((int)numericUpDown1.Value);

            RedrawMap(sender, e);
        }

        private void RedrawMap(object sender, EventArgs e)
        {
            DrawMap();
            this.Refresh();
        }

        private void DrawMap()
        {
            var settings = GetSettings();
            m_Bitmap = new CommonMapPainter().DrawMap(m_Map, settings);
        }

        private DrawSettings GetSettings()
        {
            var settings = new DrawSettings
                {
                    ApplyNoise = this.checkBoxApplyNoise.Checked,
                    DisplayCoast = this.checkBoxCoast.Checked,
                    DisplayElevation = this.checkBoxElevation.Checked,
                    DisplayLinealBorders = this.checkBoxLinealBorders.Checked,
                    DisplayNoiseBorders = this.checkBoxNoiseBorders.Checked,
                    DisplayPolygons = this.checkBoxPolygons.Checked,
                    DisplayRivers = this.checkBoxShowRivers.Checked
                };
            return settings;
        }

        private void ButtonAddRandomPointClick(object sender, EventArgs e)
        {
            Point2D point = new Point2D(m_Random.NextDouble() * MaxWidth, m_Random.NextDouble() * MaxHeight);

            try
            {                
                m_Structure.AddPoint(point);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void ButtonAddRangeClick(object sender, EventArgs e)
        {
            AddRangeOfPoints((int) nudAddCount.Value);
        }

        private void AddRangeOfPoints(int total)
        {
            List<Point2D> points = new List<Point2D>(total);
            for (int i = 0; i < total; i++)
            {
                points.Add(new Point2D(m_Random.NextDouble() * MaxWidth, m_Random.NextDouble() * MaxHeight));
            }
            m_Structure.AddPointRange(points);
        }

        private void Panel1Paint(object sender, PaintEventArgs e)
        {
            if (m_Bitmap != null)
            {
                e.Graphics.DrawImage(m_Bitmap, 0, 0);
            }
        }

        private void Panel1MouseClick(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);

            m_Structure.AddPoint(new Point2D(e.X, e.Y));
        }

        private void ButtonResetClick(object sender, EventArgs e)
        {
            InitMap();
        }

        private void ButtonRedrawClick(object sender, EventArgs e)
        {
            int currentCount = m_Structure.Points.Count;

            CreateNewStructure();

            AddRangeOfPoints(currentCount - 4);
        }

        private void ButtonSeedClick(object sender, EventArgs e)
        {
            numericUpDown1.Value = m_Random.Next((int) numericUpDown1.Maximum);
        }
    }
}
