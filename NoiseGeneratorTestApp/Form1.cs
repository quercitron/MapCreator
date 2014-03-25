using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using GeneralAlgorithms.GeometryBase;
using GeneralAlgorithms.RandomGenerator;
using GeneralAlgorithms.RandomGenerator.DefaultGenerator;
using GeneralAlgorithms.RandomGenerator.MersenneTwisterGenerator;
using GeneralAlgorithms.Sorting;

using MapGenerator.Utils;

using NPack;

using PerlinNoiseGeneration;

namespace NoiseGeneratorTestApp
{
    public partial class Form1 : Form
    {
        private Random m_Rnd;

        private Bitmap m_Bitmap;
        private readonly int m_Width = 800;
        private readonly int m_Height = 200;
        //private readonly int m_Width = 1920;
        //private readonly int m_Height = 1080;
        private readonly ImageSaver _imageSaver = new ImageSaver("Image");
        private int m_Seed;

        public Form1()
        {
            InitializeComponent();
            m_Rnd = new Random(1);
            Width = Math.Max(Width, m_Width + 100);
            Height = Math.Max(Height, m_Height + 100);
        }

        private static void CopmareRandomGeneratorsSpeed()
        {
            var sw = new Stopwatch();
            var maxCount = 10000000;

            sw.Start();
            var r1 = new Random();
            for (int i = 0; i < maxCount; i++)
            {
                var x = r1.Next();
            }
            sw.Stop();
            var time1 = sw.ElapsedMilliseconds;

            sw.Restart();
            var r2 = new MersenneTwister();
            for (int i = 0; i < maxCount; i++)
            {
                var x = r2.Next();
            }
            sw.Stop();
            var time2 = sw.ElapsedMilliseconds;
        }

        private async void ButtonGenerateClick(object sender, EventArgs e)
        {
            await CreateTestNoise();
        }

        private async Task CreateTestNoise()
        {
            double start;
            int count;
            if (int.TryParse(textBoxSeed.Text, out m_Seed) && double.TryParse(textBoxStart.Text, out start) && int.TryParse(textBoxCount.Text, out count))
            {
                //INoiseGenerator generator = new PerlinNoiseGeneratorOld();
                INoiseGenerator generator = new PerlinNoiseGenerator(new StandardComponentGenerator(), new MersenneTwisterRandomGeneratorFactory());

                if (cbNormalize.Checked)
                {
                    generator = new NormNoiseDecorator(generator);
                }

                if (cbPolarize.Checked)
                {
                    generator = new SlowedHeightIncreaseNoiseDecorator(generator);
                }

                var noise = await Task.Run(() => generator.GenerateNoise(m_Width, m_Height, m_Seed, start, count));

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
                        var botColor = new MyColor(Color.White);
                        var topColor = new MyColor(Color.Black);
                        MyColor color = topColor * noise[i, j] + botColor * (1 - noise[i, j]);
                        /*MyColor color;// = topColor * noise[i, j] + botColor * (1 - noise[i, j]);
                        if (noise[i, j] > 0.65)
                        {
                            color = new MyColor(Color.Wheat);
                        }
                        else
                        {
                            color = new MyColor(Color.White);
                        }*/
                        this.m_Bitmap.SetPixel(i, j, (Color)color);
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
