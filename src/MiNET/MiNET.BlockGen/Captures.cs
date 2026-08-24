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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Text;
using fNbt;

namespace MiNET.BlockGen;

/// <summary>
///     Reads the two captured BDS frames under Captures/. They are the CHECK the generated item
///     registry and creative catalog are measured against, never a source: nothing here feeds the
///     emitted files except through a named, counted gap (see <see cref="ItemGenerator" />).
/// </summary>
public static class Captures
{
	/// <summary>One item_registry entry as BDS sent it.</summary>
	public sealed record FrameItem(string Name, short Id, bool ComponentBased, int Version, NbtCompound Tree, byte[] TreeBytes);

	/// <summary>One item stack as a creative frame carries it (NetworkItemInstanceDescriptor).</summary>
	public sealed record FrameStack(int NetworkId, int Count, int Metadata, int BlockRuntimeId, byte[] NbtBytes, List<string> CanPlaceOn, List<string> CanDestroy);

	public sealed record FrameGroup(int Category, string Name, FrameStack Icon);

	public sealed record FrameEntry(int CreativeNetId, FrameStack Stack, int GroupIndex);

	public sealed record FrameCreative(List<FrameGroup> Groups, List<FrameEntry> Entries);

	/// <summary>
	///     item_registry (McpeItemComponent, id 162): varuint packet header, varuint count, then per
	///     entry a string id, an le16 network id, a component-based byte, a zigzag version and one
	///     network NBT tree with a named root. An item with no components carries an empty compound,
	///     which is reported here as a null tree.
	/// </summary>
	public static List<FrameItem> ReadItemRegistry(string path)
	{
		var reader = new Reader(File.ReadAllBytes(path));
		int header = (int) reader.ReadVarUInt();
		if ((header & 0x3ff) != 162) throw new InvalidDataException($"{Path.GetFileName(path)} is packet id {header & 0x3ff}, not 162");

		int count = (int) reader.ReadVarUInt();
		var items = new List<FrameItem>(count);
		for (int i = 0; i < count; i++)
		{
			string name = reader.ReadString();
			short id = reader.ReadShort();
			bool componentBased = reader.ReadByte() != 0;
			int version = reader.ReadZigZag32();

			int start = reader.Position;
			NbtCompound tree = reader.ReadNbt(useVarInt: true);
			byte[] bytes = reader.Slice(start, reader.Position - start);

			items.Add(new FrameItem(name, id, componentBased, version, tree.Count == 0 ? null : tree, tree.Count == 0 ? null : bytes));
		}

		if (!reader.AtEnd) throw new InvalidDataException($"{Path.GetFileName(path)} has {reader.Remaining} bytes left after {count} entries");
		return items;
	}

	/// <summary>
	///     creative_content (McpeCreativeContent, id 145): varuint header, then the groups (a
	///     category byte, a name and an icon stack) and the entries (a creative net id, a stack and
	///     the index of the group it sits in).
	/// </summary>
	public static FrameCreative ReadCreativeContent(string path, int shieldNetworkId)
	{
		var reader = new Reader(File.ReadAllBytes(path));
		int header = (int) reader.ReadVarUInt();
		if ((header & 0x3ff) != 145) throw new InvalidDataException($"{Path.GetFileName(path)} is packet id {header & 0x3ff}, not 145");

		int groupCount = (int) reader.ReadVarUInt();
		var groups = new List<FrameGroup>(groupCount);
		for (int i = 0; i < groupCount; i++)
		{
			int category = reader.ReadByte();
			string name = reader.ReadString();
			groups.Add(new FrameGroup(category, name, ReadStack(reader, shieldNetworkId)));
		}

		int entryCount = (int) reader.ReadVarUInt();
		var entries = new List<FrameEntry>(entryCount);
		for (int i = 0; i < entryCount; i++)
		{
			int creativeNetId = (int) reader.ReadVarUInt();
			FrameStack stack = ReadStack(reader, shieldNetworkId);
			entries.Add(new FrameEntry(creativeNetId, stack, (int) reader.ReadVarUInt()));
		}

		if (!reader.AtEnd) throw new InvalidDataException($"{Path.GetFileName(path)} has {reader.Remaining} bytes left after {groupCount} groups and {entryCount} entries");
		return new FrameCreative(groups, entries);
	}

	/// <summary>
	///     NetworkItemInstanceDescriptor: zigzag network id, then always an le16 count, a varuint
	///     aux value, a zigzag block runtime id and a length-prefixed trailer, even for id 0. The
	///     trailer holds an le16 -1 marker plus a version byte when NBT follows (fixed little endian,
	///     not varint), then the can-place-on and can-destroy string lists as le32 counts, then the
	///     blocking tick for the shield alone.
	/// </summary>
	private static FrameStack ReadStack(Reader reader, int shieldNetworkId)
	{
		int networkId = reader.ReadZigZag32();
		int count = reader.ReadUShort();
		int metadata = (int) reader.ReadVarUInt();
		int blockRuntimeId = reader.ReadZigZag32();

		int blobLength = (int) reader.ReadVarUInt();
		byte[] blob = reader.ReadBytes(blobLength);
		if (blobLength == 0) return new FrameStack(networkId, count, metadata, blockRuntimeId, null, null, null);

		var trailer = new Reader(blob);
		byte[] nbt = null;
		ushort marker = trailer.ReadUShort();
		if (marker == 0xffff)
		{
			byte version = trailer.ReadByte();
			if (version != 1) throw new InvalidDataException($"item extra data NBT version {version}");
			int start = trailer.Position;
			trailer.ReadNbt(useVarInt: false);
			nbt = trailer.Slice(start, trailer.Position - start);
		}
		else if (marker != 0)
		{
			throw new InvalidDataException($"item extra data marker {marker:x4}");
		}

		List<string> canPlaceOn = ReadNameList(trailer);
		List<string> canDestroy = ReadNameList(trailer);
		if (networkId == shieldNetworkId) trailer.ReadBytes(8); // blocking_tick

		if (!trailer.AtEnd) throw new InvalidDataException($"item extra data has {trailer.Remaining} bytes left");
		return new FrameStack(networkId, count, metadata, blockRuntimeId, nbt, canPlaceOn, canDestroy);
	}

	private static List<string> ReadNameList(Reader reader)
	{
		int count = reader.ReadInt32();
		if (count == 0) return null;

		var names = new List<string>(count);
		for (int i = 0; i < count; i++)
		{
			int length = reader.ReadShort();
			names.Add(Encoding.UTF8.GetString(reader.ReadBytes(length)));
		}

		return names;
	}

	/// <summary>The wire primitives a Bedrock frame is written in, over one buffer.</summary>
	private sealed class Reader
	{
		private readonly byte[] _buffer;
		private int _position;

		public Reader(byte[] buffer) => _buffer = buffer;

		public int Position => _position;

		public bool AtEnd => _position == _buffer.Length;

		public int Remaining => _buffer.Length - _position;

		public byte ReadByte() => _buffer[_position++];

		public byte[] ReadBytes(int count)
		{
			byte[] bytes = _buffer[_position..(_position + count)];
			_position += count;
			return bytes;
		}

		public byte[] Slice(int start, int length) => _buffer[start..(start + length)];

		public uint ReadVarUInt()
		{
			uint value = 0;
			int shift = 0;
			while (true)
			{
				byte b = ReadByte();
				value |= (uint) (b & 0x7f) << shift;
				if ((b & 0x80) == 0) return value;
				shift += 7;
			}
		}

		public int ReadZigZag32()
		{
			uint raw = ReadVarUInt();
			return (int) (raw >> 1) ^ -(int) (raw & 1);
		}

		public ushort ReadUShort()
		{
			ushort value = (ushort) (_buffer[_position] | (_buffer[_position + 1] << 8));
			_position += 2;
			return value;
		}

		public short ReadShort() => (short) ReadUShort();

		public int ReadInt32()
		{
			int value = _buffer[_position] | (_buffer[_position + 1] << 8) | (_buffer[_position + 2] << 16) | (_buffer[_position + 3] << 24);
			_position += 4;
			return value;
		}

		public string ReadString() => Encoding.UTF8.GetString(ReadBytes((int) ReadVarUInt()));

		public NbtCompound ReadNbt(bool useVarInt)
		{
			var file = new NbtFile {BigEndian = false, UseVarInt = useVarInt, AllowAlternativeRootTag = false};
			using var stream = new MemoryStream(_buffer, _position, _buffer.Length - _position, false);
			long read = file.LoadFromStream(stream, NbtCompression.None);
			_position += (int) read;
			return (NbtCompound) file.RootTag;
		}
	}
}
