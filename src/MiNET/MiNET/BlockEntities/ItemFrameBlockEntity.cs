using System.Collections.Generic;
using fNbt;
using log4net;
using MiNET.Items;

namespace MiNET.BlockEntities
{
	public class ItemFrameBlockEntity : BlockEntity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemFrameBlockEntity));

		private NbtCompound Compound { get; set; }

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
		}

		public void SetItem(Item item)
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

			var comp = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z),
			};

			comp["Item"] = newItem;
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