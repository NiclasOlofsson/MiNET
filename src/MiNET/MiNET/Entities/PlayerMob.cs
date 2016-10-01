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
		public Skin Skin { get; set; }

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

			NameTag = name;
			Skin = new Skin {Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 8192))};

			ItemInHand = new ItemAir();
		}

		public override void SpawnToPlayers(Player[] players)
		{
			{
				Player fake = new Player(null, null)
				{
					ClientUuid = Uuid,
					EntityId = EntityId,
					NameTag = NameTag,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerAddRecords {fake};
				Level.RelayBroadcast(players, playerList);
			}

			{
				McpeAddPlayer message = McpeAddPlayer.CreateObject();
				message.uuid = Uuid;
				message.username = NameTag;
				message.entityId = EntityId;
				message.runtimeEntityId = EntityId;
				message.x = KnownPosition.X;
				message.y = KnownPosition.Y;
				message.z = KnownPosition.Z;
				message.yaw = KnownPosition.Yaw;
				message.headYaw = KnownPosition.HeadYaw;
				message.pitch = KnownPosition.Pitch;
				message.metadata = GetMetadata();
				Level.RelayBroadcast(players, message);
			}
			{
				McpeMobEquipment message = McpeMobEquipment.CreateObject();
				message.entityId = EntityId;
				message.item = ItemInHand;
				message.slot = 0;
				Level.RelayBroadcast(players, message);
			}
			{
				McpeMobArmorEquipment armorEquipment = McpeMobArmorEquipment.CreateObject();
				armorEquipment.entityId = EntityId;
				armorEquipment.helmet = ItemFactory.GetItem(Helmet);
				armorEquipment.chestplate = ItemFactory.GetItem(Chest);
				armorEquipment.leggings = ItemFactory.GetItem(Leggings);
				armorEquipment.boots = ItemFactory.GetItem(Boots);
				Level.RelayBroadcast(players, armorEquipment);
			}

			{
				Player fake = new Player(null, null)
				{
					ClientUuid = Uuid,
					EntityId = EntityId,
					NameTag = NameTag,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerRemoveRecords {fake};
				Level.RelayBroadcast(players, playerList);
			}

			{
				McpeSetEntityData setEntityData = McpeSetEntityData.CreateObject();
				setEntityData.entityId = EntityId;
				setEntityData.metadata = GetMetadata();
				Level?.RelayBroadcast(players, setEntityData);
			}
		}

		public override void DespawnFromPlayers(Player[] players)
		{
			{
				Player fake = new Player(null, null)
				{
					ClientUuid = Uuid,
					EntityId = EntityId,
					NameTag = NameTag,
					Skin = Skin
				};

				McpePlayerList playerList = McpePlayerList.CreateObject();
				playerList.records = new PlayerRemoveRecords {fake};
				Level.RelayBroadcast(players, playerList);
			}

			McpeRemoveEntity mcpeRemovePlayer = McpeRemoveEntity.CreateObject();
			mcpeRemovePlayer.entityId = EntityId;
			Level.RelayBroadcast(players, mcpeRemovePlayer);
		}

		protected virtual void SendEquipment()
		{
			McpeMobEquipment message = McpeMobEquipment.CreateObject();
			message.entityId = EntityId;
			message.item = ItemInHand;
			message.slot = 0;
			Level.RelayBroadcast(message);
		}

		protected virtual void SendArmor()
		{
			McpeMobArmorEquipment armorEquipment = McpeMobArmorEquipment.CreateObject();
			armorEquipment.entityId = EntityId;
			armorEquipment.helmet = ItemFactory.GetItem(Helmet);
			armorEquipment.chestplate = ItemFactory.GetItem(Chest);
			armorEquipment.leggings = ItemFactory.GetItem(Leggings);
			armorEquipment.boots = ItemFactory.GetItem(Boots);
			Level.RelayBroadcast(armorEquipment);
		}
	}
}