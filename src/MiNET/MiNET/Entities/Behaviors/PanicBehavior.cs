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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2019 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Linq;
using System.Numerics;
using log4net;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Entities.Behaviors
{
	public class PanicBehavior : StrollBehavior
	{
		private readonly Mob _entity;
		private static readonly ILog Log = LogManager.GetLogger(typeof(PanicBehavior));

		public PanicBehavior(Mob entity, int duration, double speed, double speedMultiplier) : base(entity, duration, speed, speedMultiplier)
		{
			_entity = entity;
		}

		public override bool ShouldStart()
		{
			return _entity.HealthManager.LastDamageSource != null;
		}
	}

	public class PanicBehaviorNew : WanderBehavior
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(PanicBehaviorNew));

		private readonly Mob _entity;
		private int _duration;
		private int _timeLeft;
		private double _distanceToSource;
		private Entity _source;

		public PanicBehaviorNew(Mob entity, int duration, double speedMultiplier) : base(entity, speedMultiplier, 1)
		{
			_entity = entity;
			_duration = duration;
			_timeLeft = duration;
		}

		public override bool ShouldStart()
		{
			if (!(_entity.HealthManager.LastDamageSource is Entity source))
			{
				source = _entity.Level.Entities.FirstOrDefault(entity => entity.Value.EntityTypeId == EntityType.Wolf.ToStringId() && Vector3.Distance(_entity.KnownPosition, entity.Value.KnownPosition) < 5).Value;
			}

			bool shouldStart = false;
			if (source != null)
			{
				_source = source;
				_direction = LookAt(source.KnownPosition, _entity.KnownPosition);
				_distanceToSource = source.KnownPosition.DistanceTo(_entity.KnownPosition);
				shouldStart = base.ShouldStart();
			}

			if (shouldStart)
			{
				_entity.IsPanicking = true;
			}

			return shouldStart;
		}

		public override bool CanContinue()
		{
			if (_timeLeft-- <= 0)
			{
				return false;
			}

			if (_timeLeft % 20 == 0)
			{
				if (!(_entity.HealthManager.LastDamageSource is Entity source))
				{
					source = _entity.Level.Entities.FirstOrDefault(entity => entity.Value.EntityTypeId == EntityType.Wolf.ToStringId() && Vector3.Distance(_entity.KnownPosition, entity.Value.KnownPosition) < 2).Value;
				}

				if (source != _source) return false;
			}

			double distanceTo = _source.KnownPosition.DistanceTo(_entity.KnownPosition);
			if (distanceTo < _distanceToSource) return false;

			return base.CanContinue();
		}

		public override void OnEnd()
		{
			_source = null;
			_distanceToSource = 0;
			_timeLeft = _duration;
			_entity.IsPanicking = false;
			_direction = null;

			base.OnEnd();
		}

		public static PlayerLocation LookAt(Vector3 sourceLocation, Vector3 targetLocation)
		{
			var dx = targetLocation.X - sourceLocation.X;
			var dz = targetLocation.Z - sourceLocation.Z;

			var pos = new PlayerLocation(sourceLocation.X, sourceLocation.Y, sourceLocation.Z);
			if (dx > 0 || dz > 0)
			{
				double tanOutput = 90 - (Math.Atan(dx / (dz))).ToDegrees();
				double thetaOffset = 270d;
				if (dz < 0)
				{
					thetaOffset = 90;
				}
				var yaw = thetaOffset + tanOutput;

				double bDiff = Math.Sqrt((dx * dx) + (dz * dz));
				var dy = (sourceLocation.Y) - (targetLocation.Y);
				double pitch = (Math.Atan(dy / (bDiff))).ToDegrees();

				pos.Yaw = (float) yaw;
				pos.HeadYaw = (float) yaw;
				pos.Pitch = (float) pitch;
			}

			return pos;
		}
	}
}