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

using System.Globalization;

namespace MiNET.BdsExtract;

/// <summary>One component a block carries, with its value where the type is known.</summary>
public sealed class BlockComponent
{
	/// <summary>
	///     The server's own number for this component, from the array beside the pointer vector.
	///     This is the identity. Everything else here is an attempt to put a name to it, and the
	///     number is correct whether or not the name is known.
	/// </summary>
	public int Id { get; init; }

	/// <summary>The identifier, or null when the type could not be named.</summary>
	public string Name { get; init; }

	/// <summary>The value as the component states it. Null for a component that holds no value.</summary>
	public string Value { get; init; }

	/// <summary>The object's measured size, which is what makes an unnamed component countable.</summary>
	public int Size { get; init; }

	/// <summary>
	///     The component's payload, past its method table, for a component we cannot name.
	///     A component holds the real value, not a reference to a field elsewhere: that is why the
	///     map colour lives in the component while the field beside it is zero. So an unnamed
	///     component is a value we read and then threw away, which is the one thing this tool is
	///     not allowed to do. The bytes go out as they are, and naming the id later reinterprets
	///     them without another extraction.
	/// </summary>
	public string Bytes { get; init; }
}

/// <summary>
///     The components a block actually carries, read from the vector on BlockLegacy.
///     The schema says a block may have forty nine components. Vanilla instantiates thirteen types,
///     so most of that list is modding surface no block here holds, and the census is what tells one
///     from the other.
///     Types are found the same way block classes are, by grouping instances on their method table,
///     and then named by what the group holds rather than by its address. An address moves every
///     build; a component carrying a namespaced geometry name, or a loot table path, or four floats
///     that read as a colour, carries that whatever it is compiled to.
///     A group that cannot be named is still emitted, with its size and the blocks holding it,
///     because a component dropped for being unrecognised makes the block look like it has fewer
///     than it does.
/// </summary>
public static class ComponentReader
{
	private const int VectorReach = 4096;
	private const int MaximumSize = 512;
	private const string Namespace = "minecraft:";
	private const string LootPrefix = "loot_tables/";
	private const string LootDefault = "normal";

	/// <summary>A model reference, which the visual component uses and geometry does not.</summary>
	private const string ModelPrefix = "minecraft:geometry.";

	/// <summary>Where a component keeps its first value, past the method table.</summary>
	private const int FirstField = 8;

	/// <summary>Geometry states its name one field further in than every other string component.</summary>
	private const int GeometryText = 16;

	/// <summary>
	///     What each component id is, worked out from every instance carrying it. A registration
	///     number is per instance and two runs of one build disagree on every one of them, so the
	///     number is never published: the name it stands for is.
	/// </summary>
	public static IReadOnlyDictionary<int, string> Names { get; private set; } = new Dictionary<int, string>();

	public static Dictionary<string, List<BlockComponent>> Read(BedrockProcess process,
		IReadOnlyList<BlockProperties> blocks)
	{
		var word = new byte[8];
		var byBlock = new Dictionary<string, List<(int Id, ulong At)>>(StringComparer.Ordinal);
		var byId = new Dictionary<int, List<(string Block, ulong At)>>();

		foreach (var block in blocks)
		{
			var held = Instances(process, block.Address + (ulong) BlockLayout.At("components"), word);
			byBlock[block.Name] = held;
			foreach (var (id, at) in held)
			{
				if (!byId.TryGetValue(id, out var list)) byId[id] = list = [];
				list.Add((block.Name, at));
			}
		}

		// One name per id, decided from every instance carrying it. Grouping on the method table
		// instead put two components in one bucket, because one class serves several of them: the
		// id separates what the class does not.
		var named = new Dictionary<int, string>();
		var sized = new Dictionary<int, int>();
		foreach (var (id, instances) in byId)
		{
			sized[id] = Size(process, instances[0].At, word);
			named[id] = Identify(process, instances, blocks, word);
		}

		// The same ids answer on a state, because both come out of one registry, so the names
		// worked out here are what lets a state say what it carries rather than a bare number.
		Names = named;

		var result = new Dictionary<string, List<BlockComponent>>(StringComparer.Ordinal);
		foreach (var block in blocks)
		{
			var list = new List<BlockComponent>();
			foreach (var (id, at) in byBlock[block.Name])
			{
				string name = named.GetValueOrDefault(id);
				int size = sized.GetValueOrDefault(id);
				list.Add(new BlockComponent
				{
					Id = id,
					Name = name,
					Value = name is null ? null : ValueOf(process, name, at, word),
					Size = size,
					Bytes = name is null ? Payload(process, at, size) : null
				});
			}
			result[block.Name] = list;
		}
		return result;
	}

	/// <summary>
	///     The components on one block, each with the id the server files it under.
	///     Two vectors run in step: pointers at <see cref="MemoryLayout.BlockComponents" /> and one
	///     sixteen bit id each at <see cref="MemoryLayout.BlockComponentIds" />. They are required
	///     to be the same length, which is what says they are the same list, and a block where they
	///     disagree is skipped rather than paired up by position and hoped for.
	/// </summary>
	internal static List<(int Id, ulong At)> Instances(BedrockProcess process, ulong storage, byte[] word)
	{
		var held = new List<(int, ulong)>();

		// The keys are first and the values second, which is what the flat_map declares. The ids
		// are two bytes each and the pointers eight, and requiring both counts to agree is what
		// says the two vectors are the same list.
		ulong idBegin = process.ReadUInt64(storage, word);
		ulong idEnd = process.ReadUInt64(storage + 8, word);
		ulong begin = process.ReadUInt64(storage + 24, word);
		ulong end = process.ReadUInt64(storage + 32, word);
		if (begin < 0x10000 || end <= begin || (end - begin) % 8 != 0) return held;
		if (end - begin > VectorReach || !process.IsMapped(begin)) return held;

		int count = (int) ((end - begin) / 8);
		if (idBegin < 0x10000 || idEnd - idBegin != (ulong) (count * 2)) return held;
		if (!process.IsMapped(idBegin)) return held;

		var ids = new byte[count * 2];
		if (!process.TryRead(idBegin, ids, ids.Length)) return held;

		for (int i = 0; i < count; i++)
		{
			ulong component = process.ReadUInt64(begin + (ulong) (i * 8), word);
			if (component < 0x10000 || !process.IsMapped(component)) continue;
			held.Add((BitConverter.ToUInt16(ids, i * 2), component));
		}
		return held;
	}

	/// <summary>
	///     What a group of instances is, from what they hold. Each test is something the other
	///     twelve types fail, and a test that only most of a group passes names nothing: a
	///     component type holds one kind of value in every instance or it is not one type.
	/// </summary>
	private static string Identify(BedrockProcess process,
		List<(string Block, ulong At)> instances,
		IReadOnlyList<BlockProperties> blocks,
		byte[] word)
	{
		var sample = instances.Take(SampleSize).ToList();

		// A test on shape or on a matching number proves nothing about a handful of instances: a
		// single float is symmetric about zero, or equal to that one block's hardness, far too
		// easily. Running a pack of one-block-per-component through this named five different ids
		// "random_offset" at once. Only tests that read a distinctive STRING are allowed below the
		// bar, because a component holding "loot_tables/blocks/x.json" is not a coincidence at any
		// size.
		bool weighty = instances.Count >= MinimumEvidence;

		// Geometry names itself. The visual component also carries a namespaced string here, so
		// the model references it uses are excluded rather than folded in: minecraft:unit_cube is
		// a shape, minecraft:geometry.cross is a model, and they are not the same component.
		if (sample.All(i => StdString(process, i.At + GeometryText, word) is { } shape
							&& shape.StartsWith(Namespace, StringComparison.Ordinal)
							&& !shape.StartsWith(ModelPrefix, StringComparison.Ordinal)))
		{
			return "minecraft:geometry";
		}

		// Loot names a table in the packs. The id holding the literal "normal" on every block is a
		// different component and is deliberately not matched here: one string test covering both
		// put 1,429 instances under the wrong name.
		if (sample.All(i => StdString(process, i.At + FirstField, word)?.StartsWith(LootPrefix,
				StringComparison.Ordinal) == true))
		{
			return "minecraft:loot";
		}

		// Break particles, whose texture is optional: most blocks take their own, and the ones that
		// override name a texture outright. lever_particle and straw_bed_particle say what the
		// field is; grass_bottom and big_oak_leaves say it is the break texture rather than the
		// block's. The rule is that no instance holds anything but a plain texture name, and at
		// least one holds one, which a component with no string at all cannot pass.
		// The texture is OPTIONAL, and that is the test. A component where every instance carries
		// the same literal is a different component that merely also holds a plain word, which is
		// how the id holding "normal" everywhere ended up under this name once already.
		var textures = sample
			.Select(i => StdString(process, i.At + FirstField, word))
			.ToList();
		if (weighty && textures.Any(t => t is not null) && textures.Any(t => t is null)
			&& textures.All(t => t is null || (t.Length > 2 && !t.Contains(':') && !t.Contains('/'))))
		{
			return "minecraft:destruction_particles";
		}

		// Four floats reading as a colour, the fourth being opaque alpha.
		if (weighty && sample.All(i => Floats(process, i.At + FirstField, 4, word) is { } rgba
							&& rgba.All(v => v is >= 0f and <= 1f) && Math.Abs(rgba[3] - 1f) < 1e-6))
		{
			return "minecraft:map_color";
		}

		// A symmetric range about zero, which is what an offset jitter is.
		if (weighty && sample.All(i => Floats(process, i.At + FirstField, 2, word) is { } range
							&& range[0] < 0f && Math.Abs(range[0] + range[1]) < 1e-6))
		{
			return "minecraft:random_offset";
		}

		// Four more components were named here by matching their contents against the block's
		// hardness, its liquid pair, its flame odds and its blast resistance. Those four values live
		// on the state object, which has no measured layout on this build, so the comparison has no
		// left hand side and the names it produced are not produced any more:
		// destructible_by_mining, liquid_detection, flammable, destructible_by_explosion.

		return null;
	}

	/// <summary>Blast resistance is stated five times over in the explosion component.</summary>
	private const float ResistanceScale = 5f;

	private static string ValueOf(BedrockProcess process, string name, ulong at, byte[] word)
	{
		switch (name)
		{
			case "minecraft:geometry":
				return StdString(process, at + GeometryText, word);

			case "minecraft:loot":
			case "minecraft:destruction_particles":
				return StdString(process, at + FirstField, word);

			case "minecraft:map_color":
			{
				// Stated the same way the block's own colour field is. The floats are exactly
				// eight bit values over 255, all 5,716 channels of them, so hex is the same number
				// in a shape a reader recognises rather than a conversion that loses anything.
				// A channel outside that range would not be, so it falls back to the raw floats.
				var rgba = Floats(process, at + FirstField, 4, word);
				if (rgba is null) return null;
				if (rgba.Any(v => v is < 0f or > 1f)) return string.Join(", ", rgba.Select(Text));

				var hex = new System.Text.StringBuilder("#");
				foreach (float channel in rgba)
				{
					hex.Append(((int) Math.Round(channel * 255f)).ToString("X2", CultureInfo.InvariantCulture));
				}
				return hex.ToString();
			}

			case "minecraft:random_offset":
			{
				var range = Floats(process, at + FirstField, 2, word);
				return range is null ? null : $"{Text(range[0])} to {Text(range[1])}";
			}

			case "minecraft:destructible_by_mining":
			{
				var one = Floats(process, at + FirstField, 1, word);
				return one is null ? null : Text(one[0]);
			}

			case "minecraft:liquid_detection":
			{
				var raw = Bytes(process, at + FirstField, 3, word);
				if (raw is null || raw[2] >= MemoryLayout.LiquidReactions.Length) return null;
				return $"canContain {(raw[0] != 0 ? "true" : "false")}, "
					+ $"onTouch {MemoryLayout.LiquidReactions[raw[2]]}";
			}

			case "minecraft:flammable":
			{
				var odds = Shorts(process, at + FlammableOdds, 2, word);
				return odds is null ? null : $"flame {odds[0]}, burn {odds[1]}";
			}

			case "minecraft:destructible_by_explosion":
			{
				// As read. The component holds the authored value and the state object holds that
				// divided by five, and both are in the output, so a reader can see the relation
				// without this having decided which of the two to show them.
				var one = Floats(process, at + FirstField, 1, word);
				return one is null ? null : Text(one[0]);
			}

			default:
				return null;
		}
	}

	/// <summary>How many instances of a group are asked. Enough that a wrong test cannot pass.</summary>
	private const int SampleSize = 64;

	/// <summary>How many carriers a numeric or shape test needs before its agreement means anything.</summary>
	private const int MinimumEvidence = 10;

	/// <summary>Where the flammable component keeps its two odds, past a leading flag.</summary>
	private const int FlammableOdds = 10;

	private static byte[] Bytes(BedrockProcess process, ulong at, int count, byte[] word)
	{
		var raw = new byte[count];
		return process.TryRead(at, raw, count) ? raw : null;
	}

	private static int[] Shorts(BedrockProcess process, ulong at, int count, byte[] word)
	{
		var raw = new byte[count * 2];
		if (!process.TryRead(at, raw, raw.Length)) return null;
		var values = new int[count];
		for (int i = 0; i < count; i++) values[i] = BitConverter.ToUInt16(raw, i * 2);
		return values;
	}

	private static float[] Floats(BedrockProcess process, ulong at, int count, byte[] word)
	{
		var raw = new byte[count * 4];
		if (!process.TryRead(at, raw, raw.Length)) return null;
		var values = new float[count];
		for (int i = 0; i < count; i++)
		{
			values[i] = BitConverter.ToSingle(raw, i * 4);
			if (!float.IsFinite(values[i])) return null;
		}
		return values;
	}

	/// <summary>
	///     Everything the component holds after its method table, as hex.
	///     Emitted rather than interpreted: a component whose meaning is unknown still has a value,
	///     and guessing at the value's type is how a float field that is not a float ends up in the
	///     output reading as 1.8e-40.
	/// </summary>
	private static string Payload(BedrockProcess process, ulong at, int size)
	{
		int length = size - FirstField;
		if (length is <= 0 or > MaximumSize) return null;

		var body = new byte[length];
		return process.TryRead(at + FirstField, body, length) ? Convert.ToHexString(body) : null;
	}

	/// <summary>The allocator's size for the object, so an unnamed component is still countable.</summary>
	private static int Size(BedrockProcess process, ulong at, byte[] word)
	{
		for (int offset = 8; offset < MaximumSize; offset += 8)
		{
			if (process.ReadUInt64(at + (ulong) offset, word) >> 56 is 0x88 or 0x90) return offset;
		}
		return 0;
	}

	/// <summary>An MSVC std::string, which is short inside the object and long behind a pointer.</summary>
	private static string StdString(BedrockProcess process, ulong at, byte[] word)
	{
		var head = new byte[32];
		if (!process.TryRead(at, head, head.Length)) return null;

		ulong length = BitConverter.ToUInt64(head, 16);
		ulong capacity = BitConverter.ToUInt64(head, 24);
		if (length == 0 || length > 512 || capacity < length) return null;

		byte[] body;
		if (capacity == 15)
		{
			if (length > 15) return null;
			body = head;
		}
		else
		{
			ulong pointer = BitConverter.ToUInt64(head, 0);
			if (pointer < 0x10000 || !process.IsMapped(pointer)) return null;
			body = new byte[length];
			if (!process.TryRead(pointer, body, body.Length)) return null;
		}

		for (int i = 0; i < (int) length; i++)
		{
			if (body[i] is < 0x20 or > 0x7E) return null;
		}
		return System.Text.Encoding.ASCII.GetString(body, 0, (int) length);
	}

	private static string Text(float value)
	{
		return value.ToString("0.######", CultureInfo.InvariantCulture);
	}
}
