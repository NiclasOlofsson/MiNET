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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Linq;
using MiNET.Blocks;
using MiNET.Client;
using MiNET.Net;
using MiNET.Utils.Vectors;

namespace MiNET.AgentClient
{
	/// <summary>
	///     The protocol behaviour is all in the base; this only surfaces what the agent reads
	///     back: chat, command output and disconnects, one line each on stdout.
	/// </summary>
	public class AgentMessageHandler : McpeClientMessageHandlerBase
	{
		private static readonly bool TraceChunks = Environment.GetEnvironmentVariable("MINET_TRACE_CHUNKS") == "1";

		public AgentMessageHandler(MiNetClient client) : base(client)
		{
		}

		public override void HandleMcpeStartGame(McpeStartGame message)
		{
			base.HandleMcpeStartGame(message);
			Console.WriteLine($"< startgame position {message.position.X:0.0},{message.position.Y:0.0},{message.position.Z:0.0} rotation {message.rotation.X:0},{message.rotation.Y:0} spawn {message.settings.defaultSpawnBlockPosition.X},{message.settings.defaultSpawnBlockPosition.Y},{message.settings.defaultSpawnBlockPosition.Z} hashes {message.blockNetworkIdsAreHashes}");
			Client.BlockNetworkIdsAreHashes = message.blockNetworkIdsAreHashes;

			// The server declares the wire form of block ids here, and BlockFactory's inverse
			// (GetRuntimeIdFromNetworkId) reads the same static flag the server side sets from its
			// config. This process has no server config, so the declaration is applied by hand.
			BlockFactory.BlockNetworkIdsAreHashes = message.blockNetworkIdsAreHashes;
		}

		public override void HandleMcpeLevelChunk(McpeLevelChunk message)
		{
			// The base discards a column outside the published area silently. A picture with a
			// hole in it needs to say which column the server streamed outside its own stamp.
			if (Client.PublishedRadiusChunks > 0)
			{
				var arrived = new ChunkCoordinates(message.chunkPosition.x, message.chunkPosition.z);
				double distance = arrived.DistanceTo(Client.PublishedCenter);
				if (!arrived.IsWithinView(Client.PublishedCenter, Client.PublishedRadiusChunks))
					Console.WriteLine($"< dropped chunk {arrived.X},{arrived.Z} at {distance:0.00} from {Client.PublishedCenter.X},{Client.PublishedCenter.Z} under published radius {Client.PublishedRadiusChunks}");
			}

			// MINET_TRACE_CHUNKS=1 prints every column as it arrives, with the stamp in force,
			// which is how a server's streaming shape is read off a session.
			if (TraceChunks)
				Console.WriteLine($"< chunk {message.chunkPosition.x},{message.chunkPosition.z} sections {message.subChunkCount} limit {message.clientRequestSubchunkLimit?.ToString() ?? "-"} cache {message.cacheEnabled} hashes {message.cacheMetadata?.Count ?? 0} published {Client.PublishedCenter.X},{Client.PublishedCenter.Z} r{Client.PublishedRadiusChunks}");

			base.HandleMcpeLevelChunk(message);

			// A column delivered afresh carries every edit the server made; the overlay for it
			// would only be older than the wire now.
			World?.ForgetEdits(message.chunkPosition.x, message.chunkPosition.z);
		}

		public override void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message)
		{
			base.HandleMcpeNetworkChunkPublisherUpdate(message);
			string built = message.serverBuiltChunks == null || message.serverBuiltChunks.Count == 0
				? "none"
				: string.Join(" ", message.serverBuiltChunks.Select(c => $"{c.X},{c.Z}"));
			Console.WriteLine($"< publisher block {message.coordinates.X},{message.coordinates.Y},{message.coordinates.Z} radius {message.radius} blocks (chunk {Client.PublishedCenter.X},{Client.PublishedCenter.Z} r{Client.PublishedRadiusChunks}) built {built}, holding {Client.ChunkCache.Columns.Count}");
		}

		/// <summary>The world view the renderer reads; edits are applied to it as they arrive.</summary>
		public ClientWorld World { get; set; }

		public override void HandleMcpeUpdateBlock(McpeUpdateBlock message)
		{
			base.HandleMcpeUpdateBlock(message);

			// Layer 0 is the block layer the picture draws; layer 1 is waterlogging.
			if (message.storage == 0)
			{
				int runtimeId = BlockFactory.GetRuntimeIdFromNetworkId(message.blockRuntimeId);
				if (runtimeId >= 0) World?.SetRuntimeId(message.coordinates.X, message.coordinates.Y, message.coordinates.Z, runtimeId);
			}

			if (TraceChunks)
				Console.WriteLine($"< updateblock {message.coordinates.X},{message.coordinates.Y},{message.coordinates.Z} runtimeId {message.blockRuntimeId} {BlockFactory.GetBlockName(BlockFactory.GetRuntimeIdFromNetworkId(message.blockRuntimeId))} flags {message.blockPriority} layer {message.storage}");
		}

		public override void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
			base.HandleMcpeBlockEntityData(message);
			if (TraceChunks)
				Console.WriteLine($"< blockentity {message.coordinates.X},{message.coordinates.Y},{message.coordinates.Z} {message.namedtag?.NbtFile?.RootTag}");
		}

		/// <summary>Other players by runtime id: name and last known position, for "build it where I stand".</summary>
		public System.Collections.Concurrent.ConcurrentDictionary<long, (string Name, System.Numerics.Vector3 Position)> Players { get; } = new();

		public override void HandleMcpeAddPlayer(McpeAddPlayer message)
		{
			base.HandleMcpeAddPlayer(message);
			Players[message.runtimeEntityId] = (message.username, message.position);
		}

		public override void HandleMcpeMovePlayer(McpeMovePlayer message)
		{
			base.HandleMcpeMovePlayer(message);
			if (message.runtimeEntityId != Client.EntityId && Players.TryGetValue(message.runtimeEntityId, out var known))
				Players[message.runtimeEntityId] = (known.Name, message.position);
		}

		public override void HandleMcpeRemoveEntity(McpeRemoveEntity message)
		{
			base.HandleMcpeRemoveEntity(message);
			Players.TryRemove(message.entityIdSelf, out _);
		}

		public override void HandleMcpeText(McpeText message)
		{
			Console.WriteLine($"< text {message.type} {message.source}: {message.message}");
		}

		public override void HandleMcpeCommandOutput(McpeCommandOutput message)
		{
			foreach (CommandOutputMessage line in message.Messages ?? Array.Empty<CommandOutputMessage>())
			{
				Console.WriteLine($"< command {line}");
			}
			if (message.Data != null) Console.WriteLine($"< command data {message.Data}");
		}

		public override void HandleMcpeDisconnect(McpeDisconnect message)
		{
			Console.WriteLine($"< disconnect {message.reason}: {message.message}");
			base.HandleMcpeDisconnect(message);
		}
	}
}