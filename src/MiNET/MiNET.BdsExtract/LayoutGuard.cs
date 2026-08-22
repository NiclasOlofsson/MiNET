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

namespace MiNET.BdsExtract;

/// <summary>One offset's verdict: what it guards, whether it held, and what was counted.</summary>
public sealed class Sentinel
{
	public string Name { get; init; }

	/// <summary>The <see cref="MemoryLayout" /> constants this check stands in front of.</summary>
	public string Guards { get; init; }

	public bool Held { get; init; }

	/// <summary>What was measured, as agreed out of attempted, so a partial failure is visible.</summary>
	public string Detail { get; init; }

	/// <summary>
	///     A few of the blocks that failed, because a count is not actionable on its own. "1,452 of
	///     1,454" says something is wrong and nothing about where to look, and the two that failed
	///     are exactly what a reader needs to see.
	/// </summary>
	public IReadOnlyList<string> Examples { get; init; } = [];
}

/// <summary>
///     Proves the field offsets against the server before anything is written.
///     A moved field is the failure this tool cannot survive quietly. The read still returns a
///     number, the number still has the right type, and the row it lands in looks exactly like the
///     rows that are correct. Nothing downstream can tell the difference, so the difference has to
///     be caught here or not at all.
///     Every structural check below is all or nothing across the whole registry, never a count and
///     never a threshold. Counts move on their own when a version adds blocks, so a guard built on
///     them cries wolf on every bump and gets ignored, which is worse than no guard. An invariant
///     that holds for 1,429 blocks today holds for 1,600 tomorrow, and the only thing that breaks it
///     is the thing this is looking for.
///     Known values are the one exception, and they are a different kind of check: they compare this
///     build against a frozen extraction of another one, so a value the game itself changed reads as
///     a difference. That check holds a field at the same nine tenths the locate accepted its
///     position at, because the two run off one reference and the locate runs first. Every
///     difference is still listed and counted; what relaxes is the verdict, never the reporting.
///     What each check rests on is that arbitrary bytes cannot pass it. A pointer that round trips
///     back to the object it came from, a name that hashes to its own hash, two structures built
///     for different purposes agreeing block for block: none of those happen by accident at a
///     wrong offset.
/// </summary>
public static class LayoutGuard
{
	/// <summary>Light is four bits, so both fields are 0 to 15 whatever the block.</summary>
	private const int MaximumLight = 15;

	public static List<Sentinel> Check(BedrockProcess process,
		IReadOnlyList<BlockProperties> blocks,
		IReadOnlyList<PaletteEntry> palette)
	{
		return
		[
			DefaultStateRoundTrip(process, blocks),
			MethodTable(process, blocks),
			PropertyRegistry(process, blocks, palette),
			LegacyDataTable(process, blocks),
			SerializationIds(blocks),
			LightInRange(palette)
		];
	}

	/// <summary>
	///     The block points at its default state and the state points back. Two offsets in
	///     different objects have to be right at once for that to close, and a wrong one lands on
	///     an unrelated pointer that has no reason to name this block.
	/// </summary>
	private static Sentinel DefaultStateRoundTrip(BedrockProcess process, IReadOnlyList<BlockProperties> blocks)
	{
		var word = new byte[8];
		var failed = new List<string>();
		int agreed = 0;
		foreach (var block in blocks)
		{
			ulong state = process.ReadUInt64(
				block.Address + (ulong) (MemoryLayout.NameInsideLegacy + MemoryLayout.DefaultStatePointer), word);
			if (state >= 0x10000 && process.IsMapped(state)
				&& process.ReadUInt64(state + (ulong) MemoryLayout.BlockLegacyPointer, word) == block.Address)
			{
				agreed++;
			}
			else
			{
				failed.Add(block.Name);
			}
		}
		return Verdict("default state round trip", "NameInsideLegacy, DefaultStatePointer, BlockLegacyPointer",
			agreed, blocks.Count, "blocks point at a state that points back", failed);
	}

	/// <summary>
	///     The first field of a block is its method table, and a method table is mapped and holds
	///     mapped code pointers. This one guards the object start itself: get that wrong and the
	///     name is found at the wrong distance and every field measured from it moves together.
	/// </summary>
	private static Sentinel MethodTable(BedrockProcess process, IReadOnlyList<BlockProperties> blocks)
	{
		var word = new byte[8];
		int agreed = 0;
		foreach (var block in blocks)
		{
			ulong vtable = process.ReadUInt64(block.Address, word);
			if (vtable < 0x10000 || !process.IsMapped(vtable)) continue;
			ulong first = process.ReadUInt64(vtable, word);
			if (first > 0x10000 && process.IsMapped(first)) agreed++;
		}
		return Verdict("method table", "the object start, so every field measured from it",
			agreed, blocks.Count, "blocks whose method table holds code");
	}

	/// <summary>
	///     The strongest check here, and the only one that proves meaning rather than shape. The
	///     block keeps a table of its own state properties; the palette holds the states those
	///     properties produce. Nothing links the two, they are built for different purposes, and
	///     they agree on the property names of every block. A wrong offset cannot reproduce the
	///     palette's names, because it is not reading names.
	/// </summary>
	private static Sentinel PropertyRegistry(BedrockProcess process,
		IReadOnlyList<BlockProperties> blocks,
		IReadOnlyList<PaletteEntry> palette)
	{
		var word = new byte[8];
		var fromPalette = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
		foreach (var entry in palette)
		{
			if (!fromPalette.TryGetValue(entry.Name, out var set))
				fromPalette[entry.Name] = set = new HashSet<string>(StringComparer.Ordinal);
			foreach (var property in entry.States) set.Add(property.Name);
		}

		// Every block is asked, including the ones with no properties: a wrong offset that invents
		// a property where the palette has none has to fail too.
		int agreed = 0, carrying = 0, read = 0;
		var failed = new List<string>();
		foreach (var block in blocks)
		{
			if (!fromPalette.TryGetValue(block.Name, out var expected)) continue;
			if (expected.Count > 0) carrying++;
			var held = StateProperties.NamesOf(process, block, word);
			if (held.Count > 0) read++;
			if (held.Count == expected.Count && held.All(expected.Contains)) agreed++;
			else failed.Add($"{block.Name} (registry: [{string.Join(",", held)}] palette: [{string.Join(",", expected)}])");
		}

		// Empty matching empty is not agreement, it is two things that were not read. Both sides
		// have to carry properties before their agreeing means anything, so a run where either
		// side comes out empty fails here rather than passing on nothing.
		if (carrying == 0 || read == 0)
		{
			return new Sentinel
			{
				Name = "property registry",
				Guards = "BlockProperties",
				Held = false,
				Detail = $"nothing to compare: {carrying:N0} blocks carry properties in the palette and "
						+ $"{read:N0} carry any in their own table, so agreement would be empty matching empty"
			};
		}

		return Verdict("property registry", "BlockProperties",
			agreed, blocks.Count, $"blocks match the palette, {carrying:N0} of them carrying properties", failed);
	}

	/// <summary>
	///     The table of states by old data value holds this block's states, and each one names its
	///     block. An empty slot is legitimate, a slot naming a different block is not, so only the
	///     filled ones are asked and a table with none of them is a failure.
	/// </summary>
	private static Sentinel LegacyDataTable(BedrockProcess process, IReadOnlyList<BlockProperties> blocks)
	{
		var word = new byte[8];
		var failed = new List<string>();
		int agreed = 0;
		foreach (var block in blocks)
		{
			ulong begin = process.ReadUInt64(block.Address + (ulong) MemoryLayout.BlockStates, word);
			ulong end = process.ReadUInt64(block.Address + (ulong) MemoryLayout.BlockStates + 8, word);
			if (begin < 0x10000 || end <= begin || (end - begin) % 8 != 0 || !process.IsMapped(begin))
			{
				failed.Add(block.Name);
				continue;
			}

			bool wrong = false;
			int filled = 0;
			for (ulong at = begin; at < end; at += 8)
			{
				ulong state = process.ReadUInt64(at, word);
				if (state < 0x10000 || !process.IsMapped(state)) continue;
				if (process.ReadUInt64(state + (ulong) MemoryLayout.BlockLegacyPointer, word) == block.Address) filled++;
				else wrong = true;
			}
			if (filled > 0 && !wrong) agreed++;
			else failed.Add(block.Name);
		}
		return Verdict("legacy data table", "BlockStates",
			agreed, blocks.Count, "blocks whose data table holds only their own states", failed);
	}

	/// <summary>
	///     Every block has a pre flattening name, so an offset that produces none for some block is
	///     not pointing at that string. It is read as a std::string, which a wrong offset fails at
	///     rather than returning a short plausible word.
	/// </summary>
	private static Sentinel SerializationIds(IReadOnlyList<BlockProperties> blocks)
	{
		int agreed = blocks.Count(b => !string.IsNullOrEmpty(b.SerializationId));
		var failed = blocks.Where(b => string.IsNullOrEmpty(b.SerializationId)).Select(b => b.Name).ToList();
		return Verdict("serialization id", "SerializationId",
			agreed, blocks.Count, "blocks carry a pre flattening name", failed);
	}
	/// <summary>
	///     Light is four bits. Both fields are read as bytes, so a wrong offset lands on something
	///     that is not four bits wide and runs past fifteen almost at once.
	/// </summary>
	private static Sentinel LightInRange(IReadOnlyList<PaletteEntry> palette)
	{
		int agreed = palette.Count(e => e.LightEmission is >= 0 and <= MaximumLight
									&& e.LightDampening is >= 0 and <= MaximumLight);
		return Verdict("light range", "StateLightEmission, StateLightDampening",
			agreed, palette.Count, "states light both within four bits");
	}


	private static Sentinel Verdict(string name, string guards, int agreed, int of, string what,
		List<string> failed = null)
	{
		return new Sentinel
		{
			Name = name,
			Guards = guards,
			Held = of > 0 && agreed == of,
			Detail = $"{agreed:N0} of {of:N0} {what}",
			Examples = failed is null ? [] : failed.Take(NamedFailures).ToList()
		};
	}

	/// <summary>How many failing blocks to name. Enough to see a pattern, not a wall of text.</summary>
	private const int NamedFailures = 6;

	/// <summary>
	///     The sentinels, and a refusal that cannot be read as a warning. A build that moved a
	///     field produces output that looks entirely normal, so the only useful failure is one that
	///     stops the run and says which offset to go and find again.
	/// </summary>
	public static bool Report(IReadOnlyList<Sentinel> sentinels, TextWriter output)
	{
		foreach (var sentinel in sentinels)
		{
			output.WriteLine($"  {(sentinel.Held ? "held  " : "BROKEN")} {sentinel.Name,-24} {sentinel.Detail}");
		}

		var broken = sentinels.Where(s => !s.Held).ToList();
		if (broken.Count == 0) return true;

		output.Flush();
		var error = Console.Error;
		error.WriteLine();
		error.WriteLine("################################################################################");
		error.WriteLine("##                                                                            ##");
		error.WriteLine("##   LAYOUT GUARD FAILED. THE OFFSETS DO NOT MATCH THIS SERVER BUILD.          ##");
		error.WriteLine("##   NOTHING HAS BEEN WRITTEN. THE OUTPUT WOULD HAVE LOOKED CORRECT.           ##");
		error.WriteLine("##                                                                            ##");
		error.WriteLine("################################################################################");
		error.WriteLine();
		foreach (var sentinel in broken)
		{
			error.WriteLine($"  {sentinel.Name}");
			error.WriteLine($"      guards  {sentinel.Guards}");
			error.WriteLine($"      saw     {sentinel.Detail}");
			if (sentinel.Examples.Count > 0)
			{
				error.WriteLine($"      blocks  {string.Join(", ", sentinel.Examples)}");
			}
		}
		error.WriteLine();
		error.WriteLine("  Re-derive those constants against this build. Do not adjust them until the");
		error.WriteLine("  numbers look reasonable: reasonable is what a wrong offset already produces.");
		error.WriteLine();
		return false;
	}
}
