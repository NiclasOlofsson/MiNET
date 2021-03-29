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
		public int Damage { get; set; }
		public int PowerLevel { get; set; } = 0;
		public float HitBoxPrecision { get; set; } = 0.3f;
		public Vector3 Force { get; set; } = new Vector3();

		public bool BroadcastMovement { get; set; } = false;

		protected Projectile(Player shooter, EntityType entityTypeId, Level level, int damage, bool isCritical = false) : base(entityTypeId, level)
		{
			Shooter = shooter;
			Damage = damage;
			IsCritical = isCritical;
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

			Entity entityCollided = CheckEntityCollide(KnownPosition, Velocity);

			bool collided = false;
			Block collidedWithBlock = null;
			if (entityCollided != null)
			{
				double speed = Math.Sqrt(Velocity.X * Velocity.X + Velocity.Y * Velocity.Y + Velocity.Z * Velocity.Z);
				double damage = Math.Ceiling(speed * Damage);
				if (IsCritical)
				{
					damage += Level.Random.Next((int) (damage / 2 + 2));

					McpeAnimate animate = McpeAnimate.CreateObject();
					animate.runtimeEntityId = entityCollided.EntityId;
					animate.actionId = 4;
					Level.RelayBroadcast(animate);
				}

				if (PowerLevel > 0)
				{
					damage = damage + ((PowerLevel + 1) * 0.25);
				}

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
			else
			{
				var velocity2 = Velocity;
				velocity2 *= (float) (1.0d - Drag);
				velocity2 -= new Vector3(0, (float) Gravity, 0);
				double distance = velocity2.Length();
				velocity2 = Vector3.Normalize(velocity2) / 2;

				for (int i = 0; i < Math.Ceiling(distance) * 2; i++)
				{
					Vector3 nextPos = KnownPosition.ToVector3();
					nextPos.X += (float) velocity2.X * i;
					nextPos.Y += (float) velocity2.Y * i;
					nextPos.Z += (float) velocity2.Z * i;

					Block block = Level.GetBlock(nextPos);
					collided = block.IsSolid && block.GetBoundingBox().Contains(nextPos);
					if (collided)
					{
						SetIntersectLocation(block.GetBoundingBox(), KnownPosition.ToVector3());
						collidedWithBlock = block;
						break;
					}
				}
			}

			bool sendPosition = Velocity != Vector3.Zero;

			if (collided)
			{
				Velocity = Vector3.Zero;
			}
			else
			{
				KnownPosition.X += (float) Velocity.X;
				KnownPosition.Y += (float) Velocity.Y;
				KnownPosition.Z += (float) Velocity.Z;

				Velocity *= (float) (1.0 - Drag);
				Velocity -= new Vector3(0, (float) Gravity, 0);
				Velocity += Force;

				KnownPosition.Yaw = (float) Velocity.GetYaw();
				KnownPosition.Pitch = (float) Velocity.GetPitch();
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

		protected virtual void OnHitBlock(Block blockCollided)
		{
		}

		protected virtual void OnHitEntity(Entity entityCollided)
		{
		}

		private Entity CheckEntityCollide(Vector3 position, Vector3 direction)
		{
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

					Vector3 p = ray.x + new Vector3((float) ray.tNear) * ray.d;
					KnownPosition = new PlayerLocation(p.X, p.Y, p.Z);
					return entity;
				}
			}

			return null;
		}

		private bool SetIntersectLocation(BoundingBox bbox, Vector3 location)
		{
			Ray ray = new Ray(location - Velocity, Vector3.Normalize(Velocity));
			double? distance = ray.Intersects(bbox);
			if (distance != null)
			{
				float dist = (float) distance - 0.1f;
				Vector3 pos = ray.Position + (ray.Direction * new Vector3(dist));
				KnownPosition.X = pos.X;
				KnownPosition.Y = pos.Y;
				KnownPosition.Z = pos.Z;
				return true;
			}

			return false;
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