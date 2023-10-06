#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Drawing.Drawing2D;
using MiNET.Net;
using MiNET.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = System.Drawing.Color;
using RectangleF = System.Drawing.RectangleF;

namespace MiNET.Entities.ImageProviders
{
	public class TextMapImageProvider : IMapImageProvider
	{
		private static FontCollection _fontCollection;
		private static Font _font = null;
		
		static TextMapImageProvider()
		{
			_fontCollection = new FontCollection();
			_fontCollection.AddSystemFonts();

			if (_fontCollection.TryGet("Arial", out var family))
			{
				_font = family.CreateFont(9);
			}
		}
		
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
			else if (_mapData.Length != mapInfo.Col * mapInfo.Row)
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
			var bitmap = new Image<Rgba32>(map.Col, map.Row);
			var rectf = new RectangleF(0, 0, map.Col, map.Row);
			/*var g = Graphics.FromImage(bitmap);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.DrawString(text, new Font("SketchFlow Print", 10), Brushes.AntiqueWhite, rectf);
			g.Flush();*/
			
			bitmap.Mutate(
				x =>
				{
					x.DrawText(text, _font, SixLabors.ImageSharp.Color.AntiqueWhite, new PointF(0, 0));
				});

			byte[] bytes = new byte[bitmap.Height * bitmap.Width * 4];

			int i = 0;
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					var color = bitmap[x, y];
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