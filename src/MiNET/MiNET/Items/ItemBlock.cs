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
using System.Numerics;
using JetBrains.Annotations;
using log4net;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;
using Newtonsoft.Json;

namespace MiNET.Items
{
	/// <summary>
	///     Generic Item that will simply place the block on use. No interaction or other use supported by the block.
	/// </summary>
	public class ItemBlock : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemBlock));

		[JsonIgnore] public Block Block { get; protected set; }

		protected ItemBlock(string name, short id, short metadata = 0) : base(name, id, metadata)
		{
			//TODO: Problematic block
			Block = BlockFactory.GetBlockById(id);
		}

		public ItemBlock([NotNull] Block block, short metadata = 0) : base(block.Name, (short) (block.Id > 255 ? 255 - block.Id : block.Id), metadata)
		{
			Block = block ?? throw new ArgumentNullException(nameof(block));
	
			if (BlockFactory.BlockStates.TryGetValue(block.GetState(), out BlockStateContainer value))
			{
				Metadata = (short) (value.ItemInstance?.Metadata ?? (value.Data == -1 ? 0 : value.Data));
			}

			FuelEfficiency = Block.FuelEfficiency;
		}

		public override Item GetSmelt()
		{
			return Block.GetSmelt();
		}

		public static int GetFacingDirectionFromEntity(Entity entity)
		{
			return entity.GetDirectionEmum() switch
			{
				Entity.Direction.South => 4,
				Entity.Direction.West => 2,
				Entity.Direction.North => 5,
				Entity.Direction.East => 3,
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		public static BlockAxis GetPillarAxisFromFace(BlockFace face)
		{
			return face switch
			{
				BlockFace.Down => BlockAxis.Y,
				BlockFace.Up => BlockAxis.Y,
				BlockFace.North => BlockAxis.Z,
				BlockFace.South => BlockAxis.Z,
				BlockFace.West => BlockAxis.X,
				BlockFace.East => BlockAxis.X,
				_ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
			};
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block currentBlock = world.GetBlock(targetCoordinates);
			Block newBlock = BlockFactory.GetBlockById(Block.Id);
			newBlock.Coordinates = currentBlock.IsReplaceable ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);

			// This won't work without explicit mapping where an item dictates
			// the initial value of a block. Need some sort of manual mapping or from
			// generated data. The logic belong to the item.
			// Basically what we want to do here is to check all items for a blockstate
			// and find a matching one. Then use the blockstate for that item, to set the
			// default data for this item.
			newBlock.SetState(Block.GetState());

			//newBlock.Metadata = (byte) Metadata;

			if (!newBlock.CanPlace(world, player, targetCoordinates, face))
			{
				return;
			}

			if (!newBlock.PlaceBlock(world, player, targetCoordinates, face, faceCoords))
			{
				world.SetBlock(newBlock);
			}

			if (player.GameMode == GameMode.Survival && newBlock.Id != 0)
			{
				var itemInHand = player.Inventory.GetItemInHand();
				itemInHand.Count--;
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
			}

			world.BroadcastSound(newBlock.Coordinates, LevelSoundEventType.Place, newBlock.Id);
		}

		public override string ToString()
		{
			return $"{GetType().Name}(Id={Id}, Meta={Metadata}, UniqueId={UniqueId}) {{Block={Block?.GetType().Name}}} Count={Count}, NBT={ExtraData}";
		}
	}
}