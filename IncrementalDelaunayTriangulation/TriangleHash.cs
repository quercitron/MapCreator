using System;

using GeneralAlgorithms.GeometryBase;

namespace IncrementalDelaunayTriangulation
{
    public class TriangleHash
    {
        public TriangleHash(double width, double height, Triangle basicTriangle, int triangleCount = 1)
        {
            Width = width;
            Height = height;            

            m_Size = 2;
            m_Hash = new Triangle[2,2];

            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    m_Hash[i, j] = basicTriangle;
                }
            }

            TriangleCount = triangleCount;
        }

        private const int R = 4;

        private int m_Size;

        private Triangle[,] m_Hash;

        private int m_TriangleCount;

        public double Width { get; private set; }

        public double Height { get; private set; }

        public int TriangleCount
        {
            get { return m_TriangleCount; }
            set
            {
                m_TriangleCount = value;
                if (m_TriangleCount >= R * m_Size * m_Size)
                {
                    IncreaseSize();
                }
            }
        }

        private void IncreaseSize()
        {
            int newSize = 2 * m_Size;
            Triangle[,] newHash = new Triangle[newSize,newSize];

            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    for (int ii = 0; ii < 2; ii++)
                    {
                        for (int ji = 0; ji < 2; ji++)
                        {
                            newHash[2 * i + ii, 2 * j + ji] = m_Hash[i, j];
                        }
                    }
                }
            }

            m_Size = newSize;
            m_Hash = newHash;
        }


        public void AddTriangle(Point2D point, Triangle triangle)
        {
            var c = GetCoordinate(point);

            if (c != null)
            {
                m_Hash[c.X, c.Y] = triangle;
                TriangleCount++;
            }
        }

        public Triangle GetTriangle(Point2D point)
        {
            var c = GetCoordinate(point);

            if (c != null)
            {
                return m_Hash[c.X, c.Y];
            }

            throw new ApplicationException("Can't find triangle in Hash");
        }

        private PointInt GetCoordinate(Point2D point)
        {
            int x = (int) (m_Size * point.X / Width);
            int y = (int) (m_Size * point.Y / Height);

            if (x < 0 || x >= m_Size || y < 0 || y >= m_Size)
            {
                return null;
            }
            return new PointInt(x, y);
        }
    }
}
