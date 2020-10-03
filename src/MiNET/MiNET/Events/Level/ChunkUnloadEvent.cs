using MiNET.Worlds;

namespace MiNET.Events.Level
{
	/// <summary>
	/// 	Dispatched when a <see cref="ChunkColumn"/> was unloaded
	/// </summary>
	public class ChunkUnloadEvent : ChunkEvent
	{
		/// <summary>
		/// 	
		/// </summary>
		/// <param name="chunk">The chunk that was unloaded</param>
		/// <param name="level">The level the chunk was unloaded from</param>
		public ChunkUnloadEvent(ChunkColumn chunk, Worlds.Level level) : base(chunk, level)
		{
			
		}
	}
}
