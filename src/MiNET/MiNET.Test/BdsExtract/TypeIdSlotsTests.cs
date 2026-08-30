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
using Newtonsoft.Json.Linq;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class TypeIdSlotsTests
	{
		/// <summary>The repo's own scratch folder, found by walking up from the test binary.</summary>
		internal static string RepoFile(string relative)
		{
			var directory = new DirectoryInfo(AppContext.BaseDirectory);
			while (directory != null && !File.Exists(Path.Combine(directory.FullName, relative))) directory = directory.Parent;
			return directory == null ? null : Path.Combine(directory.FullName, relative);
		}

		[TestMethod]
		public void Each_block_component_class_resolves_to_one_16bit_slot()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			List<InventoryClass> classes = SignatureStrings.Find(image).Where(c => c.Family == "BlockComponentStorage").ToList();
			IReadOnlyList<TypeIdSlot> slots = TypeIdSlots.Find(image, code, classes);

			List<TypeIdSlot> resolved = slots.Where(s => s.Witnesses > 0).ToList();
			foreach (TypeIdSlot slot in slots.Where(s => s.Witnesses == 0))
			{
				Console.WriteLine($"unresolved: {slot.Name} candidates [{string.Join(", ", slot.Candidates.Select(c => "0x" + c.ToString("x")))}]");
			}
			Assert.IsTrue(resolved.Count >= 58, $"{resolved.Count} resolved of {slots.Count}");
			Assert.AreEqual(resolved.Count, resolved.Select(s => s.SlotRva).Distinct().Count(), "two classes share a slot");
		}

		/// <summary>
		///     The same 58 slots the Python scan under temp_auto/compid found, address for address.
		///     Its values are already RVAs: each one lands inside a section of the image on disk,
		///     which a process address never does, so nothing is subtracted here. Reading them as
		///     process addresses off the image base is tried as well and reported, so the claim is
		///     measured rather than assumed.
		/// </summary>
		[TestMethod]
		public void The_slots_agree_with_the_python_scan()
		{
			string path = RepoFile(Path.Combine("temp_auto", "compid", "ids.json"));
			if (path == null) Assert.Inconclusive("temp_auto/compid/ids.json is not in this checkout");

			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			List<InventoryClass> classes = SignatureStrings.Find(image).Where(c => c.Family == "BlockComponentStorage").ToList();
			List<TypeIdSlot> resolved = TypeIdSlots.Find(image, code, classes).Where(s => s.Witnesses > 0).ToList();

			var python = new Dictionary<string, uint>();
			foreach (JProperty property in JObject.Parse(File.ReadAllText(path)).Properties())
			{
				python[property.Name] = Convert.ToUInt32((string) property.Value[0], 16);
			}

			bool InSection(ulong value) => image.Sections.Any(s => value >= s.VirtualAddress && value < s.VirtualAddress + Math.Max(s.VirtualSize, (uint) s.RawSize));
			int asRva = python.Values.Count(v => InSection(v));
			int asProcess = python.Values.Count(v => v >= image.ImageBase && InSection(v - image.ImageBase));
			Console.WriteLine($"ids.json: {python.Count} entries, {asRva} land in a section as RVAs, {asProcess} as process addresses off 0x{image.ImageBase:x}");
			Assert.AreEqual(python.Count, asRva, "the scan's values are not RVAs of this image");

			var differences = new List<string>();
			foreach (TypeIdSlot slot in resolved)
			{
				if (!python.TryGetValue(slot.Name, out uint scanned)) differences.Add($"{slot.Name}: not in ids.json");
				else if (scanned != slot.SlotRva) differences.Add($"{slot.Name}: binary 0x{slot.SlotRva:x}, python 0x{scanned:x}");
			}
			foreach (string name in python.Keys.Where(n => resolved.All(s => s.Name != n))) differences.Add($"{name}: in ids.json, unresolved from the binary");
			foreach (string difference in differences) Console.WriteLine(difference);
			Assert.AreEqual(0, differences.Count, "the binary and the python scan disagree");
		}
	}
}
