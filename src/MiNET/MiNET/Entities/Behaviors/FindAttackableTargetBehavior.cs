using System;
using System.Linq;
using System.Numerics;

namespace MiNET.Entities.Behaviors
{
	public class FindAttackableTargetBehavior : IBehavior
	{
		private readonly Mob _entity;
		private readonly double _targetDistance;
		private Player _target;

		public FindAttackableTargetBehavior(Mob entity, double targetDistance)
		{
			_entity = entity;
			_targetDistance = targetDistance;
		}

		public bool ShouldStart()
		{
			var player = _entity.Level.Players
				.OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.Value.KnownPosition))
				.FirstOrDefault(p => p.Value.IsSpawned && _entity.DistanceTo(p.Value) < _targetDistance);

			if (player.Value == null) return false;

			if (Math.Abs(_entity.KnownPosition.Y - player.Value.KnownPosition.Y) > _entity.Height + 1) return false;

			_target = player.Value;

			return true;
		}

		public bool CanContinue()
		{
			return false;
		}

		public void OnTick()
		{
			_entity.SetTarget(_target);
		}

		public void OnEnd()
		{
		}
	}
}