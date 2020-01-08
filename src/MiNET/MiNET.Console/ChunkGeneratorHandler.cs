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
		private BlockPallet _blockPallet;
		private HashSet<BlockRecord> _internalStates;

		public ChunkGeneratorHandler(MiNetClient client) : base(client)
		{
		}

		public override void HandleMcpeText(McpeText message)
		{
		}

		public override void HandleMcpeSetTime(McpeSetTime message)
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

			_blockPallet = message.blockPallet;

			_internalStates = new HashSet<BlockRecord>(BlockFactory.BlockPallet);

			//ClientUtils.SaveLevel(_level);

			{
				int viewDistance = Config.GetProperty("ViewDistance", 11);

				var packet = McpeRequestChunkRadius.CreateObject();
				Client.ChunkRadius = viewDistance;
				packet.chunkRadius = Client.ChunkRadius;

				Client.SendPacket(packet);
			}
		}

		public override void HandleMcpeAddPlayer(McpeAddPlayer message)
		{
		}

		public override void HandleMcpeAddEntity(McpeAddEntity message)
		{
		}

		public override void HandleMcpeRemoveEntity(McpeRemoveEntity message)
		{
		}

		public override void HandleMcpeAddItemEntity(McpeAddItemEntity message)
		{
		}

		public override void HandleMcpeTakeItemEntity(McpeTakeItemEntity message)
		{
		}

		public override void HandleMcpeMoveEntity(McpeMoveEntity message)
		{
		}

		public override void HandleMcpeRiderJump(McpeRiderJump message)
		{
		}

		public override void HandleMcpeUpdateBlock(McpeUpdateBlock message)
		{
		}

		public override void HandleMcpeAddPainting(McpeAddPainting message)
		{
		}

		public override void HandleMcpeTickSync(McpeTickSync message)
		{
		}

		public override void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message)
		{
		}

		public override void HandleMcpeLevelEvent(McpeLevelEvent message)
		{
		}

		public override void HandleMcpeBlockEvent(McpeBlockEvent message)
		{
		}

		public override void HandleMcpeEntityEvent(McpeEntityEvent message)
		{
		}

		public override void HandleMcpeMobEffect(McpeMobEffect message)
		{
		}

		public override void HandleMcpeUpdateAttributes(McpeUpdateAttributes message)
		{
		}

		public override void HandleMcpeInventoryTransaction(McpeInventoryTransaction message)
		{
		}

		public override void HandleMcpeMobEquipment(McpeMobEquipment message)
		{
		}

		public override void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message)
		{
		}

		public override void HandleMcpeInteract(McpeInteract message)
		{
		}

		public override void HandleMcpeHurtArmor(McpeHurtArmor message)
		{
		}

		public override void HandleMcpeSetEntityData(McpeSetEntityData message)
		{
		}

		public override void HandleMcpeSetEntityMotion(McpeSetEntityMotion message)
		{
		}

		public override void HandleMcpeSetEntityLink(McpeSetEntityLink message)
		{
		}

		public override void HandleMcpeSetHealth(McpeSetHealth message)
		{
		}

		public override void HandleMcpeSetSpawnPosition(McpeSetSpawnPosition message)
		{
			Client.SpawnPoint = new Vector3(message.coordinates.X, message.coordinates.Y, message.coordinates.Z);
			Client.LevelInfo.SpawnX = (int) Client.SpawnPoint.X;
			Client.LevelInfo.SpawnY = (int) Client.SpawnPoint.Y;
			Client.LevelInfo.SpawnZ = (int) Client.SpawnPoint.Z;
		}

		public override void HandleMcpeAnimate(McpeAnimate message)
		{
		}

		public override void HandleMcpeRespawn(McpeRespawn message)
		{
		}

		public override void HandleMcpeContainerOpen(McpeContainerOpen message)
		{
		}

		public override void HandleMcpeContainerClose(McpeContainerClose message)
		{
		}

		public override void HandleMcpePlayerHotbar(McpePlayerHotbar message)
		{
		}

		public override void HandleMcpeInventoryContent(McpeInventoryContent message)
		{
		}

		public override void HandleMcpeInventorySlot(McpeInventorySlot message)
		{
		}

		public override void HandleMcpeContainerSetData(McpeContainerSetData message)
		{
		}

		public override void HandleMcpeCraftingData(McpeCraftingData message)
		{
		}

		public override void HandleMcpeCraftingEvent(McpeCraftingEvent message)
		{
		}

		public override void HandleMcpeGuiDataPickItem(McpeGuiDataPickItem message)
		{
		}

		public override void HandleMcpeAdventureSettings(McpeAdventureSettings message)
		{
		}

		public override void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
		}

		public override void HandleMcpeLevelChunk(McpeLevelChunk message)
		{
			if (!Client._chunks.ContainsKey(new Tuple<int, int>(message.chunkX, message.chunkZ)))
			{
				//Log.Debug($"Chunk X={message.chunkX}, Z={message.chunkZ}, size={message.chunkData.Length}, Count={Client._chunks.Count}");

				try
				{
					ChunkColumn chunk = ClientUtils.DecodeChunkColumn((int) message.subChunkCount, message.chunkData, _blockPallet, _internalStates);
					if (chunk != null)
					{
						chunk.x = message.chunkX;
						chunk.z = message.chunkZ;
						chunk.RecalcHeight();
						Client._chunks.TryAdd(new Tuple<int, int>(message.chunkX, message.chunkZ), chunk);
						//Log.Debug($"Added parsed bedrock chunk X={chunk.x}, Z={chunk.z}");
					}
				}
				catch (Exception e)
				{
					Log.Error("Reading chunk", e);
				}
			}
		}

		public override void HandleMcpeSetCommandsEnabled(McpeSetCommandsEnabled message)
		{
		}

		public override void HandleMcpeSetDifficulty(McpeSetDifficulty message)
		{
		}

		public override void HandleMcpeChangeDimension(McpeChangeDimension message)
		{
		}

		public override void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message)
		{
		}

		public override void HandleMcpePlayerList(McpePlayerList message)
		{
		}

		public override void HandleMcpeSimpleEvent(McpeSimpleEvent message)
		{
		}

		public override void HandleMcpeTelemetryEvent(McpeTelemetryEvent message)
		{
		}

		public override void HandleMcpeSpawnExperienceOrb(McpeSpawnExperienceOrb message)
		{
		}

		public override void HandleMcpeClientboundMapItemData(McpeClientboundMapItemData message)
		{
		}

		public override void HandleMcpeMapInfoRequest(McpeMapInfoRequest message)
		{
		}

		public override void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
		{
		}

		public override void HandleMcpeChunkRadiusUpdate(McpeChunkRadiusUpdate message)
		{
		}

		public override void HandleMcpeItemFrameDropItem(McpeItemFrameDropItem message)
		{
		}

		public override void HandleMcpeGameRulesChanged(McpeGameRulesChanged message)
		{
		}

		public override void HandleMcpeCamera(McpeCamera message)
		{
		}

		public override void HandleMcpeBossEvent(McpeBossEvent message)
		{
		}

		public override void HandleMcpeShowCredits(McpeShowCredits message)
		{
		}

		public override void HandleMcpeAvailableCommands(McpeAvailableCommands message)
		{
		}

		public override void HandleMcpeCommandOutput(McpeCommandOutput message)
		{
		}

		public override void HandleMcpeUpdateTrade(McpeUpdateTrade message)
		{
		}

		public override void HandleMcpeUpdateEquipment(McpeUpdateEquipment message)
		{
		}

		public override void HandleMcpeResourcePackDataInfo(McpeResourcePackDataInfo message)
		{
		}

		public override void HandleMcpeTransfer(McpeTransfer message)
		{
		}

		public override void HandleMcpePlaySound(McpePlaySound message)
		{
		}

		public override void HandleMcpeStopSound(McpeStopSound message)
		{
		}

		public override void HandleMcpeSetTitle(McpeSetTitle message)
		{
		}

		public override void HandleMcpeAddBehaviorTree(McpeAddBehaviorTree message)
		{
		}

		public override void HandleMcpeStructureBlockUpdate(McpeStructureBlockUpdate message)
		{
		}

		public override void HandleMcpeShowStoreOffer(McpeShowStoreOffer message)
		{
		}

		public override void HandleMcpePlayerSkin(McpePlayerSkin message)
		{
		}

		public override void HandleMcpeSubClientLogin(McpeSubClientLogin message)
		{
		}

		public override void HandleMcpeInitiateWebSocketConnection(McpeInitiateWebSocketConnection message)
		{
		}

		public override void HandleMcpeSetLastHurtBy(McpeSetLastHurtBy message)
		{
		}

		public override void HandleMcpeBookEdit(McpeBookEdit message)
		{
		}

		public override void HandleMcpeNpcRequest(McpeNpcRequest message)
		{
		}

		public override void HandleMcpeModalFormRequest(McpeModalFormRequest message)
		{
		}

		public override void HandleMcpeServerSettingsResponse(McpeServerSettingsResponse message)
		{
		}

		public override void HandleMcpeShowProfile(McpeShowProfile message)
		{
		}

		public override void HandleMcpeSetDefaultGameType(McpeSetDefaultGameType message)
		{
		}

		public override void HandleMcpeRemoveObjective(McpeRemoveObjective message)
		{
		}

		public override void HandleMcpeSetDisplayObjective(McpeSetDisplayObjective message)
		{
		}

		public override void HandleMcpeSetScore(McpeSetScore message)
		{
		}

		public override void HandleMcpeLabTable(McpeLabTable message)
		{
		}

		public override void HandleMcpeUpdateBlockSynced(McpeUpdateBlockSynced message)
		{
		}

		public override void HandleMcpeMoveEntityDelta(McpeMoveEntityDelta message)
		{
		}

		public override void HandleMcpeSetScoreboardIdentityPacket(McpeSetScoreboardIdentityPacket message)
		{
		}

		public override void HandleMcpeUpdateSoftEnumPacket(McpeUpdateSoftEnumPacket message)
		{
		}

		public override void HandleMcpeScriptCustomEventPacket(McpeScriptCustomEventPacket message)
		{
		}

		public override void HandleMcpeLevelSoundEventOld(McpeLevelSoundEventOld message)
		{
		}

		public override void HandleMcpeSpawnParticleEffect(McpeSpawnParticleEffect message)
		{
		}

		public override void HandleMcpeAvailableEntityIdentifiers(McpeAvailableEntityIdentifiers message)
		{
		}

		public override void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message)
		{
		}

		public override void HandleMcpeBiomeDefinitionList(McpeBiomeDefinitionList message)
		{
		}

		public override void HandleMcpeNetworkSettingsPacket(McpeNetworkSettingsPacket message)
		{
		}

		public override void HandleFtlCreatePlayer(FtlCreatePlayer message)
		{
		}

		public override void HandleMcpeLevelSoundEventV2(McpeLevelSoundEventV2 message)
		{
		}

		public override void HandleMcpeLevelEventGeneric(McpeLevelEventGeneric message)
		{
		}

		public override void HandleMcpeLecternUpdate(McpeLecternUpdate message)
		{
		}

		public override void HandleMcpeVideoStreamConnect(McpeVideoStreamConnect message)
		{
		}

		public override void HandleMcpeClientCacheStatus(McpeClientCacheStatus message)
		{
		}

		public override void HandleMcpeOnScreenTextureAnimation(McpeOnScreenTextureAnimation message)
		{
		}

		public override void HandleMcpeMapCreateLockedCopy(McpeMapCreateLockedCopy message)
		{
		}

		public override void HandleMcpeStructureTemplateDataExportRequest(McpeStructureTemplateDataExportRequest message)
		{
		}

		public override void HandleMcpeStructureTemplateDataExportResponse(McpeStructureTemplateDataExportResponse message)
		{
		}

		public override void HandleMcpeUpdateBlockProperties(McpeUpdateBlockProperties message)
		{
		}

		public override void HandleMcpeClientCacheBlobStatus(McpeClientCacheBlobStatus message)
		{
		}

		public override void HandleMcpeClientCacheMissResponse(McpeClientCacheMissResponse message)
		{
		}
	}
}