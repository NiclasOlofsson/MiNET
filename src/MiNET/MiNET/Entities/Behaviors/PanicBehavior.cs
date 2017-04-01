using log4net;

namespace MiNET.Entities.Behaviors
{
	public class PanicBehavior : StrollBehavior
	{
		private readonly Mob _entity;
		private static readonly ILog Log = LogManager.GetLogger(typeof (PanicBehavior));

		public PanicBehavior(Mob entity, int duration, double speed, double speedMultiplier) : base(entity, duration, speed, speedMultiplier)
		{
			_entity = entity;
		}

		public override bool ShouldStart()
		{
			return _entity.HealthManager.LastDamageSource != null;
		}
	}
}