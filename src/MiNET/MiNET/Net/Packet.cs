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
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
using Microsoft.IO;
using MiNET.Blocks;
using MiNET.Crafting;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.IO;
using MiNET.Utils.Metadata;
using MiNET.Utils.Nbt;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;
using Newtonsoft.Json;

namespace MiNET.Net
{
	public abstract partial class Packet
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Packet));

		private byte[] _encodedMessage;


		[JsonIgnore] public bool NoBatch { get; set; }

		/// <summary>
		///     Declares this packet wholly superseded by any later queued packet carrying the same key:
		///     a send lane that still holds both may drop the older one unsent (loss as sender policy,
		///     never as an accident of load - only the sender knows the semantics). Null, the default,
		///     means never drop. The key must carry the full identity of what supersedes what: the move
		///     roster uses one key object per level, a per-entity update would need a per-entity key.
		///     Reference equality; reset with the packet so a pooled reuse can never inherit one.
		/// </summary>
		[JsonIgnore] public object CoalesceKey { get; set; }

		[JsonIgnore] public int Id;
		[JsonIgnore] public bool IsMcpe;

		protected MemoryStreamReader _reader; // new construct for reading
		protected private Stream _buffer;
		private BinaryWriter _writer;


		public void Write(byte value)
		{
			_writer.Write(value);
		}

		public byte ReadByte()
		{
			return (byte) _reader.ReadByte();
		}

		public void Write(bool value)
		{
			Write((byte) (value ? 1 : 0));
		}

		public bool ReadBool()
		{
			return _reader.ReadByte() != 0;
		}

		public void Write(Memory<byte> value)
		{
			Write((ReadOnlyMemory<byte>) value);
		}

		public void Write(ReadOnlyMemory<byte> value)
		{
			if (value.IsEmpty)
			{
				Log.Warn("Trying to write empty Memory<byte>");
				return;
			}
			_writer.Write(value.Span);
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

		public ReadOnlyMemory<byte> Slice(int count)
		{
			return _reader.Read(count);
		}

		public ReadOnlyMemory<byte> ReadReadOnlyMemory(int count, bool slurp = false)
		{
			if (!slurp && count == 0) return Memory<byte>.Empty;

			if (count == 0)
			{
				count = (int) (_reader.Length - _reader.Position);
			}

			ReadOnlyMemory<byte> readBytes = _reader.Read(count);
			if (readBytes.Length != count) throw new ArgumentOutOfRangeException($"Expected {count} bytes, only read {readBytes.Length}.");
			return readBytes;
		}

		public byte[] ReadBytes(int count, bool slurp = false)
		{
			if (!slurp && count == 0) return new byte[0];

			if (count == 0)
			{
				count = (int) (_reader.Length - _reader.Position);
			}

			ReadOnlyMemory<byte> readBytes = _reader.Read(count);
			if (readBytes.Length != count) throw new ArgumentOutOfRangeException($"Expected {count} bytes, only read {readBytes.Length}.");
			return readBytes.ToArray(); //TODO: Replace with ReadOnlyMemory<byte> return
		}

		public void WriteByteArray(byte[] value)
		{
			if (value == null)
			{
				WriteLength(0);
				return;
			}

			WriteLength(value.Length);

			if (value.Length == 0) return;

			_writer.Write(value, 0, value.Length);
		}

		public byte[] ReadByteArray(bool slurp = false)
		{
			var len = ReadLength();
			var bytes = ReadBytes(len, slurp);
			return bytes;
		}

		public void Write(ulong[] value)
		{
			if (value == null)
			{
				WriteLength(0);
				return;
			}

			WriteLength(value.Length);

			if (value.Length == 0) return;
			for (int i = 0; i < value.Length; i++)
			{
				ulong val = value[i];
				Write(val);
			}
		}

		public ulong[] ReadUlongs(bool slurp = false)
		{
			var len = ReadLength();
			var ulongs = new ulong[len];
			for (int i = 0; i < ulongs.Length; i++)
			{
				ulongs[i] = ReadUlong();
			}
			return ulongs;
		}

		public void Write(short value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(BinaryPrimitives.ReverseEndianness(value));
			else _writer.Write(value);
		}

		public short ReadShort(bool bigEndian = false)
		{
			if (_reader.Position == _reader.Length) return 0;

			if (bigEndian) return BinaryPrimitives.ReverseEndianness(_reader.ReadInt16());

			return _reader.ReadInt16();
		}

		public void Write(ushort value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(BinaryPrimitives.ReverseEndianness(value));
			else _writer.Write(value);
		}

		public ushort ReadUshort(bool bigEndian = false)
		{
			if (_reader.Position == _reader.Length) return 0;

			if (bigEndian) return BinaryPrimitives.ReverseEndianness(_reader.ReadUInt16());

			return _reader.ReadUInt16();
		}

		public void WriteBe(short value)
		{
			_writer.Write(BinaryPrimitives.ReverseEndianness(value));
		}

		public short ReadShortBe()
		{
			if (_reader.Position == _reader.Length) return 0;

			return BinaryPrimitives.ReverseEndianness(_reader.ReadInt16());
		}

		public void Write(Int24 value)
		{
			_writer.Write(value.GetBytes());
		}

		public Int24 ReadLittle()
		{
			return new Int24(_reader.Read(3).Span);
		}

		public void Write(int value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(BinaryPrimitives.ReverseEndianness(value));
			else _writer.Write(value);
		}

		public int ReadInt(bool bigEndian = false)
		{
			if (bigEndian) return BinaryPrimitives.ReverseEndianness(_reader.ReadInt32());

			return _reader.ReadInt32();
		}

		public void WriteBe(int value)
		{
			_writer.Write(BinaryPrimitives.ReverseEndianness(value));
		}

		public int ReadIntBe()
		{
			return BinaryPrimitives.ReverseEndianness(_reader.ReadInt32());
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
			return VarInt.ReadInt32(_reader);
		}

		public void WriteSignedVarInt(int value)
		{
			VarInt.WriteSInt32(_buffer, value);
		}

		public int ReadSignedVarInt()
		{
			return VarInt.ReadSInt32(_reader);
		}

		public void WriteUnsignedVarInt(uint value)
		{
			VarInt.WriteUInt32(_buffer, value);
		}

		public uint ReadUnsignedVarInt()
		{
			return VarInt.ReadUInt32(_reader);
		}

		public int ReadLength()
		{
			return (int) VarInt.ReadUInt32(_reader);
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
			return VarInt.ReadInt64(_reader);
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
			return VarInt.ReadSInt64(_reader);
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
			return (long) VarInt.ReadUInt64(_reader);
		}

		// Unlike every other fixed-width type here, the plain Write/Read for long is big-endian.
		// That is what RakNet wants (GUIDs, ping timestamps) and it is the only reason the default
		// points that way. Bedrock's own 64-bit fields are little-endian, so those use WriteLe/
		// ReadLongLe, either directly or via endianess="LE" on the field in MCPE Protocol.xml.
		public void Write(long value)
		{
			_writer.Write(BinaryPrimitives.ReverseEndianness(value));
		}

		public long ReadLong()
		{
			return BinaryPrimitives.ReverseEndianness(_reader.ReadInt64());
		}

		public void WriteLe(long value)
		{
			_writer.Write(value);
		}

		public long ReadLeLong()
		{
			return _reader.ReadInt64();
		}

		public long ReadLongLe()
		{
			return _reader.ReadInt64();
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

		public void Write(double value)
		{
			_writer.Write(value);
		}

		public double ReadDouble()
		{
			return _reader.ReadDouble();
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
			if (_reader.Position == _reader.Length) return string.Empty;
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
			if (_reader.Position == _reader.Length) return string.Empty;
			short len = ReadShort(true);
			if (len <= 0) return string.Empty;
			return Encoding.UTF8.GetString(_reader.Read(len).Span);
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
			WriteSignedVarInt(coord.Y);
			WriteSignedVarInt(coord.Z);
		}

		public BlockCoordinates ReadBlockCoordinates()
		{
			return new BlockCoordinates(ReadSignedVarInt(), ReadSignedVarInt(), ReadSignedVarInt());
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
			var d = 256f / 360f;
			Write((byte) Math.Round(location.Pitch * d)); // 256/360
			Write((byte) Math.Round(location.Yaw * d)); // 256/360
			Write((byte) Math.Round(location.HeadYaw * d)); // 256/360
		}

		public PlayerLocation ReadPlayerLocation()
		{
			PlayerLocation location = new PlayerLocation();
			location.X = ReadFloat();
			location.Y = ReadFloat();
			location.Z = ReadFloat();
			location.Pitch = ReadByte() * 1f / 0.71f;
			location.Yaw = ReadByte() * 1f / 0.71f;
			location.HeadYaw = ReadByte() * 1f / 0.71f;

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
					Write((byte) ~byte.Parse(part));
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
				string ipAddress = $"{(byte) ~ReadByte()}.{(byte) ~ReadByte()}.{(byte) ~ReadByte()}.{(byte) ~ReadByte()}";
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
			if (count == 20 && _reader.Length < 120) count = 10;
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

		public void Write(CommandOriginData originData)
		{
			if (originData == null) throw new Exception("Expected CommandOriginData, required");

			Write(CommandOriginData.GetTypeName(originData.Type));
			Write(originData.UUID);
			Write(originData.RequestId);
			Write((ulong) originData.EntityUniqueId);
		}

		public CommandOriginData ReadCommandOriginData()
		{
			CommandOriginType type = CommandOriginData.GetTypeFromName(ReadString());
			UUID uuid = ReadUUID();
			string requestId = ReadString();
			long entityUniqueId = (long) ReadUlong();

			return new CommandOriginData(type, uuid, requestId, entityUniqueId);
		}

		public void Write(Nbt nbt)
		{
			Write(nbt, _writer.BaseStream, nbt.NbtFile.UseVarInt || this is McpeBlockEntityData || this is McpeUpdateEquipment || this is McpeUpdateTrade);
		}

		public static void Write(Nbt nbt, Stream stream, bool useVarInt)
		{
			NbtFile file = nbt.NbtFile;
			file.BigEndian = false;
			file.UseVarInt = useVarInt;

			byte[] saveToBuffer = file.SaveToBuffer(NbtCompression.None);
			stream.Write(saveToBuffer, 0, saveToBuffer.Length);
		}


		public Nbt ReadNbt()
		{
			return ReadNbt(_reader);
		}

		public static Nbt ReadNbt(Stream stream, bool allowAlternativeRootTag = true, bool useVarInt = true)
		{
			Nbt nbt = new Nbt();
			NbtFile nbtFile = new NbtFile();
			nbtFile.BigEndian = false;
			nbtFile.UseVarInt = useVarInt;
			nbtFile.AllowAlternativeRootTag = allowAlternativeRootTag;

			nbt.NbtFile = nbtFile;
			nbtFile.LoadFromStream(stream, NbtCompression.None);

			return nbt;
		}

		/// <summary>
		///     A compound written as its contents only, tags then TAG_End, with no root type byte
		///     and no root name, running to the end of the packet. LevelEventGeneric carries its
		///     payload this way.
		/// </summary>
		public void WriteNbtBody(NbtCompound compound)
		{
			if (compound == null) return;

			var file = new NbtFile(compound) {BigEndian = false, UseVarInt = true};
			byte[] buffer = file.SaveToBuffer(NbtCompression.None);

			// Drop the root header fNbt writes: the compound type byte and its zero-length name.
			const int rootHeader = 2;
			_writer.Write(buffer, rootHeader, buffer.Length - rootHeader);
		}

		public NbtCompound ReadNbtBody()
		{
			byte[] body = ReadBytes(0, true);
			if (body.Length == 0) return null;

			// Put the root header back so fNbt has something it recognises.
			var framed = new byte[body.Length + 2];
			framed[0] = 10; // TAG_Compound
			framed[1] = 0; // zero-length name
			Buffer.BlockCopy(body, 0, framed, 2, body.Length);

			var file = new NbtFile {BigEndian = false, UseVarInt = true};
			file.LoadFromBuffer(framed, 0, framed.Length, NbtCompression.None);
			return (NbtCompound) file.RootTag;
		}

		public static NbtCompound ReadNbtCompound(Stream stream, bool useVarInt = false)
		{
			NbtFile file = new NbtFile();
			file.BigEndian = false;
			file.UseVarInt = useVarInt;
			file.AllowAlternativeRootTag = false;

			file.LoadFromStream(stream, NbtCompression.None);

			return (NbtCompound) file.RootTag;
		}

		public void Write(MetadataInts metadata)
		{
			if (metadata == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint) metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				MetadataInt slot = metadata[i] as MetadataInt;
				if (slot != null)
				{
					WriteUnsignedVarInt((uint) slot.Value);
				}
			}
		}

		public MetadataInts ReadMetadataInts()
		{
			MetadataInts metadata = new MetadataInts();
			uint count = ReadUnsignedVarInt();

			for (byte i = 0; i < count; i++)
			{
				metadata[i] = new MetadataInt((int) ReadUnsignedVarInt());
			}

			return metadata;
		}

		public void Write(CreativeItemStacks itemStacks)
		{
			if (itemStacks == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}
			
			WriteUnsignedVarInt((uint) itemStacks.Count);

			foreach(var item in itemStacks)
			{
				WriteUnsignedVarInt((uint)item.NetworkId);
				Write(item, false);
			}
		}

		public CreativeItemStacks ReadCreativeItemStacks()
		{
			var metadata = new CreativeItemStacks();

			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				var networkId = ReadUnsignedVarInt();
				Item item = ReadItem(false);
				item.NetworkId = (int)networkId;
				metadata.Add(item);
				Log.Debug(item);
			}

			return metadata;
		}

		public void Write(ItemStacks itemStacks)
		{
			if (itemStacks == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint) itemStacks.Count);
			for (int i = 0; i < itemStacks.Count; i++)
			{
				Write(itemStacks[i]);
			}
		}

		public ItemStacks ReadItemStacks()
		{
			var metadata = new ItemStacks();

			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				int networkId = 0;
				if (this is McpeCreativeContent) networkId = ReadVarInt();
				Item item = ReadItem(this is not McpeCreativeContent);
				item.NetworkId = networkId;
				metadata.Add(item);
				Log.Debug(item);
			}

			return metadata;
		}


		public StackRequestSlotInfo ReadStackRequestSlotInfo()
		{
			var containerId    = (byte) ReadByte();
			// FullContainerName: optional dynamic container id (bool + lu32); unchanged at 2168
			// (gophertunnel encodes the same optional shape).
			if (ReadBool()) ReadUint();
			var slot           = (byte) ReadByte();
			var stackNetworkId = ReadInt(); // li32

			return new StackRequestSlotInfo()
			{
				ContainerId = containerId,
				Slot = slot,
				StackNetworkId = stackNetworkId
			};
		}
		
		public void Write(StackRequestSlotInfo slotInfo)
		{
			Write(slotInfo.ContainerId);
			Write(false); // FullContainerName: optional dynamic container id, none; unchanged at 2168
			Write(slotInfo.Slot);
			Write(slotInfo.StackNetworkId); // li32, matching the read
		}



		// Item registry entry, protocol 1001+ (packet id 162, "item_registry"):
		//   name: string, runtime_id: li16, component_based: bool, version: zigzag32, nbt: nbt
		public void Write(ItemComponentList list)
		{
			WriteUnsignedVarInt((uint) list.Count);

			foreach (var item in list)
			{
				Write(item.Name);
				Write(item.RuntimeId);
				Write(item.ComponentBased);
				WriteSignedVarInt(item.Version);

				// Entries from the generated registry carry the serialized bytes already, so they go
				// out as they are. Only an entry built at runtime has to be serialized here.
				if (item.RawNbt != null) Write(item.RawNbt);
				else Write(item.Nbt);
			}
		}

		public ItemComponentList ReadItemComponentList()
		{
			var               count = ReadUnsignedVarInt();
			ItemComponentList l     = new ItemComponentList();

			for (int i = 0; i < count; i++)
			{
				string        name           = ReadString();
				short         runtimeId      = ReadShort();
				bool          componentBased = ReadBool();
				int           version        = ReadSignedVarInt();
				var           nbt            = ReadNbt();

				ItemComponent component = new ItemComponent();
				component.Name = name;
				component.RuntimeId = runtimeId;
				component.ComponentBased = componentBased;
				component.Version = version;
				component.Nbt = nbt;

				l.Add(component);
			}

			return l;
		}
		
		public void Write(EnchantOptions options)
		{
			WriteUnsignedVarInt((uint) options.Count);
			foreach (EnchantOption option in options)
			{
				Write((byte) option.Cost); // u8, not a varint
				Write(option.Flags);
				WriteEnchants(option.EquipActivatedEnchantments);
				WriteEnchants(option.HeldActivatedEnchantments);
				WriteEnchants(option.SelfActivatedEnchantments);
				Write(option.Name);
				WriteVarInt(option.OptionId);
			}
		}

		private void WriteEnchants(List<Enchant> enchants)
		{
			WriteUnsignedVarInt((uint) enchants.Count);
			foreach (Enchant enchant in enchants)
			{
				WriteUnsignedVarInt((uint) enchant.Id); // enchant type id, unsigned varint since protocol 975
				Write(enchant.Level);
			}
		}

		private List<Enchant> ReadEnchants()
		{
			List<Enchant> enchants = new List<Enchant>();
			var           count    = ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				Enchant enchant = new Enchant((int) ReadUnsignedVarInt(), ReadByte());
				enchants.Add(enchant);
			}

			return enchants;
		}

		public EnchantOptions ReadEnchantOptions()
		{
			var options = new EnchantOptions();
			var count   = ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				EnchantOption option = new EnchantOption();
				option.Cost = ReadByte(); // u8, not a varint
				option.Flags = ReadInt();
				option.EquipActivatedEnchantments = ReadEnchants();
				option.HeldActivatedEnchantments = ReadEnchants();
				option.SelfActivatedEnchantments = ReadEnchants();
				option.Name = ReadString();
				option.OptionId = ReadVarInt();
				
				options.Add(option);
			}
			
			return options;
		}


		// The shield's network id, the one item the stack format treats specially: its extra-data
		// blob carries a "blocking_tick" trailer nothing else has. Resolved by name from the item
		// registry, so it follows the registry rather than being a number to keep up to date.
		private static readonly Lazy<int> ShieldNetworkId = new(() => ItemFactory.GetNetworkIdByName("minecraft:shield"));

		private static bool IsShieldNetworkId(int networkId)
		{
			return networkId == ShieldNetworkId.Value;
		}

		// Full inventory item stack ("ItemNew" / ItemV4), protocol 1001+, used by inventory_content,
		// inventory_slot, mob_equipment, and other container packets:
		//   network_id: li16, count: lu16, metadata: varint,
		//   has_stack_id: bool [+ empty: varint, id: zigzag32],
		//   block_runtime_id: varint,
		//   extra: varint-length-prefixed blob { has_nbt: lu16 (0xffff/0), [version: u8, nbt: lnbt],
		//                                        can_place_on: li32-count of ShortString, can_destroy: same,
		//                                        [blocking_tick: li64, shield only] }
		// network_id==0 (air) does NOT short-circuit; air is a full ~8-byte encoding. Confirmed against
		// live BDS 1.26.34 inventory_content/mob_equipment bytes. The zigzag short-circuit "Item" shape
		// (add_player held item, ReadNetworkItemStackDescriptor) and the catalog shape (creative content,
		// ReadNetworkItemInstanceDescriptor) are separate readers - do not conflate them.
		/// <summary>The write half of <see cref="ReadItem" />, delegating for the same reason.</summary>
		public void Write(Item stack, bool writeUniqueId = true)
		{
			WriteNetworkItemStackDescriptor(stack, writeUniqueId);
		}

		private static byte[] WriteItemExtraData(NbtCompound extraData, bool includeBlockingTick)
		{
			return WriteItemExtraData(extraData, includeBlockingTick, null, null, 0);
		}

		private static byte[] WriteItemExtraData(NbtCompound extraData, bool includeBlockingTick, List<string> canPlaceOn, List<string> canDestroy, long blockingTick)
		{
			using var ms = new MemoryStream();
			using (BinaryWriter binaryWriter = new BinaryWriter(ms, Encoding.UTF8, true))
			{
				if (extraData != null)
				{
					binaryWriter.Write((ushort) 0xffff);
					binaryWriter.Write((byte) 1);
					var nbtData = GetNbtData(extraData, false);
					binaryWriter.Write(nbtData);
				}
				else
				{
					binaryWriter.Write((ushort) 0);
				}

				binaryWriter.Write(canPlaceOn?.Count ?? 0); // can_place_on count
				if (canPlaceOn != null)
				{
					foreach (string name in canPlaceOn)
					{
						byte[] bytes = Encoding.UTF8.GetBytes(name);
						binaryWriter.Write((short) bytes.Length);
						binaryWriter.Write(bytes);
					}
				}

				binaryWriter.Write(canDestroy?.Count ?? 0); // can_destroy count
				if (canDestroy != null)
				{
					foreach (string name in canDestroy)
					{
						byte[] bytes = Encoding.UTF8.GetBytes(name);
						binaryWriter.Write((short) bytes.Length);
						binaryWriter.Write(bytes);
					}
				}

				if (includeBlockingTick)
				{
					binaryWriter.Write(blockingTick); // blocking_tick
				}
			}

			return ms.ToArray();
		}

		/// <summary>
		///     The same NetworkItemStackDescriptor as <see cref="ReadNetworkItemStackDescriptor" />,
		///     kept as the name the XML pdus and the hand-written partials call. Delegates so there is
		///     one implementation of the shape; fold the call sites over and delete this.
		/// </summary>
		public Item ReadItem(bool readUniqueId = true)
		{
			return ReadNetworkItemStackDescriptor(readUniqueId);
		}

		/// <summary>
		///     ItemStackRequestNetworkItemInstanceDescriptor: the item shape a client's stack request
		///     carries. The item is named rather than numbered and the trailer is length-prefixed, so
		///     neither ReadItem nor ReadNetworkItemInstanceDescriptor decodes it.
		/// </summary>

		// Mojang's NetworkItemInstanceDescriptor, what creative content and crafting outputs carry.
		// Nothing legacy about it whatever minecraft-data calls it: network_id is a zigzag varint that
		// short-circuits the rest of the fields when 0, there is no item-stack (unique) id, and
		// block_runtime_id is zigzag rather than plain varint.
		public Item ReadNetworkItemInstanceDescriptor()
		{
			int networkId = ReadSignedVarInt(); // network_id
			if (networkId == 0)
			{
				// Since 2168 an empty stack carries all its fields zeroed instead of
				// short-circuiting after the id (Cereal reflection serializes every field).
				ReadUshort(); // count
				ReadVarInt(); // metadata
				ReadSignedVarInt(); // block_runtime_id
				ReadItemExtraData(includeBlockingTick: false, out _, out _, out _);
				return new ItemAir();
			}

			ushort count = ReadUshort(); // count
			var metadata = ReadVarInt(); // metadata
			int blockRuntimeId = ReadSignedVarInt(); // block_runtime_id

			// Catalog/recipe item descriptors (creative content, crafting data) are static and don't carry
			// live inventory state - except the shield, which BDS includes a "blocking_tick" trailer for
			// even here (confirmed against live BDS 1.26.34 creative_content bytes).
			NbtCompound extraData = ReadItemExtraData(includeBlockingTick: IsShieldNetworkId(networkId), out var canPlaceOn, out var canDestroy, out var blockingTick);

			Item stack = ItemFactory.GetItemByNetworkId(networkId, (short) metadata, count);
			stack.RuntimeId = blockRuntimeId;
			stack.NetworkId = networkId;
			stack.NetworkMetadata = metadata;
			stack.ExtraData = extraData;
			stack.CanPlaceOn = canPlaceOn;
			stack.CanDestroy = canDestroy;
			stack.BlockingTick = blockingTick;

			return stack;
		}

		public void WriteNetworkItemInstanceDescriptor(Item stack)
		{
			// Name to network id is a single unambiguous lookup, so a decoded item and a server-built
			// one encode the same way. Metadata still comes off the decode when there was one: it is
			// aux data the registry says nothing about.
			// Air is a registry item (-158) but an empty slot is network id 0, which no item uses.
			int networkId = stack == null || stack.IsAir ? 0 : ItemFactory.GetNetworkIdByName(stack.Name);
			if (networkId == 0)
			{
				// Since 2168 an empty stack carries all its fields zeroed instead of
				// short-circuiting after the id (Cereal reflection serializes every field).
				WriteSignedVarInt(0); // network_id
				Write((ushort) 0); // count
				WriteVarInt(0); // metadata
				WriteSignedVarInt(0); // block_runtime_id
				WriteLength(0); // empty extra data blob
				return;
			}

			int metadata = stack.NetworkMetadata >= 0 ? stack.NetworkMetadata : stack.Metadata;

			WriteSignedVarInt(networkId); // network_id
			Write((ushort) stack.Count); // count
			WriteVarInt(metadata); // metadata
			WriteSignedVarInt(stack.RuntimeId); // block_runtime_id

			byte[] extraData = WriteItemExtraData(stack.ExtraData, includeBlockingTick: IsShieldNetworkId(networkId), stack.CanPlaceOn, stack.CanDestroy, stack.BlockingTick);
			WriteLength(extraData.Length);
			Write(extraData);
		}

		// Item stack with wrapper stack-id ("Item" / getItemStackWrapper), protocol 1001+, used by the
		// add_player held item. Like ReadNetworkItemInstanceDescriptor (zigzag network_id short-circuiting to air on 0,
		// zigzag block_runtime_id) but with a has_net_id bool and optional stack id between metadata and
		// block_runtime_id. Distinct from the li16 inventory descriptor (ReadItem). Confirmed against
		// PMMP CommonTypes::getItemStackWrapper and live BDS 1.26.34.
		public Item ReadNetworkItemStackDescriptor(bool readNetId = true)
		{
			// NetworkItemStackDescriptor since 2168: li16 network id, no air short-circuit
			// (empty stacks carry all fields zeroed), and the block runtime id is a plain varint
			// rather than zigzag.
			short networkId = ReadShort(); // network_id
			ushort count = ReadUshort(); // count
			var metadata = ReadVarInt(); // metadata

			bool hasNetId = ReadBool(); // has_net_id
			int uniqueId = 0;
			if (hasNetId)
			{
				uniqueId = ReadSignedVarInt(); // stack_id
			}

			int blockRuntimeId = ReadVarInt(); // block_runtime_id
			NbtCompound extraData = ReadItemExtraData(IsShieldNetworkId(networkId));

			if (networkId == 0) return new ItemAir();

			Item stack = ItemFactory.GetItemByNetworkId(networkId, (short) metadata, count);
			// Always assign, never only when present: Item.UniqueId defaults to Environment.TickCount,
			// so a stack that arrived without a net id would otherwise go back out carrying one.
			stack.UniqueId = readNetId ? uniqueId : 0;
			stack.RuntimeId = blockRuntimeId;
			stack.NetworkId = networkId;
			stack.NetworkMetadata = metadata;
			stack.ExtraData = extraData;

			return stack;
		}

		public void WriteNetworkItemStackDescriptor(Item stack, bool writeNetId = true)
		{
			// NetworkItemStackDescriptor since 2168: li16 network id, no air short-circuit
			// (empty stacks carry all fields zeroed), and the block runtime id is a plain varint
			// rather than zigzag.
			// Air is a registry item (-158) but an empty slot is network id 0, which no item uses.
			short networkId = stack == null || stack.IsAir ? (short) 0 : ItemFactory.GetNetworkIdByName(stack.Name);
			if (networkId == 0)
			{
				Write((short) 0); // network_id
				Write((ushort) 0); // count
				WriteVarInt(0); // metadata
				Write(false); // has_net_id
				WriteVarInt(0); // block_runtime_id
				WriteLength(0); // empty extra data blob
				return;
			}

			Write(networkId); // network_id
			Write((ushort) stack.Count); // count
			WriteVarInt(stack.Metadata); // metadata

			bool hasNetId = writeNetId && stack.UniqueId != 0;
			Write(hasNetId); // has_net_id
			if (hasNetId)
			{
				WriteSignedVarInt(stack.UniqueId); // stack_id
			}

			WriteVarInt(stack.RuntimeId); // block_runtime_id

			byte[] extraData = WriteItemExtraData(stack.ExtraData, IsShieldNetworkId(networkId));
			WriteLength(extraData.Length);
			Write(extraData);
		}

		private NbtCompound ReadItemExtraData(bool includeBlockingTick)
		{
			return ReadItemExtraData(includeBlockingTick, out _, out _, out _);
		}

		private NbtCompound ReadItemExtraData(bool includeBlockingTick, out List<string> canPlaceOn, out List<string> canDestroy, out long blockingTick)
		{
			int length = ReadLength();
			var data = ReadBytes(length);

			NbtCompound extraData = null;
			canPlaceOn = null;
			canDestroy = null;
			blockingTick = 0;
			if (data.Length > 0)
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader binaryReader = new BinaryReader(ms))
				{
					ushort nbtLen = binaryReader.ReadUInt16();
					if (nbtLen == 0xffff)
					{
						byte version = binaryReader.ReadByte();

						if (version != 1)
						{
							throw new Exception($"Fringe nbt version when reading item extra NBT: {version}");
						}

						extraData = ReadNbtCompound(ms, false);
					}
					else if (nbtLen > 0)
					{
						throw new Exception($"Fringe nbt length when reading item extra NBT: {nbtLen}");
					}

					int canPlace = binaryReader.ReadInt32();
					if (canPlace > 0) canPlaceOn = new List<string>(canPlace);
					for (int i = 0; i < canPlace; i++)
					{
						var l = binaryReader.ReadInt16();
						canPlaceOn.Add(Encoding.UTF8.GetString(binaryReader.ReadBytes(l)));
					}
					int canBreak = binaryReader.ReadInt32();
					if (canBreak > 0) canDestroy = new List<string>(canBreak);
					for (int i = 0; i < canBreak; i++)
					{
						var l = binaryReader.ReadInt16();
						canDestroy.Add(Encoding.UTF8.GetString(binaryReader.ReadBytes(l)));
					}

					if (includeBlockingTick) // shield only
					{
						blockingTick = binaryReader.ReadInt64(); // blocking_tick
					}
				}
			}

			return extraData;
		}


		public static byte[] GetNbtData(NbtCompound nbtCompound, bool useVarInt = true)
		{
			nbtCompound.Name = string.Empty;
			var file = new NbtFile(nbtCompound);
			file.BigEndian = false;
			file.UseVarInt = useVarInt;

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
			//_buffer.Position = _reader.Position;
			var reader = new BinaryReader(_reader);
			var dictionary = MetadataDictionary.FromStream(reader);
			//_reader.Position = (int) _buffer.Position;
			return dictionary;
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
					DefaultMinValue = ReadFloat(),
					DefaultMaxValue = ReadFloat(),
					Default = ReadFloat(),
					Name = ReadString(),
				};

				// Attribute modifiers (protocol 544+).
				uint modifierCount = ReadUnsignedVarInt();
				for (uint m = 0; m < modifierCount; m++)
				{
					attribute.Modifiers.Add(new PlayerAttributeModifier
					{
						Id = ReadString(),
						Name = ReadString(),
						Amount = ReadFloat(),
						Operation = ReadInt(),
						Operand = ReadInt(),
						Serializable = ReadBool()
					});
				}

				attributes.Add(attribute);
			}

			return attributes;
		}

		public void Write(PlayerAttributes attributes)
		{
			WriteUnsignedVarInt((uint) attributes.Count);
			foreach (PlayerAttribute attribute in attributes)
			{
				Write(attribute.MinValue);
				Write(attribute.MaxValue);
				Write(attribute.Value);
				Write(attribute.DefaultMinValue);
				Write(attribute.DefaultMaxValue);
				Write(attribute.Default);
				Write(attribute.Name);

				WriteUnsignedVarInt((uint) attribute.Modifiers.Count);
				foreach (PlayerAttributeModifier modifier in attribute.Modifiers)
				{
					Write(modifier.Id);
					Write(modifier.Name);
					Write(modifier.Amount);
					Write(modifier.Operation);
					Write(modifier.Operand);
					Write(modifier.Serializable);
				}
			}
		}


		public GameRules ReadGameRules()
		{
			GameRules gameRules = new GameRules();

			int count = ReadVarInt();
			for (int i = 0; i < count; i++)
			{
				string name = ReadString();
				bool isPlayerModifiable = ReadBool();
				var type = ReadUnsignedVarInt();
				switch (type)
				{
					case 1:
					{
						GameRule<bool> rule = new GameRule<bool>(name, ReadBool())
						{
							IsPlayerModifiable = isPlayerModifiable
						};
						gameRules.Add(rule);
						break;
					}
					case 2:
					{
						// Raw little-endian since 2168 (Cereal); was a varint before.
						GameRule<int> rule = new GameRule<int>(name, ReadInt())
						{
							IsPlayerModifiable = isPlayerModifiable
						};
						gameRules.Add(rule);
						break;
					}
					case 3:
					{
						GameRule<float> rule = new GameRule<float>(name, ReadFloat())
						{
							IsPlayerModifiable = isPlayerModifiable
						};
						gameRules.Add(rule);
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
				// Rule names go out exactly as defined; BDS 1.26.34 sends camelCase
				// ("doDayLightCycle"), and lowercasing does not match the vanilla wire data.
				Write(rule.Name);
				Write(rule.IsPlayerModifiable); // bool isPlayerModifiable

				if (rule is GameRule<bool>)
				{
					WriteUnsignedVarInt(1);
					Write(((GameRule<bool>) rule).Value);
				}
				else if (rule is GameRule<int>)
				{
					WriteUnsignedVarInt(2);
					// Raw little-endian since 2168 (Cereal); was a varint before.
					Write(((GameRule<int>) rule).Value);
				}
				else if (rule is GameRule<float>)
				{
					WriteUnsignedVarInt(3);
					Write(((GameRule<float>) rule).Value);
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
			foreach (EntityAttribute attribute in attributes)
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

				attributes.Add(attribute);
			}

			return attributes;
		}

		public BlockPalette ReadBlockPalette()
		{
			var  result = new BlockPalette();
			var count  = ReadUnsignedVarInt();

			for (int runtimeId = 0; runtimeId < count; runtimeId++)
			{
				var record = new BlockStateContainer();
				record.Id = record.RuntimeId = runtimeId;
				record.Name = ReadString();
				record.States = new List<IBlockState>();

				var nbt = ReadNbt(_reader);
				var rootTag = nbt.NbtFile.RootTag;

				foreach (var state in GetBlockStates(rootTag))
				{
					record.States.Add(state);
				}

				result.Add(record);
			}

			return result;
		}
		
		private IEnumerable<IBlockState> GetBlockStates(NbtTag tag)
		{
			switch (tag.TagType)
			{
				case NbtTagType.List:
				{
					foreach (var state in GetBlockStatesFromList((NbtList) tag))
						yield return state;
				} break;

				case NbtTagType.Compound:
				{
					foreach (var state in GetBlockStatesFromCompound((NbtCompound) tag))
						yield return state;
				} break;

				default:
				{
					if (TryGetStateFromTag(tag, out var state))
						yield return state;
				} break;
			}
		}

		private IEnumerable<IBlockState> GetBlockStatesFromCompound(NbtCompound list)
		{
			if (list.TryGet("states", out NbtTag states))
			{
				foreach (var state in GetBlockStates(states))
				{
					yield return state;
				}
			}
		}
		
		
		private IEnumerable<IBlockState> GetBlockStatesFromList(NbtList list)
		{
			foreach (NbtTag tag in list)
			{
				if (TryGetStateFromTag(tag, out var state))
				{
					yield return state;
				}
				else
				{
					foreach (var s in GetBlockStates(tag))
					{
						yield return s;
					}
				}
			}
		}

		private bool TryGetStateFromTag(NbtTag tag, out IBlockState state)
		{
			switch (tag.TagType)
			{
				case NbtTagType.Byte:
					state = new BlockStateByte()
					{
						Name = tag.Name, Value = tag.ByteValue
					};
					return true;

				case NbtTagType.Int:
					state = new BlockStateInt()
					{
						Name = tag.Name, Value = tag.IntValue
					};
					return true;

				case NbtTagType.String:
					state = new BlockStateString()
					{
						Name = tag.Name, Value = tag.StringValue
					};
					return true;
			}

			state = null;

			return false;
		}

		public void Write(BlockPalette palette)
		{
			if(palette == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}
			WriteUnsignedVarInt((uint)palette.Count);
			foreach (BlockStateContainer record in palette)
			{
				Write(record.Name);
				Write(record.StatesCacheNbt);
			}
		}

		// ActorLink: both ids are actor unique ids (zigzag varint64), and the element ends with a
		// little-endian float vehicle angular velocity. Confirmed against PMMP
		// CommonTypes::putEntityLink/getEntityLink and minecraft-data types.Link.
		public void Write(EntityLink link)
		{
			WriteSignedVarLong(link.FromEntityId);
			WriteSignedVarLong(link.ToEntityId);
			Write((byte)link.Type);
			Write(link.Immediate);
			Write(link.CausedByRider);
			Write(link.VehicleAngularVelocity);
		}

		public EntityLink ReadEntityLink()
		{
			var from = ReadSignedVarLong();
			var to = ReadSignedVarLong();
			var type = (EntityLink.EntityLinkType) ReadByte();
			var immediate = ReadBool();
			var causedByRider = ReadBool();
			var vehicleAngularVelocity = ReadFloat();

			return new EntityLink(from, to, type, immediate, causedByRider, vehicleAngularVelocity);
		}
		
		public void Write(EntityLinks links)
		{
			if (links == null)
			{
				WriteUnsignedVarInt(0); // LE
				return;
			}
			WriteUnsignedVarInt((uint) links.Count); // LE
			foreach (var link in links)
			{
				Write(link);
			}
		}

		public EntityLinks ReadEntityLinks()
		{
			var count = ReadUnsignedVarInt();

			var links = new EntityLinks();
			for (int i = 0; i < count; i++)
			{
				links.Add(ReadEntityLink());
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

		public void Write(TexturePackInfos packInfos)
		{
			if (packInfos == null)
			{
				_writer.Write((short) 0);

				return;
			}
			
			_writer.Write((short) packInfos.Count); // LE
			//WriteVarInt(packInfos.Count);
			foreach (var info in packInfos)
			{
				Write(new UUID(info.UUID ?? Guid.Empty.ToString()));
				Write(info.Version);
				Write(info.Size);
				Write(info.ContentKey);
				Write(info.SubPackName);
				Write(info.ContentIdentity);
				Write(info.HasScripts);
				Write(info.AddonPack);
				Write(info.RtxEnabled);
				Write(info.CdnUrl);
			}
		}

		public TexturePackInfos ReadTexturePackInfos()
		{
			int count = _reader.ReadInt16(); // LE
			//int count = ReadVarInt(); // LE

			var packInfos = new TexturePackInfos();
			for (int i = 0; i < count; i++)
			{
				var info            = new TexturePackInfo();
				var id              = ReadUUID();
				var version         = ReadString();
				var size            = ReadUlong();
				var encryptionKey   = ReadString();
				var subpackName     = ReadString();
				var contentIdentity = ReadString();
				var hasScripts      = ReadBool();
				var addonPack       = ReadBool();
				var rtxEnabled      = ReadBool();
				var cdnUrl          = ReadString();

				info.UUID = ((Guid) id).ToString();
				info.Version = version;
				info.Size = size;
				info.HasScripts = hasScripts;
				info.ContentKey = encryptionKey;
				info.SubPackName = subpackName;
				info.ContentIdentity = contentIdentity;
				info.AddonPack = addonPack;
				info.RtxEnabled = rtxEnabled;
				info.CdnUrl = cdnUrl;

				packInfos.Add(info);
			}

			return packInfos;
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
				Write(info.UUID);
				Write(info.Version);
				Write(info.Size);
				Write(info.ContentKey);
				Write(info.SubPackName);
				Write(info.ContentIdentity);
				Write(info.HasScripts);
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
				var encryptionKey = ReadString();
				var subpackName = ReadString();
				var contentIdentity = ReadString();
				var hasScripts = ReadBool();
				
				info.UUID = id;
				info.Version = version;
				info.Size = size;
				info.ContentKey = encryptionKey;
				info.SubPackName = subpackName;
				info.ContentIdentity = contentIdentity;
				info.HasScripts = hasScripts;
				
				packInfos.Add(info);
			}

			return packInfos;
		}

		public void Write(ResourcePackIdVersions packInfos)
		{
			if (packInfos == null || packInfos.Count == 0)
			{
				WriteUnsignedVarInt(0);
				return;
			}
			WriteUnsignedVarInt((uint) packInfos.Count); // LE
			foreach (var info in packInfos)
			{
				Write(info.Id);
				Write(info.Version);
				Write(info.SubPackName);
			}
		}

		public ResourcePackIdVersions ReadResourcePackIdVersions()
		{
			uint count = ReadUnsignedVarInt();

			var packInfos = new ResourcePackIdVersions();
			for (int i = 0; i < count; i++)
			{
				var id = ReadString();
				var version = ReadString();
				var subPackName = ReadString();
				var info = new LegacyPackIdVersion
				{
					Id = id,
					Version = version,
					SubPackName = subPackName
				};
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
			Write(skin.SkinId);
			Write(skin.PlayFabId);
			Write(skin.ResourcePatch);
			Write(skin.Width);
			Write(skin.Height);
			WriteByteArray(skin.Data);

			// List counts are varints since 2168 (Cereal reflected vectors); they were le32.
			WriteUnsignedVarInt((uint) (skin.Animations?.Count ?? 0));
			if (skin.Animations != null)
			{
				foreach (Animation animation in skin.Animations)
				{
					Write(animation.ImageWidth);
					Write(animation.ImageHeight);
					WriteByteArray(animation.Image);
					// AnimatedImageData carries both enums with x-serialization-options
					// ["Compression", "Enum-as-Value"], so they are varints, not le32.
					WriteUnsignedVarInt((uint) animation.Type);
					Write(animation.FrameCount);
					WriteUnsignedVarInt((uint) animation.Expression);
				}
			}

			Write(skin.Cape.ImageWidth);
			Write(skin.Cape.ImageHeight);
			WriteByteArray(skin.Cape.Data);
			Write(skin.GeometryData);
			Write(skin.GeometryDataVersion);
			Write(skin.AnimationData);

			Write(skin.Cape.Id);
			// The skin id again when we have nothing better, which is what vanilla BDS 1.26.34 sends
			// for a persona skin (full id == skin id, verified against a live capture). This used to
			// append a millisecond timestamp, so the same player's skin carried a different full id
			// every time the server mentioned them, and nothing keyed on it could ever match.
			Write(string.IsNullOrEmpty(skin.FullSkinId) ? skin.SkinId : skin.FullSkinId);
			// Since 2168 arm size is one byte (1 = wide, 0 = slim) and the skin color is a raw
			// ARGB le32; both were strings before. The model keeps the string forms from the JWT.
			Write((byte) ("wide".Equals(skin.ArmSize, StringComparison.OrdinalIgnoreCase) ? 1 : 0));
			Write(ParseSkinColor(skin.SkinColor));
			WriteUnsignedVarInt((uint) skin.PersonaPieces.Count);
			foreach (PersonaPiece piece in skin.PersonaPieces)
			{
				Write(piece.PieceId);
				// SerializedPersonaPieceHandle: PieceType is the enum's raw uint32 (Enum-as-Value
				// with no Compression), and PackId is an mce::UUID, sixteen bytes. Both used to go
				// out as the strings the login JWT names them with.
				Write((int) PersonaPieceTypes.Parse(piece.PieceType));
				WriteUuidBytes(piece.PackId);
				Write(piece.IsDefaultPiece);
				Write(piece.ProductId);
			}
			WriteUnsignedVarInt((uint) skin.SkinPieces.Count);
			foreach (SkinPiece skinPiece in skin.SkinPieces)
			{
				// PieceTintColors is an object, not an array: the type name is bare ("eyes", not
				// "persona_eyes") and the colours are a fixed four raw values with no count.
				Write(PersonaPieceTypes.ToTintName(PersonaPieceTypes.Parse(skinPiece.PieceType)));
				for (int i = 0; i < TintColorsPerPiece; i++)
				{
					Write(ParseSkinColor(i < skinPiece.Colors.Count ? skinPiece.Colors[i] : null));
				}
			}
			
			Write(skin.IsPremiumSkin);
			Write(skin.IsPersonaSkin);
			Write(skin.Cape.OnClassicSkin);
			Write(skin.IsPrimaryUser);
			Write(skin.OverrideAppearance); // overriding_player_appearance (protocol 1001+)
			// Since 2168 the trusted flag travels inside the skin, as a string of all things,
			// followed by the profile hash (SerializedSkin mProfileHash).
			Write(skin.IsVerified ? "true" : "false");
			Write(skin.ProfileHash ?? "");
		}

		/// <summary>
		///     A tint entry always carries four colours. PieceTintColors is an object in the schema,
		///     not an array, so there is no count on the wire and the slots a skin does not use are
		///     written as zero.
		/// </summary>
		private const int TintColorsPerPiece = 4;

		private static int ParseSkinColor(string color)
		{
			if (string.IsNullOrEmpty(color)) return 0;
			string hex = color.TrimStart('#');
			return uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out uint argb) ? (int) argb : 0;
		}

		/// <summary>An mce::UUID: sixteen raw bytes, not the string form the login JWT uses.</summary>
		private void WriteUuidBytes(string uuid)
		{
			Write(Guid.TryParse(uuid, out Guid guid) ? guid.ToByteArray() : new byte[16]);
		}

		private string ReadUuidBytes()
		{
			return new Guid(ReadBytes(16)).ToString();
		}

		public Skin ReadSkin()
		{
			Skin skin = new Skin();

			skin.SkinId = ReadString();
			skin.PlayFabId = ReadString();
			skin.ResourcePatch = ReadString();
			skin.Width = ReadInt();
			skin.Height = ReadInt();
			skin.Data = ReadByteArray(false);

			// List counts are varints since 2168 (Cereal reflected vectors); they were le32.
			int animationCount = (int) ReadUnsignedVarInt();
			for (int i = 0; i < animationCount; i++)
			{
				skin.Animations.Add(
					new Animation()
					{
						ImageWidth = ReadInt(),
						ImageHeight = ReadInt(),
						Image = ReadByteArray(false),
						Type = (int) ReadUnsignedVarInt(),
						FrameCount = ReadFloat(),
						Expression = (int) ReadUnsignedVarInt()
					}
				);
			}

			skin.Cape.ImageWidth = ReadInt();
			skin.Cape.ImageHeight = ReadInt();
			skin.Cape.Data = ReadByteArray(false);
			skin.GeometryData = ReadString();
			skin.GeometryDataVersion = ReadString();
			skin.AnimationData = ReadString();

			skin.Cape.Id = ReadString();
			skin.FullSkinId = ReadString();
			// Since 2168 arm size is one byte (1 = wide, 0 = slim) and the skin color is a raw
			// ARGB le32; both were strings before. The model keeps the string forms from the JWT.
			skin.ArmSize = ReadByte() == 1 ? "wide" : "slim";
			skin.SkinColor = "#" + ((uint) ReadInt()).ToString("x8");
			int personaPieceCount = (int) ReadUnsignedVarInt();
			for (int i = 0; i < personaPieceCount; i++)
			{
				var p = new PersonaPiece();
				p.PieceId = ReadString();
				p.PieceType = PersonaPieceTypes.ToClientDataName((PersonaPieceType) ReadInt());
				p.PackId = ReadUuidBytes();
				p.IsDefaultPiece = ReadBool();
				p.ProductId = ReadString();
				skin.PersonaPieces.Add(p);
			}

			int skinPieceCount = (int) ReadUnsignedVarInt();
			for (int i = 0; i < skinPieceCount; i++)
			{
				var piece = new SkinPiece();
				piece.PieceType = PersonaPieceTypes.ToClientDataName(PersonaPieceTypes.Parse(ReadString()));
				for (int i2 = 0; i2 < TintColorsPerPiece; i2++)
				{
					piece.Colors.Add("#" + ((uint) ReadInt()).ToString("x6"));
				}
				skin.SkinPieces.Add(piece);
			}
			
			skin.IsPremiumSkin = ReadBool();
			skin.IsPersonaSkin = ReadBool();
			skin.Cape.OnClassicSkin = ReadBool();
			skin.IsPrimaryUser = ReadBool();
			skin.OverrideAppearance = ReadBool(); // overriding_player_appearance (protocol 1001+)
			// Since 2168 the trusted flag travels inside the skin, as a string of all things,
			// followed by the profile hash (SerializedSkin mProfileHash).
			skin.IsVerified = "true".Equals(ReadString(), StringComparison.OrdinalIgnoreCase);
			skin.ProfileHash = ReadString();
			//Log.Debug($"SkinId={skin.SkinId}");
			//Log.Debug($"SkinData lenght={skin.Data.Length}");
			//Log.Debug($"CapeData lenght={skin.Cape.Data.Length}");
			//Log.Debug("\n" + HexDump(skin.Cape.Data));
			//Log.Debug($"SkinGeometryName={skin.GeometryName}");
			//Log.Debug($"SkinGeometry lenght={skin.GeometryData.Length}");

			return skin;
		}

		const int Shapeless = 0;
		const int Shaped = 1;
		const int Furnace = 2;
		const int FurnaceData = 3;
		const int Multi = 4;
		const int ShulkerBox = 5;
		const int ShapelessChemistry = 6;
		const int ShapedChemistry = 7;
		const int SmithingTransform = 8;
		const int SmithingTrim = 9;

		// Recipe wire shape, protocol 1001 (unlocking requirements added at 685, recipe network ids
		// and smithing recipes added later; the type discriminator codes above are unchanged from
		// protocol 503). Item stacks inside recipes are NetworkItemInstanceDescriptor (see
		// WriteNetworkItemInstanceDescriptor/ReadNetworkItemInstanceDescriptor), not the li16 descriptor inventory packets use.
		public void Write(Recipes recipes)
		{
			// Since 2168 recipes travel as separate vectors per type (Cereal), in a fixed order,
			// instead of one type-tagged list. The furnace entry types (2 and 3) were removed from
			// the protocol at 962, so a smelting recipe still has no wire representation and stays
			// server-side only.
			var shaped = new List<ShapedRecipe>();
			var shapeless = new List<ShapelessRecipe>();
			var multi = new List<MultiRecipe>();
			var shulkerBox = new List<ShapelessRecipe>();
			var shapelessChemistry = new List<ShapelessRecipe>();
			var shapedChemistry = new List<ShapedRecipe>();
			var smithingTransform = new List<SmithingTransformRecipe>();
			var smithingTrim = new List<SmithingTrimRecipe>();

			foreach (Recipe recipe in recipes)
			{
				switch (recipe)
				{
					case ShapedRecipe r when r.RecipeType == ShapedChemistry:
						shapedChemistry.Add(r);
						break;
					case ShapedRecipe r:
						shaped.Add(r);
						break;
					case ShapelessRecipe r when r.RecipeType == ShulkerBox:
						shulkerBox.Add(r);
						break;
					case ShapelessRecipe r when r.RecipeType == ShapelessChemistry:
						shapelessChemistry.Add(r);
						break;
					case ShapelessRecipe r:
						shapeless.Add(r);
						break;
					case MultiRecipe r:
						multi.Add(r);
						break;
					case SmithingTransformRecipe r:
						smithingTransform.Add(r);
						break;
					case SmithingTrimRecipe r:
						smithingTrim.Add(r);
						break;
				}
			}

			WriteUnsignedVarInt((uint) shaped.Count);
			foreach (ShapedRecipe rec in shaped) WriteShapedRecipeBody(rec, carriesRequirement: true);

			WriteUnsignedVarInt((uint) shapeless.Count);
			foreach (ShapelessRecipe rec in shapeless) WriteShapelessRecipeBody(rec, carriesRequirement: true);

			WriteUnsignedVarInt((uint) multi.Count);
			foreach (MultiRecipe rec in multi)
			{
				Write(rec.Id);
				WriteVarInt(rec.NetworkId); // network id
			}

			WriteUnsignedVarInt((uint) shulkerBox.Count);
			foreach (ShapelessRecipe rec in shulkerBox) WriteShapelessRecipeBody(rec, carriesRequirement: true);

			WriteUnsignedVarInt((uint) shapelessChemistry.Count);
			foreach (ShapelessRecipe rec in shapelessChemistry) WriteShapelessRecipeBody(rec, carriesRequirement: false);

			WriteUnsignedVarInt((uint) shapedChemistry.Count);
			foreach (ShapedRecipe rec in shapedChemistry) WriteShapedRecipeBody(rec, carriesRequirement: false);

			WriteUnsignedVarInt((uint) smithingTransform.Count);
			foreach (SmithingTransformRecipe rec in smithingTransform)
			{
				WriteLatinString(rec.RecipeId);
				WriteRecipeIngredient(rec.Template);
				WriteRecipeIngredient(rec.Base);
				WriteRecipeIngredient(rec.Addition);
				WriteNetworkItemInstanceDescriptor(rec.Result);
				Write(rec.Tag);
				WriteVarInt(rec.NetworkId); // network id
			}

			WriteUnsignedVarInt((uint) smithingTrim.Count);
			foreach (SmithingTrimRecipe rec in smithingTrim)
			{
				WriteLatinString(rec.RecipeId);
				WriteRecipeIngredient(rec.Template);
				WriteRecipeIngredient(rec.Input);
				WriteRecipeIngredient(rec.Addition);
				Write(rec.Block);
				WriteVarInt(rec.NetworkId); // network id
			}
		}

		private void WriteShapedRecipeBody(ShapedRecipe rec, bool carriesRequirement)
		{
			WriteLatinString(rec.RecipeId);
			WriteSignedVarInt(rec.Width);
			WriteSignedVarInt(rec.Height);
			// Counted since 2168; the count must equal width*height and the element order is
			// unchanged (column-major, as verified against BDS at 1001).
			WriteUnsignedVarInt((uint) (rec.Width * rec.Height));
			for (int w = 0; w < rec.Width; w++)
			{
				for (int h = 0; h < rec.Height; h++)
				{
					WriteRecipeIngredient(rec.Input[(h * rec.Width) + w]);
				}
			}
			WriteVarInt(rec.Result.Count);
			foreach (Item item in rec.Result)
			{
				WriteNetworkItemInstanceDescriptor(item);
			}
			Write(rec.Id);
			Write(rec.Block);
			WriteSignedVarInt(rec.Priority);
			Write(rec.AssumeSymmetry);
			// Presence bool since 2168; vanilla sends the requirement on plain shaped/shapeless
			// recipes and false on the chemistry variants.
			Write(carriesRequirement);
			if (carriesRequirement) WriteUnlockingRequirement(rec.Unlocking);
			WriteVarInt(rec.NetworkId); // network id
		}

		private void WriteShapelessRecipeBody(ShapelessRecipe rec, bool carriesRequirement)
		{
			WriteLatinString(rec.RecipeId);
			WriteVarInt(rec.Input.Count);
			foreach (Item stack in rec.Input)
			{
				WriteRecipeIngredient(stack);
			}
			WriteVarInt(rec.Result.Count);
			foreach (Item item in rec.Result)
			{
				WriteNetworkItemInstanceDescriptor(item);
			}
			Write(rec.Id);
			Write(rec.Block);
			WriteSignedVarInt(rec.Priority);
			Write(carriesRequirement);
			if (carriesRequirement) WriteUnlockingRequirement(rec.Unlocking);
			WriteVarInt(rec.NetworkId); // network id
		}

		// context 0 ("none") is the only unlocking-requirement context that carries an ingredients
		// array; every other context (always_unlocked, player_in_water, player_has_many_items) has
		// no further data. Always-unlocked (context 1) is the only sensible default for recipes MiNET
		// builds itself: it has no notion of which items should unlock a recipe.
		private void WriteUnlockingRequirement(UnlockingRequirement requirement)
		{
			requirement ??= new UnlockingRequirement();
			// Context is a zigzag varint since 2168 (was one byte), and the ingredients array sits
			// behind its own presence bool, sent true only for context 0 ("none").
			WriteSignedVarInt(requirement.Context);
			bool carriesIngredients = requirement.Context == 0;
			Write(carriesIngredients);
			if (carriesIngredients)
			{
				var ingredients = requirement.Ingredients ?? new List<Item>();
				WriteUnsignedVarInt((uint) ingredients.Count);
				foreach (Item item in ingredients)
				{
					WriteRecipeIngredient(item);
				}
			}
		}

		private UnlockingRequirement ReadUnlockingRequirement()
		{
			var requirement = new UnlockingRequirement {Context = ReadSignedVarInt()};
			if (ReadBool())
			{
				uint count = ReadUnsignedVarInt();
				requirement.Ingredients = new List<Item>((int) count);
				for (int i = 0; i < count; i++)
				{
					requirement.Ingredients.Add(ReadRecipeIngredient());
				}
			}
			return requirement;
		}

		public Recipes ReadRecipes()
		{
			// Since 2168 recipes travel as separate vectors per type (Cereal), in a fixed order;
			// the per-recipe type tag is gone and the furnace types no longer exist on the wire.
			var recipes = new Recipes();

			uint shapedCount = ReadUnsignedVarInt();
			for (int i = 0; i < shapedCount; i++)
			{
				var recipe = ReadShapedLikeRecipe();
				recipe.RecipeType = Shaped;
				recipes.Add(recipe);
			}

			uint shapelessCount = ReadUnsignedVarInt();
			for (int i = 0; i < shapelessCount; i++)
			{
				var recipe = ReadShapelessLikeRecipe();
				recipe.RecipeType = Shapeless;
				recipes.Add(recipe);
			}

			uint multiCount = ReadUnsignedVarInt();
			for (int i = 0; i < multiCount; i++)
			{
				var recipe = new MultiRecipe();
				recipe.Id = ReadUUID();
				recipe.NetworkId = ReadVarInt(); // network id
				recipes.Add(recipe);
			}

			uint shulkerBoxCount = ReadUnsignedVarInt();
			for (int i = 0; i < shulkerBoxCount; i++)
			{
				var recipe = ReadShapelessLikeRecipe();
				recipe.RecipeType = ShulkerBox;
				recipes.Add(recipe);
			}

			uint shapelessChemistryCount = ReadUnsignedVarInt();
			for (int i = 0; i < shapelessChemistryCount; i++)
			{
				var recipe = ReadShapelessLikeRecipe();
				recipe.RecipeType = ShapelessChemistry;
				recipes.Add(recipe);
			}

			uint shapedChemistryCount = ReadUnsignedVarInt();
			for (int i = 0; i < shapedChemistryCount; i++)
			{
				var recipe = ReadShapedLikeRecipe();
				recipe.RecipeType = ShapedChemistry;
				recipes.Add(recipe);
			}

			uint smithingTransformCount = ReadUnsignedVarInt();
			for (int i = 0; i < smithingTransformCount; i++)
			{
				var recipe = new SmithingTransformRecipe();
				recipe.RecipeId = ReadLatinString(); // recipe id
				recipe.Template = ReadRecipeIngredient();
				recipe.Base = ReadRecipeIngredient();
				recipe.Addition = ReadRecipeIngredient();
				recipe.Result = ReadNetworkItemInstanceDescriptor();
				recipe.Tag = ReadString();
				recipe.NetworkId = ReadVarInt(); // network id
				recipes.Add(recipe);
			}

			uint smithingTrimCount = ReadUnsignedVarInt();
			for (int i = 0; i < smithingTrimCount; i++)
			{
				var recipe = new SmithingTrimRecipe();
				recipe.RecipeId = ReadLatinString(); // recipe id
				recipe.Template = ReadRecipeIngredient();
				recipe.Input = ReadRecipeIngredient();
				recipe.Addition = ReadRecipeIngredient();
				recipe.Block = ReadString();
				recipe.NetworkId = ReadVarInt(); // network id
				recipes.Add(recipe);
			}

			Log.Trace($"Done reading {recipes.Count} recipes");

			return recipes;
		}

		private ShapelessRecipe ReadShapelessLikeRecipe()
		{
			var recipe = new ShapelessRecipe();
			recipe.RecipeId = ReadLatinString(); // recipe id
			var ingredientCount = ReadUnsignedVarInt();
			for (int j = 0; j < ingredientCount; j++)
			{
				recipe.Input.Add(ReadRecipeIngredient());
			}
			var resultCount = ReadUnsignedVarInt();
			for (int j = 0; j < resultCount; j++)
			{
				recipe.Result.Add(ReadNetworkItemInstanceDescriptor());
			}
			recipe.Id = ReadUUID();
			recipe.Block = ReadString();
			recipe.Priority = ReadSignedVarInt();
			// Presence bool since 2168; false on the chemistry variants.
			if (ReadBool()) recipe.Unlocking = ReadUnlockingRequirement();
			recipe.NetworkId = ReadVarInt(); // network id
			return recipe;
		}

		private ShapedRecipe ReadShapedLikeRecipe()
		{
			string recipeId = ReadLatinString(); // recipe id
			int width = ReadSignedVarInt();
			int height = ReadSignedVarInt();
			var recipe = new ShapedRecipe(width, height);
			recipe.RecipeId = recipeId;
			if (width > 3 || height > 3)
				throw new Exception("Wrong number of ingredients, Width=" + width + ", height=" + height);
			// Counted since 2168; the count must equal width*height, element order unchanged.
			uint inputCount = ReadUnsignedVarInt();
			if (inputCount != width * height)
				throw new Exception($"Shaped recipe input count {inputCount} does not match {width}x{height}");
			for (int w = 0; w < width; w++)
			{
				for (int h = 0; h < height; h++)
				{
					recipe.Input[(h * width) + w] = ReadRecipeIngredient();
				}
			}

			var resultCount = ReadUnsignedVarInt();
			for (int j = 0; j < resultCount; j++)
			{
				recipe.Result.Add(ReadNetworkItemInstanceDescriptor());
			}
			recipe.Id = ReadUUID();
			recipe.Block = ReadString();
			recipe.Priority = ReadSignedVarInt();
			recipe.AssumeSymmetry = ReadBool();
			// Presence bool since 2168; false on the chemistry variants.
			if (ReadBool()) recipe.Unlocking = ReadUnlockingRequirement();
			recipe.NetworkId = ReadVarInt(); // network id
			return recipe;
		}

		// RecipeIngredient: a type-discriminated union (int id+meta / molang / item tag / string
		// id+meta / complex alias) followed by a zigzag32 stack count. The descriptor on the item
		// decides the variant; an item without one is the plain int-id-meta variant, written from the
		// id it was decoded with. Registry-built ingredients (recipes.json, plugins) carry a type-1
		// descriptor naming the item, so the network id is resolved from the registry string id
		// instead of a stored number.
		public void WriteRecipeIngredient(Item stack)
		{
			// Since 2168 the ingredient descriptor is name-addressed: the numeric tag is 0 for an
			// empty slot and 1 for anything real, and a serialize-name string selects the kind.
			// The numeric int_id_meta variant is gone, which suits MiNET: recipe data is name-based
			// already. Vanilla writes aux 32767 on the invalid and item_tag kinds.
			var descriptor = stack?.IngredientDescriptor;
			if (descriptor != null)
			{
				WriteUnsignedVarInt(1);
				switch (descriptor.Type)
				{
					case 2: // molang
						Write("molang");
						Write(descriptor.Text); // expression
						Write((short) descriptor.MolangVersion); // version
						break;
					case 3: // item_tag
						Write("item_tag");
						Write(descriptor.Text); // tag
						WriteSignedVarInt(32767); // aux
						break;
					case 4: // string_id_meta
						Write("name");
						Write(descriptor.Text); // name
						WriteSignedVarInt(descriptor.Metadata);
						break;
					case 5: // complex_alias
						Write("complex_alias");
						Write(descriptor.Text); // name
						break;
					default: // 1: by registry string id
						Write("name");
						Write(descriptor.Name);
						WriteSignedVarInt(descriptor.Metadata);
						break;
				}
				WriteSignedVarInt(stack.Count);
				return;
			}

			if (stack == null || stack.IsAir)
			{
				WriteUnsignedVarInt(0); // invalid = empty slot
				WriteSignedVarInt(32767); // aux
				WriteSignedVarInt(stack?.Count ?? 0); // count
				return;
			}

			WriteUnsignedVarInt(1);
			Write("name");
			Write(stack.Name);
			WriteSignedVarInt(stack.Metadata);
			WriteSignedVarInt(stack.Count == 0 ? 1 : stack.Count);
		}

		/// <summary>
		///     The ingredient shape an item stack request carries. The type varint keys the payload
		///     directly and the count is li16, where <see cref="ReadRecipeIngredient" /> keys off a
		///     "name"/"molang"/"item_tag" string and counts with a varint.
		/// </summary>
		/// <summary>The write half of <see cref="ReadItemStackRequestIngredient" />.</summary>
		public void WriteItemStackRequestIngredient(Item item)
		{
			RecipeIngredientDescriptor descriptor = item?.IngredientDescriptor;
			uint type = descriptor?.Type ?? 0u;

			WriteUnsignedVarInt(type);
			Write((byte) type);

			switch (type)
			{
				case 1:
					Write(descriptor.Name);
					WriteSignedVarInt(descriptor.Metadata);
					break;
				case 2:
					Write(descriptor.Text);
					Write((short) descriptor.MolangVersion);
					break;
				case 3:
					Write(descriptor.Text);
					break;
			}

			Write((ushort) (item?.Count ?? 0));
		}

		public Item ReadItemStackRequestIngredient()
		{
			uint type = ReadUnsignedVarInt();
			ReadByte(); // the enum again, as a byte

			Item item;
			switch (type)
			{
				case 1: // default: a named item and its aux value
				{
					string name = ReadString();
					int metadata = ReadSignedVarInt();
					item = ItemFactory.GetItemByName(name, (short) metadata) ?? new ItemAir();
					item.IngredientDescriptor = new RecipeIngredientDescriptor {Type = 1, Name = name, Metadata = (short) metadata};
					break;
				}
				case 2: // molang
				{
					string expression = ReadString();
					short version = ReadShort();
					item = new ItemAir {IngredientDescriptor = new RecipeIngredientDescriptor {Type = 2, Text = expression, MolangVersion = (byte) version}};
					break;
				}
				case 3: // item tag. No aux here, unlike the CraftingData shape.
				{
					string tag = ReadString();
					item = new ItemAir {IngredientDescriptor = new RecipeIngredientDescriptor {Type = 3, Text = tag}};
					break;
				}
				default: // invalid, an empty slot
					item = new ItemAir();
					break;
			}

			item.Count = (byte) ReadUshort();

			return item;
		}

		public Item ReadRecipeIngredient()
		{
			uint type = ReadUnsignedVarInt();

			Item item;
			if (type == 0) // invalid = empty slot
			{
				ReadSignedVarInt(); // aux, vanilla writes 32767
				item = new ItemAir();
			}
			else
			{
				string kind = ReadString();
				switch (kind)
				{
					case "name":
					{
						string name = ReadString();
						int metadata = ReadSignedVarInt();
						item = ItemFactory.GetItemByName(name, (short) metadata);
						item.IngredientDescriptor = new RecipeIngredientDescriptor {Type = 1, Name = name, Metadata = (short) metadata};
						break;
					}
					case "molang":
					{
						string expression = ReadString();
						short version = ReadShort();
						item = new ItemAir {IngredientDescriptor = new RecipeIngredientDescriptor {Type = 2, Text = expression, MolangVersion = (byte) version}};
						break;
					}
					case "item_tag":
					{
						string tag = ReadString();
						ReadSignedVarInt(); // aux, vanilla writes 32767
						item = new ItemAir {IngredientDescriptor = new RecipeIngredientDescriptor {Type = 3, Text = tag}};
						break;
					}
					case "complex_alias":
					{
						string name = ReadString();
						item = new ItemAir {IngredientDescriptor = new RecipeIngredientDescriptor {Type = 5, Text = name}};
						break;
					}
					default:
						throw new Exception($"Unknown recipe ingredient descriptor kind: {kind}");
				}
			}

			int count = ReadSignedVarInt();
			item.Count = (byte) count;

			return item;
		}

		private void WriteLatinString(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				WriteLength(0);
				return;
			}

			byte[] bytes = Encoding.Latin1.GetBytes(value);
			WriteLength(bytes.Length);
			Write(bytes);
		}

		private string ReadLatinString()
		{
			int len = ReadLength();
			if (len <= 0) return string.Empty;
			return Encoding.Latin1.GetString(ReadBytes(len));
		}

		public void Write(PotionContainerChangeRecipe[] recipes)
		{
			WriteUnsignedVarInt((uint) recipes.Length);
			foreach (var recipe in recipes)
			{
				WriteSignedVarInt(recipe.Input);
				WriteSignedVarInt(recipe.Ingredient);
				WriteSignedVarInt(recipe.Output);
			}
		}

		public PotionContainerChangeRecipe[] ReadPotionContainerChangeRecipes()
		{
			int count = (int) ReadUnsignedVarInt();
			var recipes = new PotionContainerChangeRecipe[count];
			for (int i = 0; i < recipes.Length; i++)
			{
				var recipe = new PotionContainerChangeRecipe();
				recipe.Input = ReadSignedVarInt();
				recipe.Ingredient = ReadSignedVarInt();
				recipe.Output = ReadSignedVarInt();

				recipes[i] = recipe;
			}

			return recipes;
		}

		// MaterialReducer's wire container only documents a single (network_id, count) output pair,
		// but MiNET's MaterialReducerRecipe models multiple outputs per input (matching how the
		// feature actually behaves in-game), so outputs are read/written as an array. No vanilla
		// capture with real material reducer recipes was available to confirm the exact framing;
		// this is the best-supported reading given MiNET's existing model.
		public void Write(MaterialReducerRecipe[] reducerRecipes)
		{
			WriteUnsignedVarInt((uint) reducerRecipes.Length);

			for (int i = 0; i < reducerRecipes.Length; i++)
			{
				var recipe = reducerRecipes[i];
				WriteSignedVarInt((recipe.Input << 16) | recipe.InputMeta);
				WriteUnsignedVarInt((uint) recipe.Output.Length);

				foreach (var output in recipe.Output)
				{
					WriteSignedVarInt(output.ItemId);
					WriteSignedVarInt(output.ItemCount);
				}
			}
		}

		public MaterialReducerRecipe[] ReadMaterialReducerRecipes()
		{
			int count = (int) ReadUnsignedVarInt();
			var recipes = new MaterialReducerRecipe[count];
			for (int i = 0; i < recipes.Length; i++)
			{
				var mix = ReadSignedVarInt();
				var inputId = mix >> 16;
				var inputMeta = mix & 0x7fff;

				var outputCount = (int) ReadUnsignedVarInt();
				MaterialReducerRecipe.MaterialReducerRecipeOutput[] outputs = new MaterialReducerRecipe.MaterialReducerRecipeOutput[outputCount];

				for (int o = 0; o < outputs.Length; o++)
				{
					var itemId = ReadSignedVarInt();
					var itemCount = ReadSignedVarInt();

					outputs[o] = new MaterialReducerRecipe.MaterialReducerRecipeOutput(itemId, itemCount);
				}

				var recipe = new MaterialReducerRecipe(inputId, inputMeta, outputs);

				recipes[i] = recipe;
			}

			return recipes;
		}

		public void Write(PotionTypeRecipe[] recipes)
		{
			WriteUnsignedVarInt((uint) recipes.Length);
			foreach (var recipe in recipes)
			{
				WriteSignedVarInt(recipe.Input);
				WriteSignedVarInt(recipe.InputMeta);
				WriteSignedVarInt(recipe.Ingredient);
				WriteSignedVarInt(recipe.IngredientMeta);
				WriteSignedVarInt(recipe.Output);
				WriteSignedVarInt(recipe.OutputMeta);
			}
		}

		public PotionTypeRecipe[] ReadPotionTypeRecipes()
		{
			int count = (int) ReadUnsignedVarInt();
			var recipes = new PotionTypeRecipe[count];
			for (int i = 0; i < recipes.Length; i++)
			{
				var recipe = new PotionTypeRecipe();
				recipe.Input = ReadSignedVarInt();
				recipe.InputMeta = ReadSignedVarInt();
				recipe.Ingredient = ReadSignedVarInt();
				recipe.IngredientMeta = ReadSignedVarInt();
				recipe.Output = ReadSignedVarInt();
				recipe.OutputMeta = ReadSignedVarInt();

				recipes[i] = recipe;
			}

			return recipes;
		}


		public Experiments ReadExperiments()
		{
			Experiments experiments = new Experiments();
			var count = ReadInt();

			for (int i = 0; i < count; i++)
			{
				var experimentName = ReadString();
				var enabled = ReadBool();
				experiments.Add(new Experiments.Experiment(experimentName, enabled));
			}
			// The trailing ever-toggled bool belongs to the wire type (Experiments in the 2168 schema).
			experiments.ExperimentsEverToggled = ReadBool();
			return experiments;
		}

		public void Write(Experiments experiments)
		{
			if (experiments == null)
			{
				Write(0);
				Write(false);
				return;
			}
			Write(experiments.Count);

			foreach (var experiment in experiments)
			{
				Write(experiment.Name);
				Write(experiment.Enabled);
			}

			Write(experiments.ExperimentsEverToggled);
		}

		public void Write(EducationUriResource resource)
		{
			Write(resource.ButtonName);
			Write(resource.LinkUri);
		}
		
		public EducationUriResource ReadEducationUriResource()
		{
			string name = ReadString();
			var uri = ReadString();

			return new EducationUriResource(name, uri);
		}

		public void Write(StructureSettings settings)
		{
			Write(settings.PaletteName);
			Write(settings.IgnoreEntities);
			Write(settings.IgnoreBlocks);
			Write(settings.AllowNonTickingChunks);
			Write(settings.Size);
			Write(settings.Offset);
			WriteSignedVarLong(settings.LastEditingPlayerUniqueId);
			Write(settings.Rotation);
			Write(settings.Mirror);
			Write(settings.AnimationMode);
			Write(settings.AnimationSeconds);
			Write(settings.IntegrityValue);
			Write(settings.IntegritySeed);
			Write(settings.Pivot);
		}

		public StructureSettings ReadStructureSettings()
		{
			return new StructureSettings
			{
				PaletteName = ReadString(),
				IgnoreEntities = ReadBool(),
				IgnoreBlocks = ReadBool(),
				AllowNonTickingChunks = ReadBool(),
				Size = ReadBlockCoordinates(),
				Offset = ReadBlockCoordinates(),
				LastEditingPlayerUniqueId = ReadSignedVarLong(),
				Rotation = ReadByte(),
				Mirror = ReadByte(),
				AnimationMode = ReadByte(),
				AnimationSeconds = ReadFloat(),
				IntegrityValue = ReadFloat(),
				IntegritySeed = ReadUint(),
				Pivot = ReadVector3()
			};
		}

		public void Write(UpdateSubChunkBlocksPacketEntry entry)
		{
			Write(entry.Coordinates);
			WriteUnsignedVarInt(entry.BlockRuntimeId);
			WriteUnsignedVarInt(entry.Flags);
			WriteUnsignedVarLong(entry.SyncedUpdatedEntityUniqueId);
			WriteUnsignedVarInt(entry.SyncedUpdateType);
		}

		public UpdateSubChunkBlocksPacketEntry ReadUpdateSubChunkBlocksPacketEntry()
		{
			var entry = new UpdateSubChunkBlocksPacketEntry();
			entry.Coordinates = ReadBlockCoordinates();
			entry.BlockRuntimeId = ReadUnsignedVarInt();
			entry.Flags = ReadUnsignedVarInt();
			entry.SyncedUpdatedEntityUniqueId = ReadUnsignedVarLong();
			entry.SyncedUpdateType = ReadUnsignedVarInt();

			return entry;
		}

		public void Write(UpdateSubChunkBlocksPacketEntry[] entries)
		{
			WriteUnsignedVarInt((uint) entries.Length);
			foreach(var entry in entries)
				Write(entry);
		}

		public UpdateSubChunkBlocksPacketEntry[] ReadUpdateSubChunkBlocksPacketEntrys()
		{
			var count = ReadUnsignedVarInt();
			UpdateSubChunkBlocksPacketEntry[] entries = new UpdateSubChunkBlocksPacketEntry[(int) count];

			for (int i = 0; i < entries.Length; i++)
			{
				entries[i] = ReadUpdateSubChunkBlocksPacketEntry();
			}

			return entries;
		}

		/// <summary>
		///     A sub-chunk heightmap: 16 rows of 16 int8 heights, each row carrying its own length
		///     prefix. The grid is fixed at 16x16, but the wire still counts every row, so 272 bytes
		///     travel for 256 values. Held flat, one row after another, because every caller builds
		///     and indexes it that way.
		/// </summary>
		public void WriteSubChunkHeightmap(byte[] heights)
		{
			for (int row = 0; row < SubChunkHeightmapRows; row++)
			{
				WriteUnsignedVarInt(SubChunkHeightmapRows);
				_writer.Write(heights, row * (int) SubChunkHeightmapRows, (int) SubChunkHeightmapRows);
			}
		}

		public byte[] ReadSubChunkHeightmap()
		{
			var heights = new byte[SubChunkHeightmapRows * SubChunkHeightmapRows];

			for (int row = 0; row < SubChunkHeightmapRows; row++)
			{
				uint count = ReadUnsignedVarInt();
				if (count != SubChunkHeightmapRows) throw new Exception($"Sub-chunk heightmap row {row} carries {count} heights, expected {SubChunkHeightmapRows}");

				ReadBytes((int) SubChunkHeightmapRows).CopyTo(heights, row * (int) SubChunkHeightmapRows);
			}

			return heights;
		}

		private const uint SubChunkHeightmapRows = 16;

		public void Write(PackSettingValue value)
		{
			if (value == null)
			{
				WriteUnsignedVarInt(0);
				Write(0f);
				return;
			}

			WriteUnsignedVarInt(value.TypeId);
			switch (value.TypeId)
			{
				case 0:
					Write(value.FloatValue);
					break;
				case 1:
					Write(value.BoolValue);
					break;
				case 2:
					Write(value.StringValue ?? string.Empty);
					break;
				case 3:
					WriteUnsignedVarInt((uint) (value.StringListValue?.Count ?? 0));
					if (value.StringListValue != null) foreach (string entry in value.StringListValue) Write(entry);
					break;
			}
		}

		public PackSettingValue ReadPackSettingValue()
		{
			var value = new PackSettingValue {TypeId = ReadUnsignedVarInt()};
			switch (value.TypeId)
			{
				case 0:
					value.FloatValue = ReadFloat();
					break;
				case 1:
					value.BoolValue = ReadBool();
					break;
				case 2:
					value.StringValue = ReadString();
					break;
				case 3:
					uint count = ReadUnsignedVarInt();
					value.StringListValue = new List<string>((int) count);
					for (int i = 0; i < count; i++) value.StringListValue.Add(ReadString());
					break;
			}

			return value;
		}

		public bool CanRead()
		{
			return _reader.Position < _reader.Length;
		}

		public void SetEncodedMessage(byte[] encodedMessage)
		{
			_encodedMessage = encodedMessage;
		}

		public virtual void Reset()
		{
			ResetPacket();

			NoBatch = false;
			CoalesceKey = null;

			_encodedMessage = null;

			_encodedLease?.Dispose();
			_encodedLease = null;
			if (_attachedLeases != null)
			{
				foreach (IDisposable lease in _attachedLeases) lease.Dispose();
				_attachedLeases = null;
			}

			_writer?.Close();
			_reader?.Close();
			_buffer?.Close();
			_writer = null;
			_reader = null;
			_buffer = null;
		}

		protected virtual void ResetPacket()
		{
		}

		private object _encodeSync = new object();

		// Blocks small enough that a tiny packet leasing one is cheap, large-buffer ceiling high
		// enough that a full 1000-player roster still comes from the pool instead of the LOH.
		private static RecyclableMemoryStreamManager _streamManager = new RecyclableMemoryStreamManager(new RecyclableMemoryStreamManager.Options
		{
			BlockSize = 16 * 1024,
			LargeBufferMultiple = 256 * 1024,
			MaximumBufferSize = 32 * 1024 * 1024,
			MaximumSmallPoolFreeBytes = 32 * 1024 * 1024,
			MaximumLargePoolFreeBytes = 128 * 1024 * 1024,
		});

		private MemoryStream _encodedLease;
		private List<IDisposable> _attachedLeases;

		/// <summary>
		///     Whether <see cref="EncodeAsMemory" /> may keep the encode output in a pooled buffer.
		///     Requires a guaranteed <see cref="Reset" /> at end of life, which only pooled,
		///     non-permanent packets have; everything else materializes to a plain array.
		/// </summary>
		public virtual bool UsesEncodeLease => false;

		/// <summary>
		///     Takes ownership of a pooled resource whose lifetime must match this packet's:
		///     disposed at <see cref="Reset" />, when the last reference returns to the pool.
		/// </summary>
		public void AttachLease(IDisposable lease)
		{
			(_attachedLeases ??= new List<IDisposable>()).Add(lease);
		}

		/// <summary>
		///     The encoded bytes, backed by a pooled buffer that lives until this packet returns to
		///     the pool. Never hold the returned memory past PutPool. Falls back to a plain array
		///     for packets whose end of life cannot release a lease (see <see cref="UsesEncodeLease" />).
		/// </summary>
		public ReadOnlyMemory<byte> EncodeAsMemory()
		{
			byte[] cache = _encodedMessage;
			if (cache != null) return cache;

			lock (_encodeSync)
			{
				if (_encodedMessage != null) return _encodedMessage;

				if (_encodedLease == null)
				{
					if (!UsesEncodeLease) return Encode();

					var stream = _streamManager.GetStream();
					_buffer = stream;
					using (_writer = new BinaryWriter(_buffer, Encoding.UTF8, true))
					{
						EncodePacket();
						_writer.Flush();
					}
					_writer = null;
					_buffer = null;
					_encodedLease = stream;
				}

				return new ReadOnlyMemory<byte>(_encodedLease.GetBuffer(), 0, (int) _encodedLease.Length);
			}
		}

		/// <summary>
		///     Copies a live encode lease into an owned array and releases the pooled buffer.
		///     Permanent packets never Reset, so they must never sit on pool memory.
		/// </summary>
		protected void MaterializeEncodedLease()
		{
			lock (_encodeSync)
			{
				if (_encodedLease == null) return;
				_encodedMessage ??= _encodedLease.ToArray();
				_encodedLease.Dispose();
				_encodedLease = null;
			}
		}

		/// <summary>
		///     Converts every pooled resource this packet sits on into owned arrays. Called when a
		///     packet becomes permanent, which is the one lifetime that never reaches Reset.
		///     Subclasses holding views over an attached lease (a wrapper's payload) must copy the
		///     view to an owned array before calling base, which disposes the leases.
		/// </summary>
		public virtual void MaterializePooledState()
		{
			MaterializeEncodedLease();

			if (_attachedLeases != null)
			{
				foreach (IDisposable lease in _attachedLeases) lease.Dispose();
				_attachedLeases = null;
			}
		}

		/// <summary>
		///     Redirects this packet's write plumbing at a caller-owned stream so the generated
		///     Write(...) serializers can produce a fragment of wire format outside the normal
		///     Encode flow. Only valid on a scratch packet instance that is not concurrently
		///     encoding; end with <see cref="EndFragmentEncode" />.
		/// </summary>
		protected void BeginFragmentEncode(Stream target)
		{
			_buffer = target;
			_writer = new BinaryWriter(target, Encoding.UTF8, true);
		}

		protected void EndFragmentEncode()
		{
			_writer.Flush();
			_writer.Dispose();
			_writer = null;
			_buffer = null;
		}

		public virtual byte[] Encode()
		{
			byte[] cache = _encodedMessage;
			if (cache != null) return cache;

			lock (_encodeSync)
			{
				// This construct to avoid unnecessary contention and double encoding.
				if (_encodedMessage != null) return _encodedMessage;

				// Already encoded through the lease path: materialize the owned copy from it.
				if (_encodedLease != null)
				{
					_encodedMessage = _encodedLease.ToArray();
					return _encodedMessage;
				}

				_buffer = new MemoryStream();
				using (_writer = new BinaryWriter(_buffer, Encoding.UTF8, true))
				{
					EncodePacket();

					_writer.Flush();
					var buffer = (MemoryStream) _buffer;
					_encodedMessage = buffer.ToArray();
				}
				_buffer.Dispose();

				_writer = null;
				_buffer = null;

				return _encodedMessage;
			}
		}

		protected virtual void EncodePacket()
		{
			_buffer.Position = 0;
			if (IsMcpe) WriteVarInt(Id);
			else Write((byte) Id);
		}

		[Obsolete("Use decode with ReadOnlyMemory<byte> instead.")]
		public virtual Packet Decode(byte[] buffer)
		{
			return Decode(new ReadOnlyMemory<byte>(buffer));
		}

		public virtual Packet Decode(ReadOnlyMemory<byte> buffer)
		{
			// The buffer is borrowed for the duration of this call and nothing may outlive it. A
			// packet that has to keep bytes copies them into memory it owns while parsing, and hands
			// that back when it returns to the pool. Tracing the raw frame belongs where the frame
			// still exists, before the parse, not on a field that keeps it alive afterwards.
			_reader = new MemoryStreamReader(buffer);

			DecodePacket();

			if (Log.IsDebugEnabled && _reader.Position != (buffer.Length))
			{
				Log.Warn($"{GetType().Name}: Still have {buffer.Length - _reader.Position} bytes to read!!\n{HexDump(buffer.ToArray())}");
			}

			_reader.Dispose();
			_reader = null;

			return this;
		}

		protected virtual void DecodePacket()
		{
			Id = IsMcpe ? ReadVarInt() : ReadByte();
		}

		public abstract void PutPool();

		public static string HexDump(ReadOnlyMemory<byte> bytes, int bytesPerLine = 16, bool printLineCount = false)
		{
			return HexDump(bytes.Span, bytesPerLine, printLineCount);
		}

		private static string HexDump(ReadOnlySpan<byte> bytes, in int bytesPerLine, in bool printLineCount)
		{
			var sb = new StringBuilder();
			for (int line = 0; line < bytes.Length; line += bytesPerLine)
			{
				byte[] lineBytes = bytes.Slice(line).ToArray().Take(bytesPerLine).ToArray();
				if (printLineCount) sb.AppendFormat("{0:x8} ", line);
				sb.Append(string.Join(" ", lineBytes.Select(b => b.ToString("x2"))
						.ToArray())
					.PadRight(bytesPerLine * 3));
				sb.Append(" ");
				sb.Append(new string(lineBytes.Select(b => b < 32 ? '.' : (char) b)
					.ToArray()));
				sb.AppendLine();
			}
			return sb.ToString();
		}

		public static string ToJson(Packet message)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
				Formatting = Formatting.Indented,
			};
			jsonSerializerSettings.Converters.Add(new NbtIntConverter());
			jsonSerializerSettings.Converters.Add(new NbtStringConverter());
			jsonSerializerSettings.Converters.Add(new IPAddressConverter());
			jsonSerializerSettings.Converters.Add(new IPEndPointConverter());

			return JsonConvert.SerializeObject(message, jsonSerializerSettings);
		}
	}

	/// Base package class
	public abstract partial class Packet<T> : Packet, IDisposable where T : Packet<T>, new()
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Packet<T>));

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


		public override bool UsesEncodeLease => _isPooled && !_isPermanent;

		public T MarkPermanent(bool permanent = true)
		{
			if (!_isPooled) throw new Exception("Tried to make non pooled item permanent");
			_isPermanent = permanent;

			// A permanent packet never Resets, so pooled buffers it sits on would be pinned forever.
			if (permanent) MaterializePooledState();

			return (T) this;
		}

		public T AddReferences(long numberOfReferences)
		{
			if (_isPermanent) return (T) this;

			if (!_isPooled) throw new Exception("Tried to reference count a non pooled item");
			Interlocked.Add(ref _referenceCounter, numberOfReferences);

			return (T) this;
		}

		public T AddReference(Packet<T> item)
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
			T item = Pool.GetObject();
			item._isPooled = true;
			item._referenceCounter = numberOfReferences;
			return item;
		}

		/// <summary>
		///     Names packets collected without ever being released, which is the only way to see a
		///     lease that was never handed back: Reset is what disposes them, and Reset only runs
		///     from PutPool.
		///     <para>
		///         OFF by default and it must stay that way. Declaring a finalizer is not free even
		///         when its body does nothing: the object is registered at construction, survives an
		///         extra collection and is resurrected onto the finalizer thread. So with this off
		///         the constructor suppresses finalization outright, which is the only way the
		///         declaration costs nothing. A dump with it on shows tens of thousands of dead
		///         packets still reachable, which is the finalizer queue, not a leak.
		///     </para>
		/// </summary>
		public static bool WarnOnUnreleased = Config.GetProperty("Packet.WarnOnUnreleased", false);

		protected Packet()
		{
			if (!WarnOnUnreleased) GC.SuppressFinalize(this);
		}

		private static long _unreleased;

		/// <summary>How many pooled packets have been collected without a PutPool, since start.</summary>
		public static long UnreleasedCount => Interlocked.Read(ref _unreleased);

		~Packet()
		{
			if (!WarnOnUnreleased || !_isPooled) return;

			long total = Interlocked.Increment(ref _unreleased);

			// Every one, not a sample: the point is which types leak, and a rate tells you nothing
			// about the one that only happens on a rare path.
			Log.Warn($"Unreleased 0x{Id:x2} {GetType().Name}, refs={_referenceCounter}, total={total}");
		}

		public override void PutPool()
		{
			if (_isPermanent) return;
			if (!IsPooled) return;

			long counter = Interlocked.Decrement(ref _referenceCounter);
			if (counter > 0) return;

			if (counter < 0)
			{
				Log.Error($"Pooling error. Added pooled object too many times. 0x{Id:x2} {GetType().Name}, IsPooled={IsPooled}, IsPooled={_isPermanent}, Refs={_referenceCounter}");
				return;
			}

			Reset();

			_isPooled = false;

			//Pool.PutObject((T) this);
		}

		public void Dispose()
		{
			PutPool();
		}
	}
}