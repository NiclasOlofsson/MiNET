using System.Drawing;

namespace MiNET.Net
{
	public class Skin
	{
		public bool Slim { get; set; }
		public byte[] Texture { get; set; }
		public byte Alpha { get; set; }


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
	}
}