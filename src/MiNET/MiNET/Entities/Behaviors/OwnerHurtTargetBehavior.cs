using MiNET.Entities.Passive;

namespace MiNET.Entities.Behaviors
{
	public class OwnerHurtTargetBehavior : IBehavior
	{
		private readonly Wolf _wolf;

		public OwnerHurtTargetBehavior(Wolf wolf)
		{
			_wolf = wolf;
		}

		public bool ShouldStart()
		{
			if (!_wolf.IsTamed) return false;

			Player owner = (Player) _wolf.Owner;

			if (owner.LastAttackTarget == null) return false;

			_wolf.SetTarget(owner.LastAttackTarget);

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