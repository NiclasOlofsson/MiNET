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
using System.Linq;
using System.Numerics;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Client;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Nbt;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Console
{
	public class ChunkGeneratorHandler : McpeClientMessageHandlerBase
	{
		private readonly IWorldProvider _worldProvider;
		private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkGeneratorHandler));
		private BlockPalette BlockPalette;
		private HashSet<BlockStateContainer> _internalStates;

		public ChunkGeneratorHandler(MiNetClient client, IWorldProvider worldProvider) : base(client)
		{
			_worldProvider = worldProvider;
		}

		public override void HandleMcpeChunkRadiusUpdate(McpeChunkRadiusUpdate message)
		{
			Log.Info($"Server told us to do {message.chunkRadius} chunk radius");
		}

		public override void HandleMcpeStartGame(McpeStartGame message)
		{
			Client.EntityId = message.runtimeEntityId;
			Client.NetworkEntityId = message.entityIdSelf;
			Client.SpawnPoint = new PlayerLocation(message.position.X, message.position.Y, message.position.Z);
			Client.CurrentLocation = new PlayerLocation(Client.SpawnPoint, message.rotation.X, message.rotation.X, message.rotation.Y);

			Client.LevelInfo.LevelName = message.levelId;
			Client.LevelInfo.Version = 19133;
			Client.LevelInfo.GameType = (int) message.settings.gameType;

			_internalStates = new HashSet<BlockStateContainer>(BlockFactory.BlockPalette);

			Log.Info($"Telling server to do 1 chunk radius");
			var packet = McpeRequestChunkRadius.CreateObject();
			Client.ChunkRadius = 1;
			packet.chunkRadius = Client.ChunkRadius;
			Client.SendPacket(packet);
		}


		public override void HandleMcpeSetSpawnPosition(McpeSetSpawnPosition message)
		{
			Client.SpawnPoint = new Vector3(message.blockPosition.X, message.blockPosition.Y, message.blockPosition.Z);
			Client.LevelInfo.SpawnX = (int) Client.SpawnPoint.X;
			Client.LevelInfo.SpawnY = (int) Client.SpawnPoint.Y;
			Client.LevelInfo.SpawnZ = (int) Client.SpawnPoint.Z;
		}

		/// <summary>
		///     Blob payloads, not just their keys. This tool rebuilds real columns out of what the
		///     server sends, so unlike an ordinary client it has to keep the bytes.
		/// </summary>
		private readonly ConcurrentDictionary<ulong, byte[]> _blobPayloads = new ConcurrentDictionary<ulong, byte[]>();

		private ConcurrentDictionary<CachedChunk, object> _futureChunks = new ConcurrentDictionary<CachedChunk, object>();
		private ConcurrentDictionary<BlockCoordinates, NbtCompound> _futureBlockEntities = new ConcurrentDictionary<BlockCoordinates, NbtCompound>();

		private class CachedChunk
		{
			public int X { get; set; }
			public int Z { get; set; }
			public ulong[] SubChunks { get; set; } = new ulong[16];
			public ulong Biome { get; set; }

			public ChunkColumn Chunk { get; set; } = new ChunkColumn();
		}

		public override void HandleMcpeUpdateBlock(McpeUpdateBlock message)
		{
		}


		public override void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
			BlockCoordinates coordinates = message.blockPosition;
			Nbt nbt = message.actorDataTags;
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn((ChunkCoordinates) coordinates, true);
			if(chunk == null)
			{
				//Log.Warn($"Got block entity for non existing chunk at {coordinates}\n{nbt.NbtFile.RootTag}");
				_futureBlockEntities.TryAdd(coordinates, (NbtCompound) nbt.NbtFile.RootTag);
			}
			else
			{
				//Log.Warn($"Got block entity for existing chunk at {coordinates}\n{nbt.NbtFile.RootTag}");
				chunk.SetBlockEntity(coordinates, (NbtCompound) nbt.NbtFile.RootTag);
			}
		}

		public override void HandleMcpeClientCacheMissResponse(McpeClientCacheMissResponse message)
		{
			foreach (KeyValuePair<ulong, byte[]> kv in message.blobs)
			{
				ulong hash = kv.Key;
				byte[] data = kv.Value;

				_blobPayloads.TryAdd(hash, data);

				var chunks = _futureChunks.Where(c => c.Key.SubChunks.Contains(hash) || c.Key.Biome == hash);
				foreach (KeyValuePair<CachedChunk, object> kvp in chunks)
				{
					CachedChunk chunk = kvp.Key;

					if (chunk.Biome == hash)
					{
						chunk.Chunk.biomeId = data;
						chunk.Biome = 0;
					}
					else
					{
						for (int i = 0; i < chunk.SubChunks.Length; i++)
						{
							ulong subChunkHash = chunk.SubChunks[i];
							if (subChunkHash == hash)
							{
								// parse data
								chunk.Chunk[i] = ClientUtils.DecodeChunkColumn(1, data, BlockPalette, _internalStates)[0];
								chunk.SubChunks[i] = 0;
							}
						}
					}
					if (chunk.Biome == 0 && chunk.SubChunks.All(c => c == 0))
					{
						_futureChunks.TryRemove(chunk, out _);

						var coordinates = new ChunkCoordinates(chunk.Chunk.X, chunk.Chunk.Z);
						foreach (KeyValuePair<BlockCoordinates, NbtCompound> bePair in _futureBlockEntities.Where(be => (ChunkCoordinates) be.Key == coordinates))
						{
							chunk.Chunk.BlockEntities.Add(bePair);
							_futureBlockEntities.TryRemove(bePair.Key, out _);
						}

						chunk.Chunk.RecalcHeight();
						Client.Chunks[coordinates] = chunk.Chunk;
					}
				}
			}
		}

		public override void HandleMcpeLevelChunk(McpeLevelChunk message)
		{
			if (message.cacheEnabled) 
			{
				var chunk = new CachedChunk
				{
					X = message.chunkPosition.x,
					Z = message.chunkPosition.z,
				};
				chunk.Chunk.X = chunk.X;
				chunk.Chunk.Z = chunk.Z;

				var hits = new List<ulong>();
				var misses = new List<ulong>();

				ulong biomeHash = message.cacheMetadata.Last();
				if (_blobPayloads.TryGetValue(biomeHash, out byte[] biomes))
				{
					chunk.Chunk.biomeId = biomes;
					hits.Add(biomeHash);
				}
				else
				{
					chunk.Biome = biomeHash;
					misses.Add(biomeHash);
				}

				for (int i = 0; i < message.cacheMetadata.Count - 1; i++)
				{
					ulong hash = message.cacheMetadata[i];
					if (_blobPayloads.TryGetValue(hash, out byte[] data))
					{
						chunk.Chunk[i] = ClientUtils.DecodeChunkColumn(1, data, BlockPalette, _internalStates)[0];
						hits.Add(hash);
					}
					else
					{
						chunk.SubChunks[i] = hash;
						_futureChunks.TryAdd(chunk, null);
						misses.Add(hash);
					}
				}

				{
					var status = McpeClientCacheBlobStatus.CreateObject();
					status.hashHits = hits.ToArray();
					status.hashMisses = misses.ToArray();
					Client.SendPacket(status);
				}
			}
			else
			{
				var coord = new ChunkCoordinates(message.chunkPosition.x, message.chunkPosition.z);
				int chunkCount = (int) message.subChunkCount;
				byte[] data = message.chunkData;

				ChunkColumn chunk = null;
				try
				{
					chunk = ClientUtils.DecodeChunkColumn(chunkCount, data, BlockPalette, _internalStates);
					if (chunk != null)
					{
						chunk.X = coord.X;
						chunk.Z = coord.Z;
						chunk.RecalcHeight();
					}
				}
				catch (Exception e)
				{
					Log.Error($"Reading chunk {coord}", e);
				}

				Client.Chunks[coord] = chunk;
			}
		}
	}
}