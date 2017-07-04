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

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;
using fNbt;
using log4net;
using MiNET.Crafting;
using MiNET.Items;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET.Net
{
	public abstract partial class Package
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Package));

		private bool _isEncoded = false;
		private byte[] _encodedMessage;

		[JsonIgnore] public int DatagramSequenceNumber = 0;

		[JsonIgnore]
		public bool NoBatch { get; set; }

		[JsonIgnore] public Reliability Reliability = Reliability.Unreliable;
		[JsonIgnore] public int ReliableMessageNumber = 0;
		[JsonIgnore] public byte OrderingChannel = 0;
		[JsonIgnore] public int OrderingIndex = 0;

		[JsonIgnore] public bool ForceClear = false;

		[JsonIgnore] public byte Id;

		protected MemoryStream _buffer;
		private BinaryWriter _writer;
		private BinaryReader _reader;
		private Stopwatch _timer = new Stopwatch();

		[JsonIgnore]
		public byte[] Bytes { get; private set; }

		public Package()
		{
			_buffer = new MemoryStream();
			_reader = new BinaryReader(_buffer);
			_writer = new BinaryWriter(_buffer);
			Timer.Start();
		}

		[JsonIgnore]
		public Stopwatch Timer
		{
			get { return _timer; }
		}

		public void Write(byte value)
		{
			_writer.Write(value);
		}

		public byte ReadByte()
		{
			return _reader.ReadByte();
		}

		public void Write(bool value)
		{
			Write((byte) (value ? 1 : 0));
		}

		public bool ReadBool()
		{
			return _reader.ReadByte() != 0;
		}

		public void Write(byte[] value)
		{
			if (value == null)
			{
				Log.Warn("Trying to write null byte[]");
				return;
			}

			_writer.Write(value);
		}

		public byte[] ReadBytes(int count)
		{
			if (count == 0)
			{
				count = (int) (_reader.BaseStream.Length - _reader.BaseStream.Position);
			}

			var readBytes = _reader.ReadBytes(count);
			if (readBytes.Length != count) throw new ArgumentOutOfRangeException($"Expected {count} bytes, only read {readBytes.Length}.");
			return readBytes;
		}

		public void WriteByteArray(byte[] value)
		{
			if (value == null)
			{
				Log.Warn("Trying to write null PrefixedArray");
				return;
			}

			WriteLength(value.Length);
			_writer.Write(value, 0, value.Length);
		}

		public byte[] ReadByteArray()
		{
			var len = ReadLength();
			var bytes = ReadBytes(len);
			return bytes;
		}

		public void Write(short value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(Endian.SwapInt16(value));
			else _writer.Write(value);
		}

		public short ReadShort(bool bigEndian = false)
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return 0;

			if (bigEndian) return Endian.SwapInt16(_reader.ReadInt16());

			return _reader.ReadInt16();
		}

		public void Write(ushort value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(Endian.SwapUInt16(value));
			else _writer.Write(value);
		}

		public ushort ReadUshort(bool bigEndian = false)
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return 0;

			if (bigEndian) return Endian.SwapUInt16(_reader.ReadUInt16());

			return _reader.ReadUInt16();
		}

		public void WriteBe(short value)
		{
			_writer.Write(Endian.SwapInt16(value));
		}

		public short ReadShortBe()
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return 0;

			return Endian.SwapInt16(_reader.ReadInt16());
		}

		public void Write(Int24 value)
		{
			_writer.Write(value.GetBytes());
		}

		public Int24 ReadLittle()
		{
			return new Int24(_reader.ReadBytes(3));
		}

		public void Write(int value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(Endian.SwapInt32(value));
			else _writer.Write(value);
		}

		public int ReadInt(bool bigEndian = false)
		{
			if (bigEndian) return Endian.SwapInt32(_reader.ReadInt32());

			return _reader.ReadInt32();
		}

		public void WriteBe(int value)
		{
			_writer.Write(Endian.SwapInt32(value));
		}

		public int ReadIntBe()
		{
			return Endian.SwapInt32(_reader.ReadInt32());
		}

		public void Write(uint value)
		{
			_writer.Write(value);
		}

		public uint ReadUint()
		{
			return _reader.ReadUInt32();
		}


		public void WriteVarInt(int value)
		{
			VarInt.WriteInt32(_buffer, value);
		}

		public int ReadVarInt()
		{
			return VarInt.ReadInt32(_buffer);
		}

		public void WriteSignedVarInt(int value)
		{
			VarInt.WriteSInt32(_buffer, value);
		}

		public int ReadSignedVarInt()
		{
			return VarInt.ReadSInt32(_buffer);
		}

		public void WriteUnsignedVarInt(uint value)
		{
			VarInt.WriteUInt32(_buffer, value);
		}

		public uint ReadUnsignedVarInt()
		{
			return VarInt.ReadUInt32(_buffer);
		}

		public int ReadLength()
		{
			return (int) VarInt.ReadUInt32(_buffer);
		}

		public void WriteLength(int value)
		{
			VarInt.WriteUInt32(_buffer, (uint) value);
		}

		public void WriteVarLong(long value)
		{
			VarInt.WriteInt64(_buffer, value);
		}

		public long ReadVarLong()
		{
			return VarInt.ReadInt64(_buffer);
		}

		public void WriteEntityId(long value)
		{
			WriteSignedVarLong(value);
		}

		public void WriteSignedVarLong(long value)
		{
			VarInt.WriteSInt64(_buffer, value);
		}

		public long ReadSignedVarLong()
		{
			return VarInt.ReadSInt64(_buffer);
		}

		public void WriteRuntimeEntityId(long value)
		{
			WriteUnsignedVarLong(value);
		}

		public void WriteUnsignedVarLong(long value)
		{
			// Need to fix this to ulong later
			VarInt.WriteUInt64(_buffer, (ulong) value);
		}

		public long ReadUnsignedVarLong()
		{
			// Need to fix this to ulong later
			return (long) VarInt.ReadUInt64(_buffer);
		}

		public void Write(long value)
		{
			_writer.Write(Endian.SwapInt64(value));
		}

		public long ReadLong()
		{
			return Endian.SwapInt64(_reader.ReadInt64());
		}

		public void Write(ulong value)
		{
			_writer.Write(value);
		}

		public ulong ReadUlong()
		{
			return _reader.ReadUInt64();
		}

		public void Write(float value)
		{
			_writer.Write(value);

			//byte[] bytes = BitConverter.GetBytes(value);
			//_writer.Write(bytes[3]);
			//_writer.Write(bytes[2]);
			//_writer.Write(bytes[1]);
			//_writer.Write(bytes[0]);
		}

		public float ReadFloat()
		{
			//byte[] buffer = _reader.ReadBytes(4);
			//return BitConverter.ToSingle(new[] {buffer[3], buffer[2], buffer[1], buffer[0]}, 0);
			return _reader.ReadSingle();
		}

		public void Write(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				WriteLength(0);
				return;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(value);

			WriteLength(bytes.Length);
			Write(bytes);
		}

		public string ReadString()
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return string.Empty;
			int len = ReadLength();
			if (len <= 0) return string.Empty;
			return Encoding.UTF8.GetString(ReadBytes(len));
		}

		public void WriteFixedString(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Write((short) 0, true);
				return;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(value);

			Write((short) bytes.Length, true);
			Write(bytes);
		}

		public string ReadFixedString()
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return string.Empty;
			short len = ReadShort(true);
			if (len <= 0) return string.Empty;
			return Encoding.UTF8.GetString(ReadBytes(len));
		}

		public void Write(Vector2 vec)
		{
			Write((float) vec.X);
			Write((float) vec.Y);
		}

		public Vector2 ReadVector2()
		{
			return new Vector2(ReadFloat(), ReadFloat());
		}

		public void Write(Vector3 vec)
		{
			Write((float) vec.X);
			Write((float) vec.Y);
			Write((float) vec.Z);
		}

		public Vector3 ReadVector3()
		{
			return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
		}


		public void Write(BlockCoordinates coord)
		{
			WriteSignedVarInt(coord.X);
			WriteUnsignedVarInt((uint) coord.Y);
			WriteSignedVarInt(coord.Z);
		}

		public BlockCoordinates ReadBlockCoordinates()
		{
			return new BlockCoordinates(ReadSignedVarInt(), (int) ReadUnsignedVarInt(), ReadSignedVarInt());
		}

		public void Write(PlayerRecords records)
		{
			if (records is PlayerAddRecords)
			{
				Write((byte) 0);
				WriteUnsignedVarInt((uint) records.Count);
				foreach (var record in records)
				{
					Write(record.ClientUuid);
					WriteSignedVarLong(record.EntityId);
					Write(record.DisplayName ?? record.Username);
					Write(record.Skin);
				}
			}
			else if (records is PlayerRemoveRecords)
			{
				Write((byte) 1);
				WriteUnsignedVarInt((uint) records.Count);
				foreach (var record in records)
				{
					Write(record.ClientUuid);
				}
			}
		}

		public PlayerRecords ReadPlayerRecords()
		{
			// This should never be used in production. It is primarily for 
			// the client to work.
			byte recordType = ReadByte();
			uint count = ReadUnsignedVarInt();
			PlayerRecords records = null;
			switch (recordType)
			{
				case 0:
					records = new PlayerAddRecords();
					for (int i = 0; i < count; i++)
					{
						var player = new Player(null, null);
						try
						{
							player.ClientUuid = ReadUUID();
							player.EntityId = ReadSignedVarLong();
							player.DisplayName = ReadString();
							player.Skin = ReadSkin();
							records.Add(player);
							//Log.Error($"Reading {player.ClientUuid}, {player.EntityId}, '{player.DisplayName}'");
						}
						catch (Exception e)
						{
							Log.Error("Player List", e);
						}
					}
					break;
				case 1:
					records = new PlayerRemoveRecords();
					for (int i = 0; i < count; i++)
					{
						var player = new Player(null, null);
						player.ClientUuid = ReadUUID();
						records.Add(player);
					}
					break;
			}

			return records;
		}

		public void Write(Records records)
		{
			WriteUnsignedVarInt((uint) records.Count);
			foreach (BlockCoordinates coord in records)
			{
				Write(coord);
			}
		}

		public Records ReadRecords()
		{
			var records = new Records();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				var coord = ReadBlockCoordinates();
				records.Add(coord);
			}

			return records;
		}

		public void Write(PlayerLocation location)
		{
			Write(location.X);
			Write(location.Y);
			Write(location.Z);
			var d = 256f/360f;
			Write((byte) Math.Round(location.Pitch*d)); // 256/360
			Write((byte) Math.Round(location.HeadYaw*d)); // 256/360
			Write((byte) Math.Round(location.Yaw*d)); // 256/360
		}

		public PlayerLocation ReadPlayerLocation()
		{
			PlayerLocation location = new PlayerLocation();
			location.X = ReadFloat();
			location.Y = ReadFloat();
			location.Z = ReadFloat();
			location.Pitch = ReadByte()*1f/0.71f;
			location.HeadYaw = ReadByte()*1f/0.71f;
			location.Yaw = ReadByte()*1f/0.71f;

			return location;
		}

		public void Write(IPEndPoint endpoint)
		{
			if (endpoint.AddressFamily == AddressFamily.InterNetwork)
			{
				Write((byte) 4);
				var parts = endpoint.Address.ToString().Split('.');
				foreach (var part in parts)
				{
					Write((byte) byte.Parse(part));
				}
				Write((short) endpoint.Port, true);
			}
		}


//typedef struct sockaddr_in6
//{
//	ADDRESS_FAMILY sin6_family; // AF_INET6.
//	USHORT sin6_port;           // Transport level port number.
//	ULONG sin6_flowinfo;       // IPv6 flow information.
//	IN6_ADDR sin6_addr;         // IPv6 address.
//	union {
//ULONG sin6_scope_id;     // Set of interfaces for a scope.
//	SCOPE_ID sin6_scope_struct;
//};
//}
//SOCKADDR_IN6_LH, * PSOCKADDR_IN6_LH, FAR * LPSOCKADDR_IN6_LH;

		public IPEndPoint ReadIPEndPoint()
		{
			byte ipVersion = ReadByte();

			IPAddress address = IPAddress.Any;
			int port = 0;

			if (ipVersion == 4)
			{
				string ipAddress = $"{ReadByte()}.{ReadByte()}.{ReadByte()}.{ReadByte()}";
				address = IPAddress.Parse(ipAddress);
				port = (ushort) ReadShort(true);
			}
			else if (ipVersion == 6)
			{
				ReadShort(); // Address family
				port = (ushort) ReadShort(true); // Port
				ReadLong(); // Flow info
				var addressBytes = ReadBytes(16);
				address = new IPAddress(addressBytes);
			}
			else
			{
				Log.Error($"Wrong IP version. Expected IPv4 or IPv6 but was IPv{ipVersion}");
			}

			return new IPEndPoint(address, port);
		}

		public void Write(IPEndPoint[] endpoints)
		{
			foreach (var endpoint in endpoints)
			{
				Write(endpoint);
			}
		}

		public IPEndPoint[] ReadIPEndPoints(int count)
		{
			var endPoints = new IPEndPoint[count];
			for (int i = 0; i < endPoints.Length; i++)
			{
				endPoints[i] = ReadIPEndPoint();
			}

			return endPoints;
		}

		public void Write(UUID uuid)
		{
			if (uuid == null) throw new Exception("Expected UUID, required");
			Write(uuid.GetBytes());
		}

		public UUID ReadUUID()
		{
			UUID uuid = new UUID(ReadBytes(16));
			return uuid;
		}

		public void Write(Nbt nbt)
		{
			NbtFile file = nbt.NbtFile;
			file.BigEndian = false;
			file.UseVarInt = this is McpeBlockEntityData;

			Write(file.SaveToBuffer(NbtCompression.None));
		}

		public Nbt ReadNbt()
		{
			Nbt nbt = new Nbt();
			NbtFile file = new NbtFile();
			file.BigEndian = false;
			file.UseVarInt = this is McpeBlockEntityData;
			nbt.NbtFile = file;
			file.LoadFromStream(_reader.BaseStream, NbtCompression.None);

			return nbt;
		}

		public void Write(MetadataInts metadata)
		{
			if (metadata == null)
			{
				WriteVarInt(0);
				return;
			}

			WriteVarInt(metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				MetadataInt slot = metadata[i] as MetadataInt;
				if (slot != null)
				{
					WriteSignedVarInt(slot.Value);
				}
			}
		}

		public MetadataInts ReadMetadataInts()
		{
			MetadataInts metadata = new MetadataInts();
			int count = ReadVarInt();

			for (byte i = 0; i < count; i++)
			{
				metadata[i] = new MetadataInt(ReadSignedVarInt());
			}

			return metadata;
		}

		public void Write(ItemStacks metadata)
		{
			McpeContainerSetContent msg = this as McpeContainerSetContent;
			bool signItems = msg == null || msg.windowId != 0x79;

			if (metadata == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint) metadata.Count);

			for (int i = 0; i < metadata.Count; i++)
			{
				Write(metadata[i], signItems);
			}
		}

		public ItemStacks ReadItemStacks()
		{
			ItemStacks metadata = new ItemStacks();

			var count = ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				metadata.Add(ReadItem());
			}

			return metadata;
		}

		public void Write(Item stack, bool signItem = true)
		{
			if (stack == null || stack.Id <= 0)
			{
				WriteSignedVarInt(0);
				return;
			}

			WriteSignedVarInt(stack.Id);
			short metadata = stack.Metadata;
			if (metadata == -1) metadata = short.MaxValue;
			WriteSignedVarInt((metadata << 8) + (stack.Count & 0xff));

			if (signItem)
			{
				stack = ItemSigner.DefaultItemSigner?.SignItem(stack);
			}

			if (stack.ExtraData != null)
			{
				byte[] bytes = GetNbtData(stack.ExtraData);
				_writer.Write((short) bytes.Length);
				Write(bytes);
			}
			else
			{
				Write((short) 0);
			}

			WriteSignedVarInt(0);
			WriteSignedVarInt(0);
		}

		public Item ReadItem()
		{
			int id = ReadSignedVarInt();
			if (id <= 0)
			{
				return new ItemAir();
			}

			int tmp = ReadSignedVarInt();
			short metadata = (short) (tmp >> 8);
			if (metadata == short.MaxValue) metadata = -1;
			byte count = (byte) (tmp & 0xff);
			Item stack = ItemFactory.GetItem((short) id, metadata, count);

			int nbtLen = _reader.ReadInt16(); // NbtLen
			if (nbtLen > 0)
			{
				stack.ExtraData = ReadNbt().NbtFile.RootTag;
			}

			var canPlace = ReadSignedVarInt();
			for (int i = 0; i < canPlace; i++)
			{
				ReadString();
			}
			var canBreak = ReadSignedVarInt();
			for (int i = 0; i < canBreak; i++)
			{
				ReadString();
			}

			return stack;
		}


		private byte[] GetNbtData(NbtCompound nbtCompound)
		{
			nbtCompound.Name = string.Empty;
			var file = new NbtFile(nbtCompound);
			file.BigEndian = false;

			return file.SaveToBuffer(NbtCompression.None);
		}

		public void Write(MetadataDictionary metadata)
		{
			if (metadata != null)
			{
				metadata.WriteTo(_writer);
			}
		}

		public MetadataDictionary ReadMetadataDictionary()
		{
			return MetadataDictionary.FromStream(_reader);
		}

		public PlayerAttributes ReadPlayerAttributes()
		{
			var attributes = new PlayerAttributes();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				PlayerAttribute attribute = new PlayerAttribute
				{
					MinValue = ReadFloat(),
					MaxValue = ReadFloat(),
					Value = ReadFloat(),
					Default = ReadFloat(),
					Name = ReadString(),
				};

				attributes[attribute.Name] = attribute;
			}

			return attributes;
		}

		public void Write(PlayerAttributes attributes)
		{
			WriteUnsignedVarInt((uint) attributes.Count);
			foreach (PlayerAttribute attribute in attributes.Values)
			{
				Write(attribute.MinValue);
				Write(attribute.MaxValue);
				Write(attribute.Value);
				Write(attribute.Default); // unknown
				Write(attribute.Name);
			}
		}


		public GameRules ReadGameRules()
		{
			GameRules gameRules = new GameRules();

			int count = ReadVarInt();
			for (int i = 0; i < count; i++)
			{
				string name = ReadString();
				byte type = ReadByte();
				switch (type)
				{
					case 1:
					{
						GameRule<bool> rule = new GameRule<bool>();
						rule.Name = name;
						rule.Value = ReadBool();
						gameRules.Add(rule.Name, rule);
						break;
					}
					case 2:
					{
						GameRule<int> rule = new GameRule<int>();
						rule.Name = name;
						rule.Value = ReadVarInt();
						gameRules.Add(rule.Name, rule);
						break;
					}
					case 3:
					{
						GameRule<float> rule = new GameRule<float>();
						rule.Name = name;
						rule.Value = ReadFloat();
						gameRules.Add(rule.Name, rule);
						break;
					}
				}
			}

			return gameRules;
		}

		public void Write(GameRules gameRules)
		{
			if (gameRules == null)
			{
				WriteVarInt(0);
				return;
			}

			WriteVarInt(gameRules.Count);
			foreach (var rule in gameRules)
			{
				var value = rule.Value;
				Write(rule.Key);
				if (value is GameRule<bool>)
				{
					Write((byte) 1);
					Write(((GameRule<bool>) value).Value);
				}
				else if (value is GameRule<int>)
				{
					Write((byte) 2);
					WriteVarInt(((GameRule<int>) value).Value);
				}
				else if (value is GameRule<float>)
				{
					Write((byte) 3);
					Write(((GameRule<float>) value).Value);
				}
			}
		}

		public void Write(EntityAttributes attributes)
		{
			if (attributes == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint) attributes.Count);
			foreach (EntityAttribute attribute in attributes.Values)
			{
				Write(attribute.Name);
				Write(attribute.MinValue);
				Write(attribute.Value);
				Write(attribute.MaxValue);
			}
		}

		public EntityAttributes ReadEntityAttributes()
		{
			var attributes = new EntityAttributes();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				EntityAttribute attribute = new EntityAttribute
				{
					Name = ReadString(),
					MinValue = ReadFloat(),
					Value = ReadFloat(),
					MaxValue = ReadFloat(),
				};

				attributes[attribute.Name] = attribute;
			}

			return attributes;
		}

		public void Write(Links links)
		{
			if (links == null)
			{
				WriteUnsignedVarInt(0); // LE
				return;
			}
			WriteUnsignedVarInt((uint) links.Count); // LE
			foreach (var link in links)
			{
				WriteVarLong(link.Item1);
				WriteVarLong(link.Item2);
				_writer.Write((short) 1); // LE
			}
		}

		public Links ReadLinks()
		{
			int count = (int) ReadUnsignedVarInt(); // LE

			var links = new Links();
			for (int i = 0; i < count; i++)
			{
				Tuple<long, long> link = new Tuple<long, long>(ReadVarLong(), ReadVarLong());
				_reader.ReadInt16();
			}

			return links;
		}

		public void Write(Rules rules)
		{
			_writer.Write(rules.Count); // LE
			foreach (var rule in rules)
			{
				Write(rule.Name);
				Write(rule.Unknown1);
				Write(rule.Unknown2);
			}
		}

		public Rules ReadRules()
		{
			int count = _reader.ReadInt32(); // LE

			var rules = new Rules();
			for (int i = 0; i < count; i++)
			{
				RuleData rule = new RuleData();
				rule.Name = ReadString();
				rule.Unknown1 = ReadBool();
				rule.Unknown2 = ReadBool();
				rules.Add(rule);
			}

			return rules;
		}

		public void Write(ResourcePackInfos packInfos)
		{
			if (packInfos == null)
			{
				_writer.Write((short) 0); // LE
				//WriteVarInt(0);
				return;
			}

			_writer.Write((short) packInfos.Count); // LE
			//WriteVarInt(packInfos.Count);
			foreach (var info in packInfos)
			{
				Write(info.PackIdVersion.Id);
				Write(info.PackIdVersion.Version);
				Write(info.Size);
				Write("");
			}
		}

		public ResourcePackInfos ReadResourcePackInfos()
		{
			int count = _reader.ReadInt16(); // LE
			//int count = ReadVarInt(); // LE

			var packInfos = new ResourcePackInfos();
			for (int i = 0; i < count; i++)
			{
				var info = new ResourcePackInfo();
				var id = ReadString();
				var version = ReadString();
				var size = ReadUlong();
				var unknown = ReadString();
				info.PackIdVersion = new PackIdVersion {Id = id, Version = version};
				info.Size = size;
				packInfos.Add(info);
			}

			return packInfos;
		}

		public void Write(ResourcePackIdVersions packInfos)
		{
			if (packInfos == null)
			{
				Write((short) 0); // LE
				return;
			}
			Write((short) packInfos.Count); // LE
			foreach (var info in packInfos)
			{
				Write(info.Id);
				Write(info.Version);
			}
		}

		public ResourcePackIdVersions ReadResourcePackIdVersions()
		{
			//int count = _reader.ReadInt16(); // LE
			int count = ReadShort(); // LE

			var packInfos = new ResourcePackIdVersions();
			for (int i = 0; i < count; i++)
			{
				var id = ReadString();
				var version = ReadString();
				var info = new PackIdVersion {Id = id, Version = version};
				packInfos.Add(info);
			}

			return packInfos;
		}

		public void Write(ResourcePackIds ids)
		{
			if (ids == null)
			{
				Write((short) 0);
				return;
			}
			Write((short) ids.Count);

			foreach (var id in ids)
			{
				Write(id);
			}
		}

		public ResourcePackIds ReadResourcePackIds()
		{
			int count = ReadShort();

			var ids = new ResourcePackIds();
			for (int i = 0; i < count; i++)
			{
				var id = ReadString();
				ids.Add(id);
			}

			return ids;
		}

		public void Write(Skin skin)
		{
			if (skin.Texture != null)
			{
				var skinType = skin.SkinType;
				if (string.IsNullOrEmpty(skinType)) skinType = "Standard_Custom";
				Write(skinType);
				WriteUnsignedVarInt((uint) skin.Texture.Length);
				Write(skin.Texture);
			}
		}

		public Skin ReadSkin()
		{
			Skin skin = new Skin();

			try
			{
				skin.SkinType = ReadString();
				if (string.IsNullOrEmpty(skin.SkinType)) skin.SkinType = null;

				if (skin.SkinType != null)
				{
					int length = (int) ReadUnsignedVarInt();
					if (length == 64*32*4 || length == 64*64*4)
					{
						skin.Texture = ReadBytes(length);
					}
					else
					{
						skin.SkinType = null;
					}
				}
			}
			catch (Exception e)
			{
				skin.SkinType = null;
				skin.Texture = null;
			}

			return skin;
		}

		const byte Shapeless = 0;
		const byte Shaped = 1;
		const byte Furnace = 2;
		const byte FurnaceData = 3;
		const byte Multi = 4;
		const byte ShulkerBox = 5;

		public void Write(Recipes recipes)
		{
			WriteUnsignedVarInt((uint) recipes.Count);

			foreach (Recipe recipe in recipes)
			{
				if (recipe is ShapelessRecipe)
				{
					WriteSignedVarInt(0); // Type

					ShapelessRecipe rec = (ShapelessRecipe) recipe;
					WriteVarInt(rec.Input.Count);
					foreach (Item stack in rec.Input)
					{
						Write(stack);
					}
					WriteVarInt(1);
					Write(rec.Result);
					Write(new UUID(Guid.NewGuid().ToString()));
				}
				else if (recipe is ShapedRecipe)
				{
					WriteSignedVarInt(1); // Type

					ShapedRecipe rec = (ShapedRecipe) recipe;
					WriteSignedVarInt(rec.Width);
					WriteSignedVarInt(rec.Height);

					for (int w = 0; w < rec.Width; w++)
					{
						for (int h = 0; h < rec.Height; h++)
						{
							Write(rec.Input[(h*rec.Width) + w]);
						}
					}
					WriteVarInt(1);
					Write(rec.Result);
					Write(new UUID(Guid.NewGuid().ToString()));
				}
				else if (recipe is SmeltingRecipe)
				{
					SmeltingRecipe rec = (SmeltingRecipe) recipe;
					WriteSignedVarInt(rec.Input.Metadata == 0 ? 2 : 3); // Type
					WriteSignedVarInt(rec.Input.Id);
					if (rec.Input.Metadata != 0) WriteSignedVarInt(rec.Input.Metadata);
					Write(rec.Result);
				}
				else if (recipe is EnchantingRecipe)
				{
					//var memoryStream = MiNetServer.MemoryStreamManager.GetStream();
					//McpeWriter writer = new McpeWriter(memoryStream);

					//writer.Write((byte) 3); // Count
					//{
					//	writer.Write((int) 1); // Cost
					//	writer.Write((byte) 1); // Count
					//	writer.Write((int) 9); // Id
					//	writer.Write((int) 1); // Level
					//	writer.Write("Test1"); // Level
					//}

					//Write(4); // Type
					//var bytes = memoryStream.ToArray();
					//Write(bytes.Length);
					//Write(bytes);
				}
			}

			Write((byte) 1);
		}

		public Recipes ReadRecipes()
		{
			Recipes recipes = new Recipes();

			int count = (int) ReadUnsignedVarInt();

			Log.Error($"Reading {count} recipes");

			for (int i = 0; i < count; i++)
			{
				int recipeType = (int) ReadSignedVarInt();

				//Log.Error($"Read recipe no={i} type={recipeType}");

				if (recipeType < 0 /*|| len == 0*/)
				{
					Log.Error("Read void recipe");
					break;
				}

				if (recipeType == Shapeless || recipeType == ShulkerBox)
				{
					ShapelessRecipe recipe = new ShapelessRecipe();
					int ingrediensCount = ReadVarInt(); // 
					for (int j = 0; j < ingrediensCount; j++)
					{
						recipe.Input.Add(ReadItem());
					}
					ReadVarInt(); // 1?
					recipe.Result = ReadItem();
					recipe.Id = ReadUUID(); // Id
					recipes.Add(recipe);
					//Log.Error("Read shapeless recipe");
				}
				else if (recipeType == Shaped)
				{
					int width = ReadSignedVarInt(); // Width
					int height = ReadSignedVarInt(); // Height
					ShapedRecipe recipe = new ShapedRecipe(width, height);
					if (width > 3 || height > 3) throw new Exception("Wrong number of ingredience. Width=" + width + ", height=" + height);
					for (int w = 0; w < width; w++)
					{
						for (int h = 0; h < height; h++)
						{
							recipe.Input[(h*width) + w] = ReadItem();
						}
					}

					int resultCount = ReadVarInt(); // 1?
					for (int j = 0; j < resultCount; j++)
					{
						recipe.Result = ReadItem();
					}
					recipe.Id = ReadUUID(); // Id
					recipes.Add(recipe);
					//Log.Error("Read shaped recipe");
				}
				else if (recipeType == Furnace)
				{
					SmeltingRecipe recipe = new SmeltingRecipe();
					//short meta = (short) ReadVarInt(); // input (with metadata) 
					short id = (short) ReadSignedVarInt(); // input (with metadata) 
					Item result = ReadItem(); // Result
					recipe.Input = ItemFactory.GetItem(id, 0);
					recipe.Result = result;
					recipes.Add(recipe);
					//Log.Error("Read furnace recipe");
					//Log.Error($"Input={id}, meta={""} Item={result.Id}, Meta={result.Metadata}");
				}
				else if (recipeType == FurnaceData)
				{
					//const ENTRY_FURNACE_DATA = 3;
					SmeltingRecipe recipe = new SmeltingRecipe();
					short id = (short) ReadSignedVarInt(); // input (with metadata) 
					short meta = (short) ReadSignedVarInt(); // input (with metadata) 
					Item result = ReadItem(); // Result
					recipe.Input = ItemFactory.GetItem(id, meta);
					recipe.Result = result;
					recipes.Add(recipe);
					//Log.Error("Read smelting recipe");
					//Log.Error($"Input={id}, meta={meta} Item={result.Id}, Meta={result.Metadata}");
				}
				else if (recipeType == Multi)
				{
					Log.Error("Reading MULTI");

					ReadUUID();
				}
				else
				{
					Log.Error($"Read unknown recipe type: {recipeType}");
					//ReadBytes(len);
				}
			}

			ReadByte(); // Clean (1) or update (0)

			return recipes;
		}

		const int BITFLAG_TEXTURE_UPDATE = 0x02;
		const int BITFLAG_DECORATION_UPDATE = 0x04;
		const int BITFLAG_ENTITY_UPDATE = 0x08;

		public void Write(MapInfo map)
		{
			WriteSignedVarLong(map.MapId);
			WriteUnsignedVarInt(map.UpdateType);

			//if ((map.UpdateType & BITFLAG_ENTITY_UPDATE) == BITFLAG_ENTITY_UPDATE)
			//{
			//}

			if ((map.UpdateType & BITFLAG_TEXTURE_UPDATE) == BITFLAG_TEXTURE_UPDATE || (map.UpdateType & BITFLAG_DECORATION_UPDATE) == BITFLAG_DECORATION_UPDATE)
			{
				Write((byte) map.Scale);
			}

			if ((map.UpdateType & BITFLAG_DECORATION_UPDATE) == BITFLAG_DECORATION_UPDATE)
			{
				var count = map.Decorators.Length;
				WriteUnsignedVarInt((uint) count);
				foreach (var decorator in map.Decorators)
				{
					WriteSignedVarInt((decorator.Rotation & 0x0f) | (decorator.Icon << 4));
					Write((byte) decorator.X);
					Write((byte) decorator.Z);
					Write(decorator.Label);
					Write(decorator.Color);
				}
			}

			if ((map.UpdateType & BITFLAG_TEXTURE_UPDATE) == BITFLAG_TEXTURE_UPDATE)
			{
				WriteSignedVarInt(map.Col);
				WriteSignedVarInt(map.Row);

				WriteSignedVarInt(map.XOffset);
				WriteSignedVarInt(map.ZOffset);

				int i = 0;
				for (int col = 0; col < map.Col; col++)
				{
					for (int row = 0; row < map.Row; row++)
					{
						byte r = map.Data[i++];
						byte g = map.Data[i++];
						byte b = map.Data[i++];
						byte a = map.Data[i++];
						uint color = BitConverter.ToUInt32(new byte[] {r, g, b, 0xff}, 0);
						WriteUnsignedVarInt(color);
					}
				}
			}
		}

		public MapInfo ReadMapInfo()
		{
			MapInfo map = new MapInfo();

			map.MapId = ReadSignedVarLong();
			map.UpdateType = (byte) ReadUnsignedVarInt();

			if ((map.UpdateType & BITFLAG_ENTITY_UPDATE) == BITFLAG_ENTITY_UPDATE)
			{
				// Entities
				var count = ReadUnsignedVarInt();
				for (int i = 0; i < count - 1; i++) // This is some weird shit vanilla is doing with counting.
				{
					var eid = ReadSignedVarLong();
				}
			}

			if ((map.UpdateType & BITFLAG_TEXTURE_UPDATE) == BITFLAG_TEXTURE_UPDATE || (map.UpdateType & BITFLAG_DECORATION_UPDATE) == BITFLAG_DECORATION_UPDATE)
			{
				map.Scale = ReadByte();
				//Log.Warn($"Reading scale {map.Scale}");
			}

			if ((map.UpdateType & BITFLAG_DECORATION_UPDATE) == BITFLAG_DECORATION_UPDATE)
			{
				// Decorations
				//Log.Warn("Got decoration update, reading it");

				try
				{
					var count = ReadUnsignedVarInt();
					map.Decorators = new MapDecorator[count];
					for (int i = 0; i < count; i++)
					{
						MapDecorator decorator = new MapDecorator();
						var si = ReadSignedVarInt(); // some stuff
						decorator.Rotation = (byte) (si & 0x0f);
						decorator.Icon = (byte) ((si & 0xf0) >> 4);

						decorator.X = ReadByte();
						decorator.X = ReadByte();
						decorator.Label = ReadString();
						decorator.Color = ReadUint();
						map.Decorators[i] = decorator;
					}
				}
				catch (Exception e)
				{
					Log.Error($"Errror while reading decorations for map={map}", e);
				}
			}

			if ((map.UpdateType & BITFLAG_TEXTURE_UPDATE) == BITFLAG_TEXTURE_UPDATE)
			{
				// Full map
				try
				{
					map.Col = ReadSignedVarInt();
					map.Row = ReadSignedVarInt(); //

					map.XOffset = ReadSignedVarInt(); //
					map.ZOffset = ReadSignedVarInt(); //

					for (int col = 0; col < map.Col; col++)
					{
						for (int row = 0; row < map.Row; row++)
						{
							ReadUnsignedVarInt();
						}
					}
				}
				catch (Exception e)
				{
					Log.Error($"Errror while reading map data for map={map}", e);
				}
			}

			//else
			//{
			//	Log.Warn($"Unknown map-type 0x{map.UpdateType:X2}");
			//}

			//map.MapId = ReadLong();
			//var readBytes = ReadBytes(3);
			////Log.Warn($"{HexDump(readBytes)}");
			//map.UpdateType = ReadByte(); //
			//var bytes = ReadBytes(6);
			////Log.Warn($"{HexDump(bytes)}");

			//map.Direction = ReadByte(); //
			//map.X = ReadByte(); //
			//map.Z = ReadByte(); //

			//if (map.UpdateType == 0x06)
			//{
			//	// Full map
			//	try
			//	{
			//		if (bytes[4] == 1)
			//		{
			//			map.Col = ReadInt();
			//			map.Row = ReadInt(); //

			//			map.XOffset = ReadInt(); //
			//			map.ZOffset = ReadInt(); //

			//			map.Data = ReadBytes(map.Col*map.Row*4);
			//		}
			//	}
			//	catch (Exception e)
			//	{
			//		Log.Error($"Errror while reading map data for map={map}", e);
			//	}
			//}
			//else if (map.UpdateType == 0x04)
			//{
			//	// Map update
			//}
			//else
			//{
			//	Log.Warn($"Unknown map-type 0x{map.UpdateType:X2}");
			//}

			return map;
		}

		public bool CanRead()
		{
			return _reader.BaseStream.Position < _reader.BaseStream.Length;
		}

		protected virtual void EncodePackage()
		{
			_buffer.Position = 0;
			Write(Id);
		}

		public virtual void Reset()
		{
			DatagramSequenceNumber = -1;

			Reliability = Reliability.Unreliable;
			ReliableMessageNumber = -1;
			OrderingChannel = 0;
			OrderingIndex = -1;

			NoBatch = false;
			ForceClear = false;

			_encodedMessage = null;
			_writer.Flush();
			_buffer.SetLength(0);
			_buffer.Position = 0;
			_timer.Restart();
			_isEncoded = false;
		}

		public void SetEncodedMessage(byte[] encodedMessage)
		{
			_encodedMessage = encodedMessage;
			_isEncoded = true;
		}

		private object _encodeSync = new object();

		public virtual byte[] Encode()
		{
			lock (_encodeSync)
			{
				if (_isEncoded) return _encodedMessage;

				_isEncoded = false;

				EncodePackage();

				_writer.Flush();
				_buffer.Position = 0;
				_encodedMessage = _buffer.ToArray();
				_isEncoded = true;
				return _encodedMessage;
			}
		}

		protected virtual void DecodePackage()
		{
			_buffer.Position = 0;
			Id = ReadByte();
		}

		public virtual void Decode(byte[] buffer)
		{
			Bytes = buffer;
			_buffer.Position = 0;
			_buffer.SetLength(buffer.Length);
			_buffer.Write(buffer, 0, buffer.Length);
			_buffer.Position = 0;
			DecodePackage();
			if (Log.IsDebugEnabled && _buffer.Position != (buffer.Length))
			{
				Log.Warn($"{GetType().Name}: Still have {buffer.Length - _buffer.Position} bytes to read!!\n{HexDump(buffer)}");
			}
		}

		public void CloneReset()
		{
			_buffer = MiNetServer.MemoryStreamManager.GetStream();
			_reader = new BinaryReader(_buffer);
			_writer = new BinaryWriter(_buffer);
			Timer.Start();
		}

		public virtual object Clone()
		{
			Package clone = (Package) MemberwiseClone();
			clone.CloneReset();
			return clone;
		}

		public virtual T Clone<T>() where T : Package
		{
			return (T) Clone();
		}

		public abstract void PutPool();

		public static string HexDump(byte[] bytes, int bytesPerLine = 16, bool printLineCount = false)
		{
			StringBuilder sb = new StringBuilder();
			for (int line = 0; line < bytes.Length; line += bytesPerLine)
			{
				byte[] lineBytes = bytes.Skip(line).Take(bytesPerLine).ToArray();
				if (printLineCount) sb.AppendFormat("{0:x8} ", line);
				sb.Append(string.Join(" ", lineBytes.Select(b => b.ToString("x2"))
						.ToArray())
					.PadRight(bytesPerLine*3));
				sb.Append(" ");
				sb.Append(new string(lineBytes.Select(b => b < 32 ? '.' : (char) b)
					.ToArray()));
				sb.AppendLine();
			}
			return sb.ToString();
		}

		public static string ToJson(Package message)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.Arrays,

				Formatting = Formatting.Indented,
			};
			jsonSerializerSettings.Converters.Add(new NbtIntConverter());
			jsonSerializerSettings.Converters.Add(new NbtStringConverter());

			return JsonConvert.SerializeObject(message, jsonSerializerSettings);
		}
	}

	/// Base package class
	public abstract partial class Package<T> : Package, ICloneable where T : Package<T>, new()
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Package));

		private static readonly ObjectPool<T> Pool = new ObjectPool<T>(() => new T());

		private bool _isPermanent;
		private bool _isPooled;
		private long _referenceCounter;

		[JsonIgnore]
		public bool IsPooled
		{
			get { return _isPooled; }
		}

		[JsonIgnore]
		public long ReferenceCounter
		{
			get { return _referenceCounter; }
			set { _referenceCounter = value; }
		}


		public T MarkPermanent(bool permanent = true)
		{
			if (!_isPooled) throw new Exception("Tried to make non pooled item permanent");
			_isPermanent = permanent;

			return (T) this;
		}

		public T AddReferences(long numberOfReferences)
		{
			if (_isPermanent) return (T) this;

			if (!_isPooled) throw new Exception("Tried to referenc count a non pooled item");
			Interlocked.Add(ref _referenceCounter, numberOfReferences);

			return (T) this;
		}

		public T AddReference(Package<T> item)
		{
			if (_isPermanent) return (T) this;

			if (!item.IsPooled) throw new Exception("Item template needs to come from a pool");

			Interlocked.Increment(ref item._referenceCounter);
			return (T) item;
		}

		public T MakePoolable(long numberOfReferences = 1)
		{
			_isPooled = true;
			_referenceCounter = numberOfReferences;
			return (T) this;
		}


		public static T CreateObject(long numberOfReferences = 1)
		{
			var item = Pool.GetObject();
			item._isPooled = true;
			item._referenceCounter = numberOfReferences;
			item.Timer.Restart();
			return item;
		}

		//static Package()
		//{
		//	for (int i = 0; i < 100; i++)
		//	{
		//		Pool.PutObject(Pool.GetObject());
		//	}
		//}


		~Package()
		{
			if (_isPooled)
			{
				//Log.Error($"Unexpected dispose 0x{Id:x2} {GetType().Name}, IsPooled={_isPooled}, IsPermanent={_isPermanent}, Refs={_referenceCounter}");
			}
		}

		public override void PutPool()
		{
			if (_isPermanent) return;
			if (!IsPooled) return;

			var counter = Interlocked.Decrement(ref _referenceCounter);
			if (counter > 0) return;

			if (counter < 0)
			{
				Log.Error($"Pooling error. Added pooled object too many times. 0x{Id:x2} {GetType().Name}, IsPooled={IsPooled}, IsPooled={_isPermanent}, Refs={_referenceCounter}");
				return;
			}

			Reset();

			_isPooled = false;

			Pool.PutObject((T) this);
		}

		public static int PoolSize()
		{
			return Pool.Size;
		}
	}
}