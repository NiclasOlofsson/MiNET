namespace MiNET.Events.Level
{
	public class LevelEvent : Event
	{
		/// <summary>
		/// 	The level that the event occured in
		/// </summary>
		public Worlds.Level Level { get; }
		public LevelEvent(Worlds.Level world)
		{
			Level = world;
		}
	}
}
