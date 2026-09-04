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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Items;

namespace MiNET.Test.Items
{
	[TestClass]
	public class BucketTests
	{
		// An item resolves by registry name to a typed class, and the class is what makes a bucket
		// pour or scoop. A bucket that resolves to a plain Item has a no-op PlaceBlock, so no
		// liquid ever enters the world from a player's hand and nothing flows. The liquids are
		// registry identities of their own (water_bucket, lava_bucket), never a metadata on the
		// empty bucket.

		[TestMethod]
		public void WaterBucket_PoursFlowingWater()
		{
			Item bucket = ItemFactory.GetItemByName("minecraft:water_bucket");

			Assert.IsInstanceOfType(bucket, typeof(ItemLiquidBucket));
			Assert.AreEqual("minecraft:flowing_water", ((ItemLiquidBucket) bucket).Liquid().Name);
		}

		[TestMethod]
		public void LavaBucket_PoursFlowingLava()
		{
			Item bucket = ItemFactory.GetItemByName("minecraft:lava_bucket");

			Assert.IsInstanceOfType(bucket, typeof(ItemLiquidBucket));
			Assert.AreEqual("minecraft:flowing_lava", ((ItemLiquidBucket) bucket).Liquid().Name);
		}

		[TestMethod]
		public void EmptyBucket_Scoops()
		{
			Assert.IsInstanceOfType(ItemFactory.GetItemByName("minecraft:bucket"), typeof(ItemBucket));
		}

		// What a bucket pours is a source: the flow tick reads the depth, and anything but 0 decays
		// away instead of spreading.
		[TestMethod]
		public void PouredLiquid_IsASource()
		{
			Assert.AreEqual(0, new ItemWaterBucket().Liquid().LiquidDepth);
			Assert.AreEqual(0, new ItemLavaBucket().Liquid().LiquidDepth);
		}
	}
}