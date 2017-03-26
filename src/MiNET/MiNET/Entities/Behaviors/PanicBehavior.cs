using log4net;

namespace MiNET.Entities.Behaviors
{
	public class PanicBehavior : StrollBehavior
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PanicBehavior));

		public PanicBehavior(int duration, double speed, double speedMultiplier) : base(duration, speed, speedMultiplier)
		{
		}

		public override bool ShouldStart(Entity entity)
		{
			return entity.HealthManager.LastDamageSource != null;
		}
	}
}