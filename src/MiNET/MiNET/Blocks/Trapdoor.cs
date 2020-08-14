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
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class TrapdoorBase : Block
	{
		[StateRange(0, 3)] public virtual int Direction { get; set; } = 0;
		[StateBit] public virtual bool OpenBit { get; set; } = false;
		[StateBit] public virtual bool UpsideDownBit { get; set; } = false;

		protected TrapdoorBase(int id) : base(id)
		{
			IsTransparent = true;
			BlastResistance = 15;
			Hardness = 5;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Direction = Entity.DirectionByRotationFlat(player.KnownPosition.Yaw) switch
			{
				0 => 1, // East
				1 => 3, // South
				2 => 0, // West
				3 => 2, // North 
				_ => 0
			};

			UpsideDownBit = (faceCoords.Y > 0.5 && face != BlockFace.Up) || face == BlockFace.Down;

			return false;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			OpenBit = !OpenBit;
			world.SetBlock(this);

			return true;
		}
	}

	public partial class Trapdoor : TrapdoorBase
	{
		public Trapdoor() : base(96) { }
	}

	public partial class AcaciaTrapdoor : TrapdoorBase
	{
		public AcaciaTrapdoor() : base(400) { }
	}

	public partial class BirchTrapdoor : TrapdoorBase
	{
		public BirchTrapdoor() : base(401) { }
	}

	public partial class DarkOakTrapdoor : TrapdoorBase
	{
		public DarkOakTrapdoor() : base(402) { }
	}

	public partial class JungleTrapdoor : TrapdoorBase
	{
		public JungleTrapdoor() : base(403) { }
	}

	public partial class SpruceTrapdoor : TrapdoorBase
	{
		public SpruceTrapdoor() : base(404) { }
	}

	public partial class CrimsonTrapdoor : TrapdoorBase
	{
		public CrimsonTrapdoor() : base(501) { }
	}

	public partial class WarpedTrapdoor : TrapdoorBase
	{
		public WarpedTrapdoor() : base(502) { }
	}
}