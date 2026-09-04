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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Items;

namespace MiNET.Test.Items
{
	[TestClass]
	public class BlockItemIdentityTests
	{
		// These items name the block they place instead of carrying its pre-flattening id. A wrong
		// name resolves to null and the item places nothing, with no error, so the pairing is worth
		// pinning: an item whose block went missing is silent in every other way.

		[TestMethod]
		public void EachBlockItem_PlacesTheBlockItNames()
		{
			var expected = new Dictionary<ItemBlock, string>
			{
				[new ItemBed()] = "minecraft:bed",
				[new ItemCauldron()] = "minecraft:cauldron",
				[new ItemFrame()] = "minecraft:frame",
				[new ItemWheatSeeds()] = "minecraft:wheat",
				[new ItemBeetrootSeeds()] = "minecraft:beetroot"
			};

			foreach ((ItemBlock item, string blockName) in expected)
			{
				Assert.IsNotNull(item.Block, $"{item.GetType().Name} has no block, so it places nothing");
				Assert.AreEqual(blockName, item.Block.Name, $"{item.GetType().Name} places the wrong block");
			}
		}

		// A sign is two blocks and the face decides which, so both halves have to resolve.
		// The pairs are written out rather than derived from the class name. Oak is why: its blocks
		// are "minecraft:standing_sign" and "minecraft:wall_sign" with no wood in them at all, so any
		// rule that reads the wood off the class is a rule with an exception, and a test that infers
		// what it is checking cannot fail when the pairing itself is wrong.
		[TestMethod]
		public void SignVariants_ResolveBothHalves()
		{
			var expected = new Dictionary<ItemBlock, (string Standing, string Wall)>
			{
				[new ItemOakSign()] = ("minecraft:standing_sign", "minecraft:wall_sign"),
				[new ItemAcaciaSign()] = ("minecraft:acacia_standing_sign", "minecraft:acacia_wall_sign"),
				[new ItemSpruceSign()] = ("minecraft:spruce_standing_sign", "minecraft:spruce_wall_sign"),
				[new ItemBirchSign()] = ("minecraft:birch_standing_sign", "minecraft:birch_wall_sign"),
				[new ItemJungleSign()] = ("minecraft:jungle_standing_sign", "minecraft:jungle_wall_sign"),
				[new ItemDarkOakSign()] = ("minecraft:darkoak_standing_sign", "minecraft:darkoak_wall_sign"),
				[new ItemCrimsonSign()] = ("minecraft:crimson_standing_sign", "minecraft:crimson_wall_sign"),
				[new ItemWarpedSign()] = ("minecraft:warped_standing_sign", "minecraft:warped_wall_sign")
			};

			foreach ((ItemBlock sign, (string standing, string wall)) in expected)
			{
				string name = sign.GetType().Name;
				Assert.IsNotNull(MiNET.Blocks.BlockFactory.GetBlockByName(standing), $"{name} standing half {standing} missing");
				Assert.IsNotNull(MiNET.Blocks.BlockFactory.GetBlockByName(wall), $"{name} wall half {wall} missing");
			}
		}
	}
}
