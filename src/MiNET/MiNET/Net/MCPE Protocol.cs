
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
		void HandleMcpeClientMagic(McpeClientMagic message);
		void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message);
		void HandleMcpeText(McpeText message);
		void HandleMcpeMovePlayer(McpeMovePlayer message);
		void HandleMcpeRemoveBlock(McpeRemoveBlock message);
		void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
		void HandleMcpeEntityEvent(McpeEntityEvent message);
		void HandleMcpeMobEquipment(McpeMobEquipment message);
		void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
		void HandleMcpeInteract(McpeInteract message);
		void HandleMcpeUseItem(McpeUseItem message);
		void HandleMcpePlayerAction(McpePlayerAction message);
		void HandleMcpePlayerFall(McpePlayerFall message);
		void HandleMcpeAnimate(McpeAnimate message);
		void HandleMcpeRespawn(McpeRespawn message);
		void HandleMcpeDropItem(McpeDropItem message);
		void HandleMcpeContainerClose(McpeContainerClose message);
		void HandleMcpeContainerSetSlot(McpeContainerSetSlot message);
		void HandleMcpeCraftingEvent(McpeCraftingEvent message);
		void HandleMcpeBlockEntityData(McpeBlockEntityData message);
		void HandleMcpePlayerInput(McpePlayerInput message);
		void HandleMcpeMapInfoRequest(McpeMapInfoRequest message);
		void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message);
		void HandleMcpeItemFramDropItem(McpeItemFramDropItem message);
		void HandleMcpeCommandStep(McpeCommandStep message);
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
						package = McpePlayerStatus.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x03:
						package = McpeServerExchange.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x04:
						package = McpeClientMagic.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x05:
						package = McpeDisconnect.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x06:
						package = McpeBatch.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x07:
						package = McpeResourcePacksInfo.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x08:
						package = McpeResourcePackStack.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x09:
						package = McpeResourcePackClientResponse.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0a:
						package = McpeText.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0b:
						package = McpeSetTime.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0c:
						package = McpeStartGame.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0d:
						package = McpeAddPlayer.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0e:
						package = McpeAddEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0f:
						package = McpeRemoveEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x10:
						package = McpeAddItemEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x11:
						package = McpeAddHangingEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x12:
						package = McpeTakeItemEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x13:
						package = McpeMoveEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x14:
						package = McpeMovePlayer.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x15:
						package = McpeRiderJump.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x16:
						package = McpeRemoveBlock.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x17:
						package = McpeUpdateBlock.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x18:
						package = McpeAddPainting.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x19:
						package = McpeExplode.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1a:
						package = McpeLevelSoundEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1b:
						package = McpeLevelEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1c:
						package = McpeBlockEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1d:
						package = McpeEntityEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1e:
						package = McpeMobEffect.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1f:
						package = McpeUpdateAttributes.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x20:
						package = McpeMobEquipment.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x21:
						package = McpeMobArmorEquipment.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x22:
						package = McpeInteract.CreateObject();
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
						package = McpePlayerFall.CreateObject();
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
						package = McpeEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x41:
						package = McpeSpawnExperienceOrb.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x42:
						package = McpeClientboundMapItemData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x43:
						package = McpeMapInfoRequest.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x44:
						package = McpeRequestChunkRadius.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x45:
						package = McpeChunkRadiusUpdate.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x46:
						package = McpeItemFramDropItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x47:
						package = McpeReplaceSelectedItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x48:
						package = McpeGameRulesChanged.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x49:
						package = McpeCamera.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4a:
						package = McpeAddItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4b:
						package = McpeBossEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4d:
						package = McpeAvailableCommands.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4e:
						package = McpeCommandStep.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x4f:
						package = McpeResourcePackDataInfo.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x50:
						package = McpeResourcePackChunkData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x51:
						package = McpeResourcePackChunkRequest.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x52:
						package = McpeTransfer.CreateObject();
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
			systemAddresses = ReadIPEndPoints(10);
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
			systemAddresses = ReadIPEndPoints(10);
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

	public partial class McpePlayerStatus : Package<McpePlayerStatus>
	{
		public int status; // = null;
		public McpePlayerStatus()
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

	public partial class McpeServerExchange : Package<McpeServerExchange>
	{
		public string serverPublicKey; // = null;
		public int tokenLength; // = null;
		public byte[] token; // = null;
		public McpeServerExchange()
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

	public partial class McpeClientMagic : Package<McpeClientMagic>
	{
		public McpeClientMagic()
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

	public partial class McpeBatch : Package<McpeBatch>
	{
		public byte[] payload; // = null;
		public McpeBatch()
		{
			Id = 0x06;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteByteArray(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			payload = ReadByteArray();

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
			Id = 0x07;
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
			Id = 0x08;
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
		public ResourcePackIdVersions resourcepackidversions; // = null;
		public McpeResourcePackClientResponse()
		{
			Id = 0x09;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(responseStatus);
			Write(resourcepackidversions);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			responseStatus = ReadByte();
			resourcepackidversions = ReadResourcePackIdVersions();

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
			Id = 0x0a;
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
		public bool started; // = null;
		public McpeSetTime()
		{
			Id = 0x0b;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(time);
			Write(started);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			time = ReadSignedVarInt();
			started = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeStartGame : Package<McpeStartGame>
	{
		public long entityId; // = null;
		public long runtimeEntityId; // = null;
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
		public string secret; // = null;
		public string worldName; // = null;
		public McpeStartGame()
		{
			Id = 0x0c;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(entityId);
			WriteUnsignedVarLong(runtimeEntityId);
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
			Write(secret);
			Write(worldName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
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
			secret = ReadString();
			worldName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAddPlayer : Package<McpeAddPlayer>
	{
		public UUID uuid; // = null;
		public string username; // = null;
		public long entityId; // = null;
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
			Id = 0x0d;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(uuid);
			Write(username);
			WriteSignedVarLong(entityId);
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
			entityId = ReadSignedVarLong();
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
		public long entityId; // = null;
		public long runtimeEntityId; // = null;
		public uint entityType; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float speedX; // = null;
		public float speedY; // = null;
		public float speedZ; // = null;
		public float yaw; // = null;
		public float pitch; // = null;
		public EntityAttributes attributes; // = null;
		public MetadataDictionary metadata; // = null;
		public Links links; // = null;
		public McpeAddEntity()
		{
			Id = 0x0e;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(entityId);
			WriteUnsignedVarLong(runtimeEntityId);
			WriteUnsignedVarInt(entityType);
			Write(x);
			Write(y);
			Write(z);
			Write(speedX);
			Write(speedY);
			Write(speedZ);
			Write(yaw);
			Write(pitch);
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

			entityId = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			entityType = ReadUnsignedVarInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			speedX = ReadFloat();
			speedY = ReadFloat();
			speedZ = ReadFloat();
			yaw = ReadFloat();
			pitch = ReadFloat();
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
		public long entityId; // = null;
		public McpeRemoveEntity()
		{
			Id = 0x0f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(entityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAddItemEntity : Package<McpeAddItemEntity>
	{
		public long entityId; // = null;
		public long runtimeEntityId; // = null;
		public Item item; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float speedX; // = null;
		public float speedY; // = null;
		public float speedZ; // = null;
		public McpeAddItemEntity()
		{
			Id = 0x10;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(entityId);
			WriteUnsignedVarLong(runtimeEntityId);
			Write(item);
			Write(x);
			Write(y);
			Write(z);
			Write(speedX);
			Write(speedY);
			Write(speedZ);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			item = ReadItem();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			speedX = ReadFloat();
			speedY = ReadFloat();
			speedZ = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAddHangingEntity : Package<McpeAddHangingEntity>
	{
		public long entityId; // = null;
		public long runtimeEntityId; // = null;
		public BlockCoordinates coordinates; // = null;
		public int unknown; // = null;
		public McpeAddHangingEntity()
		{
			Id = 0x11;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(entityId);
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

			entityId = ReadSignedVarLong();
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
		public long entityId; // = null;
		public long target; // = null;
		public McpeTakeItemEntity()
		{
			Id = 0x12;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
			WriteUnsignedVarLong(target);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadUnsignedVarLong();
			target = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeMoveEntity : Package<McpeMoveEntity>
	{
		public long entityId; // = null;
		public PlayerLocation position; // = null;
		public McpeMoveEntity()
		{
			Id = 0x13;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
			Write(position);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadUnsignedVarLong();
			position = ReadPlayerLocation();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeMovePlayer : Package<McpeMovePlayer>
	{
		public long entityId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float pitch; // = null;
		public float headYaw; // = null;
		public float yaw; // = null;
		public byte mode; // = null;
		public bool onGround; // = null;
		public McpeMovePlayer()
		{
			Id = 0x14;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
			Write(x);
			Write(y);
			Write(z);
			Write(pitch);
			Write(headYaw);
			Write(yaw);
			Write(mode);
			Write(onGround);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadUnsignedVarLong();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			pitch = ReadFloat();
			headYaw = ReadFloat();
			yaw = ReadFloat();
			mode = ReadByte();
			onGround = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeRiderJump : Package<McpeRiderJump>
	{
		public McpeRiderJump()
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

	public partial class McpeRemoveBlock : Package<McpeRemoveBlock>
	{
		public BlockCoordinates coordinates; // = null;
		public McpeRemoveBlock()
		{
			Id = 0x16;
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
			Id = 0x17;
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
		public long entityId; // = null;
		public long runtimeEntityId; // = null;
		public BlockCoordinates coordinates; // = null;
		public int direction; // = null;
		public string title; // = null;
		public McpeAddPainting()
		{
			Id = 0x18;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarLong(entityId);
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

			entityId = ReadSignedVarLong();
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
		public float radius; // = null;
		public Records records; // = null;
		public McpeExplode()
		{
			Id = 0x19;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(position);
			Write(radius);
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
			radius = ReadFloat();
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
		public int volume; // = null;
		public int pitch; // = null;
		public bool unknown1; // = null;
		public bool unknown2; // = null;
		public McpeLevelSoundEvent()
		{
			Id = 0x1a;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(soundId);
			Write(position);
			WriteSignedVarInt(volume);
			WriteSignedVarInt(pitch);
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

			soundId = ReadByte();
			position = ReadVector3();
			volume = ReadSignedVarInt();
			pitch = ReadSignedVarInt();
			unknown1 = ReadBool();
			unknown2 = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeLevelEvent : Package<McpeLevelEvent>
	{
		public int eventId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public int data; // = null;
		public McpeLevelEvent()
		{
			Id = 0x1b;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(eventId);
			Write(x);
			Write(y);
			Write(z);
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
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
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
			Id = 0x1c;
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
		public long entityId; // = null;
		public byte eventId; // = null;
		public int unknown; // = null;
		public McpeEntityEvent()
		{
			Id = 0x1d;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
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

			entityId = ReadUnsignedVarLong();
			eventId = ReadByte();
			unknown = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeMobEffect : Package<McpeMobEffect>
	{
		public long entityId; // = null;
		public byte eventId; // = null;
		public int effectId; // = null;
		public int amplifier; // = null;
		public bool particles; // = null;
		public int duration; // = null;
		public McpeMobEffect()
		{
			Id = 0x1e;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
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

			entityId = ReadUnsignedVarLong();
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
		public long entityId; // = null;
		public PlayerAttributes attributes; // = null;
		public McpeUpdateAttributes()
		{
			Id = 0x1f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
			Write(attributes);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadUnsignedVarLong();
			attributes = ReadPlayerAttributes();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeMobEquipment : Package<McpeMobEquipment>
	{
		public long entityId; // = null;
		public Item item; // = null;
		public byte slot; // = null;
		public byte selectedSlot; // = null;
		public byte unknown; // = null;
		public McpeMobEquipment()
		{
			Id = 0x20;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
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

			entityId = ReadUnsignedVarLong();
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
		public long entityId; // = null;
		public Item helmet; // = null;
		public Item chestplate; // = null;
		public Item leggings; // = null;
		public Item boots; // = null;
		public McpeMobArmorEquipment()
		{
			Id = 0x21;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
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

			entityId = ReadUnsignedVarLong();
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
		public long targetEntityId; // = null;
		public McpeInteract()
		{
			Id = 0x22;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(actionId);
			WriteUnsignedVarLong(targetEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadByte();
			targetEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeUseItem : Package<McpeUseItem>
	{
		public BlockCoordinates blockcoordinates; // = null;
		public uint unknown; // = null;
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
			WriteUnsignedVarInt(unknown);
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
			unknown = ReadUnsignedVarInt();
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
		public long entityId; // = null;
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

			WriteUnsignedVarLong(entityId);
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

			entityId = ReadUnsignedVarLong();
			actionId = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();
			face = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpePlayerFall : Package<McpePlayerFall>
	{
		public McpePlayerFall()
		{
			Id = 0x25;
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
		public long entityId; // = null;
		public MetadataDictionary metadata; // = null;
		public McpeSetEntityData()
		{
			Id = 0x27;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
			Write(metadata);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadUnsignedVarLong();
			metadata = ReadMetadataDictionary();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeSetEntityMotion : Package<McpeSetEntityMotion>
	{
		public long entityId; // = null;
		public Vector3 velocity; // = null;
		public McpeSetEntityMotion()
		{
			Id = 0x28;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarLong(entityId);
			Write(velocity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadUnsignedVarLong();
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
		public int unknown1; // = null;
		public BlockCoordinates coordinates; // = null;
		public McpeSetSpawnPosition()
		{
			Id = 0x2b;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(unknown1);
			Write(coordinates);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			unknown1 = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAnimate : Package<McpeAnimate>
	{
		public int actionId; // = null;
		public long entityId; // = null;
		public McpeAnimate()
		{
			Id = 0x2c;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(actionId);
			WriteUnsignedVarLong(entityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadSignedVarInt();
			entityId = ReadUnsignedVarLong();

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
		public uint unknown; // = null;
		public Item item; // = null;
		public McpeInventoryAction()
		{
			Id = 0x2f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteUnsignedVarInt(unknown);
			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			unknown = ReadUnsignedVarInt();
			item = ReadItem();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeContainerOpen : Package<McpeContainerOpen>
	{
		public byte windowId; // = null;
		public byte type; // = null;
		public int slotCount; // = null;
		public BlockCoordinates coordinates; // = null;
		public long unknownEntityId; // = null;
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
			WriteSignedVarInt(slotCount);
			Write(coordinates);
			WriteUnsignedVarLong(unknownEntityId);

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
			slotCount = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();
			unknownEntityId = ReadUnsignedVarLong();

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
		public byte unknown2; // = null;
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
			Write(unknown2);

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
			unknown2 = ReadByte();

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
		public byte windowId; // = null;
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

			Write(windowId);
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

			windowId = ReadByte();
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
		public int unknown; // = null;
		public McpeSetPlayerGameType()
		{
			Id = 0x3e;
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

	public partial class McpeEvent : Package<McpeEvent>
	{
		public McpeEvent()
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

	public partial class McpeSpawnExperienceOrb : Package<McpeSpawnExperienceOrb>
	{
		public Vector3 position; // = null;
		public int count; // = null;
		public McpeSpawnExperienceOrb()
		{
			Id = 0x41;
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
			Id = 0x42;
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
			Id = 0x43;
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
			Id = 0x44;
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

	public partial class McpeItemFramDropItem : Package<McpeItemFramDropItem>
	{
		public BlockCoordinates coordinates; // = null;
		public Item item; // = null;
		public McpeItemFramDropItem()
		{
			Id = 0x46;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(coordinates);
			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			item = ReadItem();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeReplaceSelectedItem : Package<McpeReplaceSelectedItem>
	{
		public McpeReplaceSelectedItem()
		{
			Id = 0x47;
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

	public partial class McpeGameRulesChanged : Package<McpeGameRulesChanged>
	{
		public Rules rules; // = null;
		public McpeGameRulesChanged()
		{
			Id = 0x48;
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

			rules = ReadRules();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeCamera : Package<McpeCamera>
	{
		public McpeCamera()
		{
			Id = 0x49;
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
		public McpeAddItem()
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

	public partial class McpeBossEvent : Package<McpeBossEvent>
	{
		public McpeBossEvent()
		{
			Id = 0x4b;
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

	public partial class McpeAvailableCommands : Package<McpeAvailableCommands>
	{
		public string commands; // = null;
		public string unknown; // = null;
		public McpeAvailableCommands()
		{
			Id = 0x4d;
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
		public uint unknown2; // = null;
		public bool isOutput; // = null;
		public long unknown5; // = null;
		public string commandInputJson; // = null;
		public string commandOutputJson; // = null;
		public byte unknown7; // = null;
		public byte unknown8; // = null;
		public long entityId; // = null;
		public McpeCommandStep()
		{
			Id = 0x4e;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(commandName);
			Write(commandOverload);
			WriteUnsignedVarInt(unknown1);
			WriteUnsignedVarInt(unknown2);
			Write(isOutput);
			WriteUnsignedVarLong(unknown5);
			Write(commandInputJson);
			Write(commandOutputJson);
			Write(unknown7);
			Write(unknown8);
			WriteSignedVarLong(entityId);

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
			unknown2 = ReadUnsignedVarInt();
			isOutput = ReadBool();
			unknown5 = ReadUnsignedVarLong();
			commandInputJson = ReadString();
			commandOutputJson = ReadString();
			unknown7 = ReadByte();
			unknown8 = ReadByte();
			entityId = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeResourcePackDataInfo : Package<McpeResourcePackDataInfo>
	{
		public string packageId; // = null;
		public uint unknown1; // = null;
		public uint unknown2; // = null;
		public ulong unknown3; // = null;
		public string unknown4; // = null;
		public McpeResourcePackDataInfo()
		{
			Id = 0x4f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(packageId);
			Write(unknown1);
			Write(unknown2);
			Write(unknown3);
			Write(unknown4);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			packageId = ReadString();
			unknown1 = ReadUint();
			unknown2 = ReadUint();
			unknown3 = ReadUlong();
			unknown4 = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeResourcePackChunkData : Package<McpeResourcePackChunkData>
	{
		public string packageId; // = null;
		public uint unknown1; // = null;
		public ulong unknown3; // = null;
		public uint length; // = null;
		public byte[] payload; // = null;
		public McpeResourcePackChunkData()
		{
			Id = 0x50;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(packageId);
			Write(unknown1);
			Write(unknown3);
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
			unknown1 = ReadUint();
			unknown3 = ReadUlong();
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
		public int chunkIndex; // = null;
		public McpeResourcePackChunkRequest()
		{
			Id = 0x51;
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
			chunkIndex = ReadInt();

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
			Id = 0x52;
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

