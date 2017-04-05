using MiNET.Entities.Passive;

namespace MiNET.Entities.Behaviors
{
	public class HurtByTargetBehavior : IBehavior
	{
		private Wolf _wolf;

		public HurtByTargetBehavior(Wolf wolf)
		{
			_wolf = wolf;
		}

		public bool ShouldStart()
		{
			if (_wolf.HealthManager.LastDamageSource == null) return false;

			_wolf.SetTarget(_wolf.HealthManager.LastDamageSource);

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