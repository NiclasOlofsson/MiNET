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
using MiNET.Utils; 
using MiNET.Utils.Skins;
using MiNET.Items;
using MiNET.Crafting;
using little = MiNET.Utils.Int24; // friendly name
using LongString = System.String;

namespace MiNET.Net
{
	public interface IMcpeMessageHandler
	{
		void Disconnect(string reason, bool sendDisconnect = true);

		void HandleMcpeLogin(McpeLogin message);
		void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message);
		void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message);
		void HandleMcpeText(McpeText message);
		void HandleMcpeMovePlayer(McpeMovePlayer message);
		void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
		void HandleMcpeEntityEvent(McpeEntityEvent message);
		void HandleMcpeInventoryTransaction(McpeInventoryTransaction message);
		void HandleMcpeMobEquipment(McpeMobEquipment message);
		void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
		void HandleMcpeInteract(McpeInteract message);
		void HandleMcpeBlockPickRequest(McpeBlockPickRequest message);
		void HandleMcpePlayerAction(McpePlayerAction message);
		void HandleMcpeEntityFall(McpeEntityFall message);
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
		void HandleMcpeModalFormResponse(McpeModalFormResponse message);
		void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message);
	}

	public class PackageFactory
	{
		public static Package CreatePackage(byte messageId, byte[] buffer, string ns)
		{
			Package package = null; 
			if(ns == "raknet") 
			{
				switch (messageId)
				{
					case 0x00:
						package = ConnectedPing.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x01:
						package = UnconnectedPing.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x03:
						package = ConnectedPong.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x04:
						package = DetectLostConnections.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x1c:
						package = UnconnectedPong.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x05:
						package = OpenConnectionRequest1.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x06:
						package = OpenConnectionReply1.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x07:
						package = OpenConnectionRequest2.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x08:
						package = OpenConnectionReply2.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x09:
						package = ConnectionRequest.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x10:
						package = ConnectionRequestAccepted.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x13:
						package = NewIncomingConnection.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x14:
						package = NoFreeIncomingConnections.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x15:
						package = DisconnectionNotification.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x17:
						package = ConnectionBanned.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x1A:
						package = IpRecentlyConnected.CreateObject();
						package.Decode(buffer);
						return package;
					case 0xfe:
						package = McpeWrapper.CreateObject();
						package.Decode(buffer);
						return package;
				}
			} else if(ns == "ftl") 
			{
				switch (messageId)
				{
					case 0x01:
						package = FtlCreatePlayer.CreateObject();
						package.Decode(buffer);
						return package;
				}
			} else {

				switch (messageId)
				{
					case 0x01:
						package = McpeLogin.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x02:
						package = McpePlayStatus.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x03:
						package = McpeServerToClientHandshake.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x04:
						package = McpeClientToServerHandshake.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x05:
						package = McpeDisconnect.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x06:
						package = McpeResourcePacksInfo.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x07:
						package = McpeResourcePackStack.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x08:
						package = McpeResourcePackClientResponse.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x09:
						package = McpeText.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x0a:
						package = McpeSetTime.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x0b:
						package = McpeStartGame.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x0c:
						package = McpeAddPlayer.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x0d:
						package = McpeAddEntity.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x0e:
						package = McpeRemoveEntity.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x0f:
						package = McpeAddItemEntity.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x11:
						package = McpeTakeItemEntity.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x12:
						package = McpeMoveEntity.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x13:
						package = McpeMovePlayer.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x14:
						package = McpeRiderJump.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x15:
						package = McpeUpdateBlock.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x16:
						package = McpeAddPainting.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x17:
						package = McpeExplode.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x18:
						package = McpeLevelSoundEvent.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x19:
						package = McpeLevelEvent.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x1a:
						package = McpeBlockEvent.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x1b:
						package = McpeEntityEvent.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x1c:
						package = McpeMobEffect.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x1d:
						package = McpeUpdateAttributes.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x1e:
						package = McpeInventoryTransaction.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x1f:
						package = McpeMobEquipment.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x20:
						package = McpeMobArmorEquipment.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x21:
						package = McpeInteract.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x22:
						package = McpeBlockPickRequest.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x23:
						package = McpeEntityPickRequest.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x24:
						package = McpePlayerAction.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x25:
						package = McpeEntityFall.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x26:
						package = McpeHurtArmor.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x27:
						package = McpeSetEntityData.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x28:
						package = McpeSetEntityMotion.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x29:
						package = McpeSetEntityLink.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x2a:
						package = McpeSetHealth.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x2b:
						package = McpeSetSpawnPosition.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x2c:
						package = McpeAnimate.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x2d:
						package = McpeRespawn.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x2e:
						package = McpeContainerOpen.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x2f:
						package = McpeContainerClose.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x30:
						package = McpePlayerHotbar.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x31:
						package = McpeInventoryContent.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x32:
						package = McpeInventorySlot.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x33:
						package = McpeContainerSetData.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x34:
						package = McpeCraftingData.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x35:
						package = McpeCraftingEvent.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x36:
						package = McpeGuiDataPickItem.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x37:
						package = McpeAdventureSettings.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x38:
						package = McpeBlockEntityData.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x39:
						package = McpePlayerInput.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x3a:
						package = McpeFullChunkData.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x3b:
						package = McpeSetCommandsEnabled.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x3c:
						package = McpeSetDifficulty.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x3d:
						package = McpeChangeDimension.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x3e:
						package = McpeSetPlayerGameType.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x3f:
						package = McpePlayerList.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x40:
						package = McpeSimpleEvent.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x41:
						package = McpeTelemetryEvent.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x42:
						package = McpeSpawnExperienceOrb.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x43:
						package = McpeClientboundMapItemData.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x44:
						package = McpeMapInfoRequest.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x45:
						package = McpeRequestChunkRadius.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x46:
						package = McpeChunkRadiusUpdate.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x47:
						package = McpeItemFrameDropItem.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x48:
						package = McpeGameRulesChanged.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x49:
						package = McpeCamera.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x4a:
						package = McpeBossEvent.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x4b:
						package = McpeShowCredits.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x4c:
						package = McpeAvailableCommands.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x4d:
						package = McpeCommandRequest.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x4e:
						package = McpeCommandBlockUpdate.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x50:
						package = McpeUpdateTrade.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x51:
						package = McpeUpdateEquipment.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x52:
						package = McpeResourcePackDataInfo.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x53:
						package = McpeResourcePackChunkData.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x54:
						package = McpeResourcePackChunkRequest.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x55:
						package = McpeTransfer.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x56:
						package = McpePlaySound.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x57:
						package = McpeStopSound.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x58:
						package = McpeSetTitle.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x59:
						package = McpeAddBehaviorTree.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x5a:
						package = McpeStructureBlockUpdate.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x5b:
						package = McpeShowStoreOffer.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x5c:
						package = McpePurchaseReceipt.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x5d:
						package = McpePlayerSkin.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x5e:
						package = McpeSubClientLogin.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x5f:
						package = McpeInitiateWebSocketConnection.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x60:
						package = McpeSetLastHurtBy.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x61:
						package = McpeBookEdit.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x62:
						package = McpeNpcRequest.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x64:
						package = McpeModalFormRequest.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x65:
						package = McpeModalFormResponse.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x66:
						package = McpeServerSettingsRequest.CreateObject();
						package.Decode(buffer);
						return package;
					case 0x67:
						package = McpeServerSettingsResponse.CreateObject();
						package.Decode(buffer);
						return package;
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

	public partial class ConnectedPing : Package<ConnectedPing>
	{

		public long sendpingtime; // = null;

		public ConnectedPing()
		{
			Id = 0x00;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(sendpingtime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			sendpingtime = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			sendpingtime=default(long);
		}

	}

	public partial class UnconnectedPing : Package<UnconnectedPing>
	{

		public long pingId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long guid; // = null;

		public UnconnectedPing()
		{
			Id = 0x01;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(pingId);
			Write(offlineMessageDataId);
			Write(guid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			pingId = ReadLong();
			ReadBytes(offlineMessageDataId.Length);
			guid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			pingId=default(long);
			guid=default(long);
		}

	}

	public partial class ConnectedPong : Package<ConnectedPong>
	{

		public long sendpingtime; // = null;
		public long sendpongtime; // = null;

		public ConnectedPong()
		{
			Id = 0x03;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(sendpingtime);
			Write(sendpongtime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			sendpingtime = ReadLong();
			sendpongtime = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			sendpingtime=default(long);
			sendpongtime=default(long);
		}

	}

	public partial class DetectLostConnections : Package<DetectLostConnections>
	{


		public DetectLostConnections()
		{
			Id = 0x04;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class UnconnectedPong : Package<UnconnectedPong>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(pingId);
			Write(serverId);
			Write(offlineMessageDataId);
			WriteFixedString(serverName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			pingId = ReadLong();
			serverId = ReadLong();
			ReadBytes(offlineMessageDataId.Length);
			serverName = ReadFixedString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			pingId=default(long);
			serverId=default(long);
			serverName=default(string);
		}

	}

	public partial class OpenConnectionRequest1 : Package<OpenConnectionRequest1>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public byte raknetProtocolVersion; // = null;

		public OpenConnectionRequest1()
		{
			Id = 0x05;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(raknetProtocolVersion);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			raknetProtocolVersion = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			raknetProtocolVersion=default(byte);
		}

	}

	public partial class OpenConnectionReply1 : Package<OpenConnectionReply1>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(serverGuid);
			Write(serverHasSecurity);
			WriteBe(mtuSize);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();
			serverHasSecurity = ReadByte();
			mtuSize = ReadShortBe();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			serverGuid=default(long);
			serverHasSecurity=default(byte);
			mtuSize=default(short);
		}

	}

	public partial class OpenConnectionRequest2 : Package<OpenConnectionRequest2>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(remoteBindingAddress);
			WriteBe(mtuSize);
			Write(clientGuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			remoteBindingAddress = ReadIPEndPoint();
			mtuSize = ReadShortBe();
			clientGuid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			remoteBindingAddress=default(IPEndPoint);
			mtuSize=default(short);
			clientGuid=default(long);
		}

	}

	public partial class OpenConnectionReply2 : Package<OpenConnectionReply2>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			serverGuid=default(long);
			clientEndpoint=default(IPEndPoint);
			mtuSize=default(short);
			doSecurityAndHandshake=default(byte[]);
		}

	}

	public partial class ConnectionRequest : Package<ConnectionRequest>
	{

		public long clientGuid; // = null;
		public long timestamp; // = null;
		public byte doSecurity; // = null;

		public ConnectionRequest()
		{
			Id = 0x09;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(clientGuid);
			Write(timestamp);
			Write(doSecurity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			clientGuid = ReadLong();
			timestamp = ReadLong();
			doSecurity = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			clientGuid=default(long);
			timestamp=default(long);
			doSecurity=default(byte);
		}

	}

	public partial class ConnectionRequestAccepted : Package<ConnectionRequestAccepted>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			systemAddress=default(IPEndPoint);
			systemIndex=default(short);
			systemAddresses=default(IPEndPoint[]);
			incomingTimestamp=default(long);
			serverTimestamp=default(long);
		}

	}

	public partial class NewIncomingConnection : Package<NewIncomingConnection>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(clientendpoint);
			Write(systemAddresses);
			Write(incomingTimestamp);
			Write(serverTimestamp);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			clientendpoint = ReadIPEndPoint();
			systemAddresses = ReadIPEndPoints(20);
			incomingTimestamp = ReadLong();
			serverTimestamp = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			clientendpoint=default(IPEndPoint);
			systemAddresses=default(IPEndPoint[]);
			incomingTimestamp=default(long);
			serverTimestamp=default(long);
		}

	}

	public partial class NoFreeIncomingConnections : Package<NoFreeIncomingConnections>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;

		public NoFreeIncomingConnections()
		{
			Id = 0x14;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(serverGuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			serverGuid=default(long);
		}

	}

	public partial class DisconnectionNotification : Package<DisconnectionNotification>
	{


		public DisconnectionNotification()
		{
			Id = 0x15;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class ConnectionBanned : Package<ConnectionBanned>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;

		public ConnectionBanned()
		{
			Id = 0x17;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(serverGuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			serverGuid=default(long);
		}

	}

	public partial class IpRecentlyConnected : Package<IpRecentlyConnected>
	{

		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };

		public IpRecentlyConnected()
		{
			Id = 0x1a;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(offlineMessageDataId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeLogin : Package<McpeLogin>
	{

		public int protocolVersion; // = null;
		public byte[] payload; // = null;

		public McpeLogin()
		{
			Id = 0x01;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteBe(protocolVersion);
			WriteByteArray(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			protocolVersion = ReadIntBe();
			payload = ReadByteArray();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			protocolVersion=default(int);
			payload=default(byte[]);
		}

	}

	public partial class McpePlayStatus : Package<McpePlayStatus>
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
		}

		public int status; // = null;

		public McpePlayStatus()
		{
			Id = 0x02;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteBe(status);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			status = ReadIntBe();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			status=default(int);
		}

	}

	public partial class McpeServerToClientHandshake : Package<McpeServerToClientHandshake>
	{

		public string token; // = null;

		public McpeServerToClientHandshake()
		{
			Id = 0x03;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(token);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			token = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			token=default(string);
		}

	}

	public partial class McpeClientToServerHandshake : Package<McpeClientToServerHandshake>
	{


		public McpeClientToServerHandshake()
		{
			Id = 0x04;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeDisconnect : Package<McpeDisconnect>
	{

		public bool hideDisconnectReason; // = null;
		public string message; // = null;

		public McpeDisconnect()
		{
			Id = 0x05;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(hideDisconnectReason);
			Write(message);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			hideDisconnectReason = ReadBool();
			message = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			hideDisconnectReason=default(bool);
			message=default(string);
		}

	}

	public partial class McpeResourcePacksInfo : Package<McpeResourcePacksInfo>
	{

		public bool mustAccept; // = null;
		public ResourcePackInfos behahaviorpackinfos; // = null;
		public ResourcePackInfos resourcepackinfos; // = null;

		public McpeResourcePacksInfo()
		{
			Id = 0x06;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(mustAccept);
			Write(behahaviorpackinfos);
			Write(resourcepackinfos);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			mustAccept = ReadBool();
			behahaviorpackinfos = ReadResourcePackInfos();
			resourcepackinfos = ReadResourcePackInfos();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			mustAccept=default(bool);
			behahaviorpackinfos=default(ResourcePackInfos);
			resourcepackinfos=default(ResourcePackInfos);
		}

	}

	public partial class McpeResourcePackStack : Package<McpeResourcePackStack>
	{

		public bool mustAccept; // = null;
		public ResourcePackIdVersions behaviorpackidversions; // = null;
		public ResourcePackIdVersions resourcepackidversions; // = null;

		public McpeResourcePackStack()
		{
			Id = 0x07;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(mustAccept);
			Write(behaviorpackidversions);
			Write(resourcepackidversions);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			mustAccept = ReadBool();
			behaviorpackidversions = ReadResourcePackIdVersions();
			resourcepackidversions = ReadResourcePackIdVersions();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			mustAccept=default(bool);
			behaviorpackidversions=default(ResourcePackIdVersions);
			resourcepackidversions=default(ResourcePackIdVersions);
		}

	}

	public partial class McpeResourcePackClientResponse : Package<McpeResourcePackClientResponse>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(responseStatus);
			Write(resourcepackids);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			responseStatus = ReadByte();
			resourcepackids = ReadResourcePackIds();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			responseStatus=default(byte);
			resourcepackids=default(ResourcePackIds);
		}

	}

	public partial class McpeText : Package<McpeText>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(type);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			type = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			type=default(byte);
		}

	}

	public partial class McpeSetTime : Package<McpeSetTime>
	{

		public int time; // = null;

		public McpeSetTime()
		{
			Id = 0x0a;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(time);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			time = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			time=default(int);
		}

	}

	public partial class McpeStartGame : Package<McpeStartGame>
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
		public float rainLevel; // = null;
		public float lightnigLevel; // = null;
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
		public string levelId; // = null;
		public string worldName; // = null;
		public string premiumWorldTemplateId; // = null;
		public bool unknown0; // = null;
		public long currentTick; // = null;
		public int enchantmentSeed; // = null;

		public McpeStartGame()
		{
			Id = 0x0b;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

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
			WriteSignedVarInt(y);
			WriteSignedVarInt(z);
			Write(hasAchievementsDisabled);
			WriteSignedVarInt(dayCycleStopTime);
			Write(eduMode);
			Write(rainLevel);
			Write(lightnigLevel);
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
			Write(levelId);
			Write(worldName);
			Write(premiumWorldTemplateId);
			Write(unknown0);
			Write(currentTick);
			WriteSignedVarInt(enchantmentSeed);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

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
			y = ReadSignedVarInt();
			z = ReadSignedVarInt();
			hasAchievementsDisabled = ReadBool();
			dayCycleStopTime = ReadSignedVarInt();
			eduMode = ReadBool();
			rainLevel = ReadFloat();
			lightnigLevel = ReadFloat();
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
			levelId = ReadString();
			worldName = ReadString();
			premiumWorldTemplateId = ReadString();
			unknown0 = ReadBool();
			currentTick = ReadLong();
			enchantmentSeed = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

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
			rainLevel=default(float);
			lightnigLevel=default(float);
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
			levelId=default(string);
			worldName=default(string);
			premiumWorldTemplateId=default(string);
			unknown0=default(bool);
			currentTick=default(long);
			enchantmentSeed=default(int);
		}

	}

	public partial class McpeAddPlayer : Package<McpeAddPlayer>
	{

		public UUID uuid; // = null;
		public string username; // = null;
		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
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
		public uint userPermission; // = null;
		public uint actionPermissions; // = null;
		public uint permissionLevel; // = null;
		public uint unknown; // = null;
		public long userId; // = null;
		public Links links; // = null;

		public McpeAddPlayer()
		{
			Id = 0x0c;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(uuid);
			Write(username);
			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
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
			WriteUnsignedVarInt(userPermission);
			WriteUnsignedVarInt(actionPermissions);
			WriteUnsignedVarInt(permissionLevel);
			WriteUnsignedVarInt(unknown);
			Write(userId);
			Write(links);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			uuid = ReadUUID();
			username = ReadString();
			entityIdSelf = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
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
			userPermission = ReadUnsignedVarInt();
			actionPermissions = ReadUnsignedVarInt();
			permissionLevel = ReadUnsignedVarInt();
			unknown = ReadUnsignedVarInt();
			userId = ReadLong();
			links = ReadLinks();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			uuid=default(UUID);
			username=default(string);
			entityIdSelf=default(long);
			runtimeEntityId=default(long);
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
			userPermission=default(uint);
			actionPermissions=default(uint);
			permissionLevel=default(uint);
			unknown=default(uint);
			userId=default(long);
			links=default(Links);
		}

	}

	public partial class McpeAddEntity : Package<McpeAddEntity>
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
		public EntityAttributes attributes; // = null;
		public MetadataDictionary metadata; // = null;
		public Links links; // = null;

		public McpeAddEntity()
		{
			Id = 0x0d;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

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
			Write(attributes);
			Write(metadata);
			Write(links);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

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
			attributes = ReadEntityAttributes();
			metadata = ReadMetadataDictionary();
			links = ReadLinks();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

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
			attributes=default(EntityAttributes);
			metadata=default(MetadataDictionary);
			links=default(Links);
		}

	}

	public partial class McpeRemoveEntity : Package<McpeRemoveEntity>
	{

		public long entityIdSelf; // = null;

		public McpeRemoveEntity()
		{
			Id = 0x0e;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			entityIdSelf=default(long);
		}

	}

	public partial class McpeAddItemEntity : Package<McpeAddItemEntity>
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

		public McpeAddItemEntity()
		{
			Id = 0x0f;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

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
		}

	}

	public partial class McpeTakeItemEntity : Package<McpeTakeItemEntity>
	{

		public long runtimeEntityId; // = null;
		public long target; // = null;

		public McpeTakeItemEntity()
		{
			Id = 0x11;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteUnsignedVarLong(target);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			target = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			target=default(long);
		}

	}

	public partial class McpeMoveEntity : Package<McpeMoveEntity>
	{

		public long runtimeEntityId; // = null;
		public PlayerLocation position; // = null;
		public bool onGround; // = null;
		public bool teleport; // = null;

		public McpeMoveEntity()
		{
			Id = 0x12;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(position);
			Write(onGround);
			Write(teleport);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			position = ReadPlayerLocation();
			onGround = ReadBool();
			teleport = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			position=default(PlayerLocation);
			onGround=default(bool);
			teleport=default(bool);
		}

	}

	public partial class McpeMovePlayer : Package<McpeMovePlayer>
	{
		public enum Mode
		{
			Normal = 0,
			Reset = 1,
			Rotation = 2,
			Pitch = 3,
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

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

	public partial class McpeRiderJump : Package<McpeRiderJump>
	{

		public int unknown; // = null;

		public McpeRiderJump()
		{
			Id = 0x14;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			unknown = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			unknown=default(int);
		}

	}

	public partial class McpeUpdateBlock : Package<McpeUpdateBlock>
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
		public uint blockId; // = null;
		public uint blockMetaAndPriority; // = null;

		public McpeUpdateBlock()
		{
			Id = 0x15;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(coordinates);
			WriteUnsignedVarInt(blockId);
			WriteUnsignedVarInt(blockMetaAndPriority);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			blockId = ReadUnsignedVarInt();
			blockMetaAndPriority = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			coordinates=default(BlockCoordinates);
			blockId=default(uint);
			blockMetaAndPriority=default(uint);
		}

	}

	public partial class McpeAddPainting : Package<McpeAddPainting>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			entityIdSelf=default(long);
			runtimeEntityId=default(long);
			coordinates=default(BlockCoordinates);
			direction=default(int);
			title=default(string);
		}

	}

	public partial class McpeExplode : Package<McpeExplode>
	{

		public Vector3 position; // = null;
		public int radius; // = null;
		public Records records; // = null;

		public McpeExplode()
		{
			Id = 0x17;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(position);
			WriteSignedVarInt(radius);
			Write(records);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			position = ReadVector3();
			radius = ReadSignedVarInt();
			records = ReadRecords();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			position=default(Vector3);
			radius=default(int);
			records=default(Records);
		}

	}

	public partial class McpeLevelSoundEvent : Package<McpeLevelSoundEvent>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			soundId=default(byte);
			position=default(Vector3);
			blockId=default(int);
			entityType=default(int);
			isBabyMob=default(bool);
			isGlobal=default(bool);
		}

	}

	public partial class McpeLevelEvent : Package<McpeLevelEvent>
	{

		public int eventId; // = null;
		public Vector3 position; // = null;
		public int data; // = null;

		public McpeLevelEvent()
		{
			Id = 0x19;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(eventId);
			Write(position);
			WriteSignedVarInt(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			eventId = ReadSignedVarInt();
			position = ReadVector3();
			data = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			eventId=default(int);
			position=default(Vector3);
			data=default(int);
		}

	}

	public partial class McpeBlockEvent : Package<McpeBlockEvent>
	{

		public BlockCoordinates coordinates; // = null;
		public int case1; // = null;
		public int case2; // = null;

		public McpeBlockEvent()
		{
			Id = 0x1a;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(coordinates);
			WriteSignedVarInt(case1);
			WriteSignedVarInt(case2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			case1 = ReadSignedVarInt();
			case2 = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			coordinates=default(BlockCoordinates);
			case1=default(int);
			case2=default(int);
		}

	}

	public partial class McpeEntityEvent : Package<McpeEntityEvent>
	{

		public long runtimeEntityId; // = null;
		public byte eventId; // = null;
		public int unknown; // = null;

		public McpeEntityEvent()
		{
			Id = 0x1b;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(eventId);
			WriteSignedVarInt(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			eventId = ReadByte();
			unknown = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			eventId=default(byte);
			unknown=default(int);
		}

	}

	public partial class McpeMobEffect : Package<McpeMobEffect>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			eventId=default(byte);
			effectId=default(int);
			amplifier=default(int);
			particles=default(bool);
			duration=default(int);
		}

	}

	public partial class McpeUpdateAttributes : Package<McpeUpdateAttributes>
	{

		public long runtimeEntityId; // = null;
		public PlayerAttributes attributes; // = null;

		public McpeUpdateAttributes()
		{
			Id = 0x1d;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(attributes);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			attributes = ReadPlayerAttributes();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			attributes=default(PlayerAttributes);
		}

	}

	public partial class McpeInventoryTransaction : Package<McpeInventoryTransaction>
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
			EnchantItem = 29,
			EnchantLapis = 31,
			EnchantResult = 33,
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(transaction);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			transaction = ReadTransaction();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			transaction=default(Transaction);
		}

	}

	public partial class McpeMobEquipment : Package<McpeMobEquipment>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			item=default(Item);
			slot=default(byte);
			selectedSlot=default(byte);
			windowsId=default(byte);
		}

	}

	public partial class McpeMobArmorEquipment : Package<McpeMobArmorEquipment>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			helmet=default(Item);
			chestplate=default(Item);
			leggings=default(Item);
			boots=default(Item);
		}

	}

	public partial class McpeInteract : Package<McpeInteract>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(actionId);
			WriteUnsignedVarLong(targetRuntimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadByte();
			targetRuntimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			actionId=default(byte);
			targetRuntimeEntityId=default(long);
		}

	}

	public partial class McpeBlockPickRequest : Package<McpeBlockPickRequest>
	{

		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public byte selectedSlot; // = null;

		public McpeBlockPickRequest()
		{
			Id = 0x22;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(x);
			WriteSignedVarInt(y);
			WriteSignedVarInt(z);
			Write(selectedSlot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			x = ReadSignedVarInt();
			y = ReadSignedVarInt();
			z = ReadSignedVarInt();
			selectedSlot = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			x=default(int);
			y=default(int);
			z=default(int);
			selectedSlot=default(byte);
		}

	}

	public partial class McpeEntityPickRequest : Package<McpeEntityPickRequest>
	{


		public McpeEntityPickRequest()
		{
			Id = 0x23;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpePlayerAction : Package<McpePlayerAction>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(actionId);
			Write(coordinates);
			WriteSignedVarInt(face);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			actionId = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();
			face = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			actionId=default(int);
			coordinates=default(BlockCoordinates);
			face=default(int);
		}

	}

	public partial class McpeEntityFall : Package<McpeEntityFall>
	{

		public long runtimeEntityId; // = null;
		public float fallDistance; // = null;
		public bool unknown; // = null;

		public McpeEntityFall()
		{
			Id = 0x25;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(fallDistance);
			Write(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			fallDistance = ReadFloat();
			unknown = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			fallDistance=default(float);
			unknown=default(bool);
		}

	}

	public partial class McpeHurtArmor : Package<McpeHurtArmor>
	{

		public int health; // = null;

		public McpeHurtArmor()
		{
			Id = 0x26;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			health = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			health=default(int);
		}

	}

	public partial class McpeSetEntityData : Package<McpeSetEntityData>
	{

		public long runtimeEntityId; // = null;
		public MetadataDictionary metadata; // = null;

		public McpeSetEntityData()
		{
			Id = 0x27;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(metadata);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			metadata = ReadMetadataDictionary();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			metadata=default(MetadataDictionary);
		}

	}

	public partial class McpeSetEntityMotion : Package<McpeSetEntityMotion>
	{

		public long runtimeEntityId; // = null;
		public Vector3 velocity; // = null;

		public McpeSetEntityMotion()
		{
			Id = 0x28;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(velocity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			velocity = ReadVector3();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			velocity=default(Vector3);
		}

	}

	public partial class McpeSetEntityLink : Package<McpeSetEntityLink>
	{
		public enum LinkActions
		{
			Remove = 0,
			Ride = 1,
			Passenger = 2,
		}

		public long riderId; // = null;
		public long riddenId; // = null;
		public byte linkType; // = null;
		public byte unknown; // = null;

		public McpeSetEntityLink()
		{
			Id = 0x29;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(riderId);
			WriteUnsignedVarLong(riddenId);
			Write(linkType);
			Write(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			riderId = ReadUnsignedVarLong();
			riddenId = ReadUnsignedVarLong();
			linkType = ReadByte();
			unknown = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			riderId=default(long);
			riddenId=default(long);
			linkType=default(byte);
			unknown=default(byte);
		}

	}

	public partial class McpeSetHealth : Package<McpeSetHealth>
	{

		public int health; // = null;

		public McpeSetHealth()
		{
			Id = 0x2a;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			health = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			health=default(int);
		}

	}

	public partial class McpeSetSpawnPosition : Package<McpeSetSpawnPosition>
	{

		public int spawnType; // = null;
		public BlockCoordinates coordinates; // = null;
		public bool forced; // = null;

		public McpeSetSpawnPosition()
		{
			Id = 0x2b;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(spawnType);
			Write(coordinates);
			Write(forced);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			spawnType = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();
			forced = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			spawnType=default(int);
			coordinates=default(BlockCoordinates);
			forced=default(bool);
		}

	}

	public partial class McpeAnimate : Package<McpeAnimate>
	{

		public int actionId; // = null;
		public long runtimeEntityId; // = null;

		public McpeAnimate()
		{
			Id = 0x2c;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(actionId);
			WriteUnsignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadSignedVarInt();
			runtimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			actionId=default(int);
			runtimeEntityId=default(long);
		}

	}

	public partial class McpeRespawn : Package<McpeRespawn>
	{

		public float x; // = null;
		public float y; // = null;
		public float z; // = null;

		public McpeRespawn()
		{
			Id = 0x2d;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			x=default(float);
			y=default(float);
			z=default(float);
		}

	}

	public partial class McpeContainerOpen : Package<McpeContainerOpen>
	{

		public byte windowId; // = null;
		public byte type; // = null;
		public BlockCoordinates coordinates; // = null;
		public long unknownRuntimeEntityId; // = null;

		public McpeContainerOpen()
		{
			Id = 0x2e;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			Write(type);
			Write(coordinates);
			WriteUnsignedVarLong(unknownRuntimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			windowId = ReadByte();
			type = ReadByte();
			coordinates = ReadBlockCoordinates();
			unknownRuntimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			windowId=default(byte);
			type=default(byte);
			coordinates=default(BlockCoordinates);
			unknownRuntimeEntityId=default(long);
		}

	}

	public partial class McpeContainerClose : Package<McpeContainerClose>
	{

		public byte windowId; // = null;

		public McpeContainerClose()
		{
			Id = 0x2f;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			windowId = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			windowId=default(byte);
		}

	}

	public partial class McpePlayerHotbar : Package<McpePlayerHotbar>
	{

		public uint selectedSlot; // = null;
		public byte windowId; // = null;
		public MetadataInts hotbarData; // = null;
		public byte unknown; // = null;

		public McpePlayerHotbar()
		{
			Id = 0x30;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(selectedSlot);
			Write(windowId);
			Write(hotbarData);
			Write(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			selectedSlot = ReadUnsignedVarInt();
			windowId = ReadByte();
			hotbarData = ReadMetadataInts();
			unknown = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			selectedSlot=default(uint);
			windowId=default(byte);
			hotbarData=default(MetadataInts);
			unknown=default(byte);
		}

	}

	public partial class McpeInventoryContent : Package<McpeInventoryContent>
	{

		public uint inventoryId; // = null;
		public ItemStacks input; // = null;

		public McpeInventoryContent()
		{
			Id = 0x31;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(inventoryId);
			Write(input);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			inventoryId = ReadUnsignedVarInt();
			input = ReadItemStacks();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			inventoryId=default(uint);
			input=default(ItemStacks);
		}

	}

	public partial class McpeInventorySlot : Package<McpeInventorySlot>
	{

		public uint inventoryId; // = null;
		public uint slot; // = null;
		public Item item; // = null;

		public McpeInventorySlot()
		{
			Id = 0x32;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(inventoryId);
			WriteUnsignedVarInt(slot);
			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			inventoryId = ReadUnsignedVarInt();
			slot = ReadUnsignedVarInt();
			item = ReadItem();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			inventoryId=default(uint);
			slot=default(uint);
			item=default(Item);
		}

	}

	public partial class McpeContainerSetData : Package<McpeContainerSetData>
	{

		public byte windowId; // = null;
		public int property; // = null;
		public int value; // = null;

		public McpeContainerSetData()
		{
			Id = 0x33;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			WriteSignedVarInt(property);
			WriteSignedVarInt(value);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			windowId = ReadByte();
			property = ReadSignedVarInt();
			value = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			windowId=default(byte);
			property=default(int);
			value=default(int);
		}

	}

	public partial class McpeCraftingData : Package<McpeCraftingData>
	{

		public Recipes recipes; // = null;

		public McpeCraftingData()
		{
			Id = 0x34;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(recipes);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			recipes = ReadRecipes();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			recipes=default(Recipes);
		}

	}

	public partial class McpeCraftingEvent : Package<McpeCraftingEvent>
	{
		public enum RecipeTypes
		{
			Shapeless = 0,
			Shaped = 1,
			Furnace = 2,
			FurnaceData = 3,
			Multi = 4,
			ShulkerBox = 5,
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			windowId=default(byte);
			recipeType=default(int);
			recipeId=default(UUID);
			input=default(ItemStacks);
			result=default(ItemStacks);
		}

	}

	public partial class McpeGuiDataPickItem : Package<McpeGuiDataPickItem>
	{


		public McpeGuiDataPickItem()
		{
			Id = 0x36;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeAdventureSettings : Package<McpeAdventureSettings>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			flags=default(uint);
			commandPermission=default(uint);
			actionPermissions=default(uint);
			permissionLevel=default(uint);
			customStoredPermissions=default(uint);
			userId=default(long);
		}

	}

	public partial class McpeBlockEntityData : Package<McpeBlockEntityData>
	{

		public BlockCoordinates coordinates; // = null;
		public Nbt namedtag; // = null;

		public McpeBlockEntityData()
		{
			Id = 0x38;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(coordinates);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			coordinates=default(BlockCoordinates);
			namedtag=default(Nbt);
		}

	}

	public partial class McpePlayerInput : Package<McpePlayerInput>
	{

		public float motionX; // = null;
		public float motionZ; // = null;
		public bool flag1; // = null;
		public bool flag2; // = null;

		public McpePlayerInput()
		{
			Id = 0x39;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(motionX);
			Write(motionZ);
			Write(flag1);
			Write(flag2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			motionX = ReadFloat();
			motionZ = ReadFloat();
			flag1 = ReadBool();
			flag2 = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			motionX=default(float);
			motionZ=default(float);
			flag1=default(bool);
			flag2=default(bool);
		}

	}

	public partial class McpeFullChunkData : Package<McpeFullChunkData>
	{

		public int chunkX; // = null;
		public int chunkZ; // = null;
		public byte[] chunkData; // = null;

		public McpeFullChunkData()
		{
			Id = 0x3a;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(chunkX);
			WriteSignedVarInt(chunkZ);
			WriteByteArray(chunkData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			chunkX = ReadSignedVarInt();
			chunkZ = ReadSignedVarInt();
			chunkData = ReadByteArray();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			chunkX=default(int);
			chunkZ=default(int);
			chunkData=default(byte[]);
		}

	}

	public partial class McpeSetCommandsEnabled : Package<McpeSetCommandsEnabled>
	{

		public bool enabled; // = null;

		public McpeSetCommandsEnabled()
		{
			Id = 0x3b;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(enabled);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			enabled = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			enabled=default(bool);
		}

	}

	public partial class McpeSetDifficulty : Package<McpeSetDifficulty>
	{

		public uint difficulty; // = null;

		public McpeSetDifficulty()
		{
			Id = 0x3c;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(difficulty);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			difficulty = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			difficulty=default(uint);
		}

	}

	public partial class McpeChangeDimension : Package<McpeChangeDimension>
	{

		public int dimension; // = null;
		public Vector3 position; // = null;
		public bool unknown; // = null;

		public McpeChangeDimension()
		{
			Id = 0x3d;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(dimension);
			Write(position);
			Write(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			dimension = ReadSignedVarInt();
			position = ReadVector3();
			unknown = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			dimension=default(int);
			position=default(Vector3);
			unknown=default(bool);
		}

	}

	public partial class McpeSetPlayerGameType : Package<McpeSetPlayerGameType>
	{

		public int gamemode; // = null;

		public McpeSetPlayerGameType()
		{
			Id = 0x3e;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(gamemode);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			gamemode = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			gamemode=default(int);
		}

	}

	public partial class McpePlayerList : Package<McpePlayerList>
	{

		public PlayerRecords records; // = null;

		public McpePlayerList()
		{
			Id = 0x3f;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(records);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			records = ReadPlayerRecords();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			records=default(PlayerRecords);
		}

	}

	public partial class McpeSimpleEvent : Package<McpeSimpleEvent>
	{


		public McpeSimpleEvent()
		{
			Id = 0x40;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeTelemetryEvent : Package<McpeTelemetryEvent>
	{

		public long entityIdSelf; // = null;
		public int unk1; // = null;
		public byte unk2; // = null;

		public McpeTelemetryEvent()
		{
			Id = 0x41;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);
			WriteSignedVarInt(unk1);
			Write(unk2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();
			unk1 = ReadSignedVarInt();
			unk2 = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			entityIdSelf=default(long);
			unk1=default(int);
			unk2=default(byte);
		}

	}

	public partial class McpeSpawnExperienceOrb : Package<McpeSpawnExperienceOrb>
	{

		public Vector3 position; // = null;
		public int count; // = null;

		public McpeSpawnExperienceOrb()
		{
			Id = 0x42;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(position);
			WriteSignedVarInt(count);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			position = ReadVector3();
			count = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			position=default(Vector3);
			count=default(int);
		}

	}

	public partial class McpeClientboundMapItemData : Package<McpeClientboundMapItemData>
	{

		public MapInfo mapinfo; // = null;

		public McpeClientboundMapItemData()
		{
			Id = 0x43;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(mapinfo);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			mapinfo = ReadMapInfo();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			mapinfo=default(MapInfo);
		}

	}

	public partial class McpeMapInfoRequest : Package<McpeMapInfoRequest>
	{

		public long mapId; // = null;

		public McpeMapInfoRequest()
		{
			Id = 0x44;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(mapId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			mapId = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			mapId=default(long);
		}

	}

	public partial class McpeRequestChunkRadius : Package<McpeRequestChunkRadius>
	{

		public int chunkRadius; // = null;

		public McpeRequestChunkRadius()
		{
			Id = 0x45;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(chunkRadius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			chunkRadius = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			chunkRadius=default(int);
		}

	}

	public partial class McpeChunkRadiusUpdate : Package<McpeChunkRadiusUpdate>
	{

		public int chunkRadius; // = null;

		public McpeChunkRadiusUpdate()
		{
			Id = 0x46;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(chunkRadius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			chunkRadius = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			chunkRadius=default(int);
		}

	}

	public partial class McpeItemFrameDropItem : Package<McpeItemFrameDropItem>
	{

		public BlockCoordinates coordinates; // = null;

		public McpeItemFrameDropItem()
		{
			Id = 0x47;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(coordinates);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			coordinates=default(BlockCoordinates);
		}

	}

	public partial class McpeGameRulesChanged : Package<McpeGameRulesChanged>
	{

		public GameRules rules; // = null;

		public McpeGameRulesChanged()
		{
			Id = 0x48;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(rules);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			rules = ReadGameRules();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			rules=default(GameRules);
		}

	}

	public partial class McpeCamera : Package<McpeCamera>
	{


		public McpeCamera()
		{
			Id = 0x49;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeBossEvent : Package<McpeBossEvent>
	{

		public long bossEntityId; // = null;
		public uint eventType; // = null;

		public McpeBossEvent()
		{
			Id = 0x4a;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(bossEntityId);
			WriteUnsignedVarInt(eventType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			bossEntityId = ReadSignedVarLong();
			eventType = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			bossEntityId=default(long);
			eventType=default(uint);
		}

	}

	public partial class McpeShowCredits : Package<McpeShowCredits>
	{

		public long runtimeEntityId; // = null;
		public int status; // = null;

		public McpeShowCredits()
		{
			Id = 0x4b;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(status);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			status = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			status=default(int);
		}

	}

	public partial class McpeAvailableCommands : Package<McpeAvailableCommands>
	{


		public McpeAvailableCommands()
		{
			Id = 0x4c;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeCommandRequest : Package<McpeCommandRequest>
	{

		public string command; // = null;
		public int commandType; // = null;
		public string requestId; // = null;
		public bool unknown; // = null;

		public McpeCommandRequest()
		{
			Id = 0x4d;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(command);
			WriteSignedVarInt(commandType);
			Write(requestId);
			Write(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			command = ReadString();
			commandType = ReadSignedVarInt();
			requestId = ReadString();
			unknown = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			command=default(string);
			commandType=default(int);
			requestId=default(string);
			unknown=default(bool);
		}

	}

	public partial class McpeCommandBlockUpdate : Package<McpeCommandBlockUpdate>
	{


		public McpeCommandBlockUpdate()
		{
			Id = 0x4e;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeUpdateTrade : Package<McpeUpdateTrade>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

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

	public partial class McpeUpdateEquipment : Package<McpeUpdateEquipment>
	{


		public McpeUpdateEquipment()
		{
			Id = 0x51;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeResourcePackDataInfo : Package<McpeResourcePackDataInfo>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			packageId=default(string);
			maxChunkSize=default(uint);
			chunkCount=default(uint);
			compressedPackageSize=default(ulong);
			hash=default(string);
		}

	}

	public partial class McpeResourcePackChunkData : Package<McpeResourcePackChunkData>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			packageId=default(string);
			chunkIndex=default(uint);
			progress=default(ulong);
			length=default(uint);
			payload=default(byte[]);
		}

	}

	public partial class McpeResourcePackChunkRequest : Package<McpeResourcePackChunkRequest>
	{

		public string packageId; // = null;
		public uint chunkIndex; // = null;

		public McpeResourcePackChunkRequest()
		{
			Id = 0x54;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(packageId);
			Write(chunkIndex);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			packageId = ReadString();
			chunkIndex = ReadUint();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			packageId=default(string);
			chunkIndex=default(uint);
		}

	}

	public partial class McpeTransfer : Package<McpeTransfer>
	{

		public string serverAddress; // = null;
		public ushort port; // = null;

		public McpeTransfer()
		{
			Id = 0x55;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(serverAddress);
			Write(port);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			serverAddress = ReadString();
			port = ReadUshort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			serverAddress=default(string);
			port=default(ushort);
		}

	}

	public partial class McpePlaySound : Package<McpePlaySound>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(name);
			Write(coordinates);
			Write(volume);
			Write(pitch);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			name = ReadString();
			coordinates = ReadBlockCoordinates();
			volume = ReadFloat();
			pitch = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			name=default(string);
			coordinates=default(BlockCoordinates);
			volume=default(float);
			pitch=default(float);
		}

	}

	public partial class McpeStopSound : Package<McpeStopSound>
	{

		public string name; // = null;
		public bool stopAll; // = null;

		public McpeStopSound()
		{
			Id = 0x57;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(name);
			Write(stopAll);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			name = ReadString();
			stopAll = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			name=default(string);
			stopAll=default(bool);
		}

	}

	public partial class McpeSetTitle : Package<McpeSetTitle>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			type=default(int);
			text=default(string);
			fadeInTime=default(int);
			stayTime=default(int);
			fadeOutTime=default(int);
		}

	}

	public partial class McpeAddBehaviorTree : Package<McpeAddBehaviorTree>
	{

		public string behaviortree; // = null;

		public McpeAddBehaviorTree()
		{
			Id = 0x59;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(behaviortree);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			behaviortree = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			behaviortree=default(string);
		}

	}

	public partial class McpeStructureBlockUpdate : Package<McpeStructureBlockUpdate>
	{


		public McpeStructureBlockUpdate()
		{
			Id = 0x5a;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeShowStoreOffer : Package<McpeShowStoreOffer>
	{

		public string unknown0; // = null;
		public bool unknown1; // = null;

		public McpeShowStoreOffer()
		{
			Id = 0x5b;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(unknown0);
			Write(unknown1);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			unknown0 = ReadString();
			unknown1 = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			unknown0=default(string);
			unknown1=default(bool);
		}

	}

	public partial class McpePurchaseReceipt : Package<McpePurchaseReceipt>
	{


		public McpePurchaseReceipt()
		{
			Id = 0x5c;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpePlayerSkin : Package<McpePlayerSkin>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

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

	public partial class McpeSubClientLogin : Package<McpeSubClientLogin>
	{


		public McpeSubClientLogin()
		{
			Id = 0x5e;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeInitiateWebSocketConnection : Package<McpeInitiateWebSocketConnection>
	{

		public string server; // = null;

		public McpeInitiateWebSocketConnection()
		{
			Id = 0x5f;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(server);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			server = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			server=default(string);
		}

	}

	public partial class McpeSetLastHurtBy : Package<McpeSetLastHurtBy>
	{

		public int unknown; // = null;

		public McpeSetLastHurtBy()
		{
			Id = 0x60;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			unknown = ReadVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			unknown=default(int);
		}

	}

	public partial class McpeBookEdit : Package<McpeBookEdit>
	{


		public McpeBookEdit()
		{
			Id = 0x61;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeNpcRequest : Package<McpeNpcRequest>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(unknown0);
			Write(unknown1);
			Write(unknown2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			unknown0 = ReadByte();
			unknown1 = ReadString();
			unknown2 = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			runtimeEntityId=default(long);
			unknown0=default(byte);
			unknown1=default(string);
			unknown2=default(byte);
		}

	}

	public partial class McpeModalFormRequest : Package<McpeModalFormRequest>
	{

		public uint formId; // = null;
		public string data; // = null;

		public McpeModalFormRequest()
		{
			Id = 0x64;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(formId);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			formId = ReadUnsignedVarInt();
			data = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			formId=default(uint);
			data=default(string);
		}

	}

	public partial class McpeModalFormResponse : Package<McpeModalFormResponse>
	{

		public uint formId; // = null;
		public string data; // = null;

		public McpeModalFormResponse()
		{
			Id = 0x65;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(formId);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			formId = ReadUnsignedVarInt();
			data = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			formId=default(uint);
			data=default(string);
		}

	}

	public partial class McpeServerSettingsRequest : Package<McpeServerSettingsRequest>
	{


		public McpeServerSettingsRequest()
		{
			Id = 0x66;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

		}

	}

	public partial class McpeServerSettingsResponse : Package<McpeServerSettingsResponse>
	{

		public long formId; // = null;
		public string data; // = null;

		public McpeServerSettingsResponse()
		{
			Id = 0x67;
			IsMcpe = true;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(formId);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			formId = ReadUnsignedVarLong();
			data = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			formId=default(long);
			data=default(string);
		}

	}

	public partial class McpeWrapper : Package<McpeWrapper>
	{

		public byte[] payload; // = null;

		public McpeWrapper()
		{
			Id = 0xfe;
			IsMcpe = false;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			payload = ReadBytes(0, true);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPackage()
		{
			base.ResetPackage();

			payload=default(byte[]);
		}

	}

	public partial class FtlCreatePlayer : Package<FtlCreatePlayer>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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

		protected override void DecodePackage()
		{
			base.DecodePackage();

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

		protected override void ResetPackage()
		{
			base.ResetPackage();

			username=default(string);
			clientuuid=default(UUID);
			serverAddress=default(string);
			clientId=default(long);
			skin=default(Skin);
		}

	}

}

