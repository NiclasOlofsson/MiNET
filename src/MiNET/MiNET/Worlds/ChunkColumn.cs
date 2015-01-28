using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using Craft.Net.Anvil;
using fNbt;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class ChunkColumn
	{
		public int x;
		public int z;
		public byte[] biomeId = ArrayOf<byte>.Create(256, 2);
		public int[] biomeColor = ArrayOf<int>.Create(256, 1);

		public byte[] blocks = new byte[16*16*128];
		public NibbleArray metadata = new NibbleArray(16*16*128);
		public NibbleArray blocklight = new NibbleArray(16*16*128);
		public NibbleArray skylight = new NibbleArray(16*16*128);
		public NbtFile BlockEntity = null;

		private byte[] _cache = null;

		public ChunkColumn()
		{
			for (int i = 0; i < skylight.Length; i++)
				skylight[i] = 0xff;

			for (int i = 0; i < biomeColor.Length; i++)
				biomeColor[i] = 8761930; // Grass color?
		}

		public byte GetBlock(int x, int y, int z)
		{
			return blocks[(x*2048) + (z*128) + y];
		}

		public void SetBlock(int x, int y, int z, byte bid)
		{
			_cache = null;
			blocks[(x*2048) + (z*128) + y] = bid;
		}

		public void SetBlocklight(int x, int y, int z, byte data)
		{
			_cache = null;
			blocklight[(x*2048) + (z*128) + y] = data;
		}

		public byte GetMetadata(int x, int y, int z)
		{
			return metadata[(x*2048) + (z*128) + y];
		}

		public void SetMetadata(int x, int y, int z, byte data)
		{
			_cache = null;
			metadata[(x*2048) + (z*128) + y] = data;
		}

		public void SetSkylight(int x, int y, int z, byte data)
		{
			_cache = null;
			skylight[(x*2048) + (z*128) + y] = data;
		}

		public byte[] GetBytes()
		{
			if (_cache != null) return _cache;

			MemoryStream stream = new MemoryStream();
			stream.WriteByte(0x78);
			stream.WriteByte(0x01);
			int checksum;
			using (var compressStream = new ZLibStream(stream, CompressionLevel.Optimal, true))
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(compressStream, true);

				writer.Write(IPAddress.HostToNetworkOrder(x));
				writer.Write(IPAddress.HostToNetworkOrder(z));

				writer.Write(blocks);
				writer.Write(metadata.Data);
				writer.Write(skylight.Data);
				writer.Write(blocklight.Data);

				writer.Write(biomeId);

				for (int i = 0; i < biomeColor.Length; i++)
				{
					writer.Write(biomeColor[i]);
				}

				if (BlockEntity != null)
				{
					writer.Write(BlockEntity.SaveToBuffer(NbtCompression.None));
				}

				writer.Flush();

				checksum = compressStream.Checksum;
				writer.Close();
			}

			byte[] checksumBytes = BitConverter.GetBytes(checksum);
			if (BitConverter.IsLittleEndian)
			{
				// Adler32 checksum is big-endian
				Array.Reverse(checksumBytes);
			}
			stream.Write(checksumBytes, 0, checksumBytes.Length);

			var bytes = stream.ToArray();
			stream.Close();

			_cache = bytes;

			return bytes;
		}

		public byte[] GetBytes(bool compress)
		{
			if (compress) return GetBytes();

			MemoryStream stream = new MemoryStream();
			NbtBinaryWriter writer = new NbtBinaryWriter(stream, true);

			writer.Write(IPAddress.HostToNetworkOrder(x));
			writer.Write(IPAddress.HostToNetworkOrder(z));

			writer.Write(blocks);
			writer.Write(metadata.Data);
			writer.Write(skylight.Data);
			writer.Write(blocklight.Data);

			writer.Write(biomeId);

			for (int i = 0; i < biomeColor.Length; i++)
			{
				writer.Write(biomeColor[i]);
			}



			writer.Flush();

			writer.Close();
			return stream.ToArray();
		}
	}

	public static class ArrayOf<T> where T : new()
	{
		public static T[] Create(int size, T initialValue)
		{
			T[] array = (T[]) Array.CreateInstance(typeof (T), size);
			for (int i = 0; i < array.Length; i++)
				array[i] = initialValue;
			return array;
		}

		public static T[] Create(int size)
		{
			T[] array = (T[]) Array.CreateInstance(typeof (T), size);
			for (int i = 0; i < array.Length; i++)
				array[i] = new T();
			return array;
		}
	}
}