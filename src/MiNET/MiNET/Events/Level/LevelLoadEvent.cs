using System;

namespace MiNET.Events.Level
{
	[Obsolete("Implementation missing")]
	public class LevelLoadEvent : LevelEvent
	{
		public LevelLoadEvent(Worlds.Level world) : base(world)
		{
			throw new NotImplementedException();
		}
	}
}
