using System;
using System.IO;
using System.IO.Compression;
using MiNET.Net;

namespace MiNET.Utils
{
	public class BatchUtils
	{
		public static McpeBatch CreateBatchPacket(CompressionLevel compressionLevel, params Package[] packages)
		{
			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				foreach (var package in packages)
				{
					byte[] bytes = package.Encode();
					WriteLenght(stream, bytes.Length);
					stream.Write(bytes, 0, bytes.Length);
					package.PutPool();
				}

				var buffer = stream.GetBuffer();
				return CreateBatchPacket(buffer, 0, buffer.Length, compressionLevel, false);
			}
		}

		public static McpeBatch CreateBatchPacket(byte[] input, int offset, int length, CompressionLevel compressionLevel, bool writeLen)
		{
			using (var stream = CompressIntoStream(input, offset, length, compressionLevel, writeLen))
			{
				var batch = McpeBatch.CreateObject();
				batch.payload = stream.ToArray();
				batch.Encode();
				return batch;
			}
		}

		private static MemoryStream CompressIntoStream(byte[] input, int offset, int length, CompressionLevel compressionLevel, bool writeLen = false)
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
					WriteLenght(compressStream, length);
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

		public static void WriteLenght(Stream stream, int lenght)
		{
			VarInt.WriteUInt32(stream, (uint) lenght);
		}

		public static int ReadLenght(Stream stream)
		{
			return (int) VarInt.ReadUInt32(stream);
		}
	}
}