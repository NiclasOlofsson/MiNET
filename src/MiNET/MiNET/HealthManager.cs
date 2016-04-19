﻿using System;
using System.ComponentModel;
using System.Reflection;
using log4net;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public enum DamageCause
	{
		[Description("{0} went MIA")] Unknown,
		[Description("{0} was pricked  to death")] Contact,
		[Description("{0} was slain by {1}")] EntityAttack,
		[Description("{0} was shot by {1}")] Projectile,
		[Description("{0} suffocated in a wall")] Suffocation,
		[Description("{0} hit the ground too hard")] Fall,
		[Description("{0} went up in flames")] Fire,
		[Description("{0} burned to death")] FireTick,
		[Description("{0} tried to swim in lava")] Lava,
		[Description("{0} drowned")] Drowning,
		[Description("{0} blew up")] BlockExplosion,
		[Description("{0} blew up")] EntityExplosion,
		[Description("{0} fell out of the world")] Void,
		[Description("{0} died")] Suicide,
		[Description("{0} was killed by magic")] Magic,
		[Description("{0} died a customized death")] Custom
	}

	public class HealthManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (HealthManager));

		private int _hearts;
		public Entity Entity { get; set; }
		public int MaxHealth { get; set; }
		public int Health { get; set; }
		public short Air { get; set; }
		public bool IsDead { get; set; }
		public int FireTick { get; set; }
		public int SuffocationTicks { get; set; }
		public int LavaTicks { get; set; }
		public int CooldownTick { get; set; }
		public bool IsOnFire { get; set; }
		public bool IsInvulnerable { get; set; }
		public DamageCause LastDamageCause { get; set; }
		public Entity LastDamageSource { get; set; }

		public HealthManager(Entity entity)
		{
			Entity = entity;
			MaxHealth = 200;
			ResetHealth();
		}

		public int Hearts
		{
			get { return (int) Math.Ceiling(Health/10d); }
		}

		public virtual void Regen(int amount = 1)
		{
			Health += amount*10;
			if (Health > MaxHealth) Health = MaxHealth;

			var player = Entity as Player;
			if (player != null)
			{
				player.SendUpdateAttributes();
			}
		}

		public virtual void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
		{
			var player = Entity as Player;
			if (player != null && player.GameMode != GameMode.Survival) return;


			if (CooldownTick > 0) return;

			LastDamageSource = source;
			LastDamageCause = cause;

			Health -= damage*10;
			if (Health < 0) Health = 0;

			if (player != null)
			{
				player.HungerManager.IncreaseExhaustion(0.3f);

				player.SendUpdateAttributes();
				player.BroadcastEntityEvent();
			}
			else
			{
				var msg = McpeEntityEvent.CreateObject();
				msg.entityId = Entity.EntityId;
				msg.eventId = (byte) (Health <= 0 ? 3 : 2);
				Entity.Level.RelayBroadcast(msg);
			}

			if (source != null)
			{
				double dx = source.KnownPosition.X - Entity.KnownPosition.X;

				Random rand = new Random();
				double dz;
				for (dz = source.KnownPosition.Z - Entity.KnownPosition.Z; dx*dx + dz*dz < 0.00010; dz = (rand.NextDouble() - rand.NextDouble())*0.01D)
				{
					dx = (rand.NextDouble() - rand.NextDouble())*0.01D;
				}

				double knockbackForce = Math.Sqrt(dx*dx + dz*dz);
				float knockbackMultiplier = 0.4F;

				//this.motX /= 2.0D;
				//this.motY /= 2.0D;
				//this.motZ /= 2.0D;
				double motX = 0;
				motX -= dx/knockbackForce*knockbackMultiplier;
				double motY = knockbackMultiplier;
				double motZ = 0;
				motZ -= dz/knockbackForce*knockbackMultiplier;
				if (motY > 0.4)
				{
					motY = 0.4;
				}
				Entity.Knockback(new Vector3(motX, motY, motZ));
			}

			CooldownTick = 10;

			OnPlayerTakeHit(new HealthEventArgs(this, source, Entity));
		}

		public event EventHandler<HealthEventArgs> PlayerTakeHit;

		protected virtual void OnPlayerTakeHit(HealthEventArgs e)
		{
			EventHandler<HealthEventArgs> handler = PlayerTakeHit;
			if (handler != null) handler(this, e);
		}

		public virtual void Ignite(int ticks = 300)
		{
			Ignite(Entity, ticks);
		}

		public virtual void Ignite(Entity entity, int ticks = 300)
		{
			if (IsDead) return;

			FireTick = ticks;
			IsOnFire = true;
			entity.BroadcastSetEntityData();
		}

		public virtual void Kill()
		{
			if (IsDead) return;

			IsDead = true;
			Health = 0;
			var player = Entity as Player;
			if (player != null)
			{
				player.SendUpdateAttributes();
				player.BroadcastEntityEvent();
			}

			Entity.BroadcastSetEntityData();
			Entity.DespawnEntity();

			if (player != null)
			{
				player.DropInventory();

				var mcpeRespawn = McpeRespawn.CreateObject();
				mcpeRespawn.x = player.SpawnPosition.X;
				mcpeRespawn.y = player.SpawnPosition.Y;
				mcpeRespawn.z = player.SpawnPosition.Z;
				player.SendPackage(mcpeRespawn);
			}
			else
			{
				var drops = Entity.GetDrops();
				foreach (var drop in drops)
				{
					Entity.Level.DropItem(Entity.KnownPosition.ToVector3(), drop);
				}
			}
		}

		public virtual void ResetHealth()
		{
			IsInvulnerable = false;
			Health = MaxHealth;
			Air = 300;
			IsOnFire = false;
			FireTick = 0;
			SuffocationTicks = 10;
			LavaTicks = 0;
			IsDead = false;
			CooldownTick = 0;
			LastDamageCause = DamageCause.Unknown;
			LastDamageSource = null;
		}

		public virtual void OnTick()
		{
			if (CooldownTick > 0) CooldownTick--;

			if (!Entity.IsSpawned) return;

			if (IsDead) return;

			if (IsInvulnerable) Health = MaxHealth;

			if (Health <= 0)
			{
				Kill();
				return;
			}

			if (Entity.KnownPosition.Y < 0 && !IsDead)
			{
				CooldownTick = 0;
				TakeHit(null, 300, DamageCause.Void);
				LastDamageCause = DamageCause.Void;
				return;
			}

			if (IsInWater(Entity.KnownPosition))
			{
				Air--;
				if (Air <= 0)
				{
					if (Math.Abs(Air)%10 == 0)
					{
						Health -= 10;
						var player = Entity as Player;
						if (player != null)
						{
							player.SendUpdateAttributes();
							player.BroadcastEntityEvent();
						}
						Entity.BroadcastSetEntityData();
						LastDamageCause = DamageCause.Drowning;
					}
				}

				if (IsOnFire)
				{
					IsOnFire = false;
					FireTick = 0;
					Entity.BroadcastSetEntityData();
				}
			}
			else
			{
				Air = 300;
			}

			if (IsInSolid(Entity.KnownPosition))
			{
				if (SuffocationTicks <= 0)
				{
					Health -= 10;
					var player = Entity as Player;
					if (player != null)
					{
						player.SendUpdateAttributes();
						player.BroadcastEntityEvent();
					}
					Entity.BroadcastSetEntityData();
					LastDamageCause = DamageCause.Suffocation;
					SuffocationTicks = 10;
				}
				else
				{
					SuffocationTicks--;
				}
			}
			else
			{
				SuffocationTicks = 10;
			}

			if (IsInLava(Entity.KnownPosition))
			{
				if (LastDamageCause.Equals(DamageCause.Lava))
				{
					FireTick += 2;
				}
				else
				{
					FireTick = 300;
					IsOnFire = true;
					Entity.BroadcastSetEntityData();
				}

				if (LavaTicks <= 0)
				{
					Health -= 40;

					var player = Entity as Player;
					if (player != null)
					{
						player.SendUpdateAttributes();
						player.BroadcastEntityEvent();
					}
					Entity.BroadcastSetEntityData();
					LastDamageCause = DamageCause.Lava;
					LavaTicks = 10;
				}
				else
				{
					LavaTicks--;
				}
			}
			else
			{
				LavaTicks = 0;
			}

			if (!IsInLava(Entity.KnownPosition) && IsOnFire)
			{
				FireTick--;
				if (FireTick <= 0)
				{
					IsOnFire = false;
				}

				if (Math.Abs(FireTick)%20 == 0)
				{
					Health -= 10;
					var player = Entity as Player;
					if (player != null)
					{
						player.SendUpdateAttributes();
						player.BroadcastEntityEvent();
					}
					Entity.BroadcastSetEntityData();
					LastDamageCause = DamageCause.FireTick;
				}
			}
		}

		private bool IsInWater(PlayerLocation playerPosition)
		{
			float y = playerPosition.Y + 1.62f;

			BlockCoordinates waterPos = new BlockCoordinates
			{
				X = (int) Math.Floor(playerPosition.X),
				Y = (int) Math.Floor(y),
				Z = (int) Math.Floor(playerPosition.Z)
			};

			var block = Entity.Level.GetBlock(waterPos);

			if (block == null || (block.Id != 8 && block.Id != 9)) return false;

			return y < Math.Floor(y) + 1 - ((1/9) - 0.1111111);
		}

		private bool IsInLava(PlayerLocation playerPosition)
		{
			var block = Entity.Level.GetBlock(playerPosition);

			if (block == null || (block.Id != 10 && block.Id != 11)) return false;

			return playerPosition.Y < Math.Floor(playerPosition.Y) + 1 - ((1/9) - 0.1111111);
		}

		private bool IsInSolid(PlayerLocation playerPosition)
		{
			float y = playerPosition.Y + 1.62f;

			BlockCoordinates solidPos = new BlockCoordinates
			{
				X = (int)Math.Floor(playerPosition.X),
				Y = (int)Math.Floor(y),
				Z = (int)Math.Floor(playerPosition.Z)
			};

			var block = Entity.Level.GetBlock(solidPos);

			if (block == null) return false;

			return block.IsSolid;
		}

		public static string GetDescription(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

			if (attributes.Length > 0)
				return attributes[0].Description;

			return value.ToString();
		}
	}

	public class HealthEventArgs : EventArgs
	{
		public Entity SourceEntity { get; set; }
		public Entity TargetEntity { get; set; }
		public HealthManager HealthManager { get; set; }

		public HealthEventArgs(HealthManager healthManager, Entity sourceEntity, Entity targetEntity)
		{
			SourceEntity = sourceEntity;
			TargetEntity = targetEntity;
			HealthManager = healthManager;
		}
	}
}