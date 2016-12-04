using System;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

namespace MiNET.BuilderBase.Commands
{
	public class RegionCommands : UndoableCommand
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (RegionCommands));

		[Command(Description = "Set all blocks within selection")]
		public void SetBlock(Player player, BlockTypeEnum tileName, int tileData = 0)
		{
			var id = BlockFactory.GetBlockIdByName(tileName.Value);
			Set(player, id, tileData);
		}

		[Command(Description = "Set all blocks within selection")]
		public void Set(Player player, int tileId, int tileData = 0)
		{
			var pattern = new Pattern(tileId, tileData);
			EditSession.SetBlocks(Selector, pattern);
		}

		[Command(Description = "Set all blocks within selection")]
		public void Set(Player player, Pattern pattern)
		{
			EditSession.SetBlocks(Selector, pattern);
		}

		[Command(Description = "Draws a line segment between cuboid selection corners")]
		public void Line(Player player, BlockTypeEnum tileName, int tileData = 0, int thickness = 1, bool shell = false)
		{
			var id = BlockFactory.GetBlockIdByName(tileName.Value);
			Line(player, id, tileData, thickness, shell);
		}

		[Command(Description = "Draws a line segment between cuboid selection corners")]
		public void Line(Player player, int tileId, int tileData = 0, int thickness = 0, bool shell = false)
		{
			var pattern = new Pattern(tileId, tileData);
			EditSession.DrawLine(Selector, pattern, Selector.Position1, Selector.Position2, thickness, !shell);
		}

		[Command(Description = "Replace all blocks in the selection with another")]
		public void Replace(Player player, Mask mask, Pattern pattern)
		{
			EditSession.ReplaceBlocks(Selector, mask, pattern);
		}

		[Command(Description = "Set the center block(s)")]
		public void Center(Player player, int tileId = 1, int tileData = 0)
		{
			var pattern = new Pattern(tileId, tileData);

			EditSession.Center(Selector, pattern);
		}

		[Command(Description = "Move the contents of the selection")]
		public void Move(Player player, int count = 1, string direction = "me")
		{
			BlockCoordinates dir;
			try
			{
				dir = EditHelper.GetDirectionVector(player, direction);
			}
			catch (Exception e)
			{
				player.SendMessage($"Invalid value for direction <{direction}>");
				return;
			}

			EditSession.Move(Selector, count, dir);
		}

		[Command(Description = "Repeat the contents of the selection")]
		public void Stack(Player player, int count = 1, string direction = "me", bool skipAir = false, bool moveSelection = false)
		{
			BlockCoordinates dir;
			try
			{
				dir = EditHelper.GetDirectionVector(player, direction);
			}
			catch (Exception e)
			{
				player.SendMessage($"Invalid value for direction <{direction}>");
				return;
			}

			EditSession.Stack(Selector, count, dir, skipAir, moveSelection);
		}

		[Command(Description = "Generates a hollow sphere")]
		public void Hsphere(Player player, int radius, BlockTypeEnum tileName = null, int tileData = 0)
		{
			Sphere(player, radius, radius, radius, tileName, tileData, false);
		}

		[Command(Description = "Generates a hollow sphere")]
		public void Hsphere(Player player, int radiusX, int radiusY = 0, int radiusZ = 0, BlockTypeEnum tileName = null, int tileData = 0)
		{
			Sphere(player, radiusX, radiusY, radiusZ, tileName, tileData, false);
		}

		[Command(Description = "Generates a filled sphere")]
		public void Sphere(Player player, int radius, BlockTypeEnum tileName = null, int tileData = 0, bool filled = true)
		{
			Sphere(player, radius, radius, radius, tileName, tileData, filled);
		}

		[Command(Description = "Generates a filled sphere")]
		public void Sphere(Player player, int radiusX, int radiusY = 0, int radiusZ = 0, BlockTypeEnum tileName = null, int tileData = 0, bool filled = true)
		{
			if (tileName == null) tileName = new BlockTypeEnum {Value = "stone"};
			if (radiusY == 0) radiusY = radiusX;
			if (radiusZ == 0) radiusZ = radiusX;

			var id = BlockFactory.GetBlockIdByName(tileName.Value);

			var pattern = new Pattern(id, tileData);

			EditSession.MakeSphere((BlockCoordinates) player.KnownPosition, pattern, radiusX, radiusY, radiusZ, filled);
		}

		[Command(Description = "Grass, 2 layers of dirt, then rock below")]
		public void Naturalize(Player player)
		{
			UndoRecorder.CheckForDuplicates = false;
			EditSession.Naturalize(player, Selector);
		}
	}
}