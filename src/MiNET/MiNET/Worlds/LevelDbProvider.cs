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
using System.Threading;
using System.Threading.Tasks;
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

		public string BasePath { get; private set; }
		public LevelInfoBedrock LevelInfo { get; private set; }
		public bool IsCaching { get; } = true;
		public bool Locked { get; set; } = false;
		public IWorldGenerator MissingChunkProvider { get; set; }
		public Dimension Dimension { get; set; } = Dimension.Overworld;

		public LevelDbProvider()
		{
			MissingChunkProvider = new SuperflatGenerator(Dimension.Overworld);
		}

		public void Initialize()
		{
			BasePath = BasePath ?? Config.GetProperty("LevelDBWorldFolder", "World").Trim();

			var directory = new DirectoryInfo(Path.Combine(BasePath, "db"));

			var levelFileName = Path.Combine(BasePath, "level.dat");
			Log.Debug($"Loading level.dat from {levelFileName}");
			if (File.Exists(levelFileName))
			{
				var file = new NbtFile {BigEndian = false, UseVarInt = false};
				var levelStream = File.OpenRead(levelFileName);
				levelStream.Seek(8, SeekOrigin.Begin);
				file.LoadFromStream(levelStream, NbtCompression.None);
				Log.Debug($"Level DAT\n{file.RootTag}");
				NbtTag dataTag = file.RootTag["Data"];
				//LevelInfo = new LevelInfoBedrock(dataTag);
			}
			else
			{
				Log.Warn($"No level.dat found at {levelFileName}. Creating empty.");
				LevelInfo = new LevelInfoBedrock();
			}

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
			return _chunkCache.GetOrAdd(chunkCoordinates, coordinates => GetChunk(coordinates, MissingChunkProvider));
		}

		public ChunkColumn GetChunk(ChunkCoordinates coordinates, IWorldGenerator generator)
		{
			var index = BitConverter.GetBytes(coordinates.X).Concat(BitConverter.GetBytes(coordinates.Z)).ToArray();
			var versionKey = index.Concat(new byte[] {0x76}).ToArray();
			var version = _db.Get(versionKey);

			ChunkColumn chunkColumn = null;
			if (version != null && version.First() == 10)
			{
				chunkColumn = new ChunkColumn();
				chunkColumn.x = coordinates.X;
				chunkColumn.z = coordinates.Z;

				for (byte y = 0; y < 16; y++)
				{
					var chunkDataKey = index.Concat(new byte[] {0x2f, y}).ToArray();
					var sectionBytes = _db.Get(chunkDataKey);

					if (sectionBytes == null)
					{
						for (; y < 16; y++)
						{
							chunkColumn[y]?.PutPool();
							chunkColumn[y] = null;
						}
						break;
					}

					ParseSection((PaletteChunk) chunkColumn[y], sectionBytes);
				}

				// Biomes
				var flatDataBytes = _db.Get(index.Concat(new byte[] {0x2D}).ToArray());
				if(flatDataBytes != null)
				{
					chunkColumn.biomeId = flatDataBytes.AsSpan().Slice(512, 256).ToArray();
				}

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

						chunkColumn.SetBlockEntity(new BlockCoordinates(x, y, z), (NbtCompound) file.RootTag);
					} while (position < data.Length);
				}
			}

			if (chunkColumn == null)
			{
				if (version != null)
					Log.Error($"Expected other version, but got version={version.First()}");

				chunkColumn = generator?.GenerateChunkColumn(coordinates);
				if (chunkColumn != null)
				{
					if (Dimension == Dimension.Overworld && Config.GetProperty("CalculateLights", false))
					{
						var blockAccess = new SkyLightBlockAccess(this, chunkColumn);
						new SkyLightCalculations().RecalcSkyLight(chunkColumn, blockAccess);
					}

					chunkColumn.isDirty = false;
					chunkColumn.NeedSave = false;
				}
			}

			chunkColumn?.RecalcHeight();

			if (Dimension == Dimension.Overworld && Config.GetProperty("CalculateLights", false))
			{
				SkyLightBlockAccess blockAccess = new SkyLightBlockAccess(this, chunkColumn);
				new SkyLightCalculations().RecalcSkyLight(chunkColumn, blockAccess);
				//TODO: Block lights.
			}


			return chunkColumn;
		}

		private void ParseSection(PaletteChunk section, ReadOnlySpan<byte> data)
		{
			var reader = new SpanReader();

			var version = reader.ReadByte(data);
			if (version != 8) throw new Exception("Wrong chunk version");

			var storageSize = reader.ReadByte(data);
			for (int storage = 0; storage < storageSize; storage++)
			{
				byte paletteAndFlag = reader.ReadByte(data);
				bool isRuntime = (paletteAndFlag & 1) != 0;
				if (isRuntime)
					throw new Exception("Can't use runtime for persistent storage.");
				int bitsPerBlock = paletteAndFlag >> 1;
				int blocksPerWord = (int) Math.Floor(32d / bitsPerBlock);
				int wordCount = (int) Math.Ceiling(4096d / blocksPerWord);

				int blockIndex = reader.Position;
				reader.Position += wordCount * 4;

				int paletteSize = reader.ReadInt32(data);

				var palette = new Dictionary<int, (short, byte)>();
				for (int j = 0; j < paletteSize; j++)
				{
					var file = new NbtFile {BigEndian = false, UseVarInt = false};
					var buffer = data.Slice(reader.Position).ToArray();

					int numberOfBytesRead = (int) file.LoadFromStream(new MemoryStream(buffer), NbtCompression.None);
					reader.Position += numberOfBytesRead;
					var tag = file.RootTag;
					string blockName = tag["name"].StringValue;
					Block block = BlockFactory.GetBlockByName(blockName);
					short blockId = 0;
					if (block != null)
					{
						blockId = (short) block.Id;
					}
					else
					{
						Log.Warn($"Missing block={blockName}");
					}
					short blockMeta = tag["val"].ShortValue;
					palette.Add(j, (blockId, (byte) blockMeta));
				}

				int nextStore = reader.Position;
				reader.Position = blockIndex;

				int position = 0;
				for (int wordIdx = 0; wordIdx < wordCount; wordIdx++)
				{
					uint word = reader.ReadUInt32(data);
					for (int block = 0; block < blocksPerWord; block++)
					{
						if (position >= 4096) continue; // padding bytes

						int state = (int) ((word >> ((position % blocksPerWord) * bitsPerBlock)) & ((1 << bitsPerBlock) - 1));
						int x = (position >> 8) & 0xF;
						int y = position & 0xF;
						int z = (position >> 4) & 0xF;
						if (state > palette.Count)
							Log.Error($"Got wrong state={state} from word. bitsPerBlock={bitsPerBlock}, blocksPerWord={blocksPerWord}, Word={word}");
						short bid = palette[state].Item1;
						byte metadata = palette[state].Item2;
						if (storage == 0)
						{
							section.SetBlock(x, y, z, bid);
							section.SetMetadata(x, y, z, metadata);
						}
						else
						{
							section.SetLoggedBlock(x, y, z, bid);
							section.SetLoggedMetadata(x, y, z, metadata);
						}
						position++;
					}
				}
				reader.Position = nextStore;
			}
		}

		public Vector3 GetSpawnPoint()
		{
			return new Vector3(1, 0, 1);
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
			return 0;
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
			return _chunkCache.Values.Where(column => column != null).ToArray();
		}

		public void ClearCachedChunks()
		{
			_chunkCache.Clear();
		}

		public int UnloadChunks(Player[] players, ChunkCoordinates spawn, double maxViewDistance)
		{
			int removed = 0;

			lock (_chunkCache)
			{
				var coords = new List<ChunkCoordinates> {spawn};

				foreach (var player in players)
				{
					var chunkCoordinates = new ChunkCoordinates(player.KnownPosition);
					if (!coords.Contains(chunkCoordinates))
						coords.Add(chunkCoordinates);
				}

				Parallel.ForEach(_chunkCache, (chunkColumn) =>
				{
					bool keep = coords.Exists(c => c.DistanceTo(chunkColumn.Key) < maxViewDistance);
					if (!keep)
					{
						_chunkCache.TryRemove(chunkColumn.Key, out var waste);

						if (waste != null)
						{
							foreach (var chunk in waste)
							{
								chunk.PutPool();
							}
						}

						Interlocked.Increment(ref removed);
					}
				});
			}

			return removed;
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