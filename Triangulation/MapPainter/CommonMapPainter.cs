using System;
using System.Drawing;
using System.Linq;

using PerlinNoiseGeneration;

using Triangulation.MapObjects;

namespace Triangulation.MapPainter
{
    internal class CommonMapPainter : IMapPainter
    {
        private const int BaseAlfa = 150;

        // TODO: Change palette technic
        private readonly Palette m_Palette = new Palette();
        private readonly PerlinNoiseGenerator m_NoiseGenerator = new PerlinNoiseGenerator();
        private readonly Random m_Random = new Random();

        public Bitmap DrawMap(IMap map, DrawSettings settings)
        {
            int width = (int)map.Width;
            int height = (int)map.Height;
            var bitmap = new Bitmap(width + 1, height + 1);

            Graphics graphics = Graphics.FromImage(bitmap);
            Pen blackPen = new Pen(Color.Black);
            Pen redPen = new Pen(Color.Red);
            Brush whiteBrush = new SolidBrush(Color.White);

            Brush oceanBrush = new SolidBrush(m_Palette.OceanColor);
            Brush lakeBrush = new SolidBrush(m_Palette.LakeColor);

            Pen riverPen = new Pen(m_Palette.RiverColor);

            Brush landBrush = new SolidBrush(Color.BurlyWood);
            Brush coastBrush = new SolidBrush(Color.Bisque);

            graphics.FillRectangle(whiteBrush, 0, 0, width, height);

            var maxDistFromWater = 1; // map.Polygons.Max(p => p.DistanceForMoisture);

            if (settings.DisplayPolygons)
            {
                foreach (var polygon in map.Polygons.Where(p => p.Corners.Count > 2))
                {
                    if (polygon.IsLand)
                    {
                        if (polygon.IsOceanCoast && settings.DisplayCoast)
                        {
                            graphics.FillPolygon(coastBrush, polygon.CornersToDraw.Select(p => (PointF)p).ToArray());
                            //graphics.FillPolygon(coastBrush, polygon.CornersToDraw.Select(p => (PointF)p).Reverse().ToArray());
                        }
                        else
                        {
                            if (settings.DisplayElevation)
                            {
                                var lowLandColor = new MyColor(Color.Green);
                                var highLandColor = new MyColor(Color.Red);

                                Color color = (Color)(     polygon.Elevation / maxDistFromWater  * highLandColor +
                                                      (1 - polygon.Elevation / maxDistFromWater) * lowLandColor);

                                graphics.FillPolygon(new SolidBrush(color), polygon.CornersToDraw.Select(p => (PointF)p).ToArray());
                            }
                            else
                            {
                                graphics.FillPolygon(landBrush, polygon.CornersToDraw.Select(p => (PointF)p).Reverse().ToArray());
                                //graphics.FillPolygon(landBrush, polygon.CornersToDraw.Select(p => (PointF)p).ToArray());
                            }
                        }
                    }
                    else
                    {
                        if (polygon.IsOcean)
                        {
                            graphics.FillPolygon(oceanBrush, polygon.CornersToDraw.Select(p => (PointF)p).ToArray());
                        }
                        if (polygon.IsLake)
                        {
                            graphics.FillPolygon(lakeBrush, polygon.CornersToDraw.Select(p => (PointF)p).ToArray());
                        }
                    }

                    /*graphics.FillEllipse(new SolidBrush(Color.Red), (float) polygon.Center.X, (float) polygon.Center.Y, 5, 5);

                    for (int i = 0; i < polygon.Borders.Count - 2; i++)
                    {
                        graphics.DrawLine(new Pen(Color.Green), (PointF) polygon.Borders[i].Center, (PointF) polygon.Borders[i + 1].Center);
                    }*/
                }
            }

            if (settings.DisplayLinealBorders)
            {
                foreach (var polygon in map.Polygons.Where(p => p.Corners.Count > 2))
                {
                    graphics.DrawPolygon(blackPen, polygon.Corners.Select(p => (PointF)p).ToArray());
                }
            }

            if (settings.DisplayNoiseBorders)
            {
                foreach (var polygon in map.Polygons.Where(p => p.Corners.Count > 2))
                {
                    graphics.DrawPolygon(new Pen(Color.Black), polygon.CornersToDraw.Select(p => (PointF)p).ToArray());
                }
            }

            if (settings.ApplyNoise)
            {
                var noise = this.m_NoiseGenerator.GenerateNoise(width, height, this.m_Random.Next(), Math.Max(width, height));

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb((int)(BaseAlfa + (256 - BaseAlfa) * noise[i, j]), bitmap.GetPixel(i, j)));
                    }
                }
            }

            if (settings.DisplayCoast)
            {
                foreach (var border in map.Borders)
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
                            graphics.DrawLine(pen, (PointF)border.BorderToDraw[i], (PointF)border.BorderToDraw[i + 1]);
                        }
                    }
                }
            }

            if (settings.DisplayRivers)
            {
                foreach (var border in map.Borders)
                {
                    if (border.RiverCapacity > 0 && !border.IsLake)
                    {
                        var riverPenWidth = (float)Math.Sqrt(30 * border.RiverCapacity / Math.Sqrt(map.Polygons.Count));
                        riverPen.Width = riverPenWidth;
                        graphics.DrawLines(riverPen, border.BorderToDraw.Select(p => (PointF)p).ToArray());
                    }
                }
            }

            /*foreach (var polygon in map.Polygons)
            {
                graphics.DrawString(polygon.DistanceFromEdge.ToString(CultureInfo.InvariantCulture), new Font("Thaoma", 10), Brushes.Black, (PointF)polygon.Center);
            }*/

            /*if (this.checkBoxShowTimes.Checked)
            {
                graphics.DrawString(
                    this.m_Structure.LastActionLength.ToString(CultureInfo.InvariantCulture),
                    new Font("Thaoma", 80),
                    Brushes.Black,
                    10,
                    10);

                graphics.DrawString(
                    this.m_Structure.LandCreation.ToString(CultureInfo.InvariantCulture),
                    new Font("Thaoma", 80),
                    Brushes.Black,
                    10,
                    100);

                graphics.DrawString(
                    this.m_Structure.MapBuilding.ToString(CultureInfo.InvariantCulture),
                    new Font("Thaoma", 80),
                    Brushes.Black,
                    10,
                    190);
            }*/
            /*if (checkBoxPoints.Checked)
            {
                int r = 2;
                foreach (var corner in map.Corners)
                {
                    graphics.DrawEllipse(blackPen, (float) corner.X - r, (float) corner.Y - r, 2 * r, 2 * r);
                }
            }

            if (checkBoxBorders.Checked)
            {
                foreach (var edge in map.Borders)
                {
                    graphics.DrawLine(blackPen, (float) edge.Corners[0].X, (float) edge.Corners[0].Y,
                                      (float) edge.Corners[1].X,
                                      (float) edge.Corners[1].Y);
                }
            }

            if (checkBoxTriangles.Checked)
            {
                foreach (var edge in map.Borders)
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
            return bitmap;
        }
    }
}
