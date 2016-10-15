using System.IO;
using MiNET.Items;

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

		public Item Value { get; set; }

		public MetadataSlot()
		{
		}

		public MetadataSlot(Item value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader reader)
		{
			var id = reader.ReadInt16();
			var count = reader.ReadByte();
			var metadata = reader.ReadInt16();
			Value = new Item(id, metadata, count);
		}

		public override void WriteTo(BinaryWriter stream)
		{
			stream.Write(Value.Id);
			stream.Write(Value.Count);
			stream.Write(Value.Metadata);
		}
	}
}