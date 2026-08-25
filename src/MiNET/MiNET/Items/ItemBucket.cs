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
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	/// <summary>
	///     The empty bucket. Scoops a source block and comes back as that liquid's own bucket item;
	///     the liquids are registry identities of their own, never a metadata on this one.
	/// </summary>
	public class ItemBucket : Item
	{
		public ItemBucket() : base("minecraft:bucket")
		{
			MaxStackSize = 1;
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block block = world.GetBlock(blockCoordinates);

			// Only a source block can be scooped.
			string filled = block switch
			{
				Water {LiquidDepth: 0} or FlowingWater {LiquidDepth: 0} => "minecraft:water_bucket",
				Lava {LiquidDepth: 0} or FlowingLava {LiquidDepth: 0} => "minecraft:lava_bucket",
				_ => null
			};
			if (filled == null) return;

			world.SetAir(blockCoordinates);

			if (player.GameMode == GameMode.Survival || player.GameMode == GameMode.Adventure)
			{
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, ItemFactory.GetItemByName(filled));
			}
		}
	}

	/// <summary>A bucket holding a liquid. Pouring it places the liquid's flowing block as a source.</summary>
	public abstract class ItemLiquidBucket : Item
	{
		private readonly string _liquidName;

		protected ItemLiquidBucket(string name, string liquidName) : base(name)
		{
			_liquidName = liquidName;
			MaxStackSize = 1;
		}

		/// <summary>The flowing block this bucket pours, in its palette default state, which is the source.</summary>
		public Flowing Liquid()
		{
			return (Flowing) BlockFactory.GetBlockByName(_liquidName);
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block target = world.GetBlock(blockCoordinates);

			Flowing liquid = Liquid();
			liquid.Coordinates = target.IsReplaceable ? blockCoordinates : GetNewCoordinatesFromFace(blockCoordinates, face);
			if (!liquid.CanPlace(world, player, blockCoordinates, face)) return;

			// SetBlock runs BlockAdded, which schedules the flow tick.
			world.SetBlock(liquid);
			world.BroadcastSound(liquid.Coordinates, LevelSoundEventType.Place, liquid.GetRuntimeId());

			if (player.GameMode == GameMode.Survival || player.GameMode == GameMode.Adventure)
			{
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, ItemFactory.GetItemByName("minecraft:bucket"));
			}
		}
	}

	public class ItemWaterBucket : ItemLiquidBucket
	{
		public ItemWaterBucket() : base("minecraft:water_bucket", "minecraft:flowing_water")
		{
		}
	}

	public class ItemLavaBucket : ItemLiquidBucket
	{
		public ItemLavaBucket() : base("minecraft:lava_bucket", "minecraft:flowing_lava")
		{
			FuelEfficiency = 1000;
		}
	}
}