using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Craft.Net.Common;
using MiNET.Entities;
using MiNET.Utils;

namespace MiNET
{
	public enum DamageCause
	{
		[Description("{0} went MIA")] Unknown,
		[Description("{0} was pricked to death")] Contact,
		[Description("{0} was killed by {1}")] EntityAttack,
		[Description("{0}  was shot by {1}")] Projectile,
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
		[Description("{0} died magically")] Magic,
		[Description("{0} died a customized death")] Custom
	}

	public class HealthManager
	{
		public Entity Entity { get; set; }
		public int Health { get; set; }
		public short Air { get; set; }
		public bool IsDead { get; set; }
		public int FireTick { get; set; }
		public bool IsOnFire { get; set; }
		public DamageCause LastDamageCause { get; set; }
		public Entity LastDamageSource { get; set; }

		public HealthManager(Entity entity)
		{
			Entity = entity;
			ResetHealth();
		}

		public byte[] Export()
		{
			byte[] buffer;
			using (MemoryStream stream = new MemoryStream())
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, false);
				writer.Write(Health);
				writer.Write(Air);
				writer.Write(FireTick);
				writer.Write(IsOnFire);
				buffer = stream.GetBuffer();
			}
			return buffer;
		}

		public void Import(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				NbtBinaryReader reader = new NbtBinaryReader(stream, false);
				Health = reader.ReadInt32();
				Air = reader.ReadInt16();
				FireTick = reader.ReadInt32();
				IsOnFire = reader.ReadBoolean();
			}
		}

		public void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
		{
			if (LastDamageCause == DamageCause.Unknown) LastDamageCause = cause;

			LastDamageSource = source;

			//Untested code below, should work fine, however this is not sure yet.
			//	int Damage = ItemFactory.GetItem(sourcePlayer.PlayerInventory.ItemInHand.Value.Id).GetDamage();
			//	Health -= Damage - Entity.Armour;

			Health -= damage;

			var player = Entity as Player;
			if (player != null)
				player.SendSetHealth();
		}

		public void Kill()
		{
			if (IsDead) return;

			IsDead = true;
			Health = 0;
			var player = Entity as Player;
			if (player != null)
			{
				player.SendSetHealth();
				player.BroadcastEntityEvent();
				player.BroadcastSetEntityData();
			}

			Entity.DespawnEntity();
		}

		public void ResetHealth()
		{
			Health = 20;
			Air = 300;
			IsOnFire = false;
			FireTick = 0;
			IsDead = false;
			LastDamageCause = DamageCause.Unknown;
		}

		public void OnTick()
		{
			//TODO: Rewrite to fit all entities

			if (IsDead) return;

			if (Health <= 0)
			{
				Kill();
				return;
			}

			if (Entity.KnownPosition.Y < 0 && !IsDead)
			{
				TakeHit(null, 10);
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
						Health--;
						var player = Entity as Player;
						if (player != null)
						{
							player.SendSetHealth();
							player.BroadcastEntityEvent();
							player.BroadcastSetEntityData();
							LastDamageCause = DamageCause.Drowning;
						}
					}
				}

				if (IsOnFire)
				{
					IsOnFire = false;
					FireTick = 0;
					var player = Entity as Player;
					if (player != null)
						player.BroadcastSetEntityData();
				}
			}
			else
			{
				Air = 300;
			}

			if (!IsOnFire && IsInLava(Entity.KnownPosition))
			{
				FireTick = 300;
				IsOnFire = true;
				var player = Entity as Player;
				if (player != null)
					player.BroadcastSetEntityData();
			}

			if (IsOnFire)
			{
				FireTick--;
				if (FireTick <= 0)
				{
					IsOnFire = false;
				}

				if (Math.Abs(FireTick)%20 == 0)
				{
					Health--;
					var player = Entity as Player;
					if (player != null)
					{
						player.SendSetHealth();
						player.BroadcastEntityEvent();
						player.BroadcastSetEntityData();
						LastDamageCause = DamageCause.FireTick;
					}
				}
			}
		}

		private bool IsInWater(PlayerLocation playerPosition)
		{
			float y = playerPosition.Y + 1.62f;

			Coordinates3D waterPos = new Coordinates3D
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

		public static string GetDescription(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);
			if (attributes.Length > 0)
				return attributes[0].Description;
			else
				return value.ToString();
		}
	}
}