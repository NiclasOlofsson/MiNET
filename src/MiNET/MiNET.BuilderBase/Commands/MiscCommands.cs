using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Commands
{
	public class MiscCommands
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiscCommands));

		[Command(Description = "Fill a hole")]
		public void Fill(Player player, Pattern pattern, int radius, int depth = -1)
		{
			new EditHelper(player.Level).Fill((BlockCoordinates) player.KnownPosition, pattern, new AirBlocksMask(player.Level), radius, depth);
		}

		[Command(Description = "Fill a hole")]
		public void Drain(Player player, int radius)
		{
			new EditHelper(player.Level).Fill((BlockCoordinates) player.KnownPosition,
				new Pattern(0, 0),
				new Mask(player.Level, new List<Block> {new FlowingWater(), new StationaryWater(), new StationaryLava(), new FlowingLava()}, true),
				radius,
				-1, true);
		}

		[Command(Description = "Save world")]
		public void Save(Player player)
		{
			AnvilWorldProvider provider = player.Level._worldProvider as AnvilWorldProvider;
			if (provider != null)
			{
				provider.SaveChunks();
			}
		}

		[Command(Description = "Grass, 2 layers of dirt, then rock below")]
		public void Text(Player player, string text, string fontName = "Arial", int pxSize = 10)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			var font = new Font("SketchFlow Print", 20, GraphicsUnit.Pixel);

			SizeF size;
			// First measure the size of the text
			using (Graphics g = Graphics.FromImage(new Bitmap(1, 1)))
			{
				g.PageUnit = GraphicsUnit.Pixel;
				g.SmoothingMode = SmoothingMode.None;
				size = g.MeasureString(text, font);
			}

			Bitmap bitmap = new Bitmap((int) size.Width, (int) size.Height);
			RectangleF rectf = new RectangleF(0, 0, size.Width, size.Height);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.SmoothingMode = SmoothingMode.None;

				//g.SmoothingMode = SmoothingMode.AntiAlias;
				//g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				//g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.DrawString(text, font, Brushes.Black, rectf);
				g.Flush();

				BlockCoordinates coords = (BlockCoordinates) player.KnownPosition;

				int i = 0;
				for (int h = bitmap.Height - 1; h > 0; h--)
				{
					for (int w = 0; w < bitmap.Width; w++)
					{
						Color color = bitmap.GetPixel(w, h);
						BlockCoordinates tc = new BlockCoordinates(0, i, w);
						if (color.A == 255)
						{
							var block = new Sand {Coordinates = tc + coords};
							Log.Debug($"Pixel at {tc + coords}, {color}");
							player.Level.SetBlock(block);
						}
						else
						{
							//Log.Debug($"NO Pixel at {tc + coords}, {color}");
						}
					}
					i++;
				}
			}
		}
	}
}