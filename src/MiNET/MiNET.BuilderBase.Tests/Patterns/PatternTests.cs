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
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.BuilderBase.Patterns.Tests
{
	[TestClass]
	public class PatternTests
	{
		[TestMethod]
		public void Pattern_parse_empty_to_air_ok()
		{
			// setup
			string inputPattern = "";
			var pattern = new Pattern();
			pattern.Deserialize(null, inputPattern);

			// assert
			Assert.AreEqual(1, pattern.BlockList.Count);
			Assert.AreEqual(0, pattern.BlockList[0].Id);
			Assert.AreEqual(0, pattern.BlockList[0].Metadata);
			Assert.AreEqual(100, pattern.BlockList[0].Weight);
			Assert.AreEqual(100, pattern.BlockList[0].Accumulated);
		}

		[TestMethod]
		public void Pattern_parse_numeric_to_block_ok()
		{
			// setup
			string inputPattern = "1";
			var pattern = new Pattern();
			pattern.Deserialize(null, inputPattern);

			// assert
			Assert.AreEqual(1, pattern.BlockList.Count);
			Assert.AreEqual(1, pattern.BlockList[0].Id);
			Assert.AreEqual(0, pattern.BlockList[0].Metadata);
			Assert.AreEqual(100, pattern.BlockList[0].Weight);
			Assert.AreEqual(100, pattern.BlockList[0].Accumulated);
		}

		[TestMethod]
		public void Pattern_parse_basic_to_block_ok()
		{
			// setup
			string inputPattern = "stone";
			var pattern = new Pattern();
			pattern.Deserialize(null, inputPattern);

			// assert
			Assert.AreEqual(1, pattern.BlockList.Count);
			Assert.AreEqual(1, pattern.BlockList[0].Id);
			Assert.AreEqual(0, pattern.BlockList[0].Metadata);
			Assert.AreEqual(100, pattern.BlockList[0].Weight);
			Assert.AreEqual(100, pattern.BlockList[0].Accumulated);
		}

		[TestMethod]
		public void Pattern_parse_basic_with_meta_to_block_ok()
		{
			// setup
			string inputPattern = "stone:1";
			var pattern = new Pattern();
			pattern.Deserialize(null, inputPattern);

			// assert
			Assert.AreEqual(1, pattern.BlockList.Count);
			Assert.AreEqual(1, pattern.BlockList[0].Id);
			Assert.AreEqual(1, pattern.BlockList[0].Metadata);
			Assert.AreEqual(100, pattern.BlockList[0].Weight);
			Assert.AreEqual(100, pattern.BlockList[0].Accumulated);
		}

		[TestMethod]
		public void Pattern_parse_multiple_with_meta_to_blocks_ok()
		{
			// setup
			string inputPattern = "stone:1,dirt:1";
			var pattern = new Pattern();
			pattern.Deserialize(null, inputPattern);

			// assert
			Assert.AreEqual(2, pattern.BlockList.Count);

			Assert.AreEqual(1, pattern.BlockList[0].Id);
			Assert.AreEqual(1, pattern.BlockList[0].Metadata);
			Assert.AreEqual(100, pattern.BlockList[0].Weight);
			Assert.AreEqual(100, pattern.BlockList[0].Accumulated);

			Assert.AreEqual(3, pattern.BlockList[1].Id);
			Assert.AreEqual(1, pattern.BlockList[1].Metadata);
			Assert.AreEqual(100, pattern.BlockList[1].Weight);
			Assert.AreEqual(200, pattern.BlockList[1].Accumulated);
		}

		[TestMethod]
		public void Pattern_parse_multiple_with_weight_to_blocks_ok()
		{
			// setup
			string inputPattern = "1%stone:1,10%dirt:1";
			var pattern = new Pattern();
			pattern.Deserialize(null, inputPattern);

			// assert
			Assert.AreEqual(2, pattern.BlockList.Count);

			Assert.AreEqual(1, pattern.BlockList[0].Id);
			Assert.AreEqual(1, pattern.BlockList[0].Metadata);
			Assert.AreEqual(1, pattern.BlockList[0].Weight);
			Assert.AreEqual(1, pattern.BlockList[0].Accumulated);

			Assert.AreEqual(3, pattern.BlockList[1].Id);
			Assert.AreEqual(1, pattern.BlockList[1].Metadata);
			Assert.AreEqual(10, pattern.BlockList[1].Weight);
			Assert.AreEqual(11, pattern.BlockList[1].Accumulated);
		}

		[TestMethod]
		public void Pattern_parse_basic_with_blockstate_to_block_ok()
		{
			// setup
			string inputPattern = "stone_button[button_pressed_bit=1,facing_direction=3]";
			var pattern = new Pattern();
			pattern.Deserialize(null, inputPattern);

			// assert
			Assert.AreEqual(1, pattern.BlockList.Count);

			Assert.AreEqual(77, pattern.BlockList[0].Id);
			Assert.AreEqual(0, pattern.BlockList[0].Metadata);
			Assert.AreEqual(100, pattern.BlockList[0].Weight);
			Assert.AreEqual(100, pattern.BlockList[0].Accumulated);
		}
		[TestMethod]
		public void Pattern_parse_multiple_with_blockstate_to_blocks_ok()
		{
			// setup
			string inputPattern = "dirt,stone_button[ button_pressed_bit=1,facing_direction=3] , stone_slab[stone_slab_type=cobblestone,top_slot_bit=1],sand:1";
			var pattern = new Pattern();
			pattern.Deserialize(null, inputPattern);

			// assert
			Assert.AreEqual(4, pattern.BlockList.Count);

			Assert.AreEqual(3, pattern.BlockList[0].Id);
			Assert.AreEqual(0, pattern.BlockList[0].Metadata);
			Assert.AreEqual(100, pattern.BlockList[0].Weight);
			Assert.AreEqual(100, pattern.BlockList[0].Accumulated);

			Assert.AreEqual(77, pattern.BlockList[1].Id);
			Assert.AreEqual(0, pattern.BlockList[1].Metadata);
			Assert.AreEqual(100, pattern.BlockList[1].Weight);
			Assert.AreEqual(200, pattern.BlockList[1].Accumulated);

			Assert.AreEqual(44, pattern.BlockList[2].Id);
			Assert.AreEqual(0, pattern.BlockList[2].Metadata);
			Assert.AreEqual(100, pattern.BlockList[2].Weight);
			Assert.AreEqual(300, pattern.BlockList[2].Accumulated);

			Assert.AreEqual(12, pattern.BlockList[3].Id);
			Assert.AreEqual(1, pattern.BlockList[3].Metadata);
			Assert.AreEqual(100, pattern.BlockList[3].Weight);
			Assert.AreEqual(400, pattern.BlockList[3].Accumulated);
		}

		[TestMethod]
		public void Pattern_parse_multiple_all_with_blockstate_to_blocks_ok()
		{
			// setup
			string inputPattern = "log[old_log_type=spruce,pillar_axis=x],log[old_log_type=spruce,pillar_axis=y],log[old_log_type=spruce,pillar_axis=z]";
			var pattern = new Pattern();
			pattern.Deserialize(null, inputPattern);

			// assert
			Assert.AreEqual(3, pattern.BlockList.Count);

			Assert.AreEqual(17, pattern.BlockList[0].Id);
			Assert.AreEqual(0, pattern.BlockList[0].Metadata);
			Assert.AreEqual(100, pattern.BlockList[0].Weight);
			Assert.AreEqual(100, pattern.BlockList[0].Accumulated);
			Assert.IsTrue(pattern.BlockList[0].HasBlockStates);
			Assert.AreEqual(2, pattern.BlockList[0].BlockStates.Count);
			Assert.AreEqual("old_log_type", pattern.BlockList[0].BlockStates.First().Name);
			Assert.AreEqual("spruce", pattern.BlockList[0].BlockStates.First().Value);
			Assert.AreEqual("pillar_axis", pattern.BlockList[0].BlockStates.Last().Name);
			Assert.AreEqual("x", pattern.BlockList[0].BlockStates.Last().Value);

			Assert.AreEqual(17, pattern.BlockList[1].Id);
			Assert.AreEqual(0, pattern.BlockList[1].Metadata);
			Assert.AreEqual(100, pattern.BlockList[1].Weight);
			Assert.AreEqual(200, pattern.BlockList[1].Accumulated);
			Assert.IsTrue(pattern.BlockList[1].HasBlockStates);
			Assert.AreEqual(2, pattern.BlockList[1].BlockStates.Count);
			Assert.AreEqual("old_log_type", pattern.BlockList[1].BlockStates.First().Name);
			Assert.AreEqual("pillar_axis", pattern.BlockList[1].BlockStates.Last().Name);

			Assert.AreEqual(17, pattern.BlockList[2].Id);
			Assert.AreEqual(0, pattern.BlockList[2].Metadata);
			Assert.AreEqual(100, pattern.BlockList[2].Weight);
			Assert.AreEqual(300, pattern.BlockList[2].Accumulated);
			Assert.IsTrue(pattern.BlockList[2].HasBlockStates);
			Assert.AreEqual(2, pattern.BlockList[2].BlockStates.Count);
			Assert.AreEqual("old_log_type", pattern.BlockList[2].BlockStates.First().Name);
			Assert.AreEqual("pillar_axis", pattern.BlockList[2].BlockStates.Last().Name);

			Log block = pattern.Next(BlockCoordinates.North) as Log;
			Assert.IsNotNull(block);
			Assert.IsInstanceOfType(block, typeof(Log));
			Assert.AreEqual("spruce", block.OldLogType);
		}

		[TestMethod]
		public void RegexTest()
		{
			//string inputPattern = "trap|stone_button{button_pressed_bit=1|facing_direction=3}|stone_face{button_pressed_bit=1|facing_direction=3}";
			//var patternsEx = new Regex(@"\|(?![^{]*})");
			string inputPattern = "dirt,stone_button[button_pressed_bit=1,facing_direction=3],stone_face[button_pressed_bit=1,facing_direction=3],sand";
			var patternsEx = new Regex(@",(?![^\[]*])");
			var patterns = patternsEx.Split(inputPattern);

			foreach (string pattern in patterns)
			{
				Console.WriteLine($"Pattern:{pattern}");
			}
		}

	}
}