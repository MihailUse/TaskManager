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
		static void Main(string[] args)
		{
			Bitmap image = Generator.GenerateImage();

			image = Generator.ResizeImage(image, 512, 512);
			image.Save("./RandomImage.png");
		}
	}
}
