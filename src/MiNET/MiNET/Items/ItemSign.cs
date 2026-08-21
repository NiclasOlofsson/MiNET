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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSignBase : ItemBlock
	{
		private readonly string _standingName;
		private readonly string _wallName;

		public ItemSignBase(string name, string standingName, string wallName) : base(name)
		{
			_standingName = standingName;
			_wallName = wallName;
			MaxStackSize = 1;
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (face == BlockFace.Down) // At the bottom of block
			{
				// Doesn't work, ignore if that happen. 
				return;
			}

			if (face == BlockFace.Up) // On top of block
			{
				// Standing sign
				Block = BlockFactory.GetBlockByName(_standingName);
			}
			else
			{
				// Wall sign
				Block = BlockFactory.GetBlockByName(_wallName);
			}

			base.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
		}
	}

	public class ItemOakSign : ItemSignBase
	{
		public ItemOakSign() : base("minecraft:oak_sign", "minecraft:standing_sign", "minecraft:wall_sign") { }
	}

	public class ItemAcaciaSign : ItemSignBase
	{
		public ItemAcaciaSign() : base("minecraft:acacia_sign", "minecraft:acacia_standing_sign", "minecraft:acacia_wall_sign") { }
	}

	public class ItemSpruceSign : ItemSignBase
	{
		public ItemSpruceSign() : base("minecraft:spruce_sign", "minecraft:spruce_standing_sign", "minecraft:spruce_wall_sign") { }
	}

	public class ItemBirchSign : ItemSignBase
	{
		public ItemBirchSign() : base("minecraft:birch_sign", "minecraft:birch_standing_sign", "minecraft:birch_wall_sign") { }
	}

	public class ItemJungleSign : ItemSignBase
	{
		public ItemJungleSign() : base("minecraft:jungle_sign", "minecraft:jungle_standing_sign", "minecraft:jungle_wall_sign") { }
	}

	public class ItemDarkOakSign : ItemSignBase
	{
		public ItemDarkOakSign() : base("minecraft:dark_oak_sign", "minecraft:darkoak_standing_sign", "minecraft:darkoak_wall_sign") { }
	}

	public class ItemCrimsonSign : ItemSignBase
	{
		public ItemCrimsonSign() : base("minecraft:crimson_sign", "minecraft:crimson_standing_sign", "minecraft:crimson_wall_sign") { }
	}

	public class ItemWarpedSign : ItemSignBase
	{
		public ItemWarpedSign() : base("minecraft:warped_sign", "minecraft:warped_standing_sign", "minecraft:warped_wall_sign") { }
	}
}