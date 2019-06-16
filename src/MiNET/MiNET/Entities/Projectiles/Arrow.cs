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

using System;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Sounds;
using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	public class Arrow : Projectile
	{
		public Arrow(Player shooter, Level level, int damage = 2, bool isCritical = false) : base(shooter, EntityType.ShotArrow, level, damage, isCritical)
		{
			Width = 0.15;
			Length = 0.15;
			Height = 0.15;

			Gravity = 0.05;
			Drag = 0.01;

			//OK: Drag = 0.0083;

			HealthManager.IsInvulnerable = true;
			Ttl = 1200;
			DespawnOnImpact = false;
		}

		protected override void OnHitBlock(Block blockCollided)
		{
			IsCritical = false;
			BroadcastSetEntityData();

			McpeEntityEvent entityEvent = McpeEntityEvent.CreateObject();
			entityEvent.runtimeEntityId = EntityId;
			entityEvent.eventId = 39;
			entityEvent.data = 14;
			Level.RelayBroadcast(entityEvent);
		}

		protected override void OnHitEntity(Entity entityCollided)
		{
			IsCritical = false;
			BroadcastSetEntityData();
		}

	}
}