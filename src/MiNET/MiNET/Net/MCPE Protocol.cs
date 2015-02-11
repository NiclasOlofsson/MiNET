﻿
//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

using System;
using System.Threading;
using MiNET.Utils; 
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
				case 0x15:
					package = DisconnectionNotification.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x82:
					package = McpeLogin.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x83:
					package = McpeLoginStatus.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x86:
					package = McpeSetTime.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xaa:
					package = McpeSetHealth.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xab:
					package = McpeSetSpawnPosition.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xad:
					package = McpeRespawn.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x87:
					package = McpeStartGame.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xba:
					package = McpeFullChunkData.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x95:
					package = McpeMovePlayer.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb7:
					package = McpeAdventureSettings.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb4:
					package = McpeContainerSetContent.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xbc:
					package = McpeSetDifficulty.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x85:
					package = McpeMessage.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa7:
					package = McpeSetEntityData.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb8:
					package = McpeEntityData.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x89:
					package = McpeAddPlayer.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x8a:
					package = McpeRemovePlayer.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x8c:
					package = McpeAddEntity.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x8d:
					package = McpeRemoveEntity.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x96:
					package = McpePlaceBlock.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x97:
					package = McpeRemoveBlock.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x98:
					package = McpeUpdateBlock.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x9a:
					package = McpeExplode.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x9c:
					package = McpeTileEvent.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x9d:
					package = McpeEntityEvent.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa0:
					package = McpePlayerEquipment.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa1:
					package = McpePlayerArmorEquipment.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa2:
					package = McpeInteract.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa4:
					package = McpePlayerAction.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xac:
					package = McpeAnimate.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xa3:
					package = McpeUseItem.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb0:
					package = McpeContainerOpen.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb1:
					package = McpeContainerClose.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xb2:
					package = McpeContainerSetSlot.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x8e:
					package = McpeItemEntity.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0x8f:
					package = McpeRemoveItemEntity.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
				case 0xaf:
					package = McpeDropItem.CreateObject();
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
		public byte serverSecurity; // = null;
		public byte[] cookie; // = null;
		public short clientUdpPort; // = null;
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
			Write(serverSecurity);
			Write(cookie);
			Write(clientUdpPort);
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
			serverSecurity = ReadByte();
			cookie = ReadBytes(4);
			clientUdpPort = ReadShort();
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
		public long clientSystemAddress; // = null;
		public long systemIndex; // = null;
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

			Write(clientSystemAddress);
			Write(systemIndex);
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

			clientSystemAddress = ReadLong();
			systemIndex = ReadLong();
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

	public partial class McpeLogin : Package<McpeLogin>
	{
		public string username; // = null;
		public int protocol; // = null;
		public int protocol2; // = null;
		public int clientId; // = null;
		public string logindata; // = null;
		public McpeLogin()
		{
			Id = 0x82;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(username);
			Write(protocol);
			Write(protocol2);
			Write(clientId);
			Write(logindata);

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
			clientId = ReadInt();
			logindata = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeLoginStatus : Package<McpeLoginStatus>
	{
		public int status; // = null;
		public McpeLoginStatus()
		{
			Id = 0x83;
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

	public partial class McpeSetTime : Package<McpeSetTime>
	{
		public int time; // = null;
		public byte started; // = null;
		public McpeSetTime()
		{
			Id = 0x86;
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

	public partial class McpeSetHealth : Package<McpeSetHealth>
	{
		public byte health; // = null;
		public McpeSetHealth()
		{
			Id = 0xaa;
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

	public partial class McpeSetSpawnPosition : Package<McpeSetSpawnPosition>
	{
		public int x; // = null;
		public int z; // = null;
		public byte y; // = null;
		public McpeSetSpawnPosition()
		{
			Id = 0xab;
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
			y = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeRespawn : Package<McpeRespawn>
	{
		public int entityId; // = null;
		public float x; // = null;
		public float z; // = null;
		public float y; // = null;
		public McpeRespawn()
		{
			Id = 0xad;
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

			entityId = ReadInt();
			x = ReadFloat();
			z = ReadFloat();
			y = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeStartGame : Package<McpeStartGame>
	{
		public int seed; // = null;
		public int generator; // = null;
		public int gamemode; // = null;
		public int entityId; // = null;
		public int spawnX; // = null;
		public int spawnZ; // = null;
		public int spawnY; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public McpeStartGame()
		{
			Id = 0x87;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(seed);
			Write(generator);
			Write(gamemode);
			Write(entityId);
			Write(spawnX);
			Write(spawnZ);
			Write(spawnY);
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

			seed = ReadInt();
			generator = ReadInt();
			gamemode = ReadInt();
			entityId = ReadInt();
			spawnX = ReadInt();
			spawnZ = ReadInt();
			spawnY = ReadInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeFullChunkData : Package<McpeFullChunkData>
	{
		public byte[] chunkData; // = null;
		public McpeFullChunkData()
		{
			Id = 0xba;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(chunkData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			chunkData = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeMovePlayer : Package<McpeMovePlayer>
	{
		public int entityId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float yaw; // = null;
		public float pitch; // = null;
		public float bodyYaw; // = null;
		public byte teleport; // = null;
		public McpeMovePlayer()
		{
			Id = 0x95;
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
			Write(pitch);
			Write(bodyYaw);
			Write(teleport);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			yaw = ReadFloat();
			pitch = ReadFloat();
			bodyYaw = ReadFloat();
			teleport = ReadByte();

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
			Id = 0xb7;
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

	public partial class McpeContainerSetContent : Package<McpeContainerSetContent>
	{
		public byte windowId; // = null;
		public MetadataSlots slotData; // = null;
		public MetadataInts hotbarData; // = null;
		public McpeContainerSetContent()
		{
			Id = 0xb4;
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

	public partial class McpeSetDifficulty : Package<McpeSetDifficulty>
	{
		public int difficulty; // = null;
		public McpeSetDifficulty()
		{
			Id = 0xbc;
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

	public partial class McpeMessage : Package<McpeMessage>
	{
		public string source; // = null;
		public string message; // = null;
		public McpeMessage()
		{
			Id = 0x85;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(source);
			Write(message);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			source = ReadString();
			message = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeSetEntityData : Package<McpeSetEntityData>
	{
		public int entityId; // = null;
		public byte[] namedtag; // = null;
		public McpeSetEntityData()
		{
			Id = 0xa7;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadInt();
			namedtag = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeEntityData : Package<McpeEntityData>
	{
		public int x; // = null;
		public byte y; // = null;
		public int z; // = null;
		public Nbt namedtag; // = null;
		public McpeEntityData()
		{
			Id = 0xb8;
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
			y = ReadByte();
			z = ReadInt();
			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAddPlayer : Package<McpeAddPlayer>
	{
		public long clientId; // = null;
		public string username; // = null;
		public int entityId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public byte yaw; // = null;
		public byte pitch; // = null;
		public short item; // = null;
		public short meta; // = null;
		public byte[] metadata; // = null;
		public McpeAddPlayer()
		{
			Id = 0x89;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(clientId);
			Write(username);
			Write(entityId);
			Write(x);
			Write(y);
			Write(z);
			Write(yaw);
			Write(pitch);
			Write(item);
			Write(meta);
			Write(metadata);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			clientId = ReadLong();
			username = ReadString();
			entityId = ReadInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			yaw = ReadByte();
			pitch = ReadByte();
			item = ReadShort();
			meta = ReadShort();
			metadata = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeRemovePlayer : Package<McpeRemovePlayer>
	{
		public int entityId; // = null;
		public long clientId; // = null;
		public McpeRemovePlayer()
		{
			Id = 0x8a;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(clientId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadInt();
			clientId = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAddEntity : Package<McpeAddEntity>
	{
		public int entityId; // = null;
		public int entityType; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public int did; // = null;
		public short velocityX; // = null;
		public short velocityZ; // = null;
		public short velocityY; // = null;
		public McpeAddEntity()
		{
			Id = 0x8c;
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
			Write(did);
			Write(velocityX);
			Write(velocityZ);
			Write(velocityY);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadInt();
			entityType = ReadInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			did = ReadInt();
			velocityX = ReadShort();
			velocityZ = ReadShort();
			velocityY = ReadShort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeRemoveEntity : Package<McpeRemoveEntity>
	{
		public int entityId; // = null;
		public McpeRemoveEntity()
		{
			Id = 0x8d;
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

			entityId = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpePlaceBlock : Package<McpePlaceBlock>
	{
		public int entityId; // = null;
		public int x; // = null;
		public int z; // = null;
		public byte y; // = null;
		public byte block; // = null;
		public byte meta; // = null;
		public byte face; // = null;
		public McpePlaceBlock()
		{
			Id = 0x96;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(x);
			Write(z);
			Write(y);
			Write(block);
			Write(meta);
			Write(face);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadInt();
			x = ReadInt();
			z = ReadInt();
			y = ReadByte();
			block = ReadByte();
			meta = ReadByte();
			face = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeRemoveBlock : Package<McpeRemoveBlock>
	{
		public int entityId; // = null;
		public int x; // = null;
		public int z; // = null;
		public byte y; // = null;
		public McpeRemoveBlock()
		{
			Id = 0x97;
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

			entityId = ReadInt();
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
		public int x; // = null;
		public int z; // = null;
		public byte y; // = null;
		public byte block; // = null;
		public byte meta; // = null;
		public McpeUpdateBlock()
		{
			Id = 0x98;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(x);
			Write(z);
			Write(y);
			Write(block);
			Write(meta);

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
			y = ReadByte();
			block = ReadByte();
			meta = ReadByte();

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
			Id = 0x9a;
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

	public partial class McpeTileEvent : Package<McpeTileEvent>
	{
		public int x; // = null;
		public int z; // = null;
		public int y; // = null;
		public int case1; // = null;
		public int case2; // = null;
		public McpeTileEvent()
		{
			Id = 0x9c;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(x);
			Write(z);
			Write(y);
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
			z = ReadInt();
			y = ReadInt();
			case1 = ReadInt();
			case2 = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeEntityEvent : Package<McpeEntityEvent>
	{
		public int entityId; // = null;
		public byte eventId; // = null;
		public McpeEntityEvent()
		{
			Id = 0x9d;
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

			entityId = ReadInt();
			eventId = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpePlayerEquipment : Package<McpePlayerEquipment>
	{
		public int entityId; // = null;
		public short item; // = null;
		public short meta; // = null;
		public byte slot; // = null;
		public McpePlayerEquipment()
		{
			Id = 0xa0;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
			Write(item);
			Write(meta);
			Write(slot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadInt();
			item = ReadShort();
			meta = ReadShort();
			slot = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpePlayerArmorEquipment : Package<McpePlayerArmorEquipment>
	{
		public int entityId; // = null;
		public byte helmet; // = null;
		public byte chestplate; // = null;
		public byte leggings; // = null;
		public byte boots; // = null;
		public McpePlayerArmorEquipment()
		{
			Id = 0xa1;
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

			entityId = ReadInt();
			helmet = ReadByte();
			chestplate = ReadByte();
			leggings = ReadByte();
			boots = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeInteract : Package<McpeInteract>
	{
		public byte actionId; // = null;
		public int entityId; // = null;
		public int targetEntityId; // = null;
		public McpeInteract()
		{
			Id = 0xa2;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(actionId);
			Write(entityId);
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
			entityId = ReadInt();
			targetEntityId = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpePlayerAction : Package<McpePlayerAction>
	{
		public int actionId; // = null;
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public int face; // = null;
		public int entityId; // = null;
		public McpePlayerAction()
		{
			Id = 0xa4;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(actionId);
			Write(x);
			Write(y);
			Write(z);
			Write(face);
			Write(entityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			actionId = ReadInt();
			x = ReadInt();
			y = ReadInt();
			z = ReadInt();
			face = ReadInt();
			entityId = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeAnimate : Package<McpeAnimate>
	{
		public byte actionId; // = null;
		public int entityId; // = null;
		public McpeAnimate()
		{
			Id = 0xac;
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
			entityId = ReadInt();

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
		public short item; // = null;
		public short meta; // = null;
		public int entityId; // = null;
		public float fx; // = null;
		public float fy; // = null;
		public float fz; // = null;
		public float positionX; // = null;
		public float positionY; // = null;
		public float positionZ; // = null;
		public McpeUseItem()
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
			Write(face);
			Write(item);
			Write(meta);
			Write(entityId);
			Write(fx);
			Write(fy);
			Write(fz);
			Write(positionX);
			Write(positionY);
			Write(positionZ);

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
			item = ReadShort();
			meta = ReadShort();
			entityId = ReadInt();
			fx = ReadFloat();
			fy = ReadFloat();
			fz = ReadFloat();
			positionX = ReadFloat();
			positionY = ReadFloat();
			positionZ = ReadFloat();

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
			Id = 0xb0;
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
			Id = 0xb1;
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
		public short itemId; // = null;
		public byte itemCount; // = null;
		public short itemDamage; // = null;
		public McpeContainerSetSlot()
		{
			Id = 0xb2;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(windowId);
			Write(slot);
			Write(itemId);
			Write(itemCount);
			Write(itemDamage);

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
			itemId = ReadShort();
			itemCount = ReadByte();
			itemDamage = ReadShort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeItemEntity : Package<McpeItemEntity>
	{
		public int entityId; // = null;
		public MetadataSlot item; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public byte yaw; // = null;
		public byte pitch; // = null;
		public byte roll; // = null;
		public McpeItemEntity()
		{
			Id = 0x8e;
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
			Write(yaw);
			Write(pitch);
			Write(roll);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			entityId = ReadInt();
			item = ReadMetadataSlot();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			yaw = ReadByte();
			pitch = ReadByte();
			roll = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeRemoveItemEntity : Package<McpeRemoveItemEntity>
	{
		public int target; // = null;
		public int entityId; // = null;
		public McpeRemoveItemEntity()
		{
			Id = 0x8f;
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

			target = ReadInt();
			entityId = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class McpeDropItem : Package<McpeDropItem>
	{
		public int entityId; // = null;
		public byte unknown; // = null;
		public MetadataSlot item; // = null;
		public McpeDropItem()
		{
			Id = 0xaf;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(entityId);
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

			entityId = ReadInt();
			unknown = ReadByte();
			item = ReadMetadataSlot();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

}

