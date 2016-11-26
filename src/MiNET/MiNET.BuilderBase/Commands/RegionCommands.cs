using System;
using MiNET.Blocks;
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
			RegionSelector selector = RegionSelector.GetSelector(player);

			var id = BlockFactory.GetBlockIdByName(tileName.Value);
			var pattern = new Pattern(id, tileData);
			new DrawHelper(player.Level).SetBlocks(selector, pattern);
		}

		[Command(Description = "Set all blocks within selection")]
		public void Set(Player player, int tileId, int tileData = 0)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			var pattern = new Pattern(tileId, tileData);
			new DrawHelper(player.Level).SetBlocks(selector, pattern);
		}


		[Command(Description = "Draws a line segment between cuboid selection corners")]
		public void Line(Player player, BlockTypeEnum tileName, int tileData = 0, int thickness = 1)
		{
		}

		[Command(Description = "Draws a line segment between cuboid selection corners")]
		public void Line(Player player, int tileId, int tileData = 0, int thickness = 0, bool shell = false)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			var pattern = new Pattern(tileId, tileData);
			new DrawHelper(player.Level).DrawLine(selector, pattern, selector.Position1, selector.Position2, thickness, !shell);
		}

		//[Command(Description = "Replace all blocks in the selection with another")]
		//public void Replace(Player player, string toBlock)
		//{
		//	Replace(player, null, toBlock);
		//}

		[Command(Description = "Replace all blocks in the selection with another")]
		public void Replace(Player player, int tileId, int tileData = 0)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			var pattern = new Pattern(tileId, tileData);

			new DrawHelper(player.Level).ReplaceBlocks(selector, new AllBlocksMask(player.Level), pattern);
		}

		[Command(Description = "Set the center block(s)")]
		public void Center(Player player, int tileId = 1, int tileData = 0)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			var pattern = new Pattern(tileId, tileData);

			new DrawHelper(player.Level).Center(selector, pattern);
		}

		[Command(Description = "Move the contents of the selection")]
		public void Move(Player player, int count = 1, string direction = "me")
		{
			BlockCoordinates dir;
			try
			{
				dir = DrawHelper.GetDirectionVector(player, direction);
			}
			catch (Exception e)
			{
				player.SendMessage($"Invalid value for direction <{direction}>");
				return;
			}

			RegionSelector selector = RegionSelector.GetSelector(player);

			new DrawHelper(player.Level).Move(selector, count, dir);
		}

		[Command(Description = "Repeat the contents of the selection")]
		public void Stack(Player player, int count = 1, string direction = "me")
		{
			BlockCoordinates dir;
			try
			{
				dir = DrawHelper.GetDirectionVector(player, direction);
			}
			catch (Exception e)
			{
				player.SendMessage($"Invalid value for direction <{direction}>");
				return;
			}

			RegionSelector selector = RegionSelector.GetSelector(player);

			new DrawHelper(player.Level).Stack(selector, count, dir);
		}
	}
}