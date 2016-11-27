using System;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

namespace MiNET.BuilderBase.Commands
{
	public class RegionCommands
	{
		[Command(Description = "Set all blocks within selection")]
		public void Set(Player player, BlockTypeEnum tileName, int tileData = 0)
		{
			var id = BlockFactory.GetBlockIdByName(tileName.Value);
			Set(player, id, tileData);
		}

		[Command(Description = "Set all blocks within selection")]
		public void Set(Player player, int tileId, int tileData = 0)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			var pattern = new Pattern(tileId, tileData);
			new EditHelper(player.Level).SetBlocks(selector, pattern);
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
			RegionSelector selector = RegionSelector.GetSelector(player);

			var pattern = new Pattern(tileId, tileData);
			new EditHelper(player.Level).DrawLine(selector, pattern, selector.Position1, selector.Position2, thickness, !shell);
		}

		[Command(Description = "Replace all blocks in the selection with another")]
		public void Replace(Player player, BlockTypeEnum to, int toData = 0)
		{
			var toId = BlockFactory.GetBlockIdByName(to.Value);
			Replace(player, toId, toData);
		}

		[Command(Description = "Replace all blocks in the selection with another")]
		public void Replace(Player player, BlockTypeEnum from, int fromData, BlockTypeEnum to, int toData)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			var fromId = BlockFactory.GetBlockIdByName(from.Value);
			var toId = BlockFactory.GetBlockIdByName(to.Value);
			Replace(player, fromId, fromData, toId, toData);
		}

		[Command(Description = "Replace all blocks in the selection with another")]
		public void Replace(Player player, int toId, int toData)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			var pattern = new Pattern(toId, toData);

			new EditHelper(player.Level).ReplaceBlocks(selector, new NotAirBlocksMask(player.Level), pattern);
		}

		[Command(Description = "Replace all blocks in the selection with another")]
		public void Replace(Player player, int fromId, int fromData, int toId, int toData)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			var pattern = new Pattern(toId, toData);

			new EditHelper(player.Level).ReplaceBlocks(selector, new BlockMask(player.Level, new Block((byte) fromId) {Metadata = (byte) fromData}), pattern);
		}

		[Command(Description = "Set the center block(s)")]
		public void Center(Player player, int tileId = 1, int tileData = 0)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			var pattern = new Pattern(tileId, tileData);

			new EditHelper(player.Level).Center(selector, pattern);
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

			RegionSelector selector = RegionSelector.GetSelector(player);

			new EditHelper(player.Level).Move(selector, count, dir);
		}

		[Command(Description = "Repeat the contents of the selection")]
		public void Stack(Player player, int count = 1, string direction = "me", bool skipAir = false)
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

			RegionSelector selector = RegionSelector.GetSelector(player);

			new EditHelper(player.Level).Stack(selector, count, dir, skipAir);
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

			new EditHelper(player.Level).MakeSphere((BlockCoordinates) player.KnownPosition, pattern, radiusX, radiusY, radiusZ, filled);
		}
	}
}