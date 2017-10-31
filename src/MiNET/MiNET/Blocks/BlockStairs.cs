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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class BlockStairs : Block
	{
		protected BlockStairs(byte id) : base(id)
		{
			FuelEfficiency = 15;
			IsTransparent = true; // Partial - blocks light.
			IsBlockingSkylight = false; // Partial - blocks light.
		}

		// 000 001 010 011 100
		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (player == null) return false;

			byte direction = player.GetDirection();

			byte upper = (byte) ((faceCoords.Y > 0.5 && face != BlockFace.Up) || face == BlockFace.Down ? 0x04 : 0x00);

			switch (direction)
			{
				case 0:
					Metadata = (byte) (0 | upper);
					break;
				case 1:
					Metadata = (byte) (2 | upper);
					break;
				case 2:
					Metadata = (byte) (1 | upper);
					break;
				case 3:
					Metadata = (byte) (3 | upper);
					break;
			}

			world.SetBlock(this);
			return true;
		}
	}
}