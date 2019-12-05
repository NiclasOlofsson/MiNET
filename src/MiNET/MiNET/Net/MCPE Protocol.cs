#region LICENSE

// The contents of this file are subject to the Common Public Attribution// The contents of this file are subject to the Common Public Attribution
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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

using System;
using System.Net;
using System.Numerics;
using System.Threading;
using MiNET.Utils; 
using MiNET.Utils.Skins;
using MiNET.Items;
using MiNET.Crafting;
using little = MiNET.Utils.Int24; // friendly name
using LongString = System.String;

namespace MiNET.Net
{
	public class McpeProtocolInfo
	{
		public const int ProtocolVersion = 388;
		public const string GameVersion = "1.13.0";
	}

	public interface IMcpeMessageHandler
	{
		void Disconnect(string reason, bool sendDisconnect = true);

		void HandleMcpeLogin(McpeLogin message);
		void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message);
		void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message);
		void HandleMcpeText(McpeText message);
		void HandleMcpeMoveEntity(McpeMoveEntity message);
		void HandleMcpeMovePlayer(McpeMovePlayer message);
		void HandleMcpeRiderJump(McpeRiderJump message);
		void HandleMcpeLevelSoundEventOld(McpeLevelSoundEventOld message);
		void HandleMcpeEntityEvent(McpeEntityEvent message);
		void HandleMcpeInventoryTransaction(McpeInventoryTransaction message);
		void HandleMcpeMobEquipment(McpeMobEquipment message);
		void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
		void HandleMcpeInteract(McpeInteract message);
		void HandleMcpeBlockPickRequest(McpeBlockPickRequest message);
		void HandleMcpeEntityPickRequest(McpeEntityPickRequest message);
		void HandleMcpePlayerAction(McpePlayerAction message);
		void HandleMcpeEntityFall(McpeEntityFall message);
		void HandleMcpeSetEntityData(McpeSetEntityData message);
		void HandleMcpeSetEntityMotion(McpeSetEntityMotion message);
		void HandleMcpeAnimate(McpeAnimate message);
		void HandleMcpeRespawn(McpeRespawn message);
		void HandleMcpeContainerClose(McpeContainerClose message);
		void HandleMcpePlayerHotbar(McpePlayerHotbar message);
		void HandleMcpeInventoryContent(McpeInventoryContent message);
		void HandleMcpeInventorySlot(McpeInventorySlot message);
		void HandleMcpeCraftingEvent(McpeCraftingEvent message);
		void HandleMcpeAdventureSettings(McpeAdventureSettings message);
		void HandleMcpeBlockEntityData(McpeBlockEntityData message);
		void HandleMcpePlayerInput(McpePlayerInput message);
		void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message);
		void HandleMcpeMapInfoRequest(McpeMapInfoRequest message);
		void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message);
		void HandleMcpeItemFrameDropItem(McpeItemFrameDropItem message);
		void HandleMcpeCommandRequest(McpeCommandRequest message);
		void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message);
		void HandleMcpeResourcePackChunkRequest(McpeResourcePackChunkRequest message);
		void HandleMcpePurchaseReceipt(McpePurchaseReceipt message);
		void HandleMcpePlayerSkin(McpePlayerSkin message);
		void HandleMcpeNpcRequest(McpeNpcRequest message);
		void HandleMcpePhotoTransfer(McpePhotoTransfer message);
		void HandleMcpeModalFormResponse(McpeModalFormResponse message);
		void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message);
		void HandleMcpeLabTable(McpeLabTable message);
		void HandleMcpeSetLocalPlayerAsInitializedPacket(McpeSetLocalPlayerAsInitializedPacket message);
		void HandleMcpeNetworkStackLatencyPacket(McpeNetworkStackLatencyPacket message);
		void HandleMcpeScriptCustomEventPacket(McpeScriptCustomEventPacket message);
		void HandleMcpeLevelSoundEventV2(McpeLevelSoundEventV2 message);
		void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
	}

	public interface IMcpeClientMessageHandler
	{
		void HandleMcpePlayStatus(McpePlayStatus message);
		void HandleMcpeServerToClientHandshake(McpeServerToClientHandshake message);
		void HandleMcpeDisconnect(McpeDisconnect message);
		void HandleMcpeResourcePacksInfo(McpeResourcePacksInfo message);
		void HandleMcpeResourcePackStack(McpeResourcePackStack message);
		void HandleMcpeText(McpeText message);
		void HandleMcpeSetTime(McpeSetTime message);
		void HandleMcpeStartGame(McpeStartGame message);
		void HandleMcpeAddPlayer(McpeAddPlayer message);
		void HandleMcpeAddEntity(McpeAddEntity message);
		void HandleMcpeRemoveEntity(McpeRemoveEntity message);
		void HandleMcpeAddItemEntity(McpeAddItemEntity message);
		void HandleMcpeTakeItemEntity(McpeTakeItemEntity message);
		void HandleMcpeMoveEntity(McpeMoveEntity message);
		void HandleMcpeMovePlayer(McpeMovePlayer message);
		void HandleMcpeRiderJump(McpeRiderJump message);
		void HandleMcpeUpdateBlock(McpeUpdateBlock message);
		void HandleMcpeAddPainting(McpeAddPainting message);
		void HandleMcpeTickSync(McpeTickSync message);
		void HandleMcpeLevelSoundEventOld(McpeLevelSoundEventOld message);
		void HandleMcpeLevelEvent(McpeLevelEvent message);
		void HandleMcpeBlockEvent(McpeBlockEvent message);
		void HandleMcpeEntityEvent(McpeEntityEvent message);
		void HandleMcpeMobEffect(McpeMobEffect message);
		void HandleMcpeUpdateAttributes(McpeUpdateAttributes message);
		void HandleMcpeInventoryTransaction(McpeInventoryTransaction message);
		void HandleMcpeMobEquipment(McpeMobEquipment message);
		void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
		void HandleMcpeInteract(McpeInteract message);
		void HandleMcpeHurtArmor(McpeHurtArmor message);
		void HandleMcpeSetEntityData(McpeSetEntityData message);
		void HandleMcpeSetEntityMotion(McpeSetEntityMotion message);
		void HandleMcpeSetEntityLink(McpeSetEntityLink message);
		void HandleMcpeSetHealth(McpeSetHealth message);
		void HandleMcpeSetSpawnPosition(McpeSetSpawnPosition message);
		void HandleMcpeAnimate(McpeAnimate message);
		void HandleMcpeRespawn(McpeRespawn message);
		void HandleMcpeContainerOpen(McpeContainerOpen message);
		void HandleMcpeContainerClose(McpeContainerClose message);
		void HandleMcpePlayerHotbar(McpePlayerHotbar message);
		void HandleMcpeInventoryContent(McpeInventoryContent message);
		void HandleMcpeInventorySlot(McpeInventorySlot message);
		void HandleMcpeContainerSetData(McpeContainerSetData message);
		void HandleMcpeCraftingData(McpeCraftingData message);
		void HandleMcpeCraftingEvent(McpeCraftingEvent message);
		void HandleMcpeGuiDataPickItem(McpeGuiDataPickItem message);
		void HandleMcpeAdventureSettings(McpeAdventureSettings message);
		void HandleMcpeBlockEntityData(McpeBlockEntityData message);
		void HandleMcpeLevelChunk(McpeLevelChunk message);
		void HandleMcpeSetCommandsEnabled(McpeSetCommandsEnabled message);
		void HandleMcpeSetDifficulty(McpeSetDifficulty message);
		void HandleMcpeChangeDimension(McpeChangeDimension message);
		void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message);
		void HandleMcpePlayerList(McpePlayerList message);
		void HandleMcpeSimpleEvent(McpeSimpleEvent message);
		void HandleMcpeTelemetryEvent(McpeTelemetryEvent message);
		void HandleMcpeSpawnExperienceOrb(McpeSpawnExperienceOrb message);
		void HandleMcpeClientboundMapItemData(McpeClientboundMapItemData message);
		void HandleMcpeMapInfoRequest(McpeMapInfoRequest message);
		void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message);
		void HandleMcpeChunkRadiusUpdate(McpeChunkRadiusUpdate message);
		void HandleMcpeItemFrameDropItem(McpeItemFrameDropItem message);
		void HandleMcpeGameRulesChanged(McpeGameRulesChanged message);
		void HandleMcpeCamera(McpeCamera message);
		void HandleMcpeBossEvent(McpeBossEvent message);
		void HandleMcpeShowCredits(McpeShowCredits message);
		void HandleMcpeAvailableCommands(McpeAvailableCommands message);
		void HandleMcpeCommandOutput(McpeCommandOutput message);
		void HandleMcpeUpdateTrade(McpeUpdateTrade message);
		void HandleMcpeUpdateEquipment(McpeUpdateEquipment message);
		void HandleMcpeResourcePackDataInfo(McpeResourcePackDataInfo message);
		void HandleMcpeResourcePackChunkData(McpeResourcePackChunkData message);
		void HandleMcpeTransfer(McpeTransfer message);
		void HandleMcpePlaySound(McpePlaySound message);
		void HandleMcpeStopSound(McpeStopSound message);
		void HandleMcpeSetTitle(McpeSetTitle message);
		void HandleMcpeAddBehaviorTree(McpeAddBehaviorTree message);
		void HandleMcpeStructureBlockUpdate(McpeStructureBlockUpdate message);
		void HandleMcpeShowStoreOffer(McpeShowStoreOffer message);
		void HandleMcpePlayerSkin(McpePlayerSkin message);
		void HandleMcpeSubClientLogin(McpeSubClientLogin message);
		void HandleMcpeInitiateWebSocketConnection(McpeInitiateWebSocketConnection message);
		void HandleMcpeSetLastHurtBy(McpeSetLastHurtBy message);
		void HandleMcpeBookEdit(McpeBookEdit message);
		void HandleMcpeNpcRequest(McpeNpcRequest message);
		void HandleMcpeModalFormRequest(McpeModalFormRequest message);
		void HandleMcpeServerSettingsResponse(McpeServerSettingsResponse message);
		void HandleMcpeShowProfile(McpeShowProfile message);
		void HandleMcpeSetDefaultGameType(McpeSetDefaultGameType message);
		void HandleMcpeRemoveObjective(McpeRemoveObjective message);
		void HandleMcpeSetDisplayObjective(McpeSetDisplayObjective message);
		void HandleMcpeSetScore(McpeSetScore message);
		void HandleMcpeLabTable(McpeLabTable message);
		void HandleMcpeUpdateBlockSynced(McpeUpdateBlockSynced message);
		void HandleMcpeMoveEntityDelta(McpeMoveEntityDelta message);
		void HandleMcpeSetScoreboardIdentityPacket(McpeSetScoreboardIdentityPacket message);
		void HandleMcpeUpdateSoftEnumPacket(McpeUpdateSoftEnumPacket message);
		void HandleMcpeNetworkStackLatencyPacket(McpeNetworkStackLatencyPacket message);
		void HandleMcpeScriptCustomEventPacket(McpeScriptCustomEventPacket message);
		void HandleMcpeSpawnParticleEffect(McpeSpawnParticleEffect message);
		void HandleMcpeAvailableEntityIdentifiers(McpeAvailableEntityIdentifiers message);
		void HandleMcpeLevelSoundEventV2(McpeLevelSoundEventV2 message);
		void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message);
		void HandleMcpeBiomeDefinitionList(McpeBiomeDefinitionList message);
		void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
		void HandleMcpeLevelEventGeneric(McpeLevelEventGeneric message);
		void HandleMcpeLecternUpdate(McpeLecternUpdate message);
		void HandleMcpeVideoStreamConnect(McpeVideoStreamConnect message);
		void HandleMcpeClientCacheStatus(McpeClientCacheStatus message);
		void HandleMcpeOnScreenTextureAnimation(McpeOnScreenTextureAnimation message);
		void HandleMcpeMapCreateLockedCopy(McpeMapCreateLockedCopy message);
		void HandleMcpeStructureTemplateDataExportRequest(McpeStructureTemplateDataExportRequest message);
		void HandleMcpeStructureTemplateDataExportResponse(McpeStructureTemplateDataExportResponse message);
		void HandleMcpeUpdateBlockProperties(McpeUpdateBlockProperties message);
		void HandleMcpeClientCacheBlobStatus(McpeClientCacheBlobStatus message);
		void HandleMcpeClientCacheMissResponse(McpeClientCacheMissResponse message);
		void HandleFtlCreatePlayer(FtlCreatePlayer message);
	}

	public class McpeClientMessageDispatcher
	{
		private IMcpeClientMessageHandler _messageHandler = null;

		public McpeClientMessageDispatcher(IMcpeClientMessageHandler messageHandler)
		{
			_messageHandler = messageHandler;
		}

		public bool HandlePacket(Packet message)
		{
			if (false) {}
			else if (typeof(McpePlayStatus) == message.GetType()) _messageHandler.HandleMcpePlayStatus((McpePlayStatus) message);
			else if (typeof(McpeServerToClientHandshake) == message.GetType()) _messageHandler.HandleMcpeServerToClientHandshake((McpeServerToClientHandshake) message);
			else if (typeof(McpeDisconnect) == message.GetType()) _messageHandler.HandleMcpeDisconnect((McpeDisconnect) message);
			else if (typeof(McpeResourcePacksInfo) == message.GetType()) _messageHandler.HandleMcpeResourcePacksInfo((McpeResourcePacksInfo) message);
			else if (typeof(McpeResourcePackStack) == message.GetType()) _messageHandler.HandleMcpeResourcePackStack((McpeResourcePackStack) message);
			else if (typeof(McpeText) == message.GetType()) _messageHandler.HandleMcpeText((McpeText) message);
			else if (typeof(McpeSetTime) == message.GetType()) _messageHandler.HandleMcpeSetTime((McpeSetTime) message);
			else if (typeof(McpeStartGame) == message.GetType()) _messageHandler.HandleMcpeStartGame((McpeStartGame) message);
			else if (typeof(McpeAddPlayer) == message.GetType()) _messageHandler.HandleMcpeAddPlayer((McpeAddPlayer) message);
			else if (typeof(McpeAddEntity) == message.GetType()) _messageHandler.HandleMcpeAddEntity((McpeAddEntity) message);
			else if (typeof(McpeRemoveEntity) == message.GetType()) _messageHandler.HandleMcpeRemoveEntity((McpeRemoveEntity) message);
			else if (typeof(McpeAddItemEntity) == message.GetType()) _messageHandler.HandleMcpeAddItemEntity((McpeAddItemEntity) message);
			else if (typeof(McpeTakeItemEntity) == message.GetType()) _messageHandler.HandleMcpeTakeItemEntity((McpeTakeItemEntity) message);
			else if (typeof(McpeMoveEntity) == message.GetType()) _messageHandler.HandleMcpeMoveEntity((McpeMoveEntity) message);
			else if (typeof(McpeMovePlayer) == message.GetType()) _messageHandler.HandleMcpeMovePlayer((McpeMovePlayer) message);
			else if (typeof(McpeRiderJump) == message.GetType()) _messageHandler.HandleMcpeRiderJump((McpeRiderJump) message);
			else if (typeof(McpeUpdateBlock) == message.GetType()) _messageHandler.HandleMcpeUpdateBlock((McpeUpdateBlock) message);
			else if (typeof(McpeAddPainting) == message.GetType()) _messageHandler.HandleMcpeAddPainting((McpeAddPainting) message);
			else if (typeof(McpeTickSync) == message.GetType()) _messageHandler.HandleMcpeTickSync((McpeTickSync) message);
			else if (typeof(McpeLevelSoundEventOld) == message.GetType()) _messageHandler.HandleMcpeLevelSoundEventOld((McpeLevelSoundEventOld) message);
			else if (typeof(McpeLevelEvent) == message.GetType()) _messageHandler.HandleMcpeLevelEvent((McpeLevelEvent) message);
			else if (typeof(McpeBlockEvent) == message.GetType()) _messageHandler.HandleMcpeBlockEvent((McpeBlockEvent) message);
			else if (typeof(McpeEntityEvent) == message.GetType()) _messageHandler.HandleMcpeEntityEvent((McpeEntityEvent) message);
			else if (typeof(McpeMobEffect) == message.GetType()) _messageHandler.HandleMcpeMobEffect((McpeMobEffect) message);
			else if (typeof(McpeUpdateAttributes) == message.GetType()) _messageHandler.HandleMcpeUpdateAttributes((McpeUpdateAttributes) message);
			else if (typeof(McpeInventoryTransaction) == message.GetType()) _messageHandler.HandleMcpeInventoryTransaction((McpeInventoryTransaction) message);
			else if (typeof(McpeMobEquipment) == message.GetType()) _messageHandler.HandleMcpeMobEquipment((McpeMobEquipment) message);
			else if (typeof(McpeMobArmorEquipment) == message.GetType()) _messageHandler.HandleMcpeMobArmorEquipment((McpeMobArmorEquipment) message);
			else if (typeof(McpeInteract) == message.GetType()) _messageHandler.HandleMcpeInteract((McpeInteract) message);
			else if (typeof(McpeHurtArmor) == message.GetType()) _messageHandler.HandleMcpeHurtArmor((McpeHurtArmor) message);
			else if (typeof(McpeSetEntityData) == message.GetType()) _messageHandler.HandleMcpeSetEntityData((McpeSetEntityData) message);
			else if (typeof(McpeSetEntityMotion) == message.GetType()) _messageHandler.HandleMcpeSetEntityMotion((McpeSetEntityMotion) message);
			else if (typeof(McpeSetEntityLink) == message.GetType()) _messageHandler.HandleMcpeSetEntityLink((McpeSetEntityLink) message);
			else if (typeof(McpeSetHealth) == message.GetType()) _messageHandler.HandleMcpeSetHealth((McpeSetHealth) message);
			else if (typeof(McpeSetSpawnPosition) == message.GetType()) _messageHandler.HandleMcpeSetSpawnPosition((McpeSetSpawnPosition) message);
			else if (typeof(McpeAnimate) == message.GetType()) _messageHandler.HandleMcpeAnimate((McpeAnimate) message);
			else if (typeof(McpeRespawn) == message.GetType()) _messageHandler.HandleMcpeRespawn((McpeRespawn) message);
			else if (typeof(McpeContainerOpen) == message.GetType()) _messageHandler.HandleMcpeContainerOpen((McpeContainerOpen) message);
			else if (typeof(McpeContainerClose) == message.GetType()) _messageHandler.HandleMcpeContainerClose((McpeContainerClose) message);
			else if (typeof(McpePlayerHotbar) == message.GetType()) _messageHandler.HandleMcpePlayerHotbar((McpePlayerHotbar) message);
			else if (typeof(McpeInventoryContent) == message.GetType()) _messageHandler.HandleMcpeInventoryContent((McpeInventoryContent) message);
			else if (typeof(McpeInventorySlot) == message.GetType()) _messageHandler.HandleMcpeInventorySlot((McpeInventorySlot) message);
			else if (typeof(McpeContainerSetData) == message.GetType()) _messageHandler.HandleMcpeContainerSetData((McpeContainerSetData) message);
			else if (typeof(McpeCraftingData) == message.GetType()) _messageHandler.HandleMcpeCraftingData((McpeCraftingData) message);
			else if (typeof(McpeCraftingEvent) == message.GetType()) _messageHandler.HandleMcpeCraftingEvent((McpeCraftingEvent) message);
			else if (typeof(McpeGuiDataPickItem) == message.GetType()) _messageHandler.HandleMcpeGuiDataPickItem((McpeGuiDataPickItem) message);
			else if (typeof(McpeAdventureSettings) == message.GetType()) _messageHandler.HandleMcpeAdventureSettings((McpeAdventureSettings) message);
			else if (typeof(McpeBlockEntityData) == message.GetType()) _messageHandler.HandleMcpeBlockEntityData((McpeBlockEntityData) message);
			else if (typeof(McpeLevelChunk) == message.GetType()) _messageHandler.HandleMcpeLevelChunk((McpeLevelChunk) message);
			else if (typeof(McpeSetCommandsEnabled) == message.GetType()) _messageHandler.HandleMcpeSetCommandsEnabled((McpeSetCommandsEnabled) message);
			else if (typeof(McpeSetDifficulty) == message.GetType()) _messageHandler.HandleMcpeSetDifficulty((McpeSetDifficulty) message);
			else if (typeof(McpeChangeDimension) == message.GetType()) _messageHandler.HandleMcpeChangeDimension((McpeChangeDimension) message);
			else if (typeof(McpeSetPlayerGameType) == message.GetType()) _messageHandler.HandleMcpeSetPlayerGameType((McpeSetPlayerGameType) message);
			else if (typeof(McpePlayerList) == message.GetType()) _messageHandler.HandleMcpePlayerList((McpePlayerList) message);
			else if (typeof(McpeSimpleEvent) == message.GetType()) _messageHandler.HandleMcpeSimpleEvent((McpeSimpleEvent) message);
			else if (typeof(McpeTelemetryEvent) == message.GetType()) _messageHandler.HandleMcpeTelemetryEvent((McpeTelemetryEvent) message);
			else if (typeof(McpeSpawnExperienceOrb) == message.GetType()) _messageHandler.HandleMcpeSpawnExperienceOrb((McpeSpawnExperienceOrb) message);
			else if (typeof(McpeClientboundMapItemData) == message.GetType()) _messageHandler.HandleMcpeClientboundMapItemData((McpeClientboundMapItemData) message);
			else if (typeof(McpeMapInfoRequest) == message.GetType()) _messageHandler.HandleMcpeMapInfoRequest((McpeMapInfoRequest) message);
			else if (typeof(McpeRequestChunkRadius) == message.GetType()) _messageHandler.HandleMcpeRequestChunkRadius((McpeRequestChunkRadius) message);
			else if (typeof(McpeChunkRadiusUpdate) == message.GetType()) _messageHandler.HandleMcpeChunkRadiusUpdate((McpeChunkRadiusUpdate) message);
			else if (typeof(McpeItemFrameDropItem) == message.GetType()) _messageHandler.HandleMcpeItemFrameDropItem((McpeItemFrameDropItem) message);
			else if (typeof(McpeGameRulesChanged) == message.GetType()) _messageHandler.HandleMcpeGameRulesChanged((McpeGameRulesChanged) message);
			else if (typeof(McpeCamera) == message.GetType()) _messageHandler.HandleMcpeCamera((McpeCamera) message);
			else if (typeof(McpeBossEvent) == message.GetType()) _messageHandler.HandleMcpeBossEvent((McpeBossEvent) message);
			else if (typeof(McpeShowCredits) == message.GetType()) _messageHandler.HandleMcpeShowCredits((McpeShowCredits) message);
			else if (typeof(McpeAvailableCommands) == message.GetType()) _messageHandler.HandleMcpeAvailableCommands((McpeAvailableCommands) message);
			else if (typeof(McpeCommandOutput) == message.GetType()) _messageHandler.HandleMcpeCommandOutput((McpeCommandOutput) message);
			else if (typeof(McpeUpdateTrade) == message.GetType()) _messageHandler.HandleMcpeUpdateTrade((McpeUpdateTrade) message);
			else if (typeof(McpeUpdateEquipment) == message.GetType()) _messageHandler.HandleMcpeUpdateEquipment((McpeUpdateEquipment) message);
			else if (typeof(McpeResourcePackDataInfo) == message.GetType()) _messageHandler.HandleMcpeResourcePackDataInfo((McpeResourcePackDataInfo) message);
			else if (typeof(McpeResourcePackChunkData) == message.GetType()) _messageHandler.HandleMcpeResourcePackChunkData((McpeResourcePackChunkData) message);
			else if (typeof(McpeTransfer) == message.GetType()) _messageHandler.HandleMcpeTransfer((McpeTransfer) message);
			else if (typeof(McpePlaySound) == message.GetType()) _messageHandler.HandleMcpePlaySound((McpePlaySound) message);
			else if (typeof(McpeStopSound) == message.GetType()) _messageHandler.HandleMcpeStopSound((McpeStopSound) message);
			else if (typeof(McpeSetTitle) == message.GetType()) _messageHandler.HandleMcpeSetTitle((McpeSetTitle) message);
			else if (typeof(McpeAddBehaviorTree) == message.GetType()) _messageHandler.HandleMcpeAddBehaviorTree((McpeAddBehaviorTree) message);
			else if (typeof(McpeStructureBlockUpdate) == message.GetType()) _messageHandler.HandleMcpeStructureBlockUpdate((McpeStructureBlockUpdate) message);
			else if (typeof(McpeShowStoreOffer) == message.GetType()) _messageHandler.HandleMcpeShowStoreOffer((McpeShowStoreOffer) message);
			else if (typeof(McpePlayerSkin) == message.GetType()) _messageHandler.HandleMcpePlayerSkin((McpePlayerSkin) message);
			else if (typeof(McpeSubClientLogin) == message.GetType()) _messageHandler.HandleMcpeSubClientLogin((McpeSubClientLogin) message);
			else if (typeof(McpeInitiateWebSocketConnection) == message.GetType()) _messageHandler.HandleMcpeInitiateWebSocketConnection((McpeInitiateWebSocketConnection) message);
			else if (typeof(McpeSetLastHurtBy) == message.GetType()) _messageHandler.HandleMcpeSetLastHurtBy((McpeSetLastHurtBy) message);
			else if (typeof(McpeBookEdit) == message.GetType()) _messageHandler.HandleMcpeBookEdit((McpeBookEdit) message);
			else if (typeof(McpeNpcRequest) == message.GetType()) _messageHandler.HandleMcpeNpcRequest((McpeNpcRequest) message);
			else if (typeof(McpeModalFormRequest) == message.GetType()) _messageHandler.HandleMcpeModalFormRequest((McpeModalFormRequest) message);
			else if (typeof(McpeServerSettingsResponse) == message.GetType()) _messageHandler.HandleMcpeServerSettingsResponse((McpeServerSettingsResponse) message);
			else if (typeof(McpeShowProfile) == message.GetType()) _messageHandler.HandleMcpeShowProfile((McpeShowProfile) message);
			else if (typeof(McpeSetDefaultGameType) == message.GetType()) _messageHandler.HandleMcpeSetDefaultGameType((McpeSetDefaultGameType) message);
			else if (typeof(McpeRemoveObjective) == message.GetType()) _messageHandler.HandleMcpeRemoveObjective((McpeRemoveObjective) message);
			else if (typeof(McpeSetDisplayObjective) == message.GetType()) _messageHandler.HandleMcpeSetDisplayObjective((McpeSetDisplayObjective) message);
			else if (typeof(McpeSetScore) == message.GetType()) _messageHandler.HandleMcpeSetScore((McpeSetScore) message);
			else if (typeof(McpeLabTable) == message.GetType()) _messageHandler.HandleMcpeLabTable((McpeLabTable) message);
			else if (typeof(McpeUpdateBlockSynced) == message.GetType()) _messageHandler.HandleMcpeUpdateBlockSynced((McpeUpdateBlockSynced) message);
			else if (typeof(McpeMoveEntityDelta) == message.GetType()) _messageHandler.HandleMcpeMoveEntityDelta((McpeMoveEntityDelta) message);
			else if (typeof(McpeSetScoreboardIdentityPacket) == message.GetType()) _messageHandler.HandleMcpeSetScoreboardIdentityPacket((McpeSetScoreboardIdentityPacket) message);
			else if (typeof(McpeUpdateSoftEnumPacket) == message.GetType()) _messageHandler.HandleMcpeUpdateSoftEnumPacket((McpeUpdateSoftEnumPacket) message);
			else if (typeof(McpeNetworkStackLatencyPacket) == message.GetType()) _messageHandler.HandleMcpeNetworkStackLatencyPacket((McpeNetworkStackLatencyPacket) message);
			else if (typeof(McpeScriptCustomEventPacket) == message.GetType()) _messageHandler.HandleMcpeScriptCustomEventPacket((McpeScriptCustomEventPacket) message);
			else if (typeof(McpeSpawnParticleEffect) == message.GetType()) _messageHandler.HandleMcpeSpawnParticleEffect((McpeSpawnParticleEffect) message);
			else if (typeof(McpeAvailableEntityIdentifiers) == message.GetType()) _messageHandler.HandleMcpeAvailableEntityIdentifiers((McpeAvailableEntityIdentifiers) message);
			else if (typeof(McpeLevelSoundEventV2) == message.GetType()) _messageHandler.HandleMcpeLevelSoundEventV2((McpeLevelSoundEventV2) message);
			else if (typeof(McpeNetworkChunkPublisherUpdate) == message.GetType()) _messageHandler.HandleMcpeNetworkChunkPublisherUpdate((McpeNetworkChunkPublisherUpdate) message);
			else if (typeof(McpeBiomeDefinitionList) == message.GetType()) _messageHandler.HandleMcpeBiomeDefinitionList((McpeBiomeDefinitionList) message);
			else if (typeof(McpeLevelSoundEvent) == message.GetType()) _messageHandler.HandleMcpeLevelSoundEvent((McpeLevelSoundEvent) message);
			else if (typeof(McpeLevelEventGeneric) == message.GetType()) _messageHandler.HandleMcpeLevelEventGeneric((McpeLevelEventGeneric) message);
			else if (typeof(McpeLecternUpdate) == message.GetType()) _messageHandler.HandleMcpeLecternUpdate((McpeLecternUpdate) message);
			else if (typeof(McpeVideoStreamConnect) == message.GetType()) _messageHandler.HandleMcpeVideoStreamConnect((McpeVideoStreamConnect) message);
			else if (typeof(McpeClientCacheStatus) == message.GetType()) _messageHandler.HandleMcpeClientCacheStatus((McpeClientCacheStatus) message);
			else if (typeof(McpeOnScreenTextureAnimation) == message.GetType()) _messageHandler.HandleMcpeOnScreenTextureAnimation((McpeOnScreenTextureAnimation) message);
			else if (typeof(McpeMapCreateLockedCopy) == message.GetType()) _messageHandler.HandleMcpeMapCreateLockedCopy((McpeMapCreateLockedCopy) message);
			else if (typeof(McpeStructureTemplateDataExportRequest) == message.GetType()) _messageHandler.HandleMcpeStructureTemplateDataExportRequest((McpeStructureTemplateDataExportRequest) message);
			else if (typeof(McpeStructureTemplateDataExportResponse) == message.GetType()) _messageHandler.HandleMcpeStructureTemplateDataExportResponse((McpeStructureTemplateDataExportResponse) message);
			else if (typeof(McpeUpdateBlockProperties) == message.GetType()) _messageHandler.HandleMcpeUpdateBlockProperties((McpeUpdateBlockProperties) message);
			else if (typeof(McpeClientCacheBlobStatus) == message.GetType()) _messageHandler.HandleMcpeClientCacheBlobStatus((McpeClientCacheBlobStatus) message);
			else if (typeof(McpeClientCacheMissResponse) == message.GetType()) _messageHandler.HandleMcpeClientCacheMissResponse((McpeClientCacheMissResponse) message);
			else if (typeof(FtlCreatePlayer) == message.GetType()) _messageHandler.HandleFtlCreatePlayer((FtlCreatePlayer) message);
			else return false;

			return true;
		}
	}

	public class PacketFactory
	{
		public static Packet Create(byte messageId, byte[] buffer, string ns)
		{
			Packet packet = null; 
			if(ns == "raknet") 
			{
				switch (messageId)
				{
					case 0x00:
						packet = ConnectedPing.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x01:
						packet = UnconnectedPing.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x03:
						packet = ConnectedPong.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x04:
						packet = DetectLostConnections.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x1c:
						packet = UnconnectedPong.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x05:
						packet = OpenConnectionRequest1.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x06:
						packet = OpenConnectionReply1.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x07:
						packet = OpenConnectionRequest2.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x08:
						packet = OpenConnectionReply2.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x09:
						packet = ConnectionRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x10:
						packet = ConnectionRequestAccepted.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x13:
						packet = NewIncomingConnection.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x14:
						packet = NoFreeIncomingConnections.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x15:
						packet = DisconnectionNotification.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x17:
						packet = ConnectionBanned.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x1A:
						packet = IpRecentlyConnected.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0xfe:
						packet = McpeWrapper.CreateObject();
						packet.Decode(buffer);
						return packet;
				}
			} else if(ns == "ftl") 
			{
				switch (messageId)
				{
					case 0x01:
						packet = FtlCreatePlayer.CreateObject();
						packet.Decode(buffer);
						return packet;
				}
			} else {

				switch (messageId)
				{
					case 0x01:
						packet = McpeLogin.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x02:
						packet = McpePlayStatus.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x03:
						packet = McpeServerToClientHandshake.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x04:
						packet = McpeClientToServerHandshake.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x05:
						packet = McpeDisconnect.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x06:
						packet = McpeResourcePacksInfo.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x07:
						packet = McpeResourcePackStack.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x08:
						packet = McpeResourcePackClientResponse.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x09:
						packet = McpeText.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x0a:
						packet = McpeSetTime.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x0b:
						packet = McpeStartGame.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x0c:
						packet = McpeAddPlayer.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x0d:
						packet = McpeAddEntity.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x0e:
						packet = McpeRemoveEntity.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x0f:
						packet = McpeAddItemEntity.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x11:
						packet = McpeTakeItemEntity.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x12:
						packet = McpeMoveEntity.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x13:
						packet = McpeMovePlayer.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x14:
						packet = McpeRiderJump.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x15:
						packet = McpeUpdateBlock.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x16:
						packet = McpeAddPainting.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x17:
						packet = McpeTickSync.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x18:
						packet = McpeLevelSoundEventOld.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x19:
						packet = McpeLevelEvent.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x1a:
						packet = McpeBlockEvent.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x1b:
						packet = McpeEntityEvent.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x1c:
						packet = McpeMobEffect.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x1d:
						packet = McpeUpdateAttributes.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x1e:
						packet = McpeInventoryTransaction.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x1f:
						packet = McpeMobEquipment.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x20:
						packet = McpeMobArmorEquipment.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x21:
						packet = McpeInteract.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x22:
						packet = McpeBlockPickRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x23:
						packet = McpeEntityPickRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x24:
						packet = McpePlayerAction.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x25:
						packet = McpeEntityFall.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x26:
						packet = McpeHurtArmor.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x27:
						packet = McpeSetEntityData.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x28:
						packet = McpeSetEntityMotion.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x29:
						packet = McpeSetEntityLink.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x2a:
						packet = McpeSetHealth.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x2b:
						packet = McpeSetSpawnPosition.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x2c:
						packet = McpeAnimate.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x2d:
						packet = McpeRespawn.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x2e:
						packet = McpeContainerOpen.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x2f:
						packet = McpeContainerClose.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x30:
						packet = McpePlayerHotbar.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x31:
						packet = McpeInventoryContent.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x32:
						packet = McpeInventorySlot.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x33:
						packet = McpeContainerSetData.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x34:
						packet = McpeCraftingData.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x35:
						packet = McpeCraftingEvent.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x36:
						packet = McpeGuiDataPickItem.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x37:
						packet = McpeAdventureSettings.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x38:
						packet = McpeBlockEntityData.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x39:
						packet = McpePlayerInput.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x3a:
						packet = McpeLevelChunk.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x3b:
						packet = McpeSetCommandsEnabled.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x3c:
						packet = McpeSetDifficulty.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x3d:
						packet = McpeChangeDimension.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x3e:
						packet = McpeSetPlayerGameType.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x3f:
						packet = McpePlayerList.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x40:
						packet = McpeSimpleEvent.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x41:
						packet = McpeTelemetryEvent.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x42:
						packet = McpeSpawnExperienceOrb.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x43:
						packet = McpeClientboundMapItemData.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x44:
						packet = McpeMapInfoRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x45:
						packet = McpeRequestChunkRadius.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x46:
						packet = McpeChunkRadiusUpdate.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x47:
						packet = McpeItemFrameDropItem.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x48:
						packet = McpeGameRulesChanged.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x49:
						packet = McpeCamera.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x4a:
						packet = McpeBossEvent.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x4b:
						packet = McpeShowCredits.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x4c:
						packet = McpeAvailableCommands.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x4d:
						packet = McpeCommandRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x4e:
						packet = McpeCommandBlockUpdate.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x4f:
						packet = McpeCommandOutput.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x50:
						packet = McpeUpdateTrade.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x51:
						packet = McpeUpdateEquipment.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x52:
						packet = McpeResourcePackDataInfo.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x53:
						packet = McpeResourcePackChunkData.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x54:
						packet = McpeResourcePackChunkRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x55:
						packet = McpeTransfer.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x56:
						packet = McpePlaySound.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x57:
						packet = McpeStopSound.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x58:
						packet = McpeSetTitle.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x59:
						packet = McpeAddBehaviorTree.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x5a:
						packet = McpeStructureBlockUpdate.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x5b:
						packet = McpeShowStoreOffer.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x5c:
						packet = McpePurchaseReceipt.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x5d:
						packet = McpePlayerSkin.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x5e:
						packet = McpeSubClientLogin.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x5f:
						packet = McpeInitiateWebSocketConnection.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x60:
						packet = McpeSetLastHurtBy.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x61:
						packet = McpeBookEdit.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x62:
						packet = McpeNpcRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x63:
						packet = McpePhotoTransfer.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x64:
						packet = McpeModalFormRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x65:
						packet = McpeModalFormResponse.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x66:
						packet = McpeServerSettingsRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x67:
						packet = McpeServerSettingsResponse.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x68:
						packet = McpeShowProfile.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x69:
						packet = McpeSetDefaultGameType.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x6a:
						packet = McpeRemoveObjective.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x6b:
						packet = McpeSetDisplayObjective.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x6c:
						packet = McpeSetScore.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x6d:
						packet = McpeLabTable.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x6e:
						packet = McpeUpdateBlockSynced.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x6f:
						packet = McpeMoveEntityDelta.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x70:
						packet = McpeSetScoreboardIdentityPacket.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x71:
						packet = McpeSetLocalPlayerAsInitializedPacket.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x72:
						packet = McpeUpdateSoftEnumPacket.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x73:
						packet = McpeNetworkStackLatencyPacket.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x75:
						packet = McpeScriptCustomEventPacket.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x76:
						packet = McpeSpawnParticleEffect.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x77:
						packet = McpeAvailableEntityIdentifiers.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x78:
						packet = McpeLevelSoundEventV2.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x79:
						packet = McpeNetworkChunkPublisherUpdate.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x7a:
						packet = McpeBiomeDefinitionList.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x7b:
						packet = McpeLevelSoundEvent.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x7c:
						packet = McpeLevelEventGeneric.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x7d:
						packet = McpeLecternUpdate.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x7e:
						packet = McpeVideoStreamConnect.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x81:
						packet = McpeClientCacheStatus.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x82:
						packet = McpeOnScreenTextureAnimation.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x83:
						packet = McpeMapCreateLockedCopy.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x84:
						packet = McpeStructureTemplateDataExportRequest.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x85:
						packet = McpeStructureTemplateDataExportResponse.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x86:
						packet = McpeUpdateBlockProperties.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x87:
						packet = McpeClientCacheBlobStatus.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x88:
						packet = McpeClientCacheMissResponse.CreateObject();
						packet.Decode(buffer);
						return packet;
				}
			}

			return null;
		}
	}

	public enum AdventureFlags
	{
		Mayfly = 0x40,
		Noclip = 0x80,
		Worldbuilder = 0x100,
		Flying = 0x200,
		Muted = 0x400,
	}
	public enum CommandPermission
	{
		Normal = 0,
		Operator = 1,
		Host = 2,
		Automation = 3,
		Admin = 4,
	}
	public enum PermissionLevel
	{
		Visitor = 0,
		Member = 1,
		Operator = 2,
		Custom = 3,
	}
	public enum ActionPermissions
	{
		BuildAndMine = 0x1,
		DoorsAndSwitches = 0x2,
		OpenContainers = 0x4,
		AttackPlayers = 0x8,
		AttackMobs = 0x10,
		Operator = 0x20,
		Teleport = 0x80,
		Default = (BuildAndMine | DoorsAndSwitches | OpenContainers | AttackPlayers | AttackMobs ),
		All = (BuildAndMine | DoorsAndSwitches | OpenContainers | AttackPlayers | AttackMobs | Operator | Teleport),
	}

	public partial class ConnectedPing : Packet<ConnectedPing>
	{

		public long sendpingtime; // = null;

		public ConnectedPing()
		{
			Id = 0x00;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(sendpingtime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			sendpingtime = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			sendpingtime=default(long);
		}

	}

	public partial class UnconnectedPing : Packet<UnconnectedPing>
	{

		public long pingId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long guid; // = null;

		public UnconnectedPing()
		{
			Id = 0x01;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(pingId);
			Write(offlineMessageDataId);
			Write(guid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			pingId = ReadLong();
			ReadBytes(offlineMessageDataId.Length);
			guid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			pingId=default(long);
			guid=default(long);
		}

	}

	public partial class ConnectedPong : Packet<ConnectedPong>
	{

		public long sendpingtime; // = null;
		public long sendpongtime; // = null;

		public ConnectedPong()
		{
			Id = 0x03;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(sendpingtime);
			Write(sendpongtime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			sendpingtime = ReadLong();
			sendpongtime = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			sendpingtime=default(long);
			sendpongtime=default(long);
		}

	}

	public partial class DetectLostConnections : Packet<DetectLostConnections>
	{


		public DetectLostConnections()
		{
			Id = 0x04;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class UnconnectedPong : Packet<UnconnectedPong>
	{

		public long pingId; // = null;
		public long serverId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public string serverName; // = null;

		public UnconnectedPong()
		{
			Id = 0x1c;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(pingId);
			Write(serverId);
			Write(offlineMessageDataId);
			WriteFixedString(serverName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			pingId = ReadLong();
			serverId = ReadLong();
			ReadBytes(offlineMessageDataId.Length);
			serverName = ReadFixedString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			pingId=default(long);
			serverId=default(long);
			serverName=default(string);
		}

	}

	public partial class OpenConnectionRequest1 : Packet<OpenConnectionRequest1>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public byte raknetProtocolVersion; // = null;

		public OpenConnectionRequest1()
		{
			Id = 0x05;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(raknetProtocolVersion);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			raknetProtocolVersion = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			raknetProtocolVersion=default(byte);
		}

	}

	public partial class OpenConnectionReply1 : Packet<OpenConnectionReply1>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;
		public byte serverHasSecurity; // = null;
		public short mtuSize; // = null;

		public OpenConnectionReply1()
		{
			Id = 0x06;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(serverGuid);
			Write(serverHasSecurity);
			WriteBe(mtuSize);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();
			serverHasSecurity = ReadByte();
			mtuSize = ReadShortBe();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			serverGuid=default(long);
			serverHasSecurity=default(byte);
			mtuSize=default(short);
		}

	}

	public partial class OpenConnectionRequest2 : Packet<OpenConnectionRequest2>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public IPEndPoint remoteBindingAddress; // = null;
		public short mtuSize; // = null;
		public long clientGuid; // = null;

		public OpenConnectionRequest2()
		{
			Id = 0x07;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(remoteBindingAddress);
			WriteBe(mtuSize);
			Write(clientGuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			remoteBindingAddress = ReadIPEndPoint();
			mtuSize = ReadShortBe();
			clientGuid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			remoteBindingAddress=default(IPEndPoint);
			mtuSize=default(short);
			clientGuid=default(long);
		}

	}

	public partial class OpenConnectionReply2 : Packet<OpenConnectionReply2>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;
		public IPEndPoint clientEndpoint; // = null;
		public short mtuSize; // = null;
		public byte[] doSecurityAndHandshake; // = null;

		public OpenConnectionReply2()
		{
			Id = 0x08;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(serverGuid);
			Write(clientEndpoint);
			WriteBe(mtuSize);
			Write(doSecurityAndHandshake);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();
			clientEndpoint = ReadIPEndPoint();
			mtuSize = ReadShortBe();
			doSecurityAndHandshake = ReadBytes(0, true);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			serverGuid=default(long);
			clientEndpoint=default(IPEndPoint);
			mtuSize=default(short);
			doSecurityAndHandshake=default(byte[]);
		}

	}

	public partial class ConnectionRequest : Packet<ConnectionRequest>
	{

		public long clientGuid; // = null;
		public long timestamp; // = null;
		public byte doSecurity; // = null;

		public ConnectionRequest()
		{
			Id = 0x09;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(clientGuid);
			Write(timestamp);
			Write(doSecurity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			clientGuid = ReadLong();
			timestamp = ReadLong();
			doSecurity = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			clientGuid=default(long);
			timestamp=default(long);
			doSecurity=default(byte);
		}

	}

	public partial class ConnectionRequestAccepted : Packet<ConnectionRequestAccepted>
	{

		public IPEndPoint systemAddress; // = null;
		public short systemIndex; // = null;
		public IPEndPoint[] systemAddresses; // = null;
		public long incomingTimestamp; // = null;
		public long serverTimestamp; // = null;

		public ConnectionRequestAccepted()
		{
			Id = 0x10;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(systemAddress);
			WriteBe(systemIndex);
			Write(systemAddresses);
			Write(incomingTimestamp);
			Write(serverTimestamp);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			systemAddress = ReadIPEndPoint();
			systemIndex = ReadShortBe();
			systemAddresses = ReadIPEndPoints(20);
			incomingTimestamp = ReadLong();
			serverTimestamp = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			systemAddress=default(IPEndPoint);
			systemIndex=default(short);
			systemAddresses=default(IPEndPoint[]);
			incomingTimestamp=default(long);
			serverTimestamp=default(long);
		}

	}

	public partial class NewIncomingConnection : Packet<NewIncomingConnection>
	{

		public IPEndPoint clientendpoint; // = null;
		public IPEndPoint[] systemAddresses; // = null;
		public long incomingTimestamp; // = null;
		public long serverTimestamp; // = null;

		public NewIncomingConnection()
		{
			Id = 0x13;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(clientendpoint);
			Write(systemAddresses);
			Write(incomingTimestamp);
			Write(serverTimestamp);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			clientendpoint = ReadIPEndPoint();
			systemAddresses = ReadIPEndPoints(20);
			incomingTimestamp = ReadLong();
			serverTimestamp = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			clientendpoint=default(IPEndPoint);
			systemAddresses=default(IPEndPoint[]);
			incomingTimestamp=default(long);
			serverTimestamp=default(long);
		}

	}

	public partial class NoFreeIncomingConnections : Packet<NoFreeIncomingConnections>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;

		public NoFreeIncomingConnections()
		{
			Id = 0x14;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(serverGuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			serverGuid=default(long);
		}

	}

	public partial class DisconnectionNotification : Packet<DisconnectionNotification>
	{


		public DisconnectionNotification()
		{
			Id = 0x15;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class ConnectionBanned : Packet<ConnectionBanned>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;

		public ConnectionBanned()
		{
			Id = 0x17;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(serverGuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			serverGuid=default(long);
		}

	}

	public partial class IpRecentlyConnected : Packet<IpRecentlyConnected>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };

		public IpRecentlyConnected()
		{
			Id = 0x1a;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(offlineMessageDataId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeLogin : Packet<McpeLogin>
	{

		public int protocolVersion; // = null;
		public byte[] payload; // = null;

		public McpeLogin()
		{
			Id = 0x01;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteBe(protocolVersion);
			WriteByteArray(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			protocolVersion = ReadIntBe();
			payload = ReadByteArray();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			protocolVersion=default(int);
			payload=default(byte[]);
		}

	}

	public partial class McpePlayStatus : Packet<McpePlayStatus>
	{
		public enum PlayStatus
		{
			LoginSuccess = 0,
			LoginFailedClient = 1,
			LoginFailedServer = 2,
			PlayerSpawn = 3,
			LoginFailedInvalidTenant = 4,
			LoginFailedVanillaEdu = 5,
			LoginFailedEduVanilla = 6,
			LoginFailedServerFull = 7,
		}

		public int status; // = null;

		public McpePlayStatus()
		{
			Id = 0x02;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteBe(status);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			status = ReadIntBe();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			status=default(int);
		}

	}

	public partial class McpeServerToClientHandshake : Packet<McpeServerToClientHandshake>
	{

		public string token; // = null;

		public McpeServerToClientHandshake()
		{
			Id = 0x03;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(token);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			token = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			token=default(string);
		}

	}

	public partial class McpeClientToServerHandshake : Packet<McpeClientToServerHandshake>
	{


		public McpeClientToServerHandshake()
		{
			Id = 0x04;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeDisconnect : Packet<McpeDisconnect>
	{

		public bool hideDisconnectReason; // = null;
		public string message; // = null;

		public McpeDisconnect()
		{
			Id = 0x05;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(hideDisconnectReason);
			Write(message);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			hideDisconnectReason = ReadBool();
			message = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			hideDisconnectReason=default(bool);
			message=default(string);
		}

	}

	public partial class McpeResourcePacksInfo : Packet<McpeResourcePacksInfo>
	{

		public bool mustAccept; // = null;
		public bool hasScripts; // = null;
		public ResourcePackInfos behahaviorpackinfos; // = null;
		public ResourcePackInfos resourcepackinfos; // = null;

		public McpeResourcePacksInfo()
		{
			Id = 0x06;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(mustAccept);
			Write(hasScripts);
			Write(behahaviorpackinfos);
			Write(resourcepackinfos);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			mustAccept = ReadBool();
			hasScripts = ReadBool();
			behahaviorpackinfos = ReadResourcePackInfos();
			resourcepackinfos = ReadResourcePackInfos();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			mustAccept=default(bool);
			hasScripts=default(bool);
			behahaviorpackinfos=default(ResourcePackInfos);
			resourcepackinfos=default(ResourcePackInfos);
		}

	}

	public partial class McpeResourcePackStack : Packet<McpeResourcePackStack>
	{

		public bool mustAccept; // = null;
		public ResourcePackIdVersions behaviorpackidversions; // = null;
		public ResourcePackIdVersions resourcepackidversions; // = null;
		public bool isExperimental; // = null;
		public string gameVersion; // = null;

		public McpeResourcePackStack()
		{
			Id = 0x07;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(mustAccept);
			Write(behaviorpackidversions);
			Write(resourcepackidversions);
			Write(isExperimental);
			Write(gameVersion);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			mustAccept = ReadBool();
			behaviorpackidversions = ReadResourcePackIdVersions();
			resourcepackidversions = ReadResourcePackIdVersions();
			isExperimental = ReadBool();
			gameVersion = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			mustAccept=default(bool);
			behaviorpackidversions=default(ResourcePackIdVersions);
			resourcepackidversions=default(ResourcePackIdVersions);
			isExperimental=default(bool);
			gameVersion=default(string);
		}

	}

	public partial class McpeResourcePackClientResponse : Packet<McpeResourcePackClientResponse>
	{
		public enum ResponseStatus
		{
			Refused = 1,
			SendPacks = 2,
			HaveAllPacks = 3,
			Completed = 4,
		}

		public byte responseStatus; // = null;
		public ResourcePackIds resourcepackids; // = null;

		public McpeResourcePackClientResponse()
		{
			Id = 0x08;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(responseStatus);
			Write(resourcepackids);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			responseStatus = ReadByte();
			resourcepackids = ReadResourcePackIds();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			responseStatus=default(byte);
			resourcepackids=default(ResourcePackIds);
		}

	}

	public partial class McpeText : Packet<McpeText>
	{
		public enum ChatTypes
		{
			Raw = 0,
			Chat = 1,
			Translation = 2,
			Popup = 3,
			Jukeboxpopup = 4,
			Tip = 5,
			System = 6,
			Whisper = 7,
			Announcement = 8,
			Json = 9,
		}

		public byte type; // = null;

		public McpeText()
		{
			Id = 0x09;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(type);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			type = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			type=default(byte);
		}

	}

	public partial class McpeSetTime : Packet<McpeSetTime>
	{

		public int time; // = null;

		public McpeSetTime()
		{
			Id = 0x0a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(time);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			time = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			time=default(int);
		}

	}

	public partial class McpeStartGame : Packet<McpeStartGame>
	{

		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public int playerGamemode; // = null;
		public Vector3 spawn; // = null;
		public Vector2 rotation; // = null;
		public int seed; // = null;
		public int dimension; // = null;
		public int generator; // = null;
		public int gamemode; // = null;
		public int difficulty; // = null;
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public bool hasAchievementsDisabled; // = null;
		public int dayCycleStopTime; // = null;
		public int eduOffer; // = null;
		public bool hasEduFeaturesEnabled; // = null;
		public float rainLevel; // = null;
		public float lightningLevel; // = null;
		public bool hasConfirmedPlatformLockedContent; // = null;
		public bool isMultiplayer; // = null;
		public bool broadcastToLan; // = null;
		public int xboxLiveBroadcastMode; // = null;
		public int platformBroadcastMode; // = null;
		public bool enableCommands; // = null;
		public bool isTexturepacksRequired; // = null;
		public GameRules gamerules; // = null;
		public bool bonusChest; // = null;
		public bool mapEnabled; // = null;
		public int permissionLevel; // = null;
		public int serverChunkTickRange; // = null;
		public bool hasLockedBehaviorPack; // = null;
		public bool hasLockedResourcePack; // = null;
		public bool isFromLockedWorldTemplate; // = null;
		public bool useMsaGamertagsOnly; // = null;
		public bool isFromWorldTemplate; // = null;
		public bool isWorldTemplateOptionLocked; // = null;
		public bool onlySpawnV1Villagers; // = null;
		public string gameVersion; // = null;
		public string levelId; // = null;
		public string worldName; // = null;
		public string premiumWorldTemplateId; // = null;
		public bool isTrial; // = null;
		public bool isServerSideMovementEnabled; // = null;
		public long currentTick; // = null;
		public int enchantmentSeed; // = null;
		public BlockPallet blockPallet; // = null;
		public Itemstates itemstates; // = null;
		public string multiplayerCorrelationId; // = null;

		public McpeStartGame()
		{
			Id = 0x0b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(playerGamemode);
			Write(spawn);
			Write(rotation);
			WriteSignedVarInt(seed);
			WriteSignedVarInt(dimension);
			WriteSignedVarInt(generator);
			WriteSignedVarInt(gamemode);
			WriteSignedVarInt(difficulty);
			WriteSignedVarInt(x);
			WriteVarInt(y);
			WriteSignedVarInt(z);
			Write(hasAchievementsDisabled);
			WriteSignedVarInt(dayCycleStopTime);
			WriteSignedVarInt(eduOffer);
			Write(hasEduFeaturesEnabled);
			Write(rainLevel);
			Write(lightningLevel);
			Write(hasConfirmedPlatformLockedContent);
			Write(isMultiplayer);
			Write(broadcastToLan);
			WriteVarInt(xboxLiveBroadcastMode);
			WriteVarInt(platformBroadcastMode);
			Write(enableCommands);
			Write(isTexturepacksRequired);
			Write(gamerules);
			Write(bonusChest);
			Write(mapEnabled);
			WriteSignedVarInt(permissionLevel);
			Write(serverChunkTickRange);
			Write(hasLockedBehaviorPack);
			Write(hasLockedResourcePack);
			Write(isFromLockedWorldTemplate);
			Write(useMsaGamertagsOnly);
			Write(isFromWorldTemplate);
			Write(isWorldTemplateOptionLocked);
			Write(onlySpawnV1Villagers);
			Write(gameVersion);
			Write(levelId);
			Write(worldName);
			Write(premiumWorldTemplateId);
			Write(isTrial);
			Write(isServerSideMovementEnabled);
			Write(currentTick);
			WriteSignedVarInt(enchantmentSeed);
			Write(blockPallet);
			Write(itemstates);
			Write(multiplayerCorrelationId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			playerGamemode = ReadSignedVarInt();
			spawn = ReadVector3();
			rotation = ReadVector2();
			seed = ReadSignedVarInt();
			dimension = ReadSignedVarInt();
			generator = ReadSignedVarInt();
			gamemode = ReadSignedVarInt();
			difficulty = ReadSignedVarInt();
			x = ReadSignedVarInt();
			y = ReadVarInt();
			z = ReadSignedVarInt();
			hasAchievementsDisabled = ReadBool();
			dayCycleStopTime = ReadSignedVarInt();
			eduOffer = ReadSignedVarInt();
			hasEduFeaturesEnabled = ReadBool();
			rainLevel = ReadFloat();
			lightningLevel = ReadFloat();
			hasConfirmedPlatformLockedContent = ReadBool();
			isMultiplayer = ReadBool();
			broadcastToLan = ReadBool();
			xboxLiveBroadcastMode = ReadVarInt();
			platformBroadcastMode = ReadVarInt();
			enableCommands = ReadBool();
			isTexturepacksRequired = ReadBool();
			gamerules = ReadGameRules();
			bonusChest = ReadBool();
			mapEnabled = ReadBool();
			permissionLevel = ReadSignedVarInt();
			serverChunkTickRange = ReadInt();
			hasLockedBehaviorPack = ReadBool();
			hasLockedResourcePack = ReadBool();
			isFromLockedWorldTemplate = ReadBool();
			useMsaGamertagsOnly = ReadBool();
			isFromWorldTemplate = ReadBool();
			isWorldTemplateOptionLocked = ReadBool();
			onlySpawnV1Villagers = ReadBool();
			gameVersion = ReadString();
			levelId = ReadString();
			worldName = ReadString();
			premiumWorldTemplateId = ReadString();
			isTrial = ReadBool();
			isServerSideMovementEnabled = ReadBool();
			currentTick = ReadLong();
			enchantmentSeed = ReadSignedVarInt();
			blockPallet = ReadBlockPallet();
			itemstates = ReadItemstates();
			multiplayerCorrelationId = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityIdSelf=default(long);
			runtimeEntityId=default(long);
			playerGamemode=default(int);
			spawn=default(Vector3);
			rotation=default(Vector2);
			seed=default(int);
			dimension=default(int);
			generator=default(int);
			gamemode=default(int);
			difficulty=default(int);
			x=default(int);
			y=default(int);
			z=default(int);
			hasAchievementsDisabled=default(bool);
			dayCycleStopTime=default(int);
			eduOffer=default(int);
			hasEduFeaturesEnabled=default(bool);
			rainLevel=default(float);
			lightningLevel=default(float);
			hasConfirmedPlatformLockedContent=default(bool);
			isMultiplayer=default(bool);
			broadcastToLan=default(bool);
			xboxLiveBroadcastMode=default(int);
			platformBroadcastMode=default(int);
			enableCommands=default(bool);
			isTexturepacksRequired=default(bool);
			gamerules=default(GameRules);
			bonusChest=default(bool);
			mapEnabled=default(bool);
			permissionLevel=default(int);
			serverChunkTickRange=default(int);
			hasLockedBehaviorPack=default(bool);
			hasLockedResourcePack=default(bool);
			isFromLockedWorldTemplate=default(bool);
			useMsaGamertagsOnly=default(bool);
			isFromWorldTemplate=default(bool);
			isWorldTemplateOptionLocked=default(bool);
			onlySpawnV1Villagers=default(bool);
			gameVersion=default(string);
			levelId=default(string);
			worldName=default(string);
			premiumWorldTemplateId=default(string);
			isTrial=default(bool);
			isServerSideMovementEnabled=default(bool);
			currentTick=default(long);
			enchantmentSeed=default(int);
			blockPallet=default(BlockPallet);
			itemstates=default(Itemstates);
			multiplayerCorrelationId=default(string);
		}

	}

	public partial class McpeAddPlayer : Packet<McpeAddPlayer>
	{

		public UUID uuid; // = null;
		public string username; // = null;
		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public string platformChatId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float speedX; // = null;
		public float speedY; // = null;
		public float speedZ; // = null;
		public float pitch; // = null;
		public float yaw; // = null;
		public float headYaw; // = null;
		public Item item; // = null;
		public MetadataDictionary metadata; // = null;
		public uint flags; // = null;
		public uint commandPermission; // = null;
		public uint actionPermissions; // = null;
		public uint permissionLevel; // = null;
		public uint customStoredPermissions; // = null;
		public long userId; // = null;
		public Links links; // = null;
		public string deviceId; // = null;
		public int deviceOs; // = null;

		public McpeAddPlayer()
		{
			Id = 0x0c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(uuid);
			Write(username);
			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
			Write(platformChatId);
			Write(x);
			Write(y);
			Write(z);
			Write(speedX);
			Write(speedY);
			Write(speedZ);
			Write(pitch);
			Write(yaw);
			Write(headYaw);
			Write(item);
			Write(metadata);
			WriteUnsignedVarInt(flags);
			WriteUnsignedVarInt(commandPermission);
			WriteUnsignedVarInt(actionPermissions);
			WriteUnsignedVarInt(permissionLevel);
			WriteUnsignedVarInt(customStoredPermissions);
			Write(userId);
			Write(links);
			Write(deviceId);
			Write(deviceOs);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			uuid = ReadUUID();
			username = ReadString();
			entityIdSelf = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			platformChatId = ReadString();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			speedX = ReadFloat();
			speedY = ReadFloat();
			speedZ = ReadFloat();
			pitch = ReadFloat();
			yaw = ReadFloat();
			headYaw = ReadFloat();
			item = ReadItem();
			metadata = ReadMetadataDictionary();
			flags = ReadUnsignedVarInt();
			commandPermission = ReadUnsignedVarInt();
			actionPermissions = ReadUnsignedVarInt();
			permissionLevel = ReadUnsignedVarInt();
			customStoredPermissions = ReadUnsignedVarInt();
			userId = ReadLong();
			links = ReadLinks();
			deviceId = ReadString();
			deviceOs = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			uuid=default(UUID);
			username=default(string);
			entityIdSelf=default(long);
			runtimeEntityId=default(long);
			platformChatId=default(string);
			x=default(float);
			y=default(float);
			z=default(float);
			speedX=default(float);
			speedY=default(float);
			speedZ=default(float);
			pitch=default(float);
			yaw=default(float);
			headYaw=default(float);
			item=default(Item);
			metadata=default(MetadataDictionary);
			flags=default(uint);
			commandPermission=default(uint);
			actionPermissions=default(uint);
			permissionLevel=default(uint);
			customStoredPermissions=default(uint);
			userId=default(long);
			links=default(Links);
			deviceId=default(string);
			deviceOs=default(int);
		}

	}

	public partial class McpeAddEntity : Packet<McpeAddEntity>
	{

		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public string entityType; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float speedX; // = null;
		public float speedY; // = null;
		public float speedZ; // = null;
		public float pitch; // = null;
		public float yaw; // = null;
		public float headYaw; // = null;
		public EntityAttributes attributes; // = null;
		public MetadataDictionary metadata; // = null;
		public Links links; // = null;

		public McpeAddEntity()
		{
			Id = 0x0d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
			Write(entityType);
			Write(x);
			Write(y);
			Write(z);
			Write(speedX);
			Write(speedY);
			Write(speedZ);
			Write(pitch);
			Write(yaw);
			Write(headYaw);
			Write(attributes);
			Write(metadata);
			Write(links);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			entityType = ReadString();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			speedX = ReadFloat();
			speedY = ReadFloat();
			speedZ = ReadFloat();
			pitch = ReadFloat();
			yaw = ReadFloat();
			headYaw = ReadFloat();
			attributes = ReadEntityAttributes();
			metadata = ReadMetadataDictionary();
			links = ReadLinks();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityIdSelf=default(long);
			runtimeEntityId=default(long);
			entityType=default(string);
			x=default(float);
			y=default(float);
			z=default(float);
			speedX=default(float);
			speedY=default(float);
			speedZ=default(float);
			pitch=default(float);
			yaw=default(float);
			headYaw=default(float);
			attributes=default(EntityAttributes);
			metadata=default(MetadataDictionary);
			links=default(Links);
		}

	}

	public partial class McpeRemoveEntity : Packet<McpeRemoveEntity>
	{

		public long entityIdSelf; // = null;

		public McpeRemoveEntity()
		{
			Id = 0x0e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityIdSelf=default(long);
		}

	}

	public partial class McpeAddItemEntity : Packet<McpeAddItemEntity>
	{

		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public Item item; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float speedX; // = null;
		public float speedY; // = null;
		public float speedZ; // = null;
		public MetadataDictionary metadata; // = null;
		public bool isFromFishing; // = null;

		public McpeAddItemEntity()
		{
			Id = 0x0f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
			Write(item);
			Write(x);
			Write(y);
			Write(z);
			Write(speedX);
			Write(speedY);
			Write(speedZ);
			Write(metadata);
			Write(isFromFishing);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			item = ReadItem();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			speedX = ReadFloat();
			speedY = ReadFloat();
			speedZ = ReadFloat();
			metadata = ReadMetadataDictionary();
			isFromFishing = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityIdSelf=default(long);
			runtimeEntityId=default(long);
			item=default(Item);
			x=default(float);
			y=default(float);
			z=default(float);
			speedX=default(float);
			speedY=default(float);
			speedZ=default(float);
			metadata=default(MetadataDictionary);
			isFromFishing=default(bool);
		}

	}

	public partial class McpeTakeItemEntity : Packet<McpeTakeItemEntity>
	{

		public long runtimeEntityId; // = null;
		public long target; // = null;

		public McpeTakeItemEntity()
		{
			Id = 0x11;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteUnsignedVarLong(target);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			target = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			target=default(long);
		}

	}

	public partial class McpeMoveEntity : Packet<McpeMoveEntity>
	{

		public long runtimeEntityId; // = null;
		public byte flags; // = null;
		public PlayerLocation position; // = null;

		public McpeMoveEntity()
		{
			Id = 0x12;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(flags);
			Write(position);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			flags = ReadByte();
			position = ReadPlayerLocation();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			flags=default(byte);
			position=default(PlayerLocation);
		}

	}

	public partial class McpeMovePlayer : Packet<McpeMovePlayer>
	{
		public enum Mode
		{
			Normal = 0,
			Reset = 1,
			Teleport = 2,
			Rotation = 3,
		}
		public enum Teleportcause
		{
			Unknown = 0,
			Projectile = 1,
			ChorusFruit = 2,
			Command = 3,
			Behavior = 4,
			Count = 5,
		}

		public long runtimeEntityId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float pitch; // = null;
		public float yaw; // = null;
		public float headYaw; // = null;
		public byte mode; // = null;
		public bool onGround; // = null;
		public long otherRuntimeEntityId; // = null;

		public McpeMovePlayer()
		{
			Id = 0x13;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(x);
			Write(y);
			Write(z);
			Write(pitch);
			Write(yaw);
			Write(headYaw);
			Write(mode);
			Write(onGround);
			WriteUnsignedVarLong(otherRuntimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			pitch = ReadFloat();
			yaw = ReadFloat();
			headYaw = ReadFloat();
			mode = ReadByte();
			onGround = ReadBool();
			otherRuntimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			x=default(float);
			y=default(float);
			z=default(float);
			pitch=default(float);
			yaw=default(float);
			headYaw=default(float);
			mode=default(byte);
			onGround=default(bool);
			otherRuntimeEntityId=default(long);
		}

	}

	public partial class McpeRiderJump : Packet<McpeRiderJump>
	{

		public int unknown; // = null;

		public McpeRiderJump()
		{
			Id = 0x14;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			unknown = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			unknown=default(int);
		}

	}

	public partial class McpeUpdateBlock : Packet<McpeUpdateBlock>
	{
		public enum Flags
		{
			None = 0,
			Neighbors = 1,
			Network = 2,
			Nographic = 4,
			Priority = 8,
			All = (Neighbors | Network),
			AllPriority = (All | Priority),
		}

		public BlockCoordinates coordinates; // = null;
		public uint blockRuntimeId; // = null;
		public uint blockPriority; // = null;
		public uint storage; // = null;

		public McpeUpdateBlock()
		{
			Id = 0x15;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			WriteUnsignedVarInt(blockRuntimeId);
			WriteUnsignedVarInt(blockPriority);
			WriteUnsignedVarInt(storage);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			blockRuntimeId = ReadUnsignedVarInt();
			blockPriority = ReadUnsignedVarInt();
			storage = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			blockRuntimeId=default(uint);
			blockPriority=default(uint);
			storage=default(uint);
		}

	}

	public partial class McpeAddPainting : Packet<McpeAddPainting>
	{

		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public BlockCoordinates coordinates; // = null;
		public int direction; // = null;
		public string title; // = null;

		public McpeAddPainting()
		{
			Id = 0x16;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
			Write(coordinates);
			WriteSignedVarInt(direction);
			Write(title);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			coordinates = ReadBlockCoordinates();
			direction = ReadSignedVarInt();
			title = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityIdSelf=default(long);
			runtimeEntityId=default(long);
			coordinates=default(BlockCoordinates);
			direction=default(int);
			title=default(string);
		}

	}

	public partial class McpeTickSync : Packet<McpeTickSync>
	{

		public long requestTime; // = null;
		public long responseTime; // = null;

		public McpeTickSync()
		{
			Id = 0x17;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(requestTime);
			Write(responseTime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			requestTime = ReadLong();
			responseTime = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			requestTime=default(long);
			responseTime=default(long);
		}

	}

	public partial class McpeLevelSoundEventOld : Packet<McpeLevelSoundEventOld>
	{

		public byte soundId; // = null;
		public Vector3 position; // = null;
		public int blockId; // = null;
		public int entityType; // = null;
		public bool isBabyMob; // = null;
		public bool isGlobal; // = null;

		public McpeLevelSoundEventOld()
		{
			Id = 0x18;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(soundId);
			Write(position);
			WriteSignedVarInt(blockId);
			WriteSignedVarInt(entityType);
			Write(isBabyMob);
			Write(isGlobal);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			soundId = ReadByte();
			position = ReadVector3();
			blockId = ReadSignedVarInt();
			entityType = ReadSignedVarInt();
			isBabyMob = ReadBool();
			isGlobal = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			soundId=default(byte);
			position=default(Vector3);
			blockId=default(int);
			entityType=default(int);
			isBabyMob=default(bool);
			isGlobal=default(bool);
		}

	}

	public partial class McpeLevelEvent : Packet<McpeLevelEvent>
	{

		public int eventId; // = null;
		public Vector3 position; // = null;
		public int data; // = null;

		public McpeLevelEvent()
		{
			Id = 0x19;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(eventId);
			Write(position);
			WriteSignedVarInt(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			eventId = ReadSignedVarInt();
			position = ReadVector3();
			data = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			eventId=default(int);
			position=default(Vector3);
			data=default(int);
		}

	}

	public partial class McpeBlockEvent : Packet<McpeBlockEvent>
	{

		public BlockCoordinates coordinates; // = null;
		public int case1; // = null;
		public int case2; // = null;

		public McpeBlockEvent()
		{
			Id = 0x1a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			WriteSignedVarInt(case1);
			WriteSignedVarInt(case2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			case1 = ReadSignedVarInt();
			case2 = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			case1=default(int);
			case2=default(int);
		}

	}

	public partial class McpeEntityEvent : Packet<McpeEntityEvent>
	{

		public long runtimeEntityId; // = null;
		public byte eventId; // = null;
		public int data; // = null;

		public McpeEntityEvent()
		{
			Id = 0x1b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(eventId);
			WriteSignedVarInt(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			eventId = ReadByte();
			data = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			eventId=default(byte);
			data=default(int);
		}

	}

	public partial class McpeMobEffect : Packet<McpeMobEffect>
	{

		public long runtimeEntityId; // = null;
		public byte eventId; // = null;
		public int effectId; // = null;
		public int amplifier; // = null;
		public bool particles; // = null;
		public int duration; // = null;

		public McpeMobEffect()
		{
			Id = 0x1c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(eventId);
			WriteSignedVarInt(effectId);
			WriteSignedVarInt(amplifier);
			Write(particles);
			WriteSignedVarInt(duration);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			eventId = ReadByte();
			effectId = ReadSignedVarInt();
			amplifier = ReadSignedVarInt();
			particles = ReadBool();
			duration = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			eventId=default(byte);
			effectId=default(int);
			amplifier=default(int);
			particles=default(bool);
			duration=default(int);
		}

	}

	public partial class McpeUpdateAttributes : Packet<McpeUpdateAttributes>
	{

		public long runtimeEntityId; // = null;
		public PlayerAttributes attributes; // = null;

		public McpeUpdateAttributes()
		{
			Id = 0x1d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(attributes);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			attributes = ReadPlayerAttributes();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			attributes=default(PlayerAttributes);
		}

	}

	public partial class McpeInventoryTransaction : Packet<McpeInventoryTransaction>
	{
		public enum TransactionType
		{
			Normal = 0,
			InventoryMismatch = 1,
			ItemUse = 2,
			ItemUseOnEntity = 3,
			ItemRelease = 4,
		}
		public enum InventorySourceType
		{
			Container = 0,
			Global = 1,
			WorldInteraction = 2,
			Creative = 3,
			Crafting = 100,
			Unspecified = 99999,
		}
		public enum NormalAction
		{
			PutSlot = -2,
			GetSlot = -3,
			GetResult = -4,
			CraftUse = -5,
			EnchantItem = -15,
			EnchantLapis = -16,
			EnchantResult = -17,
			Drop = 199,
		}
		public enum ItemReleaseAction
		{
			Release = 0,
			Use = 1,
		}
		public enum ItemUseAction
		{
			Place = 0,
			Use = 1,
			Destroy = 2,
		}
		public enum ItemUseOnEntityAction
		{
			Interact = 0,
			Attack = 1,
			ItemInteract = 2,
		}

		public Transaction transaction; // = null;

		public McpeInventoryTransaction()
		{
			Id = 0x1e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(transaction);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			transaction = ReadTransaction();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			transaction=default(Transaction);
		}

	}

	public partial class McpeMobEquipment : Packet<McpeMobEquipment>
	{

		public long runtimeEntityId; // = null;
		public Item item; // = null;
		public byte slot; // = null;
		public byte selectedSlot; // = null;
		public byte windowsId; // = null;

		public McpeMobEquipment()
		{
			Id = 0x1f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(item);
			Write(slot);
			Write(selectedSlot);
			Write(windowsId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			item = ReadItem();
			slot = ReadByte();
			selectedSlot = ReadByte();
			windowsId = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			item=default(Item);
			slot=default(byte);
			selectedSlot=default(byte);
			windowsId=default(byte);
		}

	}

	public partial class McpeMobArmorEquipment : Packet<McpeMobArmorEquipment>
	{

		public long runtimeEntityId; // = null;
		public Item helmet; // = null;
		public Item chestplate; // = null;
		public Item leggings; // = null;
		public Item boots; // = null;

		public McpeMobArmorEquipment()
		{
			Id = 0x20;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(helmet);
			Write(chestplate);
			Write(leggings);
			Write(boots);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			helmet = ReadItem();
			chestplate = ReadItem();
			leggings = ReadItem();
			boots = ReadItem();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			helmet=default(Item);
			chestplate=default(Item);
			leggings=default(Item);
			boots=default(Item);
		}

	}

	public partial class McpeInteract : Packet<McpeInteract>
	{
		public enum Actions
		{
			RightClick = 1,
			LeftClick = 2,
			LeaveVehicle = 3,
			MouseOver = 4,
			OpenNpc = 5,
			OpenInventory = 6,
		}

		public byte actionId; // = null;
		public long targetRuntimeEntityId; // = null;

		public McpeInteract()
		{
			Id = 0x21;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(actionId);
			WriteUnsignedVarLong(targetRuntimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			actionId = ReadByte();
			targetRuntimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			actionId=default(byte);
			targetRuntimeEntityId=default(long);
		}

	}

	public partial class McpeBlockPickRequest : Packet<McpeBlockPickRequest>
	{

		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public bool addUserData; // = null;
		public byte selectedSlot; // = null;

		public McpeBlockPickRequest()
		{
			Id = 0x22;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(x);
			WriteSignedVarInt(y);
			WriteSignedVarInt(z);
			Write(addUserData);
			Write(selectedSlot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			x = ReadSignedVarInt();
			y = ReadSignedVarInt();
			z = ReadSignedVarInt();
			addUserData = ReadBool();
			selectedSlot = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			x=default(int);
			y=default(int);
			z=default(int);
			addUserData=default(bool);
			selectedSlot=default(byte);
		}

	}

	public partial class McpeEntityPickRequest : Packet<McpeEntityPickRequest>
	{

		public ulong runtimeEntityId; // = null;
		public byte selectedSlot; // = null;

		public McpeEntityPickRequest()
		{
			Id = 0x23;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(runtimeEntityId);
			Write(selectedSlot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUlong();
			selectedSlot = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(ulong);
			selectedSlot=default(byte);
		}

	}

	public partial class McpePlayerAction : Packet<McpePlayerAction>
	{

		public long runtimeEntityId; // = null;
		public int actionId; // = null;
		public BlockCoordinates coordinates; // = null;
		public int face; // = null;

		public McpePlayerAction()
		{
			Id = 0x24;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(actionId);
			Write(coordinates);
			WriteSignedVarInt(face);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			actionId = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();
			face = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			actionId=default(int);
			coordinates=default(BlockCoordinates);
			face=default(int);
		}

	}

	public partial class McpeEntityFall : Packet<McpeEntityFall>
	{

		public long runtimeEntityId; // = null;
		public float fallDistance; // = null;
		public bool isInVoid; // = null;

		public McpeEntityFall()
		{
			Id = 0x25;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(fallDistance);
			Write(isInVoid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			fallDistance = ReadFloat();
			isInVoid = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			fallDistance=default(float);
			isInVoid=default(bool);
		}

	}

	public partial class McpeHurtArmor : Packet<McpeHurtArmor>
	{

		public int health; // = null;

		public McpeHurtArmor()
		{
			Id = 0x26;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			health = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			health=default(int);
		}

	}

	public partial class McpeSetEntityData : Packet<McpeSetEntityData>
	{

		public long runtimeEntityId; // = null;
		public MetadataDictionary metadata; // = null;

		public McpeSetEntityData()
		{
			Id = 0x27;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(metadata);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			metadata = ReadMetadataDictionary();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			metadata=default(MetadataDictionary);
		}

	}

	public partial class McpeSetEntityMotion : Packet<McpeSetEntityMotion>
	{

		public long runtimeEntityId; // = null;
		public Vector3 velocity; // = null;

		public McpeSetEntityMotion()
		{
			Id = 0x28;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(velocity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			velocity = ReadVector3();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			velocity=default(Vector3);
		}

	}

	public partial class McpeSetEntityLink : Packet<McpeSetEntityLink>
	{
		public enum LinkActions
		{
			Remove = 0,
			Ride = 1,
			Passenger = 2,
		}

		public long riddenId; // = null;
		public long riderId; // = null;
		public byte linkType; // = null;
		public byte unknown; // = null;

		public McpeSetEntityLink()
		{
			Id = 0x29;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(riddenId);
			WriteSignedVarLong(riderId);
			Write(linkType);
			Write(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			riddenId = ReadSignedVarLong();
			riderId = ReadSignedVarLong();
			linkType = ReadByte();
			unknown = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			riddenId=default(long);
			riderId=default(long);
			linkType=default(byte);
			unknown=default(byte);
		}

	}

	public partial class McpeSetHealth : Packet<McpeSetHealth>
	{

		public int health; // = null;

		public McpeSetHealth()
		{
			Id = 0x2a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			health = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			health=default(int);
		}

	}

	public partial class McpeSetSpawnPosition : Packet<McpeSetSpawnPosition>
	{

		public int spawnType; // = null;
		public BlockCoordinates coordinates; // = null;
		public bool forced; // = null;

		public McpeSetSpawnPosition()
		{
			Id = 0x2b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(spawnType);
			Write(coordinates);
			Write(forced);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			spawnType = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();
			forced = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			spawnType=default(int);
			coordinates=default(BlockCoordinates);
			forced=default(bool);
		}

	}

	public partial class McpeAnimate : Packet<McpeAnimate>
	{

		public int actionId; // = null;
		public long runtimeEntityId; // = null;

		public McpeAnimate()
		{
			Id = 0x2c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(actionId);
			WriteUnsignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			actionId = ReadSignedVarInt();
			runtimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			actionId=default(int);
			runtimeEntityId=default(long);
		}

	}

	public partial class McpeRespawn : Packet<McpeRespawn>
	{
		public enum RespawnState
		{
			Search = 0,
			Ready = 1,
			ClientReady = 2,
		}

		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public byte state; // = null;
		public long runtimeEntityId; // = null;

		public McpeRespawn()
		{
			Id = 0x2d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);
			Write(state);
			WriteUnsignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			state = ReadByte();
			runtimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			x=default(float);
			y=default(float);
			z=default(float);
			state=default(byte);
			runtimeEntityId=default(long);
		}

	}

	public partial class McpeContainerOpen : Packet<McpeContainerOpen>
	{

		public byte windowId; // = null;
		public byte type; // = null;
		public BlockCoordinates coordinates; // = null;
		public long runtimeEntityId; // = null;

		public McpeContainerOpen()
		{
			Id = 0x2e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			Write(type);
			Write(coordinates);
			WriteSignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			type = ReadByte();
			coordinates = ReadBlockCoordinates();
			runtimeEntityId = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			type=default(byte);
			coordinates=default(BlockCoordinates);
			runtimeEntityId=default(long);
		}

	}

	public partial class McpeContainerClose : Packet<McpeContainerClose>
	{

		public byte windowId; // = null;

		public McpeContainerClose()
		{
			Id = 0x2f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
		}

	}

	public partial class McpePlayerHotbar : Packet<McpePlayerHotbar>
	{

		public uint selectedSlot; // = null;
		public byte windowId; // = null;
		public bool selectSlot; // = null;

		public McpePlayerHotbar()
		{
			Id = 0x30;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(selectedSlot);
			Write(windowId);
			Write(selectSlot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			selectedSlot = ReadUnsignedVarInt();
			windowId = ReadByte();
			selectSlot = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			selectedSlot=default(uint);
			windowId=default(byte);
			selectSlot=default(bool);
		}

	}

	public partial class McpeInventoryContent : Packet<McpeInventoryContent>
	{

		public uint inventoryId; // = null;
		public ItemStacks input; // = null;

		public McpeInventoryContent()
		{
			Id = 0x31;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(inventoryId);
			Write(input);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			inventoryId = ReadUnsignedVarInt();
			input = ReadItemStacks();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			inventoryId=default(uint);
			input=default(ItemStacks);
		}

	}

	public partial class McpeInventorySlot : Packet<McpeInventorySlot>
	{

		public uint inventoryId; // = null;
		public uint slot; // = null;
		public Item item; // = null;

		public McpeInventorySlot()
		{
			Id = 0x32;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(inventoryId);
			WriteUnsignedVarInt(slot);
			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			inventoryId = ReadUnsignedVarInt();
			slot = ReadUnsignedVarInt();
			item = ReadItem();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			inventoryId=default(uint);
			slot=default(uint);
			item=default(Item);
		}

	}

	public partial class McpeContainerSetData : Packet<McpeContainerSetData>
	{

		public byte windowId; // = null;
		public int property; // = null;
		public int value; // = null;

		public McpeContainerSetData()
		{
			Id = 0x33;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			WriteSignedVarInt(property);
			WriteSignedVarInt(value);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			property = ReadSignedVarInt();
			value = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			property=default(int);
			value=default(int);
		}

	}

	public partial class McpeCraftingData : Packet<McpeCraftingData>
	{

		public Recipes recipes; // = null;
		public uint someArraySize; // = null;
		public uint someArraySize2; // = null;
		public bool isClean; // = null;

		public McpeCraftingData()
		{
			Id = 0x34;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(recipes);
			WriteUnsignedVarInt(someArraySize);
			WriteUnsignedVarInt(someArraySize2);
			Write(isClean);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			recipes = ReadRecipes();
			someArraySize = ReadUnsignedVarInt();
			someArraySize2 = ReadUnsignedVarInt();
			isClean = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			recipes=default(Recipes);
			someArraySize=default(uint);
			someArraySize2=default(uint);
			isClean=default(bool);
		}

	}

	public partial class McpeCraftingEvent : Packet<McpeCraftingEvent>
	{
		public enum RecipeTypes
		{
			Shapeless = 0,
			Shaped = 1,
			Furnace = 2,
			FurnaceData = 3,
			Multi = 4,
			ShulkerBox = 5,
			ChemistryShapeless = 6,
			ChemistryShaped = 7,
		}

		public byte windowId; // = null;
		public int recipeType; // = null;
		public UUID recipeId; // = null;
		public ItemStacks input; // = null;
		public ItemStacks result; // = null;

		public McpeCraftingEvent()
		{
			Id = 0x35;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			WriteSignedVarInt(recipeType);
			Write(recipeId);
			Write(input);
			Write(result);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			recipeType = ReadSignedVarInt();
			recipeId = ReadUUID();
			input = ReadItemStacks();
			result = ReadItemStacks();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			recipeType=default(int);
			recipeId=default(UUID);
			input=default(ItemStacks);
			result=default(ItemStacks);
		}

	}

	public partial class McpeGuiDataPickItem : Packet<McpeGuiDataPickItem>
	{


		public McpeGuiDataPickItem()
		{
			Id = 0x36;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeAdventureSettings : Packet<McpeAdventureSettings>
	{

		public uint flags; // = null;
		public uint commandPermission; // = null;
		public uint actionPermissions; // = null;
		public uint permissionLevel; // = null;
		public uint customStoredPermissions; // = null;
		public long userId; // = null;

		public McpeAdventureSettings()
		{
			Id = 0x37;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(flags);
			WriteUnsignedVarInt(commandPermission);
			WriteUnsignedVarInt(actionPermissions);
			WriteUnsignedVarInt(permissionLevel);
			WriteUnsignedVarInt(customStoredPermissions);
			Write(userId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			flags = ReadUnsignedVarInt();
			commandPermission = ReadUnsignedVarInt();
			actionPermissions = ReadUnsignedVarInt();
			permissionLevel = ReadUnsignedVarInt();
			customStoredPermissions = ReadUnsignedVarInt();
			userId = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			flags=default(uint);
			commandPermission=default(uint);
			actionPermissions=default(uint);
			permissionLevel=default(uint);
			customStoredPermissions=default(uint);
			userId=default(long);
		}

	}

	public partial class McpeBlockEntityData : Packet<McpeBlockEntityData>
	{

		public BlockCoordinates coordinates; // = null;
		public Nbt namedtag; // = null;

		public McpeBlockEntityData()
		{
			Id = 0x38;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			namedtag=default(Nbt);
		}

	}

	public partial class McpePlayerInput : Packet<McpePlayerInput>
	{

		public float motionX; // = null;
		public float motionZ; // = null;
		public bool jumping; // = null;
		public bool sneaking; // = null;

		public McpePlayerInput()
		{
			Id = 0x39;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(motionX);
			Write(motionZ);
			Write(jumping);
			Write(sneaking);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			motionX = ReadFloat();
			motionZ = ReadFloat();
			jumping = ReadBool();
			sneaking = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			motionX=default(float);
			motionZ=default(float);
			jumping=default(bool);
			sneaking=default(bool);
		}

	}

	public partial class McpeLevelChunk : Packet<McpeLevelChunk>
	{

		public int chunkX; // = null;
		public int chunkZ; // = null;
		public uint subChunkCount; // = null;
		public bool cacheEnabled; // = null;
		public byte[] chunkData; // = null;

		public McpeLevelChunk()
		{
			Id = 0x3a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(chunkX);
			WriteSignedVarInt(chunkZ);
			WriteUnsignedVarInt(subChunkCount);
			Write(cacheEnabled);
			WriteByteArray(chunkData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			chunkX = ReadSignedVarInt();
			chunkZ = ReadSignedVarInt();
			subChunkCount = ReadUnsignedVarInt();
			cacheEnabled = ReadBool();
			chunkData = ReadByteArray();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			chunkX=default(int);
			chunkZ=default(int);
			subChunkCount=default(uint);
			cacheEnabled=default(bool);
			chunkData=default(byte[]);
		}

	}

	public partial class McpeSetCommandsEnabled : Packet<McpeSetCommandsEnabled>
	{

		public bool enabled; // = null;

		public McpeSetCommandsEnabled()
		{
			Id = 0x3b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(enabled);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			enabled = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			enabled=default(bool);
		}

	}

	public partial class McpeSetDifficulty : Packet<McpeSetDifficulty>
	{

		public uint difficulty; // = null;

		public McpeSetDifficulty()
		{
			Id = 0x3c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(difficulty);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			difficulty = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			difficulty=default(uint);
		}

	}

	public partial class McpeChangeDimension : Packet<McpeChangeDimension>
	{

		public int dimension; // = null;
		public Vector3 position; // = null;
		public bool respawn; // = null;

		public McpeChangeDimension()
		{
			Id = 0x3d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(dimension);
			Write(position);
			Write(respawn);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			dimension = ReadSignedVarInt();
			position = ReadVector3();
			respawn = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			dimension=default(int);
			position=default(Vector3);
			respawn=default(bool);
		}

	}

	public partial class McpeSetPlayerGameType : Packet<McpeSetPlayerGameType>
	{

		public int gamemode; // = null;

		public McpeSetPlayerGameType()
		{
			Id = 0x3e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(gamemode);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			gamemode = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			gamemode=default(int);
		}

	}

	public partial class McpePlayerList : Packet<McpePlayerList>
	{

		public PlayerRecords records; // = null;

		public McpePlayerList()
		{
			Id = 0x3f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(records);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			records = ReadPlayerRecords();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			records=default(PlayerRecords);
		}

	}

	public partial class McpeSimpleEvent : Packet<McpeSimpleEvent>
	{

		public ushort eventType; // = null;

		public McpeSimpleEvent()
		{
			Id = 0x40;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(eventType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			eventType = ReadUshort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			eventType=default(ushort);
		}

	}

	public partial class McpeTelemetryEvent : Packet<McpeTelemetryEvent>
	{

		public long entityIdSelf; // = null;
		public int unk1; // = null;
		public byte unk2; // = null;

		public McpeTelemetryEvent()
		{
			Id = 0x41;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);
			WriteSignedVarInt(unk1);
			Write(unk2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();
			unk1 = ReadSignedVarInt();
			unk2 = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityIdSelf=default(long);
			unk1=default(int);
			unk2=default(byte);
		}

	}

	public partial class McpeSpawnExperienceOrb : Packet<McpeSpawnExperienceOrb>
	{

		public Vector3 position; // = null;
		public int count; // = null;

		public McpeSpawnExperienceOrb()
		{
			Id = 0x42;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(position);
			WriteSignedVarInt(count);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			position = ReadVector3();
			count = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			position=default(Vector3);
			count=default(int);
		}

	}

	public partial class McpeClientboundMapItemData : Packet<McpeClientboundMapItemData>
	{

		public MapInfo mapinfo; // = null;

		public McpeClientboundMapItemData()
		{
			Id = 0x43;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(mapinfo);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			mapinfo = ReadMapInfo();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			mapinfo=default(MapInfo);
		}

	}

	public partial class McpeMapInfoRequest : Packet<McpeMapInfoRequest>
	{

		public long mapId; // = null;

		public McpeMapInfoRequest()
		{
			Id = 0x44;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(mapId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			mapId = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			mapId=default(long);
		}

	}

	public partial class McpeRequestChunkRadius : Packet<McpeRequestChunkRadius>
	{

		public int chunkRadius; // = null;

		public McpeRequestChunkRadius()
		{
			Id = 0x45;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(chunkRadius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			chunkRadius = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			chunkRadius=default(int);
		}

	}

	public partial class McpeChunkRadiusUpdate : Packet<McpeChunkRadiusUpdate>
	{

		public int chunkRadius; // = null;

		public McpeChunkRadiusUpdate()
		{
			Id = 0x46;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(chunkRadius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			chunkRadius = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			chunkRadius=default(int);
		}

	}

	public partial class McpeItemFrameDropItem : Packet<McpeItemFrameDropItem>
	{

		public BlockCoordinates coordinates; // = null;

		public McpeItemFrameDropItem()
		{
			Id = 0x47;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
		}

	}

	public partial class McpeGameRulesChanged : Packet<McpeGameRulesChanged>
	{

		public GameRules rules; // = null;

		public McpeGameRulesChanged()
		{
			Id = 0x48;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(rules);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			rules = ReadGameRules();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			rules=default(GameRules);
		}

	}

	public partial class McpeCamera : Packet<McpeCamera>
	{

		public long unknown1; // = null;
		public long unknown2; // = null;

		public McpeCamera()
		{
			Id = 0x49;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(unknown1);
			WriteSignedVarLong(unknown2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			unknown1 = ReadSignedVarLong();
			unknown2 = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			unknown1=default(long);
			unknown2=default(long);
		}

	}

	public partial class McpeBossEvent : Packet<McpeBossEvent>
	{

		public long bossEntityId; // = null;
		public uint eventType; // = null;

		public McpeBossEvent()
		{
			Id = 0x4a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(bossEntityId);
			WriteUnsignedVarInt(eventType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			bossEntityId = ReadSignedVarLong();
			eventType = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			bossEntityId=default(long);
			eventType=default(uint);
		}

	}

	public partial class McpeShowCredits : Packet<McpeShowCredits>
	{

		public long runtimeEntityId; // = null;
		public int status; // = null;

		public McpeShowCredits()
		{
			Id = 0x4b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(status);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			status = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			status=default(int);
		}

	}

	public partial class McpeAvailableCommands : Packet<McpeAvailableCommands>
	{


		public McpeAvailableCommands()
		{
			Id = 0x4c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeCommandRequest : Packet<McpeCommandRequest>
	{

		public string command; // = null;
		public uint commandType; // = null;
		public UUID unknownUuid; // = null;
		public string requestId; // = null;
		public bool unknown; // = null;

		public McpeCommandRequest()
		{
			Id = 0x4d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(command);
			WriteUnsignedVarInt(commandType);
			Write(unknownUuid);
			Write(requestId);
			Write(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			command = ReadString();
			commandType = ReadUnsignedVarInt();
			unknownUuid = ReadUUID();
			requestId = ReadString();
			unknown = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			command=default(string);
			commandType=default(uint);
			unknownUuid=default(UUID);
			requestId=default(string);
			unknown=default(bool);
		}

	}

	public partial class McpeCommandBlockUpdate : Packet<McpeCommandBlockUpdate>
	{

		public bool isBlock; // = null;

		public McpeCommandBlockUpdate()
		{
			Id = 0x4e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(isBlock);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			isBlock = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			isBlock=default(bool);
		}

	}

	public partial class McpeCommandOutput : Packet<McpeCommandOutput>
	{


		public McpeCommandOutput()
		{
			Id = 0x4f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeUpdateTrade : Packet<McpeUpdateTrade>
	{

		public byte windowId; // = null;
		public byte windowType; // = null;
		public int unknown0; // = null;
		public int unknown1; // = null;
		public int unknown2; // = null;
		public bool isWilling; // = null;
		public long traderEntityId; // = null;
		public long playerEntityId; // = null;
		public string displayName; // = null;
		public Nbt namedtag; // = null;

		public McpeUpdateTrade()
		{
			Id = 0x50;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			Write(windowType);
			WriteVarInt(unknown0);
			WriteVarInt(unknown1);
			WriteVarInt(unknown2);
			Write(isWilling);
			WriteSignedVarLong(traderEntityId);
			WriteSignedVarLong(playerEntityId);
			Write(displayName);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			windowType = ReadByte();
			unknown0 = ReadVarInt();
			unknown1 = ReadVarInt();
			unknown2 = ReadVarInt();
			isWilling = ReadBool();
			traderEntityId = ReadSignedVarLong();
			playerEntityId = ReadSignedVarLong();
			displayName = ReadString();
			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			windowType=default(byte);
			unknown0=default(int);
			unknown1=default(int);
			unknown2=default(int);
			isWilling=default(bool);
			traderEntityId=default(long);
			playerEntityId=default(long);
			displayName=default(string);
			namedtag=default(Nbt);
		}

	}

	public partial class McpeUpdateEquipment : Packet<McpeUpdateEquipment>
	{

		public byte windowId; // = null;
		public byte windowType; // = null;
		public byte unknown; // = null;
		public long entityId; // = null;
		public Nbt namedtag; // = null;

		public McpeUpdateEquipment()
		{
			Id = 0x51;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			Write(windowType);
			Write(unknown);
			WriteSignedVarLong(entityId);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			windowType = ReadByte();
			unknown = ReadByte();
			entityId = ReadSignedVarLong();
			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			windowType=default(byte);
			unknown=default(byte);
			entityId=default(long);
			namedtag=default(Nbt);
		}

	}

	public partial class McpeResourcePackDataInfo : Packet<McpeResourcePackDataInfo>
	{

		public string packageId; // = null;
		public uint maxChunkSize; // = null;
		public uint chunkCount; // = null;
		public ulong compressedPackageSize; // = null;
		public byte[] hash; // = null;
		public bool isPremium; // = null;
		public byte packType; // = null;

		public McpeResourcePackDataInfo()
		{
			Id = 0x52;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(packageId);
			Write(maxChunkSize);
			Write(chunkCount);
			Write(compressedPackageSize);
			WriteByteArray(hash);
			Write(isPremium);
			Write(packType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			packageId = ReadString();
			maxChunkSize = ReadUint();
			chunkCount = ReadUint();
			compressedPackageSize = ReadUlong();
			hash = ReadByteArray();
			isPremium = ReadBool();
			packType = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			packageId=default(string);
			maxChunkSize=default(uint);
			chunkCount=default(uint);
			compressedPackageSize=default(ulong);
			hash=default(byte[]);
			isPremium=default(bool);
			packType=default(byte);
		}

	}

	public partial class McpeResourcePackChunkData : Packet<McpeResourcePackChunkData>
	{

		public string packageId; // = null;
		public uint chunkIndex; // = null;
		public ulong progress; // = null;
		public byte[] payload; // = null;

		public McpeResourcePackChunkData()
		{
			Id = 0x53;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(packageId);
			Write(chunkIndex);
			Write(progress);
			WriteByteArray(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			packageId = ReadString();
			chunkIndex = ReadUint();
			progress = ReadUlong();
			payload = ReadByteArray();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			packageId=default(string);
			chunkIndex=default(uint);
			progress=default(ulong);
			payload=default(byte[]);
		}

	}

	public partial class McpeResourcePackChunkRequest : Packet<McpeResourcePackChunkRequest>
	{

		public string packageId; // = null;
		public uint chunkIndex; // = null;

		public McpeResourcePackChunkRequest()
		{
			Id = 0x54;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(packageId);
			Write(chunkIndex);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			packageId = ReadString();
			chunkIndex = ReadUint();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			packageId=default(string);
			chunkIndex=default(uint);
		}

	}

	public partial class McpeTransfer : Packet<McpeTransfer>
	{

		public string serverAddress; // = null;
		public ushort port; // = null;

		public McpeTransfer()
		{
			Id = 0x55;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(serverAddress);
			Write(port);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			serverAddress = ReadString();
			port = ReadUshort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			serverAddress=default(string);
			port=default(ushort);
		}

	}

	public partial class McpePlaySound : Packet<McpePlaySound>
	{

		public string name; // = null;
		public BlockCoordinates coordinates; // = null;
		public float volume; // = null;
		public float pitch; // = null;

		public McpePlaySound()
		{
			Id = 0x56;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(name);
			Write(coordinates);
			Write(volume);
			Write(pitch);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			name = ReadString();
			coordinates = ReadBlockCoordinates();
			volume = ReadFloat();
			pitch = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			name=default(string);
			coordinates=default(BlockCoordinates);
			volume=default(float);
			pitch=default(float);
		}

	}

	public partial class McpeStopSound : Packet<McpeStopSound>
	{

		public string name; // = null;
		public bool stopAll; // = null;

		public McpeStopSound()
		{
			Id = 0x57;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(name);
			Write(stopAll);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			name = ReadString();
			stopAll = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			name=default(string);
			stopAll=default(bool);
		}

	}

	public partial class McpeSetTitle : Packet<McpeSetTitle>
	{

		public int type; // = null;
		public string text; // = null;
		public int fadeInTime; // = null;
		public int stayTime; // = null;
		public int fadeOutTime; // = null;

		public McpeSetTitle()
		{
			Id = 0x58;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(type);
			Write(text);
			WriteSignedVarInt(fadeInTime);
			WriteSignedVarInt(stayTime);
			WriteSignedVarInt(fadeOutTime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			type = ReadSignedVarInt();
			text = ReadString();
			fadeInTime = ReadSignedVarInt();
			stayTime = ReadSignedVarInt();
			fadeOutTime = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			type=default(int);
			text=default(string);
			fadeInTime=default(int);
			stayTime=default(int);
			fadeOutTime=default(int);
		}

	}

	public partial class McpeAddBehaviorTree : Packet<McpeAddBehaviorTree>
	{

		public string behaviortree; // = null;

		public McpeAddBehaviorTree()
		{
			Id = 0x59;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(behaviortree);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			behaviortree = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			behaviortree=default(string);
		}

	}

	public partial class McpeStructureBlockUpdate : Packet<McpeStructureBlockUpdate>
	{


		public McpeStructureBlockUpdate()
		{
			Id = 0x5a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeShowStoreOffer : Packet<McpeShowStoreOffer>
	{

		public string unknown0; // = null;
		public bool unknown1; // = null;

		public McpeShowStoreOffer()
		{
			Id = 0x5b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(unknown0);
			Write(unknown1);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			unknown0 = ReadString();
			unknown1 = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			unknown0=default(string);
			unknown1=default(bool);
		}

	}

	public partial class McpePurchaseReceipt : Packet<McpePurchaseReceipt>
	{


		public McpePurchaseReceipt()
		{
			Id = 0x5c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpePlayerSkin : Packet<McpePlayerSkin>
	{

		public UUID uuid; // = null;
		public Skin skin; // = null;
		public string skinName; // = null;
		public string oldSkinName; // = null;

		public McpePlayerSkin()
		{
			Id = 0x5d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(uuid);
			Write(skin);
			Write(skinName);
			Write(oldSkinName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			uuid = ReadUUID();
			skin = ReadSkin();
			skinName = ReadString();
			oldSkinName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			uuid=default(UUID);
			skin=default(Skin);
			skinName=default(string);
			oldSkinName=default(string);
		}

	}

	public partial class McpeSubClientLogin : Packet<McpeSubClientLogin>
	{


		public McpeSubClientLogin()
		{
			Id = 0x5e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeInitiateWebSocketConnection : Packet<McpeInitiateWebSocketConnection>
	{

		public string server; // = null;

		public McpeInitiateWebSocketConnection()
		{
			Id = 0x5f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(server);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			server = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			server=default(string);
		}

	}

	public partial class McpeSetLastHurtBy : Packet<McpeSetLastHurtBy>
	{

		public int unknown; // = null;

		public McpeSetLastHurtBy()
		{
			Id = 0x60;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteVarInt(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			unknown = ReadVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			unknown=default(int);
		}

	}

	public partial class McpeBookEdit : Packet<McpeBookEdit>
	{


		public McpeBookEdit()
		{
			Id = 0x61;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeNpcRequest : Packet<McpeNpcRequest>
	{

		public long runtimeEntityId; // = null;
		public byte unknown0; // = null;
		public string unknown1; // = null;
		public byte unknown2; // = null;

		public McpeNpcRequest()
		{
			Id = 0x62;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(unknown0);
			Write(unknown1);
			Write(unknown2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			unknown0 = ReadByte();
			unknown1 = ReadString();
			unknown2 = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			unknown0=default(byte);
			unknown1=default(string);
			unknown2=default(byte);
		}

	}

	public partial class McpePhotoTransfer : Packet<McpePhotoTransfer>
	{

		public string fileName; // = null;
		public string imageData; // = null;
		public string unknown2; // = null;

		public McpePhotoTransfer()
		{
			Id = 0x63;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(fileName);
			Write(imageData);
			Write(unknown2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			fileName = ReadString();
			imageData = ReadString();
			unknown2 = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			fileName=default(string);
			imageData=default(string);
			unknown2=default(string);
		}

	}

	public partial class McpeModalFormRequest : Packet<McpeModalFormRequest>
	{

		public uint formId; // = null;
		public string data; // = null;

		public McpeModalFormRequest()
		{
			Id = 0x64;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(formId);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			formId = ReadUnsignedVarInt();
			data = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			formId=default(uint);
			data=default(string);
		}

	}

	public partial class McpeModalFormResponse : Packet<McpeModalFormResponse>
	{

		public uint formId; // = null;
		public string data; // = null;

		public McpeModalFormResponse()
		{
			Id = 0x65;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(formId);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			formId = ReadUnsignedVarInt();
			data = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			formId=default(uint);
			data=default(string);
		}

	}

	public partial class McpeServerSettingsRequest : Packet<McpeServerSettingsRequest>
	{


		public McpeServerSettingsRequest()
		{
			Id = 0x66;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeServerSettingsResponse : Packet<McpeServerSettingsResponse>
	{

		public long formId; // = null;
		public string data; // = null;

		public McpeServerSettingsResponse()
		{
			Id = 0x67;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(formId);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			formId = ReadUnsignedVarLong();
			data = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			formId=default(long);
			data=default(string);
		}

	}

	public partial class McpeShowProfile : Packet<McpeShowProfile>
	{

		public string xuid; // = null;

		public McpeShowProfile()
		{
			Id = 0x68;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(xuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			xuid = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			xuid=default(string);
		}

	}

	public partial class McpeSetDefaultGameType : Packet<McpeSetDefaultGameType>
	{

		public int gamemode; // = null;

		public McpeSetDefaultGameType()
		{
			Id = 0x69;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteVarInt(gamemode);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			gamemode = ReadVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			gamemode=default(int);
		}

	}

	public partial class McpeRemoveObjective : Packet<McpeRemoveObjective>
	{

		public string objectiveName; // = null;

		public McpeRemoveObjective()
		{
			Id = 0x6a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(objectiveName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			objectiveName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			objectiveName=default(string);
		}

	}

	public partial class McpeSetDisplayObjective : Packet<McpeSetDisplayObjective>
	{

		public string displaySlot; // = null;
		public string objectiveName; // = null;
		public string displayName; // = null;
		public string criteriaName; // = null;
		public int sortOrder; // = null;

		public McpeSetDisplayObjective()
		{
			Id = 0x6b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(displaySlot);
			Write(objectiveName);
			Write(displayName);
			Write(criteriaName);
			WriteSignedVarInt(sortOrder);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			displaySlot = ReadString();
			objectiveName = ReadString();
			displayName = ReadString();
			criteriaName = ReadString();
			sortOrder = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			displaySlot=default(string);
			objectiveName=default(string);
			displayName=default(string);
			criteriaName=default(string);
			sortOrder=default(int);
		}

	}

	public partial class McpeSetScore : Packet<McpeSetScore>
	{
		public enum Types
		{
			Change = 0,
			Remove = 1,
		}
		public enum ChangeTypes
		{
			Player = 1,
			Entity = 2,
			FakePlayer = 3,
		}

		public ScoreEntries entries; // = null;

		public McpeSetScore()
		{
			Id = 0x6c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(entries);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entries = ReadScoreEntries();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entries=default(ScoreEntries);
		}

	}

	public partial class McpeLabTable : Packet<McpeLabTable>
	{

		public byte uselessByte; // = null;
		public int labTableX; // = null;
		public int labTableY; // = null;
		public int labTableZ; // = null;
		public byte reactionType; // = null;

		public McpeLabTable()
		{
			Id = 0x6d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(uselessByte);
			WriteVarInt(labTableX);
			WriteVarInt(labTableY);
			WriteVarInt(labTableZ);
			Write(reactionType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			uselessByte = ReadByte();
			labTableX = ReadVarInt();
			labTableY = ReadVarInt();
			labTableZ = ReadVarInt();
			reactionType = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			uselessByte=default(byte);
			labTableX=default(int);
			labTableY=default(int);
			labTableZ=default(int);
			reactionType=default(byte);
		}

	}

	public partial class McpeUpdateBlockSynced : Packet<McpeUpdateBlockSynced>
	{

		public BlockCoordinates coordinates; // = null;
		public uint blockRuntimeId; // = null;
		public uint blockPriority; // = null;
		public uint dataLayerId; // = null;
		public long unknown0; // = null;
		public long unknown1; // = null;

		public McpeUpdateBlockSynced()
		{
			Id = 0x6e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			WriteUnsignedVarInt(blockRuntimeId);
			WriteUnsignedVarInt(blockPriority);
			WriteUnsignedVarInt(dataLayerId);
			WriteUnsignedVarLong(unknown0);
			WriteUnsignedVarLong(unknown1);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			blockRuntimeId = ReadUnsignedVarInt();
			blockPriority = ReadUnsignedVarInt();
			dataLayerId = ReadUnsignedVarInt();
			unknown0 = ReadUnsignedVarLong();
			unknown1 = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			blockRuntimeId=default(uint);
			blockPriority=default(uint);
			dataLayerId=default(uint);
			unknown0=default(long);
			unknown1=default(long);
		}

	}

	public partial class McpeMoveEntityDelta : Packet<McpeMoveEntityDelta>
	{

		public long runtimeEntityId; // = null;
		public ushort flags; // = null;

		public McpeMoveEntityDelta()
		{
			Id = 0x6f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(flags);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			flags = ReadUshort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			flags=default(ushort);
		}

	}

	public partial class McpeSetScoreboardIdentityPacket : Packet<McpeSetScoreboardIdentityPacket>
	{
		public enum Operations
		{
			RegisterIdentity = 0,
			ClearIdentity = 1,
		}

		public ScoreboardIdentityEntries entries; // = null;

		public McpeSetScoreboardIdentityPacket()
		{
			Id = 0x70;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(entries);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entries = ReadScoreboardIdentityEntries();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entries=default(ScoreboardIdentityEntries);
		}

	}

	public partial class McpeSetLocalPlayerAsInitializedPacket : Packet<McpeSetLocalPlayerAsInitializedPacket>
	{

		public long runtimeEntityId; // = null;

		public McpeSetLocalPlayerAsInitializedPacket()
		{
			Id = 0x71;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
		}

	}

	public partial class McpeUpdateSoftEnumPacket : Packet<McpeUpdateSoftEnumPacket>
	{


		public McpeUpdateSoftEnumPacket()
		{
			Id = 0x72;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeNetworkStackLatencyPacket : Packet<McpeNetworkStackLatencyPacket>
	{

		public ulong timestamp; // = null;
		public bool unknownFlag; // = null;

		public McpeNetworkStackLatencyPacket()
		{
			Id = 0x73;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(timestamp);
			Write(unknownFlag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			timestamp = ReadUlong();
			unknownFlag = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			timestamp=default(ulong);
			unknownFlag=default(bool);
		}

	}

	public partial class McpeScriptCustomEventPacket : Packet<McpeScriptCustomEventPacket>
	{

		public string eventName; // = null;
		public string eventData; // = null;

		public McpeScriptCustomEventPacket()
		{
			Id = 0x75;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(eventName);
			Write(eventData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			eventName = ReadString();
			eventData = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			eventName=default(string);
			eventData=default(string);
		}

	}

	public partial class McpeSpawnParticleEffect : Packet<McpeSpawnParticleEffect>
	{

		public byte dimensionId; // = null;
		public long entityId; // = null;
		public Vector3 position; // = null;
		public string particleName; // = null;

		public McpeSpawnParticleEffect()
		{
			Id = 0x76;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(dimensionId);
			WriteSignedVarLong(entityId);
			Write(position);
			Write(particleName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			dimensionId = ReadByte();
			entityId = ReadSignedVarLong();
			position = ReadVector3();
			particleName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			dimensionId=default(byte);
			entityId=default(long);
			position=default(Vector3);
			particleName=default(string);
		}

	}

	public partial class McpeAvailableEntityIdentifiers : Packet<McpeAvailableEntityIdentifiers>
	{

		public Nbt namedtag; // = null;

		public McpeAvailableEntityIdentifiers()
		{
			Id = 0x77;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			namedtag=default(Nbt);
		}

	}

	public partial class McpeLevelSoundEventV2 : Packet<McpeLevelSoundEventV2>
	{

		public byte soundId; // = null;
		public Vector3 position; // = null;
		public int blockId; // = null;
		public string entityType; // = null;
		public bool isBabyMob; // = null;
		public bool isGlobal; // = null;

		public McpeLevelSoundEventV2()
		{
			Id = 0x78;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(soundId);
			Write(position);
			WriteSignedVarInt(blockId);
			Write(entityType);
			Write(isBabyMob);
			Write(isGlobal);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			soundId = ReadByte();
			position = ReadVector3();
			blockId = ReadSignedVarInt();
			entityType = ReadString();
			isBabyMob = ReadBool();
			isGlobal = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			soundId=default(byte);
			position=default(Vector3);
			blockId=default(int);
			entityType=default(string);
			isBabyMob=default(bool);
			isGlobal=default(bool);
		}

	}

	public partial class McpeNetworkChunkPublisherUpdate : Packet<McpeNetworkChunkPublisherUpdate>
	{

		public BlockCoordinates coordinates; // = null;
		public uint radius; // = null;

		public McpeNetworkChunkPublisherUpdate()
		{
			Id = 0x79;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			WriteUnsignedVarInt(radius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			radius = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			radius=default(uint);
		}

	}

	public partial class McpeBiomeDefinitionList : Packet<McpeBiomeDefinitionList>
	{

		public Nbt namedtag; // = null;

		public McpeBiomeDefinitionList()
		{
			Id = 0x7a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			namedtag=default(Nbt);
		}

	}

	public partial class McpeLevelSoundEvent : Packet<McpeLevelSoundEvent>
	{

		public uint soundId; // = null;
		public Vector3 position; // = null;
		public int blockId; // = null;
		public string entityType; // = null;
		public bool isBabyMob; // = null;
		public bool isGlobal; // = null;

		public McpeLevelSoundEvent()
		{
			Id = 0x7b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(soundId);
			Write(position);
			WriteSignedVarInt(blockId);
			Write(entityType);
			Write(isBabyMob);
			Write(isGlobal);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			soundId = ReadUnsignedVarInt();
			position = ReadVector3();
			blockId = ReadSignedVarInt();
			entityType = ReadString();
			isBabyMob = ReadBool();
			isGlobal = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			soundId=default(uint);
			position=default(Vector3);
			blockId=default(int);
			entityType=default(string);
			isBabyMob=default(bool);
			isGlobal=default(bool);
		}

	}

	public partial class McpeLevelEventGeneric : Packet<McpeLevelEventGeneric>
	{


		public McpeLevelEventGeneric()
		{
			Id = 0x7c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeLecternUpdate : Packet<McpeLecternUpdate>
	{


		public McpeLecternUpdate()
		{
			Id = 0x7d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeVideoStreamConnect : Packet<McpeVideoStreamConnect>
	{

		public string serverUri; // = null;
		public float frameSendFrequency; // = null;
		public byte action; // = null;
		public int resolutionX; // = null;
		public int resolutionY; // = null;

		public McpeVideoStreamConnect()
		{
			Id = 0x7e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(serverUri);
			Write(frameSendFrequency);
			Write(action);
			Write(resolutionX);
			Write(resolutionY);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			serverUri = ReadString();
			frameSendFrequency = ReadFloat();
			action = ReadByte();
			resolutionX = ReadInt();
			resolutionY = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			serverUri=default(string);
			frameSendFrequency=default(float);
			action=default(byte);
			resolutionX=default(int);
			resolutionY=default(int);
		}

	}

	public partial class McpeClientCacheStatus : Packet<McpeClientCacheStatus>
	{


		public McpeClientCacheStatus()
		{
			Id = 0x81;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeOnScreenTextureAnimation : Packet<McpeOnScreenTextureAnimation>
	{


		public McpeOnScreenTextureAnimation()
		{
			Id = 0x82;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeMapCreateLockedCopy : Packet<McpeMapCreateLockedCopy>
	{


		public McpeMapCreateLockedCopy()
		{
			Id = 0x83;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeStructureTemplateDataExportRequest : Packet<McpeStructureTemplateDataExportRequest>
	{


		public McpeStructureTemplateDataExportRequest()
		{
			Id = 0x84;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeStructureTemplateDataExportResponse : Packet<McpeStructureTemplateDataExportResponse>
	{


		public McpeStructureTemplateDataExportResponse()
		{
			Id = 0x85;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeUpdateBlockProperties : Packet<McpeUpdateBlockProperties>
	{


		public McpeUpdateBlockProperties()
		{
			Id = 0x86;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeClientCacheBlobStatus : Packet<McpeClientCacheBlobStatus>
	{


		public McpeClientCacheBlobStatus()
		{
			Id = 0x87;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeClientCacheMissResponse : Packet<McpeClientCacheMissResponse>
	{


		public McpeClientCacheMissResponse()
		{
			Id = 0x88;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeWrapper : Packet<McpeWrapper>
	{

		public byte[] payload; // = null;

		public McpeWrapper()
		{
			Id = 0xfe;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			payload = ReadBytes(0, true);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			payload=default(byte[]);
		}

	}

	public partial class FtlCreatePlayer : Packet<FtlCreatePlayer>
	{

		public string username; // = null;
		public UUID clientuuid; // = null;
		public string serverAddress; // = null;
		public long clientId; // = null;
		public Skin skin; // = null;

		public FtlCreatePlayer()
		{
			Id = 0x01;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(username);
			Write(clientuuid);
			Write(serverAddress);
			Write(clientId);
			Write(skin);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			username = ReadString();
			clientuuid = ReadUUID();
			serverAddress = ReadString();
			clientId = ReadLong();
			skin = ReadSkin();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			username=default(string);
			clientuuid=default(UUID);
			serverAddress=default(string);
			clientId=default(long);
			skin=default(Skin);
		}

	}

}

