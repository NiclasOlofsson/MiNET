using System;
using System.Linq;
using System.Numerics;

namespace MiNET.Entities.Behaviors
{
	public class LookAtPlayerBehavior : IBehavior
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

		public bool ShouldStart()
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

		public bool CanContinue()
		{
			return _duration-- > 0;
		}

		public void OnTick()
		{
			var dx = _player.KnownPosition.X - _entity.KnownPosition.X;
			var dz = _player.KnownPosition.Z - _entity.KnownPosition.Z;

			double tanOutput = 90 - RadianToDegree(Math.Atan(dx/(dz)));
			double thetaOffset = 270d;
			if (dz < 0)
			{
				thetaOffset = 90;
			}
			var yaw = thetaOffset + tanOutput;

			double bDiff = Math.Sqrt((dx*dx) + (dz*dz));
			var dy = (_entity.KnownPosition.Y + _entity.Height) - (_player.KnownPosition.Y + 1.62);
			double pitch = RadianToDegree(Math.Atan(dy/(bDiff)));

			_entity.Direction = (float) yaw;
			_entity.KnownPosition.Yaw = (float) yaw;
			_entity.KnownPosition.HeadYaw = (float) yaw;
			_entity.KnownPosition.Pitch = (float) pitch;
			_entity.BroadcastMove(true);
		}

		public void OnEnd()
		{
			_player = null;
			_entity.KnownPosition.Pitch = 0;
			_entity.BroadcastMove(true);
		}

		private double RadianToDegree(double angle)
		{
			return angle*(180.0/Math.PI);
		}
	}
}