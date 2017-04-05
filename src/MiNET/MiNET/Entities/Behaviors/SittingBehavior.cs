using System.Numerics;
using MiNET.Entities.Passive;

namespace MiNET.Entities.Behaviors
{
	public class SittingBehavior : IBehavior
	{
		private readonly Mob _entity;

		public SittingBehavior(Mob entity)
		{
			this._entity = entity;
		}

		public bool ShouldStart()
		{
			if (!_entity.IsTamed) return false;
			if (_entity.IsInWater) return false;

			Player owner = ((Wolf) _entity).Owner as Player;

			var shouldStart = owner == null || ((!(_entity.KnownPosition.DistanceTo(owner.KnownPosition) < 144.0) || _entity.HealthManager.LastDamageSource == null) && _entity.IsSitting);
			if (!shouldStart) return false;

			_entity.Velocity *= new Vector3(0, 1, 0);

			return true;
		}

		public bool CanContinue()
		{
			return _entity.IsSitting;
		}


		public void OnTick()
		{
		}

		public void OnEnd()
		{
			_entity.IsSitting = false;
			_entity.BroadcastSetEntityData();
		}
	}
}