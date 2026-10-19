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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2019 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Linq;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Particles;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	public class Projectile : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Projectile));

		public Player Shooter { get; set; }
		public int Ttl { get; set; } = 0;
		public bool DespawnOnImpact { get; set; } = true;
		public int PowerLevel { get; set; } = 0;
		public float HitBoxPrecision { get; set; } = 0.3f;
		public Vector3 Force { get; set; } = new Vector3();

		public bool BroadcastMovement { get; set; } = false;

		// The minecraft:projectile component, by its own field names and with BDS's schema defaults.
		// A projectile sets what its vanilla definition states and inherits the rest.

		/// <summary>Launch speed, in blocks per tick.</summary>
		public double Power { get; set; } = 1.3;

		/// <summary>The share of its speed kept each tick in air.</summary>
		public double Inertia { get; set; } = 0.99;

		/// <summary>The share of its speed kept each tick in water or lava.</summary>
		public double LiquidInertia { get; set; } = 0.6;

		/// <summary>Launch inaccuracy at peaceful; each step of difficulty takes off <see cref="UncertaintyMultiplier" />.</summary>
		public double UncertaintyBase { get; set; }

		public double UncertaintyMultiplier { get; set; }

		// Its on_hit.impact_damage: a fixed range plus speed times the power multiplier.

		public double DamageMin { get; set; }
		public double DamageMax { get; set; }
		public double PowerMultiplier { get; set; }
		public bool CeilPreCriticalDamage { get; set; }
		public int MaxCriticalDamage { get; set; } = int.MaxValue;

		/// <summary>The one target the impact damage applies to, by its bare identifier; any target when null.</summary>
		public string DamageFilter { get; set; }

		protected Projectile(Player shooter, EntityType entityTypeId, Level level, double powerMultiplier, bool isCritical = false) : base(entityTypeId, level)
		{
			Shooter = shooter;
			PowerMultiplier = powerMultiplier;
			IsCritical = isCritical;
			Gravity = 0.05;
		}

		private object _spawnSync = new object();

		public override void SpawnEntity()
		{
			lock (_spawnSync)
			{
				if (IsSpawned) throw new Exception("Invalid state. Tried to spawn projectile more than one time.");


				Level.AddEntity(this);

				IsSpawned = true;

				if (BroadcastMovement)
				{
					//LastUpdatedTime = DateTime.UtcNow;

					BroadcastMoveAndMotion();
				}
			}
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();

			if (Shooter != null)
			{
				metadata[5] = new MetadataLong(Shooter.EntityId);
			}

			return metadata;
		}

		public override void OnTick(Entity[] entities)
		{
			//base.OnTick();

			if (KnownPosition.Y <= -16
				|| (Velocity.Length() <= 0 && DespawnOnImpact)
				|| (Velocity.Length() <= 0 && !DespawnOnImpact && Ttl <= 0))
			{
				if (DespawnOnImpact || (!DespawnOnImpact && Ttl <= 0))
				{
					DespawnEntity();
					return;
				}

				return;
			}

			Ttl--;

			if (KnownPosition.Y <= 0 || Velocity.Length() <= 0) return;

			// BDS tests the stretch this tick's motion covers, before drag and gravity, and acts on
			// the nearest hit, block or entity.
			Vector3 start = KnownPosition.ToVector3();
			double? blockFraction = ClipBlocks(start, start + Velocity, out Block collidedWithBlock);
			double blockDistance = blockFraction.HasValue ? blockFraction.Value * Velocity.Length() : double.MaxValue;

			Entity entityCollided = CheckEntityCollide(start, Velocity, out double entityDistance);
			if (entityCollided != null && entityDistance <= blockDistance)
			{
				Vector3 p = start + Vector3.Normalize(Velocity) * (float) entityDistance;
				KnownPosition = new PlayerLocation(p.X, p.Y, p.Z);

				double speed = Math.Sqrt(Velocity.X * Velocity.X + Velocity.Y * Velocity.Y + Velocity.Z * Velocity.Z);
				double damage = ImpactDamage(speed);
				if (IsCritical)
				{
					damage = Math.Min(damage + Level.Random.Next((int) (damage / 2 + 2)), MaxCriticalDamage);

					McpeAnimate animate = McpeAnimate.CreateObject();
					animate.runtimeEntityId = entityCollided.EntityId;
					animate.actionId = McpeAnimate.AnimatePacketPayloadAction.CriticalHit;
					Level.RelayBroadcast(animate);
				}

				if (PowerLevel > 0)
				{
					damage = damage + ((PowerLevel + 1) * 0.25);
				}

				if (!Damages(entityCollided)) damage = 0;

				Player player = entityCollided as Player;

				if (player != null)
				{
					damage = player.DamageCalculator.CalculatePlayerDamage(this, player, null, damage, DamageCause.Projectile);
					player.LastAttackTarget = entityCollided;
				}

				entityCollided.HealthManager.TakeHit(this, (int) damage, DamageCause.Projectile);
				entityCollided.HealthManager.LastDamageSource = Shooter;

				OnHitEntity(entityCollided);
				DespawnEntity();
				return;
			}

			bool collided = collidedWithBlock != null;
			bool sendPosition = Velocity != Vector3.Zero;

			if (collided)
			{
				// It rests exactly on the hit point: stick_in_ground leaves no motion and no offset.
				Vector3 hit = start + Velocity * (float) blockFraction.Value;
				KnownPosition.X = hit.X;
				KnownPosition.Y = hit.Y;
				KnownPosition.Z = hit.Z;
				Velocity = Vector3.Zero;
			}
			else
			{
				// Liquid drag is decided by where the tick starts, and only water counts.
				bool wasInWater = Level.GetBlock(KnownPosition) is Water or FlowingWater;

				KnownPosition.X += (float) Velocity.X;
				KnownPosition.Y += (float) Velocity.Y;
				KnownPosition.Z += (float) Velocity.Z;

				// Rotation turns a fifth of the way per tick toward this tick's motion, taken before drag and gravity.
				KnownPosition.Yaw += WrapDegrees((float) Velocity.GetYaw() - KnownPosition.Yaw) * 0.2f;
				KnownPosition.Pitch += WrapDegrees((float) Velocity.GetPitch() - KnownPosition.Pitch) * 0.2f;

				Velocity *= (float) (wasInWater ? LiquidInertia : Inertia);
				Velocity -= new Vector3(0, (float) Gravity, 0);
				Velocity += Force;
			}

			// For debugging of flight-path
			if (sendPosition && BroadcastMovement)
			{
				//LastUpdatedTime = DateTime.UtcNow;

				BroadcastMoveAndMotion();
			}

			if (collided)
			{
				OnHitBlock(collidedWithBlock);
			}
		}

		/// <summary>The launch inaccuracy at a difficulty, never below zero. Hardcore counts as hard.</summary>
		public double Uncertainty(Difficulty difficulty)
		{
			int step = Math.Min((int) difficulty, (int) Difficulty.Hard);
			return Math.Max(0, UncertaintyBase - UncertaintyMultiplier * step);
		}

		/// <summary>
		///     Launches along a direction the way BDS's projectile component does: the unit direction,
		///     each axis pushed by a gaussian times 0.0075 times the uncertainty, all scaled by power.
		///     The shooter's own motion is not added.
		/// </summary>
		public void Shoot(Vector3 direction, double power)
		{
			float length = direction.Length();
			direction = length < 0.001f ? Vector3.Zero : direction / length;

			double spread = 0.0075 * Uncertainty(Level.Difficulty);
			direction.X += (float) (NextGaussian(Level.Random) * spread);
			direction.Y += (float) (NextGaussian(Level.Random) * spread);
			direction.Z += (float) (NextGaussian(Level.Random) * spread);

			Velocity = direction * (float) power;
			KnownPosition.Yaw = (float) Velocity.GetYaw();
			KnownPosition.Pitch = (float) Velocity.GetPitch();
		}

		private static double NextGaussian(Random random)
		{
			double v1, v2, s;
			do
			{
				v1 = 2 * random.NextDouble() - 1;
				v2 = 2 * random.NextDouble() - 1;
				s = v1 * v1 + v2 * v2;
			} while (s >= 1 || s == 0);

			return v1 * Math.Sqrt(-2 * Math.Log(s) / s);
		}

		/// <summary>A hit's damage before criticals and enchantments: the fixed range plus speed times the power multiplier.</summary>
		public double ImpactDamage(double speed)
		{
			double fixedPart = DamageMax > DamageMin ? DamageMin + Level.Random.NextDouble() * (DamageMax - DamageMin) : DamageMin;
			double damage = fixedPart + speed * PowerMultiplier;
			return CeilPreCriticalDamage ? Math.Ceiling(damage) : damage;
		}

		/// <summary>Whether the impact damage applies to this target at all; knockback applies either way.</summary>
		public bool Damages(Entity target)
		{
			return DamageFilter == null || target.EntityTypeId == "minecraft:" + DamageFilter;
		}

		/// <summary>
		///     The first solid block the segment from start to end runs into, walking the blocks it
		///     crosses in order: the fraction of the segment at the hit, or null when the path is clear.
		/// </summary>
		private double? ClipBlocks(Vector3 start, Vector3 end, out Block hitBlock)
		{
			hitBlock = null;
			Vector3 delta = end - start;
			var ray = new Ray(start, delta);

			int x = (int) Math.Floor(start.X), y = (int) Math.Floor(start.Y), z = (int) Math.Floor(start.Z);
			int stepX = Math.Sign(delta.X), stepY = Math.Sign(delta.Y), stepZ = Math.Sign(delta.Z);
			double nextX = FirstCrossing(start.X, delta.X, x), nextY = FirstCrossing(start.Y, delta.Y, y), nextZ = FirstCrossing(start.Z, delta.Z, z);
			double perX = delta.X != 0 ? 1 / Math.Abs(delta.X) : double.MaxValue;
			double perY = delta.Y != 0 ? 1 / Math.Abs(delta.Y) : double.MaxValue;
			double perZ = delta.Z != 0 ? 1 / Math.Abs(delta.Z) : double.MaxValue;

			while (true)
			{
				Block block = Level.GetBlock(x, y, z);
				if (block.IsSolid)
				{
					double? t = ray.Intersects(block.GetBoundingBox());
					if (t <= 1)
					{
						hitBlock = block;
						return t;
					}
				}

				if (nextX > 1 && nextY > 1 && nextZ > 1) return null;

				if (nextX <= nextY && nextX <= nextZ)
				{
					x += stepX;
					nextX += perX;
				}
				else if (nextY <= nextZ)
				{
					y += stepY;
					nextY += perY;
				}
				else
				{
					z += stepZ;
					nextZ += perZ;
				}
			}
		}

		// The fraction of the segment at which it first leaves its starting block along one axis.
		private static double FirstCrossing(float start, float delta, int block)
		{
			if (delta > 0) return (block + 1 - start) / delta;
			if (delta < 0) return (block - start) / delta;
			return double.MaxValue;
		}

		// An angle difference brought into [-180, 180).
		private static float WrapDegrees(float degrees)
		{
			float wrapped = (degrees + 180) % 360;
			if (wrapped < 0) wrapped += 360;
			return wrapped - 180;
		}

		protected virtual void OnHitBlock(Block blockCollided)
		{
		}

		protected virtual void OnHitEntity(Entity entityCollided)
		{
		}

		private Entity CheckEntityCollide(Vector3 position, Vector3 direction, out double distance)
		{
			distance = double.MaxValue;
			Ray2 ray = new Ray2 {x = position, d = Vector3.Normalize(direction)};

			var entities = Level.Entities.Values.Concat(Level.GetSpawnedPlayers()).OrderBy(entity => Vector3.Distance(position, entity.KnownPosition.ToVector3()));
			foreach (Entity entity in entities)
			{
				if (entity == Shooter) continue;
				if (entity == this) continue;
				if (entity is Projectile) continue; // This should actually be handled for some projectiles
				if (entity is Player player && player.GameMode == GameMode.Spectator) continue;

				if (Intersect(entity.GetBoundingBox() + HitBoxPrecision, ray))
				{
					if (ray.tNear > direction.Length()) break;

					distance = ray.tNear;
					return entity;
				}
			}

			return null;
		}

		/// <summary>
		///     For debugging of flight-path and rotation.
		/// </summary>
		private void BroadcastMoveAndMotion()
		{
			if (new Random().Next(5) == 0)
			{
				McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
				motions.runtimeEntityId = EntityId;
				motions.velocity = Velocity;
				Level.RelayBroadcast(motions);
			}

			if (LastSentPosition != null)
			{
				McpeMoveEntityDelta move = McpeMoveEntityDelta.CreateObject();
				move.runtimeEntityId = EntityId;
				move.prevSentPosition = LastSentPosition;
				move.currentPosition = (PlayerLocation) KnownPosition.Clone();
				move.isOnGround = IsWalker && IsOnGround;
				if (move.SetFlags())
				{
					Level.RelayBroadcast(move);
				}
			}

			LastSentPosition = (PlayerLocation) KnownPosition.Clone(); // Used for delta

			if (Shooter != null && IsCritical)
			{
				var particle = new CriticalParticle(Level);
				particle.Position = KnownPosition.ToVector3();
				particle.Spawn(new[] {Shooter});
			}
		}

		public static bool Intersect(BoundingBox aabb, Ray2 ray)
		{
			Vector3 min = aabb.Min, max = aabb.Max;
			double ix = ray.x.X;
			double iy = ray.x.Y;
			double iz = ray.x.Z;
			double t, u, v;
			bool hit = false;

			ray.tNear = Double.MaxValue;

			t = (min.X - ix) / ray.d.X;
			if (t < ray.tNear && t > -Ray2.EPSILON)
			{
				u = iz + ray.d.Z * t;
				v = iy + ray.d.Y * t;
				if (u >= min.Z && u <= max.Z &&
					v >= min.Y && v <= max.Y)
				{
					hit = true;
					ray.tNear = t;
					ray.u = u;
					ray.v = v;
					ray.n.X = -1;
					ray.n.Y = 0;
					ray.n.Z = 0;
				}
			}

			t = (max.X - ix) / ray.d.X;
			if (t < ray.tNear && t > -Ray2.EPSILON)
			{
				u = iz + ray.d.Z * t;
				v = iy + ray.d.Y * t;
				if (u >= min.Z && u <= max.Z &&
					v >= min.Y && v <= max.Y)
				{
					hit = true;
					ray.tNear = t;
					ray.u = 1 - u;
					ray.v = v;
					ray.n.X = 1;
					ray.n.Y = 0;
					ray.n.Z = 0;
				}
			}

			t = (min.Y - iy) / ray.d.Y;
			if (t < ray.tNear && t > -Ray2.EPSILON)
			{
				u = ix + ray.d.X * t;
				v = iz + ray.d.Z * t;
				if (u >= min.X && u <= max.X &&
					v >= min.Z && v <= max.Z)
				{
					hit = true;
					ray.tNear = t;
					ray.u = u;
					ray.v = v;
					ray.n.X = 0;
					ray.n.Y = -1;
					ray.n.Z = 0;
				}
			}

			t = (max.Y - iy) / ray.d.Y;
			if (t < ray.tNear && t > -Ray2.EPSILON)
			{
				u = ix + ray.d.X * t;
				v = iz + ray.d.Z * t;
				if (u >= min.X && u <= max.X &&
					v >= min.Z && v <= max.Z)
				{
					hit = true;
					ray.tNear = t;
					ray.u = u;
					ray.v = v;
					ray.n.X = 0;
					ray.n.Y = 1;
					ray.n.Z = 0;
				}
			}

			t = (min.Z - iz) / ray.d.Z;
			if (t < ray.tNear && t > -Ray2.EPSILON)
			{
				u = ix + ray.d.X * t;
				v = iy + ray.d.Y * t;
				if (u >= min.X && u <= max.X &&
					v >= min.Y && v <= max.Y)
				{
					hit = true;
					ray.tNear = t;
					ray.u = 1 - u;
					ray.v = v;
					ray.n.X = 0;
					ray.n.Y = 0;
					ray.n.Z = -1;
				}
			}

			t = (max.Z - iz) / ray.d.Z;
			if (t < ray.tNear && t > -Ray2.EPSILON)
			{
				u = ix + ray.d.X * t;
				v = iy + ray.d.Y * t;
				if (u >= min.X && u <= max.X &&
					v >= min.Y && v <= max.Y)
				{
					hit = true;
					ray.tNear = t;
					ray.u = u;
					ray.v = v;
					ray.n.X = 0;
					ray.n.Y = 0;
					ray.n.Z = 1;
				}
			}

			return hit;
		}
	}
}