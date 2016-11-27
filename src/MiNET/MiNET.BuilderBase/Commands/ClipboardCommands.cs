using System;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

namespace MiNET.BuilderBase.Commands
{
	public class ClipboardCommands
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ClipboardCommands));

		[Command(Description = "Copy the selection to the clipboard")]
		public void Copy(Player player)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			Clipboard clipboard = new Clipboard(player.Level);
			clipboard.OriginPosition1 = selector.Position1;
			clipboard.OriginPosition2 = selector.Position2;
			clipboard.SourceMask = new AllBlocksMask();
			clipboard.Origin = (BlockCoordinates) player.KnownPosition;
			clipboard.Fill(selector.GetSelectedBlocks());
			selector.Clipboard = clipboard;

			player.Level.SetBlock(new GoldBlock {Coordinates = (BlockCoordinates) (player.KnownPosition + BlockCoordinates.Down)});
		}

		[Command(Description = "Cut the selection to the clipboard")]
		public void Cut(Player player, int leaveId = 0, int leaveData = 0)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			HistoryEntry history = selector.CreateSnapshot();

			Clipboard clipboard = new Clipboard(player.Level);
			clipboard.OriginPosition1 = selector.Position1;
			clipboard.OriginPosition2 = selector.Position2;
			clipboard.SourceMask = new AllBlocksMask();
			clipboard.SourceFuncion = coordinates =>
			{
				var block = BlockFactory.GetBlockById((byte) leaveId);
				block.Metadata = (byte) leaveData;
				block.Coordinates = coordinates;
				player.Level.SetBlock(block);
				return true;
			};
			clipboard.Origin = (BlockCoordinates) player.KnownPosition;
			clipboard.Fill(selector.GetSelectedBlocks());
			selector.Clipboard = clipboard;

			history.Snapshot(false);
		}

		[Command(Description = "Copy the selection to the clipboard")]
		public void Paste(Player player, bool skipAir = false, bool selectAfter = true, bool pastAtOrigin = false)
		{
			try
			{
				RegionSelector selector = RegionSelector.GetSelector(player);

				Clipboard clipboard = selector.Clipboard;
				if (clipboard == null) return;

				Vector3 to = pastAtOrigin ? clipboard.Origin : (BlockCoordinates) player.KnownPosition;

				var rotateY = clipboard.Transform;

				var clipOffset = clipboard.GetMin() - clipboard.Origin;
				var realTo = to + Vector3.Transform(clipOffset, rotateY);
				var max = realTo + Vector3.Transform(clipboard.GetMax() - clipboard.GetMin(), rotateY);

				HistoryEntry history = selector.CreateSnapshot(realTo, max);

				var blocks = clipboard.GetBuffer();
				foreach (Block block in blocks)
				{
					if (skipAir && block is Air) continue;

					Vector3 vec = Vector3.Transform(block.Coordinates - clipboard.Origin, rotateY);

					block.Coordinates = vec + to;
					player.Level.SetBlock(block);
				}

				if (selectAfter)
				{
					selector.Select(realTo, max);
				}

				history.Snapshot(false);
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