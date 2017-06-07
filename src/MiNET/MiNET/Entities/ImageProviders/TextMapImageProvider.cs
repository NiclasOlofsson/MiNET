using System.Drawing;
using System.Drawing.Drawing2D;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Entities.ImageProviders
{
	public class TextMapImageProvider : IMapImageProvider
	{
		public string Text { get; set; }

		public TextMapImageProvider(string text = "")
		{
			Text = text;
		}

		private byte[] _mapData = null;

		public virtual byte[] GetData(MapInfo mapInfo, bool forced)
		{
			byte[] data = null;

			if (_mapData == null)
			{
				_mapData = DrawText(mapInfo, Text);
				data = _mapData;
			}
			else if (_mapData.Length != mapInfo.Col*mapInfo.Row)
			{
				_mapData = DrawText(mapInfo, Text);
				data = _mapData;
			}

			return data;
		}

		public virtual McpeClientboundMapItemData GetClientboundMapItemData(MapInfo mapInfo)
		{
			return null;
		}

		public virtual McpeWrapper GetBatch(MapInfo mapInfo, bool forced)
		{
			return null;
		}

		private static byte[] DrawText(MapInfo map, string text)
		{
			Bitmap bitmap = new Bitmap(map.Col, map.Row);
			RectangleF rectf = new RectangleF(0, 0, map.Col, map.Row);
			Graphics g = Graphics.FromImage(bitmap);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.DrawString(text, new Font("SketchFlow Print", 10), Brushes.AntiqueWhite, rectf);
			g.Flush();

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
					bytes[i++] = 0xff;
				}
			}
			return bytes;
		}
	}
}