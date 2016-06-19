using MiNET;
using MiNET.Entities;

namespace TestPlugin
{
	public class NoDamageHealthManager : HealthManager
	{
		public NoDamageHealthManager(Entity entity) : base(entity)
		{
		}

		public override void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
		{
			//base.TakeHit(source, 0, cause);
		}
	}
}