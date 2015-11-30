﻿
//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

using System;
using System.Net;
using System.Threading;
using MiNET.Utils; 
using MiNET.Crafting;
using little = MiNET.Utils.Int24; // friendly name

namespace MiNET.Net
{

	public class PackageFactory
	{
		public static Package CreatePackage(byte messageId, byte[] buffer)
		{
			Package package = null; 
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
				case 0x8f:
					package = McpeLogin.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x90:
					package = McpePlayerStatus.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x91:
					package = McpeDisconnect.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x92:
					package = McpeBatch.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x93:
					package = McpeText.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x94:
					package = McpeSetTime.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x95:
					package = McpeStartGame.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x96:
					package = McpeAddPlayer.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x97:
					package = McpeRemovePlayer.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x98:
					package = McpeAddEntity.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x99:
					package = McpeRemoveEntity.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x9a:
					package = McpeAddItemEntity.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x9b:
					package = McpeTakeItemEntity.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x9c:
					package = McpeMoveEntity.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x9d:
					package = McpeMovePlayer.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x9e:
					package = McpeRemoveBlock.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x9f:
					package = McpeUpdateBlock.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa0:
					package = McpeAddPainting.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa1:
					package = McpeExplode.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa2:
					package = McpeLevelEvent.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa3:
					package = McpeTileEvent.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa4:
					package = McpeEntityEvent.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa5:
					package = McpeMobEffect.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa6:
					package = McpeUpdateAttributes.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa7:
					package = McpePlayerEquipment.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa8:
					package = McpePlayerArmorEquipment.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa9:
					package = McpeInteract.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xaa:
					package = McpeUseItem.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xab:
					package = McpePlayerAction.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xac:
					package = McpeHurtArmor.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xad:
					package = McpeSetEntityData.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xae:
					package = McpeSetEntityMotion.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xaf:
					package = McpeSetEntityLink.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb0:
					package = McpeSetHealth.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb1:
					package = McpeSetSpawnPosition.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb2:
					package = McpeAnimate.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb3:
					package = McpeRespawn.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb4:
					package = McpeDropItem.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb5:
					package = McpeContainerOpen.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb6:
					package = McpeContainerClose.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb7:
					package = McpeContainerSetSlot.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb8:
					package = McpeContainerSetData.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb9:
					package = McpeContainerSetContent.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xba:
					package = McpeCraftingData.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xbb:
					package = McpeCraftingEvent.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xbc:
					package = McpeAdventureSettings.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xbd:
					package = McpeTileEntityData.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xbf:
					package = McpeFullChunkData.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xc0:
					package = McpeSetDifficulty.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xc3:
					package = McpePlayerList.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x1b:
					package = McpeTransfer.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xc5:
					package = McpeSpawnExperienceOrb.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
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
			Write(serverName);

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
			serverName = ReadString();

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
		public IPEndPoint clientendpoint; // = null;
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
			Write(clientendpoint);
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
			clientendpoint = ReadIPEndPoint();
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
		public IPEndPoint clientendpoint; // = null;
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
			Write(clientendpoint);
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
			clientendpoint = ReadIPEndPoint();
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
			systemAddresses = ReadIPEndPoints();
			incomingTimestamp = ReadLong();
			serverTimestamp = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class NewIncomingConnection : Package<NewIncomingConnection>
	{
		public int cookie; // = null;
		public byte doSecurity; // = null;
		public short port; // = null;
		public long session; // = null;
		public long session2; // = null;
		public NewIncomingConnection()
		{
			Id = 0x13;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(cookie);
			Write(doSecurity);
			Write(port);
			Write(session);
			Write(session2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			cookie = ReadInt();
			doSecurity = ReadByte();
			port = ReadShort();
			session = ReadLong();
			session2 = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class NoFreeIncomingConnections : Package<NoFreeIncomingConnections>
	{
		public NoFreeIncomingConnections()
		{
			Id = 0x14;
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
		public ConnectionBanned()
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

	public partial class IpRecentlyConnected : Package<IpRecentlyConnected>
	{
		public IpRecentlyConnected()
		{
			Id = 0x1a;
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

	public partial class McpeLogin : Package<McpeLogin>
	{
		public string username; // = null;
		public int protocol; // = null;
		public int protocol2; // = null;
		public long clientId; // = null;
		public UUID clientUuid; // = null;
		public string serverAddress; // = null;
		public string clientSecret; // = null;
		public Skin skin; // = null;
		public McpeLogin()
		{
			Id = 0x8f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(username);
			Write(protocol);
			Write(protocol2);
			Write(clientId);
			Write(clientUuid);
			Write(serverAddress);
			Write(clientSecret);
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
			protocol = ReadInt();
			protocol2 = ReadInt();
			clientId = ReadLong();
			clientUuid = ReadUUID();
			serverAddress = ReadString();
			clientSecret = ReadString();
			skin = ReadSkin();

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
			Id = 0x90;
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

	public partial class McpeDisconnect : Package<McpeDisconnect>
	{
		public string message; // = null;
		public McpeDisconnect()
		{
			Id = 0x91;
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
		public int payloadSize; // = null;
		public byte[] payload; // = null;
		public McpeBatch()
		{
			Id = 0x92;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(payloadSize);
			Write(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			payloadSize = ReadInt();
			payload = ReadBytes(0);

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
			Id = 0x93;
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
			Id = 0x94;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(time);
			Write(started);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			time = ReadInt();
			started = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeStartGame : Package<McpeStartGame>
	{
		public int seed; // = null;
		public byte dimension; // = null;
		public int generator; // = null;
		public int gamemode; // = null;
		public long entityId; // = null;
		public int spawnX; // = null;
		public int spawnY; // = null;
		public int spawnZ; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public byte unknown; // = null;
		public McpeStartGame()
		{
			Id = 0x95;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(seed);
			Write(dimension);
			Write(generator);
			Write(gamemode);
			Write(entityId);
			Write(spawnX);
			Write(spawnY);
			Write(spawnZ);
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

			seed = ReadInt();
			dimension = ReadByte();
			generator = ReadInt();
			gamemode = ReadInt();
			entityId = ReadLong();
			spawnX = ReadInt();
			spawnY = ReadInt();
			spawnZ = ReadInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			unknown = ReadByte();

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
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float speedX; // = null;
		public float speedY; // = null;
		public float speedZ; // = null;
		public float yaw; // = null;
		public float headYaw; // = null;
		public float pitch; // = null;
		public MetadataSlot item; // = null;
		public MetadataDictionary metadata; // = null;
		public McpeAddPlayer()
		{
			Id = 0x96;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(uuid);
			Write(username);
			Write(entityId);
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
			entityId = ReadLong();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			speedX = ReadFloat();
			speedY = ReadFloat();
			speedZ = ReadFloat();
			yaw = ReadFloat();
			headYaw = ReadFloat();
			pitch = ReadFloat();
			item = ReadMetadataSlot();
			metadata = ReadMetadataDictionary();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeRemovePlayer : Package<McpeRemovePlayer>
	{
		public long entityId; // = null;
		public UUID clientUuid; // = null;
		public McpeRemovePlayer()
		{
			Id = 0x97;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(clientUuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			clientUuid = ReadUUID();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAddEntity : Package<McpeAddEntity>
	{
		public long entityId; // = null;
		public int entityType; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float speedX; // = null;
		public float speedY; // = null;
		public float speedZ; // = null;
		public float yaw; // = null;
		public float pitch; // = null;
		public MetadataDictionary metadata; // = null;
		public short links; // = null;
		public McpeAddEntity()
		{
			Id = 0x98;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(entityType);
			Write(x);
			Write(y);
			Write(z);
			Write(speedX);
			Write(speedY);
			Write(speedZ);
			Write(yaw);
			Write(pitch);
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

			entityId = ReadLong();
			entityType = ReadInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			speedX = ReadFloat();
			speedY = ReadFloat();
			speedZ = ReadFloat();
			yaw = ReadFloat();
			pitch = ReadFloat();
			metadata = ReadMetadataDictionary();
			links = ReadShort();

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
			Id = 0x99;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAddItemEntity : Package<McpeAddItemEntity>
	{
		public long entityId; // = null;
		public MetadataSlot item; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float speedX; // = null;
		public float speedY; // = null;
		public float speedZ; // = null;
		public McpeAddItemEntity()
		{
			Id = 0x9a;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
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

			entityId = ReadLong();
			item = ReadMetadataSlot();
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

	public partial class McpeTakeItemEntity : Package<McpeTakeItemEntity>
	{
		public long target; // = null;
		public long entityId; // = null;
		public McpeTakeItemEntity()
		{
			Id = 0x9b;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(target);
			Write(entityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			target = ReadLong();
			entityId = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeMoveEntity : Package<McpeMoveEntity>
	{
		public EntityLocations entities; // = null;
		public McpeMoveEntity()
		{
			Id = 0x9c;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entities);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entities = ReadEntityLocations();

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
			Id = 0x9d;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
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

			entityId = ReadLong();
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

	public partial class McpeRemoveBlock : Package<McpeRemoveBlock>
	{
		public long entityId; // = null;
		public int x; // = null;
		public int z; // = null;
		public byte y; // = null;
		public McpeRemoveBlock()
		{
			Id = 0x9e;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(x);
			Write(z);
			Write(y);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			x = ReadInt();
			z = ReadInt();
			y = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeUpdateBlock : Package<McpeUpdateBlock>
	{
		public BlockRecords blocks; // = null;
		public McpeUpdateBlock()
		{
			Id = 0x9f;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(blocks);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			blocks = ReadBlockRecords();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAddPainting : Package<McpeAddPainting>
	{
		public long entityId; // = null;
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public int direction; // = null;
		public string title; // = null;
		public McpeAddPainting()
		{
			Id = 0xa0;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(x);
			Write(y);
			Write(z);
			Write(direction);
			Write(title);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			x = ReadInt();
			y = ReadInt();
			z = ReadInt();
			direction = ReadInt();
			title = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeExplode : Package<McpeExplode>
	{
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float radius; // = null;
		public Records records; // = null;
		public McpeExplode()
		{
			Id = 0xa1;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);
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

			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			radius = ReadFloat();
			records = ReadRecords();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeLevelEvent : Package<McpeLevelEvent>
	{
		public short eventId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public int data; // = null;
		public McpeLevelEvent()
		{
			Id = 0xa2;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(eventId);
			Write(x);
			Write(y);
			Write(z);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			eventId = ReadShort();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			data = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeTileEvent : Package<McpeTileEvent>
	{
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public int case1; // = null;
		public int case2; // = null;
		public McpeTileEvent()
		{
			Id = 0xa3;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);
			Write(case1);
			Write(case2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			x = ReadInt();
			y = ReadInt();
			z = ReadInt();
			case1 = ReadInt();
			case2 = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeEntityEvent : Package<McpeEntityEvent>
	{
		public long entityId; // = null;
		public byte eventId; // = null;
		public McpeEntityEvent()
		{
			Id = 0xa4;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(eventId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			eventId = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeMobEffect : Package<McpeMobEffect>
	{
		public long entityId; // = null;
		public byte eventId; // = null;
		public byte effectId; // = null;
		public byte amplifier; // = null;
		public byte particles; // = null;
		public int duration; // = null;
		public McpeMobEffect()
		{
			Id = 0xa5;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(eventId);
			Write(effectId);
			Write(amplifier);
			Write(particles);
			Write(duration);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			eventId = ReadByte();
			effectId = ReadByte();
			amplifier = ReadByte();
			particles = ReadByte();
			duration = ReadInt();

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
			Id = 0xa6;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(attributes);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			attributes = ReadPlayerAttributes();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpePlayerEquipment : Package<McpePlayerEquipment>
	{
		public long entityId; // = null;
		public MetadataSlot item; // = null;
		public byte slot; // = null;
		public byte selectedSlot; // = null;
		public McpePlayerEquipment()
		{
			Id = 0xa7;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(item);
			Write(slot);
			Write(selectedSlot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			item = ReadMetadataSlot();
			slot = ReadByte();
			selectedSlot = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpePlayerArmorEquipment : Package<McpePlayerArmorEquipment>
	{
		public long entityId; // = null;
		public MetadataSlot helmet; // = null;
		public MetadataSlot chestplate; // = null;
		public MetadataSlot leggings; // = null;
		public MetadataSlot boots; // = null;
		public McpePlayerArmorEquipment()
		{
			Id = 0xa8;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
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

			entityId = ReadLong();
			helmet = ReadMetadataSlot();
			chestplate = ReadMetadataSlot();
			leggings = ReadMetadataSlot();
			boots = ReadMetadataSlot();

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
			Id = 0xa9;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(actionId);
			Write(targetEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadByte();
			targetEntityId = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeUseItem : Package<McpeUseItem>
	{
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public byte face; // = null;
		public float fx; // = null;
		public float fy; // = null;
		public float fz; // = null;
		public float positionX; // = null;
		public float positionY; // = null;
		public float positionZ; // = null;
		public MetadataSlot item; // = null;
		public McpeUseItem()
		{
			Id = 0xaa;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);
			Write(face);
			Write(fx);
			Write(fy);
			Write(fz);
			Write(positionX);
			Write(positionY);
			Write(positionZ);
			Write(item);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			x = ReadInt();
			y = ReadInt();
			z = ReadInt();
			face = ReadByte();
			fx = ReadFloat();
			fy = ReadFloat();
			fz = ReadFloat();
			positionX = ReadFloat();
			positionY = ReadFloat();
			positionZ = ReadFloat();
			item = ReadMetadataSlot();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpePlayerAction : Package<McpePlayerAction>
	{
		public long entityId; // = null;
		public int actionId; // = null;
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public int face; // = null;
		public McpePlayerAction()
		{
			Id = 0xab;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(actionId);
			Write(x);
			Write(y);
			Write(z);
			Write(face);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			actionId = ReadInt();
			x = ReadInt();
			y = ReadInt();
			z = ReadInt();
			face = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeHurtArmor : Package<McpeHurtArmor>
	{
		public byte health; // = null;
		public McpeHurtArmor()
		{
			Id = 0xac;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			health = ReadByte();

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
			Id = 0xad;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(metadata);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			metadata = ReadMetadataDictionary();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeSetEntityMotion : Package<McpeSetEntityMotion>
	{
		public EntityMotions entities; // = null;
		public McpeSetEntityMotion()
		{
			Id = 0xae;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entities);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entities = ReadEntityMotions();

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
			Id = 0xaf;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(riderId);
			Write(riddenId);
			Write(linkType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			riderId = ReadLong();
			riddenId = ReadLong();
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
			Id = 0xb0;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			health = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeSetSpawnPosition : Package<McpeSetSpawnPosition>
	{
		public int x; // = null;
		public int z; // = null;
		public int y; // = null;
		public McpeSetSpawnPosition()
		{
			Id = 0xb1;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(x);
			Write(z);
			Write(y);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			x = ReadInt();
			z = ReadInt();
			y = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAnimate : Package<McpeAnimate>
	{
		public byte actionId; // = null;
		public long entityId; // = null;
		public McpeAnimate()
		{
			Id = 0xb2;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(actionId);
			Write(entityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadByte();
			entityId = ReadLong();

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
			Id = 0xb3;
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
		public MetadataSlot item; // = null;
		public McpeDropItem()
		{
			Id = 0xb4;
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
			item = ReadMetadataSlot();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeContainerOpen : Package<McpeContainerOpen>
	{
		public byte windowId; // = null;
		public byte type; // = null;
		public short slotCount; // = null;
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public McpeContainerOpen()
		{
			Id = 0xb5;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			Write(type);
			Write(slotCount);
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

			windowId = ReadByte();
			type = ReadByte();
			slotCount = ReadShort();
			x = ReadInt();
			y = ReadInt();
			z = ReadInt();

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
			Id = 0xb6;
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
		public short slot; // = null;
		public short unknown; // = null;
		public MetadataSlot item; // = null;
		public McpeContainerSetSlot()
		{
			Id = 0xb7;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			Write(slot);
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

			windowId = ReadByte();
			slot = ReadShort();
			unknown = ReadShort();
			item = ReadMetadataSlot();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeContainerSetData : Package<McpeContainerSetData>
	{
		public byte windowId; // = null;
		public short property; // = null;
		public short value; // = null;
		public McpeContainerSetData()
		{
			Id = 0xb8;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			Write(property);
			Write(value);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			windowId = ReadByte();
			property = ReadShort();
			value = ReadShort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeContainerSetContent : Package<McpeContainerSetContent>
	{
		public byte windowId; // = null;
		public MetadataSlots slotData; // = null;
		public MetadataInts hotbarData; // = null;
		public McpeContainerSetContent()
		{
			Id = 0xb9;
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
			slotData = ReadMetadataSlots();
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
			Id = 0xba;
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
		public int flags; // = null;
		public McpeCraftingEvent()
		{
			Id = 0xbb;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(flags);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			flags = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAdventureSettings : Package<McpeAdventureSettings>
	{
		public int flags; // = null;
		public McpeAdventureSettings()
		{
			Id = 0xbc;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(flags);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			flags = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeTileEntityData : Package<McpeTileEntityData>
	{
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public Nbt namedtag; // = null;
		public McpeTileEntityData()
		{
			Id = 0xbd;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			x = ReadInt();
			y = ReadInt();
			z = ReadInt();
			namedtag = ReadNbt();

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
		public int chunkDataLength; // = null;
		public byte[] chunkData; // = null;
		public McpeFullChunkData()
		{
			Id = 0xbf;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(chunkX);
			Write(chunkZ);
			Write(order);
			Write(chunkDataLength);
			Write(chunkData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			chunkX = ReadInt();
			chunkZ = ReadInt();
			order = ReadByte();
			chunkDataLength = ReadInt();
			chunkData = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeSetDifficulty : Package<McpeSetDifficulty>
	{
		public int difficulty; // = null;
		public McpeSetDifficulty()
		{
			Id = 0xc0;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(difficulty);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			difficulty = ReadInt();

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
			Id = 0xc3;
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

	public partial class McpeTransfer : Package<McpeTransfer>
	{
		public IPEndPoint endpoint; // = null;
		public McpeTransfer()
		{
			Id = 0x1b;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(endpoint);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			endpoint = ReadIPEndPoint();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeSpawnExperienceOrb : Package<McpeSpawnExperienceOrb>
	{
		public long entityId; // = null;
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public int count; // = null;
		public McpeSpawnExperienceOrb()
		{
			Id = 0xc5;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(x);
			Write(y);
			Write(z);
			Write(count);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadLong();
			x = ReadInt();
			y = ReadInt();
			z = ReadInt();
			count = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

}

