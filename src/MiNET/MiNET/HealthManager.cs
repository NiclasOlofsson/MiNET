using System;
using System.IO;
<<<<<<< HEAD
using MiNET.Items;
=======
using MiNET.Entities;
>>>>>>> origin/master
using MiNET.Utils;

namespace MiNET
{
	public class HealthManager
	{
		public Entity Entity { get; set; }
		public int Health { get; set; }
		public short Air { get; set; }
		public bool IsDead { get; set; }
		public int FireTick { get; set; }
		public bool IsOnFire { get; set; }
		public DamageCause LastDamageCause { get; set; }

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

		public void TakeHit(Player sourcePlayer)
		{
			//Untested code below, should work fine, however this is not sure yet.
		//	int Damage = ItemFactory.GetItem(sourcePlayer.InventoryManager.ItemInHand.Value.Id).GetDamage();
		//	Health -= Damage;
			Health--;
			var player = Entity as Player;
			if (player != null)
				player.SendSetHealth();
		}

		public void KillPlayer()
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
				player.Kill();
			}
		}

		public void ResetHealth()
		{
			Health = 20;
			Air = 300;
			IsOnFire = false;
			FireTick = 0;
			IsDead = false;
			LastDamageCause = DamageCause.died_from_an_unkown_death;
		}

		public void OnTick()
		{
			//TODO: Rewrite to fit all entities

			if (IsDead) return;

			if (Health <= 0)
			{
				KillPlayer();
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
							LastDamageCause = DamageCause.Drowned;
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
						LastDamageCause = DamageCause.Burned_to_death;
					}
				}
			}
		}

		private bool IsInWater(PlayerLocation playerPosition)
		{
			float y = playerPosition.Y + 1.62f;

			PlayerLocation waterPos = new PlayerLocation();
			waterPos.X = playerPosition.X;
			waterPos.Y = y;
			waterPos.Z = playerPosition.Z;
			var block = Entity.Level.GetBlock(waterPos);

			if (block != null && block.Id == 8 || block != null && block.Id == 9)
			{
				return y < Math.Floor(y) + 1 - ((1/9) - 0.1111111);
			}

			return false;
		}

		private bool IsInLava(PlayerLocation playerPosition)
		{
			var block = Entity.Level.GetBlock(playerPosition);

			if (block != null && block.Id == 10 || block != null && block.Id == 11)
			{
				return playerPosition.Y < Math.Floor(playerPosition.Y) + 1 - ((1/9) - 0.1111111);
			}

			return false;
		}

		private void SetHealth(int health)
		{
		}
	}

	public enum DamageCause
	{
		Got_killed_by_other_player,
		Drowned_in_lava,
		Drowned,
		Burned_to_death,
		died_from_an_unkown_death
	}
}