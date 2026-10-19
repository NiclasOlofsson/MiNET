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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson.
// All Rights Reserved.

#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Entities.Projectiles;
using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Test
{
	/// <summary>
	///     The bow as BDS 1.26.70 fires it: the draw curve and its cut-off from the bow's release
	///     override, the arrow's speed and spread from the component group a player's shot adds.
	/// </summary>
	[TestClass]
	public class BowTests
	{
		// BDS fires from a force of 0.1, which the draw curve first reaches on the third tick.
		[TestMethod]
		public void Bow_FiresFromTheThirdTickOfDraw()
		{
			Assert.IsTrue(ItemBow.CalculateForce(2) < 0.1f);
			Assert.IsTrue(ItemBow.CalculateForce(3) >= 0.1f);
		}

		// A full draw is one second; holding longer adds nothing.
		[TestMethod]
		public void Bow_ReachesFullForceAtTwentyTicks()
		{
			Assert.IsTrue(ItemBow.CalculateForce(19) < 1f);
			Assert.AreEqual(1f, ItemBow.CalculateForce(20));
			Assert.AreEqual(1f, ItemBow.CalculateForce(200));
		}

		// minecraft:player_arrow: 3 blocks a tick at full draw, a spread of 1 at every difficulty.
		[TestMethod]
		public void PlayerArrow_FliesAtThreeWithASpreadOfOne()
		{
			var arrow = new Arrow(new Player(null, null), null);
			Assert.AreEqual(3, arrow.Power);
			Assert.AreEqual(1, arrow.Uncertainty(Difficulty.Peaceful));
			Assert.AreEqual(1, arrow.Uncertainty(Difficulty.Hard));
		}

		// The base definition, for an arrow no player shot: 1.1, and a spread of 16 that hard narrows to 4.
		[TestMethod]
		public void UnownedArrow_KeepsTheBaseDefinition()
		{
			var arrow = new Arrow(null, null);
			Assert.AreEqual(1.1, arrow.Power);
			Assert.AreEqual(16, arrow.Uncertainty(Difficulty.Peaceful));
			Assert.AreEqual(4, arrow.Uncertainty(Difficulty.Hard));
			Assert.AreEqual(4, arrow.Uncertainty(Difficulty.Hardcore));
		}
	}
}