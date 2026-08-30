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
using System.Collections.Generic;
using System.Linq;
using Iced.Intel;

namespace MiNET.BdsExtract.Binary;

/// <summary>
///     One member of one class as the reflection binding states it: the name the wire uses, the
///     class the binding belongs to, the member's offset in that class's object, the entt meta type
///     id of the member's type and the name that id hashes back to.
/// </summary>
/// <param name="Owner">The class named by the type id the getter casts to, or null when that id hashes back to no name it was given.</param>
/// <param name="Offset">The offset the getter adds to the object pointer.</param>
/// <param name="TypeHash">The member's entt meta type id.</param>
/// <param name="TypeName">The name <paramref name="TypeHash" /> hashes back to, or null when no name it was given does.</param>
/// <param name="Width">The member's width in bytes, 0 when nothing states it; <paramref name="Evidence" /> says which witness supplied it.</param>
/// <param name="SiteRva">The instruction that takes the address of the name literal.</param>
public sealed record Binding(string WireName, string Owner, int Offset, uint TypeHash, string TypeName, int Width, uint SiteRva, string Evidence);

/// <summary>
///     The reflection bindings: what the code registers about a class's members, rather than what a
///     serializer happens to do with one.
///
///     Most components do not touch their own members in buildNetworkTag. They hand this to an EnTT
///     reflection binder, and the binder is where the member's name, place and type are all stated
///     in one go. The registration is a run of parts around each name: the literal with its own
///     length beside it (a {pointer, length} string view, which proves itself because the length has
///     to be the length of the text), then a node whose last two function pointers are the member's
///     setter and getter.
///
///     The getter names everything. It casts the object it is handed to the owning class by that
///     class's entt meta type id, adds the member's offset to the resulting pointer, writes the
///     member's own type id into the returned meta_any and stores the pointer:
///     <code>
///     mov  r8d, 7D773434h                ; entt::type_hash of UseModifiersItemComponent
///     call try_cast
///     mov  rbx, rax
///     add  rbx, 10h                      ; the member's offset
///     mov  dword [rsi+20h], 0A6C45D85h   ; entt::type_hash of float
///     mov  [rsi], rbx
///     </code>
///     Every id in that is FNV-1a 32 of the type's own name, so an id that reproduces a name the
///     caller supplied is a find and an id that reproduces none is emitted as the id. Nothing is
///     named by position or adjacency.
///
///     The setter is the second witness. It checks the same type id and then writes the value
///     through the same offset with an instruction whose operand size is the member's width
///     (movss [rdi+10h], xmm0), so the width is measured rather than assumed for any type, enums and
///     classes included. A setter that disagrees with the getter about the offset is reported and
///     neither is treated as the answer.
/// </summary>
public static class Bindings
{
	/// <summary>How many instructions after the name literal the registration node is looked for. The node is built inline, around 40 instructions past the literal on this build.</summary>
	private const int RegistrationWindow = 160;

	/// <summary>How many instructions after the pointer is taken the name's own length has to appear. The same proof MemberAccesses.Find requires.</summary>
	private const int LengthWindow = 4;

	/// <summary>The widths the C++ primitives have on this target, for a member whose setter did not state one.</summary>
	private static readonly Dictionary<string, int> PrimitiveWidths = new(StringComparer.Ordinal)
	{
		["bool"] = 1,
		["char"] = 1,
		["signed char"] = 1,
		["unsigned char"] = 1,
		["short"] = 2,
		["unsigned short"] = 2,
		["int"] = 4,
		["unsigned int"] = 4,
		["long"] = 4,
		["unsigned long"] = 4,
		["long long"] = 8,
		["unsigned long long"] = 8,
		["float"] = 4,
		["double"] = 8
	};

	/// <summary>FNV-1a 32, which is what entt::type_hash computes over entt::type_name.</summary>
	public static uint TypeHash(string name)
	{
		uint hash = 2166136261;
		foreach (char c in name)
		{
			hash ^= (byte) c;
			hash *= 16777619;
		}
		return hash;
	}

	/// <summary>
	///     Every binding the image states for the given wire names. The type names are the
	///     dictionary the type ids are resolved against: the C++ primitives, every name the
	///     signature strings state, and every enum the reference names. A name is only ever read out
	///     of it; an id that reproduces none of them is emitted as the id with no name.
	/// </summary>
	public static IReadOnlyList<Binding> Find(CodeIndex code, PeImage image, IReadOnlyList<string> wireNames, IReadOnlyList<string> typeNames)
	{
		var byHash = new Dictionary<uint, string>();
		foreach (string name in typeNames)
		{
			if (string.IsNullOrEmpty(name)) continue;
			uint hash = TypeHash(name);
			// Two names that hash alike are both kept, so a collision is visible instead of
			// silently answering with whichever was added first.
			if (byHash.TryGetValue(hash, out string had) && had != name) byHash[hash] = had + " | " + name;
			else byHash[hash] = name;
		}

		var literals = new DataLiterals(image, wireNames);
		var literalRvas = new HashSet<uint>();
		foreach (string name in wireNames)
		{
			foreach (uint rva in literals.Occurrences(name)) literalRvas.Add(rva);
		}

		var result = new List<Binding>();
		var seen = new HashSet<(string Name, uint Site)>();
		foreach (string name in wireNames.Distinct(StringComparer.Ordinal))
		{
			foreach (uint rva in literals.Occurrences(name))
			{
				foreach (uint site in code.ReferencesTo(rva))
				{
					if (!code.TryFunctionOf(site, out PeFunction function)) continue;
					if (!seen.Add((name, site))) continue;
					Binding binding = ReadSite(code, function, site, name, literalRvas, byHash);
					if (binding != null) result.Add(binding);
				}
			}
		}

		return result
			.OrderBy(b => b.WireName, StringComparer.Ordinal)
			.ThenBy(b => b.SiteRva)
			.ToList();
	}

	/// <summary>
	///     One reference to the name literal. It counts as a registration when the name's own length
	///     travels beside the pointer and a function the code takes the address of soon after reads
	///     as this member's getter or setter. Anything else is a use of the word, not a binding, and
	///     yields nothing rather than a guess.
	/// </summary>
	private static Binding ReadSite(CodeIndex code, PeFunction function, uint site, string name, HashSet<uint> literalRvas, Dictionary<uint, string> byHash)
	{
		var window = new List<Instruction>();
		bool started = false;
		bool lengthTravels = false;
		int after = 0;
		foreach (Instruction ins in code.Decode(function))
		{
			uint at = (uint) (ins.IP - code.ImageBase);
			if (!started)
			{
				if (at != site) continue;
				started = true;
				continue;
			}
			if (after < LengthWindow && CarriesImmediate(ins, name.Length)) lengthTravels = true;

			// The window ends at the next name literal, because that is the next member's
			// registration and its node must not be read as this one's.
			if (after > 0 && ins.Mnemonic == Mnemonic.Lea && ins.IsIPRelativeMemoryOperand && literalRvas.Contains((uint) (ins.IPRelativeMemoryAddress - code.ImageBase))) break;
			if (++after > RegistrationWindow) break;
			window.Add(ins);
		}
		if (!lengthTravels) return null;

		// Every function the window takes the address of, in the order it takes them. The node's
		// setter and getter are the last two; the earlier pointers are the node's other operations
		// and read as neither shape.
		var candidates = new List<uint>();
		foreach (Instruction ins in window)
		{
			if (ins.Mnemonic != Mnemonic.Lea || !ins.IsIPRelativeMemoryOperand) continue;
			uint target = (uint) (ins.IPRelativeMemoryAddress - code.ImageBase);
			if (!code.TryFunctionOf(target, out PeFunction pointed) || pointed.Begin != target) continue;
			if (!candidates.Contains(target)) candidates.Add(target);
		}

		// The node stores its operations in one run and the getter is the last of them, so the first
		// candidate that reads as a getter is this member's and the search stops there. Reading on
		// reaches the next member's node, whose offset belongs to a different member.
		uint getterRva = 0;
		int memberOffset = 0;
		uint typeHash = 0;
		var ownerCandidates = new List<uint>();
		int getterAt = -1;
		for (int i = 0; i < candidates.Count; i++)
		{
			(int Offset, uint Type, List<uint> Owners)? getter = ReadGetter(code, candidates[i]);
			if (getter == null) continue;
			getterRva = candidates[i];
			getterAt = i;
			memberOffset = getter.Value.Offset;
			typeHash = getter.Value.Type;
			ownerCandidates = getter.Value.Owners;
			break;
		}
		if (getterAt < 0) return null;

		string resolved = byHash.GetValueOrDefault(typeHash);

		// The getter names ids in more places than the owner's: a line number, the empty value's
		// type, the member's own. The owner is the one that is neither the member's type nor a
		// number that reproduces no name at all, and it has to be the only one left; two would mean
		// the shape named something else as well and nothing here would say which.
		List<uint> owners = ownerCandidates.Where(o => o != typeHash && byHash.ContainsKey(o)).Distinct().ToList();
		uint ownerHash = owners.Count == 1 ? owners[0] : 0;
		string owner = ownerHash == 0 ? null : byHash[ownerHash];

		// The setter sits in the same node, just before the getter, and is the only witness that
		// measures a width: it writes the value through the member's offset with an instruction
		// whose operand size is the member's own.
		uint setterRva = 0;
		int setterOffset = 0;
		int width = 0;
		for (int i = getterAt - 1; i >= 0; i--)
		{
			if (!ReadSetter(code, candidates[i], typeHash, out setterOffset, out int measured)) continue;
			setterRva = candidates[i];
			width = measured;
			break;
		}

		string widthFrom;
		if (setterRva != 0 && setterOffset == memberOffset)
		{
			widthFrom = $"width {width} from the setter's own store at 0x{setterRva:x}";
		}
		else
		{
			string missing = setterRva == 0
				? "no setter stored the member"
				: $"setter 0x{setterRva:x} writes +{setterOffset}, the getter reads +{memberOffset}";
			width = 0;
			if (resolved != null && PrimitiveWidths.TryGetValue(resolved, out int primitive))
			{
				width = primitive;
				widthFrom = $"width {width} from the type {resolved}, {missing}";
			}
			else
			{
				widthFrom = resolved == null
					? $"width unknown: the type id 0x{typeHash:x8} reproduces no name given, and {missing}"
					: $"width unknown: {resolved} is not a primitive and {missing}";
			}
		}

		var evidence = new List<string>
		{
			$"getter 0x{getterRva:x} adds {memberOffset} and returns type 0x{typeHash:x8}",
			ownerHash == 0
				? owners.Count == 0
					? $"no id in the getter reproduces a name given; it names {string.Join(", ", ownerCandidates.Select(o => $"0x{o:x8}"))}"
					: $"the getter names {owners.Count} owners: {string.Join(", ", owners.Select(o => $"0x{o:x8} {byHash[o]}"))}"
				: $"owner 0x{ownerHash:x8} is {owner}",
			widthFrom
		};

		return new Binding(name, owner, memberOffset, typeHash, resolved, width, site, string.Join("; ", evidence));
	}

	/// <summary>
	///     The getter shape: a store of a 32-bit type id into the returned meta_any at +0x20,
	///     followed by the member pointer stored into the same object at +0. The pointer is a
	///     register the function got back from a cast call and then added the member's offset to;
	///     one that was never added to is the member at offset 0, which is a value the shape states
	///     rather than a failure to find one. The empty-value path stores an immediate zero instead
	///     of a register, so it can never be read as a member at offset 0.
	/// </summary>
	private static (int Offset, uint Type, List<uint> Owners)? ReadGetter(CodeIndex code, uint functionRva)
	{
		if (!code.TryFunctionOf(functionRva, out PeFunction function) || function.Begin != functionRva) return null;

		// A register's offset from the cast result, and whether it is one at all. A register the
		// function wrote any other way is not a member pointer and carries no offset.
		var offsets = new Dictionary<Register, int>();
		var pendingType = new Dictionary<Register, uint>();
		var found = new List<(int Offset, uint Type)>();
		var owners = new List<uint>();
		bool afterCall = false;

		foreach (Instruction instruction in code.Decode(function))
		{
			Instruction ins = instruction;
			if (ins.IsInvalid) continue;

			// The type id the returned meta_any is stamped with, remembered per object register
			// until that same register's first word takes the pointer.
			if (ins.Mnemonic == Mnemonic.Mov && ins.Op0Kind == OpKind.Memory && ins.MemorySize == MemorySize.UInt32
				&& IsImmediate(ins.GetOpKind(1)) && !ins.IsIPRelativeMemoryOperand && ins.MemoryIndex == Register.None
				&& ins.MemoryDisplacement64 == 0x20 && ins.MemoryBase != Register.None)
			{
				pendingType[Full(ins.MemoryBase)] = (uint) (long) ins.GetImmediate(1);
				continue;
			}

			// The pointer going into the meta_any's first word.
			if (ins.Mnemonic == Mnemonic.Mov && ins.Op0Kind == OpKind.Memory && ins.Op1Kind == OpKind.Register
				&& ins.MemorySize is MemorySize.UInt64 or MemorySize.Int64 && !ins.IsIPRelativeMemoryOperand
				&& ins.MemoryIndex == Register.None && ins.MemoryDisplacement64 == 0 && ins.MemoryBase != Register.None
				&& offsets.TryGetValue(Full(ins.Op1Register), out int offset)
				&& pendingType.TryGetValue(Full(ins.MemoryBase), out uint type))
			{
				found.Add((offset, type));
				continue;
			}

			if (ins.Mnemonic == Mnemonic.Call)
			{
				afterCall = true;
				foreach (Register register in offsets.Keys.Where(IsVolatile).ToList()) offsets.Remove(register);
				continue;
			}

			// mov reg, rax right after a call is the cast's result: the object pointer, offset zero.
			if (afterCall && ins.Mnemonic == Mnemonic.Mov && ins.Op0Kind == OpKind.Register && ins.Op1Kind == OpKind.Register
				&& Full(ins.Op1Register) == Register.RAX && ins.Op0Register.GetSize() == 8)
			{
				offsets[Full(ins.Op0Register)] = 0;
				afterCall = false;
				continue;
			}
			afterCall = false;

			CollectOwner(ins, owners);

			if (ins.Mnemonic == Mnemonic.Add && ins.Op0Kind == OpKind.Register && IsImmediate(ins.GetOpKind(1))
				&& offsets.TryGetValue(Full(ins.Op0Register), out int had))
			{
				offsets[Full(ins.Op0Register)] = had + (int) (long) ins.GetImmediate(1);
				continue;
			}

			Track(ins, offsets);
		}

		if (found.Count == 0) return null;
		return (found[0].Offset, found[0].Type, owners);
	}

	/// <summary>
	///     The setter shape: the member's own type id checked with a compare rather than stored, and
	///     the value written through the object pointer with an instruction whose operand size is
	///     the member's width. That store is the only place in the binding that measures a width, so
	///     it is what a class or enum member's width comes from.
	///     <para>
	///         A setter checks two ids at the same place in two different objects: the owning class,
	///         on the meta_any holding the object, and the member's type, on the meta_any holding
	///         the value. The compare does not say which is which, so the getter's type is handed in
	///         and has to be one of the ids the function checks. Taking whichever came first read
	///         the owner as the member's type on every setter in the image.
	///     </para>
	/// </summary>
	private static bool ReadSetter(CodeIndex code, uint functionRva, uint expectedType, out int memberOffset, out int width)
	{
		memberOffset = 0;
		width = 0;
		if (!code.TryFunctionOf(functionRva, out PeFunction function) || function.Begin != functionRva) return false;

		var offsets = new Dictionary<Register, int>();
		var stores = new List<(int Offset, int Width)>();
		var checkedTypes = new HashSet<uint>();
		bool afterCall = false;

		foreach (Instruction instruction in code.Decode(function))
		{
			Instruction ins = instruction;
			if (ins.IsInvalid) continue;

			if (ins.Mnemonic == Mnemonic.Cmp && ins.Op0Kind == OpKind.Memory && ins.MemorySize == MemorySize.UInt32
				&& IsImmediate(ins.GetOpKind(1)) && !ins.IsIPRelativeMemoryOperand && ins.MemoryDisplacement64 == 0x20)
			{
				checkedTypes.Add((uint) (long) ins.GetImmediate(1));
				continue;
			}

			if (ins.Op0Kind == OpKind.Memory && ins.Op1Kind == OpKind.Register && !ins.IsIPRelativeMemoryOperand
				&& ins.MemoryIndex == Register.None && ins.MemoryBase != Register.None && Writes(ins)
				&& offsets.TryGetValue(Full(ins.MemoryBase), out int at))
			{
				int size = ins.MemorySize.GetSize();
				long displacement = (long) ins.MemoryDisplacement64;
				if (size > 0 && displacement >= 0 && displacement < 0x10000) stores.Add((at + (int) displacement, size));
			}

			if (ins.Mnemonic == Mnemonic.Call)
			{
				afterCall = true;
				foreach (Register register in offsets.Keys.Where(IsVolatile).ToList()) offsets.Remove(register);
				continue;
			}

			if (afterCall && ins.Mnemonic == Mnemonic.Mov && ins.Op0Kind == OpKind.Register && ins.Op1Kind == OpKind.Register
				&& Full(ins.Op1Register) == Register.RAX && ins.Op0Register.GetSize() == 8)
			{
				offsets[Full(ins.Op0Register)] = 0;
				afterCall = false;
				continue;
			}
			afterCall = false;

			if (ins.Mnemonic == Mnemonic.Add && ins.Op0Kind == OpKind.Register && IsImmediate(ins.GetOpKind(1))
				&& offsets.TryGetValue(Full(ins.Op0Register), out int had))
			{
				offsets[Full(ins.Op0Register)] = had + (int) (long) ins.GetImmediate(1);
				continue;
			}

			Track(ins, offsets);
		}

		if (!checkedTypes.Contains(expectedType) || stores.Count == 0) return false;
		memberOffset = stores[0].Offset;
		width = stores[0].Width;
		return true;
	}

	/// <summary>
	///     Keeps the map of which registers hold the object pointer and at what offset. A plain
	///     eight-byte register copy carries the offset with it; anything else that writes a register
	///     drops it, because a value the function computed some other way is not the object.
	/// </summary>
	private static void Track(Instruction instruction, Dictionary<Register, int> offsets)
	{
		if (instruction.Mnemonic == Mnemonic.Mov && instruction.Op0Kind == OpKind.Register && instruction.Op1Kind == OpKind.Register && instruction.Op0Register.GetSize() == 8)
		{
			if (offsets.TryGetValue(Full(instruction.Op1Register), out int copied)) offsets[Full(instruction.Op0Register)] = copied;
			else offsets.Remove(Full(instruction.Op0Register));
			return;
		}
		if (instruction.OpCount > 0 && instruction.Op0Kind == OpKind.Register && instruction.Op0Register.IsGPR() && Writes(instruction)) offsets.Remove(Full(instruction.Op0Register));
	}

	/// <summary>
	///     The 32-bit ids a getter names as the class it is casting to. Two shapes carry it: handed
	///     to the cast in a register (mov r8d, id / cmp edx, id), and compared straight against the
	///     object meta_any's own type word at +0x20, which is what a getter whose member is itself a
	///     class does instead of calling out. A store into that word is the member's type or the
	///     empty value's, never the owner's, so only compares are read from memory.
	/// </summary>
	private static void CollectOwner(Instruction instruction, List<uint> owners)
	{
		if (instruction.Mnemonic is not (Mnemonic.Mov or Mnemonic.Cmp)) return;
		if (instruction.OpCount < 2 || !IsImmediate(instruction.GetOpKind(1))) return;

		if (instruction.Op0Kind == OpKind.Register)
		{
			if (instruction.Op0Register.GetSize() != 4) return;
		}
		else if (instruction.Op0Kind == OpKind.Memory)
		{
			if (instruction.Mnemonic != Mnemonic.Cmp || instruction.MemorySize != MemorySize.UInt32) return;
			if (instruction.IsIPRelativeMemoryOperand || instruction.MemoryDisplacement64 != 0x20) return;
		}
		else
		{
			return;
		}

		long value = (long) instruction.GetImmediate(1);
		if (value < 0x10000 || value > uint.MaxValue) return;
		if (!owners.Contains((uint) value)) owners.Add((uint) value);
	}

	private static bool IsImmediate(OpKind kind)
	{
		return kind is OpKind.Immediate8 or OpKind.Immediate16 or OpKind.Immediate32 or OpKind.Immediate64
			or OpKind.Immediate8to16 or OpKind.Immediate8to32 or OpKind.Immediate8to64 or OpKind.Immediate32to64;
	}

	private static bool CarriesImmediate(Instruction instruction, long value)
	{
		for (int i = 0; i < instruction.OpCount; i++)
		{
			if (IsImmediate(instruction.GetOpKind(i)) && (long) instruction.GetImmediate(i) == value) return true;
		}
		return false;
	}

	private static bool Writes(Instruction instruction)
	{
		return instruction.Mnemonic is not (Mnemonic.Cmp or Mnemonic.Test or Mnemonic.Push or Mnemonic.Jmp or Mnemonic.Ret or Mnemonic.Nop);
	}

	private static bool IsVolatile(Register register)
	{
		return register is Register.RAX or Register.RCX or Register.RDX or Register.R8 or Register.R9 or Register.R10 or Register.R11;
	}

	private static Register Full(Register register)
	{
		return register.IsGPR() ? register.GetFullRegister() : register;
	}
}