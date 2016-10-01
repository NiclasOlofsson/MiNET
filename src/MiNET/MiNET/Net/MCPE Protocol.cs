
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
		void HandleMcpeText(McpeText message);
		void HandleMcpeMovePlayer(McpeMovePlayer message);
		void HandleMcpeRemoveBlock(McpeRemoveBlock message);
		void HandleMcpeEntityEvent(McpeEntityEvent message);
		void HandleMcpeMobEquipment(McpeMobEquipment message);
		void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
		void HandleMcpeInteract(McpeInteract message);
		void HandleMcpeUseItem(McpeUseItem message);
		void HandleMcpePlayerAction(McpePlayerAction message);
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
						package = McpeText.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x08:
						package = McpeSetTime.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x09:
						package = McpeStartGame.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0a:
						package = McpeAddPlayer.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0b:
						package = McpeAddEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0c:
						package = McpeRemoveEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0d:
						package = McpeAddItemEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0e:
						package = McpeAddHangingEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x0f:
						package = McpeTakeItemEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x10:
						package = McpeMoveEntity.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x11:
						package = McpeMovePlayer.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x12:
						package = McpeRiderJump.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x13:
						package = McpeRemoveBlock.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x14:
						package = McpeUpdateBlock.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x15:
						package = McpeAddPainting.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x16:
						package = McpeExplode.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x17:
						package = McpeLevelSoundEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x18:
						package = McpeLevelEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x19:
						package = McpeBlockEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1a:
						package = McpeEntityEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1b:
						package = McpeMobEffect.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1c:
						package = McpeUpdateAttributes.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1d:
						package = McpeMobEquipment.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1e:
						package = McpeMobArmorEquipment.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x1f:
						package = McpeInteract.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x20:
						package = McpeUseItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x21:
						package = McpePlayerAction.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x22:
						package = McpeHurtArmor.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x23:
						package = McpeSetEntityData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x24:
						package = McpeSetEntityMotion.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x25:
						package = McpeSetEntityLink.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x26:
						package = McpeSetHealth.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x27:
						package = McpeSetSpawnPosition.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x28:
						package = McpeAnimate.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x29:
						package = McpeRespawn.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2a:
						package = McpeDropItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2b:
						package = McpeInventoryAction.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2c:
						package = McpeContainerOpen.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2d:
						package = McpeContainerClose.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2e:
						package = McpeContainerSetSlot.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x2f:
						package = McpeContainerSetData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x30:
						package = McpeContainerSetContent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x31:
						package = McpeCraftingData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x32:
						package = McpeCraftingEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x33:
						package = McpeAdventureSettings.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x34:
						package = McpeBlockEntityData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x35:
						package = McpePlayerInput.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x36:
						package = McpeFullChunkData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x37:
						package = McpeSetCommandsEnabled.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x38:
						package = McpeSetDifficulty.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x39:
						package = McpeChangeDimension.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3a:
						package = McpeSetPlayerGaneType.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3b:
						package = McpePlayerList.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3c:
						package = McpeTelemetryEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3d:
						package = McpeSpawnExperienceOrb.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3e:
						package = McpeClientboundMapItemData.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x3f:
						package = McpeMapInfoRequest.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x40:
						package = McpeRequestChunkRadius.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x41:
						package = McpeChunkRadiusUpdate.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x42:
						package = McpeItemFramDropItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x43:
						package = McpeReplaceSelectedItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x44:
						package = McpeGameRulesChanged.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x45:
						package = McpeCamera.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x46:
						package = McpeAddItem.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x47:
						package = McpeBossEvent.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x48:
						package = McpeAvailableCommands.CreateObject();
						//package.Timer.Start();
						package.Decode(buffer);
						return package;
					case 0x49:
						package = McpeCommandStep.CreateObject();
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
			Write(mtuSize);

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
			mtuSize = ReadShort();

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
			Write(mtuSize);
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
			mtuSize = ReadShort();
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
			Write(mtuSize);
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
			mtuSize = ReadShort();
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
			Write(systemIndex);
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
			systemIndex = ReadShort();
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

			Write(protocolVersion);
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

			protocolVersion = ReadInt();
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

			Write(status);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			status = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeServerExchange : Package<McpeServerExchange>
	{
		public string serverPublicKey; // = null;
		public int tokenLenght; // = null;
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
			WriteLenght(tokenLenght);
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
			tokenLenght = ReadLenght();
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
		public string message; // = null;
		public McpeDisconnect()
		{
			Id = 0x05;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(message);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

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

	public partial class McpeText : Package<McpeText>
	{
		public byte type; // = null;
		public McpeText()
		{
			Id = 0x07;
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
		public byte started; // = null;
		public McpeSetTime()
		{
			Id = 0x08;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(time);
			Write(started);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			time = ReadVarInt();
			started = ReadByte();

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
		public float unknown1; // = null;
		public float unknown2; // = null;
		public int seed; // = null;
		public int dimension; // = null;
		public int generator; // = null;
		public int gamemode; // = null;
		public int difficulty; // = null;
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public bool isLoadedInCreative; // = null;
		public int dayCycleStopTime; // = null;
		public bool eduMode; // = null;
		public float rainLevel; // = null;
		public float lightnigLevel; // = null;
		public bool enableCommands; // = null;
		public string secret; // = null;
		public string worldName; // = null;
		public McpeStartGame()
		{
			Id = 0x09;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			WriteVarLong(runtimeEntityId);
			Write(spawn);
			Write(unknown1);
			Write(unknown2);
			WriteSignedVarInt(seed);
			WriteSignedVarInt(dimension);
			WriteSignedVarInt(generator);
			WriteSignedVarInt(gamemode);
			WriteSignedVarInt(difficulty);
			WriteSignedVarInt(x);
			WriteSignedVarInt(y);
			WriteSignedVarInt(z);
			Write(isLoadedInCreative);
			WriteSignedVarInt(dayCycleStopTime);
			Write(eduMode);
			Write(rainLevel);
			Write(lightnigLevel);
			Write(enableCommands);
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

			entityId = ReadVarLong();
			runtimeEntityId = ReadVarLong();
			spawn = ReadVector3();
			unknown1 = ReadFloat();
			unknown2 = ReadFloat();
			seed = ReadSignedVarInt();
			dimension = ReadSignedVarInt();
			generator = ReadSignedVarInt();
			gamemode = ReadSignedVarInt();
			difficulty = ReadSignedVarInt();
			x = ReadSignedVarInt();
			y = ReadSignedVarInt();
			z = ReadSignedVarInt();
			isLoadedInCreative = ReadBool();
			dayCycleStopTime = ReadSignedVarInt();
			eduMode = ReadBool();
			rainLevel = ReadFloat();
			lightnigLevel = ReadFloat();
			enableCommands = ReadBool();
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
		public float yaw; // = null;
		public float headYaw; // = null;
		public float pitch; // = null;
		public Item item; // = null;
		public MetadataDictionary metadata; // = null;
		public McpeAddPlayer()
		{
			Id = 0x0a;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(uuid);
			Write(username);
			WriteVarLong(entityId);
			WriteVarLong(runtimeEntityId);
			Write(x);
			Write(y);
			Write(z);
			Write(speedX);
			Write(speedY);
			Write(speedZ);
			Write(yaw);
			Write(headYaw);
			Write(pitch);
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
			entityId = ReadVarLong();
			runtimeEntityId = ReadVarLong();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			speedX = ReadFloat();
			speedY = ReadFloat();
			speedZ = ReadFloat();
			yaw = ReadFloat();
			headYaw = ReadFloat();
			pitch = ReadFloat();
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
		public int links; // = null;
		public McpeAddEntity()
		{
			Id = 0x0b;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			WriteVarLong(runtimeEntityId);
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
			WriteVarInt(links);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();
			runtimeEntityId = ReadVarLong();
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
			links = ReadVarInt();

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
			Id = 0x0c;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();

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
			Id = 0x0d;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			WriteVarLong(runtimeEntityId);
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

			entityId = ReadVarLong();
			runtimeEntityId = ReadVarLong();
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
			Id = 0x0e;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			WriteVarLong(runtimeEntityId);
			Write(coordinates);
			WriteVarInt(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();
			runtimeEntityId = ReadVarLong();
			coordinates = ReadBlockCoordinates();
			unknown = ReadVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeTakeItemEntity : Package<McpeTakeItemEntity>
	{
		public long target; // = null;
		public long entityId; // = null;
		public McpeTakeItemEntity()
		{
			Id = 0x0f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(target);
			WriteVarLong(entityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			target = ReadVarLong();
			entityId = ReadVarLong();

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
			Id = 0x10;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			Write(position);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();
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
		public float yaw; // = null;
		public float headYaw; // = null;
		public float pitch; // = null;
		public byte mode; // = null;
		public byte onGround; // = null;
		public McpeMovePlayer()
		{
			Id = 0x11;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			Write(x);
			Write(y);
			Write(z);
			Write(yaw);
			Write(headYaw);
			Write(pitch);
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

			entityId = ReadVarLong();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			yaw = ReadFloat();
			headYaw = ReadFloat();
			pitch = ReadFloat();
			mode = ReadByte();
			onGround = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeRiderJump : Package<McpeRiderJump>
	{
		public McpeRiderJump()
		{
			Id = 0x12;
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
			Id = 0x13;
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
			Id = 0x14;
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
			Id = 0x15;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			WriteVarLong(runtimeEntityId);
			Write(coordinates);
			WriteVarInt(direction);
			Write(title);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();
			runtimeEntityId = ReadVarLong();
			coordinates = ReadBlockCoordinates();
			direction = ReadVarInt();
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
			Id = 0x16;
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
		public McpeLevelSoundEvent()
		{
			Id = 0x17;
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

	public partial class McpeLevelEvent : Package<McpeLevelEvent>
	{
		public int eventId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public int data; // = null;
		public McpeLevelEvent()
		{
			Id = 0x18;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(eventId);
			Write(x);
			Write(y);
			Write(z);
			WriteVarInt(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			eventId = ReadVarInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			data = ReadVarInt();

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
			Id = 0x19;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(coordinates);
			WriteVarInt(case1);
			WriteVarInt(case2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			case1 = ReadVarInt();
			case2 = ReadVarInt();

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
			Id = 0x1a;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			Write(eventId);
			WriteVarInt(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();
			eventId = ReadByte();
			unknown = ReadVarInt();

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
		public byte particles; // = null;
		public int duration; // = null;
		public McpeMobEffect()
		{
			Id = 0x1b;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			Write(eventId);
			WriteVarInt(effectId);
			WriteVarInt(amplifier);
			Write(particles);
			WriteVarInt(duration);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();
			eventId = ReadByte();
			effectId = ReadVarInt();
			amplifier = ReadVarInt();
			particles = ReadByte();
			duration = ReadVarInt();

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
			Id = 0x1c;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			Write(attributes);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();
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
			Id = 0x1d;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
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

			entityId = ReadVarLong();
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
			Id = 0x1e;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
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

			entityId = ReadVarLong();
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
			Id = 0x1f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(actionId);
			WriteVarLong(targetEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadByte();
			targetEntityId = ReadVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeUseItem : Package<McpeUseItem>
	{
		public BlockCoordinates blockcoordinates; // = null;
		public int face; // = null;
		public Vector3 facecoordinates; // = null;
		public Vector3 playerposition; // = null;
		public byte unknown; // = null;
		public Item item; // = null;
		public McpeUseItem()
		{
			Id = 0x20;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(blockcoordinates);
			WriteSignedVarInt(face);
			Write(facecoordinates);
			Write(playerposition);
			Write(unknown);
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
			face = ReadSignedVarInt();
			facecoordinates = ReadVector3();
			playerposition = ReadVector3();
			unknown = ReadByte();
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
			Id = 0x21;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
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

			entityId = ReadVarLong();
			actionId = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();
			face = ReadSignedVarInt();

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
			Id = 0x22;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			health = ReadVarInt();

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
			Id = 0x23;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			Write(metadata);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();
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
			Id = 0x24;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(entityId);
			Write(velocity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadVarLong();
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
			Id = 0x25;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(riderId);
			WriteVarLong(riddenId);
			Write(linkType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			riderId = ReadVarLong();
			riddenId = ReadVarLong();
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
			Id = 0x26;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			health = ReadVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeSetSpawnPosition : Package<McpeSetSpawnPosition>
	{
		public int unknown1; // = null;
		public BlockCoordinates coordinates; // = null;
		public bool unknown2; // = null;
		public McpeSetSpawnPosition()
		{
			Id = 0x27;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(unknown1);
			Write(coordinates);
			Write(unknown2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			unknown1 = ReadVarInt();
			coordinates = ReadBlockCoordinates();
			unknown2 = ReadBool();

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
			Id = 0x28;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(actionId);
			WriteVarLong(entityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadVarInt();
			entityId = ReadVarLong();

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
			Id = 0x29;
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
			Id = 0x2a;
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
		public int unknown; // = null;
		public Item item; // = null;
		public McpeInventoryAction()
		{
			Id = 0x2b;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(unknown);
			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			unknown = ReadVarInt();
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
		public long unownEntityId; // = null;
		public McpeContainerOpen()
		{
			Id = 0x2c;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			Write(type);
			WriteVarInt(slotCount);
			Write(coordinates);
			WriteVarLong(unownEntityId);

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
			slotCount = ReadVarInt();
			coordinates = ReadBlockCoordinates();
			unownEntityId = ReadVarLong();

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
			Id = 0x2d;
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
		public int unknown; // = null;
		public Item item; // = null;
		public McpeContainerSetSlot()
		{
			Id = 0x2e;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			WriteVarInt(slot);
			WriteVarInt(unknown);
			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			windowId = ReadByte();
			slot = ReadVarInt();
			unknown = ReadVarInt();
			item = ReadItem();

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
			Id = 0x2f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			WriteVarInt(property);
			WriteVarInt(value);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			windowId = ReadByte();
			property = ReadVarInt();
			value = ReadVarInt();

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
			Id = 0x30;
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
			Id = 0x31;
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
			Id = 0x32;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			WriteVarInt(recipeType);
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
			recipeType = ReadVarInt();
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
			Id = 0x33;
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
			Id = 0x34;
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
		public short flags; // = null;
		public McpePlayerInput()
		{
			Id = 0x35;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(motionX);
			Write(motionZ);
			Write(flags);

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
			flags = ReadShort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeFullChunkData : Package<McpeFullChunkData>
	{
		public int chunkX; // = null;
		public int chunkZ; // = null;
		public byte order; // = null;
		public byte[] chunkData; // = null;
		public McpeFullChunkData()
		{
			Id = 0x36;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteSignedVarInt(chunkX);
			WriteSignedVarInt(chunkZ);
			Write(order);
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
			order = ReadByte();
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
			Id = 0x37;
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
			Id = 0x38;
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
		public byte dimension; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public byte unknown; // = null;
		public McpeChangeDimension()
		{
			Id = 0x39;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(dimension);
			Write(x);
			Write(y);
			Write(z);
			Write(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			dimension = ReadByte();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			unknown = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeSetPlayerGaneType : Package<McpeSetPlayerGaneType>
	{
		public McpeSetPlayerGaneType()
		{
			Id = 0x3a;
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

	public partial class McpePlayerList : Package<McpePlayerList>
	{
		public PlayerRecords records; // = null;
		public McpePlayerList()
		{
			Id = 0x3b;
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

	public partial class McpeTelemetryEvent : Package<McpeTelemetryEvent>
	{
		public McpeTelemetryEvent()
		{
			Id = 0x3c;
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
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public int count; // = null;
		public McpeSpawnExperienceOrb()
		{
			Id = 0x3d;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);
			WriteVarInt(count);

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
			count = ReadVarInt();

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
			Id = 0x3e;
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
			Id = 0x3f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarLong(mapId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			mapId = ReadVarLong();

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
			Id = 0x40;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(chunkRadius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			chunkRadius = ReadVarInt();

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
			Id = 0x41;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			WriteVarInt(chunkRadius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			chunkRadius = ReadVarInt();

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
			Id = 0x42;
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
			Id = 0x43;
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
		public McpeGameRulesChanged()
		{
			Id = 0x44;
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

	public partial class McpeCamera : Package<McpeCamera>
	{
		public McpeCamera()
		{
			Id = 0x45;
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
			Id = 0x46;
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

	public partial class McpeAvailableCommands : Package<McpeAvailableCommands>
	{
		public string commands; // = null;
		public string unknown; // = null;
		public McpeAvailableCommands()
		{
			Id = 0x48;
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
		public McpeCommandStep()
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

