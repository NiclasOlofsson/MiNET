using fNbt;

namespace MiNET.BlockEntities
{
	public class Sign : BlockEntity
	{
		public string Text1 { get; set; }
		public string Text2 { get; set; }
		public string Text3 { get; set; }
		public string Text4 { get; set; }

		public Sign() : base("Sign")
		{
			Text1 = string.Empty;
			Text2 = string.Empty;
			Text3 = string.Empty;
			Text4 = string.Empty;
		}

		public override NbtCompound GetCompound()
		{
			var compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtString("Text1", Text1 ?? string.Empty),
				new NbtString("Text2", Text2 ?? string.Empty),
				new NbtString("Text3", Text3 ?? string.Empty),
				new NbtString("Text4", Text4 ?? string.Empty),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z)
			};

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			Text1 = GetTextValue(compound, "Text1");
			Text2 = GetTextValue(compound, "Text2");
			Text3 = GetTextValue(compound, "Text3");
			Text4 = GetTextValue(compound, "Text4");
		}

		private string GetTextValue(NbtCompound compound, string key)
		{
			NbtString text;
			compound.TryGet(key, out text);
			return text != null ? (text.StringValue ?? string.Empty) : string.Empty;
		}
	}
}