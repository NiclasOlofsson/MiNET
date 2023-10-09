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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Linq;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Items;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class FlowerPot
	{
		public FlowerPot() : base()
		{
			IsTransparent = true;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			return base.PlaceBlock(world, player, targetCoordinates, face, faceCoords);
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			return true;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			var itemInHand = player.Inventory.GetItemInHand() as ItemBlock;
			var block = itemInHand?.Block;

			if (world.GetBlockEntity(Coordinates) is FlowerPotBlockEntity existingBlockEntity && existingBlockEntity.PlantBlock != null)
			{
				if (existingBlockEntity.PlantBlock.Id == block?.Id
					&& existingBlockEntity.PlantBlock.GetGlobalState().Data == block.GetGlobalState().Data)
				{
					return;
				}

				player.Inventory.SetFirstEmptySlot(ItemFactory.GetItem(existingBlockEntity.PlantBlock), true);

				UpdateBit = false;
				world.SetBlock(this);
			}
			else if(block != null)
			{
				UpdateBit = true;
				world.SetBlock(this);

				itemInHand.Count--;
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
			}

			world.SetBlockEntity(new FlowerPotBlockEntity()
			{
				Coordinates = Coordinates,
				PlantBlock = block
			});

			return;
		}

		public override void BreakBlock(Level world, BlockFace face, bool silent = false)
		{
			base.BreakBlock(world, face, silent);

			world.RemoveBlockEntity(Coordinates);
		}

		public override Item[] GetDrops(Level world, Item tool)
		{
			return new[] { new ItemFlowerPot() };
		}
	}
}