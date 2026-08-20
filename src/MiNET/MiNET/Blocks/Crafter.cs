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
using MiNET.Items;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	/// <summary>The nine-slot grid, held but never crafted from: redstone does not fire it, and the
	/// per-slot disabled flags the screen can set are not kept.</summary>
	public partial class Crafter : Block
	{
		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// The orientation is the output direction and the turn, in that order. The output faces the
			// player like a dispenser does, so a steep look sends it straight up or down and the turn is
			// then the heading; a level look sends it sideways and the turn is "up".
			string cardinal = ItemBlock.GetCardinalDirectionFromEntity(player);
			Orientation = player.KnownPosition.Pitch switch
			{
				> 45 => $"up_{cardinal}",
				< -45 => $"down_{cardinal}",
				_ => $"{cardinal}_up"
			};

			world.SetBlockEntity(new CrafterBlockEntity {Coordinates = Coordinates});

			return false;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			player.OpenInventory(blockCoordinates);

			return true;
		}
	}
}
