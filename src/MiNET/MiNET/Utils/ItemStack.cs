#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Collections.Generic;
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