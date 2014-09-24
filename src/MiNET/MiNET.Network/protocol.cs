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
				case 0x01:
					return new IdUnconnectedPing();
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
				case 0xc0:
					return new Ack();
				case 0xa0:
					return new Nak();
			}

			return null;
		}
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

			Write(pingId);
			Write(offlineMessageDataId);
		}

		public override void Decode()
		{
			base.Decode();

			pingId = ReadLong();
			ReadBytes(offlineMessageDataId.Length);
		}
	}

	public partial class IdUnconnectedPong : Package
	{
		public long pingId; // = null;
		public long serverId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };

		public IdUnconnectedPong()
		{
			Id = 0x1c;
		}

		public override void Encode()
		{
			base.Encode();

			Write(pingId);
			Write(serverId);
			Write(offlineMessageDataId);
		}

		public override void Decode()
		{
			base.Decode();

			pingId = ReadLong();
			serverId = ReadLong();
			ReadBytes(offlineMessageDataId.Length);
		}
	}

	public partial class IdOpenConnectionRequest1 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public byte raknetProtocolVersion; // = null;
		public byte[] padToMtuSize; // = null;

		public IdOpenConnectionRequest1()
		{
			Id = 0x05;
		}

		public override void Encode()
		{
			base.Encode();

			Write(offlineMessageDataId);
			Write(raknetProtocolVersion);
			Write(padToMtuSize);
		}

		public override void Decode()
		{
			base.Decode();

			ReadBytes(offlineMessageDataId.Length);
			raknetProtocolVersion = ReadByte();
			padToMtuSize = ReadBytes(0);
		}
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

			Write(offlineMessageDataId);
			Write(serverGuid);
			Write(serverHasSecurity);
			Write(mtuSize);
		}

		public override void Decode()
		{
			base.Decode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();
			serverHasSecurity = ReadByte();
			mtuSize = ReadShort();
		}
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

			Write(offlineMessageDataId);
			Write(clientUdpPort);
			Write(mtuSize);
			Write(clientGuid);
		}

		public override void Decode()
		{
			base.Decode();

			ReadBytes(offlineMessageDataId.Length);
			clientUdpPort = ReadBytes(6);
			mtuSize = ReadShort();
			clientGuid = ReadLong();
		}
	}

	public partial class IdOpenConnectionReply2 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;
		public short mtuSize; // = null;
		public byte doSecurity; // = null;

		public IdOpenConnectionReply2()
		{
			Id = 0x08;
		}

		public override void Encode()
		{
			base.Encode();

			Write(offlineMessageDataId);
			Write(serverGuid);
			Write(mtuSize);
			Write(doSecurity);
		}

		public override void Decode()
		{
			base.Decode();

			ReadBytes(offlineMessageDataId.Length);
			serverGuid = ReadLong();
			mtuSize = ReadShort();
			doSecurity = ReadByte();
		}
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

			Write(clientGuid);
			Write(timestamp);
			Write(doSecurity);
		}

		public override void Decode()
		{
			base.Decode();

			clientGuid = ReadLong();
			timestamp = ReadLong();
			doSecurity = ReadByte();
		}
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

			Write(clientSystemAddress);
			Write(systemIndex);
			Write(incomingTimestamp);
			Write(serverTimestamp);
		}

		public override void Decode()
		{
			base.Decode();

			clientSystemAddress = ReadLong();
			systemIndex = ReadLong();
			incomingTimestamp = ReadLong();
			serverTimestamp = ReadLong();
		}
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

			Write(count);
			Write(onlyOneSequence);
			Write(sequenceNumber);
		}

		public override void Decode()
		{
			base.Decode();

			count = ReadShort();
			onlyOneSequence = ReadByte();
			sequenceNumber = ReadLittle();
		}
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

			Write(count);
			Write(onlyOneSequence);
			Write(sequenceNumber);
		}

		public override void Decode()
		{
			base.Decode();

			count = ReadShort();
			onlyOneSequence = ReadByte();
			sequenceNumber = ReadLittle();
		}
	}

}

