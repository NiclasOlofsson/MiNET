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
				new NbtList("Items", new NbtTagType()),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z)
			};

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
		}
	}
}