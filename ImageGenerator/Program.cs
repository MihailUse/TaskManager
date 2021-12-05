using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGenerator
{
	class Program
	{
		static Random rand = new Random();

		static void Main(string[] args)
		{
			Bitmap image = GenerateImage();

			image = ResizeImage(image, 512, 512);
			image.Save("./RandomImage.png");
		}

		public static Bitmap GenerateImage(
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
					if (rand.Next(whiteFrequency).Equals(1))
					{
						int randomColor = rand.Next(countColor);
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

			return image;
		}

		public static Color[] GenerateColors(int countColor)
		{
			Color[] colors = new Color[countColor];

			for (int i = 0; i < countColor; i++)
			{
				int red = rand.Next(256);
				int green = rand.Next(256);
				int blue = rand.Next(256);

				colors[i] = Color.FromArgb(red, green, blue);
			}

			return colors;
		}

		public static Bitmap ResizeImage(Bitmap image, int height, int width)
		{
			Bitmap resultImage = new Bitmap(width, height);
			Graphics g = Graphics.FromImage((Image)resultImage);

			g.PixelOffsetMode = PixelOffsetMode.Half;
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.DrawImage((Image)image, 0, 0, width, height);

			return new Bitmap(resultImage);
		}
	}
}
