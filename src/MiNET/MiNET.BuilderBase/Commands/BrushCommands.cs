using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;

namespace MiNET.BuilderBase.Commands
{
	public class BrushCommands
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BrushCommands));

		[Command(Name = "brush sphere", Description = "Set selection position 1")]
		public void BrushSphere(Player player, int radius, BlockTypeEnum tileName = null, int tileData = 0, bool filled = true)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null)
			{
				brush = new BrushTool();
				player.Inventory.SetFirstEmptySlot(brush, true, false);
			}

			if (tileName == null) tileName = new BlockTypeEnum {Value = "stone"};
			var id = BlockFactory.GetBlockIdByName(tileName.Value);

			var pattern = new Pattern(id, tileData);

			brush.Radius = radius;
			brush.Pattern = pattern;
			brush.Filled = filled;

			player.SendMessage($"Selected brush sphere tool");
		}

		[Command(Name = "brush mask", Description = "Set selection position 1")]
		public void BrushMask(Player player, BlockTypeEnum tileName, int tileData = 0)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			var id = BlockFactory.GetBlockIdByName(tileName.Value);

			var block = BlockFactory.GetBlockById(id);
			block.Metadata = (byte) tileData;

			brush.Mask = new BlockMask(player.Level, block);
		}
	}
}