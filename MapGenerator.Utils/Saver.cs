using System.Drawing;
using System.IO;

namespace MapGenerator.Utils
{
    public class Saver
    {
        public Saver(string path)
        {
            m_Path = path;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private readonly string m_Path;

        public void SaveBitmap(Bitmap bitmap, int seed)
        {
            var imageName = string.Format("Map_{0}x{1}_{2}", bitmap.Width, bitmap.Height, seed);

            if (File.Exists(Path.Combine(m_Path, imageName + ".bmp")))
            {
                for (int i = 2; ; i++)
                {
                    var newImageName = string.Format("{0}_{1}", imageName, i);
                    if (!File.Exists(Path.Combine(m_Path, newImageName + ".bmp")))
                    {
                        imageName = newImageName;
                        break;
                    }
                }
            }

            bitmap.Save(Path.Combine(m_Path, imageName + ".bmp"));
        }
    }
}
