using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class Lightning : Entity
	{
		public Lightning(Level level) : base(93, level)
		{
		}

		public override void OnTick(Entity[] entities)
		{
			base.OnTick(entities);

			if (!IsSpawned) return;

			if (Age > 40)
			{
				DespawnEntity();
			}
		}
	}
}