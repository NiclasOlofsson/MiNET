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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iced.Intel;

namespace MiNET.BdsExtract.Binary;

/// <summary>
///     One enum's value to name table as the code states it, the function it was read from, and
///     which shape matched. <see cref="Accepted" /> is the criterion's answer: the table reproduces
///     every seed name at the value the seed states for it. A rejected candidate keeps whatever it
///     yielded and says in <see cref="Evidence" /> where it disagreed, so a wrong shape is visible
///     rather than absent.
/// </summary>
public sealed record EnumTable(string Name, IReadOnlyDictionary<int, string> Values, uint FunctionRva, string Evidence, bool Accepted);

/// <summary>
///     What is known about an enum before the binary is read: its name, and the values the
///     reference already states for it. The names find the function; the values are the criterion
///     a candidate table has to reproduce.
/// </summary>
public sealed record EnumSeed(string Name, IReadOnlyDictionary<int, string> Known);

/// <summary>
///     Reads value to name tables for enums out of the code. Three shapes are read, all anchored on
///     the name literals the seed supplies:
///     <list type="number">
///         <item>a jump table: a bounds check, a table of 32-bit offsets in .rdata, one case block per value, each loading its name;</item>
///         <item>a compare chain: <c>cmp reg, imm</c> and a branch per value, each case block loading its name;</item>
///         <item>a registration chain: the reflection binding that names the enum and then constructs one {pointer, length} string view per value in declaration order, each handed to a per-value function whose immediate store is the value.</item>
///     </list>
///     A table is accepted only when it reproduces every seed name at the value the seed states for
///     it. That criterion is what makes a wrong shape fail loudly instead of inventing a mapping,
///     so it is checked for every candidate and no table is emitted as accepted without it.
/// </summary>
public static class EnumTables
{
	/// <summary>
	///     The plan's seed form: an enum name and its known names in value order. Position is the
	///     value, which is only true of a contiguous enum starting at zero; an enum whose reference
	///     table is sparse or starts below zero is seeded through <see cref="EnumSeed" /> instead.
	/// </summary>
	public static IReadOnlyList<EnumTable> Find(CodeIndex code, PeImage image, IReadOnlyList<(string Name, IReadOnlyList<string> KnownNames)> seeds)
	{
		var expanded = new List<EnumSeed>();
		foreach ((string name, IReadOnlyList<string> known) in seeds)
		{
			var values = new Dictionary<int, string>();
			for (int i = 0; i < known.Count; i++) values[i] = known[i];
			expanded.Add(new EnumSeed(name, values));
		}
		return Find(code, image, expanded);
	}

	public static IReadOnlyList<EnumTable> Find(CodeIndex code, PeImage image, IReadOnlyList<EnumSeed> seeds)
	{
		var names = new List<string>();
		foreach (EnumSeed seed in seeds)
		{
			names.Add(seed.Name);
			names.AddRange(seed.Known.Values);
		}
		var literals = new DataLiterals(image, names);

		var result = new List<EnumTable>();
		foreach (EnumSeed seed in seeds) result.Add(FindOne(code, image, literals, seed));
		return result;
	}

	private static EnumTable FindOne(CodeIndex code, PeImage image, DataLiterals literals, EnumSeed seed)
	{
		var wanted = seed.Known.Values.Distinct(StringComparer.Ordinal).ToList();
		var coverage = new Dictionary<uint, HashSet<string>>();
		foreach (string name in wanted)
		foreach (uint function in FunctionsReferencing(code, literals, name))
		{
			if (!coverage.TryGetValue(function, out HashSet<string> set)) coverage[function] = set = new HashSet<string>(StringComparer.Ordinal);
			set.Add(name);
		}
		var namesTheEnum = FunctionsReferencing(code, literals, seed.Name).ToHashSet();

		List<uint> candidates = coverage
			.Where(c => c.Value.Count == wanted.Count)
			.Select(c => c.Key)
			.OrderByDescending(f => namesTheEnum.Contains(f))
			.ThenBy(f => f)
			.ToList();

		if (candidates.Count == 0)
		{
			int best = coverage.Count == 0 ? 0 : coverage.Values.Max(v => v.Count);
			return new EnumTable(seed.Name, new Dictionary<int, string>(), 0,
				$"no function references all {wanted.Count} seed names; the best covers {best}", false);
		}

		var rejected = new List<string>();
		foreach (uint begin in candidates)
		{
			if (!code.TryFunctionOf(begin, out PeFunction function)) continue;
			foreach ((IReadOnlyDictionary<int, string> values, string shape) in Shapes(code, image, function))
			{
				string disagreement = Disagreement(values, seed.Known);
				if (disagreement == null)
				{
					string named = namesTheEnum.Contains(begin) ? "; the function names the enum" : "";
					return new EnumTable(seed.Name, values, begin, $"{shape} at 0x{begin:x}, {values.Count} values, every seed name at its stated value{named}", true);
				}
				rejected.Add($"{shape} at 0x{begin:x}: {disagreement}");
			}
		}

		return new EnumTable(seed.Name, new Dictionary<int, string>(), candidates[0],
			rejected.Count == 0
				? $"{candidates.Count} candidate functions reference all seed names, no shape read a table from any of them"
				: $"{candidates.Count} candidate functions, every table rejected: {string.Join(" | ", rejected.Take(6))}", false);
	}

	private static IEnumerable<(IReadOnlyDictionary<int, string> Values, string Shape)> Shapes(CodeIndex code, PeImage image, PeFunction function)
	{
		foreach ((IReadOnlyDictionary<int, string> values, string shape) in ReadJumpTables(code, image, function)) yield return (values, shape);
		IReadOnlyDictionary<int, string> chain = ReadCompareChain(code, image, function);
		if (chain != null) yield return (chain, "compare chain");
		foreach ((IReadOnlyDictionary<int, string> values, string shape) in ReadRegistrationChain(code, image, function)) yield return (values, shape);
	}

	/// <summary>Names the first seed pair the table gets wrong, or null when it reproduces every one of them.</summary>
	private static string Disagreement(IReadOnlyDictionary<int, string> values, IReadOnlyDictionary<int, string> known)
	{
		if (values.Count == 0) return "the table is empty";
		foreach ((int value, string name) in known.OrderBy(k => k.Key))
		{
			if (!values.TryGetValue(value, out string found)) return $"{value} has no name, the reference states {name}";
			if (!string.Equals(found, name, StringComparison.Ordinal)) return $"{value} is {found}, the reference states {name}";
		}
		return null;
	}

	private static IEnumerable<uint> FunctionsReferencing(CodeIndex code, DataLiterals literals, string text)
	{
		var functions = new HashSet<uint>();
		foreach (uint rva in literals.Occurrences(text))
		foreach (uint site in code.ReferencesTo(rva))
		{
			if (code.TryFunctionOf(site, out PeFunction function)) functions.Add(function.Begin);
		}
		return functions;
	}

	// ---- shape 1: jump table -------------------------------------------------------------------

	private static IEnumerable<(IReadOnlyDictionary<int, string> Values, string Shape)> ReadJumpTables(CodeIndex code, PeImage image, PeFunction function)
	{
		foreach (Instruction ins in code.Decode(function))
		{
			if (ins.Mnemonic != Mnemonic.Lea || !ins.IsIPRelativeMemoryOperand) continue;
			uint table = (uint) (ins.IPRelativeMemoryAddress - code.ImageBase);
			if (table >= function.Begin && table < function.End) continue;
			foreach (bool relativeToTable in new[] { true, false })
			{
				var values = new Dictionary<int, string>();
				for (int i = 0; i < 512; i++)
				{
					int entry;
					try
					{
						entry = BitConverter.ToInt32(image.Bytes(table + (uint) (i * 4), 4));
					}
					catch (ArgumentOutOfRangeException)
					{
						break;
					}
					long target = (relativeToTable ? table : function.Begin) + (long) entry;
					if (target < function.Begin || target >= function.End) break;
					string name = FirstNameAt(code, image, (uint) target, function, 16);
					if (name == null) break;
					values[i] = name;
				}
				if (values.Count >= 2) yield return (values, $"jump table at 0x{table:x}{(relativeToTable ? "" : ", entries off the function")}");
			}
		}
	}

	// ---- shape 2: compare chain ----------------------------------------------------------------

	private static IReadOnlyDictionary<int, string> ReadCompareChain(CodeIndex code, PeImage image, PeFunction function)
	{
		var values = new Dictionary<int, string>();
		int pending = int.MinValue;
		int since = 0;
		foreach (Instruction ins in code.Decode(function))
		{
			if (ins.Mnemonic == Mnemonic.Cmp && ins.Op0Kind == OpKind.Register && IsImmediate(ins.GetOpKind(1)))
			{
				pending = (int) ins.GetImmediate(1);
				since = 0;
				continue;
			}
			if (pending != int.MinValue && ins.Mnemonic is Mnemonic.Je or Mnemonic.Jne && ins.Op0Kind == OpKind.NearBranch64)
			{
				uint target = (uint) (ins.NearBranch64 - code.ImageBase);
				if (target >= function.Begin && target < function.End)
				{
					string name = FirstNameAt(code, image, target, function, 8);
					if (name != null && !values.ContainsKey(pending)) values[pending] = name;
				}
				pending = int.MinValue;
				continue;
			}
			if (pending != int.MinValue && ++since > 3) pending = int.MinValue;
		}
		return values.Count >= 2 ? values : null;
	}

	private static bool IsImmediate(OpKind kind)
	{
		return kind is OpKind.Immediate8 or OpKind.Immediate16 or OpKind.Immediate32 or OpKind.Immediate64
			or OpKind.Immediate8to16 or OpKind.Immediate8to32 or OpKind.Immediate8to64 or OpKind.Immediate32to64;
	}

	private static string FirstNameAt(CodeIndex code, PeImage image, uint rva, PeFunction function, int window)
	{
		if (!code.TryFunctionOf(rva, out PeFunction owner) || owner.Begin != function.Begin) return null;
		int seen = 0;
		foreach (Instruction ins in code.Decode(function))
		{
			if (ins.IP - code.ImageBase < rva) continue;
			if (++seen > window) break;
			if (ins.Mnemonic != Mnemonic.Lea || !ins.IsIPRelativeMemoryOperand) continue;
			string text = AsciiRun(image, (uint) (ins.IPRelativeMemoryAddress - code.ImageBase));
			if (text != null) return text;
		}
		return null;
	}

	// ---- shape 3: the reflection registration chain ---------------------------------------------

	private sealed record ChainEntry(string Text, uint CallTarget);

	/// <summary>
	///     The binding chain: <c>lea reg, [rip+text]</c> stored next to <c>mov qword [..], length</c>
	///     is a string view, and the pair proves itself because the length has to be the length of
	///     the text it points at. The call after each pair is the per-value registration, whose one
	///     function pointer that no other entry shares carries the value as an immediate store.
	/// </summary>
	private static IEnumerable<(IReadOnlyDictionary<int, string> Values, string Shape)> ReadRegistrationChain(CodeIndex code, PeImage image, PeFunction function)
	{
		List<ChainEntry> entries = ReadChainEntries(code, image, function);
		if (entries.Count < 2) yield break;

		// One function registers more than one thing: the enum's values and then the fields of the
		// class holding it, in one chain. Entries handed to the same registration call belong
		// together, so a run of them is read on its own before the chain as a whole, and a chain of
		// per-value instantiations (every call a different one) has no such run and is read whole.
		foreach (List<ChainEntry> group in Groups(entries))
		foreach ((IReadOnlyDictionary<int, string> values, string shape) in GroupTables(code, image, group, function, entries.Count))
			yield return (values, shape);
	}

	/// <summary>Every run of two or more consecutive entries sharing a registration call, then the whole chain.</summary>
	private static IEnumerable<List<ChainEntry>> Groups(List<ChainEntry> entries)
	{
		int start = 0;
		for (int i = 1; i <= entries.Count; i++)
		{
			if (i < entries.Count && entries[i].CallTarget == entries[start].CallTarget) continue;
			if (i - start >= 2 && i - start < entries.Count) yield return entries.GetRange(start, i - start);
			start = i;
		}
		yield return entries;
	}

	private static IEnumerable<(IReadOnlyDictionary<int, string> Values, string Shape)> GroupTables(CodeIndex code, PeImage image, List<ChainEntry> group, PeFunction function, int chainLength)
	{
		string where = group.Count == chainLength ? "the whole chain" : $"the {group.Count} entries sharing one registration call";
		foreach ((IReadOnlyDictionary<int, string> values, string shape) in ChainValuesFromThunks(code, image, group)) yield return (values, $"{shape}, {where}");

		var ordinal = new Dictionary<int, string>();
		for (int i = 0; i < group.Count; i++) ordinal[i] = group[i].Text;
		yield return (ordinal, $"registration chain at 0x{function.Begin:x}, value from declaration order, {where}");
	}

	private static List<ChainEntry> ReadChainEntries(CodeIndex code, PeImage image, PeFunction function)
	{
		var entries = new List<ChainEntry>();
		var window = new List<Instruction>();
		foreach (Instruction ins in code.Decode(function)) window.Add(ins);

		for (int i = 0; i < window.Count; i++)
		{
			Instruction lea = window[i];
			if (lea.Mnemonic != Mnemonic.Lea || !lea.IsIPRelativeMemoryOperand) continue;
			uint at = (uint) (lea.IPRelativeMemoryAddress - code.ImageBase);

			// The length travels beside the pointer, so the text is exactly that many bytes and
			// not the run up to the next NUL: the compiler merges literals, and "eat" lives inside
			// a longer string with nothing terminating it.
			string text = null;
			for (int j = i + 1; j < window.Count && j <= i + 4; j++)
			{
				Instruction ins = window[j];
				if (ins.Mnemonic != Mnemonic.Mov || ins.Op0Kind != OpKind.Memory || !IsImmediate(ins.GetOpKind(1))) continue;
				if (ins.MemorySize.GetSize() != 8) continue;
				text = Ascii(image, at, (int) (long) ins.GetImmediate(1));
				if (text != null) break;
			}
			if (text == null) continue;

			uint call = 0;
			for (int j = i + 1; j < window.Count && j <= i + 12; j++)
			{
				if (window[j].Mnemonic == Mnemonic.Call && window[j].Op0Kind == OpKind.NearBranch64)
				{
					call = (uint) (window[j].NearBranch64 - code.ImageBase);
					break;
				}
			}
			entries.Add(new ChainEntry(text, call));
		}
		return entries;
	}

	private static IEnumerable<(IReadOnlyDictionary<int, string> Values, string Shape)> ChainValuesFromThunks(CodeIndex code, PeImage image, List<ChainEntry> entries)
	{
		var pointers = new List<HashSet<uint>>();
		foreach (ChainEntry entry in entries)
		{
			if (entry.CallTarget == 0) yield break;
			if (!code.TryFunctionOf(entry.CallTarget, out PeFunction builder)) yield break;
			var set = new HashSet<uint>();
			foreach (Instruction ins in code.Decode(builder))
			{
				if (ins.Mnemonic != Mnemonic.Lea || !ins.IsIPRelativeMemoryOperand) continue;
				uint target = (uint) (ins.IPRelativeMemoryAddress - code.ImageBase);
				if (code.TryFunctionOf(target, out PeFunction pointed) && pointed.Begin == target) set.Add(target);
			}
			if (set.Count == 0) yield break;
			pointers.Add(set);
		}

		var common = new HashSet<uint>(pointers[0]);
		foreach (HashSet<uint> set in pointers.Skip(1)) common.IntersectWith(set);

		var thunks = new List<uint>();
		foreach (HashSet<uint> set in pointers)
		{
			var unique = set.Where(p => !common.Contains(p)).ToList();
			if (unique.Count != 1) yield break;
			thunks.Add(unique[0]);
		}

		var stores = new List<Dictionary<(int Offset, int Width), long>>();
		foreach (uint thunk in thunks)
		{
			var found = new Dictionary<(int, int), long>();
			if (!code.TryFunctionOf(thunk, out PeFunction body)) yield break;
			foreach (MemberAccess access in MemberAccesses.InFunction(code, body, Register.RCX, "thunk"))
			{
				if (!access.IsStore || access.Operand != "imm" || access.Width > 4) continue;
				found[(access.Offset, access.Width)] = ImmediateAt(code, body, access.InstructionRva);
			}
			stores.Add(found);
		}

		var places = new HashSet<(int, int)>(stores[0].Keys);
		foreach (Dictionary<(int, int), long> found in stores.Skip(1)) places.IntersectWith(found.Keys);

		foreach ((int offset, int width) in places.OrderBy(p => p.Item1))
		{
			var values = new Dictionary<int, string>();
			bool distinct = true;
			for (int i = 0; i < entries.Count; i++)
			{
				int value = (int) stores[i][(offset, width)];
				if (values.ContainsKey(value)) { distinct = false; break; }
				values[value] = entries[i].Text;
			}
			if (distinct && values.Count == entries.Count) yield return (values, $"registration chain, value from the per-value function's {width}-byte store at +{offset}");
		}
	}

	private static long ImmediateAt(CodeIndex code, PeFunction function, uint rva)
	{
		foreach (Instruction ins in code.Decode(function))
		{
			if (ins.IP - code.ImageBase != rva) continue;
			for (int i = 0; i < ins.OpCount; i++)
			{
				if (IsImmediate(ins.GetOpKind(i))) return (long) ins.GetImmediate(i);
			}
			break;
		}
		return long.MinValue;
	}

	/// <summary>Exactly <paramref name="length" /> printable ASCII bytes at the RVA, or null when the bytes are not that.</summary>
	internal static string Ascii(PeImage image, uint rva, int length)
	{
		if (length < 1 || length > 256) return null;
		ReadOnlySpan<byte> bytes;
		try
		{
			bytes = image.Bytes(rva, length);
		}
		catch (ArgumentOutOfRangeException)
		{
			return null;
		}
		foreach (byte b in bytes)
		{
			if (b < 0x20 || b >= 0x7f) return null;
		}
		return Encoding.ASCII.GetString(bytes);
	}

	/// <summary>The printable ASCII run starting at the RVA, or null when there is none of a usable length.</summary>
	internal static string AsciiRun(PeImage image, uint rva)
	{
		ReadOnlySpan<byte> bytes;
		try
		{
			bytes = image.Bytes(rva, 128);
		}
		catch (ArgumentOutOfRangeException)
		{
			return null;
		}
		int length = 0;
		while (length < bytes.Length && bytes[length] >= 0x20 && bytes[length] < 0x7f) length++;
		if (length < 2 || length >= bytes.Length) return null;
		return Encoding.ASCII.GetString(bytes.Slice(0, length));
	}
}