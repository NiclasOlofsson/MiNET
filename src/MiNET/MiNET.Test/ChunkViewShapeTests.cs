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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Utils.Vectors;

namespace MiNET.Test
{
	/// <summary>
	///     The view shape is vanilla's: the square of the chunk radius cut to the circle of
	///     radius r + 1/2 + sqrt(3) around the player's chunk (BDS GridArea::_fill). The server
	///     streams it, the client gates and forgets by it, and the two must agree exactly or the
	///     client waits for columns that never come. The counts are what BDS loaded for a bot at
	///     each radius on 1.26.60.21; every column it sent lay inside them.
	/// </summary>
	[TestClass]
	public class ChunkViewShapeTests
	{
		private static int Count(int radius)
		{
			var center = new ChunkCoordinates(6, 18);
			int count = 0;
			for (int x = -radius - 2; x <= radius + 2; x++)
			for (int z = -radius - 2; z <= radius + 2; z++)
				if (new ChunkCoordinates(center.X + x, center.Z + z).IsWithinView(center, radius)) count++;
			return count;
		}

		[TestMethod]
		public void View_matches_the_columns_bds_loads_at_each_radius()
		{
			Assert.AreEqual(165, Count(6));
			Assert.AreEqual(277, Count(8));
			Assert.AreEqual(577, Count(12));
			Assert.AreEqual(969, Count(16));
		}

		[TestMethod]
		public void View_reaches_the_full_radius_on_the_axes_and_cuts_the_corners()
		{
			var center = new ChunkCoordinates(0, 0);

			Assert.IsTrue(new ChunkCoordinates(12, 0).IsWithinView(center, 12));
			Assert.IsTrue(new ChunkCoordinates(12, 7).IsWithinView(center, 12), "193 < 202.6");
			Assert.IsTrue(new ChunkCoordinates(7, 12).IsWithinView(center, 12), "the column a plain disc of 12 would drop, and BDS sends");

			Assert.IsFalse(new ChunkCoordinates(12, 8).IsWithinView(center, 12), "208 > 202.6");
			Assert.IsFalse(new ChunkCoordinates(12, 12).IsWithinView(center, 12), "the corner");
			Assert.IsFalse(new ChunkCoordinates(13, 0).IsWithinView(center, 12), "the square cap");
		}
	}
}