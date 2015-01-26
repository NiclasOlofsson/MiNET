using System.IO;

namespace MiNET.Utils
{
	public class MetadataSlot : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 5; }
		}

		public override string FriendlyName
		{
			get { return "slot"; }
		}

		public ItemStack Value { get; set; }

		public static implicit operator MetadataSlot(ItemStack value)
		{
			return new MetadataSlot(value);
		}

		public MetadataSlot()
		{
		}

		public MetadataSlot(ItemStack value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader stream)
		{
			Value = ItemStack.FromStream(stream);
		}

		public override void WriteTo(BinaryWriter stream, byte index)
		{
			stream.Write(GetKey(index));
			Value.WriteTo(stream);
		}
	}
}