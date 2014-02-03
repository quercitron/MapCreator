using System.Drawing;
using System.IO;

namespace MapGenerator.Utils
{
    public class ImageSaver
    {
        public ImageSaver(string path)
        {
            m_Path = path;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private readonly string m_Path;

        public void SaveBitmap(Image image, int seed)
        {
            var imageName = string.Format("Map_{0}x{1}_{2}", image.Width, image.Height, seed);

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

            image.Save(Path.Combine(m_Path, imageName + ".bmp"));
        }
    }
}
