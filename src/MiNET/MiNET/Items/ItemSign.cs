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
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSignBase : ItemBlock
	{
		private readonly int _standingId;
		private readonly int _wallId;

		public ItemSignBase(short id, int standingId, int wallId) : base(id)
		{
			_standingId = standingId;
			_wallId = wallId;
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
				Block = BlockFactory.GetBlockById(_standingId);
			}
			else
			{
				// Wall sign
				Block = BlockFactory.GetBlockById(_wallId);
			}

			base.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
		}
	}

	public class ItemSign : ItemSignBase
	{
		public ItemSign() : base(323, 63, 68) { }
	}

	public class ItemAcaciaSign : ItemSignBase
	{
		public ItemAcaciaSign() : base(475, 445, 456) { }
	}

	public class ItemSpruceSign : ItemSignBase
	{
		public ItemSpruceSign() : base(472, 436, 437) { }
	}

	public class ItemBirchSign : ItemSignBase
	{
		public ItemBirchSign() : base(473, 441, 442) { }
	}

	public class ItemJungleSign : ItemSignBase
	{
		public ItemJungleSign() : base(474, 443, 444) { }
	}

	public class ItemDarkoakSign : ItemSignBase
	{
		public ItemDarkoakSign() : base(476, 447, 448) { }
	}

	public class ItemWarpedSign : ItemSignBase
	{
		public ItemWarpedSign() : base(754, 506, 508) { }
	}

	public class ItemCrimsonSign : ItemSignBase
	{
		public ItemCrimsonSign() : base(753, 505, 507) { }
	}
}