using System;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class ItemEntity : Entity
	{
		public Item Item { get; private set; }
		public short Count { get; set; }
		public int PickupDelay { get; set; }
		public int TimeToLive { get; set; }

		public ItemEntity(Level level, Item item) : base(64, level)
		{
			Item = item;

			Height = 0.25;
			Width = 0.25;
			Length = 0.25;

			PickupDelay = 10;
			TimeToLive = 6000;
		}

		public MetadataSlot GetMetadataSlot()
		{
			MetadataSlot metadataSlot = new MetadataSlot(new ItemStack((short) Item.Id, (byte) Item.Metadata, Count));
			return metadataSlot;
		}

		public override void SpawnEntity()
		{
			Level.AddEntity(this);

			Random random = new Random();

			float f = 0.7F;
			float xr = (float) (random.NextDouble()*f + (1.0F - f)*0.5D);
			float yr = (float) (random.NextDouble()*f + (1.0F - f)*0.5D);
			float zr = (float) (random.NextDouble()*f + (1.0F - f)*0.5D);

			McpeAddItemEntity mcpeAddItemEntity = McpeAddItemEntity.CreateObject();
			mcpeAddItemEntity.entityId = EntityId;
			mcpeAddItemEntity.item = GetMetadataSlot();
			mcpeAddItemEntity.x = KnownPosition.X + xr;
			mcpeAddItemEntity.y = KnownPosition.Y + yr;
			mcpeAddItemEntity.z = KnownPosition.Z + zr;
			Level.RelayBroadcast(mcpeAddItemEntity);

			IsSpawned = true;
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

			if (PickupDelay > 0) return;

			var players = Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				if (KnownPosition.DistanceTo(player.KnownPosition) <= 2)
				{
					//BUG: If this is sent, the client crashes for some unknown reason.
					Level.RelayBroadcast(new McpeTakeItemEntity()
					{
						//entityId = player.EntityId,
						//target = EntityId
						entityId = EntityId,
						target = player.EntityId
					});
					if (player.GameMode == GameMode.Survival)
					{
						player.Inventory.SetFirstEmptySlot((short) Item.Id, (byte) Count, Item.Metadata); //Add the items to the inventory
					}
					DespawnEntity();
					break;
				}
			}
		}
	}
}