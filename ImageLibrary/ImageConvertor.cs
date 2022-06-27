using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageLibrary
{
    public static class ImageConvertor
    {
        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                //ms.Position = 0;
                //BitmapImage image = new BitmapImage();
                //image.BeginInit();
                //image.StreamSource = ms;
                //image.CacheOption = BitmapCacheOption.OnLoad;
                //image.EndInit();

                //return image;
                return new Bitmap(ms);
            }
        }

        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            byte[] byteArray;

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byteArray = ms.ToArray();
            }

            return byteArray;
        }

        public static BitmapImage ResizeImage(Bitmap image, int height, int width)
        {
            Bitmap bitmap = new Bitmap(width, height);
            BitmapImage bitmapImage = new BitmapImage();
            Graphics graphics = Graphics.FromImage((Image)bitmap);

            graphics.PixelOffsetMode = PixelOffsetMode.Half;
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.DrawImage((Image)image, 0, 0, width, height);

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}
