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

using System.Collections.Generic;
using System.Linq;
using Iced.Intel;

namespace MiNET.BdsExtract.Binary;

/// <summary>
///     Decodes a PeImage's .text once with Iced and builds a rip-relative cross-reference index:
///     for every instruction whose memory operand resolves relative to the instruction pointer,
///     the target RVA it reads or writes maps back to the RVA of that instruction. Every other
///     binary fact family (class inventory, type-id slots, vtables, enum tables, member accesses)
///     is a query over this index rather than its own decode pass.
///
///     .text also holds data islands and padding between functions; decoding it linearly walks
///     through those bytes as garbage instructions too. That garbage can still resolve an
///     IP-relative operand to a real-looking target, so a hit here is a candidate, never proof by
///     itself: every consumer below is expected to confirm a hit sits inside a real .pdata function
///     (<see cref="TryFunctionOf" />) and, where possible, against a second, independent witness.
/// </summary>
public sealed class CodeIndex
{
	private readonly PeImage _image;
	private readonly Dictionary<uint, List<uint>> _references = new();
	private readonly Dictionary<uint, PeFunction> _primaries = new();
	private readonly Dictionary<uint, List<PeFunction>> _ranges = new();

	public ulong ImageBase => _image.ImageBase;

	/// <summary>How many .pdata entries are a chained (cold-split) range of a function that starts elsewhere.</summary>
	public int ChainedRanges { get; }

	public CodeIndex(PeImage image)
	{
		_image = image;

		// A function whose cold path the linker moved elsewhere owns several .pdata entries: one
		// primary and one chained range per fragment, each naming the primary in its unwind info.
		// They are gathered here so an anchor landing in a fragment answers with the primary and
		// decodes the whole function, rather than the few bytes of the fragment it happened to hit.
		foreach (PeFunction function in image.Functions)
		{
			uint primary = function.Parent == 0 ? function.Begin : function.Parent;
			if (!_ranges.TryGetValue(primary, out List<PeFunction> list)) _ranges[primary] = list = new List<PeFunction>();
			list.Add(function);
			if (function.Parent == 0) _primaries[primary] = function;
			else ChainedRanges++;
		}
		foreach (List<PeFunction> list in _ranges.Values) list.Sort((a, b) => a.Begin.CompareTo(b.Begin));

		PeSection text = image.Text;
		image.TryFileOffset(text.VirtualAddress, out int textOffset);
		var reader = new ByteArrayCodeReader(image.RawData, textOffset, text.RawSize);
		ulong start = image.ImageBase + text.VirtualAddress;
		ulong end = start + (ulong) text.RawSize;
		Decoder decoder = Decoder.Create(64, reader, start);
		while (decoder.IP < end)
		{
			decoder.Decode(out Instruction ins);
			if (ins.IsInvalid) continue;
			if (ins.IsIPRelativeMemoryOperand)
			{
				uint target = (uint) (ins.IPRelativeMemoryAddress - image.ImageBase);
				uint at = (uint) (ins.IP - image.ImageBase);
				if (!_references.TryGetValue(target, out List<uint> list)) _references[target] = list = new List<uint>();
				list.Add(at);
			}
		}
	}

	/// <summary>Every data address the index holds a reference to, which is where a table is looked for rather than searched for.</summary>
	public IEnumerable<uint> ReferenceTargets => _references.Keys;

	public IReadOnlyList<uint> ReferencesTo(uint targetRva)
	{
		return _references.TryGetValue(targetRva, out List<uint> list) ? list : System.Array.Empty<uint>();
	}

	/// <summary>
	///     The function whose ranges hold this RVA. A hit inside a chained range answers with the
	///     primary entry, so the caller identifies the function by one RVA however the linker split
	///     its body.
	/// </summary>
	public bool TryFunctionOf(uint rva, out PeFunction function)
	{
		// Functions are sorted by Begin; binary search the last Begin <= rva.
		IReadOnlyList<PeFunction> functions = _image.Functions;
		int lo = 0, hi = functions.Count - 1;
		while (lo <= hi)
		{
			int mid = (lo + hi) / 2;
			if (functions[mid].Begin <= rva) lo = mid + 1;
			else hi = mid - 1;
		}
		if (hi >= 0 && rva < functions[hi].End)
		{
			function = functions[hi];
			if (function.Parent != 0 && _primaries.TryGetValue(function.Parent, out PeFunction primary)) function = primary;
			return true;
		}
		function = default;
		return false;
	}

	/// <summary>The ranges of the function starting at this RVA, in address order: its own, plus every chained range whose unwind info names it.</summary>
	public IReadOnlyList<PeFunction> RangesOf(uint functionRva)
	{
		return _ranges.TryGetValue(functionRva, out List<PeFunction> list) ? list : System.Array.Empty<PeFunction>();
	}

	/// <summary>
	///     The function's instructions, every range of it decoded in address order as one stream.
	///     A window a caller counts in instructions therefore spans a range boundary; the bytes on
	///     either side of one are not adjacent in the image, so a pattern read across it is reading
	///     two places at once.
	/// </summary>
	public IEnumerable<Instruction> Decode(PeFunction function)
	{
		IReadOnlyList<PeFunction> ranges = RangesOf(function.Begin);
		if (ranges.Count == 0) ranges = new[] { function };
		foreach (PeFunction range in ranges)
		{
			foreach (Instruction ins in DecodeRange(range)) yield return ins;
		}
	}

	private IEnumerable<Instruction> DecodeRange(PeFunction range)
	{
		_image.TryFileOffset(range.Begin, out int offset);
		var reader = new ByteArrayCodeReader(_image.RawData, offset, (int) (range.End - range.Begin));
		ulong start = _image.ImageBase + range.Begin;
		ulong end = _image.ImageBase + range.End;
		Decoder decoder = Decoder.Create(64, reader, start);
		while (decoder.IP < end)
		{
			decoder.Decode(out Instruction ins);
			yield return ins;
		}
	}
}
