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

using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	/// <summary>
	///     What a <see cref="Hostile.Frostbite" /> throws in ranged mode and a player throws from the
	///     ice ball item. It hits for its speed times 0.96, with knockback, and breaks on whatever it
	///     hits. Values are BDS 1.26.70.20's vanilla definition.
	/// </summary>
	public class IceBall : Projectile
	{
		public IceBall(Player shooter, Level level) : base(shooter, EntityType.IceBall, level, 0.96)
		{
			Width = Length = Height = 0.25;

			Gravity = 0.03;
			Power = 1.5;

			HealthManager.IsInvulnerable = true;
			DespawnOnImpact = true;
			BroadcastMovement = true;
		}

		public override void DespawnEntity()
		{
			// The definition's hit sound, by the name the wire carries.
			var sound = McpeLevelSoundEvent.CreateObject();
			sound.soundId = "ice_ball.break";
			sound.blockId = -1;
			sound.position = KnownPosition;
			Level.RelayBroadcast(sound);

			base.DespawnEntity();
		}
	}
}