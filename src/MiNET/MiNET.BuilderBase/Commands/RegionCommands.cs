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

using System;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Vectors;

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
		public void Hsphere(Player player, Pattern pattern, int radius)
		{
			Sphere(player, pattern, radius, radius, radius, false);
		}

		[Command(Description = "Generates a hollow sphere")]
		public void Hsphere(Player player, Pattern pattern, int radiusX, int radiusY = 0, int radiusZ = 0)
		{
			Sphere(player, pattern, radiusX, radiusY, radiusZ, false);
		}

		[Command(Description = "Generates a filled sphere")]
		public void Sphere(Player player, Pattern pattern, int radius, bool filled = true)
		{
			Sphere(player, pattern, radius, radius, radius, filled);
		}

		[Command(Description = "Generates a filled sphere")]
		public void Sphere(Player player, Pattern pattern, int radiusX, int radiusY = 0, int radiusZ = 0, bool filled = true)
		{
			if (radiusY == 0) radiusY = radiusX;
			if (radiusZ == 0) radiusZ = radiusX;

			EditSession.MakeSphere((BlockCoordinates) player.KnownPosition, pattern, radiusX, radiusY, radiusZ, filled);
		}

		[Command(Description = "Generates a filled cylinder")]
		public void Cylinder(Player player, Pattern pattern, int radiusX, int height = 0, int radiusZ = 0, bool filled = true)
		{
			if (height == 0) height = 1;
			if (radiusZ == 0) radiusZ = radiusX;

			EditSession.MakeCylinder((BlockCoordinates) player.KnownPosition, pattern, radiusX, radiusZ, height, filled);
		}

		[Command(Description = "Grass, 2 layers of dirt, then rock below")]
		public void Naturalize(Player player)
		{
			UndoRecorder.CheckForDuplicates = false;
			EditSession.Naturalize(player, Selector);
		}
	}
}