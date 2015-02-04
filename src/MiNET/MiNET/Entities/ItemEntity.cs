using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class ItemEntity : Entity
	{
		public Item Item { get; private set; }
		public short Count { get; set; }
		public int PickupDelay { get; set; }

		public ItemEntity(Level level, Item item) : base(64, level)
		{
			Item = item;
		}

		public MetadataSlot GetMetadataSlot()
		{
			MetadataSlot metadataSlot = new MetadataSlot(new ItemStack((short) Item.Id, (byte) Item.Metadata, Count));
			return metadataSlot;
		}
	}
}