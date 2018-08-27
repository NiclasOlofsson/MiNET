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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

using System;
using System.Net;
using System.Numerics;
using System.Threading;
using System.Collections.Generic;
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
		public const int ProtocolVersion = 290;
		public const string GameVersion = "1.7.0.2";
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
		void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
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
        void HandleMcpeRemoveObjective(McpeRemoveObjective mesage);
        void HandleMcpeSetDisplayObjective(McpeSetDisplayObjective message);
        void HandleMcpeSetScore(McpeSetScore message);
		void HandleMcpeLabTable(McpeLabTable message);
		void HandleMcpeSetLocalPlayerAsInitializedPacket(McpeSetLocalPlayerAsInitialized message);
        void HandleSetScoreboardIdentity(McpeSetScoreboardIdentity message);
        void HandleUpdateEnumSoft(McpeUpdateSoftEnum message);
        void HandleNetworkStackLatency(McpeNetworkStackLatency message);
        void HandleScriptCustomEvent(McpeScriptCustomEvent message);
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
						packet = McpeExplode.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x18:
						packet = McpeLevelSoundEvent.CreateObject();
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
						packet = McpeFullChunkData.CreateObject();
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
						packet = McpeSetScoreboardIdentity.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x71:
						packet = McpeSetLocalPlayerAsInitialized.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x72:
						packet = McpeUpdateSoftEnum.CreateObject();
						packet.Decode(buffer);
						return packet;
					case 0x73:
						packet = McpeNetworkStackLatency.CreateObject();
						packet.Decode(buffer);
						return packet;
                    case 0x75:
                        packet = McpeScriptCustomEvent.CreateObject();
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
			behahaviorpackinfos=default(ResourcePackInfos);
			resourcepackinfos=default(ResourcePackInfos);
		}

	}

	public partial class McpeResourcePackStack : Packet<McpeResourcePackStack>
	{

		public bool mustAccept; // = null;
		public ResourcePackIdVersions behaviorpackidversions; // = null;
		public ResourcePackIdVersions resourcepackidversions; // = null;

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
		public Vector2 unknown1; // = null;
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
		public bool eduMode; // = null;
		public bool hasEduFeaturesEnabled; // = null;
		public float rainLevel; // = null;
		public float lightningLevel; // = null;
		public bool isMultiplayer; // = null;
		public bool broadcastToLan; // = null;
		public bool broadcastToXbl; // = null;
		public bool enableCommands; // = null;
		public bool isTexturepacksRequired; // = null;
		public GameRules gamerules; // = null;
		public bool bonusChest; // = null;
		public bool mapEnabled; // = null;
		public bool trustPlayers; // = null;
		public int permissionLevel; // = null;
		public int gamePublishSetting; // = null;
		public int serverChunkTickRange; // = null;
		public bool hasPlatformBroadcast; // = null;
		public int platformBroadcastMode; // = null;
		public bool xboxLiveBroadcastIntent; // = null;
		public bool hasLockedBehaviorPack; // = null;
		public bool hasLockedResourcePack; // = null;
		public bool isFromLockedWorldTemplate; // = null;
		public string levelId; // = null;
		public string worldName; // = null;
		public string premiumWorldTemplateId; // = null;
		public bool isTrial; // = null;
		public long currentTick; // = null;
		public int enchantmentSeed; // = null;
		public Blockstates blockstates; // = null;
		public string multiplayerCorrelationId; // = null;
        public bool useMsaGamertagsOnly; // = null;

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
			Write(unknown1);
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
			Write(eduMode);
			Write(hasEduFeaturesEnabled);
			Write(rainLevel);
			Write(lightningLevel);
			Write(isMultiplayer);
			Write(broadcastToLan);
			Write(broadcastToXbl);
			Write(enableCommands);
			Write(isTexturepacksRequired);
			Write(gamerules);
			Write(bonusChest);
			Write(mapEnabled);
			Write(trustPlayers);
			WriteSignedVarInt(permissionLevel);
			WriteSignedVarInt(gamePublishSetting);
			Write(serverChunkTickRange);
			Write(hasPlatformBroadcast);
			WriteSignedVarInt(platformBroadcastMode);
			Write(xboxLiveBroadcastIntent);
			Write(hasLockedBehaviorPack);
			Write(hasLockedResourcePack);
			Write(isFromLockedWorldTemplate);
            Write(useMsaGamertagsOnly);
			Write(levelId);
			Write(worldName);
			Write(premiumWorldTemplateId);
			Write(isTrial);
			Write(currentTick);
			WriteSignedVarInt(enchantmentSeed);
			Write(blockstates);
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
			unknown1 = ReadVector2();
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
			eduMode = ReadBool();
			hasEduFeaturesEnabled = ReadBool();
			rainLevel = ReadFloat();
			lightningLevel = ReadFloat();
			isMultiplayer = ReadBool();
			broadcastToLan = ReadBool();
			broadcastToXbl = ReadBool();
			enableCommands = ReadBool();
			isTexturepacksRequired = ReadBool();
			gamerules = ReadGameRules();
			bonusChest = ReadBool();
			mapEnabled = ReadBool();
			trustPlayers = ReadBool();
			permissionLevel = ReadSignedVarInt();
			gamePublishSetting = ReadSignedVarInt();
			serverChunkTickRange = ReadInt();
			hasPlatformBroadcast = ReadBool();
			platformBroadcastMode = ReadSignedVarInt();
			xboxLiveBroadcastIntent = ReadBool();
			hasLockedBehaviorPack = ReadBool();
			hasLockedResourcePack = ReadBool();
			isFromLockedWorldTemplate = ReadBool();
            useMsaGamertagsOnly = ReadBool();
			levelId = ReadString();
			worldName = ReadString();
			premiumWorldTemplateId = ReadString();
			isTrial = ReadBool();
			currentTick = ReadLong();
			enchantmentSeed = ReadSignedVarInt();
			blockstates = ReadBlockstates();
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
			unknown1=default(Vector2);
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
			eduMode=default(bool);
			hasEduFeaturesEnabled=default(bool);
			rainLevel=default(float);
			lightningLevel=default(float);
			isMultiplayer=default(bool);
			broadcastToLan=default(bool);
			broadcastToXbl=default(bool);
			enableCommands=default(bool);
			isTexturepacksRequired=default(bool);
			gamerules=default(GameRules);
			bonusChest=default(bool);
			mapEnabled=default(bool);
			trustPlayers=default(bool);
			permissionLevel=default(int);
			gamePublishSetting=default(int);
			serverChunkTickRange=default(int);
			hasPlatformBroadcast=default(bool);
			platformBroadcastMode=default(int);
			xboxLiveBroadcastIntent=default(bool);
			hasLockedBehaviorPack=default(bool);
			hasLockedResourcePack=default(bool);
			isFromLockedWorldTemplate=default(bool);
			levelId=default(string);
			worldName=default(string);
			premiumWorldTemplateId=default(string);
			isTrial=default(bool);
			currentTick=default(long);
			enchantmentSeed=default(int);
			blockstates=default(Blockstates);
			multiplayerCorrelationId=default(string);
		}

	}

	public partial class McpeAddPlayer : Packet<McpeAddPlayer>
	{

		public UUID uuid; // = null;
		public string username; // = null;
		public string thirdPartyName; // = null;
		public int platform; // = null;
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
			Write(thirdPartyName);
			WriteSignedVarInt(platform);
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
			thirdPartyName = ReadString();
			platform = ReadSignedVarInt();
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

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			uuid=default(UUID);
			username=default(string);
			thirdPartyName=default(string);
			platform=default(int);
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
		}

	}

	public partial class McpeAddEntity : Packet<McpeAddEntity>
	{

		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public uint entityType; // = null;
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
			WriteUnsignedVarInt(entityType);
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
			entityType = ReadUnsignedVarInt();
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
			entityType=default(uint);
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

	public partial class McpeExplode : Packet<McpeExplode>
	{

		public Vector3 position; // = null;
		public int radius; // = null;
		public Records records; // = null;

		public McpeExplode()
		{
			Id = 0x17;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(position);
			WriteSignedVarInt(radius);
			Write(records);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			position = ReadVector3();
			radius = ReadSignedVarInt();
			records = ReadRecords();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			position=default(Vector3);
			radius=default(int);
			records=default(Records);
		}

	}

	public partial class McpeLevelSoundEvent : Packet<McpeLevelSoundEvent>
	{

		public byte soundId; // = null;
		public Vector3 position; // = null;
		public int blockId; // = null;
		public int entityType; // = null;
		public bool isBabyMob; // = null;
		public bool isGlobal; // = null;

		public McpeLevelSoundEvent()
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
			LeaveCehicle = 3,
			MouseOver = 4,
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

		public float x; // = null;
		public float y; // = null;
		public float z; // = null;

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

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			recipes = ReadRecipes();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			recipes=default(Recipes);
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

	public partial class McpeFullChunkData : Packet<McpeFullChunkData>
	{

		public int chunkX; // = null;
		public int chunkZ; // = null;
		public byte[] chunkData; // = null;

		public McpeFullChunkData()
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


		public McpeCommandBlockUpdate()
		{
			Id = 0x4e;
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
		public string hash; // = null;

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
			Write(hash);

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
			hash = ReadString();

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
			hash=default(string);
		}

	}

	public partial class McpeResourcePackChunkData : Packet<McpeResourcePackChunkData>
	{

		public string packageId; // = null;
		public uint chunkIndex; // = null;
		public ulong progress; // = null;
		public uint length; // = null;
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
			Write(length);
			Write(payload);

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
			length = ReadUint();
			payload = ReadBytes((int) length);

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
			length=default(uint);
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
		public string skinId; // = null;
		public string skinName; // = null;
		public string oldSkinName; // = null;
		public byte[] skinData; // = null;
		public byte[] capeData; // = null;
		public string geometryModel; // = null;
		public string geometryData; // = null;

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
			Write(skinId);
			Write(skinName);
			Write(oldSkinName);
			WriteByteArray(skinData);
			WriteByteArray(capeData);
			Write(geometryModel);
			Write(geometryData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			uuid = ReadUUID();
			skinId = ReadString();
			skinName = ReadString();
			oldSkinName = ReadString();
			skinData = ReadByteArray();
			capeData = ReadByteArray();
			geometryModel = ReadString();
			geometryData = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			uuid=default(UUID);
			skinId=default(string);
			skinName=default(string);
			oldSkinName=default(string);
			skinData=default(byte[]);
			capeData=default(byte[]);
			geometryModel=default(string);
			geometryData=default(string);
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
			ModifyScore = 0,
			RemoveScore = 1,
		}

		public byte type; // = null;
		public ScorePacketInfos scorePacketInfos; // = null;

		public McpeSetScore()
		{
			Id = 0x6c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(type);
			Write(scorePacketInfos);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			type = ReadByte();
			scorePacketInfos = ReadScorePacketInfos();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			type=default(byte);
			scorePacketInfos=default(ScorePacketInfos);
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
		public byte flags; // = null;

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
			flags = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			flags=default(byte);
		}

	}

	public partial class McpeSetScoreboardIdentity : Packet<McpeSetScoreboardIdentity>
	{
        public enum Types
        {
            RegisterIdentity = 0,
            ClearIdentity = 1
        }

        public byte type; // = null;
        public ScoreboardIdentityPackets scoreboardIdentityPackets; // = null;

		public McpeSetScoreboardIdentity()
		{
			Id = 0x70;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();
            Write(type);
            Write(scoreboardIdentityPackets, type);
			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();
            type = ReadByte();
            scoreboardIdentityPackets = ReadScoreboardIdentityPackets(type);
			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();
            type = default(byte);
            scoreboardIdentityPackets = default(ScoreboardIdentityPackets);
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
            runtimeEntityId = default(long);
		}

	}

    public class EnumValues : List<string> { }

	public partial class McpeUpdateSoftEnum : Packet<McpeUpdateSoftEnum>
	{
        public string enumName; // = null;
        public EnumValues values; // = null;
        public byte type; // = null;

		public McpeUpdateSoftEnum()
		{
			Id = 0x72;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();
            Write(enumName);
            Write(values);
            Write(type);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();
            enumName = ReadString();
            values = ReadEnumValues();
            type = ReadByte();
			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();
            enumName = default(string);
            values = default(EnumValues);
            type = default(byte);
		}

	}

	public partial class McpeNetworkStackLatency : Packet<McpeNetworkStackLatency>
	{

        public ulong timestamp;

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

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();
            timestamp = ReadUlong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();
            timestamp = default(ulong);
		}

	}

    public partial class McpeScriptCustomEvent : Packet<McpeScriptCustomEvent>
    {
        public McpeScriptCustomEvent()
        {
            Id = 0x75;
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

