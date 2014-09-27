using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using fNbt;

namespace MiNET.Network
{
	/// <summary>
	///     BinaryReader wrapper that takes care of reading primitives from an NBT stream,
	///     while taking care of endianness, string encoding, and skipping.
	/// </summary>
	public class NbtBinaryReader : BinaryReader
	{
		private readonly byte[] floatBuffer = new byte[sizeof (float)],
			doubleBuffer = new byte[sizeof (double)];

		private byte[] seekBuffer;
		private const int SeekBufferSize = 64*1024;
		private readonly bool bigEndian;


		public NbtBinaryReader(Stream input, bool bigEndian)
			: base(input)
		{
			this.bigEndian = bigEndian;
		}


		public NbtTagType ReadTagType()
		{
			NbtTagType type = (NbtTagType) ReadByte();
			if (type < NbtTagType.End || type > NbtTagType.IntArray)
			{
				throw new NbtFormatException("NBT tag type out of range: " + (int) type);
			}
			return type;
		}


		public override short ReadInt16()
		{
			if (BitConverter.IsLittleEndian == bigEndian)
			{
				return NbtBinaryWriter.Swap(base.ReadInt16());
			}
			else
			{
				return base.ReadInt16();
			}
		}


		public override int ReadInt32()
		{
			if (BitConverter.IsLittleEndian == bigEndian)
			{
				return NbtBinaryWriter.Swap(base.ReadInt32());
			}
			else
			{
				return base.ReadInt32();
			}
		}


		public override long ReadInt64()
		{
			if (BitConverter.IsLittleEndian == bigEndian)
			{
				return NbtBinaryWriter.Swap(base.ReadInt64());
			}
			else
			{
				return base.ReadInt64();
			}
		}


		public override float ReadSingle()
		{
			if (BitConverter.IsLittleEndian == bigEndian)
			{
				BaseStream.Read(floatBuffer, 0, sizeof (float));
				Array.Reverse(floatBuffer);
				return BitConverter.ToSingle(floatBuffer, 0);
			}
			return base.ReadSingle();
		}


		public override double ReadDouble()
		{
			if (BitConverter.IsLittleEndian == bigEndian)
			{
				BaseStream.Read(doubleBuffer, 0, sizeof (double));
				Array.Reverse(doubleBuffer);
				return BitConverter.ToDouble(doubleBuffer, 0);
			}
			return base.ReadDouble();
		}


		public override string ReadString()
		{
			short length = ReadInt16();
			if (length < 0)
			{
				throw new NbtFormatException("Negative string length given!");
			}
			byte[] stringData = ReadBytes(length);
			return Encoding.UTF8.GetString(stringData);
		}


		public void Skip(int bytesToSkip)
		{
			if (bytesToSkip < 0)
			{
				throw new ArgumentOutOfRangeException("bytesToSkip");
			}
			else if (BaseStream.CanSeek)
			{
				BaseStream.Position += bytesToSkip;
			}
			else if (bytesToSkip != 0)
			{
				if (seekBuffer == null)
					seekBuffer = new byte[SeekBufferSize];
				int bytesDone = 0;
				while (bytesDone < bytesToSkip)
				{
					int readThisTime = BaseStream.Read(seekBuffer, bytesDone, bytesToSkip - bytesDone);
					if (readThisTime == 0)
					{
						throw new EndOfStreamException();
					}
					bytesDone += readThisTime;
				}
			}
		}


		public void SkipString()
		{
			short length = ReadInt16();
			if (length < 0)
			{
				throw new NbtFormatException("Negative string length given!");
			}
			Skip(length);
		}


		public TagSelector Selector { get; set; }
	}

	/// <summary>
	///     DeflateStream wrapper that calculates Adler32 checksum of the written data,
	///     to allow writing ZLib header (RFC-1950).
	/// </summary>
	internal sealed class ZLibStream : DeflateStream
	{
		private int adler32A = 1,
			adler32B;

		private const int ChecksumModulus = 65521;

		public int Checksum
		{
			get { return ((adler32B*65536) + adler32A); }
		}


		private void UpdateChecksum(IList<byte> data, int offset, int length)
		{
			for (int counter = 0; counter < length; ++counter)
			{
				adler32A = (adler32A + (data[offset + counter]))%ChecksumModulus;
				adler32B = (adler32B + adler32A)%ChecksumModulus;
			}
		}


		public ZLibStream(Stream stream, CompressionMode mode, bool leaveOpen)
			: base(stream, mode, leaveOpen)
		{
		}


		public override void Write(byte[] array, int offset, int count)
		{
			UpdateChecksum(array, offset, count);
			base.Write(array, offset, count);
		}
	}
}
