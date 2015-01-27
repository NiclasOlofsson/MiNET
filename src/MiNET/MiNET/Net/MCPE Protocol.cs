
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
				case 0xb2:
					package = McpeContainerSetSlot.CreateObject();
					//package.Timer.Start();
					package.Decode(buffer);
					return package;
			}

			return null;
		}
	}

	public partial class ConnectedPing : Package
	{
		public long sendpingtime; // = null;

		public ConnectedPing(bool pooled = false)
		{
			Id = 0x00;
			_isPooled = pooled;
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

		private static readonly ObjectPool<ConnectedPing> _pool = 
			new ObjectPool<ConnectedPing>(() => new ConnectedPing(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static ConnectedPing CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static ConnectedPing()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class UnconnectedPing : Package
	{
		public long pingId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };

		public UnconnectedPing(bool pooled = false)
		{
			Id = 0x01;
			_isPooled = pooled;
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

		private static readonly ObjectPool<UnconnectedPing> _pool = 
			new ObjectPool<UnconnectedPing>(() => new UnconnectedPing(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static UnconnectedPing CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static UnconnectedPing()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class ConnectedPong : Package
	{
		public long sendpingtime; // = null;
		public long sendpongtime; // = null;

		public ConnectedPong(bool pooled = false)
		{
			Id = 0x03;
			_isPooled = pooled;
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

		private static readonly ObjectPool<ConnectedPong> _pool = 
			new ObjectPool<ConnectedPong>(() => new ConnectedPong(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static ConnectedPong CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static ConnectedPong()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class UnconnectedPong : Package
	{
		public long pingId; // = null;
		public long serverId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public string serverName; // = null;

		public UnconnectedPong(bool pooled = false)
		{
			Id = 0x1c;
			_isPooled = pooled;
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

		private static readonly ObjectPool<UnconnectedPong> _pool = 
			new ObjectPool<UnconnectedPong>(() => new UnconnectedPong(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static UnconnectedPong CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static UnconnectedPong()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class OpenConnectionRequest1 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public byte raknetProtocolVersion; // = null;

		public OpenConnectionRequest1(bool pooled = false)
		{
			Id = 0x05;
			_isPooled = pooled;
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

		private static readonly ObjectPool<OpenConnectionRequest1> _pool = 
			new ObjectPool<OpenConnectionRequest1>(() => new OpenConnectionRequest1(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static OpenConnectionRequest1 CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static OpenConnectionRequest1()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class OpenConnectionReply1 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;
		public byte serverHasSecurity; // = null;
		public short mtuSize; // = null;

		public OpenConnectionReply1(bool pooled = false)
		{
			Id = 0x06;
			_isPooled = pooled;
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

		private static readonly ObjectPool<OpenConnectionReply1> _pool = 
			new ObjectPool<OpenConnectionReply1>(() => new OpenConnectionReply1(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static OpenConnectionReply1 CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static OpenConnectionReply1()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class OpenConnectionRequest2 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public byte serverSecurity; // = null;
		public byte[] cookie; // = null;
		public short clientUdpPort; // = null;
		public short mtuSize; // = null;
		public long clientGuid; // = null;

		public OpenConnectionRequest2(bool pooled = false)
		{
			Id = 0x07;
			_isPooled = pooled;
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

		private static readonly ObjectPool<OpenConnectionRequest2> _pool = 
			new ObjectPool<OpenConnectionRequest2>(() => new OpenConnectionRequest2(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static OpenConnectionRequest2 CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static OpenConnectionRequest2()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class OpenConnectionReply2 : Package
	{
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long serverGuid; // = null;
		public short mtuSize; // = null;
		public byte[] doSecurityAndHandshake; // = null;

		public OpenConnectionReply2(bool pooled = false)
		{
			Id = 0x08;
			_isPooled = pooled;
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

		private static readonly ObjectPool<OpenConnectionReply2> _pool = 
			new ObjectPool<OpenConnectionReply2>(() => new OpenConnectionReply2(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static OpenConnectionReply2 CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static OpenConnectionReply2()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class ConnectionRequest : Package
	{
		public long clientGuid; // = null;
		public long timestamp; // = null;
		public byte doSecurity; // = null;

		public ConnectionRequest(bool pooled = false)
		{
			Id = 0x09;
			_isPooled = pooled;
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

		private static readonly ObjectPool<ConnectionRequest> _pool = 
			new ObjectPool<ConnectionRequest>(() => new ConnectionRequest(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static ConnectionRequest CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static ConnectionRequest()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class ConnectionRequestAccepted : Package
	{
		public long clientSystemAddress; // = null;
		public long systemIndex; // = null;
		public long incomingTimestamp; // = null;
		public long serverTimestamp; // = null;

		public ConnectionRequestAccepted(bool pooled = false)
		{
			Id = 0x10;
			_isPooled = pooled;
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

		private static readonly ObjectPool<ConnectionRequestAccepted> _pool = 
			new ObjectPool<ConnectionRequestAccepted>(() => new ConnectionRequestAccepted(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static ConnectionRequestAccepted CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static ConnectionRequestAccepted()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class NewIncomingConnection : Package
	{
		public int cookie; // = null;
		public byte doSecurity; // = null;
		public short port; // = null;
		public long session; // = null;
		public long session2; // = null;

		public NewIncomingConnection(bool pooled = false)
		{
			Id = 0x13;
			_isPooled = pooled;
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

		private static readonly ObjectPool<NewIncomingConnection> _pool = 
			new ObjectPool<NewIncomingConnection>(() => new NewIncomingConnection(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static NewIncomingConnection CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static NewIncomingConnection()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class DisconnectionNotification : Package
	{

		public DisconnectionNotification(bool pooled = false)
		{
			Id = 0x15;
			_isPooled = pooled;
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

		private static readonly ObjectPool<DisconnectionNotification> _pool = 
			new ObjectPool<DisconnectionNotification>(() => new DisconnectionNotification(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static DisconnectionNotification CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static DisconnectionNotification()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeLogin : Package
	{
		public string username; // = null;
		public int protocol; // = null;
		public int protocol2; // = null;
		public int clientId; // = null;
		public string logindata; // = null;

		public McpeLogin(bool pooled = false)
		{
			Id = 0x82;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeLogin> _pool = 
			new ObjectPool<McpeLogin>(() => new McpeLogin(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeLogin CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeLogin()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeLoginStatus : Package
	{
		public int status; // = null;

		public McpeLoginStatus(bool pooled = false)
		{
			Id = 0x83;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeLoginStatus> _pool = 
			new ObjectPool<McpeLoginStatus>(() => new McpeLoginStatus(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeLoginStatus CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeLoginStatus()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeSetTime : Package
	{
		public int time; // = null;
		public byte started; // = null;

		public McpeSetTime(bool pooled = false)
		{
			Id = 0x86;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeSetTime> _pool = 
			new ObjectPool<McpeSetTime>(() => new McpeSetTime(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeSetTime CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeSetTime()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeSetHealth : Package
	{
		public byte health; // = null;

		public McpeSetHealth(bool pooled = false)
		{
			Id = 0xaa;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeSetHealth> _pool = 
			new ObjectPool<McpeSetHealth>(() => new McpeSetHealth(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeSetHealth CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeSetHealth()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeSetSpawnPosition : Package
	{
		public int x; // = null;
		public int z; // = null;
		public byte y; // = null;

		public McpeSetSpawnPosition(bool pooled = false)
		{
			Id = 0xab;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeSetSpawnPosition> _pool = 
			new ObjectPool<McpeSetSpawnPosition>(() => new McpeSetSpawnPosition(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeSetSpawnPosition CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeSetSpawnPosition()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeRespawn : Package
	{
		public int entityId; // = null;
		public float x; // = null;
		public float z; // = null;
		public float y; // = null;

		public McpeRespawn(bool pooled = false)
		{
			Id = 0xad;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeRespawn> _pool = 
			new ObjectPool<McpeRespawn>(() => new McpeRespawn(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeRespawn CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeRespawn()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeStartGame : Package
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

		public McpeStartGame(bool pooled = false)
		{
			Id = 0x87;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeStartGame> _pool = 
			new ObjectPool<McpeStartGame>(() => new McpeStartGame(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeStartGame CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeStartGame()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeFullChunkData : Package
	{
		public byte[] chunkData; // = null;

		public McpeFullChunkData(bool pooled = false)
		{
			Id = 0xba;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeFullChunkData> _pool = 
			new ObjectPool<McpeFullChunkData>(() => new McpeFullChunkData(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeFullChunkData CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeFullChunkData()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeMovePlayer : Package
	{
		public int entityId; // = null;
		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public float yaw; // = null;
		public float pitch; // = null;
		public float bodyYaw; // = null;
		public byte teleport; // = null;

		public McpeMovePlayer(bool pooled = false)
		{
			Id = 0x95;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeMovePlayer> _pool = 
			new ObjectPool<McpeMovePlayer>(() => new McpeMovePlayer(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeMovePlayer CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeMovePlayer()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeAdventureSettings : Package
	{
		public int flags; // = null;

		public McpeAdventureSettings(bool pooled = false)
		{
			Id = 0xb7;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeAdventureSettings> _pool = 
			new ObjectPool<McpeAdventureSettings>(() => new McpeAdventureSettings(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeAdventureSettings CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeAdventureSettings()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeContainerSetContent : Package
	{
		public byte windowId; // = null;
		public MetadataSlots slotData; // = null;
		public MetadataInts hotbarData; // = null;

		public McpeContainerSetContent(bool pooled = false)
		{
			Id = 0xb4;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeContainerSetContent> _pool = 
			new ObjectPool<McpeContainerSetContent>(() => new McpeContainerSetContent(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeContainerSetContent CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeContainerSetContent()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeSetDifficulty : Package
	{
		public int difficulty; // = null;

		public McpeSetDifficulty(bool pooled = false)
		{
			Id = 0xbc;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeSetDifficulty> _pool = 
			new ObjectPool<McpeSetDifficulty>(() => new McpeSetDifficulty(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeSetDifficulty CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeSetDifficulty()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeMessage : Package
	{
		public string source; // = null;
		public string message; // = null;

		public McpeMessage(bool pooled = false)
		{
			Id = 0x85;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeMessage> _pool = 
			new ObjectPool<McpeMessage>(() => new McpeMessage(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeMessage CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeMessage()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeSetEntityData : Package
	{
		public int entityId; // = null;
		public byte[] namedtag; // = null;

		public McpeSetEntityData(bool pooled = false)
		{
			Id = 0xa7;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeSetEntityData> _pool = 
			new ObjectPool<McpeSetEntityData>(() => new McpeSetEntityData(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeSetEntityData CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeSetEntityData()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeEntityData : Package
	{
		public int x; // = null;
		public byte y; // = null;
		public int z; // = null;
		public byte[] namedtag; // = null;

		public McpeEntityData(bool pooled = false)
		{
			Id = 0xb8;
			_isPooled = pooled;
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
			namedtag = ReadBytes(0);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		private static readonly ObjectPool<McpeEntityData> _pool = 
			new ObjectPool<McpeEntityData>(() => new McpeEntityData(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeEntityData CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeEntityData()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeAddPlayer : Package
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

		public McpeAddPlayer(bool pooled = false)
		{
			Id = 0x89;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeAddPlayer> _pool = 
			new ObjectPool<McpeAddPlayer>(() => new McpeAddPlayer(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeAddPlayer CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeAddPlayer()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeRemovePlayer : Package
	{
		public int entityId; // = null;
		public long clientId; // = null;

		public McpeRemovePlayer(bool pooled = false)
		{
			Id = 0x8a;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeRemovePlayer> _pool = 
			new ObjectPool<McpeRemovePlayer>(() => new McpeRemovePlayer(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeRemovePlayer CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeRemovePlayer()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpePlaceBlock : Package
	{
		public int entityId; // = null;
		public int x; // = null;
		public int z; // = null;
		public byte y; // = null;
		public byte block; // = null;
		public byte meta; // = null;
		public byte face; // = null;

		public McpePlaceBlock(bool pooled = false)
		{
			Id = 0x96;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpePlaceBlock> _pool = 
			new ObjectPool<McpePlaceBlock>(() => new McpePlaceBlock(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpePlaceBlock CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpePlaceBlock()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeRemoveBlock : Package
	{
		public int entityId; // = null;
		public int x; // = null;
		public int z; // = null;
		public byte y; // = null;

		public McpeRemoveBlock(bool pooled = false)
		{
			Id = 0x97;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeRemoveBlock> _pool = 
			new ObjectPool<McpeRemoveBlock>(() => new McpeRemoveBlock(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeRemoveBlock CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeRemoveBlock()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeUpdateBlock : Package
	{
		public int x; // = null;
		public int z; // = null;
		public byte y; // = null;
		public byte block; // = null;
		public byte meta; // = null;

		public McpeUpdateBlock(bool pooled = false)
		{
			Id = 0x98;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeUpdateBlock> _pool = 
			new ObjectPool<McpeUpdateBlock>(() => new McpeUpdateBlock(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeUpdateBlock CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeUpdateBlock()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeEntityEvent : Package
	{
		public int entityId; // = null;
		public byte eventId; // = null;

		public McpeEntityEvent(bool pooled = false)
		{
			Id = 0x9d;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeEntityEvent> _pool = 
			new ObjectPool<McpeEntityEvent>(() => new McpeEntityEvent(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeEntityEvent CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeEntityEvent()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpePlayerEquipment : Package
	{
		public int entityId; // = null;
		public short item; // = null;
		public short meta; // = null;
		public byte slot; // = null;

		public McpePlayerEquipment(bool pooled = false)
		{
			Id = 0xa0;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpePlayerEquipment> _pool = 
			new ObjectPool<McpePlayerEquipment>(() => new McpePlayerEquipment(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpePlayerEquipment CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpePlayerEquipment()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpePlayerArmorEquipment : Package
	{
		public int entityId; // = null;
		public byte helmet; // = null;
		public byte chestplate; // = null;
		public byte leggings; // = null;
		public byte boots; // = null;

		public McpePlayerArmorEquipment(bool pooled = false)
		{
			Id = 0xa1;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpePlayerArmorEquipment> _pool = 
			new ObjectPool<McpePlayerArmorEquipment>(() => new McpePlayerArmorEquipment(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpePlayerArmorEquipment CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpePlayerArmorEquipment()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeInteract : Package
	{
		public byte actionId; // = null;
		public int entityId; // = null;
		public int targetEntityId; // = null;

		public McpeInteract(bool pooled = false)
		{
			Id = 0xa2;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeInteract> _pool = 
			new ObjectPool<McpeInteract>(() => new McpeInteract(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeInteract CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeInteract()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeAnimate : Package
	{
		public byte actionId; // = null;
		public int entityId; // = null;

		public McpeAnimate(bool pooled = false)
		{
			Id = 0xac;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeAnimate> _pool = 
			new ObjectPool<McpeAnimate>(() => new McpeAnimate(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeAnimate CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeAnimate()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeUseItem : Package
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

		public McpeUseItem(bool pooled = false)
		{
			Id = 0xa3;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeUseItem> _pool = 
			new ObjectPool<McpeUseItem>(() => new McpeUseItem(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeUseItem CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeUseItem()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

	public partial class McpeContainerSetSlot : Package
	{
		public byte windowId; // = null;
		public short slot; // = null;
		public short itemId; // = null;
		public byte itemCount; // = null;
		public short itemDamage; // = null;

		public McpeContainerSetSlot(bool pooled = false)
		{
			Id = 0xb2;
			_isPooled = pooled;
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

		private static readonly ObjectPool<McpeContainerSetSlot> _pool = 
			new ObjectPool<McpeContainerSetSlot>(() => new McpeContainerSetSlot(true));

		private bool _isPooled = false;
		public int ReferenceCounter = 0;

		public static McpeContainerSetSlot CreateObject()
		{
			var obj = _pool.GetObject();
			obj.ReferenceCounter = 1;
			return obj;
		}

		public override void PutPool()
		{
			if(!_isPooled) return;

			if (Interlocked.Decrement(ref ReferenceCounter) > 0) return;

			Reset();
			_pool.PutObject(this);
		}

		static McpeContainerSetSlot()
		{
			for (int i = 0; i < 1000; i++)
			{
				_pool.PutObject(_pool.GetObject());
			}
		}

	}

}

