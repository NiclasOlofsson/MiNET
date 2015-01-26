using System.IO;

namespace MiNET.Utils
{
	public abstract class MetadataEntry
	{
		public abstract byte Identifier { get; }
		public abstract string FriendlyName { get; }

		public abstract void FromStream(BinaryReader stream);
		public abstract void WriteTo(BinaryWriter stream, byte index);

		internal byte Index { get; set; }

		public static implicit operator MetadataEntry(byte value)
		{
			return new MetadataByte(value);
		}

		public static implicit operator MetadataEntry(short value)
		{
			return new MetadataShort(value);
		}

		public static implicit operator MetadataEntry(int value)
		{
			return new MetadataInt(value);
		}

		public static implicit operator MetadataEntry(float value)
		{
			return new MetadataFloat(value);
		}

		public static implicit operator MetadataEntry(string value)
		{
			return new MetadataString(value);
		}

		public static implicit operator MetadataEntry(ItemStack value)
		{
			return new MetadataSlot(value);
		}

		protected byte GetKey(byte index)
		{
			Index = index; // Cheat to get this for ToString
			return (byte) ((Identifier << 5) | (index & 0x1F));
		}
	}
}