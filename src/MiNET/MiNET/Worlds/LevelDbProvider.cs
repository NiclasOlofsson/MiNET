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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
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
				var file = new NbtFile
				{
					BigEndian = false,
					UseVarInt = false
				};
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

			MissingChunkProvider?.Initialize(this);
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates, bool cacheOnly = false)
		{
			if (Locked || cacheOnly)
			{
				_chunkCache.TryGetValue(chunkCoordinates, out ChunkColumn chunk);
				return chunk;
			}

			// Warning: The following code MAY execute the GetChunk 2 times for the same coordinate
			// if called in rapid succession. However, for the scenario of the provider, this is highly unlikely.
			return _chunkCache.GetOrAdd(chunkCoordinates, coordinates => GetChunk(coordinates, MissingChunkProvider));
		}

		public ChunkColumn GetChunk(ChunkCoordinates coordinates, IWorldGenerator generator)
		{
			var sw = Stopwatch.StartNew();

			byte[] index = Combine(BitConverter.GetBytes(coordinates.X), BitConverter.GetBytes(coordinates.Z));
			byte[] versionKey = Combine(index, 0x76);
			byte[] version = _db.Get(versionKey);

			ChunkColumn chunkColumn = null;
			if (version != null && version.First() >= 10)
			{
				chunkColumn = new ChunkColumn
				{
					X = coordinates.X,
					Z = coordinates.Z
				};

				var chunkDataKey = Combine(index, new byte[] {0x2f, 0});
				for (byte y = 0; y < 16; y++)
				{
					chunkDataKey[9] = y;
					byte[] sectionBytes = _db.Get(chunkDataKey);

					if (sectionBytes == null)
					{
						chunkColumn[y]?.PutPool();
						chunkColumn[y] = null;
						continue;
					}

					ParseSection(chunkColumn[y], sectionBytes);
				}

				// Biomes
				var flatDataBytes = _db.Get(Combine(index, 0x2D));
				if (flatDataBytes != null)
				{
					chunkColumn.biomeId = flatDataBytes.AsSpan().Slice(512, 256).ToArray();
				}

				// Block entities
				byte[] blockEntityBytes = _db.Get(Combine(index, 0x31));
				if (blockEntityBytes != null)
				{
					var data = blockEntityBytes.AsMemory();

					var file = new NbtFile
					{
						BigEndian = false,
						UseVarInt = false
					};
					int position = 0;
					do
					{
						position += (int) file.LoadFromStream(new MemoryStreamReader(data.Slice(position)), NbtCompression.None);

						NbtTag blockEntityTag = file.RootTag;
						int x = blockEntityTag["x"].IntValue;
						int y = blockEntityTag["y"].IntValue;
						int z = blockEntityTag["z"].IntValue;

						chunkColumn.SetBlockEntity(new BlockCoordinates(x, y, z), (NbtCompound) blockEntityTag);
					} while (position < data.Length);
				}
			}

			if (chunkColumn == null)
			{
				if (version != null) Log.Error($"Expected other version, but got version={version.First()}");

				chunkColumn = generator?.GenerateChunkColumn(coordinates);
			}

			if (chunkColumn != null)
			{
				chunkColumn.RecalcHeight();

				if (Dimension == Dimension.Overworld && Config.GetProperty("CalculateLights", false))
				{
					var blockAccess = new SkyLightBlockAccess(this, chunkColumn);
					new SkyLightCalculations().RecalcSkyLight(chunkColumn, blockAccess);
					//TODO: Block lights.
				}

				chunkColumn.IsDirty = false;
				chunkColumn.NeedSave = false;
			}

			//Log.Debug($"Read chunk {coordinates.X}, {coordinates.Z} in {sw.ElapsedMilliseconds}ms");

			return chunkColumn;
		}

		private void ParseSection(SubChunk section, ReadOnlyMemory<byte> data)
		{
			var reader = new MemoryStreamReader(data);

			var version = reader.ReadByte();
			if (version != 8) throw new Exception("Wrong chunk version");

			var storageSize = reader.ReadByte();
			for (int storage = 0; storage < storageSize; storage++)
			{
				byte paletteAndFlag = (byte) reader.ReadByte();
				bool isRuntime = (paletteAndFlag & 1) != 0;
				if (isRuntime)
					throw new Exception("Can't use runtime for persistent storage.");
				int bitsPerBlock = paletteAndFlag >> 1;
				int blocksPerWord = (int) Math.Floor(32d / bitsPerBlock);
				int wordCount = (int) Math.Ceiling(4096d / blocksPerWord);

				long blockIndex = reader.Position;
				reader.Position += wordCount * 4;

				int paletteSize = reader.ReadInt32();

				var palette = new Dictionary<int, int>();
				for (int j = 0; j < paletteSize; j++)
				{
					var file = new NbtFile
					{
						BigEndian = false,
						UseVarInt = false
					};
					file.LoadFromStream(reader, NbtCompression.None);
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

					palette.Add(j, block.GetRuntimeId());
				}

				long nextStore = reader.Position;
				reader.Position = blockIndex;

				int position = 0;
				for (int wordIdx = 0; wordIdx < wordCount; wordIdx++)
				{
					uint word = reader.ReadUInt32();
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
							section.SetBlockByRuntimeId(x, y, z, palette[state]);
						}
						else
						{
							section.SetLoggedBlockByRuntimeId(x, y, z, palette[state]);
						}
						position++;
					}
				}
				reader.Position = nextStore;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte[] Combine(byte[] first, byte[] second)
		{
			var ret = new byte[first.Length + second.Length];
			Buffer.BlockCopy(first, 0, ret, 0, first.Length);
			Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
			return ret;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte[] Combine(byte[] first, byte b)
		{
			var ret = new byte[first.Length + 1];
			Buffer.BlockCopy(first, 0, ret, 0, first.Length);
			ret[^1] = b;
			return ret;
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
	}
}