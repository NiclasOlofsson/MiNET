
//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

using little = MiNET.Network.Int24; // friendly name

namespace MiNET.Network
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
					return new IdMcpeFullChunkData();
				case 0x95:
					return new IdMcpeMovePlayer();
				case 0xb7:
					return new IdMcpeAdventureSettings();
				case 0xb4:
					return new IdMcpeContainerSetContent();
				case 0x85:
					return new IdMcpeMessage();
				case 0xa7:
					return new IdMcpeEntityData();
				case 0x89:
					return new IdMcpeAddPlayer();
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
		public int z; // = null;
		public byte y; // = null;

		public IdMcpeSetSpawnPosition()
		{
			Id = 0xab;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(x);
			Write(z);
			Write(y);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			x = ReadInt();
			z = ReadInt();
			y = ReadByte();

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
		public int spawnX; // = null;
		public int spawnZ; // = null;
		public int spawnY; // = null;
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

		public override void Decode()
		{
			base.Decode();

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

	public partial class IdMcpeFullChunkData : Package
	{
		public byte[] chunkData; // = null;

		public IdMcpeFullChunkData()
		{
			Id = 0xba;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(chunkData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			chunkData = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeMovePlayer : Package
	{
		public int entityId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float yaw; // = null;
		public float pitch; // = null;
		public float bodyYaw; // = null;
		public byte teleport; // = null;

		public IdMcpeMovePlayer()
		{
			Id = 0x95;
		}

		public override void Encode()
		{
			base.Encode();

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

		public override void Decode()
		{
			base.Decode();

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

	public partial class IdMcpeAdventureSettings : Package
	{
		public int flags; // = null;

		public IdMcpeAdventureSettings()
		{
			Id = 0xb7;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(flags);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			flags = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeContainerSetContent : Package
	{
		public byte windowId; // = null;
		public short slotCount; // = null;
		public byte[] slotData; // = null;
		public short hotbarCount; // = null;
		public byte[] hotbarData; // = null;

		public IdMcpeContainerSetContent()
		{
			Id = 0xb4;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(windowId);
			Write(slotCount);
			Write(slotData);
			Write(hotbarCount);
			Write(hotbarData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			windowId = ReadByte();
			slotCount = ReadShort();
			slotData = ReadBytes(0);
			hotbarCount = ReadShort();
			hotbarData = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeMessage : Package
	{
		public string source; // = null;
		public string message; // = null;

		public IdMcpeMessage()
		{
			Id = 0x85;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(source);
			Write(message);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			source = ReadString();
			message = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeEntityData : Package
	{
		public int x; // = null;
		public byte y; // = null;
		public int z; // = null;
		public byte[] namedtag; // = null;

		public IdMcpeEntityData()
		{
			Id = 0xa7;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			x = ReadInt();
			y = ReadByte();
			z = ReadInt();
			namedtag = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	public partial class IdMcpeAddPlayer : Package
	{
		public long clientId; // = null;
		public string username; // = null;
		public int entityId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public byte yaw; // = null;
		public byte pitch; // = null;
		public short unknown1; // = null;
		public short unknown2; // = null;
		public byte[] metadata; // = null;

		public IdMcpeAddPlayer()
		{
			Id = 0x89;
		}

		public override void Encode()
		{
			base.Encode();

			BeforeEncode();

			Write(clientId);
			Write(username);
			Write(entityId);
			Write(x);
			Write(y);
			Write(z);
			Write(yaw);
			Write(pitch);
			Write(unknown1);
			Write(unknown2);
			Write(metadata);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		public override void Decode()
		{
			base.Decode();

			BeforeDecode();

			clientId = ReadLong();
			username = ReadString();
			entityId = ReadInt();
			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			yaw = ReadByte();
			pitch = ReadByte();
			unknown1 = ReadShort();
			unknown2 = ReadShort();
			metadata = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

}

