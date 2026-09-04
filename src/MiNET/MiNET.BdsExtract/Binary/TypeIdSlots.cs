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

using System.Collections.Generic;
using System.Linq;
using Iced.Intel;

namespace MiNET.BdsExtract.Binary;

/// <summary>
///     The static that holds one class's Bedrock::BasicTypeId, as an RVA of the image on disk.
/// </summary>
/// <param name="Witnesses">
///     How many of the class's own functions read that same static. Zero means the class did not
///     resolve, and <see cref="Candidates" /> then holds every static its functions did read.
/// </param>
public sealed record TypeIdSlot(string Name, string Family, uint SlotRva, int Witnesses, IReadOnlyList<uint> Candidates);

/// <summary>
///     Where each component class's type id lives.
///
///     Bedrock hands every component type a 16-bit id the first time something asks for it, and
///     keeps it in a static beside the templated accessor. The accessor is the function that
///     carries the class's clang signature literal, so the literal locates the function and the
///     function's one "movzx r32, word ptr [rip+disp]" locates the static: the id is read from
///     there, not computed. The id itself is not in the image, only the place it will be written,
///     which is why the slot is an RVA and the value is a live read.
///
///     Two things separate a find from a coincidence here. The static must sit in a writable data
///     section, because a value assigned at runtime cannot live in read-only data. And every one of
///     the class's own functions must read the same static: the agreeing functions are the
///     witnesses, and a class whose functions disagree is emitted unresolved with all its
///     candidates rather than resolved to whichever won.
/// </summary>
public static class TypeIdSlots
{
	/// <summary>
	///     The sections a runtime-assigned id can live in. The id is written after the image loads,
	///     so .rdata is excluded by what the value is, not by preference; .text is excluded because
	///     it holds code.
	/// </summary>
	private static readonly string[] WritableData = [".data", ".bss", ".tls"];

	public static IReadOnlyList<TypeIdSlot> Find(PeImage image, CodeIndex code, IReadOnlyList<InventoryClass> classes)
	{
		var writable = image.Sections.Where(s => WritableData.Contains(s.Name)).ToList();

		var result = new List<TypeIdSlot>();
		foreach (InventoryClass entry in classes)
		{
			// One function can carry several of the class's literals (an assertion and a message in
			// the same body), so the functions are counted once each and a function is one witness.
			var functions = new HashSet<uint>();
			foreach (uint stringRva in entry.SignatureRvas)
			{
				foreach (uint site in code.ReferencesTo(stringRva))
				{
					if (code.TryFunctionOf(site, out PeFunction function)) functions.Add(function.Begin);
				}
			}

			var votes = new Dictionary<uint, int>();
			foreach (uint begin in functions)
			{
				if (!code.TryFunctionOf(begin, out PeFunction function)) continue;
				var inThisFunction = new HashSet<uint>();
				foreach (Instruction ins in code.Decode(function))
				{
					if (ins.Mnemonic != Mnemonic.Movzx || ins.MemorySize != MemorySize.UInt16 || !ins.IsIPRelativeMemoryOperand) continue;
					uint slot = (uint) (ins.IPRelativeMemoryAddress - code.ImageBase);
					if (!writable.Any(s => slot >= s.VirtualAddress && slot < s.VirtualAddress + s.VirtualSize)) continue;
					inThisFunction.Add(slot);
				}
				foreach (uint slot in inThisFunction) votes[slot] = votes.GetValueOrDefault(slot) + 1;
			}

			List<uint> candidates = votes.Keys.OrderBy(s => s).ToList();
			result.Add(votes.Count == 1
				? new TypeIdSlot(entry.Name, entry.Family, candidates[0], votes[candidates[0]], candidates)
				: new TypeIdSlot(entry.Name, entry.Family, 0, 0, candidates));
		}

		return result;
	}
}
