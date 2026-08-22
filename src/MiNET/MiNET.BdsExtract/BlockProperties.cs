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

using System.Text;

namespace MiNET.BdsExtract;

/// <summary>One block as the server holds it. These are properties of the block, not of its states.</summary>
public sealed class BlockProperties
{
	public string Name { get; init; }
	public ulong NameHash { get; init; }

	/// <summary>The pre-flattening numeric id, and the tint, both read from where the class keeps them.</summary>
	public int LegacyId { get; init; }

	public string TintMethod { get; init; }

	/// <summary>
	///     The block's tags, in the server's order. These say what the block IS to the rest of the
	///     game: which tool destroys it, which family it belongs to. They are held on the block, not
	///     on its states, so every state of a block carries this same list.
	/// </summary>
	public IReadOnlyList<string> Tags { get; init; } = [];

	/// <summary>
	///     The name this block had before the flattening, tile.acacia_button. Every block has one
	///     and nothing else publishes the mapping from it to the modern id.
	/// </summary>
	public string SerializationId { get; init; }

	/// <summary>
	///     The creative tab the block appears under, or null for the ones a player cannot reach.
	/// </summary>
	public string CreativeGroup { get; init; }

	/// <summary>
	///     The shape the block declares, as the server names it rather than as a box: unit_cube for
	///     most, box_16x1x16 for a carpet, minecraft:fence or minecraft:cactus for a model. Null
	///     when the block declares none, which is most of them, and then the shape is whatever the
	///     class implementing it draws.
	///     The dimensions in a box name are sixteenths, so box_16x1x16 is the box from the origin to
	///     (1, 0.0625, 1). Checked against a published table: all 24 box geometries and all 454 unit
	///     cubes agree with it exactly.
	/// </summary>
	public string Geometry { get; init; }

	/// <summary>
	///     The method table of the C++ class implementing this block, which is the only type
	///     identity a binary with no RTTI has. Blocks sharing one are the same class: the stairs
	///     are one class, the sixteen wool colours another. Nothing publishes that grouping.
	/// </summary>
	public ulong Vtable { get; init; }

	/// <summary>Address of the BlockLegacy this was read from. Only meaningful while the server lives.</summary>
	public ulong Address { get; init; }

	/// <summary>
	///     The object's size from the allocator, and the stretches of it no field accounts for.
	///     Both go in the output rather than staying private, because without them a reader cannot
	///     tell how much of the object these values represent, and the answer is well under half.
	///     A null hole list means the size could not be measured, which is a different answer from
	///     having no holes and must not print the same.
	/// </summary>
	public int ObjectSize { get; init; }

	public IReadOnlyList<ByteRange> Unread { get; init; }
}

/// <summary>
///     Finds every block in a running server and reads its properties.
///     The search has no list of expected names. It sweeps memory for the HashedString pattern, a
///     hash followed by text that hashes to it, which identifies a block name and nothing else.
///     Each block points at its default state, which is where hardness, blast resistance and the
///     rest are read from, at the offsets in <see cref="MemoryLayout" />.
/// </summary>
public static class BlockRegistry
{
	/// <summary>
	///     How much of a candidate the sweep reads before it knows anything about the class. Far
	///     enough to hold a name and the pointer behind it; what the object actually is comes from
	///     its own class the moment the start is found.
	/// </summary>
	private const int SweepWindow = 1024;

	/// <summary>
	///     Room for the largest state object any class declares, so one buffer serves every read.
	///     Nothing is read past what a given object's own class states.
	/// </summary>
	private const int LargestState = 8192;

	private const string Namespace = "minecraft:";

	public static List<BlockProperties> Read(BedrockProcess process)
	{
		var hits = new Dictionary<string, int>(StringComparer.Ordinal);
		var found = new Dictionary<ulong, BlockProperties>();
		var window = new byte[8 * 1024 * 1024];
		var heap = new byte[256];
		// Sized to the largest state class rather than to a field offset: the class states its own
		// size and this buffer is reused, so it holds the biggest of them and each read takes only
		// as many bytes as that object actually is.
		var defaultState = new byte[LargestState];
		var word = new byte[8];

		foreach (var region in process.Regions)
		{
			// Overlap each window by one object so a block spanning a boundary is not missed.
			for (ulong at = region.Base; at < region.End; at += (ulong) (window.Length - SweepWindow))
			{
				int length = (int) Math.Min((ulong) window.Length, region.End - at);
				if (length < SweepWindow || !process.TryRead(at, window, length)) continue;

				for (int i = 0; i + SweepWindow <= length; i += 8)
				{
					// Any namespaced name, not just Mojang's. A block defined by a behaviour pack
					// is a real block with a real BlockLegacy, and requiring "minecraft:" made the
					// sweep blind to every one of them: the server's own data driven blocks reached
					// the palette and then had no row, because nothing here would look at them.
					// Nothing is loosened by this. The hash still has to match the text, and the
					// palette filter afterwards still drops anything no state refers to.
					string name = HashedString.ReadVerified(process, window, i, heap);
					if (name is null || !IsNamespaced(name)) continue;
					hits[name] = hits.GetValueOrDefault(name) + 1;

					// The HashedString sits partway into BlockLegacy, so step back to the object.
					ulong nameAddress = at + (ulong) i;
					if (nameAddress < (ulong) MemoryLayout.NameInsideLegacy) continue;
					ulong legacy = nameAddress - (ulong) MemoryLayout.NameInsideLegacy;
					var block = ReadBlock(process, legacy, name, window, i, defaultState, word);
					if (block is null) continue;

					// A candidate that gets this far has rooted: its name hashed to its own text
					// and the state it points at points back at it. That is the block, and there is
					// nothing left to weigh. Where a name appears twice the second copy has to root
					// as well, and two objects that both root are reported rather than voted on.
					if (found.TryGetValue(block.NameHash, out var existing) && existing.Address != block.Address)
					{
						Console.Error.WriteLine($"  {name} roots at 0x{existing.Address:X} and 0x{block.Address:X}");
					}

					found[block.NameHash] = block;
				}
			}
		}
		Console.WriteLine($"  names that hash to their own text: {hits.Values.Sum():N0} hits over {hits.Count:N0} names, "
						+ $"{hits.Count(h => h.Value == 1):N0} of them hit once");
		foreach ((string name, int count) in hits.OrderByDescending(h => h.Value).Take(5))
		{
			Console.WriteLine($"      {name} hit {count:N0} times");
		}
		Console.WriteLine($"      minecraft:air hit {hits.GetValueOrDefault("minecraft:air"):N0} times");

		return found.Values.OrderBy(b => b.Name, StringComparer.Ordinal).ToList();
	}

	/// <summary>
	///     An MSVC std::string at an address, for the two fields that sit in front of the name and
	///     so are outside the window the sweep already holds.
	/// </summary>
	private static string StdString(BedrockProcess process, ulong at)
	{
		var header = new byte[32];
		if (!process.IsMapped(at) || !process.TryRead(at, header, header.Length)) return null;

		ulong length = BitConverter.ToUInt64(header, 16);
		ulong capacity = BitConverter.ToUInt64(header, 24);
		if (length == 0 || length > capacity || length > 128) return null;

		byte[] body;
		int from;
		if (capacity == 15)
		{
			body = header;
			from = 0;
		}
		else
		{
			if (capacity > 4096 || (capacity + 1) % 16 != 0) return null;
			ulong pointer = BitConverter.ToUInt64(header, 0);
			if (pointer < 0x10000 || !process.IsMapped(pointer)) return null;
			body = new byte[length];
			if (!process.TryRead(pointer, body, body.Length)) return null;
			from = 0;
		}
		for (int i = from; i < from + (int) length; i++)
		{
			if (body[i] is < 0x20 or > 0x7E) return null;
		}
		return Encoding.ASCII.GetString(body, from, (int) length);
	}

	/// <summary>
	///     A block identifier: one namespace, a colon, then the name, all lower case. Loose enough
	///     for a pack's own namespace and tight enough that ordinary text does not pass.
	/// </summary>
	private static bool IsNamespaced(string name)
	{
		int colon = name.IndexOf(':');
		if (colon < 1 || colon == name.Length - 1) return false;

		for (int i = 0; i < name.Length; i++)
		{
			char c = name[i];
			if (i == colon) continue;
			if (c is (>= 'a' and <= 'z') or (>= '0' and <= '9') or '_' or '.') continue;
			return false;
		}
		return true;
	}

	private static BlockProperties ReadBlock(BedrockProcess process, ulong legacy, string name,
		byte[] window, int at, byte[] defaultState, byte[] word)
	{
		ulong stateAddress = process.ReadUInt64(legacy + (ulong) (MemoryLayout.NameInsideLegacy + MemoryLayout.DefaultStatePointer), word);
		if (stateAddress < 0x10000 || !process.IsMapped(stateAddress)) return null;
		// The state's own class says how long it is. A class that states no size is a read that
		// failed, not an object to read a made up length from.
		int stateSize = ObjectLayout.Measure(process, stateAddress, word);
		if (stateSize <= 0 || stateSize > defaultState.Length) return null;
		if (!process.TryRead(stateAddress, defaultState, stateSize)) return null;

		// The state has to name this block back. A name can appear on more than one object, and
		// without this the sweep sometimes keeps one that is not the BlockLegacy at all: it reads,
		// it scores, and every field on it is nonsense. The round trip is the same thing the layout
		// guard checks afterwards, so requiring it here means the sweep cannot hand the guard a
		// block it will reject.
		if (BitConverter.ToUInt64(defaultState, MemoryLayout.BlockLegacyPointer) != legacy) return null;

		int nameAt = at; // the HashedString the sweep found
		int size = ObjectLayout.Measure(process, legacy, word);

		return new BlockProperties
		{
			Name = name,
			NameHash = BitConverter.ToUInt64(window, nameAt),
			LegacyId = BlockLayout.Has("id")
				? (int) (process.ReadUInt64(legacy + (ulong) BlockLayout.At("id"), word) & 0xFFFF)
				: -1,
			TintMethod = BlockLayout.Has("tintMethod")
				? MemoryLayout.Describe(MemoryLayout.TintMethods,
					(byte) (process.ReadUInt64(legacy + (ulong) BlockLayout.At("tintMethod"), word) & 0xFF))
				: null,
			SerializationId = BlockLayout.Has("descriptionId")
				? StdString(process, legacy + (ulong) BlockLayout.At("descriptionId"))
				: null,
			CreativeGroup = BlockLayout.Has("creativeGroup")
				? StdString(process, legacy + (ulong) BlockLayout.At("creativeGroup"))
				: null,
			Tags = ReadTags(process, window, nameAt, word),
			Geometry = ReadGeometry(process, legacy, word),
			Vtable = process.ReadUInt64(legacy, word),
			Address = legacy,
			ObjectSize = size,
			Unread = ObjectLayout.Holes(size),
		};
	}

	/// <summary>
	///     The block's declared shape, found by what it holds rather than by which class it is.
	///     A component's class address moves every build, so the geometry is identified by content:
	///     it is the component carrying a namespaced string, and no other component on a block does.
	/// </summary>
	private static string ReadGeometry(BedrockProcess process, ulong legacy, byte[] word)
	{
		if (!BlockLayout.Has("components")) return null;
		ulong begin = process.ReadUInt64(legacy + (ulong) BlockLayout.At("components"), word);
		ulong end = process.ReadUInt64(legacy + (ulong) (BlockLayout.At("components") + 8), word);
		if (begin < 0x10000 || end <= begin || (end - begin) % 8 != 0) return null;
		if (end - begin > MaximumComponentBytes || !process.IsMapped(begin)) return null;

		for (ulong at = begin; at < end; at += 8)
		{
			ulong component = process.ReadUInt64(at, word);
			if (component < 0x10000 || !process.IsMapped(component)) continue;

			string text = StdString(process, component + (ulong) MemoryLayout.ComponentText);
			if (text is not null && text.StartsWith(Namespace, StringComparison.Ordinal)) return text;
		}
		return null;
	}

	/// <summary>No block carries anywhere near this many components.</summary>
	private const int MaximumComponentBytes = 8 * 64;

	/// <summary>
	///     The tag vector, read the same self checking way as everything else: each element is a
	///     HashedString, so a tag is only accepted when its text hashes to the number in front of
	///     it. A block with no tags and a vector this misread look the same from here, and the hash
	///     is what makes the difference not matter.
	/// </summary>
	private static List<string> ReadTags(BedrockProcess process, byte[] window, int at, byte[] word)
	{
		var tags = new List<string>();
		// The tag vector is a member of the class like any other, so it is read from where the
		// member map puts it rather than from a second offset measured later in the run.
		if (!BlockLayout.Has("tags")) return tags;
		int tagsAt = at - MemoryLayout.NameInsideLegacy + BlockLayout.At("tags");
		if (tagsAt < 0 || tagsAt + 16 > window.Length) return tags;
		ulong begin = BitConverter.ToUInt64(window, tagsAt);
		ulong end = BitConverter.ToUInt64(window, tagsAt + 8);
		if (begin < 0x10000 || end <= begin) return tags;

		ulong span = end - begin;
		if (span % (ulong) HashedString.Size != 0 || span > (ulong) MaximumTagBytes) return tags;
		if (!process.IsMapped(begin)) return tags;

		var element = new byte[HashedString.Size];
		var heap = new byte[MemoryLayout.MaximumTagLength];
		for (ulong record = begin; record < end; record += (ulong) HashedString.Size)
		{
			if (!process.TryRead(record, element, element.Length)) continue;
			string tag = HashedString.ReadVerified(process, element, 0, heap,
				MemoryLayout.MinimumTagLength, MemoryLayout.MaximumTagLength);
			if (tag is not null) tags.Add(tag);
		}
		return tags;
	}

	/// <summary>No block carries anywhere near this many, so a longer span is not a tag vector.</summary>
	private static int MaximumTagBytes => HashedString.Size * 64;



	private const string Unknown = "Unknown";

	private static string ToHex(float[] rgba)
	{
		static int Channel(float value) => Math.Clamp((int) Math.Round(value * 255f), 0, 255);
		return $"#{Channel(rgba[0]):X2}{Channel(rgba[1]):X2}{Channel(rgba[2]):X2}{Channel(rgba[3]):X2}";
	}
}
