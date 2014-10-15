using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Craft.Net.Common;
using MiNET.Network.Utils;

namespace MiNET.Network.Net
{
	/// Base package class
	public partial class Package : ICloneable
	{
		public byte Id;

		protected MemoryStream _buffer;
		private BinaryWriter _writer;
		private BinaryReader _reader;

		public Package()
		{
			_buffer = new MemoryStream();
			_reader = new BinaryReader(_buffer);
			_writer = new BinaryWriter(_buffer);
		}

		public void Write(byte value)
		{
			_writer.Write(value);
		}

		public byte ReadByte()
		{
			return _reader.ReadByte();
		}

		public sbyte ReadSByte()
		{
			return _reader.ReadSByte();
		}

		public void Write(sbyte value)
		{
			_writer.Write(value);
		}

		public void Write(byte[] value)
		{
			if (value == null) return;

			_writer.Write(value);
		}

		public byte[] ReadBytes(int count)
		{
			var readBytes = _reader.ReadBytes(count);
			if (readBytes.Length != count) throw new ArgumentOutOfRangeException();
			return readBytes;
		}

		public void Write(short value)
		{
			_writer.Write(BitConverter.GetBytes(value).Reverse().ToArray());
		}

		public short ReadShort()
		{
			return IPAddress.NetworkToHostOrder(_reader.ReadInt16());
		}

		public void Write(ushort value)
		{
			_writer.Write(BitConverter.GetBytes(value).Reverse().ToArray());
		}

		public ushort ReadUShort()
		{
			return (ushort) IPAddress.NetworkToHostOrder(_reader.ReadInt16());
		}

		public void Write(Int24 value)
		{
			_writer.Write(value.GetBytes());
		}

		public Int24 ReadLittle()
		{
			return new Int24(_reader.ReadBytes(3));
		}

		public void Write(int value)
		{
			_writer.Write(BitConverter.GetBytes(value).Reverse().ToArray());
		}

		public int ReadInt()
		{
			return IPAddress.NetworkToHostOrder(_reader.ReadInt32());
		}

		public void Write(uint value)
		{
			_writer.Write(BitConverter.GetBytes(value).Reverse().ToArray());
		}

		public uint ReadUInt()
		{
			return (uint) IPAddress.NetworkToHostOrder(_reader.ReadUInt32());
		}

		public void Write(long value)
		{
			_writer.Write(BitConverter.GetBytes(value).Reverse().ToArray());
		}

		public long ReadLong()
		{
			return IPAddress.NetworkToHostOrder(_reader.ReadInt64());
		}

		public void Write(ulong value)
		{
			_writer.Write(BitConverter.GetBytes(value).Reverse().ToArray());
		}

		public ulong ReadULong()
		{
			return (ulong) IPAddress.NetworkToHostOrder(_reader.ReadInt64());
		}

		public void Write(float value)
		{
			_writer.Write(BitConverter.GetBytes(value).Reverse().ToArray());
		}

		public float ReadFloat()
		{
			return BitConverter.ToSingle(BitConverter.GetBytes(_reader.ReadSingle()).Reverse().ToArray(), 0);
		}

		public void Write(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);

			Write((short) bytes.Length);
			Write(bytes);
		}

		public string ReadString()
		{
			short len = ReadShort();
			return Encoding.UTF8.GetString(ReadBytes(len));
		}

		public MetadataInts ReadMetadataInts()
		{
			MetadataInts metadata = new MetadataInts();
			short count = ReadShort();

			for (byte i = 0; i < count; i++)
			{
				metadata[i] = new MetadataInt(ReadInt());
			}

			return metadata;
		}

		public MetadataSlots ReadMetadataSlots()
		{
			short count = ReadShort();

			MetadataSlots metadata = new MetadataSlots();

			for (byte i = 0; i < count; i++)
			{
				MetadataSlot slot = new MetadataSlot(new ItemStack(ReadShort(), ReadSByte(), ReadShort()));
			}

			return metadata;
		}

		public void Write(MetadataDictionary metadata)
		{
			if (metadata == null)
			{
				Write((short) 0);
				return;
			}

			Write((short) metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				{
					MetadataSlot slot = metadata[i] as MetadataSlot;
					if (slot != null)
					{
						Write(slot.Value.Id);
						Write(slot.Value.Count);
						Write(slot.Value.Metadata);

						continue;
					}
				}

				{
					MetadataInt slot = metadata[i] as MetadataInt;
					if (slot != null)
					{
						Write(slot.Value);
						continue;
					}
				}
			}
		}


		protected virtual void EncodePackage()
		{
			_buffer.Position = 0;
			Write(Id);
		}

		public virtual byte[] Encode()
		{
			EncodePackage();
			_writer.Flush();
			_buffer.Position = 0;
			return _buffer.ToArray();
		}

		protected virtual void DecodePackage()
		{
			_buffer.Position = 0;
			Id = ReadByte();
		}

		public virtual void Decode(byte[] buffer)
		{
			_buffer.Position = 0;
			_buffer.SetLength(buffer.Length);
			_buffer.Write(buffer, 0, buffer.Length);
			_buffer.Position = 0;
			DecodePackage();
		}

		public virtual object Clone()
		{
			return MemberwiseClone();
		}

		public virtual T Clone<T>() where T : Package
		{
			return (T) Clone();
		}
	}
}
