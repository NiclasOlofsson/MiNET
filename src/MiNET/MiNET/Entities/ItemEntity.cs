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
			TimeToLive = 20*10;
		}

		public MetadataSlot GetMetadataSlot()
		{
			MetadataSlot metadataSlot = new MetadataSlot(new ItemStack((short) Item.Id, (byte) Item.Metadata, Count));
			return metadataSlot;
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
				if (KnownPosition.DistanceTo(player.KnownPosition) <= 1)
				{
					Level.RelayBroadcast(new McpeTakeItemEntity()
					{
						entityId = player.EntityId,
						target = EntityId
					});
					player.Inventory.SetFirstEmptySlot((short)Item.Id, (byte)Count, Item.Metadata); //Add the items to the inventory

					DespawnEntity();
					break;
				}
			}
		}
	}
}