using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PerlinNoiseGeneration;

namespace NoiseGeneratorTestApp
{
    public partial class Form1 : Form
    {
        private readonly PerlinNoiseGenerator m_Generator;
        private readonly Random m_Rnd;

        private Bitmap m_Bitmap;

        public Form1()
        {
            InitializeComponent();
            m_Generator = new PerlinNoiseGenerator();
            m_Rnd = new Random();
        }

        private void ButtonGenerateClick(object sender, EventArgs e)
        {
            int seed;
            int frequency;
            if (int.TryParse(textBoxSeed.Text, out seed) && int.TryParse(textBoxFrequency.Text, out frequency))
            {
                var width = 800;
                var height = 600;
                var noise = m_Generator.GenerateNoise(width, height, seed, frequency);
                this.m_Bitmap = new Bitmap(width, height);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
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
    }
}
