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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Client
{
	public class ClientUtils
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ClientUtils));

		private static int _waterOffsetY = 0;
		private static string _basePath = @"D:\Temp\MCPEWorldStore";

		private static object _chunkRead = new object();

		public static ChunkColumn DecodeChunkColumn(int subChunkCount, byte[] buffer, BlockPallet bedrockPallet = null, HashSet<BlockRecord> internalBlockPallet = null)
		{
			lock (_chunkRead)
			{
				var stream = new MemoryStream(buffer);
				{
					var defStream = new NbtBinaryReader(stream, true);

					if (subChunkCount < 1)
					{
						Log.Warn("Nothing to read");
						return null;
					}

					Log.Debug($"Reading {subChunkCount} sections");

					var chunkColumn = new ChunkColumn();

					for (int chunkIndex = 0; chunkIndex < subChunkCount; chunkIndex++)
					{
						int version = defStream.ReadByte();
						int storageSize = defStream.ReadByte();

						var section = (PaletteChunk) chunkColumn[chunkIndex];

						for (int storageIndex = 0; storageIndex < storageSize; storageIndex++)
						{
							int bitsPerBlock = defStream.ReadByte() >> 1;
							int blocksPerWord = (int) Math.Floor(32f / bitsPerBlock);
							int wordsPerChunk = (int) Math.Ceiling(4096f / blocksPerWord);
							Log.Debug($"New section {chunkIndex}, " +
									$"version={version}, " +
									$"storageSize={storageSize}, " +
									$"bitsPerBlock={bitsPerBlock}, " +
									$"noBlocksPerWord={blocksPerWord}, " +
									$"wordCount={wordsPerChunk}, " +
									$"");

							long jumpPos = stream.Position;
							stream.Seek(wordsPerChunk * 4, SeekOrigin.Current);

							int paletteCount = VarInt.ReadSInt32(stream);
							var palette = new uint[paletteCount];
							for (int j = 0; j < paletteCount; j++)
							{
								palette[j] = GetHashRuntimeId(bedrockPallet, internalBlockPallet, VarInt.ReadSInt32(stream));
							}

							long afterPos = stream.Position;
							stream.Position = jumpPos;
							int position = 0;
							for (int w = 0; w < wordsPerChunk; w++)
							{
								uint word = defStream.ReadUInt32();
								for (int block = 0; block < blocksPerWord; block++)
								{
									if (position >= 4096)
										continue;

									uint state = (uint) ((word >> ((position % blocksPerWord) * bitsPerBlock)) & ((1 << bitsPerBlock) - 1));

									int x = (position >> 8) & 0xF;
									int y = position & 0xF;
									int z = (position >> 4) & 0xF;

									uint hash = palette[state];

									int bid = (int) (hash >> 4);
									if (BlockFactory.GetBlockById(bid).GetType() == typeof(Block))
									{
										//Log.Error($"No block for bid={bid}");
										bid = 0;
									}

									if (storageIndex == 0)
									{
										section.SetBlock(x, y, z, bid);
										section.SetMetadata(x, y, z, (byte) (hash & 0xf));
									}
									else
									{
										section.SetLoggedBlock(x, y, z, bid);
										section.SetLoggedMetadata(x, y, z, (byte) (hash & 0xf));
									}

									position++;
								}
							}
							stream.Position = afterPos;
						}
					}

					if (defStream.Read(chunkColumn.biomeId, 0, 256) != 256) Log.Error($"Out of data biomeId");
					//Log.Debug($"biomeId:\n{Package.HexDump(chunk.biomeId)}");

					//if (stream.Position >= stream.Length - 1) return chunkColumn;

					int borderBlock = VarInt.ReadSInt32(stream);
					if (borderBlock != 0)
					{
						byte[] buf = new byte[borderBlock];
						int len = defStream.Read(buf, 0, borderBlock);
						Log.Warn($"??? Got borderblock {borderBlock}. Read {len} bytes");
						Log.Debug($"{Packet.HexDump(buf)}");
						for (int i = 0; i < borderBlock; i++)
						{
							int x = (buf[i] & 0xf0) >> 4;
							int z = buf[i] & 0x0f;
							Log.Debug($"x={x}, z={z}");
						}
					}

					if (stream.Position < stream.Length - 1)
					{
						while (stream.Position < stream.Length)
						{
							NbtFile file = new NbtFile()
							{
								BigEndian = false,
								UseVarInt = true
							};

							file.LoadFromStream(stream, NbtCompression.None);

							var blockEntityTag = file.RootTag;
							int x = blockEntityTag["x"].IntValue;
							int y = blockEntityTag["y"].IntValue;
							int z = blockEntityTag["z"].IntValue;

							chunkColumn.SetBlockEntity(new BlockCoordinates(x, y, z), (NbtCompound) file.RootTag);

							Log.Debug($"Blockentity: {file.RootTag}");
						}
					}
					if (stream.Position < stream.Length - 1)
					{
						Log.Warn($"Still have data to read\n{Packet.HexDump(defStream.ReadBytes((int) (stream.Length - stream.Position)))}");
					}

					return chunkColumn;
				}
			}
		}

		private static uint GetHashRuntimeId(BlockPallet bedrockPallet, HashSet<BlockRecord> internalBlockPallet, int runtimeId)
		{
			if (runtimeId < 0 || runtimeId >= bedrockPallet.Count) Log.Error($"RuntimeId = {runtimeId}");

			var record = bedrockPallet[runtimeId];

			if (!internalBlockPallet.TryGetValue(record, out BlockRecord internalRecord))
			{
				Log.Error($"Did not find {record.Id}");
				return 0; // air
			}
			uint hash = (uint) ((internalRecord.Id << 4) | (byte) internalRecord.Data);
			return hash;
		}

		private static void SetNibble4(byte[] arr, int index, byte value)
		{
			if (index % 2 == 0)
			{
				arr[index / 2] = (byte) ((value & 0x0F) | arr[index / 2]);
			}
			else
			{
				arr[index / 2] = (byte) (((value << 4) & 0xF0) | arr[index / 2]);
			}
		}

		public static void SaveLevel(LevelInfo level)
		{
			if (!Directory.Exists(_basePath))
				Directory.CreateDirectory(_basePath);

			NbtFile file = new NbtFile();
			NbtTag dataTag = file.RootTag["Data"] = new NbtCompound("Data");
			level.SaveToNbt(dataTag);
			file.SaveToFile(Path.Combine(_basePath, "level.dat"), NbtCompression.GZip);
		}

		public static void SaveChunkToAnvil(ChunkColumn chunk)
		{
			lock (_basePath)
			{
				AnvilWorldProvider.SaveChunk(chunk, _basePath);
			}
		}

		private static NbtFile CreateNbtFromChunkColumn(ChunkColumn chunk)
		{
			var nbt = new NbtFile();

			var levelTag = new NbtCompound("Level");
			var rootTag = (NbtCompound) nbt.RootTag;
			rootTag.Add(levelTag);

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
				int sy = i * 16;

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

							int anvilIndex = (y + _waterOffsetY) * 16 * 16 + z * 16 + x;
							int blockId = chunk.GetBlock(x, yi, z);

							// PE to Anvil friendly converstion
							if (blockId == 5) blockId = 125;
							else if (blockId == 158) blockId = 126;
							else if (blockId == 50) blockId = 75;
							else if (blockId == 50) blockId = 76;
							else if (blockId == 89) blockId = 123;
							else if (blockId == 89) blockId = 124;
							else if (blockId == 73) blockId = 152;

							blocks[anvilIndex] = (byte) blockId;
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