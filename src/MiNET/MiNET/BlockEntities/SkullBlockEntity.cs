using fNbt;

namespace MiNET.BlockEntities
{
	public class SkullBlockEntity : BlockEntity
	{
		public byte Rotation { get; set; }
		public byte SkullType { get; set; }

		public SkullBlockEntity() : base("Skull")
		{
		}

		public override NbtCompound GetCompound()
		{
			var compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z),
				new NbtByte("SkullType", SkullType),
				new NbtByte("Rot", Rotation)
			};

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
		}
	}
}