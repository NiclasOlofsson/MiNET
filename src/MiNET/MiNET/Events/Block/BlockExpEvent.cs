﻿namespace MiNET.Events.Block
{
	/// <summary>
	/// 	Dispatched when a block released experience points
	/// </summary>
	public class BlockExpEvent : BlockEvent
	{
		/// <summary>
		/// 	The amount of XP released by the block
		/// </summary>
		public float Experience { get; set; }
		
		/// <summary>
		/// 	
		/// </summary>
		/// <param name="block">The block that released the XP</param>
		/// <param name="experience">The XP that is to be released</param>
		public BlockExpEvent(MiNET.Blocks.Block block, float experience) : base(block)
		{
			Experience = experience;
		}
	}
}
