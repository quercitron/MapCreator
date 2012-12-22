using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PerlinNoiseGeneration;
using Triangulation.Algorithm.GeometryBase;
using Triangulation.Algorithm.Sorting;

namespace NoiseGeneratorTestApp
{
    public partial class Form1 : Form
    {
        private readonly PerlinNoiseGenerator m_Generator;
        private readonly Random m_Rnd;

        private Bitmap m_Bitmap;
        private readonly int m_Width = 800;
        private readonly int m_Height = 600;

        public Form1()
        {
            InitializeComponent();
            m_Generator = new PerlinNoiseGenerator();
            m_Rnd = new Random(1);
        }

        private void ButtonGenerateClick(object sender, EventArgs e)
        {
            int seed;
            int frequency;
            if (int.TryParse(textBoxSeed.Text, out seed) && int.TryParse(textBoxFrequency.Text, out frequency))
            {
                var noise = m_Generator.GenerateNoise(m_Width, m_Height, seed, frequency);
                this.m_Bitmap = new Bitmap(m_Width, m_Height);
                for (int i = 0; i < m_Width; i++)
                {
                    for (int j = 0; j < m_Height; j++)
                    {
                        this.m_Bitmap.SetPixel(i, j, Color.FromArgb(255, (int)(255 * noise[i,j]), (int)(255 * (1 - noise[i,j])), 0));
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
    }
}
