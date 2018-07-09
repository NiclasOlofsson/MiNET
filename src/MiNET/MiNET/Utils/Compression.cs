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
using System.IO;
using System.IO.Compression;
using fNbt;
using log4net;

namespace MiNET.Utils
{
	public class Compression
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Compression));

		public static byte[] Compress(Memory<byte> input, bool writeLen = false, CompressionLevel compressionLevel = CompressionLevel.Fastest)
		{
			return CompressIntoStream(input, compressionLevel, writeLen).ToArray();
		}

		public static MemoryStream CompressIntoStream(Memory<byte> input, CompressionLevel compressionLevel, bool writeLen = false)
		{
			var stream = MiNetServer.MemoryStreamManager.GetStream();

			stream.WriteByte(0x78);
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
			stream.Write(checksumBytes, 0, checksumBytes.Length);
			return stream;
		}

		public static void WriteLength(Stream stream, int lenght)
		{
			VarInt.WriteUInt32(stream, (uint) lenght);
		}

		public static int ReadLength(Stream stream)
		{
			return (int) VarInt.ReadUInt32(stream);
		}

		public static byte[] Decompress(byte[] buffer)
		{
			MemoryStream stream = new MemoryStream(buffer);
			if (stream.ReadByte() != 0x78)
			{
				throw new InvalidDataException("Incorrect ZLib header. Expected 0x78 0x9C");
			}
			stream.ReadByte();
			using (var defStream2 = new DeflateStream(stream, CompressionMode.Decompress, false))
			{
				// Get actual package out of bytes
				MemoryStream destination = new MemoryStream();
				defStream2.CopyTo(destination);
				destination.Position = 0;
				NbtBinaryReader reader = new NbtBinaryReader(destination, true);
				var len = ReadLength(destination);
				byte[] internalBuffer = reader.ReadBytes(len);

				//Log.Debug($"Package [len={len}:\n" + Package.HexDump(internalBuffer));

				if (destination.Length > destination.Position) throw new Exception($"Read {len} bytes, but have more data. Length={destination.Length}, Pos={destination.Position}");

				return internalBuffer;
			}
		}
	}
}