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

using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;
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

		protected ItemBlock(short id, short metadata = 0) : base(id, metadata)
		{
		}

		public ItemBlock(Block block, short metadata = 0) : base((short) (block.Id > 255 ? 255 - block.Id : block.Id), metadata)
		{
			Block = block;
			FuelEfficiency = Block.FuelEfficiency;
		}

		public override Item GetSmelt()
		{
			return Block.GetSmelt();
		}


		public override void PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block block = world.GetBlock(targetCoordinates);
			Block.Coordinates = block.IsReplacible ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);

			Block.Metadata = (byte) Metadata;

			if (!Block.CanPlace(world, player, targetCoordinates, face))
			{
				return;
			}

			if (!Block.PlaceBlock(world, player, targetCoordinates, face, faceCoords))
			{
				world.SetBlock(Block);
			}

			if (player.GameMode == GameMode.Survival && Block.Id != 0)
			{
				var itemInHand = player.Inventory.GetItemInHand();
				itemInHand.Count--;
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
			}

			world.BroadcastSound(targetCoordinates, LevelSoundEventType.Place, block.Id);
		}

		public override string ToString()
		{
			return $"{GetType().Name}(Id={Id}, Meta={Metadata})[{Block?.GetType().Name}] Count={Count}, NBT={ExtraData}";
		}

	}
}