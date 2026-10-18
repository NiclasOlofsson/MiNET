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

using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	/// <summary>
	///     The wind charge a player throws. A direct hit deals 1 with knockback; its vanilla
	///     definition also bursts on hit, shoving entities and toggling levers, doors and buttons,
	///     which this class does not do. It never slows, in air or water.
	/// </summary>
	public class WindChargeProjectile : Projectile
	{
		public WindChargeProjectile(Player shooter, Level level) : base(shooter, EntityType.WindChargeProjectile, level, 0)
		{
			Width = Length = Height = 0.3125;

			Gravity = 0;
			Inertia = 1;
			LiquidInertia = 1;
			Power = 1.5;
			UncertaintyBase = 1;
			DamageMin = DamageMax = 1;
			MaxCriticalDamage = 1;

			HealthManager.IsInvulnerable = true;
			DespawnOnImpact = true;
			BroadcastMovement = true;
		}
	}
}
