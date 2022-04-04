using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageLibrary
{
    public static class ImageConvertor
    {
		public static BitmapImage BytesToImage(byte[] bytes)
		{
			using (MemoryStream ms = new MemoryStream(bytes))
			{
				ms.Position = 0;
				BitmapImage image = new BitmapImage();
				image.BeginInit();
				image.StreamSource = ms;
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.EndInit();

				return image;
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

		public static Bitmap ResizeImage(Bitmap image, int height, int width)
		{
			Bitmap resultImage = new Bitmap(width, height);
			Graphics graphics = Graphics.FromImage((Image)resultImage);

			graphics.PixelOffsetMode = PixelOffsetMode.Half;
			graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			graphics.DrawImage((Image)image, 0, 0, width, height);
			
			return resultImage;
		}
	}
}
