using MiNET.Entities.Passive;

namespace MiNET.Entities.Behaviors
{
	public class OwnerHurtByTargetBehavior : IBehavior
	{
		private Wolf _wolf;

		public OwnerHurtByTargetBehavior(Wolf wolf)
		{
			_wolf = wolf;
		}

		public bool ShouldStart()
		{
			if (!_wolf.IsTamed) return false;

			Player owner = (Player) _wolf.Owner;

			if (owner.HealthManager.LastDamageSource == null) return false;

			_wolf.SetTarget(owner.HealthManager.LastDamageSource);

			return true;
		}

		public bool CanContinue()
		{
			return false;
		}

		public void OnTick()
		{
		}

		public void OnEnd()
		{
		}
	}
}