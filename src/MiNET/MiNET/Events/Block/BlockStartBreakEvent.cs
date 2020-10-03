﻿namespace MiNET.Events.Block
{
    /// <summary>
    ///     Gets dispatched when a <see cref="OpenPlayer"/> start breaking a block
    /// </summary>
    public class BlockStartBreakEvent : BlockBreakEvent
    {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="player">The player that started breaking the block.</param>
        /// <param name="block">The block that is to be destroyed</param>
        public BlockStartBreakEvent(MiNET.Entities.Entity player, MiNET.Blocks.Block block) : base(player, block, null)
        {

        }
    }
}
