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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Jose;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

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
			if (Log.IsDebugEnabled) Log.Debug($"Player status={message.status}");
			Client.PlayerStatus = message.status;

			if (Client.PlayerStatus == 3)
			{
				Client.HasSpawned = true;
				if (Client.IsEmulator)
				{
					Client.PlayerStatusChangedWaitHandle.Set();

					Client.SendMcpeMovePlayer();
				}
			}
		}

		public virtual void HandleMcpeServerToClientHandshake(McpeServerToClientHandshake message)
		{
			string token = message.token;
			Log.Debug($"JWT:\n{token}");

			IDictionary<string, dynamic> headers = JWT.Headers(token);
			string x5u = headers["x5u"];

			Log.Debug($"JWT payload:\n{JWT.Payload(token)}");

			ECPublicKeyParameters remotePublicKey = (ECPublicKeyParameters) PublicKeyFactory.CreateKey(x5u.DecodeBase64());

			var signParam = new ECParameters
			{
				Curve = ECCurve.NamedCurves.nistP384,
				Q =
				{
					X = remotePublicKey.Q.AffineXCoord.GetEncoded(),
					Y = remotePublicKey.Q.AffineYCoord.GetEncoded()
				},
			};
			signParam.Validate();

			var signKey = ECDsa.Create(signParam);

			try
			{
				var data = JWT.Decode<HandshakeData>(token, signKey);

				Client.InitiateEncryption(Base64Url.Decode(x5u), Base64Url.Decode(data.salt));
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

			if (message.resourcepackinfos.Count != 0)
			{
				ResourcePackIds resourcePackIds = new ResourcePackIds();

				foreach (var packInfo in message.resourcepackinfos)
				{
					resourcePackIds.Add(packInfo.PackIdVersion.Id);
				}

				McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
				response.responseStatus = 2;
				response.resourcepackids = resourcePackIds;
				Client.SendPacket(response);
			}
			else
			{
				McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
				response.responseStatus = 3;
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
				McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
				response.responseStatus = 4;
				Client.SendPacket(response);
			}
		}

		public abstract void HandleMcpeText(McpeText message);
		public abstract void HandleMcpeSetTime(McpeSetTime message);
		public abstract void HandleMcpeStartGame(McpeStartGame message);
		public abstract void HandleMcpeAddPlayer(McpeAddPlayer message);
		public abstract void HandleMcpeAddEntity(McpeAddEntity message);
		public abstract void HandleMcpeRemoveEntity(McpeRemoveEntity message);
		public abstract void HandleMcpeAddItemEntity(McpeAddItemEntity message);
		public abstract void HandleMcpeTakeItemEntity(McpeTakeItemEntity message);
		public abstract void HandleMcpeMoveEntity(McpeMoveEntity message);

		public virtual void HandleMcpeMovePlayer(McpeMovePlayer message)
		{
			if (message.runtimeEntityId != Client.EntityId) return;

			Client.CurrentLocation = new PlayerLocation(message.x, message.y, message.z);
			Log.Debug($"Position: {Client.CurrentLocation}");

			Client.LevelInfo.SpawnX = (int) message.x;
			Client.LevelInfo.SpawnY = (int) message.y;
			Client.LevelInfo.SpawnZ = (int) message.z;

			Client.SendMcpeMovePlayer();
		}

		public abstract void HandleMcpeRiderJump(McpeRiderJump message);
		public abstract void HandleMcpeUpdateBlock(McpeUpdateBlock message);
		public abstract void HandleMcpeAddPainting(McpeAddPainting message);
		public abstract void HandleMcpeTickSync(McpeTickSync message);
		public abstract void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
		public abstract void HandleMcpeLevelEvent(McpeLevelEvent message);
		public abstract void HandleMcpeBlockEvent(McpeBlockEvent message);
		public abstract void HandleMcpeEntityEvent(McpeEntityEvent message);
		public abstract void HandleMcpeMobEffect(McpeMobEffect message);
		public abstract void HandleMcpeUpdateAttributes(McpeUpdateAttributes message);
		public abstract void HandleMcpeInventoryTransaction(McpeInventoryTransaction message);
		public abstract void HandleMcpeMobEquipment(McpeMobEquipment message);
		public abstract void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
		public abstract void HandleMcpeInteract(McpeInteract message);
		public abstract void HandleMcpeHurtArmor(McpeHurtArmor message);
		public abstract void HandleMcpeSetEntityData(McpeSetEntityData message);
		public abstract void HandleMcpeSetEntityMotion(McpeSetEntityMotion message);
		public abstract void HandleMcpeSetEntityLink(McpeSetEntityLink message);
		public abstract void HandleMcpeSetHealth(McpeSetHealth message);
		public abstract void HandleMcpeSetSpawnPosition(McpeSetSpawnPosition message);
		public abstract void HandleMcpeAnimate(McpeAnimate message);

		public virtual void HandleMcpeRespawn(McpeRespawn message)
		{
			Client.CurrentLocation = new PlayerLocation(message.x, message.y, message.z);
		}

		public abstract void HandleMcpeContainerOpen(McpeContainerOpen message);
		public abstract void HandleMcpeContainerClose(McpeContainerClose message);
		public abstract void HandleMcpePlayerHotbar(McpePlayerHotbar message);
		public abstract void HandleMcpeInventoryContent(McpeInventoryContent message);
		public abstract void HandleMcpeInventorySlot(McpeInventorySlot message);
		public abstract void HandleMcpeContainerSetData(McpeContainerSetData message);
		public abstract void HandleMcpeCraftingData(McpeCraftingData message);
		public abstract void HandleMcpeCraftingEvent(McpeCraftingEvent message);
		public abstract void HandleMcpeGuiDataPickItem(McpeGuiDataPickItem message);

		public virtual void HandleMcpeAdventureSettings(McpeAdventureSettings message)
		{
			Client.UserPermission = (CommandPermission) message.commandPermission;
		}

		public abstract void HandleMcpeBlockEntityData(McpeBlockEntityData message);
		public abstract void HandleMcpeLevelChunk(McpeLevelChunk message);
		public abstract void HandleMcpeSetCommandsEnabled(McpeSetCommandsEnabled message);
		public abstract void HandleMcpeSetDifficulty(McpeSetDifficulty message);

		public virtual void HandleMcpeChangeDimension(McpeChangeDimension message)
		{
			Thread.Sleep(3000);
			McpePlayerAction action = McpePlayerAction.CreateObject();
			action.runtimeEntityId = Client.EntityId;
			action.actionId = (int) PlayerAction.DimensionChangeAck;
			Client.SendPacket(action);
		}

		public abstract void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message);
		public abstract void HandleMcpePlayerList(McpePlayerList message);
		public abstract void HandleMcpeSimpleEvent(McpeSimpleEvent message);
		public abstract void HandleMcpeTelemetryEvent(McpeTelemetryEvent message);
		public abstract void HandleMcpeSpawnExperienceOrb(McpeSpawnExperienceOrb message);
		public abstract void HandleMcpeClientboundMapItemData(McpeClientboundMapItemData message);
		public abstract void HandleMcpeMapInfoRequest(McpeMapInfoRequest message);
		public abstract void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message);
		public abstract void HandleMcpeChunkRadiusUpdate(McpeChunkRadiusUpdate message);
		public abstract void HandleMcpeItemFrameDropItem(McpeItemFrameDropItem message);
		public abstract void HandleMcpeGameRulesChanged(McpeGameRulesChanged message);
		public abstract void HandleMcpeCamera(McpeCamera message);
		public abstract void HandleMcpeBossEvent(McpeBossEvent message);
		public abstract void HandleMcpeShowCredits(McpeShowCredits message);
		public abstract void HandleMcpeAvailableCommands(McpeAvailableCommands message);
		public abstract void HandleMcpeCommandOutput(McpeCommandOutput message);
		public abstract void HandleMcpeUpdateTrade(McpeUpdateTrade message);
		public abstract void HandleMcpeUpdateEquipment(McpeUpdateEquipment message);

		protected Dictionary<string, uint> resourcePackDataInfos = new Dictionary<string, uint>();

		public virtual void HandleMcpeResourcePackDataInfo(McpeResourcePackDataInfo message)
		{
			var packageId = message.packageId;
			McpeResourcePackChunkRequest request = new McpeResourcePackChunkRequest();
			request.packageId = packageId;
			request.chunkIndex = 0;
			Client.SendPacket(request);
			resourcePackDataInfos.Add(message.packageId, message.chunkCount);
		}

		public virtual void HandleMcpeResourcePackChunkData(McpeResourcePackChunkData message)
		{
			if (message.chunkIndex + 1 < resourcePackDataInfos[message.packageId])
			{
				var packageId = message.packageId;
				McpeResourcePackChunkRequest request = new McpeResourcePackChunkRequest();
				request.packageId = packageId;
				request.chunkIndex = message.chunkIndex + 1;
				Client.SendPacket(request);
			}
			else
			{
				resourcePackDataInfos.Remove(message.packageId);
			}

			if (resourcePackDataInfos.Count == 0)
			{
				McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
				response.responseStatus = 3;
				Client.SendPacket(response);
			}
		}

		public abstract void HandleMcpeTransfer(McpeTransfer message);
		public abstract void HandleMcpePlaySound(McpePlaySound message);
		public abstract void HandleMcpeStopSound(McpeStopSound message);
		public abstract void HandleMcpeSetTitle(McpeSetTitle message);
		public abstract void HandleMcpeAddBehaviorTree(McpeAddBehaviorTree message);
		public abstract void HandleMcpeStructureBlockUpdate(McpeStructureBlockUpdate message);
		public abstract void HandleMcpeShowStoreOffer(McpeShowStoreOffer message);
		public abstract void HandleMcpePlayerSkin(McpePlayerSkin message);
		public abstract void HandleMcpeSubClientLogin(McpeSubClientLogin message);
		public abstract void HandleMcpeInitiateWebSocketConnection(McpeInitiateWebSocketConnection message);
		public abstract void HandleMcpeSetLastHurtBy(McpeSetLastHurtBy message);
		public abstract void HandleMcpeBookEdit(McpeBookEdit message);
		public abstract void HandleMcpeNpcRequest(McpeNpcRequest message);
		public abstract void HandleMcpeModalFormRequest(McpeModalFormRequest message);
		public abstract void HandleMcpeServerSettingsResponse(McpeServerSettingsResponse message);
		public abstract void HandleMcpeShowProfile(McpeShowProfile message);
		public abstract void HandleMcpeSetDefaultGameType(McpeSetDefaultGameType message);
		public abstract void HandleMcpeRemoveObjective(McpeRemoveObjective message);
		public abstract void HandleMcpeSetDisplayObjective(McpeSetDisplayObjective message);
		public abstract void HandleMcpeSetScore(McpeSetScore message);
		public abstract void HandleMcpeLabTable(McpeLabTable message);
		public abstract void HandleMcpeUpdateBlockSynced(McpeUpdateBlockSynced message);
		public abstract void HandleMcpeMoveEntityDelta(McpeMoveEntityDelta message);
		public abstract void HandleMcpeSetScoreboardIdentityPacket(McpeSetScoreboardIdentityPacket message);
		public abstract void HandleMcpeUpdateSoftEnumPacket(McpeUpdateSoftEnumPacket message);
		public abstract void HandleMcpeNetworkStackLatencyPacket(McpeNetworkStackLatencyPacket message);
		public abstract void HandleMcpeScriptCustomEventPacket(McpeScriptCustomEventPacket message);
		public abstract void HandleMcpeLevelSoundEventOld(McpeLevelSoundEventOld message);
		public abstract void HandleMcpeSpawnParticleEffect(McpeSpawnParticleEffect message);
		public abstract void HandleMcpeAvailableEntityIdentifiers(McpeAvailableEntityIdentifiers message);
		public abstract void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message);
		public abstract void HandleMcpeBiomeDefinitionList(McpeBiomeDefinitionList message);
		public abstract void HandleMcpeLevelSoundEventV2(McpeLevelSoundEventV2 message);

		public abstract void HandleFtlCreatePlayer(FtlCreatePlayer message);

		public abstract void HandleMcpeLevelEventGeneric(McpeLevelEventGeneric message);
		public abstract void HandleMcpeLecternUpdate(McpeLecternUpdate message);

		public abstract void HandleMcpeVideoStreamConnect(McpeVideoStreamConnect message);

		public abstract void HandleMcpeClientCacheStatus(McpeClientCacheStatus message);
		public abstract void HandleMcpeOnScreenTextureAnimation(McpeOnScreenTextureAnimation message);
		public abstract void HandleMcpeMapCreateLockedCopy(McpeMapCreateLockedCopy message);
		public abstract void HandleMcpeStructureTemplateDataExportRequest(McpeStructureTemplateDataExportRequest message);
		public abstract void HandleMcpeStructureTemplateDataExportResponse(McpeStructureTemplateDataExportResponse message);
		public abstract void HandleMcpeUpdateBlockProperties(McpeUpdateBlockProperties message);
		public abstract void HandleMcpeClientCacheBlobStatus(McpeClientCacheBlobStatus message);
		public abstract void HandleMcpeClientCacheMissResponse(McpeClientCacheMissResponse message);
	}
}