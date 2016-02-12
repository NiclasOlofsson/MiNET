using System.Collections.Generic;
using System.IO;
using fNbt;
using MiNET.Items;

namespace MiNET.Utils
{
	//public class ItemStack
	//{
	//	public Item Item { get; set; }
	//	public byte Count { get; set; }
	//	public NbtCompound ExtraData { get; set; }

	//	public short Id => (short) Item.Id;
	//	public short Metadata => Item.Metadata;

	//	public ItemStack() : this(0, 0, 0)
	//	{
	//	}

	//	public ItemStack(short id) : this(id, 1, 0)
	//	{
	//	}

	//	public ItemStack(short id, byte count) : this(id, count, 0)
	//	{
	//	}

	//	public ItemStack(short id, byte count, short metadata) : this(ItemFactory.GetItem(id, metadata), count)
	//	{
	//	}

	//	public ItemStack(Item item, byte count)
	//	{
	//		Count = count;
	//		Item = item;
	//	}

	//	public static ItemStack FromStream(BinaryReader stream)
	//	{
	//		return new ItemStack(stream.ReadInt16(), stream.ReadByte(), stream.ReadInt16());
	//	}

	//	public void WriteTo(BinaryWriter stream)
	//	{
	//		stream.Write(Id);
	//		stream.Write(Count);
	//		stream.Write(Metadata);
	//	}

	//	protected bool Equals(ItemStack other)
	//	{
	//		return Equals(Item, other.Item) && Count == other.Count;
	//	}

	//	public override bool Equals(object obj)
	//	{
	//		if (ReferenceEquals(null, obj)) return false;
	//		if (ReferenceEquals(this, obj)) return true;
	//		if (obj.GetType() != this.GetType()) return false;
	//		return Equals((ItemStack) obj);
	//	}

	//	public override int GetHashCode()
	//	{
	//		unchecked
	//		{
	//			return ((Item != null ? Item.GetHashCode() : 0)*397) ^ Count.GetHashCode();
	//		}
	//	}

	//	public override string ToString()
	//	{
	//		return $"Id={Id}, Metadata={Metadata}, Count={Count}, NBT={ExtraData}";
	//	}

	//}

	public class ItemStacks : List<Item>
	{
	}
}