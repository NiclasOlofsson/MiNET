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

namespace MiNET.Entities.Behaviors
{
	public class RandomLookaroundBehavior : BehaviorBase
	{
		private readonly Mob _entity;
		private double _rotation = 0;
		private int _duration = 0;

		public RandomLookaroundBehavior(Mob entity)
		{
			this._entity = entity;
		}

		public override bool ShouldStart()
		{
			var shouldStart = _entity.Level.Random.NextDouble() < 0.02;
			if (!shouldStart) return false;

			_duration = 20 + _entity.Level.Random.Next(20);
			_rotation = _entity.Level.Random.Next(-180, 180);

			return true;
		}

		public override bool CanContinue()
		{
			return _duration-- > 0 && Math.Abs(_rotation) > 0;
		}

		public override void OnTick(Entity[] entities)
		{
			_entity.EntityDirection += (float) Math.Sign(_rotation) * 10;
			_entity.KnownPosition.HeadYaw += (float) Math.Sign(_rotation) * 10;
			_entity.KnownPosition.Yaw += (float) Math.Sign(_rotation) * 10;
			_entity.BroadcastMove();

			_rotation -= 10;
		}

		public override void OnEnd()
		{
			_entity.EntityDirection = Mob.ClampDegrees(_entity.EntityDirection);
			_entity.KnownPosition.HeadYaw = (float) _entity.EntityDirection;
			_entity.KnownPosition.Yaw = (float) _entity.EntityDirection;
			_entity.BroadcastMove(true);
		}
	}
}