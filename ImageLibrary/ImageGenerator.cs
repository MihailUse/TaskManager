using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ImageLibrary
{
	public class ImageGenerator
	{
		static Random rand = new Random();

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

		// generage array with random colors 
		private static Color[] GenerateColors(int countColor)
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
	}
}
