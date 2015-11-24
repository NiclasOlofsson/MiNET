using System;
using System.Resources;
using System.Text;
using System.Threading;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class PlayerMob : Mob
	{
		public UUID Uuid { get; private set; }
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
			Uuid = new UUID(Guid.NewGuid().ToByteArray());

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
			//metadata[17] = new MetadataIntCoordinates(0, 0, 0);

			return metadata;
		}

		public override void SpawnEntity()
		{
			Level.AddEntity(this);
			SpawnToAll();
			IsSpawned = true;

			BroadcastSetEntityData();
		}

		public override void SpawnToPlayer(Player player)
		{
			{
				Player fake = new Player(null, null, 0)
				{
					ClientUuid = Uuid,
					EntityId = EntityId,
					NameTag = NameTag ?? Name,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords { fake };
				player.SendPackage(playerList);
				//player.SendDirectPackage(playerList);
			}

			{
				McpeAddPlayer message = McpeAddPlayer.CreateObject();
				message.uuid = Uuid;
				message.username = NameTag ?? Name;
				message.entityId = EntityId;
				message.x = KnownPosition.X;
				message.y = KnownPosition.Y;
				message.z = KnownPosition.Z;
				message.yaw = KnownPosition.Yaw;
				message.headYaw = KnownPosition.HeadYaw;
				message.pitch = KnownPosition.Pitch;
				message.metadata = GetMetadata();
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
				McpePlayerArmorEquipment armorEquipment = McpePlayerArmorEquipment.CreateObject();
				armorEquipment.entityId = EntityId;
				armorEquipment.helmet = new MetadataSlot(new ItemStack());
				armorEquipment.chestplate = new MetadataSlot(new ItemStack());
				armorEquipment.leggings = new MetadataSlot(new ItemStack());
				armorEquipment.boots = new MetadataSlot(new ItemStack());
				player.SendPackage(armorEquipment);
			}
		}

		protected virtual void SpawnToAll()
		{
			{
				Player fake = new Player(null, null, 0)
				{
					ClientUuid = Uuid,
					EntityId = EntityId,
					NameTag = NameTag ?? Name,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords { fake };
				Level.RelayBroadcast(playerList);
			}

			McpeAddPlayer message = McpeAddPlayer.CreateObject();
			message.uuid = Uuid;
			message.username = NameTag ?? Name;
			message.entityId = EntityId;
			message.x = KnownPosition.X;
			message.y = KnownPosition.Y;
			message.z = KnownPosition.Z;
			message.yaw = KnownPosition.Yaw;
			message.headYaw = KnownPosition.HeadYaw;
			message.pitch = KnownPosition.Pitch;
			message.metadata = GetMetadata();

			Level.RelayBroadcast(message);

			SendEquipment();

			SendArmor();
		}

		public override void DespawnFromPlayer(Player player)
		{
			{
				Player fake = new Player(null, null, 0)
				{
					ClientUuid = Uuid,
					EntityId = EntityId,
					NameTag = NameTag ?? Name,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerRemoveRecords { fake };
				player.SendPackage(playerList);
			}

			McpeRemovePlayer mcpeRemovePlayer = McpeRemovePlayer.CreateObject();
			mcpeRemovePlayer.entityId = EntityId;
			mcpeRemovePlayer.clientUuid = Uuid;
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
			McpePlayerArmorEquipment armorEquipment = McpePlayerArmorEquipment.CreateObject();
			armorEquipment.entityId = EntityId;
			armorEquipment.helmet = new MetadataSlot(new ItemStack());
			armorEquipment.chestplate = new MetadataSlot(new ItemStack());
			armorEquipment.leggings = new MetadataSlot(new ItemStack());
			armorEquipment.boots = new MetadataSlot(new ItemStack());
			Level.RelayBroadcast(armorEquipment);
		}
	}
}