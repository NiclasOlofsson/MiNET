using System.Drawing;
using System.Drawing.Imaging;

namespace MiNET.Net
{
	public class Skin
	{
		public bool Slim { get; set; }
		public byte[] Texture { get; set; }
		public byte Alpha { get; set; }
		public string SkinType { get; set; }


		public static byte[] GetTextureFromFile(string filename)
		{
			Bitmap bitmap = new Bitmap(filename);
			if (bitmap.Width != 64) return null;
			if (bitmap.Height != 32 && bitmap.Height != 64) return null;

			byte[] bytes = new byte[bitmap.Height*bitmap.Width*4];

			int i = 0;
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					Color color = bitmap.GetPixel(x, y);
					bytes[i++] = color.R;
					bytes[i++] = color.G;
					bytes[i++] = color.B;
					bytes[i++] = color.A;
				}
			}

			return bytes;
		}

		public static void SaveTextureToFile(string filename, byte[] bytes)
		{
			int width = 64;
			var height = bytes.Length == 64*32*4 ? 32 : 64;

			Bitmap bitmap = new Bitmap(width, height);

			int i = 0;
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					byte r = bytes[i++];
					byte g = bytes[i++];
					byte b = bytes[i++];
					byte a = bytes[i++];

					Color color = Color.FromArgb(a, r, g, b);
					bitmap.SetPixel(x, y, color);
				}
			}

			bitmap.Save(filename, ImageFormat.Png);
		}

	}
}