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
	public class MiscCommands : UndoableCommand
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiscCommands));

		[Command(Description = "Fill a hole")]
		public void Fill(Player player, Pattern pattern, int radius, int depth = -1)
		{
			EditSession.Fill((BlockCoordinates) player.KnownPosition, pattern, new AirBlocksMask(player.Level), radius, depth);
		}

		[Command(Description = "Drain lava and water")]
		public void Drain(Player player, int radius)
		{
			EditSession.Fill((BlockCoordinates) player.KnownPosition,
				new Pattern(0, 0),
				new Mask(player.Level, new List<Block> {new FlowingWater(), new Water(), new Lava(), new FlowingLava()}, true),
				radius,
				-1, true);
		}

		//[Command(Description = "Save world")]
		//public void Save(Player player)
		//{
		//	AnvilWorldProvider provider = player.Level.WorldProvider as AnvilWorldProvider;
		//	if (provider != null)
		//	{
		//		provider.SaveChunks();
		//	}
		//}

		[Command(Description = "Render block-text of any font and size.")]
		public void Text(Player player, string text, Pattern pattern, string fontName = "Minecraft", int pxSize = 20)
		{
			//var font = new Font("SketchFlow Print", 20, GraphicsUnit.Pixel);
			var font = new Font(fontName, pxSize, GraphicsUnit.Pixel);

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
				g.DrawString(text, font, Brushes.Black, rectf);
				g.Flush();

				BlockCoordinates coords = player.KnownPosition.ToVector3();

				for (int h = 0; h < bitmap.Height; h++)
				{
					for (int w = 0; w < bitmap.Width; w++)
					{
						Color color = bitmap.GetPixel(w, h);
						var y = bitmap.Height - 1 - h;
						BlockCoordinates tc = new BlockCoordinates(0, y, w);
						if (color.A == 255)
						{
							EditSession.SetBlock(tc + coords, pattern);
						}
						//else
						//{
						//	var block = new DiamondBlock {Coordinates = tc + coords};
						//	EditSession.SetBlock(block);
						//}
					}
				}
			}
		}

		[Command(Description = "Set speed")]
		public void Speed(Player player, int speed = 1)
		{
			player.MovementSpeed = speed/10f;
			player.SendUpdateAttributes();
		}
	}
}