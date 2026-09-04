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
///     One load or store the code performs on an object, as the instruction states it: the offset
///     off the register that holds the object, the width the memory operand carries, whether the
///     instruction writes it, and which register the finder accepted as the object.
///     <para>
///         <see cref="WireName" /> is the anchor the access was reached through: the field-name
///         literal for <see cref="MemberAccesses.Find" />, and the caller's label (normally the
///         class name) for the vtable and constructor anchors, which have no name literal.
///     </para>
///     <para>
///         <see cref="Operand" /> is the class of the value moving through: <c>gpr</c> for a
///         general purpose register, <c>xmm</c> for a vector register (float and double), and
///         <c>imm</c> when the other operand is an immediate.
///     </para>
/// </summary>
public sealed record MemberAccess(string WireName, uint FunctionRva, int Offset, int Width, string Operand, bool IsStore, string Register, uint InstructionRva);

/// <summary>
///     Finds the instructions that touch a member of an object, through three anchors: the field's
///     own name literal, a virtual function in a class's vtable, and the function that constructs
///     the class. Each anchor lands on a function; inside that function the object arrives in a
///     known argument register, and every memory operand off that register (and off the registers
///     it is copied into) is emitted with its offset and width.
///     <para>
///         Nothing here decides which access is the field. An access is what the code does; whether
///         it agrees with a declared offset and width is the consumer's comparison to make, and a
///         value seen at an offset never implies the member's width.
///     </para>
/// </summary>
public static class MemberAccesses
{
	/// <summary>The Windows x64 argument registers, in order, so an anchor can say which one carries the object.</summary>
	public static readonly Register[] ArgumentRegisters = { Register.RCX, Register.RDX, Register.R8, Register.R9 };

	/// <summary>
	///     For each wire name: every occurrence of the literal in the data sections, every
	///     instruction whose rip-relative operand resolves to one of them, the function that
	///     instruction sits in, and every access on the object register inside that function.
	///     <para>
	///         A pointer alone does not identify the name. Literals are merged, so a short name is
	///         a run of bytes inside a longer string and any code pointing at that address would
	///         count. The length travels beside the pointer, so a reference counts only when the
	///         name's own length appears within four instructions of the pointer being taken: that
	///         pair proves itself and a coincidence cannot pass it. A name used with no length
	///         beside it is reported as having no reference rather than guessed at.
	///     </para>
	/// </summary>
	public static IReadOnlyList<MemberAccess> Find(CodeIndex code, PeImage image, IReadOnlyList<string> wireNames)
	{
		var literals = new DataLiterals(image, wireNames);
		var result = new List<MemberAccess>();
		foreach (string name in wireNames)
		{
			var functions = new HashSet<uint>();
			foreach (uint rva in literals.Occurrences(name))
			foreach (uint site in code.ReferencesTo(rva))
			{
				if (!code.TryFunctionOf(site, out PeFunction function)) continue;
				if (!LengthTravelsWithThePointer(code, function, site, name.Length)) continue;
				if (!functions.Add(function.Begin)) continue;
				result.AddRange(InFunction(code, function, Register.RCX, name));
			}
		}
		return result;
	}

	/// <summary>
	///     Whether the name's length appears as an immediate within four instructions of the site
	///     that takes its address, either stored beside the pointer or loaded into a register.
	/// </summary>
	private static bool LengthTravelsWithThePointer(CodeIndex code, PeFunction function, uint site, int length)
	{
		int after = -1;
		foreach (Instruction ins in code.Decode(function))
		{
			uint at = (uint) (ins.IP - code.ImageBase);
			if (at == site) { after = 0; continue; }
			if (after < 0) continue;
			if (++after > 4) return false;
			for (int i = 0; i < ins.OpCount; i++)
			{
				if (ins.GetOpKind(i) is OpKind.Immediate8 or OpKind.Immediate16 or OpKind.Immediate32 or OpKind.Immediate64
					or OpKind.Immediate8to16 or OpKind.Immediate8to32 or OpKind.Immediate8to64 or OpKind.Immediate32to64
					&& (long) ins.GetImmediate(i) == length) return true;
			}
		}
		return false;
	}

	/// <summary>
	///     The accesses a class's own virtual function performs. The vtable is read from .rdata at
	///     <paramref name="vtableRva" />, the slot's entry is the function, and the object arrives
	///     in <paramref name="thisArg" />: RCX for a plain member function, RDX for one returning a
	///     class by value, because the hidden return slot takes RCX first.
	///     <paramref name="calleeDepth" /> follows direct calls that far, because a serializer
	///     normally hands the member to a helper rather than touching it in place.
	/// </summary>
	public static IReadOnlyList<MemberAccess> FindByVtableSerializer(CodeIndex code, PeImage image, uint vtableRva, int slot, Register thisArg, string label, int calleeDepth = 1)
	{
		var result = new List<MemberAccess>();
		if (vtableRva == 0) return result;
		ulong entry;
		try
		{
			entry = BitConverter.ToUInt64(image.Bytes(vtableRva + (uint) (slot * 8), 8));
		}
		catch (ArgumentOutOfRangeException)
		{
			return result;
		}
		if (entry <= image.ImageBase) return result;
		uint functionRva = (uint) (entry - image.ImageBase);
		return FindByConstructor(code, image, functionRva, thisArg, label, calleeDepth);
	}

	/// <summary>
	///     The accesses one named function performs on the object it receives in
	///     <paramref name="thisArg" />, and, when <paramref name="calleeDepth" /> is above zero, the
	///     accesses its direct callees perform on whatever the caller handed them in a register that
	///     already held the object.
	/// </summary>
	public static IReadOnlyList<MemberAccess> FindByConstructor(CodeIndex code, PeImage image, uint functionRva, Register thisArg, string label, int calleeDepth = 1)
	{
		var result = new List<MemberAccess>();
		var seen = new HashSet<(uint Function, Register Arg)>();
		Walk(code, functionRva, thisArg, label, calleeDepth, seen, result);
		return result;
	}

	/// <summary>
	///     The constructors a function calls on an object it has just allocated: the target of every
	///     direct call whose RCX holds the result of an earlier call. That is the construction path a
	///     templated <c>addComponent</c> or <c>_addStatefulComponent</c> takes, so the function
	///     carrying the class's <c>[T = Class]</c> signature string leads to the class's own
	///     constructor, where the object is back in RCX and its members are written at their offsets.
	/// </summary>
	public static IReadOnlyList<uint> ConstructorsCalledOnFreshObjects(CodeIndex code, uint functionRva)
	{
		var result = new List<uint>();
		if (!code.TryFunctionOf(functionRva, out PeFunction function)) return result;
		var fresh = new HashSet<Register>();
		foreach (Instruction ins in code.Decode(function))
		{
			if (ins.Mnemonic == Mnemonic.Call)
			{
				if (ins.Op0Kind == OpKind.NearBranch64)
				{
					uint target = (uint) (ins.NearBranch64 - code.ImageBase);
					if (fresh.Contains(Register.RCX) && !result.Contains(target)) result.Add(target);
				}
				fresh.Clear();
				fresh.Add(Register.RAX);
				continue;
			}
			Track(ins, fresh);
		}
		return result;
	}

	/// <summary>
	///     The class's constructors, found from its vtable: a constructor takes the object's address
	///     in RCX and writes the class's method table into its first eight bytes, so a function that
	///     takes the address of this vtable and stores it through a register derived from RCX is one.
	///     That tie is structural, which the field-name literal alone never is: any code using the
	///     same word points at the same bytes.
	/// </summary>
	public static IReadOnlyList<uint> ConstructorsWritingVtable(CodeIndex code, uint vtableRva)
	{
		var result = new List<uint>();
		if (vtableRva == 0) return result;
		var considered = new HashSet<uint>();
		foreach (uint site in code.ReferencesTo(vtableRva))
		{
			if (!code.TryFunctionOf(site, out PeFunction function)) continue;
			if (!considered.Add(function.Begin)) continue;

			var carriers = new HashSet<Register> { Register.RCX };
			Register holding = Register.None;
			int since = 0;
			foreach (Instruction ins in code.Decode(function))
			{
				if (ins.Mnemonic == Mnemonic.Lea && ins.IsIPRelativeMemoryOperand && (uint) (ins.IPRelativeMemoryAddress - code.ImageBase) == vtableRva && ins.Op0Kind == OpKind.Register)
				{
					holding = Full(ins.Op0Register);
					since = 0;
					continue;
				}
				if (holding != Register.None)
				{
					if (ins.Mnemonic == Mnemonic.Mov && ins.Op0Kind == OpKind.Memory && ins.Op1Kind == OpKind.Register && Full(ins.Op1Register) == holding
						&& ins.MemoryDisplacement64 == 0 && ins.MemoryIndex == Register.None && !ins.IsIPRelativeMemoryOperand && carriers.Contains(Full(ins.MemoryBase)))
					{
						result.Add(function.Begin);
						break;
					}
					if (++since > 8) holding = Register.None;
				}
				if (ins.Mnemonic == Mnemonic.Call) carriers.RemoveWhere(IsVolatile);
				else Track(ins, carriers);
			}
		}
		return result;
	}

	private static void Walk(CodeIndex code, uint functionRva, Register thisArg, string label, int depth, HashSet<(uint, Register)> seen, List<MemberAccess> result)
	{
		if (!code.TryFunctionOf(functionRva, out PeFunction function)) return;
		if (!seen.Add((function.Begin, thisArg))) return;
		result.AddRange(InFunction(code, function, thisArg, label));
		if (depth <= 0) return;

		// A serializer hands the member on rather than touching it in place, so follow a direct
		// call whose argument register already holds the object and read the callee the same way.
		foreach ((uint target, Register argument) in DirectCallsCarryingTheObject(code, function, thisArg))
		{
			Walk(code, target, argument, label, depth - 1, seen, result);
		}
	}

	/// <summary>
	///     Direct calls in the function whose argument registers hold the object at the call, paired
	///     with the argument position the callee will see it in.
	/// </summary>
	private static IEnumerable<(uint Target, Register Argument)> DirectCallsCarryingTheObject(CodeIndex code, PeFunction function, Register thisArg)
	{
		var carriers = new HashSet<Register> { Full(thisArg) };
		var found = new List<(uint, Register)>();
		foreach (Instruction ins in code.Decode(function))
		{
			if (ins.Mnemonic == Mnemonic.Call && ins.Op0Kind == OpKind.NearBranch64)
			{
				uint target = (uint) (ins.NearBranch64 - code.ImageBase);
				for (int i = 0; i < ArgumentRegisters.Length; i++)
				{
					if (carriers.Contains(ArgumentRegisters[i])) found.Add((target, ArgumentRegisters[i]));
				}
				// The call clobbers the volatile registers, so only the callee-saved copies survive.
				carriers.RemoveWhere(IsVolatile);
				continue;
			}
			Track(ins, carriers);
		}
		return found;
	}

	/// <summary>
	///     Every access on the object inside one function. The object starts in
	///     <paramref name="thisArg" /> and travels through plain register copies; a register that is
	///     written by anything else stops carrying it, so an offset is only ever reported off a
	///     register the function actually derived from the argument.
	/// </summary>
	public static IReadOnlyList<MemberAccess> InFunction(CodeIndex code, PeFunction function, Register thisArg, string label)
	{
		var result = new List<MemberAccess>();
		var carriers = new HashSet<Register> { Full(thisArg) };
		var info = new InstructionInfoFactory();
		foreach (Instruction ins in code.Decode(function))
		{
			if (ins.IsInvalid) continue;
			uint at = (uint) (ins.IP - code.ImageBase);
			if (!ins.IsIPRelativeMemoryOperand && ins.MemoryBase != Register.None && ins.MemoryIndex == Register.None && carriers.Contains(Full(ins.MemoryBase)))
			{
				long displacement = (long) ins.MemoryDisplacement64;
				if (displacement >= 0 && displacement < 0x10000)
				{
					int width = ins.MemorySize.GetSize();
					if (width > 0)
					{
						bool isStore = false;
						foreach (UsedMemory memory in info.GetInfo(ins).GetUsedMemory())
						{
							if (memory.Access is OpAccess.Write or OpAccess.CondWrite or OpAccess.ReadWrite or OpAccess.ReadCondWrite) isStore = true;
						}
						result.Add(new MemberAccess(label, function.Begin, (int) displacement, width, OperandClass(ins), isStore, ins.MemoryBase.ToString(), at));
					}
				}
			}
			if (ins.Mnemonic == Mnemonic.Call)
			{
				carriers.RemoveWhere(IsVolatile);
				continue;
			}
			Track(ins, carriers);
		}
		return result;
	}

	/// <summary>
	///     Keeps the set of registers that hold the object. A plain <c>mov reg, reg</c> from a
	///     carrier adds the destination; anything else that writes a register removes it, including
	///     an <c>lea</c> off a carrier, because that moves the base and every offset read off it
	///     would then be stated against the wrong origin.
	/// </summary>
	private static void Track(Instruction instruction, HashSet<Register> carriers)
	{
		if (instruction.Mnemonic == Mnemonic.Mov && instruction.Op0Kind == OpKind.Register && instruction.Op1Kind == OpKind.Register && instruction.Op0Register.GetSize() == 8)
		{
			if (carriers.Contains(Full(instruction.Op1Register))) carriers.Add(Full(instruction.Op0Register));
			else carriers.Remove(Full(instruction.Op0Register));
			return;
		}
		for (int i = 0; i < instruction.OpCount; i++)
		{
			if (instruction.GetOpKind(i) != OpKind.Register) continue;
			Register register = instruction.GetOpRegister(i);
			if (!register.IsGPR()) continue;
			// Operand 0 is the written one for the instructions that write a register at all; a
			// read-only use (cmp, test, push) leaves the carrier alone.
			if (i == 0 && Writes(instruction)) carriers.Remove(Full(register));
		}
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

	private static string OperandClass(Instruction instruction)
	{
		for (int i = 0; i < instruction.OpCount; i++)
		{
			OpKind kind = instruction.GetOpKind(i);
			if (kind == OpKind.Register && instruction.GetOpRegister(i).IsVectorRegister()) return "xmm";
		}
		for (int i = 0; i < instruction.OpCount; i++)
		{
			if (instruction.GetOpKind(i) is OpKind.Immediate8 or OpKind.Immediate8_2nd or OpKind.Immediate16 or OpKind.Immediate32 or OpKind.Immediate64
				or OpKind.Immediate8to16 or OpKind.Immediate8to32 or OpKind.Immediate8to64 or OpKind.Immediate32to64) return "imm";
		}
		return "gpr";
	}
}

/// <summary>
///     Where a set of literals sits in the image. One pass over every section that is not .text
///     finds every occurrence of every name asked for, matched as raw bytes rather than as a
///     NUL-terminated C string: the compiler merges short literals into longer ones and hands their
///     length separately, so "eat" exists in this binary only as three bytes inside another string
///     and a NUL-terminated search does not see it at all.
/// </summary>
internal sealed class DataLiterals
{
	private readonly Dictionary<string, List<uint>> _occurrences = new(StringComparer.Ordinal);

	public DataLiterals(PeImage image, IEnumerable<string> names)
	{
		var needles = new List<byte[]>();
		var texts = new List<string>();
		foreach (string name in names.Distinct(StringComparer.Ordinal))
		{
			if (name.Length < 2) continue;
			_occurrences[name] = new List<uint>();
			needles.Add(Encoding.ASCII.GetBytes(name));
			texts.Add(name);
		}

		// Bucketed by the first two bytes so one pass over ~65 MB of data sections serves every
		// name at once; a per-name scan costs the whole image again for each name.
		var buckets = new List<int>[0x10000];
		for (int i = 0; i < needles.Count; i++)
		{
			int key = needles[i][0] | (needles[i][1] << 8);
			(buckets[key] ??= new List<int>()).Add(i);
		}

		foreach (PeSection section in image.Sections)
		{
			if (section.Name == ".text" || section.RawSize == 0) continue;
			ReadOnlySpan<byte> bytes = image.Bytes(section.VirtualAddress, section.RawSize);
			for (int at = 0; at + 1 < bytes.Length; at++)
			{
				List<int> bucket = buckets[bytes[at] | (bytes[at + 1] << 8)];
				if (bucket == null) continue;
				foreach (int i in bucket)
				{
					byte[] needle = needles[i];
					if (at + needle.Length > bytes.Length) continue;
					if (!bytes.Slice(at, needle.Length).SequenceEqual(needle)) continue;
					_occurrences[texts[i]].Add(section.VirtualAddress + (uint) at);
				}
			}
		}
	}

	public IReadOnlyList<uint> Occurrences(string name)
	{
		return _occurrences.TryGetValue(name, out List<uint> list) ? list : Array.Empty<uint>();
	}
}