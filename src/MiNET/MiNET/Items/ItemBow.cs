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
using log4net;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Entities.Projectiles;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBow : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemBow));

		public ItemBow() : base("minecraft:bow")
		{
			MaxStackSize = 1;
			ItemType = ItemType.Bow;
		}

		public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
		{
			//TODO: This is now NBT
			switch (reason)
			{
				case ItemDamageReason.ItemUse:
				{
					Metadata++;
					return Metadata >= GetMaxUses();
				}
				default:
					return false;
			}
		}

		protected override int GetMaxUses()
		{
			return 385;
		}

		private long _useTime = 0;

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			_useTime = world.TickTime;
		}

		public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			float force = CalculateForce(world.TickTime - _useTime);
			if (force < 0.1f)
			{
				player.SendPlayerInventory(); // Need to reset inventory, because we don't know what the client did here
				return;
			}

			PlayerInventory inventory = player.Inventory;

			bool isInfinity = this.GetEnchantingLevel(EnchantingType.Infinity) > 0;
			bool haveArrow = player.GameMode == GameMode.Creative;
			if (!haveArrow)
			{
				// Try off-hand first
				Item item = inventory.OffHand;
				if (item is ItemArrow)
				{
					haveArrow = true;
					if (!isInfinity)
					{
						item.Count -= 1;
						item.UniqueId = Environment.TickCount;
						if (item.Count <= 0) inventory.OffHand = new ItemAir();

						player.SendPlayerInventory();
					}
				}
			}
			if (!haveArrow)
			{
				//TODO: Consume arrows properly
				//TODO: Make sure we deal with arrows based on "potions"
				for (byte i = 0; i < inventory.Slots.Count; i++)
				{
					Item itemStack = inventory.Slots[i];
					if (itemStack is ItemArrow)
					{
						haveArrow = true;
						if (isInfinity) inventory.RemoveItems("minecraft:arrow", 1);
						break;
					}
				}
			}

			if (!haveArrow) return;

			var arrow = new Arrow(player, world, 2, !(force < 1.0));
			arrow.PowerLevel = this.GetEnchantingLevel(EnchantingType.Power);
			arrow.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			arrow.KnownPosition.Y += 1.62f - 0.1f; // eye height plus the arrow definition's offset

			arrow.Shoot(arrow.KnownPosition.GetHeadDirection(), force * arrow.Power);
			arrow.BroadcastMovement = true;
			arrow.DespawnOnImpact = false;

			arrow.SpawnEntity();
			player.Inventory.DamageItemInHand(ItemDamageReason.ItemUse, player, null);
		}

		/// <summary>The draw: (s² + 2s) / 3 over s seconds held, capped at 1. Under 0.1, under three ticks, the bow does not fire.</summary>
		internal static float CalculateForce(long timeUsed)
		{
			float force = timeUsed / 20.0F;

			force = ((force * force) + (force * 2.0F)) / 3.0F;
			if (force < 0.1D)
			{
				return 0;
			}

			if (force > 1.0F)
			{
				force = 1.0F;
			}

			return force;
		}
	}
}