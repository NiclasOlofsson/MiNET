using System.Collections.Generic;
using MiNET.Items;

namespace MiNET.Events.Block
{
	/// <summary>
	///		Gets dispatched when a <see cref="MiNET.Entities.Entity"/> breaks a block.
	/// </summary>
	public class BlockBreakEvent : BlockExpEvent
	{
		/// <summary>
		/// 	The <see cref="MiNET.Entities.Entity"/> that broke the block.
		/// 	This can be a player, a mob or null.
		/// </summary>
		public MiNET.Entities.Entity Source { get; }
		
		/// <summary>
		/// 	The items that were to be dropped if the block gets broken.
		/// </summary>
		public List<Item> Drops { get; }
		
		/// <summary>
		/// 	
		/// </summary>
		/// <param name="player">The entity that triggered the event</param>
		/// <param name="block">The block that was broken</param>
		/// <param name="drops">The items that were gonna be dropped if the block broke</param>
		public BlockBreakEvent(MiNET.Entities.Entity player, MiNET.Blocks.Block block, List<Item> drops) : base(block, block.GetExperiencePoints())
		{
			Source = player;
			Drops = drops;
		}
	}
}
