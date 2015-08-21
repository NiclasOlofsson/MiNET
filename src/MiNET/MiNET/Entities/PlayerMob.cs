using System.Text;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class PlayerMob : Mob
	{
		public string Name { get; private set; }
		public Skin Skin { get; set; }
		public bool Silent { get; set; }
		public bool HideNameTag { get; set; }
		public bool NoAi { get; set; }

		public int Boots { get; set; }
		public int Leggings { get; set; }
		public int Chest { get; set; }
		public int Helmet { get; set; }

		public ItemStack ItemInHand { get; set; }

		public PlayerMob(string name, Level level) : base(63, level)
		{
			Width = 0.6;
			Length = 0.6;
			Height = 1.80;

			IsSpawned = false;

			Name = name;
			Skin = new Skin {Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 8192))};

			ItemInHand = new ItemStack();
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataByte((byte) (HealthManager.IsOnFire ? 1 : 0));
			metadata[1] = new MetadataShort(HealthManager.Air);
			metadata[2] = new MetadataString(NameTag ?? Name);
			metadata[3] = new MetadataByte(!HideNameTag);
			metadata[4] = new MetadataByte(Silent);
			metadata[7] = new MetadataInt(0); // Potion Color
			metadata[8] = new MetadataByte(0); // Potion Ambient
			metadata[15] = new MetadataByte(NoAi);
			metadata[16] = new MetadataByte(0); // Player flags
			metadata[17] = new MetadataLong(0);

			return metadata;
		}

		public override void SpawnEntity()
		{
			Level.AddEntity(this);
			IsSpawned = true;
			BroadcastSetEntityData();

			SpawnToAll();
		}

		public override void SpawnToPlayer(Player player)
		{
			{
				McpeAddPlayer message = McpeAddPlayer.CreateObject();
				message.uuid = player.ClientUuid;
				message.username = NameTag ?? Name;
				message.entityId = EntityId;
				message.x = KnownPosition.X;
				message.y = KnownPosition.Y;
				message.z = KnownPosition.Z;
				message.yaw = KnownPosition.Yaw;
				message.headYaw = KnownPosition.HeadYaw;
				message.pitch = KnownPosition.Pitch;
				message.metadata = GetMetadata().GetBytes();
				player.SendPackage(message);
			}
			{
				McpePlayerEquipment message = McpePlayerEquipment.CreateObject();
				message.entityId = EntityId;
				message.item = ItemInHand;
				message.slot = 0;
				player.SendPackage(message);
			}
			{
				McpePlayerArmorEquipment message = McpePlayerArmorEquipment.CreateObject();
				message.entityId = EntityId;
				message.helmet = (byte) (Helmet - 256);
				message.chestplate = (byte) (Chest - 256);
				message.leggings = (byte) (Leggings - 256);
				message.boots = (byte) (Boots - 256);
				player.SendPackage(message);
			}
		}

		protected virtual void SpawnToAll()
		{
			McpeAddPlayer message = McpeAddPlayer.CreateObject();
			message.uuid = new UUID();
			message.username = NameTag ?? Name;
			message.entityId = EntityId;
			message.x = KnownPosition.X;
			message.y = KnownPosition.Y;
			message.z = KnownPosition.Z;
			message.yaw = KnownPosition.Yaw;
			message.headYaw = KnownPosition.HeadYaw;
			message.pitch = KnownPosition.Pitch;
			message.metadata = GetMetadata().GetBytes();

			Level.RelayBroadcast(message);

			SendEquipment();

			SendArmor();
		}

		public override void DespawnFromPlayer(Player player)
		{
			McpeRemovePlayer mcpeRemovePlayer = McpeRemovePlayer.CreateObject();
			mcpeRemovePlayer.entityId = EntityId;
			player.SendPackage(mcpeRemovePlayer);
		}

		protected virtual void SendEquipment()
		{
			McpePlayerEquipment message = McpePlayerEquipment.CreateObject();
			message.entityId = EntityId;
			message.item = ItemInHand;
			message.slot = 0;
			Level.RelayBroadcast(message);
		}

		protected virtual void SendArmor()
		{
			McpePlayerArmorEquipment message = McpePlayerArmorEquipment.CreateObject();
			message.entityId = EntityId;
			message.helmet = (byte) (Helmet - 256);
			message.chestplate = (byte) (Chest - 256);
			message.leggings = (byte) (Leggings - 256);
			message.boots = (byte) (Boots - 256);
			Level.RelayBroadcast(message);
		}
	}
}
