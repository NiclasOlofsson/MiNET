using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using Craft.Net.Anvil;

namespace MiNET.Network
{
	// https://gist.github.com/Intyre/f5e169c8b15b85fbb479 // model
	// https://gist.github.com/Intyre/442c113fb0fe01776ba8 // code

	/*
		Chunk
			8 Sections
			biomeid 		256 x biomeid (bytes)
			biomecolor		256 x color (int)
 
		Sections 			4096 to make it easy to get blocks and data
			blocks			4096 x block id (byte)
			meta			4096 x block id (byte)
			blocklight		4096 x block id (byte)	
			skylight		4096 x block id (byte)
 
 
 
		Chunk file
		----------
			x			chunk location (int)
			z			chunk location (int)
			Sections 		8 * section data
				blocks		4096 x block id (byte)
				meta		2048 x block id (byte)
				blocklight	2048 x block id (byte)	
				skylight	2048 x block id (byte)
			biomeid			256 x bytes
			biomecolor		256 x int

	 class Section(object):
		"""Section for chunk"""
 
		def __init__(self):
			"""4096 bytes for blocks, meta, blocklight and skylight"""
			self.blocks = array("B", [0] * 4096)
			self.meta = array("B", [0] * 4096)
			self.blocklight = array("B", [0] * 4096)
			self.skylight = array("B", [0] * 4096)
        
        
	class Chunk(object):
 
		def __init__(self, x, z):
			self.x = x
			self.z = z
 
			self.heightmap = {}
			self.sections = [Section() for i in range(8)]
 
			self.biomeid = array("B", [1] * 256)
			self.biomecolor = array("i", [1253213440] * 256)

	*/

	public class Chunk2
	{
		public int x;
		public int z;
		public byte[] biomeId = ArrayOf<byte>.Create(256, 2);
		public int[] biomeColor = ArrayOf<int>.Create(256, 1);

		public byte[] blocks = new byte[16*16*128];
		public NibbleArray metadata = new NibbleArray(16*16*128);
		public NibbleArray blocklight = new NibbleArray(16*16*128);
		public NibbleArray skylight = new NibbleArray(16*16*128);


		public Chunk2()
		{
			for (int i = 0; i < skylight.Length; i++)
				skylight[i] = 0xff;

			for (int i = 0; i < biomeColor.Length; i++)
				biomeColor[i] = 8761930; // Grass color?
		}

		public byte GetBlock(int x, int y, int z)
		{
			return 0;
		}

		public void SetBlock(int x, int y, int z, byte bid)
		{
			blocks[(x*2048) + (z*128) + y] = bid;
		}

		public void SetBlocklight(int x, int y, int z, byte data)
		{
			blocklight[(x*2048) + (z*128) + y] = data;
		}

		public byte GetMetadata(int x, int y, int z)
		{
			return 0;
		}

		public void SetMetadata(int x, int y, int z, byte data)
		{
			metadata[(x*2048) + (z*128) + y] = data;
		}

		public void SetSkylight(int x, int y, int z, byte data)
		{
			skylight[(x*2048) + (z*128) + y] = data;
		}

		public byte[] GetBytes()
		{
			MemoryStream stream = new MemoryStream();
			stream.WriteByte(0x78);
			stream.WriteByte(0x01);
			int checksum;
			using (var compressStream = new ZLibStream(stream, CompressionMode.Compress, true))
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
