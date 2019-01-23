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
using fNbt;
using MiNET.Items;

namespace MiNET.BlockEntities
{
	public class ItemFrameBlockEntity : BlockEntity
	{
		private NbtCompound Compound { get; set; }
		public Item ItemInFrame { get; private set; }
		public int Rotation { get; private set; }
		public float DropChance { get; private set; }

		public ItemFrameBlockEntity() : base("ItemFrame")
		{
			Compound = new NbtCompound(string.Empty)
			{
				new NbtCompound("Item", new NbtCompound("Item")),
				new NbtString("id", Id),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z),
			};

			var item = (NbtCompound) Compound["Item"];
			item.Add(new NbtShort("id", 0));
			item.Add(new NbtShort("Damage", 0));
			item.Add(new NbtByte("Count", 0));

			//Log.Error($"New ItemFrame block entity: {Compound}");
		}

		public override NbtCompound GetCompound()
		{
			Compound["x"] = new NbtInt("x", Coordinates.X);
			Compound["y"] = new NbtInt("y", Coordinates.Y);
			Compound["z"] = new NbtInt("z", Coordinates.Z);

			return Compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			Compound = compound;
			if (compound.TryGet("Item", out var item))
			{
				var id = item["id"].ShortValue;
				var damage = item["Damage"].ShortValue;
				var count = item["Count"].ShortValue;
				ItemInFrame = ItemFactory.GetItem(id, damage, count);
			}
			if (compound.TryGet("ItemRotation", out var rotation))
			{
				Rotation = rotation.ByteValue;
			}
			if (compound.TryGet("ItemDropChance", out var dropChance))
			{
				DropChance = dropChance.FloatValue;
			}
		}

		public void SetItem(Item item, int rotation)
		{
			ItemInFrame = item;
			Rotation = rotation;

			var comp = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z),
				new NbtFloat("ItemDropChance", DropChance),
				new NbtByte("ItemRotation", (byte) Rotation),
			};

			if (item != null)
			{
				var newItem = new NbtCompound("Item")
				{
					new NbtShort("id", item.Id),
					new NbtShort("Damage", item.Metadata),
					new NbtByte("Count", 1)
				};

				if (item.ExtraData != null)
				{
					var newTag = (NbtTag) item.ExtraData.Clone();
					newTag.Name = "tag";
					newItem.Add(newTag);
				}

				comp["Item"] = newItem;
			}
			else
			{
				comp.Remove("Item");
			}

			Compound = comp;
		}

		public override List<Item> GetDrops()
		{
			List<Item> slots = new List<Item>();

			var itemComp = Compound["Item"] as NbtCompound;
			if (itemComp == null) return slots;

			Item item = ItemFactory.GetItem(itemComp["id"].ShortValue, itemComp["Damage"].ShortValue, itemComp["Count"].ByteValue);
			slots.Add(item);

			return slots;
		}
	}
}