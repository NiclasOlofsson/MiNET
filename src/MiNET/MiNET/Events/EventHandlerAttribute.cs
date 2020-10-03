﻿using System;

namespace MiNET.Events
{
	/// <summary>
	/// Marks the method as an Event Handler method
	/// In order for the method to be called the parent class must implement <see cref="IEventHandler"/> and be registered with an <see cref="EventDispatcher"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class EventHandlerAttribute : Attribute
	{
		/// <summary>
		/// Determines when the methhod will be invoked by the <see cref="EventDispatcher"/>.
		/// For more details see <see cref="EventPriority"/>
		/// </summary>
		public EventPriority Priority { get; }
		
		/// <summary>
		/// Determines whether this method should still be invoked even if another EventHandler has already cancelled the event.
		/// </summary>
		public bool IgnoreCanceled { get; }
		
		/// <summary>
		/// 	
		/// </summary>
		/// <param name="priority">The priority to be used for this eventhandler</param>
		/// <param name="ignoreCanceled">See <see cref="IgnoreCanceled"/> </param>
		public EventHandlerAttribute(EventPriority priority, bool ignoreCanceled = false)
		{
			Priority = priority;
			IgnoreCanceled = ignoreCanceled;
		}

		public EventHandlerAttribute() : this(EventPriority.Normal)
		{
			
		}
	}
}
