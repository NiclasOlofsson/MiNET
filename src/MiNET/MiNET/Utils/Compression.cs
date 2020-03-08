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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.IO;
using System.IO.Compression;
using log4net;

namespace MiNET.Utils
{
	public class Compression
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Compression));

		//private static ReadOnlySpan<byte> WriteRawVarInt32(uint value)
		//{
		//	var buf = new Span<byte>(new byte[10]);
		//	int i = 0;
		//	while ((value & -128) != 0)
		//	{
		//		buf[i++] = ((byte) ((value & 0x7F) | 0x80));
		//		value >>= 7;
		//	}

		//	buf[i] = (byte) value;

		//	return buf.Slice(0, i + 1);
		//}

		public static byte[] Compress(Memory<byte> input, bool writeLen = false, CompressionLevel compressionLevel = CompressionLevel.Fastest)
		{
			//if (/*!useOld && */compressionLevel == CompressionLevel.NoCompression)
			//{
			//	// header 2 bytes
			//	// compressed bytes
			//	// - header (1 byte) 0x01
			//	// - size total (2 bytes)
			//	// - ~size total (2 bytes)
			//	// - uncompressed data

			//	int i = 0;
			//	var output = new Span<byte>(new byte[2 + 1 + 2 + 2 + input.Length + 10 + 4]);
			//	output[i++] = 0x78;
			//	output[i++] = 0x01;
			//	output[i++] = 0x01;

			//	int lenAdd = 0;
			//	if (writeLen)
			//	{
			//		ReadOnlySpan<byte> length = WriteRawVarInt32((uint) input.Length);
			//		lenAdd += length.Length;
			//	}

			//	Span<byte> len = BitConverter.GetBytes(input.Length + lenAdd);
			//	len.CopyTo(output.Slice(i));
			//	i += 2;

			//	Span<byte> lenCompl = BitConverter.GetBytes(~(input.Length + lenAdd));
			//	lenCompl.CopyTo(output.Slice(i));
			//	i += 2;


			//	int expectedSize = 2 + 1 + 2 + 2 + input.Length + 4;

			//	if (writeLen)
			//	{
			//		ReadOnlySpan<byte> length = WriteRawVarInt32((uint) input.Length);
			//		length.CopyTo(output.Slice(i));
			//		expectedSize += length.Length;
			//		i += length.Length;
			//	}

			//	input.Span.CopyTo(output.Slice(i));

			//	byte[] result = output.Slice(0, expectedSize).ToArray();
			//	return result;
			//}


			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				stream.WriteByte(0x78); // zlib header
				switch (compressionLevel)
				{
					case CompressionLevel.Optimal:
						stream.WriteByte(0xda);
						break;
					case CompressionLevel.Fastest:
						stream.WriteByte(0x9c);
						break;
					case CompressionLevel.NoCompression:
						stream.WriteByte(0x01);
						break;
				}
				int checksum = 0;
				using (var compressStream = new ZLibStream(stream, compressionLevel, true))
				{
					if (writeLen)
					{
						WriteLength(compressStream, input.Length);
					}

					compressStream.Write(input.Span);
					checksum = compressStream.Checksum;
				}

				var checksumBytes = BitConverter.GetBytes(checksum);
				if (BitConverter.IsLittleEndian)
				{
					// Adler32 checksum is big-endian
					Array.Reverse(checksumBytes);
				}
				stream.Write(checksumBytes);

				byte[] bytes = stream.ToArray();
				return bytes;
			}
		}

		public static void WriteLength(Stream stream, int lenght)
		{
			VarInt.WriteUInt32(stream, (uint) lenght);
		}

		public static int ReadLength(Stream stream)
		{
			return (int) VarInt.ReadUInt32(stream);
		}
	}
}