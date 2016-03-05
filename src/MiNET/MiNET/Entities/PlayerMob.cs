using System;
using System.Text;
using MiNET.Items;
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

		public short Boots { get; set; }
		public short Leggings { get; set; }
		public short Chest { get; set; }
		public short Helmet { get; set; }

		public Item ItemInHand { get; set; }

		public PlayerMob(string name, Level level) : base(63, level)
		{
			Uuid = new UUID(Guid.NewGuid().ToByteArray());

			Width = 0.6;
			Length = 0.6;
			Height = 1.80;

			IsSpawned = false;

			Name = name;
			Skin = new Skin {Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 8192))};

			ItemInHand = new ItemAir();
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

		public override void SpawnToPlayers(Player[] players)
		{
			{
				Player fake = new Player(null, null)
				{
					ClientUuid = Uuid,
					EntityId = EntityId,
					NameTag = NameTag ?? Name,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords {fake};
				Level.RelayBroadcast(this, players, playerList);
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
				Level.RelayBroadcast(this, players, message);
			}
			{
				McpePlayerEquipment message = McpePlayerEquipment.CreateObject();
				message.entityId = EntityId;
				message.item = ItemInHand;
				message.slot = 0;
				Level.RelayBroadcast(this, players, message);
			}
			{
				McpePlayerArmorEquipment armorEquipment = McpePlayerArmorEquipment.CreateObject();
				armorEquipment.entityId = EntityId;
				armorEquipment.helmet = ItemFactory.GetItem(Helmet);
				armorEquipment.chestplate = ItemFactory.GetItem(Chest);
				armorEquipment.leggings = ItemFactory.GetItem(Leggings);
				armorEquipment.boots = ItemFactory.GetItem(Boots);
				Level.RelayBroadcast(this, players, armorEquipment);
			}

			{
				Player fake = new Player(null, null)
				{
					ClientUuid = Uuid,
					EntityId = EntityId,
					NameTag = NameTag ?? Name,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerRemoveRecords {fake};
				Level.RelayBroadcast(this, players, playerList);
			}

			// Probably not needed
			BroadcastSetEntityData();
		}

		public override void DespawnFromPlayers(Player[] players)
		{
			{
				Player fake = new Player(null, null)
				{
					ClientUuid = Uuid,
					EntityId = EntityId,
					NameTag = NameTag ?? Name,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerRemoveRecords { fake };
				Level.RelayBroadcast(this, players, playerList);
			}

			McpeRemovePlayer mcpeRemovePlayer = McpeRemovePlayer.CreateObject();
			mcpeRemovePlayer.entityId = EntityId;
			mcpeRemovePlayer.clientUuid = Uuid;
			Level.RelayBroadcast(this, players, mcpeRemovePlayer);
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
			armorEquipment.helmet = ItemFactory.GetItem(Helmet);
			armorEquipment.chestplate = ItemFactory.GetItem(Chest);
			armorEquipment.leggings = ItemFactory.GetItem(Leggings);
			armorEquipment.boots = ItemFactory.GetItem(Boots);
			Level.RelayBroadcast(armorEquipment);
		}
	}
}