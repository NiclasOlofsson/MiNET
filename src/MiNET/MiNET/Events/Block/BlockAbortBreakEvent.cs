namespace MiNET.Events.Block
{
    /// <summary>
    ///     Get's dispatched whenever a <see cref="OpenPlayer"/> stops breaking a block before the block could be destroyed.
    /// </summary>
    public class BlockAbortBreakEvent : BlockStartBreakEvent
    {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="player">The player that triggered the event</param>
        /// <param name="block">The block that the player was targetting</param>
        public BlockAbortBreakEvent(MiNET.Player.Player player, MiNET.Blocks.Block block) : base(player, block)
        {
        }
    }
}
