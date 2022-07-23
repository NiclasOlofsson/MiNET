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
using MiNET.Net.RakNet;
using little = MiNET.Utils.Int24; // friendly name
using LongString = System.String;
using MiNET.Utils.Metadata;
using MiNET.Utils.Vectors;
using MiNET.Utils.Nbt;

namespace MiNET.Net
{
	public class McpeProtocolInfo
	{
		public const int ProtocolVersion = 534;
		public const string GameVersion = "1.19.10";
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
		void HandleMcpeTickSync(McpeTickSync message);
		void HandleMcpeLevelSoundEventOld(McpeLevelSoundEventOld message);
		void HandleMcpeEntityEvent(McpeEntityEvent message);
		void HandleMcpeInventoryTransaction(McpeInventoryTransaction message);
		void HandleMcpeMobEquipment(McpeMobEquipment message);
		void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
		void HandleMcpeInteract(McpeInteract message);
		void HandleMcpeBlockPickRequest(McpeBlockPickRequest message);
		void HandleMcpeEntityPickRequest(McpeEntityPickRequest message);
		void HandleMcpePlayerAction(McpePlayerAction message);
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
		void HandleMcpeSetLocalPlayerAsInitialized(McpeSetLocalPlayerAsInitialized message);
		void HandleMcpeNetworkStackLatency(McpeNetworkStackLatency message);
		void HandleMcpeScriptCustomEvent(McpeScriptCustomEvent message);
		void HandleMcpeLevelSoundEventV2(McpeLevelSoundEventV2 message);
		void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
		void HandleMcpeClientCacheStatus(McpeClientCacheStatus message);
		void HandleMcpeNetworkSettings(McpeNetworkSettings message);
		void HandleMcpePlayerAuthInput(McpePlayerAuthInput message);
		void HandleMcpeItemStackRequest(McpeItemStackRequest message);
		void HandleMcpeUpdatePlayerGameType(McpeUpdatePlayerGameType message);
		void HandleMcpePacketViolationWarning(McpePacketViolationWarning message);
		void HandleMcpeFilterTextPacket(McpeFilterTextPacket message);
		void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocksPacket message);
		void HandleMcpeSubChunkRequestPacket(McpeSubChunkRequestPacket message);
		void HandleMcpeRequestAbility(McpeRequestAbility message);
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
		void HandleMcpeSetScoreboardIdentity(McpeSetScoreboardIdentity message);
		void HandleMcpeUpdateSoftEnum(McpeUpdateSoftEnum message);
		void HandleMcpeNetworkStackLatency(McpeNetworkStackLatency message);
		void HandleMcpeScriptCustomEvent(McpeScriptCustomEvent message);
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
		void HandleMcpeNetworkSettings(McpeNetworkSettings message);
		void HandleMcpeCreativeContent(McpeCreativeContent message);
		void HandleMcpePlayerEnchantOptions(McpePlayerEnchantOptions message);
		void HandleMcpeItemStackResponse(McpeItemStackResponse message);
		void HandleMcpeItemComponent(McpeItemComponent message);
		void HandleMcpeFilterTextPacket(McpeFilterTextPacket message);
		void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocksPacket message);
		void HandleMcpeSubChunkPacket(McpeSubChunkPacket message);
		void HandleMcpeDimensionData(McpeDimensionData message);
		void HandleMcpeUpdateAbilities(McpeUpdateAbilities message);
		void HandleMcpeUpdateAdventureSettings(McpeUpdateAdventureSettings message);
		void HandleMcpeAlexEntityAnimation(McpeAlexEntityAnimation message);
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
			switch (message)
			{
				case McpePlayStatus msg:
					_messageHandler.HandleMcpePlayStatus(msg);
					break;
				case McpeServerToClientHandshake msg:
					_messageHandler.HandleMcpeServerToClientHandshake(msg);
					break;
				case McpeDisconnect msg:
					_messageHandler.HandleMcpeDisconnect(msg);
					break;
				case McpeResourcePacksInfo msg:
					_messageHandler.HandleMcpeResourcePacksInfo(msg);
					break;
				case McpeResourcePackStack msg:
					_messageHandler.HandleMcpeResourcePackStack(msg);
					break;
				case McpeText msg:
					_messageHandler.HandleMcpeText(msg);
					break;
				case McpeSetTime msg:
					_messageHandler.HandleMcpeSetTime(msg);
					break;
				case McpeStartGame msg:
					_messageHandler.HandleMcpeStartGame(msg);
					break;
				case McpeAddPlayer msg:
					_messageHandler.HandleMcpeAddPlayer(msg);
					break;
				case McpeAddEntity msg:
					_messageHandler.HandleMcpeAddEntity(msg);
					break;
				case McpeRemoveEntity msg:
					_messageHandler.HandleMcpeRemoveEntity(msg);
					break;
				case McpeAddItemEntity msg:
					_messageHandler.HandleMcpeAddItemEntity(msg);
					break;
				case McpeTakeItemEntity msg:
					_messageHandler.HandleMcpeTakeItemEntity(msg);
					break;
				case McpeMoveEntity msg:
					_messageHandler.HandleMcpeMoveEntity(msg);
					break;
				case McpeMovePlayer msg:
					_messageHandler.HandleMcpeMovePlayer(msg);
					break;
				case McpeRiderJump msg:
					_messageHandler.HandleMcpeRiderJump(msg);
					break;
				case McpeUpdateBlock msg:
					_messageHandler.HandleMcpeUpdateBlock(msg);
					break;
				case McpeAddPainting msg:
					_messageHandler.HandleMcpeAddPainting(msg);
					break;
				case McpeTickSync msg:
					_messageHandler.HandleMcpeTickSync(msg);
					break;
				case McpeLevelSoundEventOld msg:
					_messageHandler.HandleMcpeLevelSoundEventOld(msg);
					break;
				case McpeLevelEvent msg:
					_messageHandler.HandleMcpeLevelEvent(msg);
					break;
				case McpeBlockEvent msg:
					_messageHandler.HandleMcpeBlockEvent(msg);
					break;
				case McpeEntityEvent msg:
					_messageHandler.HandleMcpeEntityEvent(msg);
					break;
				case McpeMobEffect msg:
					_messageHandler.HandleMcpeMobEffect(msg);
					break;
				case McpeUpdateAttributes msg:
					_messageHandler.HandleMcpeUpdateAttributes(msg);
					break;
				case McpeInventoryTransaction msg:
					_messageHandler.HandleMcpeInventoryTransaction(msg);
					break;
				case McpeMobEquipment msg:
					_messageHandler.HandleMcpeMobEquipment(msg);
					break;
				case McpeMobArmorEquipment msg:
					_messageHandler.HandleMcpeMobArmorEquipment(msg);
					break;
				case McpeInteract msg:
					_messageHandler.HandleMcpeInteract(msg);
					break;
				case McpeHurtArmor msg:
					_messageHandler.HandleMcpeHurtArmor(msg);
					break;
				case McpeSetEntityData msg:
					_messageHandler.HandleMcpeSetEntityData(msg);
					break;
				case McpeSetEntityMotion msg:
					_messageHandler.HandleMcpeSetEntityMotion(msg);
					break;
				case McpeSetEntityLink msg:
					_messageHandler.HandleMcpeSetEntityLink(msg);
					break;
				case McpeSetHealth msg:
					_messageHandler.HandleMcpeSetHealth(msg);
					break;
				case McpeSetSpawnPosition msg:
					_messageHandler.HandleMcpeSetSpawnPosition(msg);
					break;
				case McpeAnimate msg:
					_messageHandler.HandleMcpeAnimate(msg);
					break;
				case McpeRespawn msg:
					_messageHandler.HandleMcpeRespawn(msg);
					break;
				case McpeContainerOpen msg:
					_messageHandler.HandleMcpeContainerOpen(msg);
					break;
				case McpeContainerClose msg:
					_messageHandler.HandleMcpeContainerClose(msg);
					break;
				case McpePlayerHotbar msg:
					_messageHandler.HandleMcpePlayerHotbar(msg);
					break;
				case McpeInventoryContent msg:
					_messageHandler.HandleMcpeInventoryContent(msg);
					break;
				case McpeInventorySlot msg:
					_messageHandler.HandleMcpeInventorySlot(msg);
					break;
				case McpeContainerSetData msg:
					_messageHandler.HandleMcpeContainerSetData(msg);
					break;
				case McpeCraftingData msg:
					_messageHandler.HandleMcpeCraftingData(msg);
					break;
				case McpeCraftingEvent msg:
					_messageHandler.HandleMcpeCraftingEvent(msg);
					break;
				case McpeGuiDataPickItem msg:
					_messageHandler.HandleMcpeGuiDataPickItem(msg);
					break;
				case McpeAdventureSettings msg:
					_messageHandler.HandleMcpeAdventureSettings(msg);
					break;
				case McpeBlockEntityData msg:
					_messageHandler.HandleMcpeBlockEntityData(msg);
					break;
				case McpeLevelChunk msg:
					_messageHandler.HandleMcpeLevelChunk(msg);
					break;
				case McpeSetCommandsEnabled msg:
					_messageHandler.HandleMcpeSetCommandsEnabled(msg);
					break;
				case McpeSetDifficulty msg:
					_messageHandler.HandleMcpeSetDifficulty(msg);
					break;
				case McpeChangeDimension msg:
					_messageHandler.HandleMcpeChangeDimension(msg);
					break;
				case McpeSetPlayerGameType msg:
					_messageHandler.HandleMcpeSetPlayerGameType(msg);
					break;
				case McpePlayerList msg:
					_messageHandler.HandleMcpePlayerList(msg);
					break;
				case McpeSimpleEvent msg:
					_messageHandler.HandleMcpeSimpleEvent(msg);
					break;
				case McpeTelemetryEvent msg:
					_messageHandler.HandleMcpeTelemetryEvent(msg);
					break;
				case McpeSpawnExperienceOrb msg:
					_messageHandler.HandleMcpeSpawnExperienceOrb(msg);
					break;
				case McpeClientboundMapItemData msg:
					_messageHandler.HandleMcpeClientboundMapItemData(msg);
					break;
				case McpeMapInfoRequest msg:
					_messageHandler.HandleMcpeMapInfoRequest(msg);
					break;
				case McpeRequestChunkRadius msg:
					_messageHandler.HandleMcpeRequestChunkRadius(msg);
					break;
				case McpeChunkRadiusUpdate msg:
					_messageHandler.HandleMcpeChunkRadiusUpdate(msg);
					break;
				case McpeItemFrameDropItem msg:
					_messageHandler.HandleMcpeItemFrameDropItem(msg);
					break;
				case McpeGameRulesChanged msg:
					_messageHandler.HandleMcpeGameRulesChanged(msg);
					break;
				case McpeCamera msg:
					_messageHandler.HandleMcpeCamera(msg);
					break;
				case McpeBossEvent msg:
					_messageHandler.HandleMcpeBossEvent(msg);
					break;
				case McpeShowCredits msg:
					_messageHandler.HandleMcpeShowCredits(msg);
					break;
				case McpeAvailableCommands msg:
					_messageHandler.HandleMcpeAvailableCommands(msg);
					break;
				case McpeCommandOutput msg:
					_messageHandler.HandleMcpeCommandOutput(msg);
					break;
				case McpeUpdateTrade msg:
					_messageHandler.HandleMcpeUpdateTrade(msg);
					break;
				case McpeUpdateEquipment msg:
					_messageHandler.HandleMcpeUpdateEquipment(msg);
					break;
				case McpeResourcePackDataInfo msg:
					_messageHandler.HandleMcpeResourcePackDataInfo(msg);
					break;
				case McpeResourcePackChunkData msg:
					_messageHandler.HandleMcpeResourcePackChunkData(msg);
					break;
				case McpeTransfer msg:
					_messageHandler.HandleMcpeTransfer(msg);
					break;
				case McpePlaySound msg:
					_messageHandler.HandleMcpePlaySound(msg);
					break;
				case McpeStopSound msg:
					_messageHandler.HandleMcpeStopSound(msg);
					break;
				case McpeSetTitle msg:
					_messageHandler.HandleMcpeSetTitle(msg);
					break;
				case McpeAddBehaviorTree msg:
					_messageHandler.HandleMcpeAddBehaviorTree(msg);
					break;
				case McpeStructureBlockUpdate msg:
					_messageHandler.HandleMcpeStructureBlockUpdate(msg);
					break;
				case McpeShowStoreOffer msg:
					_messageHandler.HandleMcpeShowStoreOffer(msg);
					break;
				case McpePlayerSkin msg:
					_messageHandler.HandleMcpePlayerSkin(msg);
					break;
				case McpeSubClientLogin msg:
					_messageHandler.HandleMcpeSubClientLogin(msg);
					break;
				case McpeInitiateWebSocketConnection msg:
					_messageHandler.HandleMcpeInitiateWebSocketConnection(msg);
					break;
				case McpeSetLastHurtBy msg:
					_messageHandler.HandleMcpeSetLastHurtBy(msg);
					break;
				case McpeBookEdit msg:
					_messageHandler.HandleMcpeBookEdit(msg);
					break;
				case McpeNpcRequest msg:
					_messageHandler.HandleMcpeNpcRequest(msg);
					break;
				case McpeModalFormRequest msg:
					_messageHandler.HandleMcpeModalFormRequest(msg);
					break;
				case McpeServerSettingsResponse msg:
					_messageHandler.HandleMcpeServerSettingsResponse(msg);
					break;
				case McpeShowProfile msg:
					_messageHandler.HandleMcpeShowProfile(msg);
					break;
				case McpeSetDefaultGameType msg:
					_messageHandler.HandleMcpeSetDefaultGameType(msg);
					break;
				case McpeRemoveObjective msg:
					_messageHandler.HandleMcpeRemoveObjective(msg);
					break;
				case McpeSetDisplayObjective msg:
					_messageHandler.HandleMcpeSetDisplayObjective(msg);
					break;
				case McpeSetScore msg:
					_messageHandler.HandleMcpeSetScore(msg);
					break;
				case McpeLabTable msg:
					_messageHandler.HandleMcpeLabTable(msg);
					break;
				case McpeUpdateBlockSynced msg:
					_messageHandler.HandleMcpeUpdateBlockSynced(msg);
					break;
				case McpeMoveEntityDelta msg:
					_messageHandler.HandleMcpeMoveEntityDelta(msg);
					break;
				case McpeSetScoreboardIdentity msg:
					_messageHandler.HandleMcpeSetScoreboardIdentity(msg);
					break;
				case McpeUpdateSoftEnum msg:
					_messageHandler.HandleMcpeUpdateSoftEnum(msg);
					break;
				case McpeNetworkStackLatency msg:
					_messageHandler.HandleMcpeNetworkStackLatency(msg);
					break;
				case McpeScriptCustomEvent msg:
					_messageHandler.HandleMcpeScriptCustomEvent(msg);
					break;
				case McpeSpawnParticleEffect msg:
					_messageHandler.HandleMcpeSpawnParticleEffect(msg);
					break;
				case McpeAvailableEntityIdentifiers msg:
					_messageHandler.HandleMcpeAvailableEntityIdentifiers(msg);
					break;
				case McpeLevelSoundEventV2 msg:
					_messageHandler.HandleMcpeLevelSoundEventV2(msg);
					break;
				case McpeNetworkChunkPublisherUpdate msg:
					_messageHandler.HandleMcpeNetworkChunkPublisherUpdate(msg);
					break;
				case McpeBiomeDefinitionList msg:
					_messageHandler.HandleMcpeBiomeDefinitionList(msg);
					break;
				case McpeLevelSoundEvent msg:
					_messageHandler.HandleMcpeLevelSoundEvent(msg);
					break;
				case McpeLevelEventGeneric msg:
					_messageHandler.HandleMcpeLevelEventGeneric(msg);
					break;
				case McpeLecternUpdate msg:
					_messageHandler.HandleMcpeLecternUpdate(msg);
					break;
				case McpeVideoStreamConnect msg:
					_messageHandler.HandleMcpeVideoStreamConnect(msg);
					break;
				case McpeClientCacheStatus msg:
					_messageHandler.HandleMcpeClientCacheStatus(msg);
					break;
				case McpeOnScreenTextureAnimation msg:
					_messageHandler.HandleMcpeOnScreenTextureAnimation(msg);
					break;
				case McpeMapCreateLockedCopy msg:
					_messageHandler.HandleMcpeMapCreateLockedCopy(msg);
					break;
				case McpeStructureTemplateDataExportRequest msg:
					_messageHandler.HandleMcpeStructureTemplateDataExportRequest(msg);
					break;
				case McpeStructureTemplateDataExportResponse msg:
					_messageHandler.HandleMcpeStructureTemplateDataExportResponse(msg);
					break;
				case McpeUpdateBlockProperties msg:
					_messageHandler.HandleMcpeUpdateBlockProperties(msg);
					break;
				case McpeClientCacheBlobStatus msg:
					_messageHandler.HandleMcpeClientCacheBlobStatus(msg);
					break;
				case McpeClientCacheMissResponse msg:
					_messageHandler.HandleMcpeClientCacheMissResponse(msg);
					break;
				case McpeNetworkSettings msg:
					_messageHandler.HandleMcpeNetworkSettings(msg);
					break;
				case McpeCreativeContent msg:
					_messageHandler.HandleMcpeCreativeContent(msg);
					break;
				case McpePlayerEnchantOptions msg:
					_messageHandler.HandleMcpePlayerEnchantOptions(msg);
					break;
				case McpeItemStackResponse msg:
					_messageHandler.HandleMcpeItemStackResponse(msg);
					break;
				case McpeItemComponent msg:
					_messageHandler.HandleMcpeItemComponent(msg);
					break;
				case McpeFilterTextPacket msg:
					_messageHandler.HandleMcpeFilterTextPacket(msg);
					break;
				case McpeUpdateSubChunkBlocksPacket msg:
					_messageHandler.HandleMcpeUpdateSubChunkBlocksPacket(msg);
					break;
				case McpeSubChunkPacket msg:
					_messageHandler.HandleMcpeSubChunkPacket(msg);
					break;
				case McpeDimensionData msg:
					_messageHandler.HandleMcpeDimensionData(msg);
					break;
				case McpeUpdateAbilities msg:
					_messageHandler.HandleMcpeUpdateAbilities(msg);
					break;
				case McpeUpdateAdventureSettings msg:
					_messageHandler.HandleMcpeUpdateAdventureSettings(msg);
					break;
				case McpeAlexEntityAnimation msg:
					_messageHandler.HandleMcpeAlexEntityAnimation(msg);
					break;
				case FtlCreatePlayer msg:
					_messageHandler.HandleFtlCreatePlayer(msg);
					break;
				default:
					return false;
			}

			return true;
		}
	}

	public class PacketFactory
	{
		public static ICustomPacketFactory CustomPacketFactory { get; set; } = null;

		public static Packet Create(byte messageId, ReadOnlyMemory<byte> buffer, string ns)
		{
			Packet packet = CustomPacketFactory?.Create(messageId, buffer, ns);
			if (packet != null) return packet;

			if(ns == "raknet") 
			{
				switch (messageId)
				{
					case 0x00:
						return ConnectedPing.CreateObject().Decode(buffer);
					case 0x01:
						return UnconnectedPing.CreateObject().Decode(buffer);
					case 0x03:
						return ConnectedPong.CreateObject().Decode(buffer);
					case 0x04:
						return DetectLostConnections.CreateObject().Decode(buffer);
					case 0x1c:
						return UnconnectedPong.CreateObject().Decode(buffer);
					case 0x05:
						return OpenConnectionRequest1.CreateObject().Decode(buffer);
					case 0x06:
						return OpenConnectionReply1.CreateObject().Decode(buffer);
					case 0x07:
						return OpenConnectionRequest2.CreateObject().Decode(buffer);
					case 0x08:
						return OpenConnectionReply2.CreateObject().Decode(buffer);
					case 0x09:
						return ConnectionRequest.CreateObject().Decode(buffer);
					case 0x10:
						return ConnectionRequestAccepted.CreateObject().Decode(buffer);
					case 0x13:
						return NewIncomingConnection.CreateObject().Decode(buffer);
					case 0x14:
						return NoFreeIncomingConnections.CreateObject().Decode(buffer);
					case 0x15:
						return DisconnectionNotification.CreateObject().Decode(buffer);
					case 0x17:
						return ConnectionBanned.CreateObject().Decode(buffer);
					case 0x1A:
						return IpRecentlyConnected.CreateObject().Decode(buffer);
					case 0xfe:
						return McpeWrapper.CreateObject().Decode(buffer);
				}
			} else if(ns == "ftl") 
			{
				switch (messageId)
				{
					case 0x01:
						return FtlCreatePlayer.CreateObject().Decode(buffer);
				}
			} else {

				switch (messageId)
				{
					case 0x01:
						return McpeLogin.CreateObject().Decode(buffer);
					case 0x02:
						return McpePlayStatus.CreateObject().Decode(buffer);
					case 0x03:
						return McpeServerToClientHandshake.CreateObject().Decode(buffer);
					case 0x04:
						return McpeClientToServerHandshake.CreateObject().Decode(buffer);
					case 0x05:
						return McpeDisconnect.CreateObject().Decode(buffer);
					case 0x06:
						return McpeResourcePacksInfo.CreateObject().Decode(buffer);
					case 0x07:
						return McpeResourcePackStack.CreateObject().Decode(buffer);
					case 0x08:
						return McpeResourcePackClientResponse.CreateObject().Decode(buffer);
					case 0x09:
						return McpeText.CreateObject().Decode(buffer);
					case 0x0a:
						return McpeSetTime.CreateObject().Decode(buffer);
					case 0x0b:
						return McpeStartGame.CreateObject().Decode(buffer);
					case 0x0c:
						return McpeAddPlayer.CreateObject().Decode(buffer);
					case 0x0d:
						return McpeAddEntity.CreateObject().Decode(buffer);
					case 0x0e:
						return McpeRemoveEntity.CreateObject().Decode(buffer);
					case 0x0f:
						return McpeAddItemEntity.CreateObject().Decode(buffer);
					case 0x11:
						return McpeTakeItemEntity.CreateObject().Decode(buffer);
					case 0x12:
						return McpeMoveEntity.CreateObject().Decode(buffer);
					case 0x13:
						return McpeMovePlayer.CreateObject().Decode(buffer);
					case 0x14:
						return McpeRiderJump.CreateObject().Decode(buffer);
					case 0x15:
						return McpeUpdateBlock.CreateObject().Decode(buffer);
					case 0x16:
						return McpeAddPainting.CreateObject().Decode(buffer);
					case 0x17:
						return McpeTickSync.CreateObject().Decode(buffer);
					case 0x18:
						return McpeLevelSoundEventOld.CreateObject().Decode(buffer);
					case 0x19:
						return McpeLevelEvent.CreateObject().Decode(buffer);
					case 0x1a:
						return McpeBlockEvent.CreateObject().Decode(buffer);
					case 0x1b:
						return McpeEntityEvent.CreateObject().Decode(buffer);
					case 0x1c:
						return McpeMobEffect.CreateObject().Decode(buffer);
					case 0x1d:
						return McpeUpdateAttributes.CreateObject().Decode(buffer);
					case 0x1e:
						return McpeInventoryTransaction.CreateObject().Decode(buffer);
					case 0x1f:
						return McpeMobEquipment.CreateObject().Decode(buffer);
					case 0x20:
						return McpeMobArmorEquipment.CreateObject().Decode(buffer);
					case 0x21:
						return McpeInteract.CreateObject().Decode(buffer);
					case 0x22:
						return McpeBlockPickRequest.CreateObject().Decode(buffer);
					case 0x23:
						return McpeEntityPickRequest.CreateObject().Decode(buffer);
					case 0x24:
						return McpePlayerAction.CreateObject().Decode(buffer);
					case 0x26:
						return McpeHurtArmor.CreateObject().Decode(buffer);
					case 0x27:
						return McpeSetEntityData.CreateObject().Decode(buffer);
					case 0x28:
						return McpeSetEntityMotion.CreateObject().Decode(buffer);
					case 0x29:
						return McpeSetEntityLink.CreateObject().Decode(buffer);
					case 0x2a:
						return McpeSetHealth.CreateObject().Decode(buffer);
					case 0x2b:
						return McpeSetSpawnPosition.CreateObject().Decode(buffer);
					case 0x2c:
						return McpeAnimate.CreateObject().Decode(buffer);
					case 0x2d:
						return McpeRespawn.CreateObject().Decode(buffer);
					case 0x2e:
						return McpeContainerOpen.CreateObject().Decode(buffer);
					case 0x2f:
						return McpeContainerClose.CreateObject().Decode(buffer);
					case 0x30:
						return McpePlayerHotbar.CreateObject().Decode(buffer);
					case 0x31:
						return McpeInventoryContent.CreateObject().Decode(buffer);
					case 0x32:
						return McpeInventorySlot.CreateObject().Decode(buffer);
					case 0x33:
						return McpeContainerSetData.CreateObject().Decode(buffer);
					case 0x34:
						return McpeCraftingData.CreateObject().Decode(buffer);
					case 0x35:
						return McpeCraftingEvent.CreateObject().Decode(buffer);
					case 0x36:
						return McpeGuiDataPickItem.CreateObject().Decode(buffer);
					case 0x37:
						return McpeAdventureSettings.CreateObject().Decode(buffer);
					case 0x38:
						return McpeBlockEntityData.CreateObject().Decode(buffer);
					case 0x39:
						return McpePlayerInput.CreateObject().Decode(buffer);
					case 0x3a:
						return McpeLevelChunk.CreateObject().Decode(buffer);
					case 0x3b:
						return McpeSetCommandsEnabled.CreateObject().Decode(buffer);
					case 0x3c:
						return McpeSetDifficulty.CreateObject().Decode(buffer);
					case 0x3d:
						return McpeChangeDimension.CreateObject().Decode(buffer);
					case 0x3e:
						return McpeSetPlayerGameType.CreateObject().Decode(buffer);
					case 0x3f:
						return McpePlayerList.CreateObject().Decode(buffer);
					case 0x40:
						return McpeSimpleEvent.CreateObject().Decode(buffer);
					case 0x41:
						return McpeTelemetryEvent.CreateObject().Decode(buffer);
					case 0x42:
						return McpeSpawnExperienceOrb.CreateObject().Decode(buffer);
					case 0x43:
						return McpeClientboundMapItemData.CreateObject().Decode(buffer);
					case 0x44:
						return McpeMapInfoRequest.CreateObject().Decode(buffer);
					case 0x45:
						return McpeRequestChunkRadius.CreateObject().Decode(buffer);
					case 0x46:
						return McpeChunkRadiusUpdate.CreateObject().Decode(buffer);
					case 0x47:
						return McpeItemFrameDropItem.CreateObject().Decode(buffer);
					case 0x48:
						return McpeGameRulesChanged.CreateObject().Decode(buffer);
					case 0x49:
						return McpeCamera.CreateObject().Decode(buffer);
					case 0x4a:
						return McpeBossEvent.CreateObject().Decode(buffer);
					case 0x4b:
						return McpeShowCredits.CreateObject().Decode(buffer);
					case 0x4c:
						return McpeAvailableCommands.CreateObject().Decode(buffer);
					case 0x4d:
						return McpeCommandRequest.CreateObject().Decode(buffer);
					case 0x4e:
						return McpeCommandBlockUpdate.CreateObject().Decode(buffer);
					case 0x4f:
						return McpeCommandOutput.CreateObject().Decode(buffer);
					case 0x50:
						return McpeUpdateTrade.CreateObject().Decode(buffer);
					case 0x51:
						return McpeUpdateEquipment.CreateObject().Decode(buffer);
					case 0x52:
						return McpeResourcePackDataInfo.CreateObject().Decode(buffer);
					case 0x53:
						return McpeResourcePackChunkData.CreateObject().Decode(buffer);
					case 0x54:
						return McpeResourcePackChunkRequest.CreateObject().Decode(buffer);
					case 0x55:
						return McpeTransfer.CreateObject().Decode(buffer);
					case 0x56:
						return McpePlaySound.CreateObject().Decode(buffer);
					case 0x57:
						return McpeStopSound.CreateObject().Decode(buffer);
					case 0x58:
						return McpeSetTitle.CreateObject().Decode(buffer);
					case 0x59:
						return McpeAddBehaviorTree.CreateObject().Decode(buffer);
					case 0x5a:
						return McpeStructureBlockUpdate.CreateObject().Decode(buffer);
					case 0x5b:
						return McpeShowStoreOffer.CreateObject().Decode(buffer);
					case 0x5c:
						return McpePurchaseReceipt.CreateObject().Decode(buffer);
					case 0x5d:
						return McpePlayerSkin.CreateObject().Decode(buffer);
					case 0x5e:
						return McpeSubClientLogin.CreateObject().Decode(buffer);
					case 0x5f:
						return McpeInitiateWebSocketConnection.CreateObject().Decode(buffer);
					case 0x60:
						return McpeSetLastHurtBy.CreateObject().Decode(buffer);
					case 0x61:
						return McpeBookEdit.CreateObject().Decode(buffer);
					case 0x62:
						return McpeNpcRequest.CreateObject().Decode(buffer);
					case 0x63:
						return McpePhotoTransfer.CreateObject().Decode(buffer);
					case 0x64:
						return McpeModalFormRequest.CreateObject().Decode(buffer);
					case 0x65:
						return McpeModalFormResponse.CreateObject().Decode(buffer);
					case 0x66:
						return McpeServerSettingsRequest.CreateObject().Decode(buffer);
					case 0x67:
						return McpeServerSettingsResponse.CreateObject().Decode(buffer);
					case 0x68:
						return McpeShowProfile.CreateObject().Decode(buffer);
					case 0x69:
						return McpeSetDefaultGameType.CreateObject().Decode(buffer);
					case 0x6a:
						return McpeRemoveObjective.CreateObject().Decode(buffer);
					case 0x6b:
						return McpeSetDisplayObjective.CreateObject().Decode(buffer);
					case 0x6c:
						return McpeSetScore.CreateObject().Decode(buffer);
					case 0x6d:
						return McpeLabTable.CreateObject().Decode(buffer);
					case 0x6e:
						return McpeUpdateBlockSynced.CreateObject().Decode(buffer);
					case 0x6f:
						return McpeMoveEntityDelta.CreateObject().Decode(buffer);
					case 0x70:
						return McpeSetScoreboardIdentity.CreateObject().Decode(buffer);
					case 0x71:
						return McpeSetLocalPlayerAsInitialized.CreateObject().Decode(buffer);
					case 0x72:
						return McpeUpdateSoftEnum.CreateObject().Decode(buffer);
					case 0x73:
						return McpeNetworkStackLatency.CreateObject().Decode(buffer);
					case 0x75:
						return McpeScriptCustomEvent.CreateObject().Decode(buffer);
					case 0x76:
						return McpeSpawnParticleEffect.CreateObject().Decode(buffer);
					case 0x77:
						return McpeAvailableEntityIdentifiers.CreateObject().Decode(buffer);
					case 0x78:
						return McpeLevelSoundEventV2.CreateObject().Decode(buffer);
					case 0x79:
						return McpeNetworkChunkPublisherUpdate.CreateObject().Decode(buffer);
					case 0x7a:
						return McpeBiomeDefinitionList.CreateObject().Decode(buffer);
					case 0x7b:
						return McpeLevelSoundEvent.CreateObject().Decode(buffer);
					case 0x7c:
						return McpeLevelEventGeneric.CreateObject().Decode(buffer);
					case 0x7d:
						return McpeLecternUpdate.CreateObject().Decode(buffer);
					case 0x7e:
						return McpeVideoStreamConnect.CreateObject().Decode(buffer);
					case 0x81:
						return McpeClientCacheStatus.CreateObject().Decode(buffer);
					case 0x82:
						return McpeOnScreenTextureAnimation.CreateObject().Decode(buffer);
					case 0x83:
						return McpeMapCreateLockedCopy.CreateObject().Decode(buffer);
					case 0x84:
						return McpeStructureTemplateDataExportRequest.CreateObject().Decode(buffer);
					case 0x85:
						return McpeStructureTemplateDataExportResponse.CreateObject().Decode(buffer);
					case 0x86:
						return McpeUpdateBlockProperties.CreateObject().Decode(buffer);
					case 0x87:
						return McpeClientCacheBlobStatus.CreateObject().Decode(buffer);
					case 0x88:
						return McpeClientCacheMissResponse.CreateObject().Decode(buffer);
					case 0x8f:
						return McpeNetworkSettings.CreateObject().Decode(buffer);
					case 0x90:
						return McpePlayerAuthInput.CreateObject().Decode(buffer);
					case 0x91:
						return McpeCreativeContent.CreateObject().Decode(buffer);
					case 0x92:
						return McpePlayerEnchantOptions.CreateObject().Decode(buffer);
					case 0x93:
						return McpeItemStackRequest.CreateObject().Decode(buffer);
					case 0x94:
						return McpeItemStackResponse.CreateObject().Decode(buffer);
					case 0x97:
						return McpeUpdatePlayerGameType.CreateObject().Decode(buffer);
					case 0x9c:
						return McpePacketViolationWarning.CreateObject().Decode(buffer);
					case 0xa2:
						return McpeItemComponent.CreateObject().Decode(buffer);
					case 0xa3:
						return McpeFilterTextPacket.CreateObject().Decode(buffer);
					case 0xac:
						return McpeUpdateSubChunkBlocksPacket.CreateObject().Decode(buffer);
					case 0xae:
						return McpeSubChunkPacket.CreateObject().Decode(buffer);
					case 0xaf:
						return McpeSubChunkRequestPacket.CreateObject().Decode(buffer);
					case 0xb4:
						return McpeDimensionData.CreateObject().Decode(buffer);
					case 0xbb:
						return McpeUpdateAbilities.CreateObject().Decode(buffer);
					case 0xbc:
						return McpeUpdateAdventureSettings.CreateObject().Decode(buffer);
					case 0xb8:
						return McpeRequestAbility.CreateObject().Decode(buffer);
					case 0xe0:
						return McpeAlexEntityAnimation.CreateObject().Decode(buffer);
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
		public bool forceServerPacks; // = null;
		public ResourcePackInfos behahaviorpackinfos; // = null;
		public TexturePackInfos texturepacks; // = null;

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
			Write(forceServerPacks);
			Write(behahaviorpackinfos);
			Write(texturepacks);

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
			forceServerPacks = ReadBool();
			behahaviorpackinfos = ReadResourcePackInfos();
			texturepacks = ReadTexturePackInfos();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			mustAccept=default(bool);
			hasScripts=default(bool);
			forceServerPacks=default(bool);
			behahaviorpackinfos=default(ResourcePackInfos);
			texturepacks=default(TexturePackInfos);
		}

	}

	public partial class McpeResourcePackStack : Packet<McpeResourcePackStack>
	{

		public bool mustAccept; // = null;
		public ResourcePackIdVersions behaviorpackidversions; // = null;
		public ResourcePackIdVersions resourcepackidversions; // = null;
		public string gameVersion; // = null;
		public Experiments experiments; // = null;
		public bool experimentsPreviouslyToggled; // = null;

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
			Write(gameVersion);
			Write(experiments);
			Write(experimentsPreviouslyToggled);

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
			gameVersion = ReadString();
			experiments = ReadExperiments();
			experimentsPreviouslyToggled = ReadBool();

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
			gameVersion=default(string);
			experiments=default(Experiments);
			experimentsPreviouslyToggled=default(bool);
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
			Jsonwhisper = 10,
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


		public McpeStartGame()
		{
			Id = 0x0b;
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

	public partial class McpeAddPlayer : Packet<McpeAddPlayer>
	{

		public UUID uuid; // = null;
		public string username; // = null;
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
		public uint gameType; // = null;
		public MetadataDictionary metadata; // = null;
		public long entityIdSelf; // = null;
		public byte playerPermissions; // = null;
		public byte commandPermissions; // = null;
		public AbilityLayers layers; // = null;
		public EntityLinks links; // = null;
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
			WriteUnsignedVarInt(gameType);
			Write(metadata);
			WriteSignedVarLong(entityIdSelf);
			Write(playerPermissions);
			Write(commandPermissions);
			Write(layers);
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
			gameType = ReadUnsignedVarInt();
			metadata = ReadMetadataDictionary();
			entityIdSelf = ReadSignedVarLong();
			playerPermissions = ReadByte();
			commandPermissions = ReadByte();
			layers = ReadAbilityLayers();
			links = ReadEntityLinks();
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
			gameType=default(uint);
			metadata=default(MetadataDictionary);
			entityIdSelf=default(long);
			playerPermissions=default(byte);
			commandPermissions=default(byte);
			layers=default(AbilityLayers);
			links=default(EntityLinks);
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
		public float bodyYaw; // = null;
		public EntityAttributes attributes; // = null;
		public MetadataDictionary metadata; // = null;
		public EntityLinks links; // = null;

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
			Write(bodyYaw);
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
			bodyYaw = ReadFloat();
			attributes = ReadEntityAttributes();
			metadata = ReadMetadataDictionary();
			links = ReadEntityLinks();

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
			bodyYaw=default(float);
			attributes=default(EntityAttributes);
			metadata=default(MetadataDictionary);
			links=default(EntityLinks);
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
		public long tick; // = null;

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
			WriteUnsignedVarLong(tick);

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
			tick = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			attributes=default(PlayerAttributes);
			tick=default(long);
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
		public enum CraftingAction
		{
			CraftAddIngredient = -2,
			CraftRemoveIngredient = -3,
			CraftResult = -4,
			CraftUseIngredient = -5,
			AnvilInput = -10,
			AnvilMaterial = -11,
			AnvilResult = -12,
			AnvilOutput = -13,
			EnchantItem = -15,
			EnchantLapis = -16,
			EnchantResult = -17,
			Drop = -100,
		}
		public enum ItemReleaseAction
		{
			Release = 0,
			Use = 1,
		}
		public enum ItemUseAction
		{
			Place,Clickblock = 0,
			Use,Clickair = 1,
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
		public bool addUserData; // = null;

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
			Write(addUserData);

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
			addUserData = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(ulong);
			selectedSlot=default(byte);
			addUserData=default(bool);
		}

	}

	public partial class McpePlayerAction : Packet<McpePlayerAction>
	{

		public long runtimeEntityId; // = null;
		public int actionId; // = null;
		public BlockCoordinates coordinates; // = null;
		public BlockCoordinates resultCoordinates; // = null;
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
			Write(resultCoordinates);
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
			resultCoordinates = ReadBlockCoordinates();
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
			resultCoordinates=default(BlockCoordinates);
			face=default(int);
		}

	}

	public partial class McpeHurtArmor : Packet<McpeHurtArmor>
	{

		public int cause; // = null;
		public int health; // = null;
		public long armorSlotFlags; // = null;

		public McpeHurtArmor()
		{
			Id = 0x26;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteVarInt(cause);
			WriteSignedVarInt(health);
			WriteUnsignedVarLong(armorSlotFlags);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			cause = ReadVarInt();
			health = ReadSignedVarInt();
			armorSlotFlags = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			cause=default(int);
			health=default(int);
			armorSlotFlags=default(long);
		}

	}

	public partial class McpeSetEntityData : Packet<McpeSetEntityData>
	{

		public long runtimeEntityId; // = null;
		public MetadataDictionary metadata; // = null;
		public long tick; // = null;

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
			WriteUnsignedVarLong(tick);

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
			tick = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			metadata=default(MetadataDictionary);
			tick=default(long);
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
		public int dimension; // = null;
		public BlockCoordinates unknownCoordinates; // = null;

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
			WriteSignedVarInt(dimension);
			Write(unknownCoordinates);

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
			dimension = ReadSignedVarInt();
			unknownCoordinates = ReadBlockCoordinates();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			spawnType=default(int);
			coordinates=default(BlockCoordinates);
			dimension=default(int);
			unknownCoordinates=default(BlockCoordinates);
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
		public bool server; // = null;

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
			Write(server);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			server = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			server=default(bool);
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
		public PotionTypeRecipe[] potionTypeRecipes; // = null;
		public PotionContainerChangeRecipe[] potionContainerRecipes; // = null;
		public MaterialReducerRecipe[] materialReducerRecipes; // = null;
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
			Write(potionTypeRecipes);
			Write(potionContainerRecipes);
			Write(materialReducerRecipes);
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
			potionTypeRecipes = ReadPotionTypeRecipes();
			potionContainerRecipes = ReadPotionContainerChangeRecipes();
			materialReducerRecipes = ReadMaterialReducerRecipes();
			isClean = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			recipes=default(Recipes);
			potionTypeRecipes=default(PotionTypeRecipe[]);
			potionContainerRecipes=default(PotionContainerChangeRecipe[]);
			materialReducerRecipes=default(MaterialReducerRecipe[]);
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
		public long entityUniqueId; // = null;

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
			Write(entityUniqueId);

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
			entityUniqueId = ReadLong();

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
			entityUniqueId=default(long);
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

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			chunkX=default(int);
			chunkZ=default(int);
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

		public long runtimeEntityId; // = null;
		public int eventData; // = null;
		public byte eventType; // = null;
		public byte[] auxData; // = null;

		public McpeTelemetryEvent()
		{
			Id = 0x41;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(eventData);
			Write(eventType);
			Write(auxData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			eventData = ReadSignedVarInt();
			eventType = ReadByte();
			auxData = ReadBytes(0, true);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			eventData=default(int);
			eventType=default(byte);
			auxData=default(byte[]);
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
		public enum Type
		{
			AddBoss = 0,
			AddPlayer = 1,
			RemoveBoss = 2,
			RemovePlayer = 3,
			UpdateProgress = 4,
			UpdateName = 5,
			UpdateOptions = 6,
			UpdateStyle = 7,
			Query = 8,
		}

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
		public string xuid; // = null;
		public string platformOnlineId; // = null;

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
			Write(xuid);
			Write(platformOnlineId);

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
			xuid = ReadString();
			platformOnlineId = ReadString();

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
			xuid=default(string);
			platformOnlineId=default(string);
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
		public bool isVerified; // = null;

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
			Write(isVerified);

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
			isVerified = ReadBool();

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
			isVerified=default(bool);
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

	public partial class McpeSetScoreboardIdentity : Packet<McpeSetScoreboardIdentity>
	{
		public enum Operations
		{
			RegisterIdentity = 0,
			ClearIdentity = 1,
		}

		public ScoreboardIdentityEntries entries; // = null;

		public McpeSetScoreboardIdentity()
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

	public partial class McpeSetLocalPlayerAsInitialized : Packet<McpeSetLocalPlayerAsInitialized>
	{

		public long runtimeEntityId; // = null;

		public McpeSetLocalPlayerAsInitialized()
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

	public partial class McpeUpdateSoftEnum : Packet<McpeUpdateSoftEnum>
	{


		public McpeUpdateSoftEnum()
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

	public partial class McpeNetworkStackLatency : Packet<McpeNetworkStackLatency>
	{

		public ulong timestamp; // = null;
		public byte unknownFlag; // = null;

		public McpeNetworkStackLatency()
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
			unknownFlag = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			timestamp=default(ulong);
			unknownFlag=default(byte);
		}

	}

	public partial class McpeScriptCustomEvent : Packet<McpeScriptCustomEvent>
	{

		public string eventName; // = null;
		public string eventData; // = null;

		public McpeScriptCustomEvent()
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
		public string molangVariablesJson; // = null;

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
			Write(molangVariablesJson);

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
			molangVariablesJson = ReadString();

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
			molangVariablesJson=default(string);
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

		public bool enabled; // = null;

		public McpeClientCacheStatus()
		{
			Id = 0x81;
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

		public Nbt namedtag; // = null;

		public McpeUpdateBlockProperties()
		{
			Id = 0x86;
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

	public partial class McpeNetworkSettings : Packet<McpeNetworkSettings>
	{
		public enum Compression
		{
			Nothing = 0,
			Everything = 1,
		}

		public byte unknown; // = null;
		public byte compressionThreshold; // = null;

		public McpeNetworkSettings()
		{
			Id = 0x8f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(unknown);
			Write(compressionThreshold);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			unknown = ReadByte();
			compressionThreshold = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			unknown=default(byte);
			compressionThreshold=default(byte);
		}

	}

	public partial class McpePlayerAuthInput : Packet<McpePlayerAuthInput>
	{


		public McpePlayerAuthInput()
		{
			Id = 0x90;
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

	public partial class McpeCreativeContent : Packet<McpeCreativeContent>
	{

		public CreativeItemStacks input; // = null;

		public McpeCreativeContent()
		{
			Id = 0x91;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(input);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			input = ReadCreativeItemStacks();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			input=default(CreativeItemStacks);
		}

	}

	public partial class McpePlayerEnchantOptions : Packet<McpePlayerEnchantOptions>
	{

		public EnchantOptions enchantOptions; // = null;

		public McpePlayerEnchantOptions()
		{
			Id = 0x92;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(enchantOptions);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			enchantOptions = ReadEnchantOptions();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			enchantOptions=default(EnchantOptions);
		}

	}

	public partial class McpeItemStackRequest : Packet<McpeItemStackRequest>
	{
		public enum ActionType
		{
			Take = 0,
			Place = 1,
			Swap = 2,
			Drop = 3,
			Destroy = 4,
			Consume = 5,
			Create = 6,
			PlaceIntoBundle = 7,
			TakeFromBundle = 8,
			LabTableCombine = 9,
			BeaconPayment = 10,
			MineBlock = 11,
			CraftRecipe = 12,
			CraftRecipeAuto = 13,
			CraftCreative = 14,
			CraftRecipeOptional = 15,
			CraftGrindstone = 16,
			CraftLoom = 17,
			CraftNotImplementedDeprecated = 18,
			CraftResultsDeprecated = 19,
		}

		public ItemStackRequests requests; // = null;

		public McpeItemStackRequest()
		{
			Id = 0x93;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(requests);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			requests = ReadItemStackRequests();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			requests=default(ItemStackRequests);
		}

	}

	public partial class McpeItemStackResponse : Packet<McpeItemStackResponse>
	{

		public ItemStackResponses responses; // = null;

		public McpeItemStackResponse()
		{
			Id = 0x94;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(responses);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			responses = ReadItemStackResponses();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			responses=default(ItemStackResponses);
		}

	}

	public partial class McpeUpdatePlayerGameType : Packet<McpeUpdatePlayerGameType>
	{


		public McpeUpdatePlayerGameType()
		{
			Id = 0x97;
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

	public partial class McpePacketViolationWarning : Packet<McpePacketViolationWarning>
	{

		public int violationType; // = null;
		public int severity; // = null;
		public int packetId; // = null;
		public string reason; // = null;

		public McpePacketViolationWarning()
		{
			Id = 0x9c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(violationType);
			WriteSignedVarInt(severity);
			WriteSignedVarInt(packetId);
			Write(reason);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			violationType = ReadSignedVarInt();
			severity = ReadSignedVarInt();
			packetId = ReadSignedVarInt();
			reason = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			violationType=default(int);
			severity=default(int);
			packetId=default(int);
			reason=default(string);
		}

	}

	public partial class McpeItemComponent : Packet<McpeItemComponent>
	{

		public ItemComponentList entries; // = null;

		public McpeItemComponent()
		{
			Id = 0xa2;
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

			entries = ReadItemComponentList();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entries=default(ItemComponentList);
		}

	}

	public partial class McpeFilterTextPacket : Packet<McpeFilterTextPacket>
	{

		public string text; // = null;
		public bool fromServer; // = null;

		public McpeFilterTextPacket()
		{
			Id = 0xa3;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(text);
			Write(fromServer);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			text = ReadString();
			fromServer = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			text=default(string);
			fromServer=default(bool);
		}

	}

	public partial class McpeUpdateSubChunkBlocksPacket : Packet<McpeUpdateSubChunkBlocksPacket>
	{

		public BlockCoordinates subchunkCoordinates; // = null;
		public UpdateSubChunkBlocksPacketEntry[] layerZeroUpdates; // = null;
		public UpdateSubChunkBlocksPacketEntry[] layerOneUpdates; // = null;

		public McpeUpdateSubChunkBlocksPacket()
		{
			Id = 0xac;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(subchunkCoordinates);
			Write(layerZeroUpdates);
			Write(layerOneUpdates);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			subchunkCoordinates = ReadBlockCoordinates();
			layerZeroUpdates = ReadUpdateSubChunkBlocksPacketEntrys();
			layerOneUpdates = ReadUpdateSubChunkBlocksPacketEntrys();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			subchunkCoordinates=default(BlockCoordinates);
			layerZeroUpdates=default(UpdateSubChunkBlocksPacketEntry[]);
			layerOneUpdates=default(UpdateSubChunkBlocksPacketEntry[]);
		}

	}

	public partial class McpeSubChunkPacket : Packet<McpeSubChunkPacket>
	{

		public bool cacheEnabled; // = null;
		public int dimension; // = null;
		public BlockCoordinates subchunkCoordinates; // = null;

		public McpeSubChunkPacket()
		{
			Id = 0xae;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(cacheEnabled);
			WriteVarInt(dimension);
			Write(subchunkCoordinates);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			cacheEnabled = ReadBool();
			dimension = ReadVarInt();
			subchunkCoordinates = ReadBlockCoordinates();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			cacheEnabled=default(bool);
			dimension=default(int);
			subchunkCoordinates=default(BlockCoordinates);
		}

	}

	public partial class McpeSubChunkRequestPacket : Packet<McpeSubChunkRequestPacket>
	{

		public int dimension; // = null;
		public BlockCoordinates basePosition; // = null;
		public SubChunkPositionOffset[] offsets; // = null;

		public McpeSubChunkRequestPacket()
		{
			Id = 0xaf;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteVarInt(dimension);
			Write(basePosition);
			Write(offsets);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			dimension = ReadVarInt();
			basePosition = ReadBlockCoordinates();
			offsets = ReadSubChunkPositionOffsets();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			dimension=default(int);
			basePosition=default(BlockCoordinates);
			offsets=default(SubChunkPositionOffset[]);
		}

	}

	public partial class McpeDimensionData : Packet<McpeDimensionData>
	{

		public DimensionDefinitions definitions; // = null;

		public McpeDimensionData()
		{
			Id = 0xb4;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(definitions);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			definitions = ReadDimensionDefinitions();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			definitions=default(DimensionDefinitions);
		}

	}

	public partial class McpeUpdateAbilities : Packet<McpeUpdateAbilities>
	{

		public long entityUniqueId; // = null;
		public byte playerPermissions; // = null;
		public byte commandPermissions; // = null;
		public AbilityLayers layers; // = null;

		public McpeUpdateAbilities()
		{
			Id = 0xbb;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(entityUniqueId);
			Write(playerPermissions);
			Write(commandPermissions);
			Write(layers);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityUniqueId = ReadLong();
			playerPermissions = ReadByte();
			commandPermissions = ReadByte();
			layers = ReadAbilityLayers();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityUniqueId=default(long);
			playerPermissions=default(byte);
			commandPermissions=default(byte);
			layers=default(AbilityLayers);
		}

	}

	public partial class McpeUpdateAdventureSettings : Packet<McpeUpdateAdventureSettings>
	{

		public bool noPvm; // = null;
		public bool noMvp; // = null;
		public bool immutableWorld; // = null;
		public bool showNametags; // = null;
		public bool autoJump; // = null;

		public McpeUpdateAdventureSettings()
		{
			Id = 0xbc;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(noPvm);
			Write(noMvp);
			Write(immutableWorld);
			Write(showNametags);
			Write(autoJump);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			noPvm = ReadBool();
			noMvp = ReadBool();
			immutableWorld = ReadBool();
			showNametags = ReadBool();
			autoJump = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			noPvm=default(bool);
			noMvp=default(bool);
			immutableWorld=default(bool);
			showNametags=default(bool);
			autoJump=default(bool);
		}

	}

	public partial class McpeRequestAbility : Packet<McpeRequestAbility>
	{

		public int ability; // = null;

		public McpeRequestAbility()
		{
			Id = 0xb8;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteVarInt(ability);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			ability = ReadVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			ability=default(int);
		}

	}

	public partial class McpeAlexEntityAnimation : Packet<McpeAlexEntityAnimation>
	{

		public long runtimeEntityId; // = null;
		public string boneId; // = null;
		public AnimationKey[] keys; // = null;

		public McpeAlexEntityAnimation()
		{
			Id = 0xe0;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(boneId);
			Write(keys);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			boneId = ReadString();
			keys = ReadAnimationKeys();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			boneId=default(string);
			keys=default(AnimationKey[]);
		}

	}

	public partial class McpeWrapper : Packet<McpeWrapper>
	{


		public McpeWrapper()
		{
			Id = 0xfe;
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

