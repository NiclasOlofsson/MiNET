using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using fNbt;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Client
{
	public class ClientUtils
	{
		private static int _waterOffsetY = 0;
		private static string _basePath = @"D:\Temp\MCPEWorldStore";

		public static ChunkColumn DecocedChunkColumn(byte[] buffer)
		{
			return null;
			MemoryStream stream = new MemoryStream(buffer);
			{
				NbtBinaryReader defStream = new NbtBinaryReader(stream, true);
				ChunkColumn chunk = new ChunkColumn();

				chunk.x = IPAddress.NetworkToHostOrder(defStream.ReadInt32());
				chunk.z = IPAddress.NetworkToHostOrder(defStream.ReadInt32());

				int chunkSize = 16*16*128;
				defStream.Read(chunk.blocks, 0, chunkSize);
				defStream.Read(chunk.metadata.Data, 0, chunkSize/2);
				defStream.Read(chunk.skylight.Data, 0, chunkSize/2);
				defStream.Read(chunk.blocklight.Data, 0, chunkSize/2);

				defStream.Read(chunk.height, 0, 256);

				byte[] ints = new byte[256*4];
				defStream.Read(ints, 0, ints.Length);
				int j = 0;
				for (int i = 0; i < ints.Length; i = i + 4)
				{
					chunk.biomeColor[j++] = BitConverter.ToInt32(new[] {ints[i], ints[i + 1], ints[i + 2], ints[i + 3]}, 0);
				}

				return chunk;
			}
		}

		private static void SetNibble4(byte[] arr, int index, byte value)
		{
			if (index%2 == 0)
			{
				arr[index/2] = (byte) ((value & 0x0F) | arr[index/2]);
			}
			else
			{
				arr[index/2] = (byte) (((value << 4) & 0xF0) | arr[index/2]);
			}
		}

		public static void SaveLevel(LevelInfo level)
		{
			if (!Directory.Exists(_basePath))
				Directory.CreateDirectory(_basePath);

			NbtFile file = new NbtFile();
			NbtTag dataTag = file.RootTag["Data"] = new NbtCompound("Data");
			level.SaveToNbt(dataTag);
			file.SaveToFile(Path.Combine(_basePath, "level.dat"), NbtCompression.ZLib);
		}

		public static void SaveChunkToAnvil(ChunkColumn chunk)
		{
			AnvilWorldProvider.SaveChunk(chunk, _basePath, 0);
		}

		private static NbtFile CreateNbtFromChunkColumn(ChunkColumn chunk)
		{
			var nbt = new NbtFile();

			NbtCompound levelTag = new NbtCompound("Level");
			nbt.RootTag.Add(levelTag);

			levelTag.Add(new NbtInt("xPos", chunk.x));
			levelTag.Add(new NbtInt("zPos", chunk.z));
			levelTag.Add(new NbtByteArray("Biomes", chunk.biomeId));

			NbtList sectionsTag = new NbtList("Sections");
			levelTag.Add(sectionsTag);

			for (int i = 0; i < 8; i++)
			{
				NbtCompound sectionTag = new NbtCompound();
				sectionsTag.Add(sectionTag);
				sectionTag.Add(new NbtByte("Y", (byte) i));
				int sy = i*16;

				byte[] blocks = new byte[4096];
				byte[] data = new byte[2048];
				byte[] blockLight = new byte[2048];
				byte[] skyLight = new byte[2048];

				for (int x = 0; x < 16; x++)
				{
					for (int z = 0; z < 16; z++)
					{
						for (int y = 0; y < 16; y++)
						{
							int yi = sy + y;
							if (yi < 0 || yi >= 256) continue; // ?

							int anvilIndex = (y + _waterOffsetY)*16*16 + z*16 + x;
							byte blockId = chunk.GetBlock(x, yi, z);

							// PE to Anvil friendly converstion
							if (blockId == 5) blockId = 125;
							else if (blockId == 158) blockId = 126;
							else if (blockId == 50) blockId = 75;
							else if (blockId == 50) blockId = 76;
							else if (blockId == 89) blockId = 123;
							else if (blockId == 89) blockId = 124;
							else if (blockId == 73) blockId = 152;

							blocks[anvilIndex] = blockId;
							SetNibble4(data, anvilIndex, chunk.GetMetadata(x, yi, z));
							SetNibble4(blockLight, anvilIndex, chunk.GetBlocklight(x, yi, z));
							SetNibble4(skyLight, anvilIndex, chunk.GetSkylight(x, yi, z));
						}
					}
				}

				sectionTag.Add(new NbtByteArray("Blocks", blocks));
				sectionTag.Add(new NbtByteArray("Data", data));
				sectionTag.Add(new NbtByteArray("BlockLight", blockLight));
				sectionTag.Add(new NbtByteArray("SkyLight", skyLight));
			}

			levelTag.Add(new NbtList("Entities", NbtTagType.Compound));
			levelTag.Add(new NbtList("TileEntities", NbtTagType.Compound));
			levelTag.Add(new NbtList("TileTicks", NbtTagType.Compound));

			return nbt;
		}
	}
}