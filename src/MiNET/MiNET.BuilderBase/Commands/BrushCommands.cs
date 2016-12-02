using log4net;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Plugins.Attributes;

namespace MiNET.BuilderBase.Commands
{
	public class BrushCommands
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BrushCommands));

		//[Command(Description = "Only for command completion, does nothing")]
		//public void Brush(Player player)
		//{
		//	// Intentionally left empty
		//}

		[Command(Name = "brush sphere", Aliases = new[] {"br s", "sphere", "s"}, Description = "Choose the sphere brush")]
		public void BrushSphere(Player player, Pattern pattern, int radius = 2, bool hollow = false)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			string op = "Set";
			if (brush == null)
			{
				brush = new BrushTool();
				player.Inventory.SetFirstEmptySlot(brush, true, false);
				op = "Added";
			}

			brush.BrushType = 0; // Sphere
			brush.Radius = radius;
			brush.Pattern = pattern;
			brush.Filled = !hollow;

			player.SendMessage($"{op} {(!hollow ? "filling" : "hollow")} sphere brush with radius={radius}");
		}

		[Command(Name = "brush cylinder", Aliases = new[] {"br c", "cylinder", "cyl", "c"}, Description = "Choose the sphere brush")]
		public void BrushCylinder(Player player, Pattern pattern, int radius = 2, int height = 1, bool hollow = false)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			string op = "Set";
			if (brush == null)
			{
				brush = new BrushTool();
				player.Inventory.SetFirstEmptySlot(brush, true, false);
				op = "Added";
			}

			brush.BrushType = 1; // Cylinder
			brush.Radius = radius;
			brush.Height = height;
			brush.Pattern = pattern;
			brush.Filled = !hollow;

			player.SendMessage($"{op} {(!hollow ? "filling" : "hollow")} sphere cylinder with radius={radius}");
		}

		[Command(Name = "brush fill", Aliases = new[] {"br f"}, Description = "Choose the fill brush")]
		public void BrushFill(Player player, int radius = 2)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			string op = "Set";
			if (brush == null)
			{
				brush = new BrushTool();
				player.Inventory.SetFirstEmptySlot(brush, true, false);
				op = "Added";
			}

			brush.BrushType = 3; // Fill
			brush.Radius = radius;

			player.SendMessage($"{op} fill brush with radius={radius}");
		}

		[Command(Name = "brush melt", Aliases = new[] {"br m", "melt", "m"}, Description = "Choose the melt brush")]
		public void BrushMelt(Player player, int radius = 2)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			string op = "Set";
			if (brush == null)
			{
				brush = new BrushTool();
				player.Inventory.SetFirstEmptySlot(brush, true, false);
				op = "Added";
			}

			brush.BrushType = 2; // Melt
			brush.Radius = radius;

			player.SendMessage($"{op} melt brush with radius={radius}");
		}

		[Command(Name = "brush mask", Description = "Set the brush mask")]
		public void BrushMask(Player player, Mask mask = null)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			if (mask == null)
			{
				brush.Mask = new AnyBlockMask();
				player.SendMessage($"Removed source mask from brush");
				return;
			}

			brush.Mask = mask;
			player.SendMessage($"Set new brush source mask");
		}

		[Command(Name = "brush material", Description = "Set the brush material")]
		public void BrushMaterial(Player player, Pattern pattern)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			brush.Pattern = pattern;
			player.SendMessage($"Set brush material");
		}

		[Command(Name = "brush Size", Description = "Set the brush size")]
		public void BrushSize(Player player, int radius = 2)
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