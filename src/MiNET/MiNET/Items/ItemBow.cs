using System;
using System.Numerics;
using log4net;
using MiNET.Entities.Projectiles;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBow : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemBow));

		public ItemBow() : base(261)
		{
			MaxStackSize = 1;
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

			Arrow arrow = new Arrow(player, world, 2, !(force < 1.0));
			arrow.PowerLevel = this.GetEnchantingLevel(EnchantingType.Power);
			arrow.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			arrow.KnownPosition.Y += 1.62f;

			arrow.Velocity = arrow.KnownPosition.GetHeadDirection() * (force * 2.0f * 1.5f);
			arrow.KnownPosition.Yaw = (float) arrow.Velocity.GetYaw();
			arrow.KnownPosition.Pitch = (float) arrow.Velocity.GetPitch();
			arrow.BroadcastMovement = true;
			arrow.DespawnOnImpact = false;
			arrow.SpawnEntity();
		}

		private float CalculateForce(long timeUsed)
		{
			float force = timeUsed/20.0F;

			force = ((force*force) + (force*2.0F))/3.0F;
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
			double f2 = Math.Sqrt(motX*motX + motY*motY + motZ*motZ);

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