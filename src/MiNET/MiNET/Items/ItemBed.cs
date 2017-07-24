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
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBed : Item
	{
		public ItemBed() : base(355)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();

			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

			// Base block, meta sets orientation

			Bed block = new Bed
			{
				Coordinates = coordinates
			};

			switch (direction)
			{
				case 1:
					block.Metadata = 0;
					break; // West
				case 2:
					block.Metadata = 1;
					break; // North
				case 3:
					block.Metadata = 2;
					break; // East
				case 0:
					block.Metadata = 3;
					break; // South 
			}

			if (!block.CanPlace(world, blockCoordinates, face)) return;

			BlockFace lowerFace = BlockFace.None;
			switch (block.Metadata)
			{
				case 0:
					lowerFace = (BlockFace) 3;
					break; // West
				case 1:
					lowerFace = (BlockFace) 4;
					break; // North
				case 2:
					lowerFace = (BlockFace) 2;
					break; // East
				case 3:
					lowerFace = (BlockFace) 5;
					break; // South 
			}

			Bed blockUpper = new Bed
			{
				Coordinates = GetNewCoordinatesFromFace(coordinates, lowerFace),
				Metadata = (byte) (block.Metadata | 0x08)
			};

			if (!blockUpper.CanPlace(world, blockCoordinates, face)) return;

			//TODO: Check down from both blocks, must be solids

			world.SetBlock(block);
			world.SetBlockEntity(new BedBlockEntity
			{
				Coordinates = block.Coordinates,
				Color = (byte) Metadata
			});

			world.SetBlock(blockUpper);
			world.SetBlockEntity(new BedBlockEntity
			{
				Coordinates = blockUpper.Coordinates,
				Color = (byte)Metadata
			});
		}
	}
}