using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Events.Level
{
    /// <summary>
    ///     Dispatched when a new chunk was generated
    /// </summary>
    public class ChunkGeneratedEvent : ChunkEvent
    {
        /// <summary>
        ///     The coordinates of the chunk that got generated
        /// </summary>
        public ChunkCoordinates Coordinates { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates">The coordinates of the new chunk</param>
        /// <param name="chunk">An instance of the affected chunk</param>
        /// <param name="level">The level the chunk was generated in</param>
        public ChunkGeneratedEvent(ChunkCoordinates coordinates, ChunkColumn chunk, Worlds.Level level) : base(chunk, level)
        {
            Coordinates = coordinates;
        }
    }
}