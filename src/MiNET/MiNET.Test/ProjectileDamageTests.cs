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
using MiNET.Entities;
using MiNET.Entities.Projectiles;

namespace MiNET.Test
{
	/// <summary>
	///     Projectile damage follows BDS's on_hit.impact_damage: a fixed range plus speed times the
	///     power multiplier, rounded up only where the definition says so, and filtered to one target
	///     where it names one. Each case is a vanilla definition whose in-game result is known.
	/// </summary>
	[TestClass]
	public class ProjectileDamageTests
	{
		// An arrow's damage is its speed times 2, rounded up: 1.4 blocks a tick hits for 3.
		[TestMethod]
		public void Arrow_HitsForSpeedTimesTwoRoundedUp()
		{
			var arrow = new Arrow(null, null);
			Assert.AreEqual(3, arrow.ImpactDamage(1.4));
		}

		// A wind charge's direct hit is a fixed 1, however fast it flies.
		[TestMethod]
		public void WindCharge_HitsForOneAtAnySpeed()
		{
			var charge = new WindChargeProjectile(null, null);
			Assert.AreEqual(1, charge.ImpactDamage(1.5));
			Assert.AreEqual(1, charge.ImpactDamage(0.2));
		}

		// A snowball hurts only blazes, for 3; anything else gets the knockback and no damage.
		[TestMethod]
		public void Snowball_DamagesOnlyBlazes()
		{
			var snowball = new Snowball(null, null);
			Assert.AreEqual(3, snowball.ImpactDamage(1.5));
			Assert.IsTrue(snowball.Damages(new Entity(EntityType.Blaze, null)));
			Assert.IsFalse(snowball.Damages(new Entity(EntityType.Zombie, null)));
		}
	}
}