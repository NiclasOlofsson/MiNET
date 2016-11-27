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

		[Command(Name = "brush sphere", Description = "Choose the sphere brush")]
		public void BrushSphere(Player player, int radius, BlockTypeEnum tileName = null, int tileData = 0, bool filled = true)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			string op = "Set";
			if (brush == null)
			{
				brush = new BrushTool();
				player.Inventory.SetFirstEmptySlot(brush, true, false);
				op = "Added";
			}

			if (tileName == null) tileName = new BlockTypeEnum {Value = "stone"};
			var id = BlockFactory.GetBlockIdByName(tileName.Value);

			var pattern = new Pattern(id, tileData);

			brush.Radius = radius;
			brush.Pattern = pattern;
			brush.Filled = filled;

			player.SendMessage($"{op} {(filled ? "filling" : "hollow")} sphere brush with block={id}:{tileData}, using radius={radius}");
		}

		[Command(Name = "brush mask", Description = "Set the brush mask")]
		public void BrushMask(Player player, BlockTypeEnum tileName = null, int tileData = 0)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			if (tileName == null)
			{
				brush.Mask = new AllBlocksMask();
				player.SendMessage($"Removed source mask from brush");
				return;
			}

			var id = BlockFactory.GetBlockIdByName(tileName.Value);

			var block = BlockFactory.GetBlockById(id);
			block.Metadata = (byte) tileData;

			brush.Mask = new BlockMask(player.Level, block);
			player.SendMessage($"Set brush to source mask {id}:{tileData}");
		}

		[Command(Name = "brush material", Description = "Set the brush material")]
		public void BrushMaterial(Player player, BlockTypeEnum tileName, int tileData = 0)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			var id = BlockFactory.GetBlockIdByName(tileName.Value);

			var pattern = new Pattern(id, tileData);
			brush.Pattern = pattern;
			player.SendMessage($"Set brush to material {id}:{tileData}");
		}

		[Command(Name = "brush radius", Description = "Set the brush radius")]
		public void BrushRadius(Player player, int radius = 4)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			brush.Radius = radius;

			player.SendMessage($"Set brush radius to {radius}");
		}

		[Command(Name = "brush range", Description = "Set the brush range")]
		public void BrushRange(Player player, int range = 300)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			brush.Range = range;

			player.SendMessage($"Set brush range to {range}");
		}
	}
}