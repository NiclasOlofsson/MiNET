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

using System;
using System.Collections.Generic;
using System.Numerics;
using fNbt;
using log4net;
using MiNET.Entities.Projectiles;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemFireworkRocket : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemFireworkRocket));

		public float Spread { get; set; } = 5f;

		public ItemFireworkRocket() : base("minecraft:firework_rocket", 401)
		{
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Random random = new Random();
			var rocket = new FireworksRocket(player, world, this, random);
			rocket.KnownPosition = blockCoordinates;
			rocket.KnownPosition += faceCoords + new Vector3(0, 0.01f, 0);
			rocket.KnownPosition.Yaw = random.Next(360);
			rocket.KnownPosition.Pitch = -1 * (float) (90f + (random.NextDouble() * Spread - Spread / 2));
			rocket.BroadcastMovement = true;
			rocket.DespawnOnImpact = true;
			rocket.SpawnEntity();

			if (player.GameMode == GameMode.Survival)
			{
				var itemInHand = player.Inventory.GetItemInHand();
				itemInHand.Count--;
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
			}
		}

		//TODO: Enable this when we can figure out the difference between placing a block, and use item transactions :-(
		//
		//public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		//{
		//	Random random = new Random();
		//	var rocket = new FireworksRocket(player, world, this, random);
		//	rocket.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
		//	rocket.KnownPosition.Y += 1.62f;
		//	rocket.BroadcastMovement = true;
		//	rocket.DespawnOnImpact = true;
		//	rocket.SpawnEntity();
		//}

		//TAG_Compound: 1 entries {
		//	TAG_Compound("Fireworks"): 2 entries {
		//		TAG_List("Explosions"): 1 entries {
		//			TAG_Compound: 5 entries {
		//				TAG_Byte_Array("FireworkColor"): [1 bytes]
		//				TAG_Byte_Array("FireworkFade"): [0 bytes]
		//				TAG_Byte("FireworkFlicker"): 0
		//				TAG_Byte("FireworkTrail"): 0
		//				TAG_Byte("FireworkType"): 0
		//			}
		//		}
		//		TAG_Byte("Flight"): 1
		//	}
		//}

		public static NbtCompound ToNbt(FireworksData data)
		{
			var explosions = new NbtList("Explosions", NbtTagType.Compound);
			foreach (var explosion in data.Explosions)
			{
				explosions.Add(new NbtCompound()
				{
					new NbtByteArray("FireworkColor", explosion.FireworkColor),
					new NbtByteArray("FireworkFade", explosion.FireworkFade),
					new NbtByte("FireworkFlicker", (byte) (explosion.FireworkFlicker ? 1 : 0)),
					new NbtByte("FireworkTrail", (byte) (explosion.FireworkTrail ? 1 : 0)),
					new NbtByte("FireworkType", (byte) explosion.FireworkType),
				});
			}

			NbtCompound root = new NbtCompound
			{
				new NbtCompound("Fireworks")
				{
					explosions,
					new NbtByte("Flight", (byte) data.Flight)
				}
			};

			return root;
		}

		public class FireworksData
		{
			public int Flight { get; set; } = 1;
			public List<FireworksExplosion> Explosions { get; set; } = new List<FireworksExplosion>();
		}

		public class FireworksExplosion
		{
			public byte[] FireworkColor { get; set; } = new byte[3];
			public byte[] FireworkFade { get; set; } = new byte[3];
			public bool FireworkFlicker { get; set; }
			public bool FireworkTrail { get; set; }
			public int FireworkType { get; set; }
		}
	}
}