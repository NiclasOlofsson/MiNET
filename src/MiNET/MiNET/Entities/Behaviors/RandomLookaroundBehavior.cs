using System;

namespace MiNET.Entities.Behaviors
{
	public class RandomLookaroundBehavior : IBehavior
	{
		private double _rotation = 0;
		private int _duration = 0;

		public bool ShouldStart(Entity entity)
		{
			var shouldStart = entity.Level.Random.NextDouble() < 0.02;
			if (!shouldStart) return false;

			_duration = 20 + entity.Level.Random.Next(20);
			_rotation = entity.Level.Random.Next(-180, 180);

			return true;
		}

		public bool OnTick(Entity entity)
		{
			return false;
		}

		public bool CalculateNextMove(Entity entity)
		{
			entity.KnownPosition.HeadYaw += (float) Math.Sign(_rotation)*10;
			entity.KnownPosition.Yaw += (float) Math.Sign(_rotation)*10;
			entity.BroadcastMove();

			_rotation -= 10;

			return _duration-- < 0 || Math.Abs(_rotation) < 0;
		}

		public void OnEnd(Entity entity)
		{
		}
	}
}