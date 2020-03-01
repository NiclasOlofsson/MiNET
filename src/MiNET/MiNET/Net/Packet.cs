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
using MiNET.Crafting;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Skins;
using Newtonsoft.Json;

namespace MiNET.Net
{
	public abstract partial class Packet
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Packet));

		private bool _isEncoded;
		private byte[] _encodedMessage;

		[JsonIgnore] public int DatagramSequenceNumber;

		[JsonIgnore] public bool NoBatch { get; set; }

		[JsonIgnore] public Reliability Reliability = Reliability.Unreliable;
		[JsonIgnore] public int ReliableMessageNumber;
		[JsonIgnore] public byte OrderingChannel;
		[JsonIgnore] public int OrderingIndex;

		[JsonIgnore] public bool ForceClear;

		[JsonIgnore] public byte Id;
		[JsonIgnore] public bool IsMcpe;

		protected MemoryStream _buffer;
		private BinaryWriter _writer;
		private BinaryReader _reader;

		[JsonIgnore] public byte[] Bytes { get; private set; }
		[JsonIgnore] public Stopwatch Timer { get; } = new Stopwatch();

		public Packet()
		{
			Timer.Start();
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

		public void Write(Memory<byte> value)
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

		public byte[] ReadBytes(int count, bool slurp = false)
		{
			if (!slurp && count == 0) return new byte[0];

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

			if (value.Length == 0) return;

			_writer.Write(value, 0, value.Length);
		}

		public byte[] ReadByteArray(bool slurp = false)
		{
			var len = ReadLength();
			var bytes = ReadBytes(len, slurp);
			return bytes;
		}

		public void Write(short value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(BinaryPrimitives.ReverseEndianness(value));
			else _writer.Write(value);
		}

		public short ReadShort(bool bigEndian = false)
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return 0;

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
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return 0;

			if (bigEndian) return BinaryPrimitives.ReverseEndianness(_reader.ReadUInt16());

			return _reader.ReadUInt16();
		}

		public void WriteBe(short value)
		{
			_writer.Write(BinaryPrimitives.ReverseEndianness(value));
		}

		public short ReadShortBe()
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return 0;

			return BinaryPrimitives.ReverseEndianness(_reader.ReadInt16());
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
						ReadString(); // TODO: xuid
						var platformChatId = ReadString(); // TODO: platform chat ID
						ReadInt(); // TODO: device os
						player.Skin = ReadSkin();
						ReadBool(); // is teacher
						ReadBool(); // is host

						records.Add(player);
						Log.Debug($"Reading {player.ClientUuid}, {player.EntityId}, '{player.DisplayName}', {platformChatId}");
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
			if (count == 20 && _reader.BaseStream.Length < 120) count = 10;
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
			return ReadNbt(_reader.BaseStream);
		}

		public static Nbt ReadNbt(Stream stream)
		{
			Nbt nbt = new Nbt();
			NbtFile file = new NbtFile();
			file.BigEndian = false;
			file.UseVarInt = true;
			nbt.NbtFile = file;
			file.LoadFromStream(stream, NbtCompression.None);

			return nbt;
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

		public void Write(ItemStacks metadata)
		{
			if (metadata == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint) metadata.Count);

			for (int i = 0; i < metadata.Count; i++)
			{
				Write(metadata[i]);
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

		public void Write(Transaction transaction)
		{
			switch (transaction)
			{
				case InventoryMismatchTransaction _:
					WriteVarInt((int) McpeInventoryTransaction.TransactionType.InventoryMismatch);
					break;
				case ItemReleaseTransaction _:
					WriteVarInt((int) McpeInventoryTransaction.TransactionType.ItemRelease);
					break;
				case ItemUseOnEntityTransaction _:
					WriteVarInt((int) McpeInventoryTransaction.TransactionType.ItemUseOnEntity);
					break;
				case ItemUseTransaction _:
					WriteVarInt((int) McpeInventoryTransaction.TransactionType.ItemUse);
					break;
				case NormalTransaction _:
					WriteVarInt((int) McpeInventoryTransaction.TransactionType.Normal);
					break;
			}

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
			}

			switch (transaction)
			{
				case NormalTransaction _:
				case InventoryMismatchTransaction _:
					break;
				case ItemUseTransaction t:
					WriteVarInt((int) t.ActionType);
					Write(t.Position);
					WriteSignedVarInt(t.Face);
					WriteSignedVarInt(t.Slot);
					Write(t.Item);
					Write(t.FromPosition);
					Write(t.ClickPosition);
					WriteUnsignedVarInt(t.BlockRuntimeId);
					break;
				case ItemUseOnEntityTransaction t:
					WriteVarLong(t.EntityId);
					WriteVarInt((int) t.ActionType);
					WriteSignedVarInt(t.Slot);
					Write(t.Item);
					Write(t.FromPosition);
					Write(t.ClickPosition);
					break;
				case ItemReleaseTransaction t:
					WriteVarInt((int) t.ActionType);
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
			var transactions = new List<TransactionRecord>();
			var transactionType = (McpeInventoryTransaction.TransactionType) ReadVarInt();
			var count = ReadUnsignedVarInt();
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

			return transaction;
		}

		public void Write(Item stack)
		{
			if (stack == null || stack.Id == 0)
			{
				WriteSignedVarInt(0);
				return;
			}

			WriteSignedVarInt(stack.Id);
			short metadata = stack.Metadata;
			if (metadata == -1) metadata = short.MaxValue;
			WriteSignedVarInt((metadata << 8) + (stack.Count & 0xff));

			if (stack.ExtraData != null)
			{
				byte[] bytes = GetNbtData(stack.ExtraData);
				Write((ushort) 0xffff); //(short) bytes.Length
				Write((byte) 0x01);
				Write(bytes);
			}
			else
			{
				Write((short) 0);
			}

			WriteSignedVarInt(0);
			WriteSignedVarInt(0);

			if (stack.Id == 513) // shield
			{
				WriteSignedVarInt(0); // something about tick, crap code
			}
		}

		public Item ReadItem()
		{
			int id = ReadSignedVarInt();
			if (id == 0)
			{
				return new ItemAir();
			}

			int tmp = ReadSignedVarInt();
			short metadata = (short) (tmp >> 8);
			if (metadata == short.MaxValue) metadata = -1;
			byte count = (byte) (tmp & 0xff);
			Item stack = ItemFactory.GetItem((short) id, metadata, count);

			ushort nbtLen = _reader.ReadUInt16(); // NbtLen
			if (nbtLen == 0xffff && ReadByte() == 1)
			{
				stack.ExtraData = (NbtCompound) ReadNbt().NbtFile.RootTag;
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

			if (id == 513) // shield
			{
				ReadSignedVarInt(); // something about tick, crap code
			}

			return stack;
		}


		public static byte[] GetNbtData(NbtCompound nbtCompound)
		{
			nbtCompound.Name = string.Empty;
			var file = new NbtFile(nbtCompound);
			file.BigEndian = false;
			file.UseVarInt = true;

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
						GameRule<bool> rule = new GameRule<bool>(name, ReadBool());
						gameRules.Add(rule);
						break;
					}
					case 2:
					{
						GameRule<int> rule = new GameRule<int>(name, ReadVarInt());
						gameRules.Add(rule);
						break;
					}
					case 3:
					{
						GameRule<float> rule = new GameRule<float>(name, ReadFloat());
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
				if (rule is GameRule<bool>)
				{
					Write((byte) 1);
					Write(((GameRule<bool>) rule).Value);
				}
				else if (rule is GameRule<int>)
				{
					Write((byte) 2);
					WriteVarInt(((GameRule<int>) rule).Value);
				}
				else if (rule is GameRule<float>)
				{
					Write((byte) 3);
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
				result.Add(runtimeId, new Itemstate
				{
					Id = legacyId,
					RuntimeId = runtimeId,
					Name = name
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
			foreach (var itemstate in itemstates.OrderBy(kvp => kvp.Key))
			{
				Write(itemstate.Value.Name);
				Write(itemstate.Value.Id);
			}
		}

		public BlockPalette ReadBlockPalette()
		{
			var result = new BlockPalette();

			var nbt = new Nbt();
			var file = new NbtFile();
			file.BigEndian = false;
			file.UseVarInt = true;
			file.AllowAlternativeRootTag = true;
			nbt.NbtFile = file;
			file.LoadFromStream(_reader.BaseStream, NbtCompression.None);

			int runtimeId = 0;
			var rootTag = (NbtList) file.RootTag;
			foreach (NbtTag tag in rootTag)
			{
				var record = new BlockStateContainer();
				record.RuntimeId = runtimeId++;
				record.Id = tag["id"].ShortValue;

				var blockTag = (NbtCompound) tag["block"];
				record.Name = blockTag["name"].StringValue;
				if (blockTag.Contains("data")) record.Data = blockTag["data"].ShortValue;

				record.States = new List<IBlockState>();
				var states = (NbtCompound) blockTag["states"];
				foreach (NbtTag stateTag in states)
				{
					IBlockState state = null;
					switch (stateTag.TagType)
					{
						case NbtTagType.Byte:
							state = new BlockStateByte()
							{
								Name = stateTag.Name,
								Value = stateTag.ByteValue
							};
							break;
						case NbtTagType.Int:
							state = new BlockStateInt()
							{
								Name = stateTag.Name,
								Value = stateTag.IntValue
							};
							break;
						case NbtTagType.String:
							state = new BlockStateString()
							{
								Name = stateTag.Name,
								Value = stateTag.StringValue
							};
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					record.States.Add(state);
				}

				result.Add(record);
			}

			return result;
		}

		public void Write(BlockPalette palette)
		{
			var list = new NbtList("", NbtTagType.Compound);
			foreach (BlockStateContainer record in palette)
			{
				var states = new List<NbtTag>();
				foreach (IBlockState state in record.States)
				{
					NbtTag stateTag = null;
					switch (state)
					{
						case BlockStateByte blockStateByte:
							stateTag = new NbtByte(state.Name, blockStateByte.Value);
							break;
						case BlockStateInt blockStateInt:
							stateTag = new NbtInt(state.Name, blockStateInt.Value);
							break;
						case BlockStateString blockStateString:
							stateTag = new NbtString(state.Name, blockStateString.Value);
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(state));
					}

					states.Add(stateTag);
				}

				var recordTag = new NbtCompound()
				{
					new NbtShort("id", (short) record.Data),
					new NbtShort("data", (short) record.Id),
					new NbtCompound("block")
					{
						new NbtString("name", record.Name),
						new NbtCompound("states", states),
						new NbtInt("version", BlockPalette.Version),
					}
				};

				list.Add(recordTag);
			}

			// Get bytes
			var nbt = new NbtFile()
			{
				BigEndian = false,
				UseVarInt = true,
				RootTag = list
			};

			byte[] nbtBinary = nbt.SaveToBuffer(NbtCompression.None);

			Write(nbtBinary);
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
				_writer.Write((byte) 1);
				_writer.Write((byte) 0);
			}
		}

		public Links ReadLinks()
		{
			var count = ReadUnsignedVarInt();

			var links = new Links();
			for (int i = 0; i < count; i++)
			{
				Tuple<long, long> link = new Tuple<long, long>(ReadSignedVarLong(), ReadSignedVarLong());
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
				Write(""); //TODO: encryption key
				Write(""); //TODO: subpack name
				Write(""); //TODO: content identity
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
				info.PackIdVersion = new PackIdVersion
				{
					Id = id,
					Version = version,
				};
				info.Size = size;
				info.HasScripts = hasScripts;
				packInfos.Add(info);
			}

			return packInfos;
		}

		public void Write(ResourcePackIdVersions packInfos)
		{
			if (packInfos == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}
			WriteUnsignedVarInt((uint) packInfos.Count); // LE
			foreach (var info in packInfos)
			{
				Write(info.Id);
				Write(info.Version);
				Write(info.Unknown);
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
				var unknown = ReadString();
				var info = new PackIdVersion
				{
					Id = id,
					Version = version,
					Unknown = unknown
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
			Write(skin.AnimationData);
			Write(skin.IsPremiumSkin);
			Write(skin.IsPersonaSkin);
			Write(skin.Cape.OnClassicSkin);
			Write(skin.Cape.Id);
			Write(skin.SkinId + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()); // some unique skin id
		}

		public Skin ReadSkin()
		{
			Skin skin = new Skin();

			skin.SkinId = ReadString();
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
					}
				);
			}

			skin.Cape.ImageWidth = ReadInt();
			skin.Cape.ImageHeight = ReadInt();
			skin.Cape.Data = ReadByteArray(false);
			skin.GeometryData = ReadString();
			skin.AnimationData = ReadString();
			skin.IsPremiumSkin = ReadBool();
			skin.IsPersonaSkin = ReadBool();
			skin.Cape.OnClassicSkin = ReadBool();
			skin.Cape.Id = ReadString();
			ReadString(); // some unique skin id

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
					case MultiRecipe _:
						WriteSignedVarInt(Multi); // Type
						Write(recipe.Id);
						break;
				}
			}
		}

		public Recipes ReadRecipes()
		{
			var recipes = new Recipes();

			int count = (int) ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				int recipeType = ReadSignedVarInt();

				//Log.Error($"Read recipe no={i} type={recipeType}");

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
						int ingrediensCount = ReadVarInt(); // 
						for (int j = 0; j < ingrediensCount; j++)
						{
							recipe.Input.Add(ReadRecipeIngredient());
						}
						int resultCount = ReadVarInt(); // 1?
						for (int j = 0; j < resultCount; j++)
						{
							recipe.Result.Add(ReadItem());
						}
						recipe.Id = ReadUUID(); // Id
						recipe.Block = ReadString(); // block?
						recipes.Add(recipe);
						ReadSignedVarInt(); // priority
						//Log.Error("Read shapeless recipe");
						break;
					}
					case Shaped:
					{
						ReadString(); // some unique id
						int width = ReadSignedVarInt(); // Width
						int height = ReadSignedVarInt(); // Height
						var recipe = new ShapedRecipe(width, height);
						if (width > 3 || height > 3) throw new Exception("Wrong number of ingredience. Width=" + width + ", height=" + height);
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
							recipe.Result.Add(ReadItem());
						}
						recipe.Id = ReadUUID(); // Id
						recipe.Block = ReadString(); // block?
						recipes.Add(recipe);
						ReadSignedVarInt(); // priority
						//Log.Error("Read shaped recipe");
						break;
					}
					case Furnace:
					{
						var recipe = new SmeltingRecipe();
						//short meta = (short) ReadVarInt(); // input (with metadata) 
						short id = (short) ReadSignedVarInt(); // input (with metadata) 
						Item result = ReadItem(); // Result
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
						short id = (short) ReadSignedVarInt(); // input (with metadata) 
						short meta = (short) ReadSignedVarInt(); // input (with metadata) 
						Item result = ReadItem(); // Result
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
						recipes.Add(recipe);
						break;
					}
					case ShapelessChemistry:
					{
						int ingrediensCount = ReadVarInt(); // 
						for (int j = 0; j < ingrediensCount; j++)
						{
							ReadItem();
						}
						int resultCount = ReadVarInt(); // 
						for (int j = 0; j < resultCount; j++)
						{
							ReadItem();
						}

						ReadUUID();
						ReadString(); // block?
						break;
					}
					case ShapedChemistry:
					{
						int width = ReadSignedVarInt(); // Width
						int height = ReadSignedVarInt(); // Height
						if (width > 3 || height > 3) throw new Exception("Wrong number of ingredience. Width=" + width + ", height=" + height);
						for (int w = 0; w < width; w++)
						{
							for (int h = 0; h < height; h++)
							{
								ReadItem();
							}
						}

						int resultCount = ReadVarInt(); // 1?
						for (int j = 0; j < resultCount; j++)
						{
							ReadItem();
						}

						ReadUUID(); // Id
						ReadString(); // block?
						break;
					}
					default:
						Log.Error($"Read unknown recipe type: {recipeType}");
						//ReadBytes(len);
						break;
				}
			}

			return recipes;
		}

		public void WriteRecipeIngredient(Item stack)
		{
			if (stack == null || stack.Id == 0)
			{
				WriteSignedVarInt(0);
				return;
			}

			WriteSignedVarInt(stack.Id);
			WriteSignedVarInt(stack.Metadata);
			WriteSignedVarInt(stack.Count);
		}

		public Item ReadRecipeIngredient()
		{
			short id = (short) ReadSignedVarInt();
			if (id == 0)
			{
				return new ItemAir();
			}

			short metadata = (short) ReadSignedVarInt();
			int count = ReadSignedVarInt();

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
				recipe.InputItemId = ReadVarInt();
				recipe.IngredientItemId = ReadVarInt();
				recipe.OutputItemId = ReadVarInt();

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
				recipe.InputPotionType = ReadVarInt();
				recipe.IngredientItemId = ReadVarInt();
				recipe.OutputPotionType = ReadVarInt();

				recipes[i] = recipe;
			}

			return recipes;
		}


		const int BITFLAG_TEXTURE_UPDATE = 0x02;
		const int BITFLAG_DECORATION_UPDATE = 0x04;
		const int BITFLAG_ENTITY_UPDATE = 0x08;

		public void Write(MapInfo map)
		{
			WriteSignedVarLong(map.MapId);
			WriteUnsignedVarInt((uint) map.UpdateType);
			Write((byte) 0); // dimension
			Write(false); // Locked

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

			if ((map.UpdateType & BITFLAG_TEXTURE_UPDATE) == BITFLAG_TEXTURE_UPDATE)
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

			if ((map.UpdateType & BITFLAG_TEXTURE_UPDATE) == BITFLAG_TEXTURE_UPDATE)
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

			Write((byte) (list.FirstOrDefault() is ScoreboardClearIdentityEntry ? McpeSetScoreboardIdentityPacket.Operations.ClearIdentity : McpeSetScoreboardIdentityPacket.Operations.RegisterIdentity));
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

			McpeSetScoreboardIdentityPacket.Operations type = (McpeSetScoreboardIdentityPacket.Operations) ReadByte();
			var length = ReadUnsignedVarInt();
			for (var i = 0; i < length; ++i)
			{
				var scoreboardId = ReadSignedVarLong();

				switch (type)
				{
					case McpeSetScoreboardIdentityPacket.Operations.RegisterIdentity:
						list.Add(new ScoreboardRegisterIdentityEntry()
						{
							Id = scoreboardId,
							EntityId = ReadSignedVarLong()
						});
						break;
					case McpeSetScoreboardIdentityPacket.Operations.ClearIdentity:
						list.Add(new ScoreboardClearIdentityEntry() {Id = scoreboardId});
						break;
				}

				// https://github.com/pmmp/PocketMine-MP/commit/39808dd94f4f2d1716eca31cb5a1cfe9000b6c38#diff-041914be0a0493190a4911ae5c4ac502R62
			}

			return list;
		}


		public bool CanRead()
		{
			return _reader.BaseStream.Position < _reader.BaseStream.Length;
		}

		public void SetEncodedMessage(byte[] encodedMessage)
		{
			_encodedMessage = encodedMessage;
			_isEncoded = true;
		}

		public virtual void Reset()
		{
			ResetPacket();

			DatagramSequenceNumber = -1;

			Reliability = Reliability.Unreliable;
			ReliableMessageNumber = -1;
			OrderingChannel = 0;
			OrderingIndex = -1;

			NoBatch = false;
			ForceClear = false;

			_encodedMessage = null;
			Bytes = null;
			Timer.Restart();
			_isEncoded = false;

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
		private static ConcurrentDictionary<Type, bool> _isLob = new ConcurrentDictionary<Type, bool>();

		public virtual byte[] Encode()
		{
			lock (_encodeSync)
			{
				if (_isEncoded) return _encodedMessage;

				_isEncoded = false;

				// Dynamic pooling. If this packet has been registered as a large object in previous
				// runs, we use the pooled stream for it instead to avoid LOB allocations
				bool isLob = _isLob.ContainsKey(GetType());
				_buffer = isLob ? _streamManager.GetStream() : new MemoryStream();
				using (_writer = new BinaryWriter(_buffer, Encoding.UTF8, true))
				{
					EncodePacket();

					_writer.Flush();
					// This WILL allocate LOB. Need to convert this to work with array segment and pool it.
					// then we will use GetBuffer instead.
					// Also remember to move dispose entirely to Reset (dispose) when that happens.
					_encodedMessage = _buffer.ToArray();
					if (!isLob && _encodedMessage.Length >= 85_000)
					{
						_isLob.TryAdd(GetType(), true);
						//Log.Warn($"LOB {GetType().Name} {_encodedMessage.Length}, IsLOB={_isLob}");
					}
					//else if (isLob && _encodedMessage.Length < 85_000)
					//{
					//	if (GetType() != typeof(McpeWrapper)) Log.Warn($"Marked as LOB {GetType().Name} but size was not LOB {_encodedMessage.Length}");
					//}
					_isEncoded = true;

					//ResetPacket();

					//_writer.Dispose();
					//_buffer.Dispose();
					//_writer = null;
					//_buffer = null;
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
			Write(Id); //TODO: If MCPE write varint
			//if (IsMcpe) Write((short) 0);
		}

		public virtual void Decode(byte[] buffer)
		{
			Bytes = buffer;
			_buffer = new MemoryStream(buffer, false);
			_reader = new BinaryReader(_buffer, Encoding.UTF8, true);

			DecodePacket();

			if (Log.IsDebugEnabled && _buffer.Position != (buffer.Length))
			{
				Log.Warn($"{GetType().Name}: Still have {buffer.Length - _buffer.Position} bytes to read!!\n{HexDump(buffer)}");
			}

			_reader.Dispose();
			_buffer.Dispose();
			_reader = null;
			_buffer = null;
		}

		protected virtual void DecodePacket()
		{
			_buffer.Position = 0;
			if (!IsMcpe) Id = ReadByte();
		}

		public void CloneReset()
		{
			_buffer = new MemoryStream();
			_reader = new BinaryReader(_buffer);
			_writer = new BinaryWriter(_buffer);
			Timer.Start();
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