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
			Player player = _entity.Level.GetSpawnedPlayers()
				.OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition.ToVector3()))
				.FirstOrDefault(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition) < _targetDistance);

			if (player == null) return false;

			_target = player;

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