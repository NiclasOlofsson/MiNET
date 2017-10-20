using System;

namespace MiNET.Entities.Behaviors
{
	public class RandomLookaroundBehavior : IBehavior
	{
		private readonly Mob _entity;
		private double _rotation = 0;
		private int _duration = 0;

		public RandomLookaroundBehavior(Mob entity)
		{
			this._entity = entity;
		}

		public bool ShouldStart()
		{
			var shouldStart = _entity.Level.Random.NextDouble() < 0.02;
			if (!shouldStart) return false;

			_duration = 20 + _entity.Level.Random.Next(20);
			_rotation = _entity.Level.Random.Next(-180, 180);

			return true;
		}

		public bool CanContinue()
		{
			return _duration-- > 0 && Math.Abs(_rotation) > 0;
		}

		public void OnTick(Entity[] entities)
		{
			_entity.Direction += (float) Math.Sign(_rotation)*10;
			_entity.KnownPosition.HeadYaw += (float) Math.Sign(_rotation)*10;
			_entity.KnownPosition.Yaw += (float) Math.Sign(_rotation)*10;
			_entity.BroadcastMove();

			_rotation -= 10;
		}

		public void OnEnd()
		{
			_entity.Direction = Mob.ClampDegrees(_entity.Direction);
			_entity.KnownPosition.HeadYaw = (float)_entity.Direction;
			_entity.KnownPosition.Yaw = (float)_entity.Direction;
			_entity.BroadcastMove(true);
		}
	}
}