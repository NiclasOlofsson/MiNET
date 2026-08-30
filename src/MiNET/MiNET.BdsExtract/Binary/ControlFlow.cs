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
using System.Threading;
using System.Threading.Tasks;
using Iced.Intel;

namespace MiNET.BdsExtract.Binary;

/// <summary>Where a function start came from, in trust order: a .pdata entry first, a tail jump last.</summary>
public enum FunctionOrigin
{
	Pdata,
	EntryPoint,
	Export,
	TlsCallback,
	Handler,
	Pointer,
	Call,
	TailCall,
}

/// <summary>
///     A function the flow walk reached. End is the .pdata end for a Pdata function; for a
///     discovered one it is one past the contiguous run of decoded instructions from Rva, which is
///     the only span the function is taken to own, while its blocks may lie beyond it with other
///     code in between. Witness is the RVA of what named it: the call or
///     jump instruction, the pointer slot, the unwind info's function. Unverified is null when the
///     walk from Rva ended every path cleanly (ret, no-return call, tail jump, trap) inside bytes no
///     other function owns; otherwise it says what went wrong, and the function is still listed.
///     Name is the import an import thunk (a lone jmp through the IAT) forwards to, null otherwise.
/// </summary>
public sealed record DiscoveredFunction(uint Rva, uint End, FunctionOrigin Origin, uint Witness, string Unverified, string Name);

/// <summary>How many entries a jump table was read for: the count the preceding cmp states, or as many as land inside the function.</summary>
public enum JumpTableBound
{
	Compare,
	Range,
}

/// <summary>A resolved MSVC switch: the indirect jmp at At, its table of 32-bit RVAs at TableRva, and how many cases were read.</summary>
public readonly record struct JumpTable(uint At, uint TableRva, int Cases, JumpTableBound Bound);

/// <summary>
///     Recursive-descent code discovery over a PeImage: every .pdata function is walked by flow
///     (fall-through, both arms of a conditional branch, jump targets, continuing after a call),
///     and every call or tail-jump target, entry point, export, TLS callback and exception handler
///     that no .pdata range covers is walked the same way as a discovered function. The result is
///     the set of instruction starts flow actually reaches (<see cref="IsCode" />), the block starts
///     per function, and the call graph in both directions.
///
///     .pdata ranges are the trusted boundary: a walk inside one never leaves it, and anything
///     found outside every range is a claim that carries its witness and its verification state.
/// </summary>
public sealed class ControlFlow
{
	private readonly PeImage _image;
	private readonly uint _textStart;
	private readonly uint _textEnd;
	private readonly int _textOffset;
	private readonly ulong[] _code;
	private readonly Dictionary<uint, FunctionFacts> _facts = new();
	private readonly Dictionary<uint, List<uint>> _callers = new();
	private readonly List<DiscoveredFunction> _functions = new();
	private readonly List<DiscoveredFunction> _discovered = new();
	private readonly SortedDictionary<uint, FunctionFacts> _byStart = new();

	public ControlFlow(PeImage image)
	{
		_image = image;
		PeSection text = image.Text;
		_textStart = text.VirtualAddress;
		_textEnd = text.VirtualAddress + (uint) text.RawSize;
		image.TryFileOffset(_textStart, out _textOffset);
		_code = new ulong[(text.RawSize + 63) / 64];

		// Phase 1: every .pdata primary, with its chained ranges, walked in parallel. Ranges are
		// disjoint, so the only shared writes are the code bits, which are or'ed atomically.
		var pdata = new List<FunctionFacts>();
		foreach (IGrouping<uint, PeFunction> group in image.Functions.GroupBy(f => f.Parent == 0 ? f.Begin : f.Parent))
		{
			var facts = new FunctionFacts(group.Key, FunctionOrigin.Pdata, 0, group.Select(f => (f.Begin, f.End)).OrderBy(r => r.Begin).ToList());
			pdata.Add(facts);
			_byStart[facts.Rva] = facts;
		}
		Parallel.ForEach(pdata, facts => Walk(facts));
		_pdataRanges = pdata.SelectMany(f => f.Ranges.Select(r => (r.Begin, r.End, f.Rva))).OrderBy(r => r.Begin).ToList();

		// Phase 2: what phase 1 pointed at outside every .pdata range, plus the header seeds,
		// walked serially so each new function is a known start before the next walk begins.
		var seeds = new SortedDictionary<uint, (FunctionOrigin Origin, uint Witness)>();
		void Seed(uint rva, FunctionOrigin origin, uint witness)
		{
			if (rva < _textStart || rva >= _textEnd || TryFunctionOf(rva, out _)) return;
			if (!seeds.TryGetValue(rva, out var have) || origin < have.Origin) seeds[rva] = (origin, witness);
		}
		Seed(image.EntryPoint, FunctionOrigin.EntryPoint, 0);
		foreach (PeExport export in image.Exports) Seed(export.Rva, FunctionOrigin.Export, 0);
		foreach (uint callback in image.TlsCallbacks) Seed(callback, FunctionOrigin.TlsCallback, 0);
		foreach (PeFunction function in image.Functions) if (function.Handler != 0) Seed(function.Handler, FunctionOrigin.Handler, function.Begin);
		SeedPointers(Seed);
		foreach (FunctionFacts facts in pdata) SeedTargets(facts, Seed);

		while (seeds.Count > 0)
		{
			uint rva = seeds.Keys.First();
			(FunctionOrigin origin, uint witness) = seeds[rva];
			seeds.Remove(rva);
			if (TryFunctionOf(rva, out _)) continue;
			var facts = new FunctionFacts(rva, origin, witness, new List<(uint, uint)>());
			_byStart[rva] = facts;
			Walk(facts);
			facts.Ranges.Add((rva, facts.End));
			_discoveredRanges[rva] = facts.End;
			SeedTargets(facts, Seed);
		}

		// Phase 3: no-return to a fixpoint. Two sources: the compiler's own marks (a call that a
		// .pdata range ends on, or that flow falls from into a trap, names a callee that does not
		// return), and the walk's finding that no path out of a function returns: no ret, no fault,
		// no unresolved indirect jump, no indirect tail call, and every direct tail jump lands on
		// a no-return function or import. Each round re-walks the callers of what just qualified,
		// because their own ret may only have been reached by falling through such a call, and
		// stops when a round adds nothing.
		var callersOf = new Dictionary<uint, HashSet<uint>>();
		var importCallersOf = new Dictionary<string, HashSet<uint>>();
		foreach (FunctionFacts facts in _byStart.Values)
		{
			foreach ((_, uint target, _) in facts.Calls)
			{
				if (!callersOf.TryGetValue(target, out HashSet<uint> set)) callersOf[target] = set = new HashSet<uint>();
				set.Add(facts.Rva);
			}
			foreach ((_, string import, _) in facts.ImportCalls)
			{
				if (!importCallersOf.TryGetValue(import, out HashSet<uint> set)) importCallersOf[import] = set = new HashSet<uint>();
				set.Add(facts.Rva);
			}
		}
		while (true)
		{
			var rewalk = new HashSet<uint>();
			foreach (FunctionFacts facts in _byStart.Values)
			{
				foreach (uint callee in facts.MarkedCallees)
				{
					if (_noReturn.Add(callee) && callersOf.TryGetValue(callee, out HashSet<uint> callers)) rewalk.UnionWith(callers);
				}
				foreach (string import in facts.MarkedImports)
				{
					if (!IsNoReturnImport(import) && _learnedNoReturnImports.Add(import) && importCallersOf.TryGetValue(import, out HashSet<uint> callers)) rewalk.UnionWith(callers);
				}
			}
			var fresh = _byStart.Values.Where(f => !_noReturn.Contains(f.Rva) && NeverReturns(f)).Select(f => f.Rva).ToList();
			if (fresh.Count == 0 && rewalk.Count == 0) break;
			foreach (uint rva in fresh)
			{
				_noReturn.Add(rva);
				if (callersOf.TryGetValue(rva, out HashSet<uint> callers)) rewalk.UnionWith(callers);
			}
			foreach (uint rva in rewalk)
			{
				FunctionFacts facts = _byStart[rva];
				facts.Reset();
				Walk(facts);
				if (facts.Origin != FunctionOrigin.Pdata)
				{
					facts.Ranges[0] = (rva, facts.End);
					_discoveredRanges[rva] = facts.End;
				}
			}
		}

		// Phase 4: the walk that counts. The code bits and facts written above reflect whatever
		// no-return set each walk ran with; this pass redoes every function against the final
		// set, so a byte is code only if flow reaches it with every no-return call honoured.
		Array.Clear(_code);
		foreach (FunctionFacts facts in _byStart.Values) facts.Reset();
		Parallel.ForEach(pdata, facts => Walk(facts));
		foreach (FunctionFacts facts in _byStart.Values.Where(f => f.Origin != FunctionOrigin.Pdata))
		{
			Walk(facts);
			facts.Ranges[0] = (facts.Rva, facts.End);
			_discoveredRanges[facts.Rva] = facts.End;
		}

		foreach (FunctionFacts facts in _byStart.Values)
		{
			_facts[facts.Rva] = facts;
			string name = facts.ImportCalls.FirstOrDefault(c => c.Tail && c.At == facts.Rva).Import;
			var function = new DiscoveredFunction(facts.Rva, facts.End, facts.Origin, facts.Witness, facts.Fault, name);
			_functions.Add(function);
			if (facts.Origin != FunctionOrigin.Pdata) _discovered.Add(function);
			foreach ((_, uint target, _) in facts.Calls)
			{
				if (!_callers.TryGetValue(target, out List<uint> list)) _callers[target] = list = new List<uint>();
				if (!list.Contains(facts.Rva)) list.Add(facts.Rva);
			}
		}
		foreach (List<uint> list in _callers.Values) list.Sort();
	}

	/// <summary>Every function, .pdata and discovered, sorted by RVA.</summary>
	public IReadOnlyList<DiscoveredFunction> Functions => _functions;

	/// <summary>The functions no .pdata range covers, sorted by RVA, verified or not.</summary>
	public IReadOnlyList<DiscoveredFunction> Discovered => _discovered;

	/// <summary>True when no path out of the function starting at rva returns to its caller.</summary>
	public bool IsNoReturn(uint functionRva)
	{
		return _noReturn.Contains(functionRva);
	}

	/// <summary>
	///     Imports that never return to their caller, by symbol name. The CRT and Win32 exits, the
	///     C++ throw and terminate entry points, and the CRT's own fatal handlers.
	/// </summary>
	private static readonly HashSet<string> NoReturnImports = new(StringComparer.Ordinal)
	{
		"_exit", "exit", "_Exit", "quick_exit", "abort", "terminate", "__std_terminate",
		"ExitProcess", "ExitThread", "FreeLibraryAndExitThread", "RaiseFailFastException", "FatalAppExitA", "FatalAppExitW",
		"_CxxThrowException", "_invalid_parameter_noinfo_noreturn", "_invoke_watson", "_purecall", "longjmp",
	};

	private readonly HashSet<uint> _noReturn = new();

	/// <summary>Imports a .pdata range end proved no-return in this image, by full "MODULE!Symbol" name.</summary>
	private readonly HashSet<string> _learnedNoReturnImports = new(StringComparer.Ordinal);

	private bool IsNoReturnImport(string import)
	{
		int bang = import.IndexOf('!');
		return NoReturnImports.Contains(bang < 0 ? import : import.Substring(bang + 1)) || _learnedNoReturnImports.Contains(import);
	}

	private bool NeverReturns(FunctionFacts facts)
	{
		if (facts.HasReturn || facts.Fault != null || facts.UnresolvedJumps.Count > 0 || facts.IndirectTailCalls > 0) return false;
		foreach ((_, uint target, bool tail) in facts.Calls) if (tail && !_noReturn.Contains(target)) return false;
		foreach ((_, string import, bool tail) in facts.ImportCalls) if (tail && !IsNoReturnImport(import)) return false;
		return true;
	}

	/// <summary>True when flow reached an instruction starting at this RVA.</summary>
	public bool IsCode(uint rva)
	{
		if (rva < _textStart || rva >= _textEnd) return false;
		uint bit = rva - _textStart;
		return (Volatile.Read(ref _code[bit >> 6]) & (1UL << (int) (bit & 63))) != 0;
	}

	/// <summary>Block starts of the function starting at rva: its entry, every branch target and every fall-through after a conditional branch.</summary>
	public IReadOnlyList<uint> BlocksOf(uint functionRva)
	{
		return _facts.TryGetValue(functionRva, out FunctionFacts facts) ? facts.Blocks : Array.Empty<uint>();
	}

	/// <summary>Tail jumps through a pointer in the function starting at rva: the CFG dispatcher, a vtable slot, a global function pointer. Their targets are not knowable here.</summary>
	public int IndirectTailCallsOf(uint functionRva)
	{
		return _facts.TryGetValue(functionRva, out FunctionFacts facts) ? facts.IndirectTailCalls : 0;
	}

	/// <summary>Traps (int3, ud2) flow reached in the function starting at rva, each with whether flow fell into it from a call.</summary>
	public IReadOnlyList<(uint At, bool AfterCall)> TrapsOf(uint functionRva)
	{
		return _facts.TryGetValue(functionRva, out FunctionFacts facts) ? facts.Traps : Array.Empty<(uint, bool)>();
	}

	/// <summary>RVAs of the indirect jumps in the function starting at rva that neither an import nor a jump table explained.</summary>
	public IReadOnlyList<uint> UnresolvedIndirectJumpsOf(uint functionRva)
	{
		return _facts.TryGetValue(functionRva, out FunctionFacts facts) ? facts.UnresolvedJumps : Array.Empty<uint>();
	}

	/// <summary>The switches resolved in the function starting at rva, in the order the walk met them.</summary>
	public IReadOnlyList<JumpTable> JumpTablesOf(uint functionRva)
	{
		return _facts.TryGetValue(functionRva, out FunctionFacts facts) ? facts.JumpTables : Array.Empty<JumpTable>();
	}

	/// <summary>Calls and tail jumps through the import address table in the function starting at rva, in walk order.</summary>
	public IReadOnlyList<(uint At, string Import, bool Tail)> ImportCallsOf(uint functionRva)
	{
		return _facts.TryGetValue(functionRva, out FunctionFacts facts) ? facts.ImportCalls : Array.Empty<(uint, string, bool)>();
	}

	/// <summary>Direct call and tail-jump targets of the function starting at rva, sorted, distinct.</summary>
	public IReadOnlyList<uint> CalleesOf(uint functionRva)
	{
		return _facts.TryGetValue(functionRva, out FunctionFacts facts) ? facts.Calls.Select(c => c.Target).Distinct().OrderBy(t => t).ToList() : Array.Empty<uint>();
	}

	/// <summary>Functions that directly call or tail-jump to the function starting at rva, sorted.</summary>
	public IReadOnlyList<uint> CallersOf(uint functionRva)
	{
		return _callers.TryGetValue(functionRva, out List<uint> list) ? list : Array.Empty<uint>();
	}

	/// <summary>The function whose ranges hold rva, .pdata or discovered.</summary>
	public bool TryFunctionOf(uint rva, out uint functionRva)
	{
		// A chained cold range can sit far from its primary, so .pdata is searched by range, not
		// by function start. Discovered functions are one range each, keyed by start.
		int lo = 0, hi = _pdataRanges.Count - 1;
		while (lo <= hi)
		{
			int mid = (lo + hi) / 2;
			if (_pdataRanges[mid].Begin <= rva) lo = mid + 1;
			else hi = mid - 1;
		}
		if (hi >= 0 && rva < _pdataRanges[hi].End)
		{
			functionRva = _pdataRanges[hi].Function;
			return true;
		}
		IList<uint> starts = _discoveredRanges.Keys;
		lo = 0;
		hi = starts.Count - 1;
		while (lo <= hi)
		{
			int mid = (lo + hi) / 2;
			if (starts[mid] <= rva) lo = mid + 1;
			else hi = mid - 1;
		}
		if (hi >= 0 && rva < _discoveredRanges.Values[hi])
		{
			functionRva = starts[hi];
			return true;
		}
		functionRva = 0;
		return false;
	}

	private List<(uint Begin, uint End, uint Function)> _pdataRanges = new();
	private readonly SortedList<uint, uint> _discoveredRanges = new();

	private void SeedTargets(FunctionFacts facts, Action<uint, FunctionOrigin, uint> seed)
	{
		foreach ((uint at, uint target, bool tail) in facts.Calls) seed(target, tail ? FunctionOrigin.TailCall : FunctionOrigin.Call, at);
	}

	/// <summary>
	///     Every 8-byte aligned qword in a section other than .text whose value is an absolute address
	///     inside .text: vtable slots, function tables, registered callbacks. The witness is the slot.
	/// </summary>
	private void SeedPointers(Action<uint, FunctionOrigin, uint> seed)
	{
		ulong low = _image.ImageBase + _textStart;
		ulong high = _image.ImageBase + _textEnd;
		foreach (PeSection section in _image.Sections)
		{
			if (section.Name == ".text" || section.RawSize < 8) continue;
			ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(_image.RawData, section.RawOffset, section.RawSize);
			for (int offset = 0; offset + 8 <= bytes.Length; offset += 8)
			{
				ulong value = System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(bytes.Slice(offset));
				if (value >= low && value < high) seed((uint) (value - _image.ImageBase), FunctionOrigin.Pointer, section.VirtualAddress + (uint) offset);
			}
		}
	}

	private void MarkCode(uint rva)
	{
		uint bit = rva - _textStart;
		ulong mask = 1UL << (int) (bit & 63);
		ref ulong word = ref _code[bit >> 6];
		ulong seen = Volatile.Read(ref word);
		while ((seen & mask) == 0)
		{
			ulong was = Interlocked.CompareExchange(ref word, seen | mask, seen);
			if (was == seen) break;
			seen = was;
		}
	}

	/// <summary>
	///     Walks one function by flow from its start. With ranges (a .pdata function) the walk never
	///     leaves them; without (a discovered function) it stops where it would run into another
	///     function's start or off .text, and reports that as a fault.
	/// </summary>
	private void Walk(FunctionFacts facts)
	{
		bool bounded = facts.Origin == FunctionOrigin.Pdata;
		bool Inside(uint rva)
		{
			if (bounded)
			{
				foreach ((uint begin, uint end) in facts.Ranges) if (rva >= begin && rva < end) return true;
				return false;
			}
			return rva >= _textStart && rva < _textEnd && (!TryFunctionOf(rva, out uint owner) || owner == facts.Rva);
		}

		var reader = new ByteArrayCodeReader(_image.RawData, _textOffset, (int) (_textEnd - _textStart));
		Decoder decoder = Decoder.Create(64, reader, _image.ImageBase + facts.Rva);
		var visited = new Dictionary<uint, int>();
		var pending = new Stack<uint>();
		var decoded = new List<Instruction>();
		var blocks = new HashSet<uint> { facts.Rva };
		var leaValues = new Dictionary<Register, uint>();
		var guardRegisters = new HashSet<Register>();
		void Track(in Instruction ins)
		{
			// What a register holds, as far as a jump-table base or the CFG dispatcher goes, kept
			// in decode order: the entry block is decoded first, and that is where the compiler
			// hoists both loads. Any other write to the register forgets it.
			if (!WritesOp0(ins)) return;
			Register full = ins.Op0Register.GetFullRegister();
			leaValues.Remove(full);
			guardRegisters.Remove(full);
			if (ins.Mnemonic == Mnemonic.Lea && ins.IsIPRelativeMemoryOperand) leaValues[full] = (uint) (ins.IPRelativeMemoryAddress - _image.ImageBase);
			else if (ins.Mnemonic == Mnemonic.Mov && ins.IsIPRelativeMemoryOperand && _image.GuardDispatchSlot != 0 && ins.IPRelativeMemoryAddress - _image.ImageBase == _image.GuardDispatchSlot) guardRegisters.Add(full);
			else if (ins.Mnemonic == Mnemonic.Mov && ins.Op1Kind == OpKind.Register)
			{
				Register source = ins.Op1Register.GetFullRegister();
				if (guardRegisters.Contains(source)) guardRegisters.Add(full);
				if (leaValues.TryGetValue(source, out uint value)) leaValues[full] = value;
			}
		}
		pending.Push(facts.Rva);
		uint maxEnd = facts.Rva;

		// Which registers hold a copy of which, per block: the state at the branch that created a
		// block is the block's entry state, so a bound checked on a register the index was copied
		// from in the block before is still found. In-block renames are handled by the scan itself.
		var blockAliases = new Dictionary<uint, Dictionary<Register, Register>>();
		var blockOfDecoded = new List<uint>();
		var aliases = new Dictionary<Register, Register>();
		void Alias(in Instruction ins)
		{
			if (!WritesOp0(ins)) return;
			Register destination = ins.Op0Register.GetFullRegister();
			if (aliases.Count > 0)
			{
				List<Register> stale = null;
				foreach (KeyValuePair<Register, Register> pair in aliases) if (pair.Key == destination || pair.Value == destination) (stale ??= new List<Register>()).Add(pair.Key);
				if (stale != null) foreach (Register key in stale) aliases.Remove(key);
			}
			if (ins.Mnemonic == Mnemonic.Mov && ins.Op1Kind == OpKind.Register)
			{
				Register source = ins.Op1Register.GetFullRegister();
				if (source != destination) aliases[destination] = source;
			}
		}

		void Block(uint rva)
		{
			if (blocks.Add(rva))
			{
				pending.Push(rva);
				blockAliases[rva] = new Dictionary<Register, Register>(aliases);
			}
		}

		while (pending.Count > 0)
		{
			uint ip = pending.Pop();
			reader.Position = (int) (ip - _textStart);
			decoder.IP = _image.ImageBase + ip;
			// A copy: the stored entry state must survive what this block does to its registers.
			aliases = blockAliases.TryGetValue(ip, out Dictionary<Register, Register> entryState) ? new Dictionary<Register, Register>(entryState) : new Dictionary<Register, Register>();
			bool fellThrough = false;
			while (true)
			{
				uint at = (uint) (decoder.IP - _image.ImageBase);
				if (!visited.TryAdd(at, 0)) break;
				bool afterCall = fellThrough && decoded[decoded.Count - 1].FlowControl is FlowControl.Call or FlowControl.IndirectCall;
				if (!Inside(at))
				{
					// Falling off the end of a .pdata range is the compiler's own statement that
					// nothing follows: the last instruction ends the function. When that is a
					// call, the callee does not return, and that is recorded for the fixpoint.
					if (bounded && fellThrough && afterCall) MarkCallee(facts, decoded[decoded.Count - 1]);
					else facts.Fault ??= $"flow reaches 0x{at:X}, outside the function";
					break;
				}
				fellThrough = true;
				decoder.Decode(out Instruction ins);
				if (ins.IsInvalid)
				{
					facts.Fault ??= $"invalid instruction at 0x{at:X}";
					break;
				}
				uint next = at + (uint) ins.Length;
				if (!Inside(next - 1))
				{
					facts.Fault ??= $"instruction at 0x{at:X} runs past the function";
					break;
				}
				MarkCode(at);
				visited[at] = ins.Length;
				if (next > maxEnd) maxEnd = next;
				decoded.Add(ins);
				blockOfDecoded.Add(ip);
				Track(ins);
				Alias(ins);

				bool stop = false;
				switch (ins.FlowControl)
				{
					case FlowControl.Next:
					case FlowControl.XbeginXabortXend:
						break;
					case FlowControl.ConditionalBranch:
					{
						uint target = (uint) (ins.NearBranchTarget - _image.ImageBase);
						if (Inside(target)) Block(target);
						else facts.Calls.Add((at, target, true));
						Block(next);
						stop = true;
						break;
					}
					case FlowControl.UnconditionalBranch:
					{
						uint target = (uint) (ins.NearBranchTarget - _image.ImageBase);
						if (Inside(target)) Block(target);
						else facts.Calls.Add((at, target, true));
						stop = true;
						break;
					}
					case FlowControl.IndirectBranch:
						if (ins.IsIPRelativeMemoryOperand && _image.Imports.TryGetValue((uint) (ins.IPRelativeMemoryAddress - _image.ImageBase), out string importJump)) facts.ImportCalls.Add((at, importJump, true));
						else if (ins.Op0Kind == OpKind.Memory || (ins.Op0Kind == OpKind.Register && guardRegisters.Contains(ins.Op0Register.GetFullRegister()))) facts.IndirectTailCalls++;
						else if (TryJumpTable(decoded, blockOfDecoded, blockAliases, leaValues, at, Inside, out List<uint> cases, out JumpTable table))
						{
							facts.JumpTables.Add(table);
							foreach (uint target in cases) Block(target);
						}
						else facts.UnresolvedJumps.Add(at);
						stop = true;
						break;
					case FlowControl.Call:
					{
						uint target = (uint) (ins.NearBranchTarget - _image.ImageBase);
						facts.Calls.Add((at, target, false));
						if (_noReturn.Contains(target)) stop = true;
						break;
					}
					case FlowControl.IndirectCall:
						if (ins.IsIPRelativeMemoryOperand && _image.Imports.TryGetValue((uint) (ins.IPRelativeMemoryAddress - _image.ImageBase), out string importCall))
						{
							facts.ImportCalls.Add((at, importCall, false));
							if (IsNoReturnImport(importCall)) stop = true;
						}
						break;
					case FlowControl.Return:
						facts.HasReturn = true;
						stop = true;
						break;
					case FlowControl.Interrupt:
					case FlowControl.Exception:
						// A trap flow falls into from a call is the compiler's mark that the callee
						// does not return, the same statement a range ending on the call makes.
						facts.Traps.Add((at, afterCall));
						if (afterCall) MarkCallee(facts, decoded[decoded.Count - 2]);
						stop = true;
						break;
				}
				if (stop) break;
			}
		}

		facts.Blocks = blocks.OrderBy(b => b).ToList();
		if (bounded) facts.End = facts.Ranges.Max(r => r.End);
		else
		{
			// The contiguous run from the entry: each decoded instruction followed by another.
			uint end = facts.Rva;
			while (visited.TryGetValue(end, out int length) && length > 0) end += (uint) length;
			facts.End = end;
		}
	}

	/// <summary>Records the callee of a call the compiler marked as not returning: a direct target, or an import through the IAT.</summary>
	private void MarkCallee(FunctionFacts facts, in Instruction call)
	{
		if (call.FlowControl == FlowControl.Call) facts.MarkedCallees.Add((uint) (call.NearBranchTarget - _image.ImageBase));
		else if (call.FlowControl == FlowControl.IndirectCall && call.IsIPRelativeMemoryOperand && _image.Imports.TryGetValue((uint) (call.IPRelativeMemoryAddress - _image.ImageBase), out string import)) facts.MarkedImports.Add(import);
	}

	/// <summary>
	///     Recognises the MSVC x64 switch behind an indirect register jump. The shape, read backwards
	///     from the jmp over the instructions decoded so far: add R,Base; mov R32,[Base+Index*4+disp]
	///     (or movsxd); and, when the compiler emitted one, cmp Index,N with the index possibly
	///     renamed through one mov Index,Other in between. Base is what a lea Base,[rip+X] above
	///     loaded: __ImageBase, with disp the table's RVA and the entries RVAs, or the table itself,
	///     with disp 0 and the entries offsets from the table. Without a lea in reach Base is taken
	///     as __ImageBase. With a cmp the table has N+1 entries and every one must land inside the
	///     function; without one (a switch whose index came out of a byte table, or whose bound
	///     check is out of pattern) entries are read while they land inside the function. Either
	///     way an entry outside is the stop, so the table is proven by its own contents.
	/// </summary>
	private bool TryJumpTable(List<Instruction> decoded, List<uint> blockOfDecoded, Dictionary<uint, Dictionary<Register, Register>> blockAliases, Dictionary<Register, uint> leaValues, uint jmpRva, Func<uint, bool> inside, out List<uint> cases, out JumpTable table)
	{
		cases = null;
		table = default;
		Instruction jmp = decoded[decoded.Count - 1];
		if (jmp.Op0Kind != OpKind.Register) return false;
		Register r = jmp.Op0Register;
		Register r32 = r.GetFullRegister32();

		Register baseRegister = Register.None;
		Register index = Register.None;
		uint tableRva = 0;
		int movAt = -1;
		for (int i = decoded.Count - 2, n = 0; i >= 0 && n < 32; i--, n++)
		{
			Instruction ins = decoded[i];
			if (EndsBlock(ins)) break;
			if (baseRegister == Register.None)
			{
				if (ins.Mnemonic == Mnemonic.Add && ins.Op0Kind == OpKind.Register && ins.Op0Register == r && ins.Op1Kind == OpKind.Register) baseRegister = ins.Op1Register;
				continue;
			}
			bool loadsEntry = (ins.Mnemonic == Mnemonic.Mov || ins.Mnemonic == Mnemonic.Movsxd) && ins.Op0Kind == OpKind.Register && (ins.Op0Register == r32 || ins.Op0Register == r) && ins.Op1Kind == OpKind.Memory && ins.MemoryBase == baseRegister && ins.MemoryIndex != Register.None && ins.MemoryIndexScale == 4;
			if (loadsEntry)
			{
				index = ins.MemoryIndex;
				tableRva = (uint) ins.MemoryDisplacement64;
				movAt = i;
				break;
			}
		}
		if (index == Register.None) return false;

		uint baseRva = leaValues.TryGetValue(baseRegister.GetFullRegister(), out uint leaBase) ? leaBase : 0;
		tableRva += baseRva;

		Dictionary<Register, Register> entryAliases = blockAliases.GetValueOrDefault(blockOfDecoded[movAt]);
		int? bound = Bound(decoded, movAt - 1, index, entryAliases, out int writer);
		if (bound == null && writer >= 0)
		{
			// Two-level switch: the index came out of a byte table, whose own index is what the
			// cmp bounds; the largest byte in it plus one is the rva table's size.
			Instruction load = decoded[writer];
			if (load.Mnemonic == Mnemonic.Movzx && load.Op1Kind == OpKind.Memory && load.MemorySize == MemorySize.UInt8 && load.MemoryIndex != Register.None && load.MemoryIndexScale == 1)
			{
				int? byteBound = Bound(decoded, writer - 1, load.MemoryIndex, entryAliases, out _);
				if (byteBound != null)
				{
					uint byteTable = (leaValues.TryGetValue(load.MemoryBase.GetFullRegister(), out uint byteBase) ? byteBase : 0) + (uint) load.MemoryDisplacement64;
					int largest = -1;
					for (int k = 0; k < byteBound; k++)
					{
						if (!_image.TryFileOffset(byteTable + (uint) k, out _))
						{
							largest = -1;
							break;
						}
						largest = Math.Max(largest, _image.Bytes(byteTable + (uint) k, 1)[0]);
					}
					if (largest >= 0) bound = largest + 1;
				}
			}
		}

		cases = new List<uint>();
		int limit = bound ?? 65536;
		for (int k = 0; k < limit; k++)
		{
			uint slot = tableRva + 4 * (uint) k;
			if (!_image.TryFileOffset(slot, out _) || !_image.TryFileOffset(slot + 3, out _))
			{
				if (bound != null) return false;
				break;
			}
			uint entry = baseRva + System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(_image.Bytes(slot, 4));
			if (!inside(entry))
			{
				if (bound != null) return false;
				break;
			}
			cases.Add(entry);
		}
		if (cases.Count == 0) return false;
		table = new JumpTable(jmpRva, tableRva, cases.Count, bound != null ? JumpTableBound.Compare : JumpTableBound.Range);
		return true;
	}

	/// <summary>
	///     How many indexes dispatch through the table, read from the check above position from on
	///     the index register in any width, following one mov Index,Other rename: cmp Index,N
	///     followed by the branch that leaves on ja/jg (N+1 cases) or jae/jge (N), or and Index,mask
	///     (mask+1). Null when no check is in reach; writer is then the position of the instruction
	///     that produced the index, or -1 when none was met.
	/// </summary>
	private static int? Bound(List<Instruction> decoded, int from, Register index, Dictionary<Register, Register> entryAliases, out int writer)
	{
		writer = -1;
		Register full = index.GetFullRegister();
		bool SameValue(Register r)
		{
			if (r == full) return true;
			if (entryAliases == null) return false;
			return (entryAliases.TryGetValue(full, out Register source) && source == r) || (entryAliases.TryGetValue(r, out Register copied) && copied == full);
		}
		for (int i = from, n = 0; i >= 0 && n < 32; i--, n++)
		{
			Instruction ins = decoded[i];
			if (EndsBlock(ins)) return null;
			bool onIndex = ins.Op0Kind == OpKind.Register && SameValue(ins.Op0Register.GetFullRegister());
			if (!onIndex) continue;
			bool immediate = ins.Op1Kind != OpKind.Register && ins.Op1Kind != OpKind.Memory;
			if (ins.Mnemonic == Mnemonic.Cmp && immediate)
			{
				int limit = (int) ins.GetImmediate(1);
				for (int j = i + 1; j < decoded.Count && j <= i + 4; j++)
				{
					Instruction branch = decoded[j];
					if (branch.FlowControl != FlowControl.ConditionalBranch) continue;
					return branch.Mnemonic is Mnemonic.Jae or Mnemonic.Jge or Mnemonic.Jb or Mnemonic.Jl ? limit : limit + 1;
				}
				return limit + 1;
			}
			if (ins.Mnemonic == Mnemonic.And && immediate) return (int) ins.GetImmediate(1) + 1;
			if (ins.Mnemonic == Mnemonic.Test) continue;
			if (ins.Mnemonic == Mnemonic.Mov && ins.Op1Kind == OpKind.Register)
			{
				full = ins.Op1Register.GetFullRegister();
				continue;
			}
			writer = i;
			return null;
		}
		return null;
	}

	/// <summary>Op0 is a register the instruction writes: any register first operand except on the mnemonics that only read it.</summary>
	private static bool WritesOp0(in Instruction ins)
	{
		return ins.Op0Kind == OpKind.Register && ins.Mnemonic is not (Mnemonic.Cmp or Mnemonic.Test or Mnemonic.Push or Mnemonic.Bt or Mnemonic.Jmp or Mnemonic.Call);
	}

	private static bool EndsBlock(in Instruction ins)
	{
		return ins.FlowControl is FlowControl.Return or FlowControl.UnconditionalBranch or FlowControl.IndirectBranch or FlowControl.Call or FlowControl.IndirectCall or FlowControl.Interrupt or FlowControl.Exception;
	}

	private sealed class FunctionFacts
	{
		public FunctionFacts(uint rva, FunctionOrigin origin, uint witness, List<(uint Begin, uint End)> ranges)
		{
			Rva = rva;
			Origin = origin;
			Witness = witness;
			Ranges = ranges;
		}

		public uint Rva { get; }
		public FunctionOrigin Origin { get; }
		public uint Witness { get; }
		public List<(uint Begin, uint End)> Ranges { get; }
		public uint End { get; set; }
		public List<uint> Blocks { get; set; } = new();
		public List<(uint At, uint Target, bool Tail)> Calls { get; } = new();
		public List<(uint At, string Import, bool Tail)> ImportCalls { get; } = new();
		public List<JumpTable> JumpTables { get; } = new();
		public bool HasReturn { get; set; }
		public int IndirectTailCalls { get; set; }
		public List<(uint At, bool AfterCall)> Traps { get; } = new();
		public List<uint> UnresolvedJumps { get; } = new();
		public List<uint> MarkedCallees { get; } = new();
		public List<string> MarkedImports { get; } = new();
		public string Fault { get; set; }

		/// <summary>Clears everything a walk writes, so the function can be walked again against a larger no-return set.</summary>
		public void Reset()
		{
			End = 0;
			Blocks = new List<uint>();
			Calls.Clear();
			ImportCalls.Clear();
			JumpTables.Clear();
			HasReturn = false;
			IndirectTailCalls = 0;
			Traps.Clear();
			UnresolvedJumps.Clear();
			MarkedCallees.Clear();
			MarkedImports.Clear();
			Fault = null;
		}
	}
}
