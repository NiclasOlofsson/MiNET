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
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Entities.Projectiles;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBow : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemBow));

		public ItemBow() : base(261)
		{
			MaxStackSize = 1;
		}

		public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
		{
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

		public override void Release(Level world, Player player, BlockCoordinates blockCoordinates, long timeUsed)
		{
			var inventory = player.Inventory;
			bool haveArrows = player.GameMode == GameMode.Creative;
			haveArrows = haveArrows || this.GetEnchantingLevel(EnchantingType.Infinity) > 0;
			if (!haveArrows)
			{
				for (byte i = 0; i < inventory.Slots.Count; i++)
				{
					var itemStack = inventory.Slots[i];
					if (itemStack.Id == 262)
					{
						if (--itemStack.Count <= 0)
						{
							// set empty
							inventory.Slots[i] = new ItemAir();
						}
						haveArrows = true;
						break;
					}
				}
			}
			if (!haveArrows) return;
			if (timeUsed < 6) return; // questionable, but we go with it for now.

			float force = CalculateForce(timeUsed);
			if (force < 0.1D) return;

			var arrow = new Arrow(player, world, 2, !(force < 1.0));
			arrow.PowerLevel = this.GetEnchantingLevel(EnchantingType.Power);
			arrow.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			arrow.KnownPosition.Y += 1.62f;

			arrow.Velocity = arrow.KnownPosition.GetHeadDirection().Normalize() * (force * 3);
			arrow.KnownPosition.Yaw = (float) arrow.Velocity.GetYaw();
			arrow.KnownPosition.Pitch = (float) arrow.Velocity.GetPitch();
			arrow.BroadcastMovement = true;
			arrow.DespawnOnImpact = false;

			arrow.SpawnEntity();
			player.Inventory.DamageItemInHand(ItemDamageReason.ItemUse, player, null);
		}

		private float CalculateForce(long timeUsed)
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

		public Vector3 GetShootVector(double motX, double motY, double motZ, double f, double f1)
		{
			double f2 = Math.Sqrt(motX * motX + motY * motY + motZ * motZ);

			motX /= f2;
			motY /= f2;
			motZ /= f2;
			//motX += this.random.nextGaussian() * (double)(this.random.nextBoolean() ? -1 : 1) * 0.007499999832361937D * (double)f1;
			//motY += this.random.nextGaussian() * (double)(this.random.nextBoolean() ? -1 : 1) * 0.007499999832361937D * (double)f1;
			//motZ += this.random.nextGaussian() * (double)(this.random.nextBoolean() ? -1 : 1) * 0.007499999832361937D * (double)f1;
			motX *= f;
			motY *= f;
			motZ *= f;
			return new Vector3((float) motX, (float) motY, (float) motZ);
			//thismotX = motX;
			//thismotY = motY;
			//thismotZ = motZ;
			//double f3 = Math.Sqrt(motX * motX + motZ * motZ);

			//thislastYaw = this.yaw = (float)(Math.atan2(motX, motZ) * 180.0D / 3.1415927410125732D);
			//this.lastPitch = this.pitch = (float)(Math.atan2(motY, (double)f3) * 180.0D / 3.1415927410125732D);
			//this.ttl = 0;
		}
	}
}
