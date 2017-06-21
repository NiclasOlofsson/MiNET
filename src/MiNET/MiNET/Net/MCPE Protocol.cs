
//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

using System;
using System.Net;
using System.Numerics;
using System.Threading;
using MiNET.Utils; 
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
		void HandleMcpeRemoveBlock(McpeRemoveBlock message);
		void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
		void HandleMcpeEntityEvent(McpeEntityEvent message);
		void HandleMcpeMobEquipment(McpeMobEquipment message);
		void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
		void HandleMcpeInteract(McpeInteract message);
		void HandleMcpeBlockPickRequest(McpeBlockPickRequest message);
		void HandleMcpeUseItem(McpeUseItem message);
		void HandleMcpePlayerAction(McpePlayerAction message);
		void HandleMcpeEntityFall(McpeEntityFall message);
		void HandleMcpeAnimate(McpeAnimate message);
		void HandleMcpeRespawn(McpeRespawn message);
		void HandleMcpeDropItem(McpeDropItem message);
		void HandleMcpeContainerClose(McpeContainerClose message);
		void HandleMcpeContainerSetSlot(McpeContainerSetSlot message);
		void HandleMcpeCraftingEvent(McpeCraftingEvent message);
		void HandleMcpeAdventureSettings(McpeAdventureSettings message);
		void HandleMcpeBlockEntityData(McpeBlockEntityData message);
		void HandleMcpePlayerInput(McpePlayerInput message);
		void HandleMcpeMapInfoRequest(McpeMapInfoRequest message);
		void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message);
		void HandleMcpeItemFrameDropItem(McpeItemFrameDropItem message);
		void HandleMcpeCommandStep(McpeCommandStep message);
		void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message);
		void HandleMcpeResourcePackChunkRequest(McpeResourcePackChunkRequest message);
	}

	public class PackageFactory
	{
		public static Package CreatePackage(byte messageId, byte[] buffer, string ns)
		{
			Package package = null; 
			if(ns == "raknet") {
				switch (messageId)
				{
					case 0x00:
						package = ConnectedPing.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x01:
						package = UnconnectedPing.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x03:
						package = ConnectedPong.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x04:
						package = DetectLostConnections.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1c:
						package = UnconnectedPong.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x05:
						package = OpenConnectionRequest1.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x06:
						package = OpenConnectionReply1.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x07:
						package = OpenConnectionRequest2.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x08:
						package = OpenConnectionReply2.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x09:
						package = ConnectionRequest.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x10:
						package = ConnectionRequestAccepted.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x13:
						package = NewIncomingConnection.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x14:
						package = NoFreeIncomingConnections.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x15:
						package = DisconnectionNotification.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x17:
						package = ConnectionBanned.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1A:
						package = IpRecentlyConnected.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0xfe:
						package = McpeWrapper.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
				}
			} else if(ns == "ftl") {
				switch (messageId)
				{
					case 0x01:
						package = FtlCreatePlayer.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
				}
			} else {

				switch (messageId)
				{
					case 0x01:
						package = McpeLogin.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x02:
						package = McpePlayStatus.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x03:
						package = McpeServerToClientHandshake.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x04:
						package = McpeClientToServerHandshake.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x05:
						package = McpeDisconnect.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x06:
						package = McpeResourcePacksInfo.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x07:
						package = McpeResourcePackStack.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x08:
						package = McpeResourcePackClientResponse.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x09:
						package = McpeText.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0a:
						package = McpeSetTime.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0b:
						package = McpeStartGame.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0c:
						package = McpeAddPlayer.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0d:
						package = McpeAddEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0e:
						package = McpeRemoveEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0f:
						package = McpeAddItemEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x10:
						package = McpeAddHangingEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x11:
						package = McpeTakeItemEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x12:
						package = McpeMoveEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x13:
						package = McpeMovePlayer.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x14:
						package = McpeRiderJump.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x15:
						package = McpeRemoveBlock.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x16:
						package = McpeUpdateBlock.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x17:
						package = McpeAddPainting.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x18:
						package = McpeExplode.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x19:
						package = McpeLevelSoundEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1a:
						package = McpeLevelEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1b:
						package = McpeBlockEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1c:
						package = McpeEntityEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1d:
						package = McpeMobEffect.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1e:
						package = McpeUpdateAttributes.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1f:
						package = McpeMobEquipment.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x20:
						package = McpeMobArmorEquipment.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x21:
						package = McpeInteract.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x22:
						package = McpeBlockPickRequest.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x23:
						package = McpeUseItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x24:
						package = McpePlayerAction.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x25:
						package = McpeEntityFall.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x26:
						package = McpeHurtArmor.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x27:
						package = McpeSetEntityData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x28:
						package = McpeSetEntityMotion.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x29:
						package = McpeSetEntityLink.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2a:
						package = McpeSetHealth.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2b:
						package = McpeSetSpawnPosition.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2c:
						package = McpeAnimate.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2d:
						package = McpeRespawn.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2e:
						package = McpeDropItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2f:
						package = McpeInventoryAction.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x30:
						package = McpeContainerOpen.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x31:
						package = McpeContainerClose.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x32:
						package = McpeContainerSetSlot.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x33:
						package = McpeContainerSetData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x34:
						package = McpeContainerSetContent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x35:
						package = McpeCraftingData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x36:
						package = McpeCraftingEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x37:
						package = McpeAdventureSettings.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x38:
						package = McpeBlockEntityData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x39:
						package = McpePlayerInput.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3a:
						package = McpeFullChunkData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3b:
						package = McpeSetCommandsEnabled.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3c:
						package = McpeSetDifficulty.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3d:
						package = McpeChangeDimension.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3e:
						package = McpeSetPlayerGameType.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3f:
						package = McpePlayerList.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x40:
						package = McpeSimpleEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x41:
						package = McpeEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x42:
						package = McpeSpawnExperienceOrb.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x43:
						package = McpeClientboundMapItemData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x44:
						package = McpeMapInfoRequest.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x45:
						package = McpeRequestChunkRadius.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x46:
						package = McpeChunkRadiusUpdate.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x47:
						package = McpeItemFrameDropItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x48:
						package = McpeReplaceSelectedItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x49:
						package = McpeGameRulesChanged.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4a:
						package = McpeCamera.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4b:
						package = McpeAddItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4c:
						package = McpeBossEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4d:
						package = McpeShowCredits.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4e:
						package = McpeAvailableCommands.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4f:
						package = McpeCommandStep.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x50:
						package = McpeCommandBlockUpdate.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x51:
						package = McpeUpdateTrade.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x52:
						package = McpeUpdateEquip.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x53:
						package = McpeResourcePackDataInfo.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x54:
						package = McpeResourcePackChunkData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x55:
						package = McpeResourcePackChunkRequest.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x56:
						package = McpeTransfer.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x57:
						package = McpePlaySound.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x58:
						package = McpeStopSound.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x59:
						package = McpeSetTitle.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x5a:
						package = McpeAddBehaviorTreePacket.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x5b:
						package = McpeStructureBlockUpdatePacket.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
				}
			}

			return null;
		}
	}

	public partial class ConnectedPing : Package<ConnectedPing>
	{
		public long sendpingtime; // = null;
		public ConnectedPing()
		{
			Id = 0x00;
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

	}

	public partial class UnconnectedPing : Package<UnconnectedPing>
	{
		public long pingId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long guid; // = null;
		public UnconnectedPing()
		{
			Id = 0x01;
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

	}

	public partial class ConnectedPong : Package<ConnectedPong>
	{
		public long sendpingtime; // = null;
		public long sendpongtime; // = null;
		public ConnectedPong()
		{
			Id = 0x03;
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

	}

	public partial class DetectLostConnections : Package<DetectLostConnections>
	{
		public DetectLostConnections()
		{
			Id = 0x04;
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

	}

	public partial class OpenConnectionRequest1 : Package<OpenConnectionRequest1>
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public byte raknetProtocolVersion; // = null;
		public OpenConnectionRequest1()
		{
			Id = 0x05;
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
			doSecurityAndHandshake = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class ConnectionRequest : Package<ConnectionRequest>
	{
		public long clientGuid; // = null;
		public long timestamp; // = null;
		public byte doSecurity; // = null;
		public ConnectionRequest()
		{
			Id = 0x09;
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

	}

	public partial class NoFreeIncomingConnections : Package<NoFreeIncomingConnections>
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;
		public NoFreeIncomingConnections()
		{
			Id = 0x14;
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

	}

	public partial class DisconnectionNotification : Package<DisconnectionNotification>
	{
		public DisconnectionNotification()
		{
			Id = 0x15;
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

	}

	public partial class ConnectionBanned : Package<ConnectionBanned>
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;
		public ConnectionBanned()
		{
			Id = 0x17;
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

	}

	public partial class IpRecentlyConnected : Package<IpRecentlyConnected>
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public IpRecentlyConnected()
		{
			Id = 0x1a;
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

	}

	public partial class McpeLogin : Package<McpeLogin>
	{
		public int protocolVersion; // = null;
		public byte edition; // = null;
		public byte[] payload; // = null;
		public McpeLogin()
		{
			Id = 0x01;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteBe(protocolVersion);
			Write(edition);
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
			edition = ReadByte();
			payload = ReadByteArray();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpePlayStatus : Package<McpePlayStatus>
	{
		public int status; // = null;
		public McpePlayStatus()
		{
			Id = 0x02;
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

	}

	public partial class McpeServerToClientHandshake : Package<McpeServerToClientHandshake>
	{
		public string serverPublicKey; // = null;
		public int tokenLength; // = null;
		public byte[] token; // = null;
		public McpeServerToClientHandshake()
		{
			Id = 0x03;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(serverPublicKey);
			WriteLength(tokenLength);
			Write(token);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			serverPublicKey = ReadString();
			tokenLength = ReadLength();
			token = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeClientToServerHandshake : Package<McpeClientToServerHandshake>
	{
		public McpeClientToServerHandshake()
		{
			Id = 0x04;
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

	}

	public partial class McpeDisconnect : Package<McpeDisconnect>
	{
		public bool hideDisconnectReason; // = null;
		public string message; // = null;
		public McpeDisconnect()
		{
			Id = 0x05;
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

	}

	public partial class McpeResourcePacksInfo : Package<McpeResourcePacksInfo>
	{
		public bool mustAccept; // = null;
		public ResourcePackInfos behahaviorpackinfos; // = null;
		public ResourcePackInfos resourcepackinfos; // = null;
		public McpeResourcePacksInfo()
		{
			Id = 0x06;
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

	}

	public partial class McpeResourcePackStack : Package<McpeResourcePackStack>
	{
		public bool mustAccept; // = null;
		public ResourcePackIdVersions behaviorpackidversions; // = null;
		public ResourcePackIdVersions resourcepackidversions; // = null;
		public McpeResourcePackStack()
		{
			Id = 0x07;
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

	}

	public partial class McpeResourcePackClientResponse : Package<McpeResourcePackClientResponse>
	{
		public byte responseStatus; // = null;
		public ResourcePackIds resourcepackids; // = null;
		public McpeResourcePackClientResponse()
		{
			Id = 0x08;
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

	}

	public partial class McpeText : Package<McpeText>
	{
		public byte type; // = null;
		public McpeText()
		{
			Id = 0x09;
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

	}

	public partial class McpeSetTime : Package<McpeSetTime>
	{
		public int time; // = null;
		public McpeSetTime()
		{
			Id = 0x0a;
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
		public bool enableCommands; // = null;
		public bool isTexturepacksRequired; // = null;
		public GameRules gamerules; // = null;
		public string levelId; // = null;
		public string worldName; // = null;
		public string premiumWorldTemplateId; // = null;
		public bool unknown0; // = null;
		public long currentTick; // = null;
		public McpeStartGame()
		{
			Id = 0x0b;
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
			Write(enableCommands);
			Write(isTexturepacksRequired);
			Write(gamerules);
			Write(levelId);
			Write(worldName);
			Write(premiumWorldTemplateId);
			Write(unknown0);
			Write(currentTick);

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
			enableCommands = ReadBool();
			isTexturepacksRequired = ReadBool();
			gamerules = ReadGameRules();
			levelId = ReadString();
			worldName = ReadString();
			premiumWorldTemplateId = ReadString();
			unknown0 = ReadBool();
			currentTick = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

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
		public float headYaw; // = null;
		public float yaw; // = null;
		public Item item; // = null;
		public MetadataDictionary metadata; // = null;
		public McpeAddPlayer()
		{
			Id = 0x0c;
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
			Write(headYaw);
			Write(yaw);
			Write(item);
			Write(metadata);

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
			headYaw = ReadFloat();
			yaw = ReadFloat();
			item = ReadItem();
			metadata = ReadMetadataDictionary();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

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

	}

	public partial class McpeRemoveEntity : Package<McpeRemoveEntity>
	{
		public long entityIdSelf; // = null;
		public McpeRemoveEntity()
		{
			Id = 0x0e;
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

	}

	public partial class McpeAddHangingEntity : Package<McpeAddHangingEntity>
	{
		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public BlockCoordinates coordinates; // = null;
		public int unknown; // = null;
		public McpeAddHangingEntity()
		{
			Id = 0x10;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
			Write(coordinates);
			WriteSignedVarInt(unknown);

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
			unknown = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeTakeItemEntity : Package<McpeTakeItemEntity>
	{
		public long runtimeEntityId; // = null;
		public long target; // = null;
		public McpeTakeItemEntity()
		{
			Id = 0x11;
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

	}

	public partial class McpeMovePlayer : Package<McpeMovePlayer>
	{
		public long runtimeEntityId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float pitch; // = null;
		public float headYaw; // = null;
		public float yaw; // = null;
		public byte mode; // = null;
		public bool onGround; // = null;
		public long otherRuntimeEntityId; // = null;
		public McpeMovePlayer()
		{
			Id = 0x13;
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
			Write(headYaw);
			Write(yaw);
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
			headYaw = ReadFloat();
			yaw = ReadFloat();
			mode = ReadByte();
			onGround = ReadBool();
			otherRuntimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeRiderJump : Package<McpeRiderJump>
	{
		public int unknown; // = null;
		public McpeRiderJump()
		{
			Id = 0x14;
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

	}

	public partial class McpeRemoveBlock : Package<McpeRemoveBlock>
	{
		public BlockCoordinates coordinates; // = null;
		public McpeRemoveBlock()
		{
			Id = 0x15;
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

	}

	public partial class McpeUpdateBlock : Package<McpeUpdateBlock>
	{
		public BlockCoordinates coordinates; // = null;
		public uint blockId; // = null;
		public uint blockMetaAndPriority; // = null;
		public McpeUpdateBlock()
		{
			Id = 0x16;
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
			Id = 0x17;
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

	}

	public partial class McpeExplode : Package<McpeExplode>
	{
		public Vector3 position; // = null;
		public int radius; // = null;
		public Records records; // = null;
		public McpeExplode()
		{
			Id = 0x18;
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

	}

	public partial class McpeLevelSoundEvent : Package<McpeLevelSoundEvent>
	{
		public byte soundId; // = null;
		public Vector3 position; // = null;
		public int extraData; // = null;
		public int pitch; // = null;
		public bool unknown1; // = null;
		public bool disableRelativeVolume; // = null;
		public McpeLevelSoundEvent()
		{
			Id = 0x19;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(soundId);
			Write(position);
			WriteSignedVarInt(extraData);
			WriteSignedVarInt(pitch);
			Write(unknown1);
			Write(disableRelativeVolume);

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
			extraData = ReadSignedVarInt();
			pitch = ReadSignedVarInt();
			unknown1 = ReadBool();
			disableRelativeVolume = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeLevelEvent : Package<McpeLevelEvent>
	{
		public int eventId; // = null;
		public Vector3 position; // = null;
		public int data; // = null;
		public McpeLevelEvent()
		{
			Id = 0x1a;
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

	}

	public partial class McpeBlockEvent : Package<McpeBlockEvent>
	{
		public BlockCoordinates coordinates; // = null;
		public int case1; // = null;
		public int case2; // = null;
		public McpeBlockEvent()
		{
			Id = 0x1b;
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

	}

	public partial class McpeEntityEvent : Package<McpeEntityEvent>
	{
		public long runtimeEntityId; // = null;
		public byte eventId; // = null;
		public int unknown; // = null;
		public McpeEntityEvent()
		{
			Id = 0x1c;
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
			Id = 0x1d;
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

	}

	public partial class McpeUpdateAttributes : Package<McpeUpdateAttributes>
	{
		public long runtimeEntityId; // = null;
		public PlayerAttributes attributes; // = null;
		public McpeUpdateAttributes()
		{
			Id = 0x1e;
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

	}

	public partial class McpeMobEquipment : Package<McpeMobEquipment>
	{
		public long runtimeEntityId; // = null;
		public Item item; // = null;
		public byte slot; // = null;
		public byte selectedSlot; // = null;
		public byte unknown; // = null;
		public McpeMobEquipment()
		{
			Id = 0x1f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(item);
			Write(slot);
			Write(selectedSlot);
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
			item = ReadItem();
			slot = ReadByte();
			selectedSlot = ReadByte();
			unknown = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

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

	}

	public partial class McpeInteract : Package<McpeInteract>
	{
		public byte actionId; // = null;
		public long targetRuntimeEntityId; // = null;
		public McpeInteract()
		{
			Id = 0x21;
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

	}

	public partial class McpeUseItem : Package<McpeUseItem>
	{
		public BlockCoordinates blockcoordinates; // = null;
		public uint blockId; // = null;
		public int face; // = null;
		public Vector3 facecoordinates; // = null;
		public Vector3 playerposition; // = null;
		public int slot; // = null;
		public Item item; // = null;
		public McpeUseItem()
		{
			Id = 0x23;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(blockcoordinates);
			WriteUnsignedVarInt(blockId);
			WriteSignedVarInt(face);
			Write(facecoordinates);
			Write(playerposition);
			WriteSignedVarInt(slot);
			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			blockcoordinates = ReadBlockCoordinates();
			blockId = ReadUnsignedVarInt();
			face = ReadSignedVarInt();
			facecoordinates = ReadVector3();
			playerposition = ReadVector3();
			slot = ReadSignedVarInt();
			item = ReadItem();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

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

	}

	public partial class McpeEntityFall : Package<McpeEntityFall>
	{
		public long runtimeEntityId; // = null;
		public float fallDistance; // = null;
		public bool unknown; // = null;
		public McpeEntityFall()
		{
			Id = 0x25;
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

	}

	public partial class McpeHurtArmor : Package<McpeHurtArmor>
	{
		public int health; // = null;
		public McpeHurtArmor()
		{
			Id = 0x26;
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

	}

	public partial class McpeSetEntityData : Package<McpeSetEntityData>
	{
		public long runtimeEntityId; // = null;
		public MetadataDictionary metadata; // = null;
		public McpeSetEntityData()
		{
			Id = 0x27;
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

	}

	public partial class McpeSetEntityMotion : Package<McpeSetEntityMotion>
	{
		public long runtimeEntityId; // = null;
		public Vector3 velocity; // = null;
		public McpeSetEntityMotion()
		{
			Id = 0x28;
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

	}

	public partial class McpeSetEntityLink : Package<McpeSetEntityLink>
	{
		public long riderId; // = null;
		public long riddenId; // = null;
		public byte linkType; // = null;
		public McpeSetEntityLink()
		{
			Id = 0x29;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(riderId);
			WriteUnsignedVarLong(riddenId);
			Write(linkType);

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

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeSetHealth : Package<McpeSetHealth>
	{
		public int health; // = null;
		public McpeSetHealth()
		{
			Id = 0x2a;
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

	}

	public partial class McpeSetSpawnPosition : Package<McpeSetSpawnPosition>
	{
		public int spawnType; // = null;
		public BlockCoordinates coordinates; // = null;
		public bool forced; // = null;
		public McpeSetSpawnPosition()
		{
			Id = 0x2b;
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

	}

	public partial class McpeAnimate : Package<McpeAnimate>
	{
		public int actionId; // = null;
		public long runtimeEntityId; // = null;
		public McpeAnimate()
		{
			Id = 0x2c;
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

	}

	public partial class McpeRespawn : Package<McpeRespawn>
	{
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public McpeRespawn()
		{
			Id = 0x2d;
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

	}

	public partial class McpeDropItem : Package<McpeDropItem>
	{
		public byte itemtype; // = null;
		public Item item; // = null;
		public McpeDropItem()
		{
			Id = 0x2e;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(itemtype);
			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			itemtype = ReadByte();
			item = ReadItem();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeInventoryAction : Package<McpeInventoryAction>
	{
		public uint actionId; // = null;
		public Item item; // = null;
		public int enchantmentId; // = null;
		public int enchantmentLevel; // = null;
		public McpeInventoryAction()
		{
			Id = 0x2f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(actionId);
			Write(item);
			WriteSignedVarInt(enchantmentId);
			WriteSignedVarInt(enchantmentLevel);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadUnsignedVarInt();
			item = ReadItem();
			enchantmentId = ReadSignedVarInt();
			enchantmentLevel = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeContainerOpen : Package<McpeContainerOpen>
	{
		public byte windowId; // = null;
		public byte type; // = null;
		public BlockCoordinates coordinates; // = null;
		public long unknownRuntimeEntityId; // = null;
		public McpeContainerOpen()
		{
			Id = 0x30;
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

	}

	public partial class McpeContainerClose : Package<McpeContainerClose>
	{
		public byte windowId; // = null;
		public McpeContainerClose()
		{
			Id = 0x31;
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

	}

	public partial class McpeContainerSetSlot : Package<McpeContainerSetSlot>
	{
		public byte windowId; // = null;
		public int slot; // = null;
		public int hotbarslot; // = null;
		public Item item; // = null;
		public byte selectedSlot; // = null;
		public McpeContainerSetSlot()
		{
			Id = 0x32;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			WriteSignedVarInt(slot);
			WriteSignedVarInt(hotbarslot);
			Write(item);
			Write(selectedSlot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			windowId = ReadByte();
			slot = ReadSignedVarInt();
			hotbarslot = ReadSignedVarInt();
			item = ReadItem();
			selectedSlot = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeContainerSetData : Package<McpeContainerSetData>
	{
		public byte windowId; // = null;
		public int property; // = null;
		public int value; // = null;
		public McpeContainerSetData()
		{
			Id = 0x33;
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

	}

	public partial class McpeContainerSetContent : Package<McpeContainerSetContent>
	{
		public uint windowId; // = null;
		public long entityIdSelf; // = null;
		public ItemStacks slotData; // = null;
		public MetadataInts hotbarData; // = null;
		public McpeContainerSetContent()
		{
			Id = 0x34;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(windowId);
			WriteSignedVarLong(entityIdSelf);
			Write(slotData);
			Write(hotbarData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			windowId = ReadUnsignedVarInt();
			entityIdSelf = ReadSignedVarLong();
			slotData = ReadItemStacks();
			hotbarData = ReadMetadataInts();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeCraftingData : Package<McpeCraftingData>
	{
		public Recipes recipes; // = null;
		public McpeCraftingData()
		{
			Id = 0x35;
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

	}

	public partial class McpeCraftingEvent : Package<McpeCraftingEvent>
	{
		public byte windowId; // = null;
		public int recipeType; // = null;
		public UUID recipeId; // = null;
		public ItemStacks input; // = null;
		public ItemStacks result; // = null;
		public McpeCraftingEvent()
		{
			Id = 0x36;
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

	}

	public partial class McpeAdventureSettings : Package<McpeAdventureSettings>
	{
		public uint flags; // = null;
		public uint userPermission; // = null;
		public McpeAdventureSettings()
		{
			Id = 0x37;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(flags);
			WriteUnsignedVarInt(userPermission);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			flags = ReadUnsignedVarInt();
			userPermission = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeBlockEntityData : Package<McpeBlockEntityData>
	{
		public BlockCoordinates coordinates; // = null;
		public Nbt namedtag; // = null;
		public McpeBlockEntityData()
		{
			Id = 0x38;
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

	}

	public partial class McpeFullChunkData : Package<McpeFullChunkData>
	{
		public int chunkX; // = null;
		public int chunkZ; // = null;
		public byte[] chunkData; // = null;
		public McpeFullChunkData()
		{
			Id = 0x3a;
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

	}

	public partial class McpeSetCommandsEnabled : Package<McpeSetCommandsEnabled>
	{
		public bool enabled; // = null;
		public McpeSetCommandsEnabled()
		{
			Id = 0x3b;
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

	}

	public partial class McpeSetDifficulty : Package<McpeSetDifficulty>
	{
		public uint difficulty; // = null;
		public McpeSetDifficulty()
		{
			Id = 0x3c;
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

	}

	public partial class McpeChangeDimension : Package<McpeChangeDimension>
	{
		public int dimension; // = null;
		public Vector3 position; // = null;
		public bool unknown; // = null;
		public McpeChangeDimension()
		{
			Id = 0x3d;
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

	}

	public partial class McpeSetPlayerGameType : Package<McpeSetPlayerGameType>
	{
		public int gamemode; // = null;
		public McpeSetPlayerGameType()
		{
			Id = 0x3e;
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

	}

	public partial class McpePlayerList : Package<McpePlayerList>
	{
		public PlayerRecords records; // = null;
		public McpePlayerList()
		{
			Id = 0x3f;
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

	}

	public partial class McpeSimpleEvent : Package<McpeSimpleEvent>
	{
		public McpeSimpleEvent()
		{
			Id = 0x40;
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

	}

	public partial class McpeEvent : Package<McpeEvent>
	{
		public long entityIdSelf; // = null;
		public int unk1; // = null;
		public byte unk2; // = null;
		public McpeEvent()
		{
			Id = 0x41;
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

	}

	public partial class McpeSpawnExperienceOrb : Package<McpeSpawnExperienceOrb>
	{
		public Vector3 position; // = null;
		public int count; // = null;
		public McpeSpawnExperienceOrb()
		{
			Id = 0x42;
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

	}

	public partial class McpeClientboundMapItemData : Package<McpeClientboundMapItemData>
	{
		public MapInfo mapinfo; // = null;
		public McpeClientboundMapItemData()
		{
			Id = 0x43;
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

	}

	public partial class McpeMapInfoRequest : Package<McpeMapInfoRequest>
	{
		public long mapId; // = null;
		public McpeMapInfoRequest()
		{
			Id = 0x44;
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

	}

	public partial class McpeRequestChunkRadius : Package<McpeRequestChunkRadius>
	{
		public int chunkRadius; // = null;
		public McpeRequestChunkRadius()
		{
			Id = 0x45;
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

	}

	public partial class McpeChunkRadiusUpdate : Package<McpeChunkRadiusUpdate>
	{
		public int chunkRadius; // = null;
		public McpeChunkRadiusUpdate()
		{
			Id = 0x46;
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

	}

	public partial class McpeItemFrameDropItem : Package<McpeItemFrameDropItem>
	{
		public BlockCoordinates coordinates; // = null;
		public McpeItemFrameDropItem()
		{
			Id = 0x47;
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

	}

	public partial class McpeReplaceSelectedItem : Package<McpeReplaceSelectedItem>
	{
		public Item item; // = null;
		public McpeReplaceSelectedItem()
		{
			Id = 0x48;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			item = ReadItem();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeGameRulesChanged : Package<McpeGameRulesChanged>
	{
		public GameRules rules; // = null;
		public McpeGameRulesChanged()
		{
			Id = 0x49;
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

	}

	public partial class McpeCamera : Package<McpeCamera>
	{
		public McpeCamera()
		{
			Id = 0x4a;
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

	}

	public partial class McpeAddItem : Package<McpeAddItem>
	{
		public Item item; // = null;
		public McpeAddItem()
		{
			Id = 0x4b;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			item = ReadItem();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeBossEvent : Package<McpeBossEvent>
	{
		public long bossEntityId; // = null;
		public uint eventType; // = null;
		public McpeBossEvent()
		{
			Id = 0x4c;
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

	}

	public partial class McpeShowCredits : Package<McpeShowCredits>
	{
		public long runtimeEntityId; // = null;
		public int status; // = null;
		public McpeShowCredits()
		{
			Id = 0x4d;
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

	}

	public partial class McpeAvailableCommands : Package<McpeAvailableCommands>
	{
		public string commands; // = null;
		public string unknown; // = null;
		public McpeAvailableCommands()
		{
			Id = 0x4e;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(commands);
			Write(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			commands = ReadString();
			unknown = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeCommandStep : Package<McpeCommandStep>
	{
		public string commandName; // = null;
		public string commandOverload; // = null;
		public uint unknown1; // = null;
		public uint currentStep; // = null;
		public bool isOutput; // = null;
		public long clientId; // = null;
		public string commandInputJson; // = null;
		public string commandOutputJson; // = null;
		public byte unknown7; // = null;
		public byte unknown8; // = null;
		public long entityIdSelf; // = null;
		public McpeCommandStep()
		{
			Id = 0x4f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(commandName);
			Write(commandOverload);
			WriteUnsignedVarInt(unknown1);
			WriteUnsignedVarInt(currentStep);
			Write(isOutput);
			WriteUnsignedVarLong(clientId);
			Write(commandInputJson);
			Write(commandOutputJson);
			Write(unknown7);
			Write(unknown8);
			WriteSignedVarLong(entityIdSelf);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			commandName = ReadString();
			commandOverload = ReadString();
			unknown1 = ReadUnsignedVarInt();
			currentStep = ReadUnsignedVarInt();
			isOutput = ReadBool();
			clientId = ReadUnsignedVarLong();
			commandInputJson = ReadString();
			commandOutputJson = ReadString();
			unknown7 = ReadByte();
			unknown8 = ReadByte();
			entityIdSelf = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeCommandBlockUpdate : Package<McpeCommandBlockUpdate>
	{
		public McpeCommandBlockUpdate()
		{
			Id = 0x50;
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

	}

	public partial class McpeUpdateTrade : Package<McpeUpdateTrade>
	{
		public McpeUpdateTrade()
		{
			Id = 0x51;
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

	}

	public partial class McpeUpdateEquip : Package<McpeUpdateEquip>
	{
		public McpeUpdateEquip()
		{
			Id = 0x52;
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
			Id = 0x53;
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
			Id = 0x54;
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
			payload = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeResourcePackChunkRequest : Package<McpeResourcePackChunkRequest>
	{
		public string packageId; // = null;
		public uint chunkIndex; // = null;
		public McpeResourcePackChunkRequest()
		{
			Id = 0x55;
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

	}

	public partial class McpeTransfer : Package<McpeTransfer>
	{
		public string serverAddress; // = null;
		public ushort port; // = null;
		public McpeTransfer()
		{
			Id = 0x56;
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

	}

	public partial class McpePlaySound : Package<McpePlaySound>
	{
		public string name; // = null;
		public BlockCoordinates coordinates; // = null;
		public float volume; // = null;
		public float pitch; // = null;
		public McpePlaySound()
		{
			Id = 0x57;
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

	}

	public partial class McpeStopSound : Package<McpeStopSound>
	{
		public string name; // = null;
		public bool stopAll; // = null;
		public McpeStopSound()
		{
			Id = 0x58;
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
			Id = 0x59;
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

	}

	public partial class McpeAddBehaviorTreePacket : Package<McpeAddBehaviorTreePacket>
	{
		public McpeAddBehaviorTreePacket()
		{
			Id = 0x5a;
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

	}

	public partial class McpeStructureBlockUpdatePacket : Package<McpeStructureBlockUpdatePacket>
	{
		public McpeStructureBlockUpdatePacket()
		{
			Id = 0x5b;
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

	}

	public partial class McpeWrapper : Package<McpeWrapper>
	{
		public byte[] payload; // = null;
		public McpeWrapper()
		{
			Id = 0xfe;
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

			payload = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

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

	}

}

