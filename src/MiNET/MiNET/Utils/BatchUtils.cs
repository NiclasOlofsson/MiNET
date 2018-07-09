using System;
using System.IO;
using System.IO.Compression;
using MiNET.Net;
using MiNET.Plugins;

namespace MiNET.Utils
{
	public class BatchUtils
	{
		public static McpeWrapper CreateBatchPacket(CompressionLevel compressionLevel, params Packet[] packets)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				foreach (var packet in packets)
				{
					byte[] bytes = packet.Encode();
					WriteLength(stream, bytes.Length);
					stream.Write(bytes, 0, bytes.Length);
					packet.PutPool();
				}

				Memory<byte> buffer = new Memory<byte>(stream.GetBuffer(), 0, (int) stream.Length);
				return CreateBatchPacket(buffer, compressionLevel, false);
			}
		}

		public static McpeWrapper CreateBatchPacket(Memory<byte> input, CompressionLevel compressionLevel, bool writeLen)
		{
			var batch = McpeWrapper.CreateObject();
			batch.payload = Compression.Compress(input, writeLen, compressionLevel);
			batch.Encode(); // prepare
			return batch;
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