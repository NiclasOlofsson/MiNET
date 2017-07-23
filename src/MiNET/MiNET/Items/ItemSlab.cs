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
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSlab : ItemBlock
	{
		public ItemSlab(short id, short metadata) : base(id, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return null;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// 8 = 1000
			byte upperBit = 0x08;
			// 7 = 0111
			byte materialMask = 0x07;

			Block existingBlock = world.GetBlock(blockCoordinates);

			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			Block newBlock = world.GetBlock(coordinates);

			if (face == BlockFace.Up && faceCoords.Y == 0.5 && existingBlock.Id == Id && (existingBlock.Metadata & materialMask) == Metadata)
			{
				// Replace with double block
				SetDoubleSlab(world, blockCoordinates);
				return;
			}

			if (face == BlockFace.Down && faceCoords.Y == 0.5 && (existingBlock.Metadata & materialMask) == Metadata)
			{
				// Replace with double block
				SetDoubleSlab(world, blockCoordinates);
				return;
			}

			if (newBlock.Id != Id || (newBlock.Metadata & materialMask) != Metadata)
			{
				Block slab = BlockFactory.GetBlockById((byte) (Id));
				slab.Coordinates = coordinates;
				slab.Metadata = (byte) Metadata;
				if (face != BlockFace.Up && faceCoords.Y > 0.5 || (face == BlockFace.Down && faceCoords.Y == 0.0))
				{
					slab.Metadata |= upperBit;
				}
				world.SetBlock(slab);
				return;
			}

			// Same material in existing block, make double slab

			{
				// Create double slab, replace existing
				SetDoubleSlab(world, coordinates);
			}
		}

		private void SetDoubleSlab(Level world, BlockCoordinates coordinates)
		{
			Block slab = BlockFactory.GetBlockById((byte) (Id - 1));
			slab.Coordinates = coordinates;
			slab.Metadata = (byte) Metadata;
			world.SetBlock(slab);
		}
	}

	public class ItemStoneSlab : ItemSlab
	{
		public ItemStoneSlab(short metadata) : base(44, metadata)
		{
		}
	}

	public class ItemWoodenSlab : ItemSlab
	{
		public ItemWoodenSlab(short metadata) : base(158, metadata)
		{
		}
	}

	public class ItemStoneSlab2 : ItemSlab
	{
		public ItemStoneSlab2(short metadata) : base(182, metadata)
		{
		}
	}

}