using System.Collections.Generic;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Plugins.Attributes;
using MiNET.Utils.Vectors;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
			var font = SixLabors.Fonts.SystemFonts.CreateFont(fontName, 100); //new Font(fontName, pxSize, GraphicsUnit.Pixel);
			var size = TextMeasurer.Measure(text, new TextOptions(font));

			float scalingFactor = pxSize / size.Height;
			font = new Font(font, scalingFactor * font.Size);
			size = TextMeasurer.Measure(text, new TextOptions(font));
			
			var bitmap = new Image<Rgba32>((int) size.Width, (int) size.Height);
			RectangleF rectf = new RectangleF(0, 0, size.Width, size.Height);
			//using (Graphics g = Graphics.FromImage(bitmap))
			{
				bitmap.Mutate(
					x =>
					{
						x.DrawText(text, font, SixLabors.ImageSharp.Color.Black, PointF.Empty);
					});

				BlockCoordinates coords = player.KnownPosition.ToVector3();

				for (int h = 0; h < bitmap.Height; h++)
				{
					for (int w = 0; w < bitmap.Width; w++)
					{
						var color = bitmap[w,h];//.GetPixel(w, h);
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