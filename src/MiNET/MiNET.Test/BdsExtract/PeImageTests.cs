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
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BdsExtract.Binary;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class PeImageTests
	{
		internal static string ExePath()
		{
			string path = Environment.GetEnvironmentVariable("MINET_BDS_EXE");
			if (string.IsNullOrEmpty(path) || !File.Exists(path)) Assert.Inconclusive("set MINET_BDS_EXE to a bedrock_server.exe");
			return path;
		}

		[TestMethod]
		public void Sections_and_functions_come_from_the_headers()
		{
			var image = PeImage.Open(ExePath());
			Assert.AreEqual(".text", image.Text.Name);
			Assert.IsTrue(image.Sections.Any(s => s.Name == ".pdata"));
			Assert.IsTrue(image.Functions.Count > 100_000, $"only {image.Functions.Count} functions");
			Assert.IsTrue(image.Functions.Zip(image.Functions.Skip(1)).All(p => p.First.Begin < p.Second.Begin), "not sorted");
			Assert.IsTrue(image.TryFileOffset(image.Text.VirtualAddress, out int offset) && offset == image.Text.RawOffset);
		}

		[TestMethod]
		public void Chained_entries_name_a_primary_parent_and_the_other_seeds_lie_in_text()
		{
			var image = PeImage.Open(ExePath());
			uint textStart = image.Text.VirtualAddress;
			uint textEnd = textStart + (uint) image.Text.RawSize;
			bool InText(uint rva) => rva >= textStart && rva < textEnd;

			var primaries = new HashSet<uint>(image.Functions.Where(f => f.Parent == 0).Select(f => f.Begin));
			var chained = image.Functions.Where(f => f.Parent != 0).ToList();
			Assert.IsTrue(chained.Count > 0, "no chained unwind entries");
			Assert.IsTrue(chained.All(f => primaries.Contains(f.Parent)), "a chained entry names a parent that is not itself a primary entry");

			var handlers = image.Functions.Where(f => f.Handler != 0).Select(f => f.Handler).Distinct().ToList();
			Assert.IsTrue(handlers.Count > 0, "no exception handlers");
			string SectionOf(uint rva) => image.Sections.FirstOrDefault(s => rva >= s.VirtualAddress && rva < s.VirtualAddress + s.VirtualSize).Name ?? "none";
			var outside = handlers.Where(h => !InText(h)).ToList();
			Assert.IsTrue(outside.Count == 0, $"{outside.Count} of {handlers.Count} handlers outside .text: {string.Join(", ", outside.Take(5).Select(h => $"0x{h:X} in {SectionOf(h)}"))}");

			Assert.IsTrue(InText(image.EntryPoint), $"entry point 0x{image.EntryPoint:X} outside .text");
			Assert.IsTrue(image.Imports.Count > 100, $"only {image.Imports.Count} imports");
			Assert.IsTrue(image.Imports.Values.Contains("api-ms-win-crt-runtime-l1-1-0.dll!_exit"), "_exit not among the imports");
			Assert.IsTrue(image.Imports.Keys.All(iat => !InText(iat)), "an IAT slot lies inside .text");
			// BDS exports only its build stamp constants (data in .rdata), so exports are not required to be code, only to be inside the image.
			Assert.IsTrue(image.Exports.Count > 0 && image.Exports.All(e => SectionOf(e.Rva) != "none"), "an export lies in no section");
			Assert.IsTrue(image.TlsCallbacks.All(InText), "a TLS callback lies outside .text");

			// Control Flow Guard: the load config names the slot every guarded indirect call and
			// tail jump loads its dispatcher from; the slot holds the dispatcher's address.
			Assert.AreNotEqual(0u, image.GuardDispatchSlot, "no CFG dispatch slot");
			Assert.AreEqual(".rdata", SectionOf(image.GuardDispatchSlot));
			ulong dispatcher = System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(image.Bytes(image.GuardDispatchSlot, 8));
			Assert.IsTrue(InText((uint) (dispatcher - image.ImageBase)), $"dispatcher 0x{dispatcher:X} outside .text");

			Console.WriteLine($"entries {image.Functions.Count}, primaries {primaries.Count}, chained {chained.Count}, handlers {handlers.Count}, imports {image.Imports.Count}, exports {image.Exports.Count}, tls {image.TlsCallbacks.Count}");
		}
	}
}
