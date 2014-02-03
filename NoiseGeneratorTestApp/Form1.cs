using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MapGenerator.Utils;
using PerlinNoiseGeneration;
using Triangulation.Algorithm.GeometryBase;
using Triangulation.Algorithm.Sorting;

namespace NoiseGeneratorTestApp
{
    public partial class Form1 : Form
    {
        private readonly Random m_Rnd;

        private Bitmap m_Bitmap;
        //private readonly int m_Width = 800;
        //private readonly int m_Height = 600;
        private readonly int m_Width = 1920;
        private readonly int m_Height = 1080;
        private readonly ImageSaver _imageSaver = new ImageSaver("Image");
        private int m_Seed;

        public Form1()
        {
            InitializeComponent();
            m_Rnd = new Random(1);
        }

        private void ButtonGenerateClick(object sender, EventArgs e)
        {
            int frequency;
            if (int.TryParse(textBoxSeed.Text, out m_Seed) && int.TryParse(textBoxFrequency.Text, out frequency))
            {
                INoiseGenerator generator = new PerlinNoiseGenerator();

                if (cbNormalize.Checked)
                {
                    generator = new NormNoiseDecorator(generator);
                }

                if (cbPolarize.Checked)
                {
                    generator = new SlowedHeightIncreaseNoiseDecorator(generator);
                }

                var noise = generator.GenerateNoise(m_Width, m_Height, m_Seed, frequency);

                var values = new List<double>();
                foreach (var value in noise)
                {
                    values.Add(value);
                }
                values.Sort();

                for (int i = 0; i < values.Count - 1; i++)
                {
                    if (values[i + 1] - values[i] > 0.001)
                    {
                        
                    }
                }

                this.m_Bitmap = new Bitmap(m_Width, m_Height);
                for (int i = 0; i < m_Width; i++)
                {
                    for (int j = 0; j < m_Height; j++)
                    {
                        //var botColor = new MyColor(Color.FromArgb(255, 0, 255, 0));
                        //var botColor = new MyColor(Color.White);
                        //var topColor = new MyColor(Color.Black);
                        MyColor color;// = topColor * noise[i, j] + botColor * (1 - noise[i, j]);
                        if (noise[i, j] > 0.65)
                        {
                            color = new MyColor(Color.Wheat);
                        }
                        else
                        {
                            color = new MyColor(Color.White);
                        }
                        this.m_Bitmap.SetPixel(i, j, (Color) color);
                    }
                }
            }

            this.Refresh();
        }

        private void ButtonNextRandomClick(object sender, EventArgs e)
        {
            textBoxSeed.Text = m_Rnd.Next().ToString();
        }

        private void Form1Paint(object sender, PaintEventArgs e)
        {
            if (m_Bitmap != null)
            {
                e.Graphics.DrawImage(m_Bitmap, 10, 10);
            }
        }

        private void buttonTestSort_Click(object sender, EventArgs e)
        {
            int N = 4;

            /*while (true)
            {
                List<int> list = new List<int>(4);
                for (int i = 0; i < N; i++)
                {
                    list.Add(m_Rnd.Next(10));
                }

                list.QuickSort((a, b) => a.CompareTo(b));

                for (int i = 0; i < N - 2; i++)
                {
                    if (list[i] > list[i + 1])
                    {
                        return;
                    }
                }
            }*/

            while (true)
            {
                List<Point> p = new List<Point>(N);
                for (int i = 0; i < N; i++)
                {
                    p.Add(new Point(m_Rnd.Next(m_Width), m_Rnd.Next(m_Height)));
                }

                var center = new Point((int) p.Average(a => a.X), (int) p.Average(a => a.Y));

                //var c = new List<Point>(p);

                p.QuickSort((a, b) => Geometry.Vect(center, a, b).CompareTo(0));

                p.Add(p[0]);

                bool intersect = false;

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (Geometry.SegmentsIntersects(p[i], p[i + 1], p[j], p[j + 1]))
                        {
                            intersect = true;
                            break;
                        }
                    }

                    if (intersect)
                    {
                        break;
                    }
                }

                if (intersect)
                {
                    m_Bitmap = new Bitmap(m_Width, m_Height);
                    var g = Graphics.FromImage(m_Bitmap);

                    for (int i = 0; i < N; i++)
                    {
                        g.DrawLine(new Pen(Color.Red), center, p[i]);
                    }

                    for (int i = 0; i < N; i++)
                    {
                        g.DrawLine(new Pen(Color.Black), p[i], p[i + 1]);
                    }

                    break;
                }

                /*c.QuickSort((a, b) => Geometry.Vect(center, a, b).CompareTo(0));

            c.Add(c[0]);

            for (int i = 0; i < N; i++)
            {
                g.DrawLine(new Pen(Color.Red), center, c[i]);
            }

            for (int i = 0; i < N; i++)
            {
                g.DrawLine(new Pen(Color.Black), c[i], c[i + 1]);
            }*/
            }

            this.Refresh();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (m_Bitmap != null)
            {
                _imageSaver.SaveBitmap(m_Bitmap, m_Seed);
            }
        }
    }
}
