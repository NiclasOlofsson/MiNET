using System;
using System.Numerics;
using log4net;
using MiNET;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin.Pets
{
	public class Pet : Mob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Pet));

		public Player Owner { get; set; }
		public Player AttackTarget { get; set; }
		public int Age { get; set; }
		public bool IsInRage { get; set; }
		public int RageTick { get; set; }

		public Pet(Player owner, Level level, int id = 12) : base(id, level)
		{
			IsBaby = true;
			IsInRage = false;
			Owner = owner;
			HealthManager = new PetHealthManager(this);
			Age = 0;
		}

		public override void OnTick()
		{
			if (Age++ < 100) return;
			if (!IsSpawned) return;
			if (HealthManager.IsDead) return;

			if (Owner == null || !Owner.IsConnected)
			{
				HealthManager.Kill();
				return;
			}

			base.OnTick();

			if (RageTick > 0)
			{
				RageTick--;
			}
			else
			{
				// Get angry randomly
				if (Level.Random.Next(10000) == 0)
				{
					AttackTarget = Owner;
					RageTick = 20*3;
					return;
				}
				else
				{
					AttackTarget = null;
				}
			}

			if (Age == 20*300) // World-ticks damn it
			{
				IsBaby = false;
				BroadcastSetEntityData();
			}

			//if (Age == 20*50)
			//{
			//	// Too old, die
			//	HealthManager.Kill();
			//	return;
			//}

			if (Owner.HealthManager.LastDamageSource != null && AttackTarget == null)
			{
				if (Owner.HealthManager.LastDamageSource is Player)
				{
					if (Owner.HealthManager.Health == 100)
					{
						AttackTarget = (Player) Owner.HealthManager.LastDamageSource;
						RageTick = 20*3;
					}
				}
			}

			try
			{
				if (Age > 20 && Math.Abs(Velocity.Length()) < 0.001)
				{
					Player target = AttackTarget ?? Owner;
					{
						// Look at the player, always
						Player closestPlayer = target;

						var dx = closestPlayer.KnownPosition.X - KnownPosition.X;
						var dz = closestPlayer.KnownPosition.Z - KnownPosition.Z;

						double tanOutput = 90 - RadianToDegree(Math.Atan(dx/(dz)));
						double thetaOffset = 270d;
						if (dz < 0)
						{
							thetaOffset = 90;
						}
						var yaw = thetaOffset + tanOutput;

						double bDiff = Math.Sqrt((dx*dx) + (dz*dz));
						var dy = (KnownPosition.Y + Height) - (closestPlayer.KnownPosition.Y + 1.62);
						double pitch = RadianToDegree(Math.Atan(dy/(bDiff)));

						KnownPosition.Yaw = (float) yaw;
						KnownPosition.HeadYaw = (float) yaw;
						KnownPosition.Pitch = (float) pitch;
						LastUpdatedTime = DateTime.UtcNow;
					}

					if (RageTick > 0)
					{
						if (!HealthManager.IsOnFire) HealthManager.Ignite();

						// 40s for testing..
						// GET VERY AGRESSIVE

						double dx = target.KnownPosition.X - KnownPosition.X;

						Random rand = new Random();
						double dz;
						for (dz = target.KnownPosition.Z - KnownPosition.Z; dx*dx + dz*dz < 0.00010; dz = (rand.NextDouble() - rand.NextDouble())*0.01D)
						{
							dx = (rand.NextDouble() - rand.NextDouble())*0.01D;
						}

						double knockbackForce = Math.Sqrt(dx*dx + dz*dz);
						float knockbackMultiplier = 0.4F;

						double motX = 0;
						motX += dx/knockbackForce*knockbackMultiplier;
						double motY = knockbackMultiplier;
						double motZ = 0;
						motZ += dz/knockbackForce*knockbackMultiplier;
						if (motY > 1)
						{
							motY = 1;
						}

						Knockback(new Vector3((float) motX, (float) motY, (float) motZ));
						target.HealthManager.TakeHit(this, IsBaby ? 1 : 10, DamageCause.EntityAttack);
					}
					else if ((DateTime.UtcNow.Ticks - Owner.LastUpdatedTime.Ticks > 0.6f*TimeSpan.TicksPerSecond && Owner.KnownPosition.DistanceTo(KnownPosition) > 3.5f)
					         || Owner.KnownPosition.DistanceTo(KnownPosition) > 10)
					{
						// SO, the pet got lonely, do jumps until it reaches the player

						double dx = Owner.KnownPosition.X - KnownPosition.X;

						Random rand = new Random();
						double dz;
						for (dz = Owner.KnownPosition.Z - KnownPosition.Z; dx*dx + dz*dz < 0.00010; dz = (rand.NextDouble() - rand.NextDouble())*0.01D)
						{
							dx = (rand.NextDouble() - rand.NextDouble())*0.01D;
						}

						double knockbackForce = Math.Sqrt(dx*dx + dz*dz);
						float knockbackMultiplier = 0.4F;

						double motX = 0;
						motX += dx/knockbackForce*knockbackMultiplier;
						double motY = knockbackMultiplier;
						double motZ = 0;
						motZ += dz/knockbackForce*knockbackMultiplier;
						if (motY > 0.2) motY = 0.2;
						if (!IsBaby && motY > 0.05) motY = 0.05;

						Knockback(new Vector3((float) motX, (float) motY, (float) motZ));
					}
				}
			}
			catch (Exception e)
			{
				Log.Warn("Pet OnTick Error", e);
			}
		}

		private double RadianToDegree(double angle)
		{
			return angle*(180.0/Math.PI);
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[14] = new MetadataByte((byte) (IsBaby ? 1 : 0));

			return metadata;
		}
	}
}