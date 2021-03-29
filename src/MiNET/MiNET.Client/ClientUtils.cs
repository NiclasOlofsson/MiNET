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
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Client
{
	public class ClientUtils
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ClientUtils));

		private static int _waterOffsetY = 0;
		private static string _basePath = @"D:\Temp\MCPEWorldStore";

		private static object _chunkRead = new object();

		public static ChunkColumn DecodeChunkColumn(int subChunkCount, byte[] buffer, BlockPalette bedrockPalette = null, HashSet<BlockStateContainer> internalBlockPallet = null)
		{
			//lock (_chunkRead)
			{
				var stream = new MemoryStream(buffer);
				{
					var defStream = new BinaryReader(stream);

					if (subChunkCount < 1)
					{
						Log.Warn("Nothing to read");
						return null;
					}

					//if (Log.IsTraceEnabled()) Log.Trace($"Reading {subChunkCount} sections");

					var chunkColumn = new ChunkColumn(false);

					for (int chunkIndex = 0; chunkIndex < subChunkCount; chunkIndex++)
					{
						int version = stream.ReadByte();
						int storageSize = stream.ReadByte();

						var subChunk = chunkColumn[chunkIndex];

						for (int storageIndex = 0; storageIndex < storageSize; storageIndex++)
						{
							int flags = stream.ReadByte();
							bool isRuntime = (flags & 1) != 0;
							int bitsPerBlock = flags >> 1;
							int blocksPerWord = (int) Math.Floor(32f / bitsPerBlock);
							int wordsPerChunk = (int) Math.Ceiling(4096f / blocksPerWord);
							if (Log.IsTraceEnabled())
								Log.Trace($"New section {chunkIndex}, " +
										$"version={version}, " +
										$"storageSize={storageSize}, " +
										$"storageIndex={storageIndex}, " +
										$"bitsPerBlock={bitsPerBlock}, " +
										$"isRuntime={isRuntime}, " +
										$"noBlocksPerWord={blocksPerWord}, " +
										$"wordCount={wordsPerChunk}, " +
										$"");

							long jumpPos = stream.Position;
							stream.Seek(wordsPerChunk * 4, SeekOrigin.Current);

							int paletteCount = VarInt.ReadSInt32(stream);
							var palette = new int[paletteCount];
							for (int j = 0; j < paletteCount; j++)
							{
								if (!isRuntime)
								{
									var file = new NbtFile
									{
										BigEndian = false,
										UseVarInt = true
									};
									file.LoadFromStream(stream, NbtCompression.None);
									var tag = (NbtCompound) file.RootTag;

									Block block = BlockFactory.GetBlockByName(tag["name"].StringValue);
									if (block != null && block.GetType() != typeof(Block) && !(block is Air))
									{
										List<IBlockState> blockState = ReadBlockState(tag);
										block.SetState(blockState);
									}
									else
									{
										block = new Air();
									}

									palette[j] = block.GetRuntimeId();
								}
								else
								{
									int runtimeId = VarInt.ReadSInt32(stream);
									if (bedrockPalette == null || internalBlockPallet == null) continue;

									palette[j] = GetServerRuntimeId(bedrockPalette, internalBlockPallet, runtimeId);
								}
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

									int runtimeId = palette[state];

									if (storageIndex == 0)
									{
										subChunk.SetBlockByRuntimeId(x, y, z, (int) runtimeId);
									}
									else
									{
										subChunk.SetLoggedBlockByRuntimeId(x, y, z, (int) runtimeId);
									}

									position++;
								}
							}
							stream.Position = afterPos;
						}
					}

					if (stream.Read(chunkColumn.biomeId, 0, 256) != 256) return chunkColumn;
					//Log.Debug($"biomeId:\n{Package.HexDump(chunk.biomeId)}");

					if (stream.Position >= stream.Length - 1) return chunkColumn;

					int borderBlock = VarInt.ReadSInt32(stream);
					if (borderBlock != 0)
					{
						Log.Warn($"??? Got borderblock with value {borderBlock}.");

						int len = (int) (stream.Length - stream.Position);
						var bytes = new byte[len];
						stream.Read(bytes, 0, len);
						Log.Warn($"Data to read for border blocks\n{Packet.HexDump(new ReadOnlyMemory<byte>(bytes))}");

						//byte[] buf = new byte[borderBlock];
						//int len = stream.Read(buf, 0, borderBlock);
						//Log.Warn($"??? Got borderblock {borderBlock}. Read {len} bytes");
						//Log.Debug($"{Packet.HexDump(buf)}");
						//for (int i = 0; i < borderBlock; i++)
						//{
						//	int x = (buf[i] & 0xf0) >> 4;
						//	int z = buf[i] & 0x0f;
						//	Log.Debug($"x={x}, z={z}");
						//}
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
							if (blockEntityTag.Name != "alex")
							{
								int x = blockEntityTag["x"].IntValue;
								int y = blockEntityTag["y"].IntValue;
								int z = blockEntityTag["z"].IntValue;

								chunkColumn.SetBlockEntity(new BlockCoordinates(x, y, z), (NbtCompound) file.RootTag);

								if (Log.IsTraceEnabled()) Log.Trace($"Blockentity:\n{file.RootTag}");
							}
						}
					}

					if (stream.Position < stream.Length - 1)
					{
						int len = (int) (stream.Length - stream.Position);
						var bytes = new byte[len];
						stream.Read(bytes, 0, len);
						Log.Warn($"Still have data to read\n{Packet.HexDump(new ReadOnlyMemory<byte>(bytes))}");
					}

					return chunkColumn;
				}
			}
		}

		private static List<IBlockState> ReadBlockState(NbtCompound tag)
		{
			//Log.Debug($"Palette nbt:\n{tag}");

			var states = new List<IBlockState>();
			var nbtStates = (NbtCompound) tag["states"];
			foreach (NbtTag stateTag in nbtStates)
			{
				IBlockState state = stateTag.TagType switch
				{
					NbtTagType.Byte => (IBlockState) new BlockStateByte()
					{
						Name = stateTag.Name,
						Value = stateTag.ByteValue
					},
					NbtTagType.Int => new BlockStateInt()
					{
						Name = stateTag.Name,
						Value = stateTag.IntValue
					},
					NbtTagType.String => new BlockStateString()
					{
						Name = stateTag.Name,
						Value = stateTag.StringValue
					},
					_ => throw new ArgumentOutOfRangeException()
				};
				states.Add(state);
			}

			return states;
		}

		private static int GetServerRuntimeId(BlockPalette bedrockPalette, HashSet<BlockStateContainer> internalBlockPallet, int runtimeId)
		{
			if (runtimeId < 0 || runtimeId >= bedrockPalette.Count) Log.Error($"RuntimeId = {runtimeId}");

			var record = bedrockPalette[runtimeId];

			if (!internalBlockPallet.TryGetValue(record, out BlockStateContainer internalRecord))
			{
				Log.Error($"Did not find {record.Id}");
				return 0; // air
			}

			return internalRecord.RuntimeId;
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

		//private static NbtFile CreateNbtFromChunkColumn(ChunkColumn chunk)
		//{
		//	var nbt = new NbtFile();

		//	var levelTag = new NbtCompound("Level");
		//	var rootTag = (NbtCompound) nbt.RootTag;
		//	rootTag.Add(levelTag);

		//	levelTag.Add(new NbtInt("xPos", chunk.x));
		//	levelTag.Add(new NbtInt("zPos", chunk.z));
		//	levelTag.Add(new NbtByteArray("Biomes", chunk.biomeId));

		//	NbtList sectionsTag = new NbtList("Sections");
		//	levelTag.Add(sectionsTag);

		//	for (int i = 0; i < 8; i++)
		//	{
		//		NbtCompound sectionTag = new NbtCompound();
		//		sectionsTag.Add(sectionTag);
		//		sectionTag.Add(new NbtByte("Y", (byte) i));
		//		int sy = i * 16;

		//		byte[] blocks = new byte[4096];
		//		byte[] data = new byte[2048];
		//		byte[] blockLight = new byte[2048];
		//		byte[] skyLight = new byte[2048];

		//		for (int x = 0; x < 16; x++)
		//		{
		//			for (int z = 0; z < 16; z++)
		//			{
		//				for (int y = 0; y < 16; y++)
		//				{
		//					int yi = sy + y;
		//					if (yi < 0 || yi >= 256) continue; // ?

		//					int anvilIndex = (y + _waterOffsetY) * 16 * 16 + z * 16 + x;
		//					int blockId = chunk.GetBlockId(x, yi, z);

		//					// PE to Anvil friendly converstion
		//					if (blockId == 5) blockId = 125;
		//					else if (blockId == 158) blockId = 126;
		//					else if (blockId == 50) blockId = 75;
		//					else if (blockId == 50) blockId = 76;
		//					else if (blockId == 89) blockId = 123;
		//					else if (blockId == 89) blockId = 124;
		//					else if (blockId == 73) blockId = 152;

		//					blocks[anvilIndex] = (byte) blockId;
		//					SetNibble4(data, anvilIndex, chunk.GetMetadata(x, yi, z));
		//					SetNibble4(blockLight, anvilIndex, chunk.GetBlocklight(x, yi, z));
		//					SetNibble4(skyLight, anvilIndex, chunk.GetSkylight(x, yi, z));
		//				}
		//			}
		//		}

		//		sectionTag.Add(new NbtByteArray("Blocks", blocks));
		//		sectionTag.Add(new NbtByteArray("Data", data));
		//		sectionTag.Add(new NbtByteArray("BlockLight", blockLight));
		//		sectionTag.Add(new NbtByteArray("SkyLight", skyLight));
		//	}

		//	levelTag.Add(new NbtList("Entities", NbtTagType.Compound));
		//	levelTag.Add(new NbtList("TileEntities", NbtTagType.Compound));
		//	levelTag.Add(new NbtList("TileTicks", NbtTagType.Compound));

		//	return nbt;
		//}
	}
}