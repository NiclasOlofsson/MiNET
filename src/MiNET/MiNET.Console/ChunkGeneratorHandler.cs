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
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Client;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Console
{
	public class ChunkGeneratorHandler : McpeClientMessageHandlerBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkGeneratorHandler));
		private BlockPalette BlockPalette;
		private HashSet<BlockStateContainer> _internalStates;

		public ChunkGeneratorHandler(MiNetClient client) : base(client)
		{
		}

		public override void HandleMcpeStartGame(McpeStartGame message)
		{
			Client.EntityId = message.runtimeEntityId;
			Client.NetworkEntityId = message.entityIdSelf;
			Client.SpawnPoint = message.spawn;
			Client.CurrentLocation = new PlayerLocation(Client.SpawnPoint, message.rotation.X, message.rotation.X, message.rotation.Y);

			Client.LevelInfo.LevelName = message.levelId;
			Client.LevelInfo.Version = 19133;
			Client.LevelInfo.GameType = message.gamemode;

			BlockPalette = message.blockPalette;

			_internalStates = new HashSet<BlockStateContainer>(BlockFactory.BlockPalette);

			{
				int viewDistance = Config.GetProperty("ViewDistance", 11);

				var packet = McpeRequestChunkRadius.CreateObject();
				Client.ChunkRadius = viewDistance;
				packet.chunkRadius = Client.ChunkRadius;

				Client.SendPacket(packet);
			}
		}


		public override void HandleMcpeSetSpawnPosition(McpeSetSpawnPosition message)
		{
			Client.SpawnPoint = new Vector3(message.coordinates.X, message.coordinates.Y, message.coordinates.Z);
			Client.LevelInfo.SpawnX = (int) Client.SpawnPoint.X;
			Client.LevelInfo.SpawnY = (int) Client.SpawnPoint.Y;
			Client.LevelInfo.SpawnZ = (int) Client.SpawnPoint.Z;
		}


		public override void HandleMcpeLevelChunk(McpeLevelChunk message)
		{
			var coord = new ChunkCoordinates(message.chunkX, message.chunkZ);
			int chunkCount = (int) message.subChunkCount;
			byte[] data = message.chunkData;

			Client.Chunks.GetOrAdd(coord, _ =>
			{
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

				return chunk;
			});
		}
	}
}