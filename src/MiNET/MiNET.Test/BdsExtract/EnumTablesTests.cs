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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BdsExtract.Binary;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class EnumTablesTests
	{
		[TestMethod]
		public void UseAnimation_names_come_from_the_serializer_and_agree_below_six()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			var seeds = new[] { ("UseAnimation", (IReadOnlyList<string>) new[] { "none", "eat", "drink", "block", "bow", "camera" }) };
			EnumTable table = EnumTables.Find(code, image, seeds).Single(t => t.Name == "UseAnimation");
			Assert.IsTrue(table.Accepted, table.Evidence);
			Assert.AreEqual("none", table.Values[0]);
			Assert.AreEqual("eat", table.Values[1]);
			Assert.AreEqual("camera", table.Values[5]);
			Assert.IsTrue(table.Values.ContainsKey(6), "the table must settle what 6 is");
			Console.WriteLine(string.Join(", ", table.Values.OrderBy(v => v.Key).Select(v => $"{v.Key}={v.Value}")));
		}

		/// <summary>
		///     The criterion, exercised on the largest table the reference states. LevelSoundEvent is
		///     570 values wide and sparse, and its declaration order is not its value order, so a
		///     table that reproduces every one of them is read from the code rather than assumed.
		/// </summary>
		[TestMethod]
		public void LevelSoundEvent_reproduces_every_value_the_reference_states()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			var known = new Dictionary<int, string> { [0] = "item.use.on", [221] = "step.baby", [314] = "record.pigstep", [462] = "brush", [614] = "undefined" };
			EnumTable table = EnumTables.Find(code, image, new[] { new EnumSeed("LevelSoundEvent", known) }).Single();
			Assert.IsTrue(table.Accepted, table.Evidence);
			Assert.AreEqual("step.baby", table.Values[221]);
			Assert.AreEqual("item.trident.thunder", table.Values[184]);
			Assert.IsTrue(table.Values.Count > 500, $"only {table.Values.Count} values");
		}

		/// <summary>
		///     A seed whose names are not in the binary has to come back as not found with its
		///     reason, never as a table. TintMethod's names are the class's own, and the code
		///     carries the wire spellings instead.
		/// </summary>
		[TestMethod]
		public void An_enum_whose_names_are_not_in_the_binary_is_reported_not_found()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			var known = new Dictionary<int, string> { [0] = "None", [1] = "DefaultFoliage", [2] = "BirchFoliage", [3] = "EvergreenFoliage", [4] = "DryFoliage", [5] = "Grass", [6] = "Water", [7] = "Stem", [8] = "RedStoneWire" };
			EnumTable table = EnumTables.Find(code, image, new[] { new EnumSeed("TintMethod", known) }).Single();
			Assert.IsFalse(table.Accepted);
			Assert.AreEqual(0, table.Values.Count);
			Assert.IsTrue(table.Evidence.Length > 0, "a rejection has to say why");
			Console.WriteLine(table.Evidence);
		}
	}
}