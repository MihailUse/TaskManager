using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageLibrary
{
    public class ImageGenerator
    {
        private static readonly Random _rand = new Random();

        public static byte[] GenerateImage(
            int pixelsInWidth = 8,
            int pixelsInHeight = 8,
            int countColor = 3,
            int whiteFrequency = 2
        )
        {
            int halfPixelsInWidth = pixelsInWidth / 2;
            Bitmap image = new Bitmap(pixelsInWidth, pixelsInHeight);
            Color[] colors = GenerateColors(countColor);

            for (int y = 0; y < pixelsInHeight; y++)
            {
                for (int x = 0; x < halfPixelsInWidth; x++)
                {
                    if (_rand.Next(whiteFrequency).Equals(1))
                    {
                        int randomColor = _rand.Next(countColor);
                        image.SetPixel(x, y, colors[randomColor]);
                        image.SetPixel((pixelsInWidth - 1) - x, y, colors[randomColor]); // width-1 because SetPixel range [0, width-1]
                    }
                    else
                    {
                        image.SetPixel(x, y, Color.White);
                        image.SetPixel((pixelsInWidth - 1) - x, y, Color.White); // width-1 because SetPixel range [0, width-1]
                    }
                }
            }

            return BitmapToBytes(image);
        }

        // generage array with random colors 
        private static Color[] GenerateColors(int countColor)
        {
            Color[] colors = new Color[countColor];

            for (int i = 0; i < countColor; i++)
            {
                int red = _rand.Next(256);
                int green = _rand.Next(256);
                int blue = _rand.Next(256);

                colors[i] = Color.FromArgb(red, green, blue);
            }

            return colors;
        }

        private static byte[] BitmapToBytes(Bitmap bitmap)
        {
            byte[] byteArray;

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byteArray = ms.ToArray();
            }

            return byteArray;
        }
    }
}
