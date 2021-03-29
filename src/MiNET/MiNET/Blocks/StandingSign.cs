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

using System;
using System.Linq;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class StandingSignBase : Block
	{
		private readonly int _itemDropId;

		public StandingSignBase(int id, int itemDropId) : base(id)
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
			var direction = (BlockStateInt) container.States.First(s => s.Name == "ground_sign_direction");
			direction.Value = (byte) ((int) (Math.Floor((player.KnownPosition.Yaw + 180) * 16 / 360) + 0.5) & 0x0f);
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

	public partial class StandingSign : StandingSignBase
	{
		public StandingSign() : base(63, 323) { }
	}


	public partial class SpruceStandingSign : StandingSignBase
	{
		public SpruceStandingSign() : base(436, 472) { }
	}

	public partial class BirchStandingSign : StandingSignBase
	{
		public BirchStandingSign() : base(441, 473) { }
	}

	public partial class JungleStandingSign : StandingSignBase
	{
		public JungleStandingSign() : base(443, 474) { }
	}

	public partial class AcaciaStandingSign : StandingSignBase
	{
		public AcaciaStandingSign() : base(445, 475) { }
	}

	public partial class DarkoakStandingSign : StandingSignBase
	{
		public DarkoakStandingSign() : base(447, 476) { }
	}

	public partial class CrimsonStandingSign : StandingSignBase
	{
		public CrimsonStandingSign() : base(505, 753) { }
	}

	public partial class WarpedStandingSign : StandingSignBase
	{
		public WarpedStandingSign() : base(506, 754) { }
	}
}