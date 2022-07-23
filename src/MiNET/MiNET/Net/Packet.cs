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
using MiNET.Net.RakNet;
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

		[JsonIgnore] public ReliabilityHeader ReliabilityHeader = new ReliabilityHeader();

		[JsonIgnore] public bool ForceClear;
		[JsonIgnore] public bool NoBatch { get; set; }

		[JsonIgnore] public byte Id;
		[JsonIgnore] public bool IsMcpe;

		protected MemoryStreamReader _reader; // new construct for reading
		protected private Stream _buffer;
		private BinaryWriter _writer;

		[JsonIgnore] public ReadOnlyMemory<byte> Bytes { get; private set; }
		[JsonIgnore] public Stopwatch Timer { get; } = Stopwatch.StartNew();

		public Packet()
		{
			Timer.Start();
		}
		
		public void Write(sbyte value)
		{
			_writer.Write(value);
		}

		public sbyte ReadSByte()
		{
			return (sbyte)_reader.ReadByte();
		}
		
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

		public void Write(long value)
		{
			_writer.Write(BinaryPrimitives.ReverseEndianness(value));
		}

		public long ReadLong()
		{
			return BinaryPrimitives.ReverseEndianness(_reader.ReadInt64());
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
					Write(record.PlayerInfo.CertificateData?.ExtraData?.Xuid ?? String.Empty);
					Write(record.PlayerInfo.PlatformChatId);
					Write(record.PlayerInfo.DeviceOS);
					Write(record.Skin);
					Write(false); // is teacher
					Write(false); // is host
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

			if (records is PlayerAddRecords)
			{
				foreach (var record in records)
				{
					Write(record.Skin.IsVerified); // is verified
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
						player.ClientUuid = ReadUUID();
						player.EntityId = ReadSignedVarLong();
						player.DisplayName = ReadString();
						var xuid =  ReadString();
						var platformChatId = ReadString();
						var deviceOS = ReadInt();
						player.Skin = ReadSkin();
						ReadBool(); // is teacher
						ReadBool(); // is host

						player.PlayerInfo = new PlayerInfo()
						{
							PlatformChatId = platformChatId,
							DeviceOS = deviceOS,
							CertificateData = new CertificateData()
							{
								ExtraData = new ExtraData()
								{
									Xuid = xuid
								}
							}
						};
						records.Add(player);
						//Log.Debug($"Reading {player.ClientUuid}, {player.EntityId}, '{player.DisplayName}', {platformChatId}");
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

			if (records is PlayerAddRecords)
			{
				foreach (Player player in records)
				{
					bool isVerified = ReadBool();

					if (player.Skin != null) 
						player.Skin.IsVerified = isVerified;
				}
			}
			//if (!_reader.Eof) ReadBool(); // damn BS
			//if (!_reader.Eof) ReadBool(); // damn BS

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
			var d = 256f / 360f;
			Write((byte) Math.Round(location.Pitch * d)); // 256/360
			Write((byte) Math.Round(location.HeadYaw * d)); // 256/360
			Write((byte) Math.Round(location.Yaw * d)); // 256/360
		}

		public PlayerLocation ReadPlayerLocation()
		{
			PlayerLocation location = new PlayerLocation();
			location.X = ReadFloat();
			location.Y = ReadFloat();
			location.Z = ReadFloat();
			location.Pitch = ReadByte() * 1f / 0.71f;
			location.HeadYaw = ReadByte() * 1f / 0.71f;
			location.Yaw = ReadByte() * 1f / 0.71f;

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

		public void Write(Nbt nbt)
		{
			Write(nbt, _writer.BaseStream, nbt.NbtFile.UseVarInt || this is McpeBlockEntityData || this is McpeUpdateEquipment);
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

		public void Write(Transaction transaction)
		{
			WriteSignedVarInt(transaction.RequestId);

			if (transaction.RequestId != 0)
			{
				WriteUnsignedVarInt((uint) transaction.RequestRecords.Count);

				foreach (var record in transaction.RequestRecords)
				{
					Write(record.ContainerId);
					WriteUnsignedVarInt((uint) record.Slots.Count);

					foreach (var slot in record.Slots)
					{
						Write(slot);
					}
				}
			}
			
			switch (transaction)
			{
				case InventoryMismatchTransaction _:
					WriteUnsignedVarInt((int) McpeInventoryTransaction.TransactionType.InventoryMismatch);
					break;
				case ItemReleaseTransaction _:
					WriteUnsignedVarInt((int) McpeInventoryTransaction.TransactionType.ItemRelease);
					break;
				case ItemUseOnEntityTransaction _:
					WriteUnsignedVarInt((int) McpeInventoryTransaction.TransactionType.ItemUseOnEntity);
					break;
				case ItemUseTransaction _:
					WriteUnsignedVarInt((int) McpeInventoryTransaction.TransactionType.ItemUse);
					break;
				case NormalTransaction _:
					WriteUnsignedVarInt((int) McpeInventoryTransaction.TransactionType.Normal);
					break;
			}
			//Write(transaction.HasNetworkIds);
			
			WriteUnsignedVarInt((uint) transaction.TransactionRecords.Count);
			foreach (var record in transaction.TransactionRecords)
			{
				switch (record)
				{
					case ContainerTransactionRecord r:
						WriteVarInt((int) McpeInventoryTransaction.InventorySourceType.Container);
						WriteSignedVarInt(r.InventoryId);
						break;
					case GlobalTransactionRecord _:
						WriteVarInt((int) McpeInventoryTransaction.InventorySourceType.Global);
						break;
					case WorldInteractionTransactionRecord r:
						WriteVarInt((int) McpeInventoryTransaction.InventorySourceType.WorldInteraction);
						WriteVarInt(r.Flags);
						break;
					case CreativeTransactionRecord _:
						WriteVarInt((int) McpeInventoryTransaction.InventorySourceType.Creative);
						break;
					case CraftTransactionRecord r:
						WriteVarInt((int) McpeInventoryTransaction.InventorySourceType.Crafting);
						WriteVarInt((int) r.Action);
						break;
				}

				WriteVarInt(record.Slot);
				Write(record.OldItem);
				Write(record.NewItem);
				
				//if (transaction.HasNetworkIds)
				//	WriteSignedVarInt(record.StackNetworkId);
			}

			switch (transaction)
			{
				case NormalTransaction _:
				case InventoryMismatchTransaction _:
					break;
				case ItemUseTransaction t:
					WriteUnsignedVarInt((uint) t.ActionType);
					Write(t.Position);
					WriteSignedVarInt(t.Face);
					WriteSignedVarInt(t.Slot);
					Write(t.Item);
					Write(t.FromPosition);
					Write(t.ClickPosition);
					WriteUnsignedVarInt(t.BlockRuntimeId);
					break;
				case ItemUseOnEntityTransaction t:
					WriteUnsignedVarLong(t.EntityId);
					WriteUnsignedVarInt((uint) t.ActionType);
					WriteSignedVarInt(t.Slot);
					Write(t.Item);
					Write(t.FromPosition);
					Write(t.ClickPosition);
					break;
				case ItemReleaseTransaction t:
					WriteUnsignedVarInt((uint) t.ActionType);
					WriteSignedVarInt(t.Slot);
					Write(t.Item);
					Write(t.FromPosition);
					break;
				default:
					break;
			}
		}

		public Transaction ReadTransaction()
		{
			var requestId = ReadSignedVarInt(); // request id
			var requestRecords = new List<RequestRecord>();
			if (requestId != 0)
			{
				var c1 = ReadUnsignedVarInt();
				for (int i = 0; i < c1; i++)
				{
					var rr = new RequestRecord();
					rr.ContainerId = ReadByte();
					var c2 = ReadUnsignedVarInt();
					for (int j = 0; j < c2; j++)
					{
						byte slot = ReadByte();
						rr.Slots.Add(slot);
						Log.Debug($"RequestId:{requestId}, containerId:{rr.ContainerId}, slot:{slot}");
					}
					requestRecords.Add(rr);
				}
			}

			var transactionType = (McpeInventoryTransaction.TransactionType) ReadVarInt();
			//bool hasItemStacks = ReadBool();
			//if(hasItemStacks) Log.Warn($"Got item stacks in old transaction");

			var transactions = new List<TransactionRecord>();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				TransactionRecord record;
				int sourceType = ReadVarInt();
				switch ((McpeInventoryTransaction.InventorySourceType) sourceType)
				{
					case McpeInventoryTransaction.InventorySourceType.Container:
						record = new ContainerTransactionRecord() {InventoryId = ReadSignedVarInt()};
						break;
					case McpeInventoryTransaction.InventorySourceType.Global:
						record = new GlobalTransactionRecord();
						break;
					case McpeInventoryTransaction.InventorySourceType.WorldInteraction:
						record = new WorldInteractionTransactionRecord() {Flags = ReadVarInt()};
						break;
					case McpeInventoryTransaction.InventorySourceType.Creative:
						record = new CreativeTransactionRecord() {InventoryId = 0x79};
						break;
					case McpeInventoryTransaction.InventorySourceType.Unspecified:
					case McpeInventoryTransaction.InventorySourceType.Crafting:
						record = new CraftTransactionRecord() {Action = (McpeInventoryTransaction.CraftingAction) ReadSignedVarInt()};
						break;
					default:
						Log.Error($"Unknown inventory source type={sourceType}");
						continue;
				}

				record.Slot = ReadVarInt();
				record.OldItem = ReadItem();
				record.NewItem = ReadItem();
			//	if (hasItemStacks) 
				//	record.StackNetworkId = ReadSignedVarInt();
				
				transactions.Add(record);
			}

			Transaction transaction = null;
			switch (transactionType)
			{
				case McpeInventoryTransaction.TransactionType.Normal:
					transaction = new NormalTransaction();
					break;
				case McpeInventoryTransaction.TransactionType.InventoryMismatch:
					transaction = new InventoryMismatchTransaction();
					break;
				case McpeInventoryTransaction.TransactionType.ItemUse:
					transaction = new ItemUseTransaction()
					{
						ActionType = (McpeInventoryTransaction.ItemUseAction) ReadVarInt(),
						Position = ReadBlockCoordinates(),
						Face = ReadSignedVarInt(),
						Slot = ReadSignedVarInt(),
						Item = ReadItem(),
						FromPosition = ReadVector3(),
						ClickPosition = ReadVector3(),
						BlockRuntimeId = ReadUnsignedVarInt()
					};
					break;
				case McpeInventoryTransaction.TransactionType.ItemUseOnEntity:
					transaction = new ItemUseOnEntityTransaction()
					{
						EntityId = ReadVarLong(),
						ActionType = (McpeInventoryTransaction.ItemUseOnEntityAction) ReadVarInt(),
						Slot = ReadSignedVarInt(),
						Item = ReadItem(),
						FromPosition = ReadVector3(),
						ClickPosition = ReadVector3()
					};
					break;
				case McpeInventoryTransaction.TransactionType.ItemRelease:
					transaction = new ItemReleaseTransaction()
					{
						ActionType = (McpeInventoryTransaction.ItemReleaseAction) ReadVarInt(),
						Slot = ReadSignedVarInt(),
						Item = ReadItem(),
						FromPosition = ReadVector3()
					};
					break;
			}

			transaction.TransactionRecords = transactions;
			transaction.RequestId = requestId;
			transaction.RequestRecords = requestRecords;

			return transaction;
		}

		public StackRequestSlotInfo ReadStackRequestSlotInfo()
		{
			var containerId    = (byte) ReadByte();
			var slot           = (byte) ReadByte();
			var stackNetworkId = ReadSignedVarInt();

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
			Write(slotInfo.Slot);
			WriteSignedVarInt(slotInfo.StackNetworkId);
		}

		public void Write(ItemStackRequests requests)
		{
			WriteUnsignedVarInt((uint) requests.Count);

			foreach (ItemStackActionList request in requests)
			{
				WriteSignedVarInt(request.RequestId);
				WriteUnsignedVarInt((uint) request.Count);

				foreach (ItemStackAction action in request)
				{
					switch (action)
					{
						case TakeAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.Take);
							Write(ta.Count);
							Write(ta.Source);
							Write(ta.Destination);
							break;
						}
						
						case PlaceAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.Place);
							Write(ta.Count);
							Write(ta.Source);
							Write(ta.Destination);
							break;
						}
						
						case SwapAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.Swap);
							Write(ta.Source);
							Write(ta.Destination);
							break;
						}
						
						case DropAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.Drop);
							Write(ta.Count);
							Write(ta.Source);
							Write(ta.Randomly);
							break;
						}
						
						case DestroyAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.Destroy);
							Write(ta.Count);
							Write(ta.Source);
							break;
						}
						
						case ConsumeAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.Consume);
							Write(ta.Count);
							Write(ta.Source);
							break;
						}
						
						case CreateAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.Create);
							Write(ta.ResultSlot);
							break;
						}

						case PlaceIntoBundleAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.PlaceIntoBundle);
							break;
						}
						
						case TakeFromBundleAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.TakeFromBundle);
							break;
						}
						
						case LabTableCombineAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.LabTableCombine);
							break;
						}
						
						case BeaconPaymentAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.BeaconPayment);
							WriteSignedVarInt(ta.PrimaryEffect);
							WriteSignedVarInt(ta.SecondaryEffect);
							break;
						}
						
						case CraftAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.CraftRecipe);
							WriteUnsignedVarInt(ta.RecipeNetworkId);
							break;
						}

						case CraftAutoAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.CraftRecipeAuto);
							WriteUnsignedVarInt(ta.RecipeNetworkId);
							break;
						}
						
						case CraftCreativeAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.CraftCreative);
							WriteUnsignedVarInt(ta.CreativeItemNetworkId);
							break;
						}

						case CraftRecipeOptionalAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.CraftRecipeOptional);
							WriteUnsignedVarInt(ta.RecipeNetworkId);
							Write(ta.FilteredStringIndex);
							break;
						}

						case GrindstoneStackRequestAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.CraftGrindstone);
							WriteUnsignedVarInt(ta.RecipeNetworkId);
							WriteVarInt(ta.RepairCost);
							break;
						}
						
						case LoomStackRequestAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.CraftLoom);
							Write(ta.PatternId);
							break;
						}

						case CraftNotImplementedDeprecatedAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.CraftNotImplementedDeprecated);
							break;
						}
						
						case CraftResultDeprecatedAction ta:
						{
							Write((byte) McpeItemStackRequest.ActionType.CraftResultsDeprecated);
							Write(ta.ResultItems);
							Write(ta.TimesCrafted);
							break;
						}
					}
				}
				
				WriteUnsignedVarInt(0); //FilterStrings
			}
		}

		//public const TAKE = 0;
		//public const PLACE = 1;
		//public const SWAP = 2;
		//public const DROP = 3;
		//public const DESTROY = 4;
		//public const CRAFTING_CONSUME_INPUT = 5;
		//public const CRAFTING_MARK_SECONDARY_RESULT_SLOT = 6;
		//public const LAB_TABLE_COMBINE = 7;
		//public const BEACON_PAYMENT = 8;
		//public const CRAFTING_RECIPE = 9;
		//public const CRAFTING_RECIPE_AUTO = 10; //recipe book?
		//public const CREATIVE_CREATE = 11;
		//public const CRAFT_RECIPE_OPTIONAL = 12;
		//public const CRAFTING_NON_IMPLEMENTED_DEPRECATED_ASK_TY_LAING = 13; 
		//public const CRAFTING_RESULTS_DEPRECATED_ASK_TY_LAING = 14; //no idea what this is for

		public ItemStackRequests ReadItemStackRequests()
		{
			var requests = new ItemStackRequests();

			var c = ReadUnsignedVarInt();
			Log.Debug($"Count: {c}");
			for (int i = 0; i < c; i++)
			{
				var actions = new ItemStackActionList();
				actions.RequestId = ReadSignedVarInt();
				Log.Debug($"Request ID: {actions.RequestId}");

				uint count = ReadUnsignedVarInt();
				Log.Debug($"Count: {count}");
				for (int j = 0; j < count; j++)
				{
					var actionType = (McpeItemStackRequest.ActionType) ReadByte();
					Log.Debug($"Action type: {actionType}");
					switch (actionType)
					{
						case McpeItemStackRequest.ActionType.Take:
						{
							var action = new TakeAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							action.Destination = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.Place:
						{
							var action = new PlaceAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							action.Destination = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.Swap:
						{
							var action = new SwapAction();
							action.Source = ReadStackRequestSlotInfo();
							action.Destination = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.Drop:
						{
							var action = new DropAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							action.Randomly = ReadBool();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.Destroy:
						{
							var action = new DestroyAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.Consume:
						{
							var action = new ConsumeAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.Create:
						{
							var action = new CreateAction();
							action.ResultSlot = ReadByte();
							actions.Add(action);
							break;
						}

						case McpeItemStackRequest.ActionType.PlaceIntoBundle:
						{
							var action = new PlaceIntoBundleAction();
							actions.Add(action);
							break;
						}

						case McpeItemStackRequest.ActionType.TakeFromBundle:
						{
							var action = new TakeFromBundleAction();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.LabTableCombine:
						{
							var action = new LabTableCombineAction();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.BeaconPayment:
						{
							var action = new BeaconPaymentAction();
							action.PrimaryEffect = ReadSignedVarInt();
							action.SecondaryEffect = ReadSignedVarInt();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.CraftRecipe:
						{
							var action = new CraftAction();
							action.RecipeNetworkId = ReadUnsignedVarInt();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.CraftRecipeAuto:
						{
							var action = new CraftAutoAction();
							action.RecipeNetworkId = ReadUnsignedVarInt();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.CraftCreative:
						{
							var action = new CraftCreativeAction();
							action.CreativeItemNetworkId = ReadUnsignedVarInt();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.CraftRecipeOptional:
						{
							var action = new CraftRecipeOptionalAction();
							action.RecipeNetworkId = ReadUnsignedVarInt();
							action.FilteredStringIndex = ReadInt();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.CraftGrindstone:
						{
							var action = new GrindstoneStackRequestAction();
							action.RecipeNetworkId = ReadUnsignedVarInt();
							action.RepairCost = ReadVarInt();
							
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.CraftLoom:
						{
							var action = new LoomStackRequestAction();
							action.PatternId = ReadString();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.CraftNotImplementedDeprecated:
						{
							var action = new CraftNotImplementedDeprecatedAction();
							actions.Add(action);
							break;
						}
						case McpeItemStackRequest.ActionType.CraftResultsDeprecated:
						{
							var action = new CraftResultDeprecatedAction();
							action.ResultItems = ReadItems();
							action.TimesCrafted = ReadByte();
							actions.Add(action);
							break;
						}
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
				
				requests.Add(actions);

				var filterStringCount = ReadUnsignedVarInt();

				for (int fi = 0; fi < filterStringCount; fi++)
				{
					ReadString();
				}
			}

			return requests;
		}

		public void Write(ItemStackResponses responses)
		{
			WriteUnsignedVarInt((uint) responses.Count);
			foreach (ItemStackResponse stackResponse in responses)
			{
				Write((byte) stackResponse.Result);
				WriteSignedVarInt(stackResponse.RequestId);
				if (stackResponse.Result != StackResponseStatus.Ok) 
					continue;
				WriteUnsignedVarInt((uint) stackResponse.ResponseContainerInfos.Count);
				foreach (StackResponseContainerInfo containerInfo in stackResponse.ResponseContainerInfos)
				{
					Write(containerInfo.ContainerId);
					WriteUnsignedVarInt((uint) containerInfo.Slots.Count);
					foreach (StackResponseSlotInfo slot in containerInfo.Slots)
					{
						Write(slot.Slot);
						Write(slot.HotbarSlot);
						Write(slot.Count);
						WriteSignedVarInt(slot.StackNetworkId);
						Write(slot.CustomName);
						WriteSignedVarInt(slot.DurabilityCorrection);
					}
				}
			}
		}


		public ItemStackResponses ReadItemStackResponses()
		{
			var responses = new ItemStackResponses();
			var count     = ReadUnsignedVarInt();

			for (var i = 0; i < count; i++)
			{
				var response = new ItemStackResponse();
				response.Result = (StackResponseStatus) ReadByte();
				response.RequestId = ReadSignedVarInt();
				
				if (response.Result != StackResponseStatus.Ok)
					continue;
				
				response.ResponseContainerInfos = new List<StackResponseContainerInfo>();
				var subCount = ReadUnsignedVarInt();
				for (int sub = 0; sub < subCount; sub++)
				{
					var containerInfo = new StackResponseContainerInfo();
					containerInfo.ContainerId = ReadByte();

					var slotCount = ReadUnsignedVarInt();
					containerInfo.Slots = new List<StackResponseSlotInfo>();
					
					for (int si = 0; si < slotCount; si++)
					{
						var slot = new StackResponseSlotInfo();
						slot.Slot = ReadByte();
						slot.HotbarSlot = ReadByte();
						slot.Count = ReadByte();
						slot.StackNetworkId = ReadSignedVarInt();
						slot.CustomName = ReadString();
						slot.DurabilityCorrection = ReadSignedVarInt();
						
						containerInfo.Slots.Add(slot);
					}
					
					response.ResponseContainerInfos.Add(containerInfo);
				}
				
				responses.Add(response);
			}
			
			return responses;
		}

		public void Write(ItemComponentList list)
		{
			WriteUnsignedVarInt((uint) list.Count);

			foreach (var item in list)
			{
				Write(item.Name);
				Write(item.Nbt);
			}
		}
		
		public ItemComponentList ReadItemComponentList()
		{
			var               count = ReadUnsignedVarInt();
			ItemComponentList l     = new ItemComponentList();

			for (int i = 0; i < count; i++)
			{
				string        name      = ReadString();
				var           nbt       = ReadNbt();
				
				ItemComponent component = new ItemComponent();
				component.Name = name;
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
				WriteUnsignedVarInt(option.Cost);
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
				Write(enchant.Id);	
				Write(enchant.Level);	
			}
		}

		private List<Enchant> ReadEnchants()
		{
			List<Enchant> enchants = new List<Enchant>();
			var           count    = ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				Enchant enchant = new Enchant(ReadByte(), ReadByte());
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
				option.Cost = ReadUnsignedVarInt();
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

		public void Write(AnimationKey[] keys)
		{
			WriteUnsignedVarInt((uint) keys.Length);
			foreach (AnimationKey key in keys)
			{
				Write(key.ExecuteImmediate);
				Write(key.ResetBefore);
				Write(key.ResetAfter);
				Write(key.StartRotation);
				Write(key.EndRotation);
				WriteUnsignedVarInt(key.Duration);
			}
		}

		public AnimationKey[] ReadAnimationKeys()
		{
			var count = ReadUnsignedVarInt();
			var keys = new AnimationKey[count];
			for (int i = 0; i < count; i++)
			{
				AnimationKey key = new AnimationKey();
				key.ExecuteImmediate = ReadBool();
				key.ResetBefore = ReadBool();
				key.ResetAfter = ReadBool();
				key.StartRotation = ReadVector3();
				key.EndRotation = ReadVector3();
				key.Duration = ReadUnsignedVarInt();
				keys[i] = key;
			}

			return keys;
		}


		private ItemStacks ReadItems()
		{
			var items = new ItemStacks();

			var count = ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				items.Add(ReadItem(false));
			}

			return items;
		}

		private const int ShieldId = 355;
		public void Write(Item stack, bool writeUniqueId = true)
		{
			if (stack == null || stack.Id == 0 || !ItemFactory.Translator.TryGetNetworkId(stack.Id, stack.Metadata, out var netData))
			{
				WriteSignedVarInt(0);
				return;
			}

			//var netId = ItemFactory.Translator.ToNetworkId(stack.Id, stack.Metadata);
			WriteSignedVarInt(netData.Id);
			
			//WriteSignedVarInt(id);

			Write((short) stack.Count);
			WriteUnsignedVarInt((uint)netData.Meta);

			if (writeUniqueId)
			{
				Write(stack.UniqueId != 0);

				if (stack.UniqueId != 0)
				{
					WriteVarInt(stack.UniqueId);
				}
			}
			
			WriteSignedVarInt(stack.RuntimeId);

			byte[] extraData = null;
			//Write extra data
			using (var ms = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(ms, Encoding.UTF8, true))
				{
					if (stack.ExtraData != null)
					{
						binaryWriter.Write((ushort) 0xffff);
						binaryWriter.Write((byte)1);
						var nbtData = GetNbtData(stack.ExtraData, false);
						binaryWriter.Write(nbtData);
					}
					else
					{
						binaryWriter.Write((short) 0);
					}

					binaryWriter.Write(0); //Write Int
					binaryWriter.Write(0); //Write Int

					if (stack.Id == 513)
					{
						binaryWriter.Write((long) 0);
					}
				}

				extraData = ms.ToArray();
			}
			
			WriteLength(extraData.Length);
			Write(extraData);
		}

		public Item ReadItem(bool readUniqueId = true)
		{
			int id = ReadSignedVarInt();
			if (id == 0)
			{
				return new ItemAir();
			}

			short count = (short) ReadShort();
			var metadata = ReadUnsignedVarInt();

			var translated = ItemFactory.Translator.FromNetworkId(id, (short)metadata);

			Item stack = ItemFactory.GetItem((short)translated.Id, translated.Meta, count);

			if (readUniqueId)
			{
				if (ReadBool()) stack.UniqueId = ReadVarInt();
			}

			stack.RuntimeId = ReadSignedVarInt();

			int length = ReadLength();
			var data = ReadBytes(length);

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

						var beforeRead = ms.Position;
						stack.ExtraData = ReadNbtCompound(ms, false);
						var afterRead = ms.Position;
						var nbtCompoundLength = afterRead - beforeRead;
					}
					else if (nbtLen > 0)
					{
						throw new Exception($"Fringe nbt length when reading item extra NBT: {nbtLen}");
					}
					
					int canPlace = binaryReader.ReadInt32();
					for (int i = 0; i < canPlace; i++)
					{
						var l = binaryReader.ReadInt16();
						binaryReader.ReadBytes(l);
					}
					int canBreak = binaryReader.ReadInt32();
					for (int i = 0; i < canBreak; i++)
					{
						var l = binaryReader.ReadInt16();
						binaryReader.ReadBytes(l);
					}

					if (stack.RuntimeId == ShieldId) // shield
					{
						binaryReader.ReadInt64(); // something about tick, crap code
					}
				}
			}
			return stack;
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
						GameRule<int> rule = new GameRule<int>(name, ReadVarInt())
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
				Write(rule.Name.ToLower());
				Write(rule.IsPlayerModifiable); // bool isPlayerModifiable

				if (rule is GameRule<bool>)
				{
					WriteUnsignedVarInt(1);
					Write(((GameRule<bool>) rule).Value);
				}
				else if (rule is GameRule<int>)
				{
					WriteUnsignedVarInt(2);
					WriteVarInt(((GameRule<int>) rule).Value);
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

		public Itemstates ReadItemstates()
		{
			var result = new Itemstates();
			uint count = ReadUnsignedVarInt();
			for (int runtimeId = 0; runtimeId < count; runtimeId++)
			{
				var name = ReadString();
				var legacyId = ReadShort();
				var component = ReadBool();

				if (name == "minecraft:shield")
				{
					Log.Warn($"Got shield with runtime id {runtimeId}, legacy {legacyId}");
				}

				result.Add(new Itemstate
				{
					Id = legacyId,
					Name = name,
					ComponentBased = component
				});
			}

			return result;
		}

		public void Write(Itemstates itemstates)
		{
			if (itemstates == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}
			WriteUnsignedVarInt((uint) itemstates.Count);
			foreach (var itemstate in itemstates)
			{
				Write(itemstate.Name);
				Write(itemstate.Id);
				Write(itemstate.ComponentBased);
			}
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

		public void Write(AbilityLayer layer)
		{
			Write((ushort)layer.Type);
			Write((uint)layer.Abilities);
			Write((uint)layer.Values);
			Write(layer.FlySpeed);
			Write(layer.WalkSpeed);
		}

		public AbilityLayer ReadAbilityLayer()
		{
			AbilityLayer layer = new AbilityLayer();
			layer.Type = (AbilityLayerType) ReadUshort();
			layer.Abilities = (PlayerAbility)ReadUint();
			layer.Values = ReadUint();
			layer.FlySpeed = ReadFloat();
			layer.WalkSpeed = ReadFloat();

			return layer;
		}

		public void Write(AbilityLayers layers)
		{
			Write((byte)layers.Count);

			foreach (var layer in layers)
			{
				Write(layer);
			}
		}
		
		public AbilityLayers ReadAbilityLayers()
		{
			AbilityLayers layers = new AbilityLayers();
			var count = ReadByte();

			for (int i = 0; i < count; i++)
			{
				layers.Add(ReadAbilityLayer());
			}
			return layers;
		}

		public void Write(EntityLink link)
		{
			WriteVarLong(link.FromEntityId);
			WriteVarLong(link.ToEntityId);
			Write((byte)link.Type);
			Write(link.Immediate);
			Write(link.CausedByRider);
		}

		public EntityLink ReadEntityLink()
		{
			var from = ReadVarLong();
			var to = ReadVarLong();
			var type = (EntityLink.EntityLinkType) ReadByte();
			var immediate = ReadBool();
			var causedByRider = ReadBool();

			return new EntityLink(from, to, type, immediate, causedByRider);
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
				Write(info.UUID);
				Write(info.Version);
				Write(info.Size);
				Write(info.ContentKey);
				Write(info.SubPackName);
				Write(info.ContentIdentity);
				Write(info.HasScripts);
				Write(info.RtxEnabled);
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
				var id              = ReadString();
				var version         = ReadString();
				var size            = ReadUlong();
				var encryptionKey   = ReadString();
				var subpackName     = ReadString();
				var contentIdentity = ReadString();
				var hasScripts      = ReadBool();
				var rtxEnabled      = ReadBool();
				
				info.UUID = id;
				info.Version = version;
				info.Size = size;
				info.HasScripts = hasScripts;
				info.ContentKey = encryptionKey;
				info.SubPackName = subpackName;
				info.ContentIdentity = contentIdentity;
				info.RtxEnabled = rtxEnabled;
				
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
				var info = new PackIdVersion
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

			if (skin.Animations?.Count > 0)
			{
				Write(skin.Animations.Count);
				foreach (Animation animation in skin.Animations)
				{
					Write(animation.ImageWidth);
					Write(animation.ImageHeight);
					WriteByteArray(animation.Image);
					Write(animation.Type);
					Write(animation.FrameCount);
					Write(animation.Expression);
				}
			}
			else
			{
				Write(0);
			}

			Write(skin.Cape.ImageWidth);
			Write(skin.Cape.ImageHeight);
			WriteByteArray(skin.Cape.Data);
			Write(skin.GeometryData);
			Write(skin.GeometryDataVersion);
			Write(skin.AnimationData);

			Write(skin.Cape.Id);
			Write(skin.SkinId + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()); // some unique skin id
			Write(skin.ArmSize);
			Write(skin.SkinColor);
			Write(skin.PersonaPieces.Count);
			foreach (PersonaPiece piece in skin.PersonaPieces)
			{
				Write(piece.PieceId);
				Write(piece.PieceType);
				Write(piece.PackId);
				Write(piece.IsDefaultPiece);
				Write(piece.ProductId);
			}
			Write(skin.SkinPieces.Count);
			foreach (SkinPiece skinPiece in skin.SkinPieces)
			{
				Write(skinPiece.PieceType);
				Write(skinPiece.Colors.Count);
				foreach (string color in skinPiece.Colors)
				{
					Write(color);
				}
			}
			
			Write(skin.IsPremiumSkin);
			Write(skin.IsPersonaSkin);
			Write(skin.Cape.OnClassicSkin);
			Write(skin.IsPrimaryUser);
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

			int animationCount = ReadInt();
			for (int i = 0; i < animationCount; i++)
			{
				skin.Animations.Add(
					new Animation()
					{
						ImageWidth = ReadInt(),
						ImageHeight = ReadInt(),
						Image = ReadByteArray(false),
						Type = ReadInt(),
						FrameCount = ReadFloat(),
						Expression = ReadInt() 
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
			ReadString(); // fullSkinId
			skin.ArmSize = ReadString();
			skin.SkinColor = ReadString();
			int personaPieceCount = ReadInt();
			for (int i = 0; i < personaPieceCount; i++)
			{
				var p = new PersonaPiece();
				p.PieceId = ReadString();
				p.PieceType = ReadString();
				p.PackId = ReadString();
				p.IsDefaultPiece = ReadBool();
				p.ProductId = ReadString();
				skin.PersonaPieces.Add(p);
			}

			int skinPieceCount = ReadInt();
			for (int i = 0; i < skinPieceCount; i++)
			{
				var piece = new SkinPiece();
				piece.PieceType = ReadString();
				int colorAmount = ReadInt();
				for (int i2 = 0; i2 < colorAmount; i2++)
				{
					piece.Colors.Add(ReadString());
				}
				skin.SkinPieces.Add(piece);
			}
			
			skin.IsPremiumSkin = ReadBool();
			skin.IsPersonaSkin = ReadBool();
			skin.Cape.OnClassicSkin = ReadBool();
			skin.IsPrimaryUser = ReadBool();
			//Log.Debug($"SkinId={skin.SkinId}");
			//Log.Debug($"SkinData lenght={skin.Data.Length}");
			//Log.Debug($"CapeData lenght={skin.Cape.Data.Length}");
			//Log.Debug("\n" + HexDump(skin.Cape.Data));
			//Log.Debug($"SkinGeometryName={skin.GeometryName}");
			//Log.Debug($"SkinGeometry lenght={skin.GeometryData.Length}");

			return skin;
		}

		const byte Shapeless = 0;
		const byte Shaped = 1;
		const byte Furnace = 2;
		const byte FurnaceData = 3;
		const byte Multi = 4;
		const byte ShulkerBox = 5;
		const byte ShapelessChemistry = 6;
		const byte ShapedChemistry = 7;

		public void Write(Recipes recipes)
		{
			WriteUnsignedVarInt((uint) recipes.Count);

			foreach (Recipe recipe in recipes)
			{
				switch (recipe)
				{
					case ShapelessRecipe shapelessRecipe:
					{
						WriteSignedVarInt(Shapeless); // Type

						var rec = shapelessRecipe;
						Write(rec.Id.ToString());
						WriteVarInt(rec.Input.Count);
						foreach (Item stack in rec.Input)
						{
							WriteRecipeIngredient(stack);
						}
						WriteVarInt(rec.Result.Count);
						foreach (Item item in rec.Result)
						{
							Write(item);
						}
						Write(rec.Id);
						Write(rec.Block);
						WriteSignedVarInt(0); // priority
						WriteVarInt(shapelessRecipe.UniqueId); // unique id
						break;
					}
					case ShapedRecipe shapedRecipe:
					{
						WriteSignedVarInt(Shaped); // Type

						var rec = shapedRecipe;
						Write(rec.Id.ToString());
						WriteSignedVarInt(rec.Width);
						WriteSignedVarInt(rec.Height);

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
							Write(item);
						}
						Write(rec.Id);
						Write(rec.Block);
						WriteSignedVarInt(0); // priority
						WriteVarInt(shapedRecipe.UniqueId); // unique id
						break;
					}
					case SmeltingRecipe smeltingRecipe:
					{
						var rec = smeltingRecipe;
						WriteSignedVarInt(rec.Input.Metadata == 0 ? Furnace : FurnaceData); // Type
						WriteSignedVarInt(rec.Input.Id);
						if (rec.Input.Metadata != 0)
						{
							WriteSignedVarInt(rec.Input.Metadata);
						}
						Write(rec.Result);
						Write(rec.Block);
						break;
					}
					case MultiRecipe multiRecipe:
					{
						WriteSignedVarInt(Multi); // Type
						Write(recipe.Id);
						WriteVarInt(multiRecipe.UniqueId); // unique id
						break;
					}
				}
			}
		}

		public Recipes ReadRecipes()
		{
			var recipes = new Recipes();

			int count = (int) ReadUnsignedVarInt();
			Log.Trace($"Reading {count} recipes");

			for (int i = 0; i < count; i++)
			{
				int recipeType = ReadSignedVarInt();

				Log.Trace($"Read recipe no={i} type={recipeType}");

				if (recipeType < 0 /*|| len == 0*/)
				{
					Log.Error("Read void recipe");
					break;
				}

				switch (recipeType)
				{
					case Shapeless:
					case ShulkerBox:
					{
						var recipe = new ShapelessRecipe();
						ReadString(); // some unique id
						var ingrediensCount = ReadUnsignedVarInt(); // 
						for (int j = 0; j < ingrediensCount; j++)
						{
							recipe.Input.Add(ReadRecipeIngredient());
						}
						var resultCount = ReadUnsignedVarInt(); // 1?
						for (int j = 0; j < resultCount; j++)
						{
							recipe.Result.Add(ReadItem(false));
						}
						recipe.Id = ReadUUID(); // Id
						recipe.Block = ReadString(); // block?
						ReadVarInt(); // priority
						recipe.UniqueId = ReadVarInt(); // unique id
						recipes.Add(recipe);
						//Log.Error("Read shapeless recipe");
						break;
					}
					case Shaped:
					{
						ReadString(); // some unique id
						int width = ReadSignedVarInt(); // Width
						int height = ReadSignedVarInt(); // Height
						var recipe = new ShapedRecipe(width, height);
						if (width > 3 || height > 3) 
							throw new Exception("Wrong number of ingredients, Width=" + width + ", height=" + height);
						for (int w = 0; w < width; w++)
						{
							for (int h = 0; h < height; h++)
							{
								recipe.Input[(h * width) + w] = ReadRecipeIngredient();
							}
						}

						var resultCount = ReadUnsignedVarInt(); // 1?
						for (int j = 0; j < resultCount; j++)
						{
							recipe.Result.Add(ReadItem(false));
						}
						recipe.Id = ReadUUID(); // Id
						recipe.Block = ReadString(); // block?
						ReadVarInt(); // priority
						recipe.UniqueId = ReadVarInt(); // unique id
						recipes.Add(recipe);
						//Log.Error("Read shaped recipe");
						break;
					}
					case Furnace:
					{
						var recipe = new SmeltingRecipe();
						short id = (short) ReadVarInt(); // input (with metadata) 
						Item result = ReadItem(false); // Result
						recipe.Block = ReadString(); // block?
						recipe.Input = ItemFactory.GetItem(id, 0);
						recipe.Result = result;
						recipes.Add(recipe);
						//Log.Error("Read furnace recipe");
						//Log.Error($"Input={id}, meta={""} Item={result.Id}, Meta={result.Metadata}");
						break;
					}
					case FurnaceData:
					{
						//const ENTRY_FURNACE_DATA = 3;
						var recipe = new SmeltingRecipe();
						short id = (short) ReadVarInt(); // input (with metadata) 
						short meta = (short) ReadVarInt(); // input (with metadata) 
						Item result = ReadItem(false); // Result
						recipe.Block = ReadString(); // block?
						recipe.Input = ItemFactory.GetItem(id, meta);
						recipe.Result = result;
						recipes.Add(recipe);
						//Log.Error("Read smelting recipe");
						//Log.Error($"Input={id}, meta={meta} Item={result.Id}, Meta={result.Metadata}");
						break;
					}
					case Multi:
					{
						//Log.Error("Reading MULTI");

						var recipe = new MultiRecipe();
						recipe.Id = ReadUUID();
						recipe.UniqueId = ReadVarInt(); // unique id
						recipes.Add(recipe);
						break;
					}
					case ShapelessChemistry:
					{
						var recipe = new ShapelessRecipe();
						ReadString(); // some unique id
						int ingrediensCount = ReadVarInt(); // 
						for (int j = 0; j < ingrediensCount; j++)
						{
							recipe.Input.Add(ReadRecipeIngredient());
						}
						int resultCount = ReadVarInt(); // 1?
						for (int j = 0; j < resultCount; j++)
						{
							recipe.Result.Add(ReadItem(false));
						}
						recipe.Id = ReadUUID(); // Id
						recipe.Block = ReadString(); // block?
						ReadSignedVarInt(); // priority
						recipe.UniqueId = ReadVarInt(); // unique id
						//recipes.Add(recipe);
						//Log.Error("Read shapeless recipe");
						break;
					}
					case ShapedChemistry:
					{
						ReadString(); // some unique id
						int width = ReadSignedVarInt(); // Width
						int height = ReadSignedVarInt(); // Height
						var recipe = new ShapedRecipe(width, height);
						if (width > 3 || height > 3) throw new Exception("Wrong number of ingredients. Width=" + width + ", height=" + height);
						for (int w = 0; w < width; w++)
						{
							for (int h = 0; h < height; h++)
							{
								recipe.Input[(h * width) + w] = ReadRecipeIngredient();
							}
						}

						int resultCount = ReadVarInt(); // 1?
						for (int j = 0; j < resultCount; j++)
						{
							recipe.Result.Add(ReadItem(false));
						}
						recipe.Id = ReadUUID(); // Id
						recipe.Block = ReadString(); // block?
						ReadSignedVarInt(); // priority
						recipe.UniqueId = ReadVarInt(); // unique id
						//recipes.Add(recipe);
						//Log.Error("Read shaped recipe");
						break;
					}
					default:
						Log.Error($"Read unknown recipe type: {recipeType}");
						//ReadBytes(len);
						break;
				}
			}

			Log.Trace($"Done reading {count} recipes");

			return recipes;
		}

		public void WriteRecipeIngredient(Item stack)
		{
			if (stack == null || stack.Id == 0)
			{
				WriteVarInt(0);
				return;
			}

			WriteVarInt(stack.Id);
			WriteVarInt(stack.Metadata);
			WriteVarInt(stack.Count);
		}

		public Item ReadRecipeIngredient()
		{
			short id = (short) ReadVarInt();
			if (id == 0)
			{
				return new ItemAir();
			}

			short metadata = (short) ReadVarInt();
			int count = ReadVarInt();

			return ItemFactory.GetItem(id, metadata, count);
		}


		public void Write(PotionContainerChangeRecipe[] recipes)
		{
			WriteSignedVarInt(0);
		}

		public PotionContainerChangeRecipe[] ReadPotionContainerChangeRecipes()
		{
			int count = (int) ReadUnsignedVarInt();
			var recipes = new PotionContainerChangeRecipe[count];
			for (int i = 0; i < recipes.Length; i++)
			{
				var recipe = new PotionContainerChangeRecipe();
				recipe.Input = ReadVarInt();
				recipe.Ingredient = ReadVarInt();
				recipe.Output = ReadVarInt();

				recipes[i] = recipe;
			}

			return recipes;
		}

		public void Write(MaterialReducerRecipe[] reducerRecipes)
		{
			WriteUnsignedVarInt((uint) reducerRecipes.Length);

			for (int i = 0; i < reducerRecipes.Length; i++)
			{
				var recipe = reducerRecipes[i];
				WriteVarInt((recipe.Input << 16) | recipe.InputMeta);
				WriteUnsignedVarInt((uint) recipe.Output.Length);

				foreach (var output in recipe.Output)
				{
					WriteVarInt(output.ItemId);
					WriteVarInt(output.ItemCount);
				}
			}
		} 

		public MaterialReducerRecipe[] ReadMaterialReducerRecipes()
		{
			int count = (int) ReadUnsignedVarInt();
			var recipes = new MaterialReducerRecipe[count];
			for (int i = 0; i < recipes.Length; i++)
			{
				var inputIdAndMeta = ReadVarInt();
				var inputId = inputIdAndMeta >> 16;
				var inputMeta = inputIdAndMeta & 0x7fff;

				var outputCount = (int) ReadUnsignedVarInt();
				MaterialReducerRecipe.MaterialReducerRecipeOutput[] outputs = new MaterialReducerRecipe.MaterialReducerRecipeOutput[outputCount];

				for (int o = 0; o < outputs.Length; o++)
				{
					var itemId = ReadVarInt();
					var itemCount = ReadVarInt();

					outputs[o] = new MaterialReducerRecipe.MaterialReducerRecipeOutput(itemId, itemCount);
				}
				
				var recipe = new MaterialReducerRecipe(inputId, inputMeta, outputs);

				recipes[i] = recipe;
			}

			return recipes;
		}

		public void Write(PotionTypeRecipe[] recipes)
		{
			WriteSignedVarInt(0);
		}

		public PotionTypeRecipe[] ReadPotionTypeRecipes()
		{
			int count = (int) ReadUnsignedVarInt();
			var recipes = new PotionTypeRecipe[count];
			for (int i = 0; i < recipes.Length; i++)
			{
				var recipe = new PotionTypeRecipe();
				recipe.Input = ReadVarInt();
				recipe.InputMeta = ReadVarInt();
				recipe.Ingredient = ReadVarInt();
				recipe.IngredientMeta = ReadVarInt();
				recipe.Output = ReadVarInt();
				recipe.OutputMeta = ReadVarInt();

				recipes[i] = recipe;
			}

			return recipes;
		}


		const int MapUpdateFlagTexture = 0x02;
		const int MapUpdateFlagDecoration = 0x04;
		const int MapUpdateFlagInitialisation = 0x08;

		public void Write(MapInfo map)
		{
			WriteSignedVarLong(map.MapId);
			WriteUnsignedVarInt((uint) map.UpdateType);
			Write((byte) 0); // dimension
			Write(false); // Locked

			if ((map.UpdateType & MapUpdateFlagInitialisation) != 0)
			{
				WriteUnsignedVarInt(1);
				WriteSignedVarLong(map.MapId);
			}

			if ((map.UpdateType & (MapUpdateFlagInitialisation | MapUpdateFlagDecoration | MapUpdateFlagTexture)) != 0)
			{
				Write((byte) map.Scale);
			}

			if ((map.UpdateType & MapUpdateFlagDecoration) != 0)
			{
				var count = map.Decorators.Length;

				WriteUnsignedVarInt((uint) count);
				foreach (var decorator in map.Decorators)
				{
					if (decorator is EntityMapDecorator entity)
					{
						WriteSignedVarLong(entity.EntityId);
					}
					else if (decorator is BlockMapDecorator block)
					{
						Write(block.Coordinates);
					}
				}

				WriteUnsignedVarInt((uint) count);
				foreach (var decorator in map.Decorators)
				{
					Write((byte) decorator.Icon);
					Write((byte) decorator.Rotation);
					Write((byte) decorator.X);
					Write((byte) decorator.Z);
					Write(decorator.Label);
					WriteUnsignedVarInt(decorator.Color);
				}
			}

			if ((map.UpdateType & MapUpdateFlagTexture) != 0)
			{
				WriteSignedVarInt(map.Col);
				WriteSignedVarInt(map.Row);

				WriteSignedVarInt(map.XOffset);
				WriteSignedVarInt(map.ZOffset);

				WriteUnsignedVarInt((uint) (map.Col * map.Row));
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
			ReadByte(); // Dimension (waste)
			ReadBool(); // Locked (waste)

			if ((map.UpdateType & MapUpdateFlagInitialisation) == MapUpdateFlagInitialisation)
			{
				// Entities
				var count = ReadUnsignedVarInt();
				for (int i = 0; i < count - 1; i++) // This is some weird shit vanilla is doing with counting.
				{
					var eid = ReadSignedVarLong();
				}
			}

			if ((map.UpdateType & MapUpdateFlagTexture) == MapUpdateFlagTexture || (map.UpdateType & MapUpdateFlagDecoration) == MapUpdateFlagDecoration)
			{
				map.Scale = ReadByte();
				//Log.Warn($"Reading scale {map.Scale}");
			}

			if ((map.UpdateType & MapUpdateFlagDecoration) == MapUpdateFlagDecoration)
			{
				// Decorations
				//Log.Warn("Got decoration update, reading it");

				try
				{
					var entityCount = ReadUnsignedVarInt();
					for (int i = 0; i < entityCount; i++)
					{
						var type = ReadInt();
						if (type == 0)
						{
							// entity
							var q = ReadSignedVarLong();
						}
						else if (type == 1)
						{
							// block
							var b = ReadBlockCoordinates();
						}
					}

					var count = ReadUnsignedVarInt();
					map.Decorators = new MapDecorator[count];
					for (int i = 0; i < count; i++)
					{
						MapDecorator decorator = new MapDecorator();
						decorator.Icon = ReadByte();
						decorator.Rotation = ReadByte();
						decorator.X = ReadByte();
						decorator.Z = ReadByte();
						decorator.Label = ReadString();
						decorator.Color = ReadUnsignedVarInt();
						map.Decorators[i] = decorator;
					}
				}
				catch (Exception e)
				{
					Log.Error($"Error while reading decorations for map={map}", e);
				}
			}

			if ((map.UpdateType & MapUpdateFlagTexture) == MapUpdateFlagTexture)
			{
				// Full map
				try
				{
					map.Col = ReadSignedVarInt();
					map.Row = ReadSignedVarInt(); //

					map.XOffset = ReadSignedVarInt(); //
					map.ZOffset = ReadSignedVarInt(); //
					ReadUnsignedVarInt(); // size
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

		public void Write(ScoreEntries list)
		{
			if (list == null) list = new ScoreEntries();

			Write((byte) (list.FirstOrDefault() is ScoreEntryRemove ? McpeSetScore.Types.Remove : McpeSetScore.Types.Change));
			WriteUnsignedVarInt((uint) list.Count);
			foreach (var entry in list)
			{
				WriteSignedVarLong(entry.Id);
				Write(entry.ObjectiveName);
				Write(entry.Score);

				if (entry is ScoreEntryRemove)
				{
					continue;
				}

				if (entry is ScoreEntryChangePlayer player)
				{
					Write((byte) McpeSetScore.ChangeTypes.Player);
					WriteSignedVarLong(player.EntityId);
				}
				else if (entry is ScoreEntryChangeEntity entity)
				{
					Write((byte) McpeSetScore.ChangeTypes.Entity);
					WriteSignedVarLong(entity.EntityId);
				}
				else if (entry is ScoreEntryChangeFakePlayer fakePlayer)
				{
					Write((byte) McpeSetScore.ChangeTypes.FakePlayer);
					Write(fakePlayer.CustomName);
				}
			}
		}

		public ScoreEntries ReadScoreEntries()
		{
			var list = new ScoreEntries();
			byte type = ReadByte();
			var length = ReadUnsignedVarInt();
			for (var i = 0; i < length; ++i)
			{
				var entryId = ReadSignedVarLong();
				var entryObjectiveName = ReadString();
				var entryScore = ReadUint();

				ScoreEntry entry = null;

				if (type == (int) McpeSetScore.Types.Remove)
				{
					entry = new ScoreEntryRemove();
				}
				else
				{
					McpeSetScore.ChangeTypes changeType = (McpeSetScore.ChangeTypes) ReadByte();
					switch (changeType)
					{
						case McpeSetScore.ChangeTypes.Player:
							entry = new ScoreEntryChangePlayer {EntityId = ReadSignedVarLong()};
							break;
						case McpeSetScore.ChangeTypes.Entity:
							entry = new ScoreEntryChangeEntity {EntityId = ReadSignedVarLong()};
							break;
						case McpeSetScore.ChangeTypes.FakePlayer:
							entry = new ScoreEntryChangeFakePlayer {CustomName = ReadString()};
							break;
					}
				}

				if (entry == null) continue;

				entry.Id = entryId;
				entry.ObjectiveName = entryObjectiveName;
				entry.Score = entryScore;

				list.Add(entry);
			}

			return list;
		}

		public void Write(ScoreboardIdentityEntries list)
		{
			if (list == null) list = new ScoreboardIdentityEntries();

			Write((byte) (list.FirstOrDefault() is ScoreboardClearIdentityEntry ? McpeSetScoreboardIdentity.Operations.ClearIdentity : McpeSetScoreboardIdentity.Operations.RegisterIdentity));
			WriteUnsignedVarInt((uint) list.Count);
			foreach (var entry in list)
			{
				WriteSignedVarLong(entry.Id);
				if (entry is ScoreboardRegisterIdentityEntry reg)
				{
					WriteSignedVarLong(reg.EntityId);
				}
			}
		}

		public ScoreboardIdentityEntries ReadScoreboardIdentityEntries()
		{
			ScoreboardIdentityEntries list = new ScoreboardIdentityEntries();

			McpeSetScoreboardIdentity.Operations type = (McpeSetScoreboardIdentity.Operations) ReadByte();
			var length = ReadUnsignedVarInt();
			for (var i = 0; i < length; ++i)
			{
				var scoreboardId = ReadSignedVarLong();

				switch (type)
				{
					case McpeSetScoreboardIdentity.Operations.RegisterIdentity:
						list.Add(new ScoreboardRegisterIdentityEntry()
						{
							Id = scoreboardId,
							EntityId = ReadSignedVarLong()
						});
						break;
					case McpeSetScoreboardIdentity.Operations.ClearIdentity:
						list.Add(new ScoreboardClearIdentityEntry() {Id = scoreboardId});
						break;
				}

				// https://github.com/pmmp/PocketMine-MP/commit/39808dd94f4f2d1716eca31cb5a1cfe9000b6c38#diff-041914be0a0493190a4911ae5c4ac502R62
			}

			return list;
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
			return experiments;
		}

		public void Write(Experiments experiments)
		{
			if (experiments == null)
			{
				Write(0);
				return;
			}
			Write(experiments.Count);

			foreach (var experiment in experiments)
			{
				Write(experiment.Name);
				Write(experiment.Enabled);
			}
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

		public void Write(HeightMapData data)
		{
			if (data == null)
			{
				Write((byte) SubChunkPacketHeightMapType.NoData);

				return;
			}

			if (data.IsAllTooHigh)
			{
				Write((byte) SubChunkPacketHeightMapType.AllTooHigh);

				return;
			}

			if (data.IsAllTooLow)
			{
				Write((byte) SubChunkPacketHeightMapType.AllTooLow);

				return;
			}
			
			Write((byte) SubChunkPacketHeightMapType.Data);

			for (int i = 0; i < data.Heights.Length; i++)
			{
				Write((byte) data.Heights[i]);
			}
		}

		public HeightMapData ReadHeightMapData()
		{
			SubChunkPacketHeightMapType type = (SubChunkPacketHeightMapType) ReadByte();

			if (type != SubChunkPacketHeightMapType.Data)
				return null;

			short[] heights = new short[256];

			for (int i = 0; i < heights.Length; i++)
			{
				heights[i] = (short) ReadByte();
			}

			return new HeightMapData(heights);
		}

		public void Write(SubChunkPositionOffset offset)
		{
			Write(offset.XOffset);
			Write(offset.YOffset);
			Write(offset.ZOffset);
		}

		public SubChunkPositionOffset ReadSubChunkPositionOffset()
		{
			SubChunkPositionOffset result = new SubChunkPositionOffset();
			result.XOffset = ReadSByte();
			result.YOffset = ReadSByte();
			result.ZOffset = ReadSByte();
			return result;
		}

		public void Write(SubChunkPositionOffset[] offsets)
		{
			Write(offsets.Length);

			foreach (var offset in offsets)
			{
				Write(offset);
			}
		}
		
		public SubChunkPositionOffset[] ReadSubChunkPositionOffsets()
		{
			var count = ReadInt();
			SubChunkPositionOffset[] offsets = new SubChunkPositionOffset[count];

			for (int i = 0; i < offsets.Length; i++)
			{
				offsets[i] = ReadSubChunkPositionOffset();
			}

			return offsets;
		}

		public DimensionData ReadDimensionData()
		{
			DimensionData data = new DimensionData();
			data.MaxHeight = ReadVarInt();
			data.MinHeight = ReadVarInt();
			data.Generator = ReadVarInt();

			return data;
		}

		public void Write(DimensionData data)
		{
			WriteVarInt(data.MaxHeight);
			WriteVarInt(data.MinHeight);
			WriteVarInt(data.Generator);
		}
		
		public void Write(DimensionDefinitions definitions)
		{
			WriteUnsignedVarInt((uint) definitions.Count);

			foreach (var def in definitions)
			{
				Write(def.Key);
				Write(def.Value);
			}
		}
		
		public DimensionDefinitions ReadDimensionDefinitions()
		{
			DimensionDefinitions definitions = new DimensionDefinitions();
			
			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				var stringId = ReadString();
				var data = ReadDimensionData();

				definitions.TryAdd(stringId, data);
			}

			return definitions;
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

			ReliabilityHeader = new ReliabilityHeader();

			NoBatch = false;
			ForceClear = false;

			_encodedMessage = null;
			Bytes = null;
			Timer.Restart();

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

		private static RecyclableMemoryStreamManager _streamManager = new RecyclableMemoryStreamManager();
		private static ConcurrentDictionary<int, bool> _isLob = new ConcurrentDictionary<int, bool>();

		public virtual byte[] Encode()
		{
			byte[] cache = _encodedMessage;
			if (cache != null) return cache;

			lock (_encodeSync)
			{
				// This construct to avoid unnecessary contention and double encoding.
				if (_encodedMessage != null) return _encodedMessage;

				// Dynamic pooling. If this packet has been registered as a large object in previous
				// runs, we use the pooled stream for it instead to avoid LOB allocations
				bool isLob = _isLob.ContainsKey(Id);
				_buffer = isLob ? _streamManager.GetStream() : new MemoryStream();
				using (_writer = new BinaryWriter(_buffer, Encoding.UTF8, true))
				{
					EncodePacket();

					_writer.Flush();
					// This WILL allocate LOB. Need to convert this to work with array segment and pool it.
					// then we will use GetBuffer instead.
					// Also remember to move dispose entirely to Reset (dispose) when that happens.
					var buffer = (MemoryStream) _buffer;
					_encodedMessage = buffer.ToArray();
					if (!isLob && _encodedMessage.Length >= 85_000)
					{
						_isLob.TryAdd(Id, true);
						//Log.Warn($"LOB {GetType().Name} {_encodedMessage.Length}, IsLOB={_isLob}");
					}
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
			else Write(Id);
		}

		[Obsolete("Use decode with ReadOnlyMemory<byte> instead.")]
		public virtual Packet Decode(byte[] buffer)
		{
			return Decode(new ReadOnlyMemory<byte>(buffer));
		}

		public virtual Packet Decode(ReadOnlyMemory<byte> buffer)
		{
			Bytes = buffer;
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
			Id = IsMcpe ? (byte) ReadVarInt() : ReadByte();
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


		public T MarkPermanent(bool permanent = true)
		{
			if (!_isPooled) throw new Exception("Tried to make non pooled item permanent");
			_isPermanent = permanent;

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
			item.Timer.Restart();
			return item;
		}

		// DO NOT UNCOMMENT THIS!!!
		// It will have > 100% performance impact overall.
		//~Packet()
		//{
		//	if (_isPooled)
		//	{
		//		//Log.Error($"Unexpected dispose 0x{Id:x2} {GetType().Name}, IsPooled={_isPooled}, IsPermanent={_isPermanent}, Refs={_referenceCounter}");
		//	}
		//}

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