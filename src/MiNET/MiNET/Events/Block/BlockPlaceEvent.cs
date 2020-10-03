﻿namespace MiNET.Events.Block
{
	/// <summary>
	/// 	Get's dispatched whenever a <see cref="OpenPlayer"/> tries to place a block.
	/// </summary>
	public class BlockPlaceEvent : BlockEvent
	{
		public MiNET.Player Player { get; }
		
		/// <summary>
		/// 	
		/// </summary>
		/// <param name="player">The player that tried placing the block</param>
		/// <param name="block">The block the player was trying to place.</param>
		public BlockPlaceEvent(MiNET.Player player, MiNET.Blocks.Block block) : base(block)
		{
			Player = player;
		}
	}
}
