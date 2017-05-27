using System;
using System.IO;
using System.IO.Compression;
using fNbt;
using log4net;
using MiNET.Net;

namespace MiNET.Utils
{
	public class Compression
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Compression));

		//public static byte[] Compress(byte[] inputData)
		//{
		//	if (inputData == null)
		//		throw new ArgumentNullException("inputData");

		//	using (var compressIntoMs = MiNetServer.MemoryStreamManager.GetStream())
		//	{
		//		using (var gzs = new BufferedStream(new GZipStream(compressIntoMs, CompressionMode.Compress), 2*4096))
		//		{
		//			gzs.Write(inputData, 0, inputData.Length);
		//		}
		//		return compressIntoMs.ToArray();
		//	}
		//}

		//public static byte[] Decompress(byte[] inputData)
		//{
		//	if (inputData == null)
		//		throw new ArgumentNullException("inputData");

		//	using (var compressedMs = new MemoryStream(inputData))
		//	{
		//		using (var decompressedMs = MiNetServer.MemoryStreamManager.GetStream())
		//		{
		//			using (var gzs = new BufferedStream(new GZipStream(compressedMs, CompressionMode.Decompress), 2*4096))
		//			{
		//				gzs.CopyTo(decompressedMs);
		//			}
		//			return decompressedMs.ToArray();
		//		}
		//	}
		//}

		public static byte[] Compress(byte[] input, int offset, int length, bool writeLen = false)
		{
			return CompressIntoStream(input, offset, length, CompressionLevel.Optimal, writeLen).ToArray();
		}

		public static MemoryStream CompressIntoStream(byte[] input, int offset, int length, CompressionLevel compressionLevel, bool writeLen = false)
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
			int checksum;
			using (var compressStream = new ZLibStream(stream, compressionLevel, true))
			{
				if (writeLen)
				{
					WriteLength(compressStream, length);
				}

				//var lenBytes = BitConverter.GetBytes(length);
				//Array.Reverse(lenBytes);
				//if (writeLen) compressStream.Write(lenBytes, 0, lenBytes.Length); // ??
				compressStream.Write(input, offset, length);
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
			VarInt.WriteUInt32(stream, (uint)lenght);
		}

		public static int ReadLength(Stream stream)
		{
			return (int)VarInt.ReadUInt32(stream);
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