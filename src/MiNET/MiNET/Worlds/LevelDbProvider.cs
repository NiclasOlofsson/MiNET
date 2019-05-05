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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2019 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.LevelDB;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class LevelDbProvider : IWorldProvider, ICachingWorldProvider, ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(LevelDbProvider));

		private ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
		private Database _db;

		public bool IsCaching { get; } = true;
		public bool Locked { get; set; } = false;
		public string BasePath { get; private set; }
		public IWorldGenerator MissingChunkProvider { get; set; }
		public Dimension Dimension { get; set; } = Dimension.Overworld;

		public LevelDbProvider()
		{
			MissingChunkProvider = new SuperflatGenerator(Dimension.Overworld);
		}

		public void Initialize()
		{
			string newDirPath = Path.Combine(Path.GetTempPath(), "My World.mcworld");
			var directory = new DirectoryInfo(Path.Combine(newDirPath, "db"));

			var db = new Database(directory);
			db.Open();
			_db = db;

			MissingChunkProvider?.Initialize();
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates, bool cacheOnly = false)
		{
			if (Locked || cacheOnly)
			{
				ChunkColumn chunk;
				_chunkCache.TryGetValue(chunkCoordinates, out chunk);
				return chunk;
			}

			// Warning: The following code MAY execute the GetChunk 2 times for the same coordinate
			// if called in rapid succession. However, for the scenario of the provider, this is highly unlikely.
			return _chunkCache.GetOrAdd(chunkCoordinates, coordinates => GetChunk(coordinates, BasePath, MissingChunkProvider));
		}

		public ChunkColumn GetChunk(ChunkCoordinates coordinates, string basePath, IWorldGenerator generator)
		{
			var index = BitConverter.GetBytes(coordinates.X).Concat(BitConverter.GetBytes(coordinates.Z)).ToArray();
			var versionKey = index.Concat(new byte[] {0x76}).ToArray();
			var version = _db.Get(versionKey);

			ChunkColumn chunkColumn = null;
			if (version == null || version.First() != 10)
			{
				if (version != null)
					Log.Error($"Expected other version, but got version={version.First()}");

				chunkColumn = generator?.GenerateChunkColumn(coordinates);
				if (chunkColumn != null)
				{
					if (Dimension == Dimension.Overworld && Config.GetProperty("CalculateLights", false))
					{
						SkyLightBlockAccess blockAccess = new SkyLightBlockAccess(this, chunkColumn);
						new SkyLightCalculations().RecalcSkyLight(chunkColumn, blockAccess);
					}

					chunkColumn.isDirty = false;
					chunkColumn.NeedSave = false;
				}
			}
			else
			{
				for (byte y = 0; y < 16; y++)
				{
					var chunkDataKey = index.Concat(new byte[] {0x2f, y}).ToArray();
					var sectionBytes = _db.Get(chunkDataKey);

					if (sectionBytes == null) break;

					if (y == 0)
					{
						chunkColumn = new ChunkColumn();
						chunkColumn.x = coordinates.X;
						chunkColumn.z = coordinates.Z;
					}

					//Log.Debug($"Parsing x={coordinates.X}, z={coordinates.Z}, y={y}");
					if (chunkColumn != null) chunkColumn[y] = ParseSection(sectionBytes);
				}

				if (chunkColumn != null)
				{
					// Biomes
					var flatDataBytes = _db.Get(index.Concat(new byte[] {0x2D}).ToArray());
					chunkColumn.biomeId = flatDataBytes.AsSpan().Slice(512, 256).ToArray();

					// Block entities
					var blockEntityBytes = _db.Get(index.Concat(new byte[] {0x31}).ToArray());
					if (blockEntityBytes != null)
					{
						var data = blockEntityBytes.AsSpan();

						var file = new NbtFile {BigEndian = false, UseVarInt = false};
						int position = 0;
						do
						{
							position += (int) file.LoadFromStream(new MemoryStream(data.Slice(position).ToArray()), NbtCompression.None);

							var blockEntityTag = file.RootTag;
							int x = blockEntityTag["x"].IntValue;
							int y = blockEntityTag["y"].IntValue;
							int z = blockEntityTag["z"].IntValue;

							chunkColumn.SetBlockEntity(new BlockCoordinates(x, y, z), file.RootTag);
						} while (position < data.Length);
					}

				}
			}

			return chunkColumn;
		}

		private PaletteChunk ParseSection(ReadOnlySpan<byte> data)
		{
			var stream = new SpanReader();

			var version = stream.ReadByte(data);
			if (version != 8) throw new Exception("Wrong chunk version");

			var storageSize = stream.ReadByte(data);
			var section = PaletteChunk.CreateObject();
			for (int storage = 0; storage < storageSize; storage++)
			{
				byte paletteAndFlag = stream.ReadByte(data);
				bool isRuntime = (paletteAndFlag & 1) != 0;
				if (isRuntime)
					throw new Exception("Can't use runtime for persistent storage.");
				int bitsPerBlock = paletteAndFlag >> 1;
				int blocksPerWord = (int) Math.Floor(32d / bitsPerBlock);
				int wordCount = (int) Math.Ceiling(4096d / blocksPerWord);

				int blockIndex = stream.Position;
				stream.Position += wordCount * 4;

				int paletteSize = stream.ReadInt32(data);

				var palette = new Dictionary<int, (int, short)>();
				for (int j = 0; j < paletteSize; j++)
				{
					var file = new NbtFile {BigEndian = false, UseVarInt = false};
					var buffer = data.Slice(stream.Position).ToArray();

					int numberOfBytesRead = (int) file.LoadFromStream(new MemoryStream(buffer), NbtCompression.None);
					stream.Position += numberOfBytesRead;
					var tag = file.RootTag;
					string blockName = tag["name"].StringValue;
					Block block = BlockFactory.GetBlockByName(blockName);
					int blockId = 0;
					if (block != null)
					{
						blockId = block.Id;
					}
					else
					{
						Log.Warn($"Missing block={blockName}");
					}
					short blockMeta = tag["val"].ShortValue;
					palette.Add(j, (blockId, blockMeta));
				}

				int nextStore = stream.Position;
				stream.Position = blockIndex;

				int position = 0;
				for (int wordIdx = 0; wordIdx < wordCount; wordIdx++)
				{
					uint word = stream.ReadUInt32(data);
					for (int block = 0; block < blocksPerWord; block++)
					{
						if (position >= 4096) continue; // padding bytes

						int state = (int) ((word >> ((position % blocksPerWord) * bitsPerBlock)) & ((1 << bitsPerBlock) - 1));
						int x = (position >> 8) & 0xF;
						int y = position & 0xF;
						int z = (position >> 4) & 0xF;
						if (state > palette.Count)
							Log.Error($"Got wrong state={state} from word. bitsPerBlock={bitsPerBlock}, blocksPerWord={blocksPerWord}, Word={word}");
						if (storage == 0)
						{
							section.SetBlock(x, y, z, palette[state].Item1);
							section.SetMetadata(x, y, z, (byte) palette[state].Item2);
						}
						else
						{
							section.SetLoggedBlock(x, y, z, palette[state].Item1);
							section.SetLoggedMetadata(x, y, z, (byte) palette[state].Item2);
						}
						position++;
					}
				}
				stream.Position = nextStore;
			}

			return section;
		}

		public Vector3 GetSpawnPoint()
		{
			return new Vector3();
		}

		public string GetName()
		{
			return "LevelDB World";
		}

		public long GetTime()
		{
			return 0;
		}

		public long GetDayTime()
		{
			return 0;
		}

		public int SaveChunks()
		{
			throw new NotImplementedException();
		}

		public bool HaveNether()
		{
			return false;
		}

		public bool HaveTheEnd()
		{
			return false;
		}

		public ChunkColumn[] GetCachedChunks()
		{
			throw new NotImplementedException();
		}

		public void ClearCachedChunks()
		{
			throw new NotImplementedException();
		}

		public int UnloadChunks(Player[] players, ChunkCoordinates spawn, double maxViewDistance)
		{
			throw new NotImplementedException();
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}
	}

	public class SpanReader
	{
		public int Position { get; set; }

		public byte ReadByte(ReadOnlySpan<byte> span)
		{
			byte val = span[Position];
			Position++;
			return val;
		}

		public int ReadInt32(ReadOnlySpan<byte> span)
		{
			int val = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(Position, 4));
			Position += 4;
			return val;
		}

		public uint ReadUInt32(ReadOnlySpan<byte> span)
		{
			uint val = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(Position, 4));
			Position += 4;
			return val;
		}

		public ReadOnlySpan<byte> ReadBytes(ReadOnlySpan<byte> data, int length)
		{
			ReadOnlySpan<byte> bytes = data.Slice(Position, length);
			Position += length;

			return bytes;
		}

		public int ReadSignedVarInt(ReadOnlySpan<byte> data)
		{
			return DecodeZigZag32((uint) ReadVarInt(data));
		}

		private static int DecodeZigZag32(uint n)
		{
			return (int) (n >> 1) ^ -(int) (n & 1);
		}


		private uint ReadVarInt(ReadOnlySpan<byte> input)
		{
			uint result = 0;
			for (int shift = 0; shift <= 31; shift += 7)
			{
				byte b = input[Position++];

				// add the lower 7 bits to the result
				result |= (uint) ((b & 0x7f) << shift);

				// if high bit is not set, this is the last byte in the number
				if ((b & 0x80) == 0)
				{
					return result;
				}
			}
			throw new Exception("last byte of variable length int has high bit set");
		}
	}
}