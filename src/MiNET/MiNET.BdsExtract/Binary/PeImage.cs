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
using System.IO;
using System.Linq;

namespace MiNET.BdsExtract.Binary;

/// <summary>
///     Opens a PE image (bedrock_server.exe) and exposes what the binary facts phase needs: the
///     image base, the section table, the function ranges from the exception directory (.pdata),
///     RVA to file offset conversion, and raw byte views. Parsing the headers themselves is
///     AsmResolver.PE's job (<see cref="AsmResolver.PE.PEImage" />); this class keeps its own copy
///     of the file bytes so <see cref="Bytes" /> and <see cref="RawData" /> can hand out spans into
///     one buffer without a second read per call, and so CodeIndex can decode straight from it.
/// </summary>
public sealed class PeImage
{
	private readonly byte[] _data;

	private PeImage(byte[] data, ulong imageBase, IReadOnlyList<PeSection> sections, IReadOnlyList<PeFunction> functions, uint entryPoint, IReadOnlyDictionary<uint, string> imports, IReadOnlyList<PeExport> exports, IReadOnlyList<uint> tlsCallbacks, uint guardDispatchSlot)
	{
		_data = data;
		ImageBase = imageBase;
		Sections = sections;
		Functions = functions;
		EntryPoint = entryPoint;
		Imports = imports;
		Exports = exports;
		TlsCallbacks = tlsCallbacks;
		GuardDispatchSlot = guardDispatchSlot;
	}

	/// <summary>
	///     RVA of the Control Flow Guard dispatch slot (the load config's GuardCFDispatchFunctionPointer):
	///     every guarded indirect call or tail jump loads its dispatcher from here with the real target
	///     in rax. 0 when the image has no load config or no guard.
	/// </summary>
	public uint GuardDispatchSlot { get; }

	public ulong ImageBase { get; }

	/// <summary>Sections, in the order the section table declares them.</summary>
	public IReadOnlyList<PeSection> Sections { get; }

	/// <summary>
	///     Function ranges read from the exception directory (.pdata), sorted by Begin. One entry per
	///     RUNTIME_FUNCTION, so a function split into a hot and a cold part appears twice: the cold
	///     range's unwind info chains to the primary, and <see cref="PeFunction.Parent" /> carries that
	///     primary's Begin. An entry with Parent 0 is a function start; one with a Parent is a range
	///     of the function that starts there, never a function of its own.
	/// </summary>
	public IReadOnlyList<PeFunction> Functions { get; }

	/// <summary>The RVA the loader jumps to (OptionalHeader.AddressOfEntryPoint).</summary>
	public uint EntryPoint { get; }

	/// <summary>Import address table slots by RVA, each named "MODULE.dll!Symbol" (or "MODULE.dll!#ordinal").</summary>
	public IReadOnlyDictionary<uint, string> Imports { get; }

	/// <summary>Exported symbols that resolve to code in this image (forwarders excluded).</summary>
	public IReadOnlyList<PeExport> Exports { get; }

	/// <summary>TLS callback RVAs, empty when the image has no TLS directory.</summary>
	public IReadOnlyList<uint> TlsCallbacks { get; }

	public PeSection Text => Sections.First(s => s.Name == ".text");

	/// <summary>
	///     The file's own bytes, for a consumer (CodeIndex) that wants to decode straight from the
	///     image's buffer instead of copying a section into a new array.
	/// </summary>
	internal byte[] RawData => _data;

	public static PeImage Open(string path)
	{
		byte[] data = File.ReadAllBytes(path);
		AsmResolver.PE.PEImage image = AsmResolver.PE.PEImage.FromFile(path);

		var sections = new List<PeSection>();
		foreach (AsmResolver.PE.File.PESection section in image.PEFile.Sections)
		{
			sections.Add(new PeSection(section.Name.ToString(), section.Rva, section.GetVirtualSize(), (int) section.Offset, (int) section.GetPhysicalSize()));
		}

		var functions = new List<PeFunction>();
		var chainedTo = new Dictionary<uint, uint>();
		if (image.Exceptions != null)
		{
			var probe = new PeImage(data, image.ImageBase, sections, Array.Empty<PeFunction>(), 0, null, null, null, 0);
			foreach (AsmResolver.PE.Exceptions.IRuntimeFunction function in image.Exceptions.GetFunctions())
			{
				uint begin = function.Begin.Rva;
				uint end = function.End.Rva;
				if (begin == 0 && end == 0) continue;
				uint handler = 0;
				if (function is AsmResolver.PE.Exceptions.X64RuntimeFunction { UnwindInfo: { } unwind })
				{
					// UNWIND_INFO: byte 0 is version:3 | flags:5, byte 2 the unwind code count, then the
					// codes as 16-bit slots padded to an even count, then one 32-bit slot that holds the
					// handler RVA (flags EHANDLER/UHANDLER) or the chained RUNTIME_FUNCTION (flag
					// CHAININFO). AsmResolver's X64UnwindInfo places that slot wrongly on some entries,
					// so it is read from the bytes here.
					ReadOnlySpan<byte> head = probe.Bytes(unwind.Rva, 4);
					int flags = head[0] >> 3;
					int codes = head[2] + (head[2] & 1);
					uint slotRva = unwind.Rva + 4 + 2 * (uint) codes;
					if ((flags & 4) != 0) chainedTo[begin] = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(probe.Bytes(slotRva, 4));
					else if ((flags & 3) != 0) handler = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(probe.Bytes(slotRva, 4));
				}
				functions.Add(new PeFunction(begin, end, 0, handler));
			}
		}
		for (int i = 0; i < functions.Count; i++)
		{
			// A chain can itself chain; the parent is the entry at the end of it.
			uint parent = functions[i].Begin;
			int hops = 0;
			while (chainedTo.TryGetValue(parent, out uint next) && hops++ < 64) parent = next;
			if (parent != functions[i].Begin) functions[i] = functions[i] with { Parent = parent };
		}
		functions.Sort((a, b) => a.Begin.CompareTo(b.Begin));

		var imports = new Dictionary<uint, string>();
		foreach (AsmResolver.PE.Imports.ImportedModule module in image.Imports)
		{
			foreach (AsmResolver.PE.Imports.ImportedSymbol symbol in module.Symbols)
			{
				if (symbol.AddressTableEntry == null) continue;
				imports[symbol.AddressTableEntry.Rva] = $"{module.Name}!{(symbol.IsImportByName ? symbol.Name : "#" + symbol.Ordinal)}";
			}
		}

		var exports = new List<PeExport>();
		if (image.Exports != null)
		{
			foreach (AsmResolver.PE.Exports.ExportedSymbol symbol in image.Exports.Entries)
			{
				if (symbol.IsForwarder || symbol.Address == null) continue;
				exports.Add(new PeExport(symbol.Address.Rva, symbol.IsByName ? symbol.Name : "#" + symbol.Ordinal));
			}
		}

		var tlsCallbacks = new List<uint>();
		if (image.TlsDirectory != null)
		{
			foreach (AsmResolver.ISegmentReference callback in image.TlsDirectory.CallbackFunctions)
			{
				if (callback != null && callback.Rva != 0) tlsCallbacks.Add(callback.Rva);
			}
		}

		// IMAGE_LOAD_CONFIG_DIRECTORY64: GuardCFDispatchFunctionPointer is the VA at +0x78, present
		// when the directory's own Size field (at +0) reaches past it.
		uint guardDispatchSlot = 0;
		AsmResolver.PE.File.DataDirectory loadConfig = image.PEFile.OptionalHeader.DataDirectories[10];
		if (loadConfig.VirtualAddress != 0)
		{
			var probe = new PeImage(data, image.ImageBase, sections, Array.Empty<PeFunction>(), 0, null, null, null, 0);
			uint size = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(probe.Bytes(loadConfig.VirtualAddress, 4));
			if (size >= 0x80)
			{
				ulong dispatcher = System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(probe.Bytes(loadConfig.VirtualAddress + 0x78, 8));
				if (dispatcher != 0) guardDispatchSlot = (uint) (dispatcher - image.ImageBase);
			}
		}

		return new PeImage(data, image.ImageBase, sections, functions, image.PEFile.OptionalHeader.AddressOfEntryPoint, imports, exports, tlsCallbacks, guardDispatchSlot);
	}

	/// <summary>Builds a PeImage from hand-supplied sections and functions, for tests that assemble a fake .text without a real exe on disk.</summary>
	public static PeImage FromSections(ulong imageBase, IReadOnlyList<PeSection> sections, IReadOnlyList<PeFunction> functions, byte[] data, uint entryPoint = 0, IReadOnlyDictionary<uint, string> imports = null, IReadOnlyList<PeExport> exports = null, IReadOnlyList<uint> tlsCallbacks = null, uint guardDispatchSlot = 0)
	{
		return new PeImage(data, imageBase, sections, functions, entryPoint, imports ?? new Dictionary<uint, string>(), exports ?? Array.Empty<PeExport>(), tlsCallbacks ?? Array.Empty<uint>(), guardDispatchSlot);
	}

	public bool TryFileOffset(uint rva, out int offset)
	{
		foreach (PeSection section in Sections)
		{
			if (rva >= section.VirtualAddress && rva < section.VirtualAddress + Math.Max(section.VirtualSize, (uint) section.RawSize))
			{
				offset = section.RawOffset + (int) (rva - section.VirtualAddress);
				return true;
			}
		}
		offset = 0;
		return false;
	}

	/// <summary>A view of the file's own bytes at the given RVA. Throws when the range leaves the section it started in.</summary>
	public ReadOnlySpan<byte> Bytes(uint rva, int length)
	{
		if (!TryFileOffset(rva, out int offset) || !TryFileOffset(rva + (uint) length - 1, out _)) throw new ArgumentOutOfRangeException(nameof(rva), $"0x{rva:X} + {length} leaves the section it started in");
		return new ReadOnlySpan<byte>(_data, offset, length);
	}
}

public readonly record struct PeSection(string Name, uint VirtualAddress, uint VirtualSize, int RawOffset, int RawSize);

/// <summary>
///     One .pdata range. Parent is 0 for a function's primary entry and the primary's Begin for a
///     chained (cold-split) range; Handler is the RVA of the language-specific exception handler,
///     0 when the unwind info has none.
/// </summary>
public readonly record struct PeFunction(uint Begin, uint End, uint Parent = 0, uint Handler = 0);

public readonly record struct PeExport(uint Rva, string Name);
