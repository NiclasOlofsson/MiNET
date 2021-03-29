using System;
using System.Numerics;
using System.Threading.Tasks;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Commands
{
	public class ClipboardCommands : UndoableCommand
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ClipboardCommands));

		[Command(Description = "Copy the selection to the clipboard")]
		public void Copy(Player player)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			Clipboard clipboard = new Clipboard(player.Level);
			clipboard.OriginPosition1 = selector.Position1;
			clipboard.OriginPosition2 = selector.Position2;
			clipboard.SourceMask = new AnyBlockMask();
			clipboard.Origin = (BlockCoordinates) player.KnownPosition;
			clipboard.Fill(selector.GetSelectedBlocks());
			selector.Clipboard = clipboard;
		}

		[Command(Description = "Cut the selection to the clipboard")]
		public void Cut(Player player, int leaveId = 0, int leaveData = 0)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			Clipboard clipboard = new Clipboard(player.Level);
			clipboard.OriginPosition1 = selector.Position1;
			clipboard.OriginPosition2 = selector.Position2;
			clipboard.SourceMask = new AnyBlockMask();
			clipboard.SourceFuncion = coordinates =>
			{
				var block = BlockFactory.GetBlockById((byte) leaveId);
				block.Metadata = (byte) leaveData;
				block.Coordinates = coordinates;
				EditSession.SetBlock(block);
				return true;
			};
			clipboard.Origin = (BlockCoordinates) player.KnownPosition;
			clipboard.Fill(selector.GetSelectedBlocks());
			selector.Clipboard = clipboard;

			//Task.Run(() =>
			//{
			//	SkyLightCalculations.Calculate(player.Level);
			//	player.CleanCache();
			//	player.ForcedSendChunks(() =>
			//	{
			//		player.SendMessage("Calculated skylights and resent chunks.");
			//	});
			//});

		}

		[Command(Description = "Copy the selection to the clipboard")]
		public void Paste(Player player, bool skipAir = false, bool selectAfter = true, bool pastAtOrigin = false)
		{
			try
			{
				RegionSelector selector = RegionSelector.GetSelector(player);

				Clipboard clipboard = selector.Clipboard;
				if (clipboard == null)
				{
					player.SendMessage("Nothing in clipboard");
					return;
				}

				Vector3 to = pastAtOrigin ? clipboard.Origin : (BlockCoordinates) player.KnownPosition;

				var rotateY = clipboard.Transform;

				var clipOffset = clipboard.GetMin() - clipboard.Origin;
				var realTo = to + Vector3.Transform(clipOffset, rotateY);
				var max = realTo + Vector3.Transform(clipboard.GetMax() - clipboard.GetMin(), rotateY);

				var blocks = clipboard.GetBuffer();
				foreach (Block block in blocks)
				{
					if (skipAir && block is Air) continue;

					if (rotateY != Matrix4x4.Identity)
					{
						Vector3 vec = Vector3.Transform(block.Coordinates - clipboard.Origin, rotateY);
						block.Coordinates = vec + to;
					}
					else
					{
						//Vector3 vec = (block.Coordinates - clipboard.Origin);
						Vector3 vec = new Vector3(
							block.Coordinates.X - clipboard.Origin.X + to.X,
							block.Coordinates.Y - clipboard.Origin.Y + to.Y,
							block.Coordinates.Z - clipboard.Origin.Z + to.Z);

						block.Coordinates = vec;
					}

					EditSession.SetBlock(block);
				}

				if (selectAfter)
				{
					selector.Select(realTo, max);
				}

				//Task.Run(() =>
				//{
				//	SkyLightCalculations.Calculate(player.Level);
				//	player.CleanCache();
				//	player.ForcedSendChunks(() =>
				//	{
				//		player.SendMessage("Calculated skylights and resent chunks.");
				//	});
				//});
			}
			catch (Exception e)
			{
				Log.Error("Paste", e);
			}
		}

		[Command(Description = "Copy the selection to the clipboard")]
		public void Rotate(Player player, int rotY = 0, int rotX = 0, int rotZ = 0)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			Clipboard clipboard = selector.Clipboard;
			if (clipboard == null) return;
			var rotate = clipboard.Transform;

			rotate *= Matrix4x4.CreateRotationY((float) (rotY*Math.PI/180d));
			rotate *= Matrix4x4.CreateRotationX((float) (rotX*Math.PI/180d));
			rotate *= Matrix4x4.CreateRotationZ((float) (rotZ*Math.PI/180d));

			clipboard.Transform = rotate;
		}

		[Command(Description = "Flip the contents of the clipboard")]
		public void Flip(Player player, string direction = "me")
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			Clipboard clipboard = selector.Clipboard;
			if (clipboard == null) return;
			var rotate = clipboard.Transform;

			BlockCoordinates dir;
			try
			{
				dir = EditHelper.GetDirectionVector(player, direction).Abs();
			}
			catch (Exception e)
			{
				player.SendMessage($"Invalid value for direction <{direction}>");
				return;
			}
			rotate *= Matrix4x4.CreateScale(dir*-2 + BlockCoordinates.One);

			clipboard.Transform = rotate;
		}

		[Command(Description = "Clear your clipboard")]
		public void ClearClipboard(Player player)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			selector.Clipboard = null;
		}
	}
}