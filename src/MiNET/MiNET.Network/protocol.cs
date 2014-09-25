
//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

using System;
//using System.IO;
using little = MiNET.Int24; // friendly name

namespace MiNET
{

	public class PackageFactory
	{
		public static Package CreatePackage(byte messageId)
		{
			switch (messageId)
			{
				case 0x00:
					return new IdConnectedPing();
				case 0x01:
					return new IdUnconnectedPing();
				case 0x03:
					return new IdConnectedPong();
				case 0xc0:
					return new Ack();
				case 0xa0:
					return new Nak();
				case 0x1c:
					return new IdUnconnectedPong();
				case 0x05:
					return new IdOpenConnectionRequest1();
				case 0x06:
					return new IdOpenConnectionReply1();
				case 0x07:
					return new IdOpenConnectionRequest2();
				case 0x08:
					return new IdOpenConnectionReply2();
				case 0x09:
					return new IdConnectionRequest();
				case 0x10:
					return new IdConnectionRequestAccepted();
				case 0x13:
					return new IdNewIncomingConnection();
				case 0x82:
					return new IdMcpeLogin();
				case 0x83:
					return new IdMcpeLoginStatus();
				case 0x84:
					return new IdMcpeReady();
				case 0x86:
					return new IdMcpeSetTime();
				case 0xaa:
					return new IdMcpeSetHealth();
				case 0xab:
					return new IdMcpeSetSpawnPosition();
				case 0x87:
					return new IdMcpeStartGame();
				case 0xba:
					return new IdMcpeFullChunkDataPacket();
			}

			return null;
		}
	}

	public partial class IdConnectedPing : Package
	{
		public long sendpingtime; // = null;

		public IdConnectedPing()
		{
			Id = 0x00;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(sendpingtime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			sendpingtime = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdUnconnectedPing : Package
	{
		public long pingId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };

		public IdUnconnectedPing()
		{
			Id = 0x01;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(pingId);
			Write(offlineMessageDataId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			pingId = ReadLong();
			ReadBytes(offlineMessageDataId.Length);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdConnectedPong : Package
	{
		public long sendpingtime; // = null;
		public long sendpongtime; // = null;

		public IdConnectedPong()
		{
			Id = 0x03;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(sendpingtime);
			Write(sendpongtime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			sendpingtime = ReadLong();
			sendpongtime = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class Ack : Package
	{
		public short count; // = null;
		public byte onlyOneSequence; // = null;
		public little sequenceNumber; // = null;

		public Ack()
		{
			Id = 0xc0;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(count);
			Write(onlyOneSequence);
			Write(sequenceNumber);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			count = ReadShort();
			onlyOneSequence = ReadByte();
			sequenceNumber = ReadLittle();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class Nak : Package
	{
		public short count; // = null;
		public byte onlyOneSequence; // = null;
		public little sequenceNumber; // = null;

		public Nak()
		{
			Id = 0xa0;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(count);
			Write(onlyOneSequence);
			Write(sequenceNumber);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			count = ReadShort();
			onlyOneSequence = ReadByte();
			sequenceNumber = ReadLittle();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdUnconnectedPong : Package
	{
		public long pingId; // = null;
		public long serverId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public string serverName; // = null;

		public IdUnconnectedPong()
		{
			Id = 0x1c;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(pingId);
			Write(serverId);
			Write(offlineMessageDataId);
			Write(serverName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

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

	public partial class IdOpenConnectionRequest1 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public byte raknetProtocolVersion; // = null;

		public IdOpenConnectionRequest1()
		{
			Id = 0x05;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(raknetProtocolVersion);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			raknetProtocolVersion = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdOpenConnectionReply1 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;
		public byte serverHasSecurity; // = null;
		public short mtuSize; // = null;

		public IdOpenConnectionReply1()
		{
			Id = 0x06;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(serverGuid);
			Write(serverHasSecurity);
			Write(mtuSize);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

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

	public partial class IdOpenConnectionRequest2 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public byte[] clientUdpPort; // = null;
		public short mtuSize; // = null;
		public long clientGuid; // = null;

		public IdOpenConnectionRequest2()
		{
			Id = 0x07;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(clientUdpPort);
			Write(mtuSize);
			Write(clientGuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			clientUdpPort = ReadBytes(6);
			mtuSize = ReadShort();
			clientGuid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdOpenConnectionReply2 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;
		public short clientUdpPort; // = null;
		public short mtuSize; // = null;
		public byte doSecurity; // = null;

		public IdOpenConnectionReply2()
		{
			Id = 0x08;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(offlineMessageDataId);
			Write(serverGuid);
			Write(clientUdpPort);
			Write(mtuSize);
			Write(doSecurity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();
			clientUdpPort = ReadShort();
			mtuSize = ReadShort();
			doSecurity = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdConnectionRequest : Package
	{
		public long clientGuid; // = null;
		public long timestamp; // = null;
		public byte doSecurity; // = null;

		public IdConnectionRequest()
		{
			Id = 0x09;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(clientGuid);
			Write(timestamp);
			Write(doSecurity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			clientGuid = ReadLong();
			timestamp = ReadLong();
			doSecurity = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdConnectionRequestAccepted : Package
	{
		public long clientSystemAddress; // = null;
		public long systemIndex; // = null;
		public long incomingTimestamp; // = null;
		public long serverTimestamp; // = null;

		public IdConnectionRequestAccepted()
		{
			Id = 0x10;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(clientSystemAddress);
			Write(systemIndex);
			Write(incomingTimestamp);
			Write(serverTimestamp);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

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

	public partial class IdNewIncomingConnection : Package
	{
		public int cookie; // = null;
		public byte doSecurity; // = null;
		public short port; // = null;
		public long session; // = null;
		public long session2; // = null;

		public IdNewIncomingConnection()
		{
			Id = 0x13;
		}

		public override void Encode()
		{
			base.Encode();

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

		public override void Decode()
		{
			base.Decode();

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

	public partial class IdMcpeLogin : Package
	{
		public string username; // = null;
		public int protocol; // = null;
		public int protocol2; // = null;
		public int clientId; // = null;
		public string logindata; // = null;

		public IdMcpeLogin()
		{
			Id = 0x82;
		}

		public override void Encode()
		{
			base.Encode();

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

		public override void Decode()
		{
			base.Decode();

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

	public partial class IdMcpeLoginStatus : Package
	{
		public int status; // = null;

		public IdMcpeLoginStatus()
		{
			Id = 0x83;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(status);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			status = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeReady : Package
	{

		public IdMcpeReady()
		{
			Id = 0x84;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeSetTime : Package
	{
		public int time; // = null;
		public byte started; // = null;

		public IdMcpeSetTime()
		{
			Id = 0x86;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(time);
			Write(started);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			time = ReadInt();
			started = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeSetHealth : Package
	{
		public byte health; // = null;

		public IdMcpeSetHealth()
		{
			Id = 0xaa;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			health = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeSetSpawnPosition : Package
	{
		public int x; // = null;
		public int y; // = null;
		public byte z; // = null;

		public IdMcpeSetSpawnPosition()
		{
			Id = 0xab;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			x = ReadInt();
			y = ReadInt();
			z = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeStartGame : Package
	{
		public int seed; // = null;
		public int generator; // = null;
		public int gamemode; // = null;
		public int entityId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;

		public IdMcpeStartGame()
		{
			Id = 0x87;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(seed);
			Write(generator);
			Write(gamemode);
			Write(entityId);
			Write(x);
			Write(y);
			Write(z);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			seed = ReadInt();
			generator = ReadInt();
			gamemode = ReadInt();
			entityId = ReadInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeFullChunkDataPacket : Package
	{
		public int chunkX; // = null;
		public byte chunkZ; // = null;
		public byte[] chunkData; // = null;

		public IdMcpeFullChunkDataPacket()
		{
			Id = 0xba;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(chunkX);
			Write(chunkZ);
			Write(chunkData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			chunkX = ReadInt();
			chunkZ = ReadByte();
			chunkData = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

}

