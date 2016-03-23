using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Wolf : PassiveMob
	{
		public Wolf(Level level) : base((int) EntityType.Wolf, level)
		{
			Width = Length = 0.6;
			Height = 0.8;
		}
	}
}