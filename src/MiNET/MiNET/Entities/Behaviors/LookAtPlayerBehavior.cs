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
using System.Linq;
using System.Numerics;

namespace MiNET.Entities.Behaviors
{
	public class LookAtPlayerBehavior : BehaviorBase
	{
		private readonly Mob _entity;
		private readonly double _lookDistance;
		private int _duration = 0;
		private Player _player;

		public LookAtPlayerBehavior(Mob entity, double lookDistance = 6.0)
		{
			this._entity = entity;
			_lookDistance = lookDistance;
		}

		public override bool ShouldStart()
		{
			var shouldStart = _entity.Level.Random.NextDouble() < 0.02;
			if (!shouldStart) return false;

			Player player = _entity.Level.GetSpawnedPlayers().OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition.ToVector3()))
				.FirstOrDefault(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition) < _lookDistance);

			if (player == null) return false;

			_player = player;
			_duration = 40 + _entity.Level.Random.Next(40);

			return true;
		}

		public override bool CanContinue()
		{
			return _duration-- > 0;
		}

		public override void OnTick(Entity[] entities)
		{
			var dx = _player.KnownPosition.X - _entity.KnownPosition.X;
			var dz = _player.KnownPosition.Z - _entity.KnownPosition.Z;

			double tanOutput = 90 - RadianToDegree(Math.Atan(dx / (dz)));
			double thetaOffset = 270d;
			if (dz < 0)
			{
				thetaOffset = 90;
			}
			var yaw = thetaOffset + tanOutput;

			double bDiff = Math.Sqrt((dx * dx) + (dz * dz));
			var dy = (_entity.KnownPosition.Y + _entity.Height) - (_player.KnownPosition.Y + 1.62);
			double pitch = RadianToDegree(Math.Atan(dy / (bDiff)));

			_entity.EntityDirection = (float) yaw;
			_entity.KnownPosition.Yaw = (float) yaw;
			_entity.KnownPosition.HeadYaw = (float) yaw;
			_entity.KnownPosition.Pitch = (float) pitch;
			_entity.BroadcastMove(true);
		}

		public override void OnEnd()
		{
			_player = null;
			_entity.KnownPosition.Pitch = 0;
			_entity.BroadcastMove(true);
		}

		private double RadianToDegree(double angle)
		{
			return angle * (180.0 / Math.PI);
		}
	}
}