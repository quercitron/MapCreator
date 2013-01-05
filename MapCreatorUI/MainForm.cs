using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Triangulation;
using Triangulation.Algorithm.GeometryBase;
using Triangulation.MapPainter;

namespace MapCreatorUI
{
    public partial class MainForm : Form
    {
        private readonly MapCreatorModel m_MapCreatorModel;

        private readonly Random m_Random = new Random();

        private const int MaxWidth = 1200;
        //private const int MaxWidth = 1920;

        private const int MaxHeight = 900;
        //private const int MaxHeight = 1080;

        public MainForm()
        {
            InitializeComponent();

            AddCheckboxEvents();

            m_MapCreatorModel = new MapCreatorModel(MaxWidth, MaxHeight)
                { Seed = (int)this.numericUpDownSeed.Value, DrawSettings = this.GetSettings() };
            m_MapCreatorModel.ImageUpdated += this.RedrawMapImage;

            panel1.Width = MaxWidth;
            panel1.Height = MaxHeight;
        }

        private void RedrawMapImage(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void AddCheckboxEvents()
        {
            foreach (var checkbox in this.GetAllControls(this, typeof (CheckBox)))
            {
                ((CheckBox) checkbox).CheckedChanged += UpdateSettings;
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

        private void UpdateSettings(object sender, EventArgs e)
        {
            m_MapCreatorModel.DrawSettings = this.GetSettings();
            m_MapCreatorModel.UpdateImage();
        }

        public IEnumerable<Control> GetAllControls(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>().ToList();

            return controls.SelectMany(ctrl => this.GetAllControls(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private DrawSettings GetSettings()
        {
            var settings = new DrawSettings
                {
                    ApplyNoise = this.checkBoxApplyNoise.Checked,
                    DisplayCoast = this.checkBoxCoast.Checked,
                    DisplayCoastline = this.checkBoxCoastLine.Checked,
                    DisplayElevation = this.checkBoxElevation.Checked,
                    DisplayMoisture = this.checkBoxShowMoisture.Checked,
                    DisplayLinealBorders = this.checkBoxLinealBorders.Checked,
                    DisplayNoiseBorders = this.checkBoxNoiseBorders.Checked,
                    DisplayPolygons = this.checkBoxPolygons.Checked,
                    DisplayRivers = this.checkBoxShowRivers.Checked,
                    DisplayLakes = this.checkBoxShowLakes.Checked
                };
            return settings;
        }

        private void ButtonAddRandomPointClick(object sender, EventArgs e)
        {
            Point2D point = new Point2D(m_Random.NextDouble() * MaxWidth, m_Random.NextDouble() * MaxHeight);

            // TODO: centralize try/catch
            try
            {                
                m_MapCreatorModel.AddPoint(point);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void ButtonAddRangeClick(object sender, EventArgs e)
        {
            m_MapCreatorModel.AddRangeOfPoints((int)nudAddCount.Value);
        }

        private void Panel1Paint(object sender, PaintEventArgs e)
        {
            if (m_MapCreatorModel.Bitmap != null)
            {
                e.Graphics.DrawImage(m_MapCreatorModel.Bitmap, 0, 0);
            }
        }

        private void Panel1MouseClick(object sender, MouseEventArgs e)
        {
            // TODO: think about functionality
            /*base.OnMouseClick(e);

            m_MapCreatorModel.AddPoint(new Point2D(e.X, e.Y));*/
        }

        private void ButtonResetClick(object sender, EventArgs e)
        {
            //InitMap();
            m_MapCreatorModel.Reset();
        }

        private void ButtonRedrawClick(object sender, EventArgs e)
        {
            m_MapCreatorModel.Redraw();
        }

        private void ButtonSeedClick(object sender, EventArgs e)
        {
            numericUpDownSeed.Value = m_Random.Next((int) numericUpDownSeed.Maximum);
        }

        private void ButtonSaveClick(object sender, EventArgs e)
        {
            m_MapCreatorModel.SaveMapImage();
        }

        private void NumericUpDownSeedValueChanged(object sender, EventArgs e)
        {
            m_MapCreatorModel.Seed = (int)this.numericUpDownSeed.Value;
        }
    }
}
