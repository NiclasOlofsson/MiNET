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

using MiNET.Entities.Behaviors;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Creeper : HostileMob
	{
		private int _timeSinceIgnition = 0;
		private int _fuseTime = 30;
		private bool _isPrimed = false;

		public Creeper(Level level) : base(EntityType.Creeper, level)
		{
			Width = Length = 0.6;
			Height = 1.7;
			NoAi = true;
			Speed = 0.25f;

			TargetBehaviors.Add(new HurtByTargetBehavior(this));
			TargetBehaviors.Add(new FindAttackableTargetBehavior(this, 16));

			Behaviors.Add(new CreeperSwellBehavior(this));
			Behaviors.Add(new MeleeAttackBehavior(this, 1.0, 16));
			Behaviors.Add(new WanderBehavior(this, 0.8));
			Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public void Prime(bool state)
		{
			_isPrimed = state;
			IsIgnited = _isPrimed;
			BroadcastSetEntityData();
		}

		public override void OnTick(Entity[] entities)
		{
			if (HealthManager.IsDead) return;

			if (!_isPrimed)
			{
				_timeSinceIgnition = 0;
			}
			else
			{
				_timeSinceIgnition++;
				if (_timeSinceIgnition > _fuseTime)
				{
					Explosion exp = new Explosion(Level, (BlockCoordinates) KnownPosition, 3);
					exp.Explode();
					HealthManager.Kill();
				}
			}

			base.OnTick(entities);
		}
	}
}