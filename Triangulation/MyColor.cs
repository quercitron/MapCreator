using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Triangulation
{
    class MyColor
    {
        public MyColor(double a, double r, double g, double b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public MyColor(Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public double A { get; set; }
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public static MyColor operator +(MyColor a, MyColor b)
        {
            return new MyColor(a.A + b.A, a.R + b.R, a.G + b.G, a.B + b.B);
        }

        public static MyColor operator *(double k, MyColor a)
        {
            return new MyColor(a.A * k, a.R * k, a.G * k, a.B * k);
        }

        public static MyColor operator *(MyColor a, double k)
        {
            return new MyColor(a.A * k, a.R * k, a.G * k, a.B * k);
        }

        public static explicit operator Color(MyColor a)
        {
            return Color.FromArgb((int) a.A, (int) a.R, (int) a.G, (int) a.B);
        }
    }
}
