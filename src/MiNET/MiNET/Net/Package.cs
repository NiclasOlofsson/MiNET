using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MiNET.Utils;

namespace MiNET.Net
{
	/// Base package class
	public abstract partial class Package : ICloneable
	{
		public virtual void PutPool()
		{
			//Console.WriteLine("Put pool for object wrong: " + this.GetType().Name);
		}

		protected object _bufferSync = new object();
		private bool _isEncoded = false;
		private byte[] _encodedMessage;

		public byte Id;

		protected MemoryStream _buffer;
		private BinaryWriter _writer;
		private BinaryReader _reader;
		private Stopwatch _timer = new Stopwatch();

		public Package()
		{
			_buffer = new MemoryStream();
			_reader = new BinaryReader(_buffer);
			_writer = new BinaryWriter(_buffer);
			Timer.Start();
		}

		public Stopwatch Timer
		{
			get { return _timer; }
		}

		public void Write(byte value)
		{
			_writer.Write(value);
		}

		public byte ReadByte()
		{
			return _reader.ReadByte();
		}

		public void Write(sbyte value)
		{
			_writer.Write(value);
		}

		public sbyte ReadSByte()
		{
			return _reader.ReadSByte();
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
			_writer.Write(Endian.SwapInt16(value));
		}

		public short ReadShort()
		{
			return Endian.SwapInt16(_reader.ReadInt16());
		}

		public void Write(ushort value)
		{
			_writer.Write(Endian.SwapUInt16(value));
		}

		public ushort ReadUShort()
		{
			return Endian.SwapUInt16(_reader.ReadUInt16());
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
			_writer.Write(Endian.SwapInt32(value));
		}

		public int ReadInt()
		{
			return Endian.SwapInt32(_reader.ReadInt32());
		}

		public void Write(uint value)
		{
			_writer.Write(Endian.SwapUInt32(value));
		}

		public uint ReadUInt()
		{
			return Endian.SwapUInt32(_reader.ReadUInt32());
		}

		public void Write(long value)
		{
			_writer.Write(Endian.SwapInt64(value));
		}

		public long ReadLong()
		{
			return Endian.SwapInt64(_reader.ReadInt64());
		}

		public void Write(ulong value)
		{
			_writer.Write(Endian.SwapUInt64(value));
		}

		public ulong ReadULong()
		{
			return Endian.SwapUInt64(_reader.ReadUInt64());
		}

		public void Write(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);

			_writer.Write(bytes[3]);
			_writer.Write(bytes[2]);
			_writer.Write(bytes[1]);
			_writer.Write(bytes[0]);
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

		public void Write(MetadataInts metadata)
		{
			if (metadata == null)
			{
				Write((short) 0);
				return;
			}

			Write((short) metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				MetadataInt slot = metadata[i] as MetadataInt;
				if (slot != null)
				{
					Write(slot.Value);
				}
			}
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

		public void Write(MetadataSlots metadata)
		{
			if (metadata == null)
			{
				Write((short) 0);
				return;
			}

			Write((short) metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				MetadataSlot slot = metadata[i] as MetadataSlot;
				if (slot != null)
				{
					Write(slot.Value.Id);
					Write(slot.Value.Count);
					Write(slot.Value.Metadata);
				}
			}
		}

		public MetadataSlots ReadMetadataSlots()
		{
			short count = ReadShort();

			MetadataSlots metadata = new MetadataSlots();

			for (byte i = 0; i < count; i++)
			{
				metadata[i] = new MetadataSlot(new ItemStack(ReadShort(), ReadSByte(), ReadShort()));
			}

			return metadata;
		}

		protected virtual void EncodePackage()
		{
			_buffer.Position = 0;
			Write(Id);
		}

		public void Reset()
		{
			_encodedMessage = null;
			_writer.Flush();
			_buffer.SetLength(0);
			_buffer.Position = 0;
			_timer.Reset();
			_isEncoded = false;
		}

		public void SetEncodedMessage(byte[] encodedMessage)
		{
			_encodedMessage = encodedMessage;
			_isEncoded = true;
		}

		public virtual byte[] Encode()
		{
			if (_isEncoded) return _encodedMessage;

			EncodePackage();
			_writer.Flush();
			_buffer.Position = 0;
			_encodedMessage = _buffer.ToArray();
			_isEncoded = true;
			return _encodedMessage;
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

		public void CloneReset()
		{
			_buffer = new MemoryStream();
			_reader = new BinaryReader(_buffer);
			_writer = new BinaryWriter(_buffer);
			Timer.Start();
		}

		public virtual object Clone()
		{
			Package clone = (Package) MemberwiseClone();
			clone.CloneReset();
			return clone;
		}

		public virtual T Clone<T>() where T : Package
		{
			return (T) Clone();
		}

	}
}