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
using System.Numerics;
using log4net;
using MiNET.Entities.Passive;

namespace MiNET.Entities.Behaviors
{
	public class JumpAttackBehavior : BehaviorBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(JumpAttackBehavior));

		private readonly Wolf _wolf;
		private readonly double _leapHeight;

		public JumpAttackBehavior(Wolf wolf, double leapHeight)
		{
			_wolf = wolf;
			_leapHeight = leapHeight;
		}

		public override bool ShouldStart()
		{
			if (_wolf.Target == null) return false;
			if (_wolf.Target.HealthManager.IsDead) return false;

			var distance = _wolf.DistanceTo(_wolf.Target);

			return distance >= 4 && distance <= 16 && _wolf.IsOnGround && new Random().Next(5) == 0;
		}

		public override bool CanContinue()
		{
			if (_wolf.Target == null)
				return false;
			if (_wolf.Target.HealthManager.IsDead)
				return false;

			return !_wolf.IsOnGround;
		}

		public override void OnTick(Entity[] entities)
		{
			Entity wolfTarget = _wolf.Target;
			if (wolfTarget == null || wolfTarget.HealthManager.IsDead) return;

			var direction = (Vector3) wolfTarget.KnownPosition - _wolf.KnownPosition;
			var distance = _wolf.DistanceTo(wolfTarget);

			var velocity = _wolf.Velocity;
			var x = direction.X / distance * 0.5D * 0.8 + velocity.X * 0.2;
			var z = direction.Z / distance * 0.5D * 0.8 + velocity.Z * 0.2;
			var y = _leapHeight;

			if (wolfTarget.HealthManager.IsDead) return;

			_wolf.Velocity += new Vector3((float) x, (float) y, (float) z);
			if(_wolf.Velocity.Length() > 1) Log.Warn($"Wolf velocity too big {velocity}"); 

		}
	}
}