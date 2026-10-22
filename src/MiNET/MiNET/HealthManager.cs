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
using System.ComponentModel;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET
{
	// Values mirror Mojang's ActorDamageCause (protocol 1001) so a future wire use cannot
	// misreport the cause. Unknown sits on the reference's Override slot; Custom is MiNET's
	// own and lives above the reference range.
	public enum DamageCause
	{
		[Description("{0} went MIA")] Unknown = 0,

		[Description("{0} was pricked  to death")]
		Contact = 1,
		[Description("{0} was slain by {1}")] EntityAttack = 2,
		[Description("{0} was shot by {1}")] Projectile = 3,

		[Description("{0} suffocated in a wall")]
		Suffocation = 4,

		[Description("{0} hit the ground too hard")]
		Fall = 5,
		[Description("{0} went up in flames")] Fire = 6,
		[Description("{0} burned to death")] FireTick = 7,

		[Description("{0} tried to swim in lava")]
		Lava = 8,
		[Description("{0} drowned")] Drowning = 9,
		[Description("{0} blew up")] BlockExplosion = 10,
		[Description("{0} blew up")] EntityExplosion = 11,

		[Description("{0} fell out of the world")]
		Void = 12,
		[Description("{0} died")] Suicide = 13,

		[Description("{0} was killed by magic")]
		Magic = 14,
		[Description("{0} withered away")] Wither = 15,
		[Description("{0} starved to death")] Starving = 16,

		[Description("{0} was squashed by a falling anvil")]
		Anvil = 17,

		[Description("{0} was killed while trying to hurt {1}")]
		Thorns = 18,

		[Description("{0} was squashed by a falling block")]
		FallingBlock = 19,
		[Description("{0} was squished too much")] Piston = 20,

		[Description("{0} experienced kinetic energy")]
		FlyIntoWall = 21,

		[Description("{0} discovered the floor was lava")]
		Magma = 22,
		[Description("{0} went off with a bang")] Fireworks = 23,
		[Description("{0} was struck by lightning")] Lightning = 24,
		[Description("{0} was killed by {1}")] Charging = 25,
		[Description("{0} froze to death")] Temperature = 26,
		[Description("{0} froze to death")] Freezing = 27,

		[Description("{0} was skewered by a falling stalactite")]
		Stalactite = 28,

		[Description("{0} was impaled on a stalagmite")]
		Stalagmite = 29,
		[Description("{0} was rammed by {1}")] RamAttack = 30,

		[Description("{0} was obliterated by a sonically-charged shriek")]
		SonicBoom = 31,
		[Description("{0} went up in flames")] Campfire = 32,
		[Description("{0} went up in flames")] SoulCampfire = 33,

		[Description("{0} died a customized death")]
		Custom = 100
	}

	public class HealthManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(HealthManager));

		public Entity Entity { get; set; }
		public int MaxHealth { get; set; } = 200;
		public int Health { get; set; }
		public float Absorption { get; set; }
		public short MaxAir { get; set; } = 400;
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
			ResetHealth();
		}

		public int Hearts
		{
			get { return (int) Math.Ceiling(Health / 10d); }
		}

		public int MaxHearts
		{
			get { return (int) Math.Ceiling(MaxHealth / 10d); }
		}

		public virtual void Regen(int amount = 1)
		{
			Health += amount * 10;
			if (Health > MaxHealth) Health = MaxHealth;

			var player = Entity as Player;
			if (player != null)
			{
				player.SendUpdateAttributes();
			}
		}

		public virtual void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
		{
			TakeHit(source, null, damage, cause);
		}

		public virtual void TakeHit(Entity source, Item tool, int damage = 1, DamageCause cause = DamageCause.Unknown)
		{
			var player = Entity as Player;
			if (player != null && player.GameMode != GameMode.Survival) return;


			if (CooldownTick > 0) return;

			LastDamageSource = source;
			LastDamageCause = cause;
			if (Absorption > 0)
			{
				float abs = Absorption * 10;
				abs = abs - damage;
				if (abs < 0)
				{
					Absorption = 0;
					damage = Math.Abs((int) Math.Floor(abs));
				}
				else
				{
					Absorption = abs / 10f;
					damage = 0;
				}
			}

			if (cause == DamageCause.Starving)
			{
				if (Entity.Level.Difficulty <= Difficulty.Easy && Hearts <= 10) return;
				if (Entity.Level.Difficulty <= Difficulty.Normal && Hearts <= 1) return;
			}

			Health -= damage * 10;
			if (Health < 0)
			{
				OnPlayerTakeHit(new HealthEventArgs(this, source, Entity));
				Health = 0;
				Kill();
				return;
			}

			if (player != null)
			{
				player.HungerManager.IncreaseExhaustion(0.3f);

				player.SendUpdateAttributes();
			}

			Entity.BroadcastEntityEvent();

			if (source != null)
			{
				DoKnockback(source, tool);
			}

			CooldownTick = 10;

			OnPlayerTakeHit(new HealthEventArgs(this, source, Entity));
		}

		protected virtual void DoKnockback(Entity source, Item tool)
		{
			double dx = source.KnownPosition.X - Entity.KnownPosition.X;

			Random rand = new Random();
			double dz;
			for (dz = source.KnownPosition.Z - Entity.KnownPosition.Z; dx * dx + dz * dz < 0.00010; dz = (rand.NextDouble() - rand.NextDouble()) * 0.01D)
			{
				dx = (rand.NextDouble() - rand.NextDouble()) * 0.01D;
			}

			double knockbackForce = Math.Sqrt(dx * dx + dz * dz);
			float knockbackMultiplier = 0.4F;

			//this.motX /= 2.0D;
			//this.motY /= 2.0D;
			//this.motZ /= 2.0D;
			double motX = 0;
			motX -= dx / knockbackForce * knockbackMultiplier;
			double motY = knockbackMultiplier;
			double motZ = 0;
			motZ -= dz / knockbackForce * knockbackMultiplier;
			if (motY > 0.4)
			{
				motY = 0.4;
			}

			var velocity = new Vector3((float) motX, (float) motY + 0.0f, (float) motZ);

			if (tool != null)
			{
				var knockback = tool.GetEnchantingLevel(EnchantingType.Knockback);
				velocity += Vector3.Normalize(velocity) * new Vector3(knockback * 0.5f, 0.1f, knockback * 0.5f);
			}

			Entity.Knockback(velocity);
		}

		public event EventHandler<HealthEventArgs> PlayerTakeHit;

		protected virtual void OnPlayerTakeHit(HealthEventArgs e)
		{
			EventHandler<HealthEventArgs> handler = PlayerTakeHit;
			if (handler != null) handler(this, e);
		}

		public virtual void Ignite(int ticks = 300)
		{
			if (IsDead) return;

			Player player = Entity as Player;
			if (player != null)
			{
				ticks -= ticks * player.DamageCalculator.CalculateFireTickReduction(player);
			}

			ticks = Math.Max(0, ticks);

			FireTick = ticks;
			if (!IsOnFire)
			{
				IsOnFire = true;
				Entity.BroadcastSetEntityData();
			}
		}

		private object _killSync = new object();

		public virtual void Kill()
		{
			lock (_killSync)
			{
				if (IsDead) return;
				IsDead = true;

				Health = 0;
			}

			var player = Entity as Player;
			if (player != null)
			{
				player.StopSleep();
				player.SendUpdateAttributes();
			}

			Entity.BroadcastEntityEvent();

			if (player != null)
			{
				//SendWithDelay(2000, () =>
				//{
				//});

				Entity.BroadcastSetEntityData();
				Entity.DespawnEntity();

				if (!Entity.Level.KeepInventory)
				{
					player.DropInventory();
				}

				var mcpeRespawn = McpeRespawn.CreateObject();
				mcpeRespawn.x = player.SpawnPosition.X;
				mcpeRespawn.y = player.SpawnPosition.Y;
				mcpeRespawn.z = player.SpawnPosition.Z;
				mcpeRespawn.state = (byte) McpeRespawn.RespawnState.Search;
				mcpeRespawn.runtimeEntityId = player.EntityId;
				player.SendPacket(mcpeRespawn);
			}
			else
			{
				if (LastDamageSource is Player && Entity.Level.DoMobloot)
				{
					var drops = Entity.GetDrops();
					foreach (var drop in drops)
					{
						Entity.Level.DropItem(Entity.KnownPosition.ToVector3(), drop);
					}
				}

				// This is semi-good, but we need to give the death-animation time to play.
				
				_ = SendWithDelay(2000, () =>
				{
					Entity.BroadcastSetEntityData();
					Entity.DespawnEntity();
				});
			}
		}

		private async Task SendWithDelay(int delay, Action action)
		{
			await Task.Delay(delay);
			action();
		}

		public virtual void ResetHealth()
		{
			Health = MaxHealth;
			Air = MaxAir;
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
			if (!Entity.IsSpawned) return;

			if (IsDead) return;

			if (CooldownTick > 0)
			{
				CooldownTick--;
			}
			else
			{
				LastDamageSource = null;
			}

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
				return;
			}

			if (IsInWater(Entity.KnownPosition))
			{
				// Broadcast on the wet/dry TRANSITION and on damage ticks only. An unconditional
				// broadcast here is one SetEntityData to every player on the level, per submerged
				// entity, per tick: at fleet scale that alone was a measurable share of the tick.
				if (!Entity.IsInWater)
				{
					Entity.IsInWater = true;
					Entity.BroadcastSetEntityData();
				}

				// Only entities that can actually drown run the air counter. A creative player's
				// air never moves, so their metadata stays stable and the broadcast hash gate
				// holds; without this, Air ticking down made every submerged entity a genuine
				// metadata change every tick, forever.
				if (Entity is not Player player || player.GameMode == GameMode.Survival)
				{
					Air--;
					if (Air <= 0)
					{
						if (Math.Abs(Air) % 10 == 0)
						{
							TakeHit(null, 1, DamageCause.Drowning);
							Entity.BroadcastSetEntityData();
						}
					}
				}
			}
			else
			{
				Air = MaxAir;

				if (Entity.IsInWater)
				{
					Entity.IsInWater = false;
					Entity.BroadcastSetEntityData();
				}
			}

			if (IsOnFire && (Entity.IsInWater || IsStandingInWater(Entity.KnownPosition)))
			{
				IsOnFire = false;
				FireTick = 0;
				Entity.BroadcastSetEntityData();
			}

			if (IsInOpaque(Entity.KnownPosition))
			{
				if (SuffocationTicks <= 0)
				{
					TakeHit(null, 1, DamageCause.Suffocation);
					Entity.BroadcastSetEntityData();

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
					Ignite(300);
				}

				if (LavaTicks <= 0)
				{
					TakeHit(null, 4, DamageCause.Lava);
					Entity.BroadcastSetEntityData();

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
				if (FireTick <= 0)
				{
					IsOnFire = false;
					Entity.BroadcastSetEntityData();
				}
				else if (FireTick % 20 == 0)
				{
					var player = Entity as Player;
					if (player != null)
					{
						player.DamageCalculator.CalculatePlayerDamage(null, player, null, 1, DamageCause.FireTick);
						TakeHit(null, 1, DamageCause.FireTick);
					}
					else
					{
						TakeHit(null, 1, DamageCause.FireTick);
					}
					//Entity.BroadcastSetEntityData();
				}

				FireTick--;
			}
		}

		public bool IsInWater(PlayerLocation playerPosition)
		{
			float y = playerPosition.Y + 1.62f;

			BlockCoordinates waterPos = new BlockCoordinates
			{
				X = (int) Math.Floor(playerPosition.X),
				Y = (int) Math.Floor(y),
				Z = (int) Math.Floor(playerPosition.Z)
			};

			int runtimeId = Entity.Level.GetRuntimeIdAt(waterPos);
			if (!BlockFactory.Is<Water>(runtimeId) && !BlockFactory.Is<FlowingWater>(runtimeId)) return false;

			return y < Math.Floor(y) + 1 - ((1f / 9f) - 0.1111111);
		}

		public bool IsStandingInWater(PlayerLocation playerPosition)
		{
			int runtimeId = Entity.Level.GetRuntimeIdAt((BlockCoordinates) playerPosition);
			if (!BlockFactory.Is<Water>(runtimeId) && !BlockFactory.Is<FlowingWater>(runtimeId)) return false;

			return playerPosition.Y < Math.Floor(playerPosition.Y) + 1 - ((1f / 9f) - 0.1111111);
		}

		private bool IsInLava(PlayerLocation playerPosition)
		{
			int runtimeId = Entity.Level.GetRuntimeIdAt((BlockCoordinates) playerPosition);
			if (!BlockFactory.Is<Lava>(runtimeId) && !BlockFactory.Is<FlowingLava>(runtimeId)) return false;

			return playerPosition.Y < Math.Floor(playerPosition.Y) + 1 - ((1f / 9f) - 0.1111111);
		}

		private bool IsInOpaque(PlayerLocation playerPosition)
		{
			BlockCoordinates solidPos = (BlockCoordinates) playerPosition;
			if (Entity.Height >= 1)
			{
				solidPos.Y += 1;
			}

			return !Entity.Level.IsTransparent(solidPos);
		}

		public static string GetDescription(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

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