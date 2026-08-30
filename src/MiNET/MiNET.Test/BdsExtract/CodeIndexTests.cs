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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BdsExtract.Binary;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class CodeIndexTests
	{
		private static PeImage Fake(byte[] text)
		{
			var data = new byte[0x200 + text.Length];
			text.CopyTo(data, 0x200);
			var sections = new[] { new PeSection(".text", 0x1000, (uint) text.Length, 0x200, text.Length) };
			var functions = new[] { new PeFunction(0x1000, 0x1000 + (uint) text.Length) };
			return PeImage.FromSections(0x140000000, sections, functions, data);
		}

		[TestMethod]
		public void Rip_relative_operands_are_indexed_by_their_target()
		{
			// lea rax, [rip+0x10] ; movzx eax, word ptr [rip+0x20] ; ret
			byte[] text = { 0x48, 0x8D, 0x05, 0x10, 0, 0, 0, 0x0F, 0xB7, 0x05, 0x20, 0, 0, 0, 0xC3 };
			var index = new CodeIndex(Fake(text));

			// lea (7 bytes) at RVA 0x1000 targets 0x1000 + 7 + 0x10 = 0x1017.
			CollectionAssert.AreEqual(new[] { 0x1000u }, index.ReferencesTo(0x1017).ToArray());
			// movzx (7 bytes) at RVA 0x1007 targets 0x1007 + 7 + 0x20 = 0x102E.
			CollectionAssert.AreEqual(new[] { 0x1007u }, index.ReferencesTo(0x102E).ToArray());
			Assert.AreEqual(0, index.ReferencesTo(0x1234).Count);
			Assert.IsTrue(index.TryFunctionOf(0x1007, out PeFunction f) && f.Begin == 0x1000);
		}
	}
}
