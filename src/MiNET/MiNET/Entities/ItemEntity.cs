using System;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;
using fNbt;

namespace MiNET.Entities
{
	public class ItemEntity : Mob
	{
		public Item Item { get; private set; }
		public byte Count { get; set; }
		public int PickupDelay { get; set; }
		public int TimeToLive { get; set; }
		public MetadataSlot metadataSlot { get; set; }
		
		public ItemEntity(Level level, Item item) : base(64, level)
		{
			Item = item;

			Height = 0.25;
			Width = 0.25;
			Length = 0.25;

			PickupDelay = 10;
			TimeToLive = 6000;
			metadataSlot = new MetadataSlot(new ItemStack((short) Item.Id, Count, Item.Metadata));
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = base.GetMetadata();
			metadata[15] = new MetadataByte(1);
			return metadata;
		}
		
		public void setExtraData(NbtCompound nbt){
 			metadataSlot.Value.ExtraData = nbt;
 		}

		public MetadataSlot GetMetadataSlot()
		{
			return metadataSlot;
		}

		public override void SpawnEntity()
		{
			Level.AddEntity(this);

			Random random = Level.Random;

			//double f = 0.7;
			//float xr = (float)(random.NextDouble() * f + (1.0 - f) * 0.5);
			//float yr = (float)(random.NextDouble() * f + (1.0 - f) * 0.5);
			//float zr = (float)(random.NextDouble() * f + (1.0 - f) * 0.5);

			float xr = 0;
			float yr = 0;
			float zr = 0;

			McpeAddItemEntity mcpeAddItemEntity = McpeAddItemEntity.CreateObject();
			mcpeAddItemEntity.entityId = EntityId;
			mcpeAddItemEntity.item = GetMetadataSlot();
			mcpeAddItemEntity.x = KnownPosition.X + xr;
			mcpeAddItemEntity.y = KnownPosition.Y + yr;
			mcpeAddItemEntity.z = KnownPosition.Z + zr;

			mcpeAddItemEntity.speedX = (float) Velocity.X;
			mcpeAddItemEntity.speedY = (float) Velocity.Y;
			mcpeAddItemEntity.speedZ = (float) Velocity.Z;
			Level.RelayBroadcast(mcpeAddItemEntity);

			BroadcastSetEntityData();

			IsSpawned = true;
		}

		public override void SpawnToPlayer(Player player)
		{
			McpeAddItemEntity mcpeAddItemEntity = McpeAddItemEntity.CreateObject();
			mcpeAddItemEntity.entityId = EntityId;
			mcpeAddItemEntity.item = GetMetadataSlot();
			mcpeAddItemEntity.x = KnownPosition.X;
			mcpeAddItemEntity.y = KnownPosition.Y;
			mcpeAddItemEntity.z = KnownPosition.Z;
			mcpeAddItemEntity.speedX = (float) Velocity.X;
			mcpeAddItemEntity.speedY = (float) Velocity.Y;
			mcpeAddItemEntity.speedZ = (float) Velocity.Z;
			player.SendPackage(mcpeAddItemEntity);

			BroadcastSetEntityData();
		}

		public override void DespawnFromPlayer(Player player)
		{
			base.DespawnFromPlayer(player);
		}

		public override void OnTick()
		{
			base.OnTick();

			TimeToLive--;
			PickupDelay--;

			if (TimeToLive <= 0)
			{
				DespawnEntity();
				return;
			}

			// Motion



			if (PickupDelay > 0) return;

			var players = Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				if (KnownPosition.DistanceTo(player.KnownPosition) <= 2)
				{
					if (player.GameMode == GameMode.Survival)
					{

						//Add the items to the inventory if posible
						if (player.Inventory.SetFirstEmptySlot((short)Item.Id, Count, Item.Metadata))
						{
							//BUG: If this is sent, the client crashes for some unknown reason.
							var takeItemEntity = McpeTakeItemEntity.CreateObject();
							takeItemEntity.entityId = EntityId;
							takeItemEntity.target = player.EntityId;

							Level.RelayBroadcast(takeItemEntity);
							DespawnEntity();
							break;
						}
					}
				}
			}
		}
	}
}
