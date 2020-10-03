﻿namespace MiNET.Events.Block
{
	/// <summary>
	/// 	The base class for any events related to blocks
	/// </summary>
	public class BlockEvent : Event
	{
		public MiNET.Blocks.Block Block { get; }
		public BlockEvent(MiNET.Blocks.Block block)
		{
			Block = block;
		}
	}
}
