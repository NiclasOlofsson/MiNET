using System;
using System.Linq;
using System.Numerics;

namespace MiNET.Entities.Behaviors
{
	public class LookAtPlayerBehavior : IBehavior
	{
		private readonly double _lookDistance;
		private int _duration = 0;
		private Player _player;

		public LookAtPlayerBehavior(double lookDistance = 6.0)
		{
			_lookDistance = lookDistance;
		}

		public bool ShouldStart(Entity entity)
		{
			var shouldStart = entity.Level.Random.NextDouble() < 0.02;
			if (!shouldStart) return false;

			Player player = entity.Level.GetSpawnedPlayers().OrderBy(p => Vector3.Distance(entity.KnownPosition, p.KnownPosition.ToVector3()))
				.FirstOrDefault(p => Vector3.Distance(entity.KnownPosition, p.KnownPosition) < _lookDistance);

			if (player == null) return false;

			_player = player;
			_duration = 40 + entity.Level.Random.Next(40);

			return true;
		}

		public bool OnTick(Entity entity)
		{
			return false;
		}

		public bool CalculateNextMove(Entity entity)
		{
			var dx = _player.KnownPosition.X - entity.KnownPosition.X;
			var dz = _player.KnownPosition.Z - entity.KnownPosition.Z;

			double tanOutput = 90 - RadianToDegree(Math.Atan(dx/(dz)));
			double thetaOffset = 270d;
			if (dz < 0)
			{
				thetaOffset = 90;
			}
			var yaw = thetaOffset + tanOutput;

			double bDiff = Math.Sqrt((dx*dx) + (dz*dz));
			var dy = (entity.KnownPosition.Y + entity.Height) - (_player.KnownPosition.Y + 1.62);
			double pitch = RadianToDegree(Math.Atan(dy/(bDiff)));

			entity.KnownPosition.Yaw = (float) yaw;
			entity.KnownPosition.HeadYaw = (float) yaw;
			entity.KnownPosition.Pitch = (float) pitch;
			entity.BroadcastMove();

			return _duration-- < 0;
		}

		public void OnEnd(Entity entity)
		{
			_player = null;
			entity.KnownPosition.Pitch = 0;
			entity.BroadcastMove();
		}

		private double RadianToDegree(double angle)
		{
			return angle*(180.0/Math.PI);
		}
	}
}