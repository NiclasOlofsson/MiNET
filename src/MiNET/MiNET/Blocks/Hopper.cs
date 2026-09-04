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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	/// <summary>Five slots that hold what is put in them. Nothing is pulled in from above and nothing
	/// is pushed on.</summary>
	public partial class Hopper : Block
	{
		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// The spout points into the block that was clicked, which is the opposite of the face it
			// was clicked on. A hopper cannot point up, so clicking a block's underside gives the same
			// downward spout as standing on top of one. BlockFace and facing_direction number the six
			// faces identically, so the face is the value.
			FacingDirection = face switch
			{
				BlockFace.North => (int) BlockFace.South,
				BlockFace.South => (int) BlockFace.North,
				BlockFace.West => (int) BlockFace.East,
				BlockFace.East => (int) BlockFace.West,
				_ => (int) BlockFace.Down
			};

			world.SetBlockEntity(new HopperBlockEntity {Coordinates = Coordinates});

			return false;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			player.OpenInventory(blockCoordinates);

			return true;
		}
	}
}
