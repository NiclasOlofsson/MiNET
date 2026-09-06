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
using System.Security.Cryptography;
using System.Threading;
using Jose;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Cryptography;
using MiNET.Utils.Vectors;

namespace MiNET.Client
{
	public abstract class McpeClientMessageHandlerBase : IMcpeClientMessageHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(McpeClientMessageHandlerBase));

		public MiNetClient Client { get; }

		public McpeClientMessageHandlerBase(MiNetClient client)
		{
			Client = client;
		}

		public virtual void HandleMcpePlayStatus(McpePlayStatus message)
		{
			Client.PlayerStatus = (McpePlayStatus.PlayStatus) message.status;

			if (Client.PlayerStatus == McpePlayStatus.PlayStatus.LoginSuccess)
			{
				// A real client announces its blob cache right after login success; the server
				// keys the chunk send path (cached or plain) off this packet.
				var packet = McpeClientCacheStatus.CreateObject();
				packet.enabled = Client.UseBlobCache;
				Client.SendPacket(packet);
			}

			if (Client.PlayerStatus == McpePlayStatus.PlayStatus.PlayerSpawn)
			{
				// Spawn tail exactly like a real 1.26 client (captured live): close the loading
				// screen, then announce spawn completion. Servers gate spawn-complete logic
				// (mob spawning, etc.) on set_local_player_as_initialized.
				var loadingScreen = McpeServerBoundLoadingScreen.CreateObject();
				loadingScreen.type = 2;
				loadingScreen.loadingScreenId = null;
				Client.SendPacket(loadingScreen);

				var initialized = McpeSetLocalPlayerAsInitialized.CreateObject();
				initialized.runtimeEntityId = Client.EntityId;
				Client.SendPacket(initialized);

				Client.HasSpawned = true;
				//if (Client.IsEmulator)
				{
					Client.PlayerStatusChangedWaitHandle.Set();
					//Client.SendMcpeMovePlayer();
				}
			}
		}

		public virtual void HandleMcpeServerToClientHandshake(McpeServerToClientHandshake message)
		{
			string token = message.token;
			if (Log.IsDebugEnabled) Log.Debug($"JWT:\n{token}");

			IDictionary<string, dynamic> headers = JWT.Headers(token);
			string x5u = headers["x5u"];

			if (Log.IsDebugEnabled) Log.Debug($"JWT payload:\n{JWT.Payload(token)}");

			// x5u is a DER SubjectPublicKeyInfo; the curve and the point come out of the encoding.
			using var signKey = ECDsa.Create();
			signKey.ImportSubjectPublicKeyInfo(x5u.DecodeBase64(), out _);

			try
			{
				// Decoded to prove the token really is signed by the key it names. The salt it carries
				// seeded the Bedrock session cipher, which nothing runs any more: the transport is
				// DTLS, so the handshake is acknowledged and nothing is enciphered.
				JWT.Decode<HandshakeData>(token, signKey);

				Client.AcknowledgeHandshake();
			}
			catch (Exception e)
			{
				Log.Error(token, e);
				throw;
			}
		}

		public virtual void HandleMcpeDisconnect(McpeDisconnect message)
		{
			Client.StopClient();
		}

		public virtual void HandleMcpeResourcePacksInfo(McpeResourcePacksInfo message)
		{
			//McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
			//response.responseStatus = 3;
			//SendPackage(response);

			if (message.resourcePacks.Count != 0)
			{
				var downloadingPacks = new List<string>();

				foreach (PackInfoData packInfo in message.resourcePacks)
				{
					downloadingPacks.Add(packInfo.packIdVersion.packUuid.ToString());
				}

				var response = new McpeResourcePackClientResponse();
				response.response = new ResourcePackClientResponseDownloading {downloadingPacks = downloadingPacks};
				Client.SendPacket(response);
			}
			else
			{
				var response = new McpeResourcePackClientResponse();
				response.response = new ResourcePackClientResponseDownloadingFinished();
				Client.SendPacket(response);
			}
		}

		public virtual void HandleMcpeResourcePackStack(McpeResourcePackStack message)
		{
			//if (message.resourcepackidversions.Count != 0)
			//{
			//	McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
			//	response.responseStatus = 2;
			//	response.resourcepackidversions = message.resourcepackidversions;
			//	SendPackage(response);
			//}
			//else
			{
				var response = new McpeResourcePackClientResponse();
				response.response = new ResourcePackClientResponseResourcePackStackFinished();
				Client.SendPacket(response);
			}
		}

		public virtual void HandleMcpeText(McpeText message)
		{
		}

		public virtual void HandleMcpeSetTime(McpeSetTime message)
		{
		}

		public virtual void HandleMcpeStartGame(McpeStartGame message)
		{
			var client = Client;
			client.EntityId = message.runtimeEntityId;
			client.NetworkEntityId = message.entityIdSelf;
			client.SpawnPoint = new PlayerLocation(message.position.X, message.position.Y, message.position.Z);
			client.CurrentLocation = new PlayerLocation(client.SpawnPoint, message.rotation.X, message.rotation.X, message.rotation.Y);

			// The LEVEL spawn, distinct from the player position above: plugins (Plotter) persist
			// per-player spawns, so position is wherever this player last stood, while this is the
			// world's fixed point. The emulator anchors its walk band on it.
			client.WorldSpawn = new Vector3(
				message.settings.defaultSpawnBlockPosition.X,
				message.settings.defaultSpawnBlockPosition.Y,
				message.settings.defaultSpawnBlockPosition.Z);

			client.LevelInfo.LevelName = message.levelName;
			client.LevelInfo.Version = 19133;
			client.LevelInfo.GameType = (int) message.settings.gameType;

			var packet = McpeRequestChunkRadius.CreateObject();
			// Whatever the client was configured with (bots take it from the CLI), floored at 4:
			// the join-burst radius that must be covered for the client to spawn.
			client.ChunkRadius = Math.Max(4, client.ChunkRadius);
			packet.chunkRadius = client.ChunkRadius;
			packet.maxRadius = 32;

			client.SendPacket(packet);

			// A real client opens its loading screen right after requesting the chunk radius
			// (captured live); the matching type 2 close is sent on PlayStatus(3).
			var loadingScreen = McpeServerBoundLoadingScreen.CreateObject();
			loadingScreen.type = 1;
			loadingScreen.loadingScreenId = null;
			client.SendPacket(loadingScreen);
		}

		public virtual void HandleMcpeAddPlayer(McpeAddPlayer message)
		{
		}

		public virtual void HandleMcpeAddEntity(McpeAddEntity message)
		{
		}

		public virtual void HandleMcpeRemoveEntity(McpeRemoveEntity message)
		{
		}

		public virtual void HandleMcpeAddItemEntity(McpeAddItemEntity message)
		{
		}

		public virtual void HandleMcpeTakeItemEntity(McpeTakeItemEntity message)
		{
		}

		public virtual void HandleMcpeServerPlayerPostMovePosition(McpeServerPlayerPostMovePosition message)
		{
		}

		public virtual void HandleMcpeMoveEntity(McpeMoveEntity message)
		{
		}

		public virtual void HandleMcpeMovePlayer(McpeMovePlayer message)
		{
			if (message.runtimeEntityId != Client.EntityId) return;

			Client.CurrentLocation = new PlayerLocation(message.position.X, message.position.Y, message.position.Z);

			//Client.LevelInfo.SpawnX = (int) message.position.X;
			//Client.LevelInfo.SpawnY = (int) message.position.Y;
			//Client.LevelInfo.SpawnZ = (int) message.position.Z;

			//Client.SendMcpeMovePlayer();
		}

		public virtual void HandleMcpeUpdateBlock(McpeUpdateBlock message)
		{
		}

		public virtual void HandleMcpeAddPainting(McpeAddPainting message)
		{
		}

		public virtual void HandleMcpeLevelEvent(McpeLevelEvent message)
		{
		}

		public virtual void HandleMcpeBlockEvent(McpeBlockEvent message)
		{
		}

		public virtual void HandleMcpeEntityEvent(McpeEntityEvent message)
		{
		}

		public virtual void HandleMcpeMobEffect(McpeMobEffect message)
		{
		}

		public virtual void HandleMcpeUpdateAttributes(McpeUpdateAttributes message)
		{
		}

		public virtual void HandleMcpeInventoryTransaction(McpeInventoryTransaction message)
		{
		}

		public virtual void HandleMcpeMobEquipment(McpeMobEquipment message)
		{
		}

		public virtual void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message)
		{
		}

		public virtual void HandleMcpeInteract(McpeInteract message)
		{
		}

		public virtual void HandleMcpeHurtArmor(McpeHurtArmor message)
		{
		}

		public virtual void HandleMcpeSetEntityData(McpeSetEntityData message)
		{
		}

		public virtual void HandleMcpeSetEntityMotion(McpeSetEntityMotion message)
		{
		}

		public virtual void HandleMcpeSetEntityLink(McpeSetEntityLink message)
		{
		}

		public virtual void HandleMcpeSetHealth(McpeSetHealth message)
		{
		}

		public virtual void HandleMcpeSetSpawnPosition(McpeSetSpawnPosition message)
		{
			Client.SpawnPoint = new Vector3(message.coordinates.X, message.coordinates.Y, message.coordinates.Z);
			Client.LevelInfo.SpawnX = (int) Client.SpawnPoint.X;
			Client.LevelInfo.SpawnY = (int) Client.SpawnPoint.Y;
			Client.LevelInfo.SpawnZ = (int) Client.SpawnPoint.Z;
		}

		public virtual void HandleMcpeAnimate(McpeAnimate message)
		{
		}

		public virtual void HandleMcpeRespawn(McpeRespawn message)
		{
			Client.CurrentLocation = new PlayerLocation(message.x, message.y, message.z);
		}

		public virtual void HandleMcpeContainerOpen(McpeContainerOpen message)
		{
		}

		public virtual void HandleMcpeContainerClose(McpeContainerClose message)
		{
		}

		public virtual void HandleMcpePlayerHotbar(McpePlayerHotbar message)
		{
		}

		public virtual void HandleMcpeInventoryContent(McpeInventoryContent message)
		{
		}

		public virtual void HandleMcpeInventorySlot(McpeInventorySlot message)
		{
		}

		public virtual void HandleMcpeContainerSetData(McpeContainerSetData message)
		{
		}

		public virtual void HandleMcpeCraftingData(McpeCraftingData message)
		{
		}

		public virtual void HandleMcpeGuiDataPickItem(McpeGuiDataPickItem message)
		{
		}


		public virtual void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
		}

		/// <summary>
		///     Queues the verdicts owed for a batch of announced hashes, and sends them straight
		///     away unless the consumer batches on its own tick.
		/// </summary>
		private void AnswerVerdicts(List<ulong> hits, List<ulong> misses)
		{
			foreach (ulong hash in hits) Client.PendingBlobHits.Enqueue(hash);
			foreach (ulong hash in misses) Client.PendingBlobMisses.Enqueue(hash);

			if (!Client.BatchChunkResponses) Client.FlushChunkResponses();
		}

		/// <summary>
		///     A column in any of its three forms: legacy push (everything inline), cached push
		///     (every section announced by hash) and the skeleton the join burst uses (biomes only,
		///     block data asked for a section at a time). The cache sorts out which one this is,
		///     answers for the hashes it announces, and holds what arrives.
		/// </summary>
		public virtual void HandleMcpeLevelChunk(McpeLevelChunk message)
		{
			// The publisher's acceptance window, first thing and cheap: "this is the area I
			// will publish chunks for" - an arrival outside it is an in-flight stray from a
			// window the stream moved past. Discard on receive, no further work. The window has
			// vanilla's shape (IsWithinView), so a column the server streams inside its own stamp
			// is never rejected here; one outside it is the server's defect, since a dropped
			// column is never sent again.
			if (Client.PublishedRadiusChunks > 0)
			{
				var arrived = new ChunkCoordinates(message.chunkPosition.x, message.chunkPosition.z);
				if (!arrived.IsWithinView(Client.PublishedCenter, Client.PublishedRadiusChunks)) return;
			}

			var hits = new List<ulong>();
			var misses = new List<ulong>();

			// The sub-chunks are asked for once per column. A real client keeps what it was sent;
			// re-asking for all 24 sections every time the server re-pushes a column is what made a
			// walking bot pull its whole surroundings again on every chunk boundary it crossed.
			bool needsRequest = Client.ChunkCache.OnLevelChunk(message, hits, misses);

			AnswerVerdicts(hits, misses);

			if (!needsRequest) return;

			Client.PendingSubChunkColumns.Enqueue((message.chunkPosition.x, message.chunkPosition.z, message.clientRequestSubchunkLimit.Value, message.dimension));

			if (!Client.BatchChunkResponses) Client.FlushChunkResponses();
		}

		public virtual void HandleMcpeSetCommandsEnabled(McpeSetCommandsEnabled message)
		{
		}

		public virtual void HandleMcpeSetDifficulty(McpeSetDifficulty message)
		{
		}

		public virtual void HandleMcpeChangeDimension(McpeChangeDimension message)
		{
			Thread.Sleep(3000);

			var action = McpePlayerAction.CreateObject();
			action.runtimeEntityId = Client.EntityId;
			action.actionId = (int) PlayerAction.DimensionChangeAck;
			Client.SendPacket(action);
		}

		public virtual void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message)
		{
		}

		public virtual void HandleMcpePlayerList(McpePlayerList message)
		{
		}

		public virtual void HandleMcpeSimpleEvent(McpeSimpleEvent message)
		{
		}

		public virtual void HandleMcpeTelemetryEvent(McpeTelemetryEvent message)
		{
		}

		public virtual void HandleMcpeSpawnExperienceOrb(McpeSpawnExperienceOrb message)
		{
		}

		public virtual void HandleMcpeClientboundMapItemData(McpeClientboundMapItemData message)
		{
		}

		public virtual void HandleMcpeMapInfoRequest(McpeMapInfoRequest message)
		{
		}

		public virtual void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
		{
		}

		public virtual void HandleMcpeChunkRadiusUpdate(McpeChunkRadiusUpdate message)
		{
			// The granted radius is the server's fact, not the request: the window the client
			// keeps (and forgets by) is this one, whatever it asked for.
			Client.ChunkRadius = message.chunkRadius;
		}

		public virtual void HandleMcpeGameRulesChanged(McpeGameRulesChanged message)
		{
		}

		public virtual void HandleMcpeCamera(McpeCamera message)
		{
		}

		public virtual void HandleMcpeBossEvent(McpeBossEvent message)
		{
		}

		public virtual void HandleMcpeShowCredits(McpeShowCredits message)
		{
		}

		public virtual void HandleMcpeAvailableCommands(McpeAvailableCommands message)
		{
		}

		public virtual void HandleMcpeCommandOutput(McpeCommandOutput message)
		{
		}

		public virtual void HandleMcpeUpdateTrade(McpeUpdateTrade message)
		{
		}

		public virtual void HandleMcpeUpdateEquipment(McpeUpdateEquipment message)
		{
		}

		private Dictionary<string, uint> _resourcePackDataInfos = new Dictionary<string, uint>();

		public virtual void HandleMcpeResourcePackDataInfo(McpeResourcePackDataInfo message)
		{
			var request = new McpeResourcePackChunkRequest();
			request.packageId = message.packageId;
			request.chunkIndex = 0;
			Client.SendPacket(request);
			_resourcePackDataInfos.Add(message.packageId, message.chunkCount);
		}

		public virtual void HandleMcpeResourcePackChunkData(McpeResourcePackChunkData message)
		{
			if (message.chunkIndex + 1 < _resourcePackDataInfos[message.packageId])
			{
				var request = new McpeResourcePackChunkRequest();
				request.packageId = message.packageId;
				request.chunkIndex = message.chunkIndex + 1;
				Client.SendPacket(request);
			}
			else
			{
				_resourcePackDataInfos.Remove(message.packageId);
			}

			if (_resourcePackDataInfos.Count == 0)
			{
				var response = new McpeResourcePackClientResponse();
				response.response = new ResourcePackClientResponseDownloadingFinished();
				Client.SendPacket(response);
			}
		}

		public virtual void HandleMcpeTransfer(McpeTransfer message)
		{
		}

		public virtual void HandleMcpePlaySound(McpePlaySound message)
		{
		}

		public virtual void HandleMcpeStopSound(McpeStopSound message)
		{
		}

		public virtual void HandleMcpeSetTitle(McpeSetTitle message)
		{
		}

		public virtual void HandleMcpeAddBehaviorTree(McpeAddBehaviorTree message)
		{
		}

		public virtual void HandleMcpeStructureBlockUpdate(McpeStructureBlockUpdate message)
		{
		}

		public virtual void HandleMcpeShowStoreOffer(McpeShowStoreOffer message)
		{
		}

		public virtual void HandleMcpePlayerSkin(McpePlayerSkin message)
		{
		}

		public virtual void HandleMcpeSubClientLogin(McpeSubClientLogin message)
		{
		}

		public virtual void HandleMcpeInitiateWebSocketConnection(McpeInitiateWebSocketConnection message)
		{
		}

		public virtual void HandleMcpeSetLastHurtBy(McpeSetLastHurtBy message)
		{
		}

		public virtual void HandleMcpeBookEdit(McpeBookEdit message)
		{
		}

		public virtual void HandleMcpeNpcRequest(McpeNpcRequest message)
		{
		}

		public virtual void HandleMcpeModalFormRequest(McpeModalFormRequest message)
		{
		}

		public virtual void HandleMcpeServerSettingsResponse(McpeServerSettingsResponse message)
		{
		}

		public virtual void HandleMcpeShowProfile(McpeShowProfile message)
		{
		}

		public virtual void HandleMcpeSetDefaultGameType(McpeSetDefaultGameType message)
		{
		}

		public virtual void HandleMcpeRemoveObjective(McpeRemoveObjective message)
		{
		}

		public virtual void HandleMcpeSetDisplayObjective(McpeSetDisplayObjective message)
		{
		}

		public virtual void HandleMcpeSetScore(McpeSetScore message)
		{
		}

		public virtual void HandleMcpeLabTable(McpeLabTable message)
		{
		}

		public virtual void HandleMcpeUpdateBlockSynced(McpeUpdateBlockSynced message)
		{
		}

		public virtual void HandleMcpeMoveEntityDelta(McpeMoveEntityDelta message)
		{
		}

		public virtual void HandleMcpeSetScoreboardIdentity(McpeSetScoreboardIdentity message)
		{
		}

		public virtual void HandleMcpeUpdateSoftEnum(McpeUpdateSoftEnum message)
		{
		}

		public virtual void HandleMcpeNetworkStackLatency(McpeNetworkStackLatency message)
		{
			var packet = McpeNetworkStackLatency.CreateObject();
			packet.timestamp = message.timestamp;
			packet.unknownFlag = 0;

			Client.SendPacket(packet);
		}

		public virtual void HandleMcpeSpawnParticleEffect(McpeSpawnParticleEffect message)
		{
		}

		public virtual void HandleMcpeAvailableEntityIdentifiers(McpeAvailableEntityIdentifiers message)
		{
		}

		public virtual void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message)
		{
			// The acceptance window, NOT an eviction order: "this is the area being streamed,
			// ignore anything arriving outside it". Its job is discarding in-flight columns
			// the stream has moved past. What the client HOLDS is a separate mechanism
			// entirely: the disc around its own position and radius, forgotten position-driven
			// as it moves (see BotWalker).
			Client.PublishedCenter = new ChunkCoordinates(message.coordinates.X >> 4, message.coordinates.Z >> 4);
			Client.PublishedRadiusChunks = (int) (message.radius >> 4);
		}

		public virtual void HandleMcpeBiomeDefinitionList(McpeBiomeDefinitionList message)
		{
		}

		public virtual void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message)
		{
		}

		public virtual void HandleMcpeLevelEventGeneric(McpeLevelEventGeneric message)
		{
		}

		public virtual void HandleMcpeLecternUpdate(McpeLecternUpdate message)
		{
		}

		public virtual void HandleMcpeClientCacheStatus(McpeClientCacheStatus message)
		{
		}

		public virtual void HandleMcpeOnScreenTextureAnimation(McpeOnScreenTextureAnimation message)
		{
		}

		public virtual void HandleMcpeMapCreateLockedCopy(McpeMapCreateLockedCopy message)
		{
		}

		public virtual void HandleMcpeStructureTemplateDataExportRequest(McpeStructureTemplateDataExportRequest message)
		{
		}

		public virtual void HandleMcpeStructureTemplateDataExportResponse(McpeStructureTemplateDataExportResponse message)
		{
		}

		public virtual void HandleMcpeClientCacheBlobStatus(McpeClientCacheBlobStatus message)
		{
		}

		/// <summary>
		///     The bytes behind the hashes we reported as missing. This is where terrain actually
		///     arrives in both cached flows, so a client that drops these has announced chunks it can
		///     never draw.
		/// </summary>
		public virtual void HandleMcpeClientCacheMissResponse(McpeClientCacheMissResponse message)
		{
			Client.ChunkCache.OnBlobPayloads(message.blobs);
		}

		public virtual void HandleMcpeEducationSettings(McpeEducationSettings message)
		{
		}

		public virtual void HandleMcpeEmote(McpeEmote message)
		{
		}

		public virtual void HandleMcpeMultiplayerSettings(McpeMultiplayerSettings message)
		{
		}

		public virtual void HandleMcpeCompletedUsingItem(McpeCompletedUsingItem message)
		{
		}

		public virtual void HandleMcpeNetworkSettings(McpeNetworkSettings message)
		{
			if (message.compressionAlgorithm == 1) Log.Warn("Server negotiated snappy compression, which is not implemented. Expect failures.");

			Client.SendLogin(Client.Username);
		}

		public virtual void HandleMcpeCreativeContent(McpeCreativeContent message)
		{
		}

		public void HandleMcpePlayerEnchantOptions(McpePlayerEnchantOptions message)
		{
		}

		public virtual void HandleMcpeItemStackResponse(McpeItemStackResponse message)
		{
		}

		public virtual void HandleMcpePlayerArmorDamage(McpePlayerArmorDamage message)
		{
		}

		public virtual void HandleMcpeCodeBuilder(McpeCodeBuilder message)
		{
		}

		public virtual void HandleMcpePositionTrackingDbServerBroadcast(McpePositionTrackingDbServerBroadcast message)
		{
		}

		public virtual void HandleMcpeDebugInfo(McpeDebugInfo message)
		{
		}

		public virtual void HandleMcpeMotionPredictionHints(McpeMotionPredictionHints message)
		{
		}

		public virtual void HandleMcpeAnimateEntity(McpeAnimateEntity message)
		{
		}

		/// <inheritdoc />
		public virtual void HandleMcpeCorrectPlayerMovePrediction(McpeCorrectPlayerMovePrediction message)
		{
		}

		/// <inheritdoc />
		public virtual void HandleMcpeItemComponent(McpeItemComponent message)
		{

		}

		/// <inheritdoc />
		public virtual void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocksPacket message)
		{

		}

		/// <inheritdoc />
		public virtual void HandleMcpeSubChunkPacket(McpeSubChunkPacket message)
		{
			// The answers to what this client asked for. Cache-enabled, the entries announce section
			// blobs by hash exactly like a cached LevelChunk announces the biome blob, and they get
			// the same verdicts: a cold client misses and the server answers with the payload, which
			// is most of the terrain bandwidth a real join costs. Uncached, the section payload is in
			// the entry itself.
			var hits = new List<ulong>();
			var misses = new List<ulong>();

			Client.ChunkCache.OnSubChunkResponse(message, hits, misses);

			if (hits.Count + misses.Count == 0) return;

			AnswerVerdicts(hits, misses);
		}

		/// <inheritdoc />
		public void HandleMcpeDimensionData(McpeDimensionData message)
		{

		}

		public virtual void HandleMcpePlayerStartItemCooldown(McpePlayerStartItemCooldown message)
		{
		}

		public virtual void HandleMcpeScriptMessage(McpeScriptMessage message)
		{
		}

		public virtual void HandleMcpeTickingAreasLoadStatus(McpeTickingAreasLoadStatus message)
		{
		}

		public virtual void HandleMcpeAgentActionEvent(McpeAgentActionEvent message)
		{
		}

		public virtual void HandleMcpeLessonProgress(McpeLessonProgress message)
		{
		}

		public virtual void HandleMcpeToastRequest(McpeToastRequest message)
		{
		}

		public virtual void HandleMcpeDeathInfo(McpeDeathInfo message)
		{
		}

		public virtual void HandleMcpeEditorNetwork(McpeEditorNetwork message)
		{
		}

		public virtual void HandleMcpeFeatureRegistry(McpeFeatureRegistry message)
		{
		}

		public virtual void HandleMcpeServerStats(McpeServerStats message)
		{
		}

		public virtual void HandleFtlCreatePlayer(FtlCreatePlayer message)
		{
		}

		public virtual void HandleMcpeTrimData(McpeTrimData message)
		{
		}

		public virtual void HandleMcpeJigsawStructureData(McpeJigsawStructureData message)
		{
		}

		public virtual void HandleMcpeCurrentStructureFeature(McpeCurrentStructureFeature message)
		{
		}

		public virtual void HandleMcpeSetHud(McpeSetHud message)
		{
		}

		public virtual void HandleMcpeAwardAchievement(McpeAwardAchievement message)
		{
		}

		public virtual void HandleMcpeClientboundCloseForm(McpeClientboundCloseForm message)
		{
		}

		public virtual void HandleMcpeCameraAimAssist(McpeCameraAimAssist message)
		{
		}

		public virtual void HandleMcpeContainerRegistryCleanup(McpeContainerRegistryCleanup message)
		{
		}

		public virtual void HandleMcpeMovementEffect(McpeMovementEffect message)
		{
		}

		public virtual void HandleMcpeCameraAimAssistPresets(McpeCameraAimAssistPresets message)
		{
		}

		public virtual void HandleMcpeCameraAimAssistActorPriority(McpeCameraAimAssistActorPriority message)
		{
		}

		public virtual void HandleMcpeClientboundAttributeLayerSync(McpeClientboundAttributeLayerSync message)
		{
		}

		public virtual void HandleMcpeServerStoreInfo(McpeServerStoreInfo message)
		{
		}

		public virtual void HandleMcpeServerPresenceInfo(McpeServerPresenceInfo message)
		{
		}

		public virtual void HandleMcpePlayerVideoCapture(McpePlayerVideoCapture message)
		{
		}

		public virtual void HandleMcpePlayerUpdateEntityOverrides(McpePlayerUpdateEntityOverrides message)
		{
		}

		public virtual void HandleMcpeClientboundControlSchemeSet(McpeClientboundControlSchemeSet message)
		{
		}

		public virtual void HandleMcpePrimitiveShapes(McpePrimitiveShapes message)
		{
		}

		public virtual void HandleMcpeClientboundDataStore(McpeClientboundDataStore message)
		{
		}

		public virtual void HandleMcpeGraphicsOverrideParameter(McpeGraphicsOverrideParameter message)
		{
		}

		public virtual void HandleMcpeClientboundDataDrivenUiShowScreen(McpeClientboundDataDrivenUiShowScreen message)
		{
		}

		public virtual void HandleMcpeClientboundDataDrivenUiCloseScreen(McpeClientboundDataDrivenUiCloseScreen message)
		{
		}

		public virtual void HandleMcpeClientboundDataDrivenUiReload(McpeClientboundDataDrivenUiReload message)
		{
		}

		public virtual void HandleMcpeClientboundTextureShift(McpeClientboundTextureShift message)
		{
		}

		public virtual void HandleMcpeVoxelShapes(McpeVoxelShapes message)
		{
		}

		public virtual void HandleMcpeCameraSpline(McpeCameraSpline message)
		{
		}

		public virtual void HandleMcpeClientboundUpdateSoundData(McpeClientboundUpdateSoundData message)
		{
		}

		public virtual void HandleMcpeRecordStarted(McpeRecordStarted message)
		{
		}

		public virtual void HandleMcpeSetPlayerFurnaceOptions(McpeSetPlayerFurnaceOptions message)
		{
		}

		public virtual void HandleMcpeSendPartyDestinationCookie(McpeSendPartyDestinationCookie message)
		{
		}

		public virtual void HandleMcpeSyncWorldClocks(McpeSyncWorldClocks message)
		{
			foreach (var clock in message.Clocks)
			{
				Log.Warn($"WorldClock registry: id={clock.Id} name={clock.Name} time={clock.Time} paused={clock.Paused} markers={clock.TimeMarkers.Count}");
			}

			foreach (var state in message.SyncStates)
			{
				Log.Warn($"WorldClock state: id={state.ClockId} time={state.Time} paused={state.Paused}");
			}
		}

		public virtual void HandleMcpePlayerFog(McpePlayerFog message)
		{
		}

		public virtual void HandleMcpeSyncEntityProperty(McpeSyncEntityProperty message)
		{
		}

		public virtual void HandleMcpeClientboundDebugRenderer(McpeClientboundDebugRenderer message)
		{
		}

		public virtual void HandleMcpeAddVolumeEntity(McpeAddVolumeEntity message)
		{
		}

		public virtual void HandleMcpeRemoveVolumeEntity(McpeRemoveVolumeEntity message)
		{
		}

		public virtual void HandleMcpeSimulationType(McpeSimulationType message)
		{
		}

		public virtual void HandleMcpeNpcDialogue(McpeNpcDialogue message)
		{
		}

		public virtual void HandleMcpeEduUriResource(McpeEduUriResource message)
		{
		}

		public virtual void HandleMcpeUpdateAbilities(McpeUpdateAbilities message)
		{
			Client.UserPermission = (CommandPermission) message.commandPermission;
		}

		public virtual void HandleMcpeUpdateAdventureSettings(McpeUpdateAdventureSettings message)
		{
		}

		public virtual void HandleMcpeCameraPresets(McpeCameraPresets message)
		{
		}

		public virtual void HandleMcpeCameraInstruction(McpeCameraInstruction message)
		{
		}

		public virtual void HandleMcpeCameraShake(McpeCameraShake message)
		{
		}

		public virtual void HandleMcpePlayerLocation(McpePlayerLocation message)
		{
		}

		public virtual void HandleMcpeLocatorBar(McpeLocatorBar message)
		{
		}

		public virtual void HandleMcpeGameTestResults(McpeGameTestResults message)
		{
		}

		public virtual void HandleMcpeUpdateClientInputLocks(McpeUpdateClientInputLocks message)
		{
		}

		public virtual void HandleMcpeUnlockedRecipes(McpeUnlockedRecipes message)
		{
		}

		public virtual void HandleMcpeOpenSign(McpeOpenSign message)
		{
		}

		public virtual void HandleMcpeAgentAnimation(McpeAgentAnimation message)
		{
		}

		public virtual void HandleMcpeRefreshEntitlements(McpeRefreshEntitlements message)
		{
		}
	}

	public class DefaultMessageHandler : McpeClientMessageHandlerBase
	{
		public DefaultMessageHandler(MiNetClient client) : base(client)
		{
		}
	}
}

