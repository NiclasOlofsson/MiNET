using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Mob : Entity
	{
		public Mob(int entityTypeId, Level level) : base(entityTypeId, level)
		{
			Width = 0.6;
			Length = 0.6;
			Height = 1.95;
		}

		public override void OnTick()
		{
			base.OnTick();
		}
	}
}