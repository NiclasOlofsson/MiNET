﻿using System;
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

		private string _worldDir = "";

		public void SaveChunk()
		{
			if (_worldDir == "")
			{
				_worldDir = ConfigParser.GetProperty("MiNETWorldFolder", "world");
				if (!Directory.Exists(_worldDir))
					Directory.CreateDirectory(_worldDir);
			}

			byte[] strm;
			using (MemoryStream stream = new MemoryStream())
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, false);
				writer.Write(blocks.Length);
				writer.Write(blocks);

				writer.Write(metadata.Data.Length);
				writer.Write(metadata.Data);

				writer.Write(skylight.Data.Length);
				writer.Write(skylight.Data);

				writer.Write(blocklight.Data.Length);
				writer.Write(blocklight.Data);

				writer.Write(biomeId.Length);
				writer.Write(biomeId);
				writer.Flush();
				strm = stream.ToArray();
			}

			File.WriteAllBytes(_worldDir + "/" + x + "." + z + ".cfile", Compress(strm));
		}

		public bool TryLoadFromFile(int chunkX, int chunkZ)
		{
			if (_worldDir == "")
			{
				_worldDir = ConfigParser.GetProperty("MiNETWorldFolder", "world");
				if (!Directory.Exists(_worldDir))
					Directory.CreateDirectory(_worldDir);
			}

			if (!File.Exists(_worldDir + "/" + chunkX + "." + chunkZ + ".cfile")) return false;

			byte[] ToRead = Decompress(File.ReadAllBytes(_worldDir + "/" + chunkX + "." + chunkZ + ".cfile"));
			using (MemoryStream stream = new MemoryStream(ToRead))
			{
				NbtBinaryReader reader = new NbtBinaryReader(stream, false);

				int blockLength = reader.ReadInt32();
				blocks = reader.ReadBytes(blockLength);

				int metaLength = reader.ReadInt32();
				metadata.Data = reader.ReadBytes(metaLength);

				int skyLength = reader.ReadInt32();
				skylight.Data = reader.ReadBytes(skyLength);

				int blockLightLength = reader.ReadInt32();
				blocklight.Data = reader.ReadBytes(blockLightLength);

				int Biomeidlength = reader.ReadInt32();
				biomeId = reader.ReadBytes(Biomeidlength);
			}

			return true;
		}

		public static byte[] Compress(byte[] inputData)
		{
			if (inputData == null)
				throw new ArgumentNullException("inputData must be non-null");

			using (var compressIntoMs = new MemoryStream())
			{
				using (var gzs = new BufferedStream(new GZipStream(compressIntoMs,
					CompressionMode.Compress), 2*4096))
				{
					gzs.Write(inputData, 0, inputData.Length);
				}
				return compressIntoMs.ToArray();
			}
		}

		public static byte[] Decompress(byte[] inputData)
		{
			if (inputData == null)
				throw new ArgumentNullException("inputData must be non-null");

			using (var compressedMs = new MemoryStream(inputData))
			{
				using (var decompressedMs = new MemoryStream())
				{
					using (var gzs = new BufferedStream(new GZipStream(compressedMs, CompressionMode.Decompress), 2*4096))
					{
						gzs.CopyTo(decompressedMs);
					}
					return decompressedMs.ToArray();
				}
			}
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