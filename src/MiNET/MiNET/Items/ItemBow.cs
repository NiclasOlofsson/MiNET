using System;
using log4net;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBow : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemBow));

		public ItemBow(short metadata) : base(261, metadata)
		{
			MaxStackSize = 1;
		}

		public override void Release(Level world, Player player, BlockCoordinates blockCoordinates, long timeUsed)
		{
			var inventory = player.Inventory;
			bool haveArrows = false;
			for (byte i = 0; i < inventory.Slots.Count; i++)
			{
				var itemStack = inventory.Slots[i];
				if (itemStack.Id == 262)
				{
					if (--itemStack.Count <= 0)
					{
						// set empty
						Log.Debug($"Send arrows on slot {i} to 0");
						inventory.Slots[i] = new ItemAir();
					}
					haveArrows = true;
					break;
				}
			}

			if (!haveArrows) return;

			float force = CalculateForce(timeUsed);
			if (force < 0.1D) return;

			Arrow arrow = new Arrow(player, world, !(force < 1.0));
			arrow.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			arrow.KnownPosition.Y += 1.62f;

			arrow.Velocity = arrow.KnownPosition.GetDirection()*(force*2.0f*1.5f);
			arrow.KnownPosition.Yaw = (float) arrow.Velocity.GetYaw();
			arrow.KnownPosition.Pitch = (float) arrow.Velocity.GetPitch();
			arrow.BroadcastMovement = false;
			arrow.DespawnOnImpact = true;
			arrow.SpawnEntity();
		}

		private float CalculateForce(long timeUsed)
		{
			long dt = timeUsed/50;
			float force = dt/20.0F;

			force = (force*force + force*2.0F)/3.0F;
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
			return new Vector3(motX, motY, motZ);
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