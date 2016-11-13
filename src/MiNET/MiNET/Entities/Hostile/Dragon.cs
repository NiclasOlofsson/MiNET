using log4net;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Dragon : HostileMob, IAgeable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ElderGuardian));

		public Dragon(Level level) : base(EntityType.Dragon, level)
		{
			Width = Length = 13;
			Height = 4;
			IsElder = true;
		}
	}
}