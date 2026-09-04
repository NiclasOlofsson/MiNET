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
using fNbt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BlockGen;
using MiNET.Blocks;
using MiNET.Net;

namespace MiNET.Test
{
	/// <summary>
	///     What the server puts into StartGame for the data-driven blocks has to be what vanilla
	///     puts there, because the client draws those blocks from nothing else. The generator proves
	///     the trees it emits; this proves the compiled result the server actually sends: the
	///     emitted construction code, the writer, and the experiments the server declares.
	/// </summary>
	[TestClass]
	public class StartGameCaptureTests
	{
		private static string CapturePath => Path.Combine(AppContext.BaseDirectory, "Data", "registry", "startgame-1.26.50.26.bin");

		[TestMethod]
		public void BlockProperties_EqualTheCapture()
		{
			StartGameCapture capture = StartGameCapture.Read(CapturePath);
			List<ServerBlockProperty> sent = BlockDefinitions.ServerBlockProperties();

			CollectionAssert.AreEquivalent(capture.BlockOrder.ToList(), sent.Select(p => p.blockName).ToList(), "the set of blocks with a definition");
			foreach (ServerBlockProperty property in sent)
			{
				byte[] bytes = BlockDefinitionWriter.ToNetworkBytes((NbtCompound) property.blockDefinition.NbtFile.RootTag);
				CollectionAssert.AreEqual(capture.BlockProperties[property.blockName], bytes, property.blockName);
			}

			// The order is a catalogued divergence, like the item registry's: BDS sends its map
			// order, MiNET the palette's. Content is what the client interprets; the order is
			// reported here, never fitted.
			List<string> order = sent.Select(p => p.blockName).ToList();
			if (!order.SequenceEqual(capture.BlockOrder))
			{
				Console.WriteLine($"blockProperties order differs from the frame: MiNET starts {order[0]}, BDS starts {capture.BlockOrder[0]}");
			}
		}

		[TestMethod]
		public void Experiments_EqualTheCapture()
		{
			StartGameCapture capture = StartGameCapture.Read(CapturePath);
			Experiments sent = Experiments.Vanilla();

			CollectionAssert.AreEqual(capture.Experiments.Select(e => e.Name).ToList(), sent.Select(e => e.Name).ToList());
			CollectionAssert.AreEqual(capture.Experiments.Select(e => e.Enabled).ToList(), sent.Select(e => e.Enabled).ToList());
			Assert.AreEqual(capture.ExperimentsEverToggled, sent.ExperimentsEverToggled);
		}
	}
}
