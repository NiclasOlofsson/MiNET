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
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract partial class StandingSignBase : Block
	{
		public StandingSignBase() : base()
		{
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
			if (player != null)
			{
				McpeOpenSign openSign = McpeOpenSign.CreateObject();
				openSign.coordinates = Coordinates;
				openSign.front = true;
				player.SendPacket(openSign);
			}
			return false;
		}


		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			return true;
		}
	}

	public partial class StandingSign : StandingSignBase
	{
		public StandingSign() : base() { }
	}


	public partial class SpruceStandingSign : StandingSignBase
	{
		public SpruceStandingSign() : base() { }
	}

	public partial class BirchStandingSign : StandingSignBase
	{
		public BirchStandingSign() : base() { }
	}

	public partial class JungleStandingSign : StandingSignBase
	{
		public JungleStandingSign() : base() { }
	}

	public partial class AcaciaStandingSign : StandingSignBase
	{
		public AcaciaStandingSign() : base() { }
	}

	public partial class DarkoakStandingSign : StandingSignBase
	{
		public DarkoakStandingSign() : base() { }
	}

	public partial class CrimsonStandingSign : StandingSignBase
	{
		public CrimsonStandingSign() : base() { }
	}

	public partial class WarpedStandingSign : StandingSignBase
	{
		public WarpedStandingSign() : base() { }
	}
}