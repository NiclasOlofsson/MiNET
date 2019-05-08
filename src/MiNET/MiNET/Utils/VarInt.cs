#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using MiNET.Net;
using ByteArray = System.Array;

namespace MiNET.Utils
{
	public static class VarInt
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(VarInt));

		private static uint EncodeZigZag32(int n)
		{
			// Note:  the right-shift must be arithmetic
			return (uint) ((n << 1) ^ (n >> 31));
		}

		private static int DecodeZigZag32(uint n)
		{
			return (int) (n >> 1) ^ -(int) (n & 1);
		}

		private static ulong EncodeZigZag64(long n)
		{
			return (ulong) ((n << 1) ^ (n >> 63));
		}

		private static long DecodeZigZag64(ulong n)
		{
			return (long) (n >> 1) ^ -(long) (n & 1);
		}

		private static uint ReadRawVarInt32(Stream buf, int maxSize)
		{
			uint result = 0;
			int j = 0;
			int b0;

			do
			{
				b0 = buf.ReadByte(); // -1 if EOS
				if (b0 < 0) throw new EndOfStreamException("Not enough bytes for VarInt");

				result |= (uint) (b0 & 0x7f) << j++ * 7;

				if (j > maxSize)
				{
					throw new OverflowException("VarInt too big");
				}
			} while ((b0 & 0x80) == 0x80);

			return result;
		}

		private static ulong ReadRawVarInt64(Stream buf, int maxSize, bool printBytes = false)
		{
			List<byte> bytes = new List<byte>();

			ulong result = 0;
			int j = 0;
			int b0;

			do
			{
				b0 = buf.ReadByte(); // -1 if EOS
				bytes.Add((byte) b0);
				if (b0 < 0) throw new EndOfStreamException("Not enough bytes for VarInt");

				result |= (ulong) (b0 & 0x7f) << j++ * 7;

				if (j > maxSize)
				{
					throw new OverflowException("VarInt too big");
				}
			} while ((b0 & 0x80) == 0x80);

			byte[] byteArray = bytes.ToArray();

			if (printBytes) Log.Debug($"Long bytes: {Packet.HexDump(byteArray)} ");

			return result;
		}

		private static void WriteRawVarInt32(Stream buf, uint value)
		{
			while ((value & -128) != 0)
			{
				buf.WriteByte((byte) ((value & 0x7F) | 0x80));
				value >>= 7;
			}

			buf.WriteByte((byte) value);
		}

		private static void WriteRawVarInt64(Stream buf, ulong value)
		{
			while ((value & 0xFFFFFFFFFFFFFF80) != 0)
			{
				buf.WriteByte((byte) ((value & 0x7F) | 0x80));
				value >>= 7;
			}

			buf.WriteByte((byte) value);
		}

		// Int

		public static void WriteInt32(Stream stream, int value)
		{
			WriteRawVarInt32(stream, (uint) value);
		}

		public static int ReadInt32(Stream stream)
		{
			return (int) ReadRawVarInt32(stream, 5);
		}

		public static void WriteSInt32(Stream stream, int value)
		{
			WriteRawVarInt32(stream, EncodeZigZag32(value));
		}

		public static int ReadSInt32(Stream stream)
		{
			return DecodeZigZag32(ReadRawVarInt32(stream, 5));
		}

		public static void WriteUInt32(Stream stream, uint value)
		{
			WriteRawVarInt32(stream, value);
		}

		public static uint ReadUInt32(Stream stream)
		{
			return ReadRawVarInt32(stream, 5);
		}

		// Long

		public static void WriteInt64(Stream stream, long value)
		{
			WriteRawVarInt64(stream, (ulong) value);
		}

		public static long ReadInt64(Stream stream, bool printBytes = false)
		{
			return (long) ReadRawVarInt64(stream, 10, printBytes);
		}

		public static void WriteSInt64(Stream stream, long value)
		{
			WriteRawVarInt64(stream, EncodeZigZag64(value));
		}

		public static long ReadSInt64(Stream stream)
		{
			return DecodeZigZag64(ReadRawVarInt64(stream, 10));
		}

		public static void WriteUInt64(Stream stream, ulong value)
		{
			WriteRawVarInt64(stream, value);
		}

		public static ulong ReadUInt64(Stream stream)
		{
			return ReadRawVarInt64(stream, 10);
		}
	}
}