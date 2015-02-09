using fNbt;

namespace MiNET.BlockEntities
{
	public class ChestBlockEntity : BlockEntity
	{
		public ChestBlockEntity() : base("Chest")
		{
		}

		public override NbtCompound GetCompound()
		{
			var compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtList("Items", new NbtCompound()),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z)
			};

			NbtList items = (NbtList) compound["Items"];
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

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
		}
	}
}