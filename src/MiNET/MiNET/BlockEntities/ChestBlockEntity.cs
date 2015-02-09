using fNbt;

namespace MiNET.BlockEntities
{
	public class ChestBlockEntity : BlockEntity
	{
		private NbtCompound Compound { get; set; }

		public ChestBlockEntity() : base("Chest")
		{
			Compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtList("Items", new NbtCompound()),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z)
			};

			NbtList items = (NbtList) Compound["Items"];
			for (byte i = 0; i < 27; i++)
			{
				items.Add(new NbtCompound("")
				{
					new NbtByte("Count", 0),
					new NbtByte("Slot", i),
					new NbtShort("id", 0),
					new NbtByte("Damage", 0),
				});
			}
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

			if (Compound["Items"] == null)
			{
				NbtList items = new NbtList("Items");
				for (byte i = 0; i < 27; i++)
				{
					items.Add(new NbtCompound("")
					{
						new NbtByte("Count", 0),
						new NbtByte("Slot", i),
						new NbtShort("id", 0),
						new NbtByte("Damage", 0),
					});
				}
				Compound["Items"] = items;
			}
		}
	}
}