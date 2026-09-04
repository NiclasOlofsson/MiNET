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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BlockGen;
using MiNET.Blocks;

namespace MiNET.Test
{
	/// <summary>
	///     The typed block definition model and the writer that turns it into the network NBT
	///     StartGame carries. The check is the captured BDS frame: one block built by hand from the
	///     wire tree has to serialize to the frame's own bytes, which is what pins the child order,
	///     the tag type of every leaf and the empty containers the wire still writes.
	/// </summary>
	[TestClass]
	public class BlockDefinitionWriterTests
	{
		private static string CaptureDirectory => Path.Combine(AppContext.BaseDirectory, "Data", "registry");

		[TestMethod]
		public void LightGrayConcreteStairs_WritesTheFrameBytes()
		{
			StartGameCapture frame = StartGameCapture.Read(Path.Combine(CaptureDirectory, "startgame-1.26.60.21.bin"));
			byte[] expected = frame.BlockProperties["minecraft:light_gray_concrete_stairs"];

			var definition = new BlockDefinition(
				Tags: ["minecraft:is_pickaxe_item_destructible"],
				MenuCategory: new BlockMenuCategory("construction", "minecraft:itemGroup.name.stairs", false),
				VanillaBlockData: new VanillaBlockData(1464, "solid", new StairArchetype("minecraft:light_gray_concrete"), true, null, false, false),
				Components: new BlockComponents(
					DestructibleByMining: 1.8f,
					DestructionParticles: new DestructionParticles(48, "", "none"),
					LiquidDetection: [new LiquidDetectionRule(true, "water", "blocking", 0, false)],
					RedstoneConductivity: new RedstoneConductivity(false, true),
					SupportShape: "stair"),
				Permutations: []);

			byte[] actual = BlockDefinitionWriter.ToNetworkBytes(BlockDefinitionWriter.Write(definition));

			CollectionAssert.AreEqual(expected, actual,
				$"expected {Convert.ToHexString(expected)}{Environment.NewLine}actual   {Convert.ToHexString(actual)}");
		}
	}
}
