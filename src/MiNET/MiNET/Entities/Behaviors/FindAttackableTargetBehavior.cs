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
			if (_entity.Level.Random.Next(10) != 0)
			{
				_entity.SetTarget(null);
				return false;
			}

			var player = _entity.Level.Players
				.OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.Value.KnownPosition))
				.FirstOrDefault(p => p.Value.IsSpawned && _entity.DistanceTo(p.Value) < _targetDistance && _entity.CanSee(p.Value));

			if (player.Value == null)
			{
				_entity.SetTarget(null);
				return false;
			}

			_target = player.Value;

			return true;
		}

		public bool CanContinue()
		{
			return false;
		}

		public void OnTick(Entity[] entities)
		{
			_entity.SetTarget(_target);
		}

		public void OnEnd()
		{
		}
	}
}