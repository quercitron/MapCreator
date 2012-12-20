using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using PerlinNoiseGeneration;

using Triangulation.MapObjects;

namespace Triangulation
{
    public partial class Form1 : Form
    {
        private Structure m_Structure;

        private Map m_Map;

        private Bitmap m_Bitmap;

        private readonly Random m_Random = new Random();
        private readonly PerlinNoiseGenerator m_NoiseGenerator = new PerlinNoiseGenerator();
        private const int AddRangeCount = 50000;

        private const int MaxWidth = 1200;
        private const int MaxHeight = 900;

        private readonly Color m_LakeColor = Color.CornflowerBlue;
        private readonly Color m_OceanColor = Color.RoyalBlue;

        private readonly Color m_RiverColor = Color.CornflowerBlue;

        public Form1()
        {
            InitializeComponent();

            AddCheckboxEvents();

            panel1.Width = MaxWidth;
            panel1.Height = MaxHeight;

            InitMap();
        }

        private void AddCheckboxEvents()
        {
            foreach (var checkbox in GetAll(this, typeof (CheckBox)))
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

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
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
            m_Bitmap = new Bitmap(MaxWidth + 1, MaxHeight + 1);

            Graphics graphics = Graphics.FromImage(m_Bitmap);
            Pen blackPen = new Pen(Color.Black);
            Pen redPen = new Pen(Color.Red);
            Brush whiteBrush = new SolidBrush(Color.White);   
         
            Brush oceanBrush = new SolidBrush(m_OceanColor);
            Brush lakeBrush = new SolidBrush(m_LakeColor);

            Pen riverPen = new Pen(m_RiverColor);

            Brush landBrush = new SolidBrush(Color.BurlyWood);
            Brush coastBrush = new SolidBrush(Color.Bisque);

            graphics.FillRectangle(whiteBrush, 0, 0, MaxWidth, MaxHeight);

            var maxDistFromWater = 1;// m_Map.Polygons.Max(p => p.DistanceForMoisture);

            if (checkBoxPolygons.Checked)
            {
                foreach (var polygon in m_Map.Polygons.Where(p => p.Corners.Count > 2))
                {
                    if (polygon.IsLand)
                    {
                        if (polygon.IsOceanCoast && checkBoxCoast.Checked)
                        {
                            graphics.FillPolygon(coastBrush, polygon.CornersToDraw.Select(p => (PointF) p).ToArray());
                        }
                        else
                        {
                            if (checkBoxElevation.Checked)
                            {                                
                                var lowLandColor = new MyColor(Color.Green);
                                var highLandColor = new MyColor(Color.Red);

                                Color color = (Color)(polygon.Elevation / maxDistFromWater * highLandColor + (1 - polygon.Elevation / maxDistFromWater) * lowLandColor);

                                graphics.FillPolygon(new SolidBrush(color), polygon.CornersToDraw.Select(p => (PointF)p).ToArray());    
                            }
                            else
                            {
                                graphics.FillPolygon(landBrush, polygon.CornersToDraw.Select(p => (PointF)p).ToArray());    
                            }                            
                        }
                    }
                    else
                    {
                        if (polygon.IsOcean)
                        {
                            graphics.FillPolygon(oceanBrush, polygon.CornersToDraw.Select(p => (PointF) p).ToArray());
                        }
                        if (polygon.IsLake)
                        {
                            graphics.FillPolygon(lakeBrush, polygon.CornersToDraw.Select(p => (PointF)p).ToArray());
                        }
                    }
                }
            }

            if (checkBoxLinealBorders.Checked)
            {
                foreach (var polygon in m_Map.Polygons.Where(p => p.Corners.Count > 2))
                {
                    graphics.DrawPolygon(blackPen, polygon.Corners.Select(p => (PointF)p).ToArray());
                }
            }

            if (checkBoxNoiseBorders.Checked)
            {
                foreach (var polygon in m_Map.Polygons.Where(p => p.Corners.Count > 2))
                {
                    graphics.DrawPolygon(new Pen(Color.Black), polygon.CornersToDraw.Select(p => (PointF)p).ToArray());
                }
            }

            if (checkBoxApplyNoise.Checked)
            {
                var noise = m_NoiseGenerator.GenerateNoise(MaxWidth, MaxHeight, m_Random.Next(),
                                                               Math.Max(Width, Height));

                const int baseAlfa = 150;

                for (int i = 0; i < MaxWidth; i++)
                {
                    for (int j = 0; j < MaxHeight; j++)
                    {
                        m_Bitmap.SetPixel(i, j, Color.FromArgb((int) (baseAlfa + (256 - baseAlfa) * noise[i, j]), m_Bitmap.GetPixel(i, j)));
                    }
                }
            }

            if (checkBoxCoast.Checked)
            {
                foreach (var border in m_Map.Borders)
                {
                    if (border.IsCoast)
                    {
                        Pen pen;
                        if (border.IsOceanCoast)
                        {
                            pen = new Pen(Color.Brown, 3);
                        }
                        else
                        {
                            pen = new Pen(Color.RoyalBlue, 2);
                        }

                        for (int i = 0; i < border.BorderToDraw.Count - 1; i++)
                        {
                            graphics.DrawLine(pen, (PointF) border.BorderToDraw[i], (PointF) border.BorderToDraw[i + 1]);
                        }
                    }
                }
            }

            if (checkBoxShowRivers.Checked)
            {
                foreach (var border in m_Map.Borders)
                {
                    if (border.RiverCapacity > 0 && !border.IsLake)
                    {
                        var riverPenWidth = (float) Math.Sqrt(30 * border.RiverCapacity / Math.Sqrt(m_Map.Polygons.Count));
                        riverPen.Width = riverPenWidth;
                        graphics.DrawLines(riverPen, border.BorderToDraw.Select(p => (PointF)p).ToArray());
                    }
                }
            }

            /*foreach (var polygon in m_Map.Polygons)
            {
                graphics.DrawString(polygon.DistanceFromEdge.ToString(CultureInfo.InvariantCulture), new Font("Thaoma", 10), Brushes.Black, (PointF)polygon.Center);
            }*/

            if (checkBoxShowTimes.Checked)
            {
                graphics.DrawString(m_Structure.LastActionLength.ToString(CultureInfo.InvariantCulture),
                                    new Font("Thaoma", 80), Brushes.Black, 10, 10);

                graphics.DrawString(m_Structure.LandCreation.ToString(CultureInfo.InvariantCulture),
                                    new Font("Thaoma", 80), Brushes.Black, 10, 100);

                graphics.DrawString(m_Structure.MapBuilding.ToString(CultureInfo.InvariantCulture),
                                    new Font("Thaoma", 80), Brushes.Black, 10, 190);

            }
            /*if (checkBoxPoints.Checked)
            {
                int r = 2;
                foreach (var corner in m_Map.Corners)
                {
                    graphics.DrawEllipse(blackPen, (float) corner.X - r, (float) corner.Y - r, 2 * r, 2 * r);
                }
            }

            if (checkBoxBorders.Checked)
            {
                foreach (var edge in m_Map.Borders)
                {
                    graphics.DrawLine(blackPen, (float) edge.Corners[0].X, (float) edge.Corners[0].Y,
                                      (float) edge.Corners[1].X,
                                      (float) edge.Corners[1].Y);
                }
            }

            if (checkBoxTriangles.Checked)
            {
                foreach (var edge in m_Map.Borders)
                {

                    graphics.DrawLine(redPen, (float) edge.Polygons[0].BasePoint.X,
                                      (float) edge.Polygons[0].BasePoint.Y,
                                      (float) edge.Polygons[1].BasePoint.X,
                                      (float) edge.Polygons[1].BasePoint.Y);
                }
            }*/

            /*int r = 2;

            if (checkBoxPoints.Checked)
            {
                foreach (var point in m_Structure.Points)
                {
                    graphics.DrawEllipse(blackPen, (float) point.X - r, (float) point.Y - r, 2 * r, 2 * r);
                }
            }*/

            /*foreach (var triangle in m_Structure.Triangles)
            {
                if (checkBoxTriangles.Checked)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Edge edge = triangle.Edge(i);
                        graphics.DrawLine(blackPen, (float) edge.First.X, (float) edge.First.Y, (float) edge.Second.X,
                                          (float) edge.Second.Y);
                    }
                }

                if (checkBoxBorders.Checked)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (triangle.Triangles[i] != null)
                        {
                            Point2D a = triangle.Center;
                            Point2D b = triangle.Triangles[i].Center;

                            graphics.DrawLine(redPen, (float) a.X, (float) a.Y, (float) b.X, (float) b.Y);
                        }
                    }
                }
            }*/
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
            AddRangeOfPoints(AddRangeCount);
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
