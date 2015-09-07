using System.IO;
using MiNET.Items;

namespace MiNET.Utils
{
	public class ItemStack
	{
		public Item Item { get; set; }
		public byte Count { get; set; }

		public short Id
		{
			get { return (short) Item.Id; }
		}

		public short Metadata
		{
			get { return Item.Metadata; }
		}

		public ItemStack() : this(0, 0, 0)
		{
		}

		public ItemStack(short id) : this(id, 1, 0)
		{
		}

		public ItemStack(short id, byte count) : this(id, count, 0)
		{
		}

		public ItemStack(short id, byte count, short metadata) : this(ItemFactory.GetItem(id, metadata), count)
		{
		}

		public ItemStack(Item item, byte count)
		{
			Count = count;
			Item = item;
		}

		public static ItemStack FromStream(BinaryReader stream)
		{
			return new ItemStack(stream.ReadInt16(), stream.ReadByte(), stream.ReadInt16());
		}

		public void WriteTo(BinaryWriter stream)
		{
			stream.Write(Id);
			stream.Write(Count);
			stream.Write(Metadata);
		}
	}
}