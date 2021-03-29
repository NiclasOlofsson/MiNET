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

using System.Linq;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class WallSignBase : Block
	{
		private readonly int _itemDropId;

		public WallSignBase(int id, int itemDropId) : base(id)
		{
			_itemDropId = itemDropId;
			IsTransparent = true;
			IsSolid = false;
			BlastResistance = 5;
			Hardness = 1;

			IsFlammable = true; // Only in PE!!
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return world.GetBlock(blockCoordinates).IsReplaceable;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var container = GetState();
			var direction = (BlockStateInt) container.States.First(s => s.Name == "facing_direction");
			direction.Value = (int) face;
			SetState(container);
			var signBlockEntity = new SignBlockEntity {Coordinates = Coordinates};
			world.SetBlockEntity(signBlockEntity);

			return false;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			return true;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem((short) _itemDropId)}; // Drop sign item
		}
	}

	public partial class WallSign : WallSignBase
	{
		public WallSign() : base(68, 323) { }
	}

	public partial class SpruceWallSign : WallSignBase
	{
		public SpruceWallSign() : base(437, 472) { }
	}

	public partial class BirchWallSign : WallSignBase
	{
		public BirchWallSign() : base(442, 473) { }
	}

	public partial class JungleWallSign : WallSignBase
	{
		public JungleWallSign() : base(444, 474) { }
	}

	public partial class AcaciaWallSign : WallSignBase
	{
		public AcaciaWallSign() : base(446, 475) { }
	}

	public partial class DarkoakWallSign : WallSignBase
	{
		public DarkoakWallSign() : base(448, 476) { }
	}

	public partial class CrimsonWallSign : WallSignBase
	{
		public CrimsonWallSign() : base(507, 505) { }
	}

	public partial class WarpedWallSign : WallSignBase
	{
		public WarpedWallSign() : base(508, 506) { }
	}
}