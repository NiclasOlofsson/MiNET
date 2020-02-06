#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using log4net;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.BuilderBase.Tools;
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
		public void BrushSphere(Player player, Pattern material, int radius = 2, bool hollow = false)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			string op = "Set";
			if (brush == null)
			{
				brush = new BrushTool();
				brush.Mask = new AirBlocksMask(player.Level);
				player.Inventory.AddItem(brush, true);
				op = "Added";
			}

			brush.BrushType = 0; // Sphere
			brush.Radius = radius;
			brush.Pattern = material;
			brush.Filled = !hollow;
			brush.UpdateDisplay(player);

			player.SendMessage($"{op} {(!hollow ? "filling" : "hollow")} sphere brush with radius={radius}");
		}

		[Command(Name = "brush cylinder", Aliases = new[] {"br c", "cylinder", "cyl", "c"}, Description = "Choose the sphere brush")]
		public void BrushCylinder(Player player, Pattern material, int radius = 2, int height = 1, bool hollow = false)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			string op = "Set";
			if (brush == null)
			{
				brush = new BrushTool();
				brush.Mask = new AirBlocksMask(player.Level);
				player.Inventory.AddItem(brush, true);
				op = "Added";
			}

			brush.BrushType = 1; // Cylinder
			brush.Radius = radius;
			brush.Height = height;
			brush.Pattern = material;
			brush.Filled = !hollow;
			brush.UpdateDisplay(player);

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
				brush.Mask = new AirBlocksMask(player.Level);
				player.Inventory.AddItem(brush, true);
				op = "Added";
			}

			brush.BrushType = 3; // Fill
			brush.Radius = radius;
			brush.UpdateDisplay(player);

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
				player.Inventory.AddItem(brush, true);
				op = "Added";
			}

			brush.BrushType = 2; // Melt
			brush.Radius = radius;
			brush.UpdateDisplay(player);

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
				brush.UpdateDisplay(player);

				player.SendMessage($"Removed source mask from brush");
				return;
			}

			brush.Mask = mask;
			brush.UpdateDisplay(player);

			player.SendMessage($"Set new brush source mask");
		}

		[Command(Name = "brush material", Description = "Set the brush material")]
		public void BrushMaterial(Player player, Pattern material)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			brush.Pattern = material;
			brush.UpdateDisplay(player);

			player.SendMessage($"Set brush material");
		}

		[Command(Name = "brush Size", Description = "Set the brush size")]
		public void BrushSize(Player player, int radius = 2)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			brush.Radius = radius;
			brush.UpdateDisplay(player);

			player.SendMessage($"Set brush radius to {radius}");
		}

		[Command(Name = "brush range", Description = "Set the brush range")]
		public void BrushRange(Player player, int range = 300)
		{
			BrushTool brush = player.Inventory.GetItemInHand() as BrushTool;
			if (brush == null) return;

			brush.Range = range;
			brush.UpdateDisplay(player);

			player.SendMessage($"Set brush range to {range}");
		}
	}
}