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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using log4net;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Entity));


		public Level Level { get; set; }

		public string EntityTypeId { get; protected set; }
		public long EntityId { get; set; }
		public bool IsSpawned { get; set; }
		public bool CanDespawn { get; set; } = true;

		public DateTime LastUpdatedTime { get; set; }
		public PlayerLocation KnownPosition { get; set; }
		public Vector3 Velocity { get; set; }
		public float PositionOffset { get; set; }
		public bool IsOnGround { get; set; } = true;

		public PlayerLocation LastSentPosition { get; set; }

		public HealthManager HealthManager { get; set; }

		public string NameTag { get; set; }

		public bool IsPanicking { get; set; }

		public bool NoAi { get; set; }
		public bool HideNameTag { get; set; } = true;
		public bool Silent { get; set; }
		public bool IsInWater { get; set; } = false;
		public bool IsOutOfWater => !IsInWater;
		public int PotionColor { get; set; }
		public int Variant { get; set; }
		public int EatingHaystack { get; set; }

		public long Age { get; set; }
		public double Scale { get; set; } = 1.0;
		public virtual double Height { get; set; } = 1;
		public virtual double Width { get; set; } = 1;
		public virtual double Length { get; set; } = 1;
		public double Drag { get; set; } = 0.02;
		public double Gravity { get; set; } = 0.08;
		public int AttackDamage { get; set; } = 2;
		public int Data { get; set; }

		public long PortalDetected { get; set; }

		public Vector3 RiderSeatPosition { get; set; }
		public bool RiderRotationLocked { get; set; }
		public double RiderMaxRotation { get; set; }
		public double RiderMinRotation { get; set; }

		public ConcurrentDictionary<Type, object> PluginStore { get; set; } = new ConcurrentDictionary<Type, object>();

		public Entity(string entityTypeId, Level level)
		{
			EntityId = EntityManager.EntityIdUndefined;
			Level = level;
			EntityTypeId = entityTypeId;
			KnownPosition = new PlayerLocation();
			HealthManager = new HealthManager(this);
		}

		public Entity(EntityType entityTypeId, Level level) : this(entityTypeId.ToStringId(), level)
		{
		}

		public Entity(int entityTypeId, Level level) : this((EntityType) entityTypeId, level)
		{
		}

		public enum MetadataFlags
		{
			EntityFlags = 0,
			Variant = 2,
			Color = 3,
			HideNameTag = 3,
			NameTag = 4,
			Owner = 5,
			AvailableAir = 7,
			PotionColor = 8,
			EatingHaystack = 16,
			FireworksType = 16,
			MaybeAge = 24,
			PlayerFlags = 26,
			BedPosition = 28,
			Scale = 38,
			MaxAir = 42,
			Markings = 43,
			CollisionBoxWidth = 53,
			CollisionBoxHeight = 54,

			DataFuseLength = 55,

			RiderSeatPosition = 56,
			RiderRotationLocked = 57,
			RiderMaxRotation = 58,
			RiderMinRotation = 59,
			AlwaysShowNameTag = 81,

			EntityFlags2 = 91, // same treatment as 0 flags, perhaps

			ButtonText = 99,
		}

		public virtual MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[(int) MetadataFlags.EntityFlags] = new MetadataLong(GetDataValue());
			metadata[1] = new MetadataInt(1);
			metadata[(int) MetadataFlags.HideNameTag] = new MetadataByte(!HideNameTag);
			metadata[(int) MetadataFlags.NameTag] = new MetadataString(NameTag ?? string.Empty);
			metadata[(int) MetadataFlags.AvailableAir] = new MetadataShort(HealthManager.Air);
			metadata[(int) MetadataFlags.PotionColor] = new MetadataInt(PotionColor);
			metadata[(int) MetadataFlags.Scale] = new MetadataFloat(Scale); // Scale
			metadata[(int) MetadataFlags.MaxAir] = new MetadataShort(HealthManager.MaxAir);
			metadata[(int) MetadataFlags.CollisionBoxWidth] = new MetadataFloat(Width); // Collision box height
			metadata[(int) MetadataFlags.CollisionBoxHeight] = new MetadataFloat(Height); // Collision box width
			metadata[(int) MetadataFlags.RiderSeatPosition] = new MetadataVector3(RiderSeatPosition);
			metadata[(int) MetadataFlags.RiderRotationLocked] = new MetadataByte(RiderRotationLocked);
			metadata[(int) MetadataFlags.RiderMaxRotation] = new MetadataFloat(RiderMaxRotation);
			metadata[(int) MetadataFlags.RiderMinRotation] = new MetadataFloat(RiderMinRotation);
			metadata[(int) MetadataFlags.AlwaysShowNameTag] = new MetadataByte(IsAlwaysShowName);
			return metadata;
		}

		public virtual long GetDataValue()
		{
			//Player: 10000000000000011001000000000000
			// 12, 15, 16, 31

			BitArray bits = GetFlags();

			byte[] bytes = new byte[8];
			bits.CopyTo(bytes, 0);

			long dataValue = BitConverter.ToInt64(bytes, 0);
			//Log.Debug($"Bit-array datavalue: dec={dataValue} hex=0x{dataValue:x2}, bin={Convert.ToString((long) dataValue, 2)}b ");
			//if (Log.IsDebugEnabled) Log.Debug($"// {Convert.ToString(dataValue, 2)}; {FlagsToString(dataValue)}");
			return dataValue;
		}

		public static string MetadataToCode(MetadataDictionary metadata)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine();
			sb.AppendLine("MetadataDictionary metadata = new MetadataDictionary();");

			foreach (var kvp in metadata._entries)
			{
				int idx = kvp.Key;
				MetadataEntry entry = kvp.Value;

				sb.Append($"metadata[{idx}] = new ");
				switch (entry.Identifier)
				{
					case 0:
					{
						var e = (MetadataByte) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 1:
					{
						var e = (MetadataShort) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 2:
					{
						var e = (MetadataInt) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 3:
					{
						var e = (MetadataFloat) entry;
						sb.Append($"{e.GetType().Name}({e.Value.ToString(NumberFormatInfo.InvariantInfo)}f);");
						break;
					}
					case 4:
					{
						var e = (MetadataString) entry;
						sb.Append($"{e.GetType().Name}(\"{e.Value}\");");
						break;
					}
					case 5:
					{
						var e = (MetadataNbt) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 6:
					{
						var e = (MetadataIntCoordinates) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 7:
					{
						var e = (MetadataLong) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						if (idx == 0)
						{
							sb.Append($" // {Convert.ToString((long) e.Value, 2)}; {FlagsToString(e.Value)}");
						}
						break;
					}
					case 8:
					{
						var e = (MetadataVector3) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}

		private static string FlagsToString(long input)
		{
			BitArray bits = new BitArray(BitConverter.GetBytes(input));

			byte[] bytes = new byte[8];
			bits.CopyTo(bytes, 0);

			List<DataFlags> flags = new List<DataFlags>();
			foreach (var val in Enum.GetValues(typeof(DataFlags)))
			{
				if (bits[(int) val]) flags.Add((DataFlags) val);
			}

			StringBuilder sb = new StringBuilder();
			sb.Append(string.Join(", ", flags));
			sb.Append("; ");
			for (var i = 0; i < bits.Count; i++)
			{
				if (bits[i]) sb.Append($"{i}, ");
			}

			return sb.ToString();
		}

		public bool IsSneaking { get; set; }
		public bool IsRiding { get; set; }
		public bool IsSprinting { get; set; }
		public bool IsUsingItem { get; set; }
		public bool IsInvisible { get; set; }
		public bool IsTempted { get; set; }
		public bool IsInLove { get; set; }
		public bool IsSaddled { get; set; }
		public bool IsPowered { get; set; }
		public bool IsIgnited { get; set; }
		public bool IsBaby { get; set; }
		public bool IsConverting { get; set; }
		public bool IsCritical { get; set; }
		public bool IsShowName => !HideNameTag;
		public bool IsAlwaysShowName { get; set; }
		public bool IsNoAi => NoAi;
		public bool HaveAi => !NoAi;
		public bool IsSilent { get; set; }
		public bool IsWallClimbing { get; set; }
		public bool CanClimb { get; set; }
		public bool IsWalker { get; set; }
		public bool IsResting { get; set; }
		public bool IsSitting { get; set; }
		public bool IsAngry { get; set; }
		public bool IsInterested { get; set; }
		public bool IsCharged { get; set; }
		public bool IsTamed { get; set; }
		public bool IsLeashed { get; set; }
		public bool IsSheared { get; set; }
		public bool IsGliding { get; set; }
		public bool IsElder { get; set; }
		public bool IsIdling { get; set; }
		public bool IsRearing { get; set; }
		public bool IsVibrating { get; set; }
		public bool IsMoving { get; set; }
		public bool IsBreathing => !IsInWater;
		public bool IsChested { get; set; }
		public bool IsStackable { get; set; }
		public bool HasCollision { get; set; }
		public bool IsAffectedByGravity { get; set; }
		public bool IsWasdControlled { get; set; }
		public bool CanPowerJump { get; set; }

		public enum DataFlags
		{
			OnFire = 0,
			Sneaking,
			Riding,
			Sprinting,
			UsingItem,
			Invisible,
			Tempted,
			InLove,

			Saddled,
			Powered,
			Ignited,
			Baby,
			Converting,
			Critcal,
			ShowName,
			AlwaysShowName,

			NoAi,
			Silent,
			WallClimbing,

			CanClimb,
			CanSwim,
			CanFly,
			Walker,

			Resting,
			Sitting,
			Angry,
			Interested,
			Charged,

			Tamed,
			Orphaned,
			Leashed,
			Sheared,
			FlagAllFlying,
			Elder,
			Moving,
			Breathing,
			Chested,

			Stackable,
			Showbase,
			Rearing,
			Vibrating,
			Idling,
			EvokerSpell,
			ChargeAttack,
			WasdControlled,
			CanPowerJump,
			Linger,
			HasCollision,
			AffectedByGravity,
			FireImmune,
			Dancing,
			Enchanted
		}

		protected virtual BitArray GetFlags()
		{
			BitArray bits = new BitArray(64);
			bits[(int) DataFlags.OnFire] = HealthManager.IsOnFire;
			bits[(int) DataFlags.Sneaking] = IsSneaking;
			bits[(int) DataFlags.Riding] = IsRiding;
			bits[(int) DataFlags.Sprinting] = IsSprinting;
			bits[(int) DataFlags.UsingItem] = IsUsingItem;
			bits[(int) DataFlags.Invisible] = IsInvisible;
			bits[(int) DataFlags.Tempted] = IsTempted;
			bits[(int) DataFlags.InLove] = IsInLove;
			bits[(int) DataFlags.Saddled] = IsSaddled;
			bits[(int) DataFlags.Powered] = IsPowered;
			bits[(int) DataFlags.Ignited] = IsIgnited;
			bits[(int) DataFlags.Baby] = IsBaby;
			bits[(int) DataFlags.Converting] = IsConverting;
			bits[(int) DataFlags.Critcal] = IsCritical;
			bits[(int) DataFlags.ShowName] = IsShowName;
			bits[(int) DataFlags.AlwaysShowName] = IsAlwaysShowName;
			bits[(int) DataFlags.NoAi] = IsNoAi;
			bits[(int) DataFlags.Silent] = IsSilent;
			bits[(int) DataFlags.WallClimbing] = IsWallClimbing;
			bits[(int) DataFlags.CanClimb] = CanClimb;
			bits[(int) DataFlags.Walker] = IsWalker;
			bits[(int) DataFlags.Resting] = IsResting;
			bits[(int) DataFlags.Sitting] = IsSitting;
			bits[(int) DataFlags.Angry] = IsAngry;
			bits[(int) DataFlags.Interested] = IsInterested;
			bits[(int) DataFlags.Charged] = IsCharged;
			bits[(int) DataFlags.Tamed] = IsTamed;
			bits[(int) DataFlags.Leashed] = IsLeashed;
			bits[(int) DataFlags.Sheared] = IsSheared;
			bits[(int) DataFlags.FlagAllFlying] = IsGliding;
			bits[(int) DataFlags.Elder] = IsElder;
			bits[(int) DataFlags.Moving] = IsMoving;
			bits[(int) DataFlags.Breathing] = IsBreathing;
			bits[(int) DataFlags.Chested] = IsChested;
			bits[(int) DataFlags.Stackable] = IsStackable;
			bits[(int) DataFlags.Idling] = IsIdling;
			bits[(int) DataFlags.Rearing] = IsRearing;
			bits[(int) DataFlags.Vibrating] = IsVibrating;

			bits[(int) DataFlags.HasCollision] = HasCollision;
			bits[(int) DataFlags.AffectedByGravity] = IsAffectedByGravity;

			bits[(int) DataFlags.WasdControlled] = IsWasdControlled;
			bits[(int) DataFlags.CanPowerJump] = CanPowerJump;

			return bits;
		}

		protected virtual bool DetectInPortal()
		{
			if (Level.Dimension == Dimension.Overworld && Level.NetherLevel == null) return false;
			if (Level.Dimension == Dimension.Nether && Level.OverworldLevel == null) return false;

			return Level.GetBlock(KnownPosition + new Vector3(0, 0.3f, 0)) is Portal;
		}

		public virtual void OnTick(Entity[] entities)
		{
			SeenEntities.Clear();
			UnseenEntities.Clear();
			Age++;

			HealthManager.OnTick();
		}

		private void CheckBlockCollisions()
		{
			// Check all blocks within entity BB
		}

		public virtual void SpawnEntity()
		{
			Level.AddEntity(this);

			IsSpawned = true;
		}

		public virtual void SpawnToPlayers(Player[] players)
		{
			var addEntity = McpeAddEntity.CreateObject();
			addEntity.entityType = EntityTypeId;
			addEntity.entityIdSelf = EntityId;
			addEntity.runtimeEntityId = EntityId;
			addEntity.x = KnownPosition.X;
			addEntity.y = KnownPosition.Y;
			addEntity.z = KnownPosition.Z;
			addEntity.pitch = KnownPosition.Pitch;
			addEntity.yaw = KnownPosition.Yaw;
			addEntity.headYaw = KnownPosition.HeadYaw;
			addEntity.metadata = GetMetadata();
			addEntity.speedX = Velocity.X;
			addEntity.speedY = Velocity.Y;
			addEntity.speedZ = Velocity.Z;
			addEntity.attributes = GetEntityAttributes();

			Level.RelayBroadcast(players, addEntity);
		}

		public virtual EntityAttributes GetEntityAttributes()
		{
			var attributes = new EntityAttributes();
			attributes["minecraft:attack_damage"] = new EntityAttribute
			{
				Name = "minecraft:attack_damage",
				MinValue = 0,
				MaxValue = 16,
				Value = AttackDamage
			};
			attributes["minecraft:absorption"] = new EntityAttribute
			{
				Name = "minecraft:absorption",
				MinValue = 0,
				MaxValue = float.MaxValue,
				Value = HealthManager.Absorption
			};
			attributes["minecraft:health"] = new EntityAttribute
			{
				Name = "minecraft:health",
				MinValue = 0,
				MaxValue = HealthManager.MaxHearts,
				Value = HealthManager.Hearts
			};
			attributes["minecraft:knockback_resistance"] = new EntityAttribute
			{
				Name = "minecraft:knockback_resistance",
				MinValue = 0,
				MaxValue = 1,
				Value = 0
			};
			attributes["minecraft:luck"] = new EntityAttribute
			{
				Name = "minecraft:luck",
				MinValue = -1025,
				MaxValue = 1024,
				Value = 0
			};
			attributes["minecraft:follow_range"] = new EntityAttribute
			{
				Name = "minecraft:follow_range",
				MinValue = 0,
				MaxValue = 2048,
				Value = 16
			};

			return attributes;
		}


		public virtual void DespawnEntity()
		{
			Level.RemoveEntity(this);
			IsSpawned = false;
		}

		public virtual void DespawnFromPlayers(Player[] players)
		{
			McpeRemoveEntity mcpeRemoveEntity = McpeRemoveEntity.CreateObject();
			mcpeRemoveEntity.entityIdSelf = EntityId;
			Level.RelayBroadcast(players, mcpeRemoveEntity);
		}

		public virtual void SetEntityData(MetadataDictionary message)
		{
		}

		public virtual void BroadcastSetEntityData()
		{
			BroadcastSetEntityData(GetMetadata());
		}

		public virtual void BroadcastSetEntityData(MetadataDictionary metadata)
		{
			McpeSetEntityData mcpeSetEntityData = McpeSetEntityData.CreateObject();
			mcpeSetEntityData.runtimeEntityId = EntityId;
			mcpeSetEntityData.metadata = metadata;
			Level?.RelayBroadcast(mcpeSetEntityData);
		}

		public virtual void BroadcastEntityEvent()
		{
			var entityEvent = McpeEntityEvent.CreateObject();
			entityEvent.runtimeEntityId = EntityId;
			entityEvent.eventId = (byte) (HealthManager.Health <= 0 ? 3 : 2);
			Level.RelayBroadcast(entityEvent);
		}


		public bool IsColliding(Entity other)
		{
			return IsColliding(GetBoundingBox(), other);
		}

		public bool IsColliding(BoundingBox bbox, Entity other)
		{
			//if (!Compare((int) KnownPosition.X, (int) other.KnownPosition.X, 5)) return false;
			//if (!Compare((int) KnownPosition.Z, (int) other.KnownPosition.Z, 5)) return false;
			if (!Compare((int) KnownPosition.X, (int) other.KnownPosition.X, 4)) return false;
			if (!Compare((int) KnownPosition.Z, (int) other.KnownPosition.Z, 4)) return false;
			if (!bbox.Intersects(other.GetBoundingBox())) return false;

			return true;
		}


		private bool Compare(int a, int b, int m)
		{
			a = a >> m;
			b = b >> m;
			return a == b || a == b - 1 || a == b + 1;
		}

		private Tuple<Vector3, BoundingBox> _bboxCache = new Tuple<Vector3, BoundingBox>(new Vector3(0, -1000, 0), new BoundingBox());

		public virtual BoundingBox GetBoundingBox()
		{
			//var pos = KnownPosition;
			////if (Math.Abs(pos.X - _bboxCache.Item1.X) < 0.01 && Math.Abs(pos.Y - _bboxCache.Item1.Y) < 0.01 && Math.Abs(pos.Z - _bboxCache.Item1.Z) < 0.01) return _bboxCache.Item2;

			//float halfWidth = (float) (Width/2);

			//var bbox = new BoundingBox(
			//	Vector3.Min(new Vector3(pos.X - halfWidth, pos.Y, pos.Z - halfWidth), new Vector3(pos.X + halfWidth, pos.Y, pos.Z + halfWidth)),
			//	Vector3.Max(new Vector3(pos.X - halfWidth, (float) (pos.Y - Height), pos.Z - halfWidth), new Vector3(pos.X + halfWidth, (float) (pos.Y + Height), pos.Z + halfWidth)));
			////_bboxCache = new Tuple<Vector3, BoundingBox>(KnownPosition, bbox);
			//return bbox;
			return GetBoundingBox(KnownPosition);
		}

		public virtual BoundingBox GetBoundingBox(Vector3 pos)
		{
			float halfWidth = (float) (Width / 2);

			var bbox = new BoundingBox(
				new Vector3(pos.X - halfWidth, pos.Y, pos.Z - halfWidth),
				new Vector3(pos.X + halfWidth, (float) (pos.Y + Height), pos.Z + halfWidth));
			return bbox;
		}

		public double DistanceToHorizontal(Entity entity)
		{
			if (entity == null) return -1;
			return Vector2.Distance(KnownPosition, entity.KnownPosition);
		}

		public double DistanceTo(Entity entity)
		{
			if (entity == null) return -1;
			return Vector3.Distance(KnownPosition, entity.KnownPosition);
		}

		public byte GetOppositeDirection()
		{
			return (byte) ((GetDirection() + 1) % 4);
		}

		public byte GetDirection()
		{
			return DirectionByRotationFlat(KnownPosition.Yaw);
		}

		public byte GetProperDirection()
		{
			return DirectionByRotationFlat(KnownPosition.Yaw) switch
			{
				0 => 0, // East
				1 => 2, // South
				2 => 1, // West
				3 => 3, // North
				_ => 0
			};
		}

		public enum Direction
		{
			South = 0,
			West = 1,
			North = 2,
			East = 3,
		}

		public enum ProperDirection
		{
			East = 0,
			West = 1,
			South = 2,
			North = 3,
		}

		public Direction GetDirectionEmum()
		{
			return (Direction) DirectionByRotationFlat(KnownPosition.Yaw);
		}


		public static byte DirectionByRotationFlat(float yaw)
		{
			byte direction = (byte) ((int) Math.Floor((yaw * 4F) / 360F + 0.5D) & 0x03);
			return direction switch
			{
				0 => 1,
				1 => 2,
				2 => 3,
				3 => 0,
				_ => 0
			};
		}

		public virtual void Knockback(Vector3 velocity)
		{
			Velocity += velocity;
			BroadcastMotion(!NoAi);
		}

		public void BroadcastMotion(bool forceMove = false)
		{
			//return;
			//if (NoAi || forceMove)
			//{
			//	McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
			//	motions.runtimeEntityId = EntityId;
			//	motions.velocity = Velocity;
			//	motions.Encode();
			//	Level.RelayBroadcast(motions);
			//}
		}

		public void BroadcastMove(bool forceMove = false)
		{
			//if (NoAi || forceMove)
			{
				//McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
				//moveEntity.runtimeEntityId = EntityId;
				//moveEntity.position = LastSentPosition;
				//moveEntity.flags = (short) (IsOnGround? 1: 0);
				//moveEntity.Encode();
				//Level.RelayBroadcast(moveEntity);

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
			}
		}


		public virtual Item[] GetDrops()
		{
			return new Item[] { };
		}

		public virtual void DoInteraction(int actionId, Player player)
		{
		}

		public virtual void DoItemInteraction(Player player, Item itemInHand)
		{
		}

		public virtual void DoMouseOverInteraction(byte actionId, Player player)
		{
			if (!string.IsNullOrEmpty(player.ButtonText))
			{
				player.ButtonText = null;
				player.SendSetEntityData();
			}
		}

		public virtual void Mount(Entity rider)
		{
		}

		public virtual void Unmount(Entity rider)
		{
		}

		public HashSet<Entity> SeenEntities { get; set; } = new HashSet<Entity>();
		public HashSet<Entity> UnseenEntities { get; set; } = new HashSet<Entity>();

		public virtual bool CanSee(Entity target)
		{
			if (SeenEntities.Contains(target)) return true;
			if (UnseenEntities.Contains(target)) return false;

			Vector3 entityPos = KnownPosition + new Vector3(0, (float) (this is Player ? 1.62f : Height), 0);
			Vector3 targetPos = target.KnownPosition + new Vector3(0, (float) (target is Player ? 1.62f : target.Height), 0);
			float distance = Vector3.Distance(entityPos, targetPos);

			Vector3 rayPos = entityPos;
			var direction = Vector3.Normalize(targetPos - entityPos);

			if (distance < direction.Length())
			{
				UnseenEntities.Add(target);
				return true;
			}

			do
			{
				if (Level.GetBlock(rayPos).IsSolid)
				{
					//Log.Debug($"{GetType()} can not see target");
					//BroadcastEntityEvent();
					UnseenEntities.Add(target);
					return false;
				}

				//var particle = new DustParticle(Level, Color.AntiqueWhite);
				//particle.Position = rayPos;
				//particle.Spawn();

				rayPos += direction;
			} while (distance > Vector3.Distance(entityPos, rayPos));

			SeenEntities.Add(target);
			return true;
		}
	}
}