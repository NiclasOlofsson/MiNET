using MiNET.Net;

namespace MiNET.Events.Entity
{
	/// <summary>
	/// 	Dispatched when a <see cref="OpenPlayer"/> interacts with an entity
	/// </summary>
	public class EntityInteractEvent : EntityEvent
	{
		public MiNET.Player                                   SourcePlayer { get; }
		public McpeInventoryTransaction.ItemUseOnEntityAction Action       { get; }
		
		/// <summary>
		/// 	
		/// </summary>
		/// <param name="entity">The entity the player has interacted with</param>
		/// <param name="source">The player that initiated the interaction</param>
		/// <param name="action">The action that got initiated</param>
		public EntityInteractEvent(MiNET.Entities.Entity entity, MiNET.Player source, McpeInventoryTransaction.ItemUseOnEntityAction action) : base(entity)
		{
			SourcePlayer = source;
			Action = action;
		}
	}
}
