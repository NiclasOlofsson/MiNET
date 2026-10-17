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
using log4net;
using MiNET.Plugins;

namespace MiNET.Net
{
	public class BedrockMessageHandler : BedrockMessageHandlerBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BedrockMessageHandler));
		private PluginManager _pluginManager;

		public IMcpeMessageHandler Handler { get; set; }

		public BedrockMessageHandler(INetworkHandler session, IServerManager serverManager, PluginManager pluginManager) : base(session)
		{
			_pluginManager = pluginManager;
			Handler = new LoginMessageHandler(this, session, serverManager);
		}

		protected override object HandlerTarget => Handler;

		protected override bool HasPluginInterceptor(Type packetType) => _pluginManager?.HasReceivePacketHandler(packetType) ?? false;

		public override void Connected()
		{
		}

		public override void Disconnect(string reason, bool sendDisconnect = true)
		{
			Handler.Disconnect(reason, sendDisconnect);
		}

		/// <summary>
		///     Nothing here any more: plugins see outgoing packets in Player.SendPacket, before the
		///     packet is queued, so a suppressed one never reaches the send lane at all.
		/// </summary>
		public override Packet OnSendCustomPacket(Packet packet)
		{
			return packet;
		}

		public override void HandleCustomPacket(Packet message)
		{
			HandleBedrockMessage(Handler, message);
		}

		private void HandleBedrockMessage(IMcpeMessageHandler handler, Packet message)
		{
			if (handler is Player player)
			{
				Packet result = _pluginManager.PluginPacketHandler(message, true, player);
				if (result != message) message.PutPool();
				message = result;
			}

			if (message == null) return;

			// Raw intercept (MiNET.Tunnel and friends): a handler that consumes the frame here
			// keeps it out of normal dispatch entirely.
			if (handler is IRawPacketHandler raw && raw.HandleRawPacket(message)) return;

			switch (message)
			{
				case McpeClientToServerHandshake msg:
					// Start encryption
					handler.HandleMcpeClientToServerHandshake(msg);
					break;
				case McpeResourcePackClientResponse msg:
					handler.HandleMcpeResourcePackClientResponse(msg);
					break;
				case McpeResourcePackChunkRequest msg:
					handler.HandleMcpeResourcePackChunkRequest(msg);
					break;
				case McpeSetLocalPlayerAsInitialized msg:
					handler.HandleMcpeSetLocalPlayerAsInitialized(msg);
					break;
				case McpeUpdateBlock _:
					// DO NOT USE. Will dissapear from MCPE any release. 
					// It is a bug that it leaks these messages.
					break;
				case McpeLevelSoundEvent msg:
					handler.HandleMcpeLevelSoundEvent(msg);
					break;
				case McpeClientCacheStatus msg:
					handler.HandleMcpeClientCacheStatus(msg);
					break;
				case McpeClientCacheBlobStatus msg:
					handler.HandleMcpeClientCacheBlobStatus(msg);
					break;
				case McpeEmoteList msg:
					handler.HandleMcpeEmoteList(msg);
					break;
				case McpeAnimate msg:
					handler.HandleMcpeAnimate(msg);
					break;
				case McpeEntityEvent msg:
					handler.HandleMcpeEntityEvent(msg);
					break;
				case McpeText msg:
					handler.HandleMcpeText(msg);
					break;
				case McpeRemoveEntity _:
					// Do nothing right now, but should clear out the entities and stuff
					// from this players internal structure.
					break;
				case McpeLogin msg:
					handler.HandleMcpeLogin(msg);
					break;
				case McpeMovePlayer msg:
					handler.HandleMcpeMovePlayer(msg);
					break;
				case McpePlayerAuthInput msg:
					// The 1.26 client uses PlayerAuthInput (0x90) for all movement/input instead of
					// MovePlayer. Not dispatching it is what the client reports as "InitialConnection-90".
					handler.HandleMcpePlayerAuthInput(msg);
					break;
				case McpeServerBoundLoadingScreen msg:
					handler.HandleMcpeServerBoundLoadingScreen(msg);
					break;
				case McpeServerBoundDiagnostics msg:
					handler.HandleMcpeServerBoundDiagnostics(msg);
					break;
				case McpeClientCameraAimAssist msg:
					handler.HandleMcpeClientCameraAimAssist(msg);
					break;
				case McpeClientMovementPredictionSync msg:
					handler.HandleMcpeClientMovementPredictionSync(msg);
					break;
				case McpeUpdateClientOptions msg:
					handler.HandleMcpeUpdateClientOptions(msg);
					break;
				case McpeServerboundPackSettingChange msg:
					handler.HandleMcpeServerboundPackSettingChange(msg);
					break;
				case McpeServerboundDataStore msg:
					handler.HandleMcpeServerboundDataStore(msg);
					break;
				case McpeServerboundMatchmakingCancel msg:
					handler.HandleMcpeServerboundMatchmakingCancel(msg);
					break;
				case McpeServerboundCursorItemDrag msg:
					handler.HandleMcpeServerboundCursorItemDrag(msg);
					break;
				case McpeServerboundRegisterAudioContent msg:
					handler.HandleMcpeServerboundRegisterAudioContent(msg);
					break;
				case McpeChat msg:
					handler.HandleMcpeChat(msg);
					break;
				case McpeSetPlayerInventoryOptions msg:
					handler.HandleMcpeSetPlayerInventoryOptions(msg);
					break;
				case McpeResourcePacksReadyForValidation msg:
					handler.HandleMcpeResourcePacksReadyForValidation(msg);
					break;
				case McpePartyDestinationCookieResponse msg:
					handler.HandleMcpePartyDestinationCookieResponse(msg);
					break;
				case McpePartyChanged msg:
					handler.HandleMcpePartyChanged(msg);
					break;
				case McpeServerboundDataDrivenScreenClosed msg:
					handler.HandleMcpeServerboundDataDrivenScreenClosed(msg);
					break;
				case McpePlayerToggleCrafterSlotRequest msg:
					handler.HandleMcpePlayerToggleCrafterSlotRequest(msg);
					break;
				case McpeInteract msg:
					handler.HandleMcpeInteract(msg);
					break;
				case McpeRespawn msg:
					handler.HandleMcpeRespawn(msg);
					break;
				case McpeBlockEntityData msg:
					handler.HandleMcpeBlockEntityData(msg);
					break;
				case McpePlayerAction msg:
					handler.HandleMcpePlayerAction(msg);
					break;
				case McpeContainerClose msg:
					handler.HandleMcpeContainerClose(msg);
					break;
				case McpeMobEquipment msg:
					handler.HandleMcpeMobEquipment(msg);
					break;
				case McpeMobArmorEquipment msg:
					handler.HandleMcpeMobArmorEquipment(msg);
					break;
				case McpeInventoryTransaction msg:
					handler.HandleMcpeInventoryTransaction(msg);
					break;
				case McpeServerSettingsRequest msg:
					handler.HandleMcpeServerSettingsRequest(msg);
					break;
				case McpeSetPlayerGameType msg:
					handler.HandleMcpeSetPlayerGameType(msg);
					break;
				case McpePlayerHotbar msg:
					handler.HandleMcpePlayerHotbar(msg);
					break;
				case McpeInventoryContent msg:
					handler.HandleMcpeInventoryContent(msg);
					break;
				case McpeRequestChunkRadius msg:
					handler.HandleMcpeRequestChunkRadius(msg);
					break;
				case McpeMapInfoRequest msg:
					handler.HandleMcpeMapInfoRequest(msg);
					break;
				case McpeItemStackRequest nms:
					handler.HandleMcpeItemStackRequest(nms);
					break;
				case McpeCommandRequest msg:
					handler.HandleMcpeCommandRequest(msg);
					break;
				case McpeBlockPickRequest msg:
					handler.HandleMcpeBlockPickRequest(msg);
					break;
				case McpeEntityPickRequest msg:
					handler.HandleMcpeEntityPickRequest(msg);
					break;
				case McpeModalFormResponse msg:
					handler.HandleMcpeModalFormResponse(msg);
					break;
				case McpeCommandBlockUpdate msg:
					handler.HandleMcpeCommandBlockUpdate(msg);
					break;
				case McpeMoveEntity msg:
					handler.HandleMcpeMoveEntity(msg);
					break;
				case McpeSetEntityMotion msg:
					handler.HandleMcpeSetEntityMotion(msg);
					break;
				case McpePhotoTransfer msg:
					handler.HandleMcpePhotoTransfer(msg);
					break;
				case McpeSetEntityData msg:
					handler.HandleMcpeSetEntityData(msg);
					break;
				case McpeNpcRequest msg:
					handler.HandleMcpeNpcRequest(msg);
					break;
				case McpePacketViolationWarning msg:
					handler.HandleMcpePacketViolationWarning(msg);
					break;
				case McpeNetworkStackLatency msg:
					handler.HandleMcpeNetworkStackLatency(msg);
					break;
				case McpePlayerSkin msg:
					handler.HandleMcpePlayerSkin(msg);
					break;

				case McpeRequestNetworkSettings msg:
					handler.HandleMcpeRequestNetworkSettings(msg);
					break;

				case McpeGameTestRequest msg:
					handler.HandleMcpeGameTestRequest(msg);
					break;

				case McpeScriptMessage msg:
					handler.HandleMcpeScriptMessage(msg);
					break;

				case McpeCodeBuilderSource msg:
					handler.HandleMcpeCodeBuilderSource(msg);
					break;

				case McpeChangeMobProperty msg:
					handler.HandleMcpeChangeMobProperty(msg);
					break;

				case McpeRequestAbility msg:
					handler.HandleMcpeRequestAbility(msg);
					break;

				case McpeRequestPermissions msg:
					handler.HandleMcpeRequestPermissions(msg);
					break;

				case McpeEditorNetwork msg:
					handler.HandleMcpeEditorNetwork(msg);
					break;

				case McpeEmote msg:
					handler.HandleMcpeEmote(msg);
					break;

				case McpeMultiplayerSettings msg:
					handler.HandleMcpeMultiplayerSettings(msg);
					break;

				case McpeSettingsCommand msg:
					handler.HandleMcpeSettingsCommand(msg);
					break;

				case McpeAnvilDamage msg:
					handler.HandleMcpeAnvilDamage(msg);
					break;

				case McpePositionTrackingDbClientRequest msg:
					handler.HandleMcpePositionTrackingDbClientRequest(msg);
					break;

				case McpeDebugInfo msg:
					handler.HandleMcpeDebugInfo(msg);
					break;

				case McpeSubChunkRequestPacket msg:
					handler.HandleMcpeSubChunkRequestPacket(msg);
					break;

				default:
				{
					Log.Error($"Unhandled packet: {message.GetType().Name} 0x{message.Id:X2} for user: {_session.Username}, IP {_session.GetClientEndPoint().Address}");
					break;
				}
			}
		}
	}
}