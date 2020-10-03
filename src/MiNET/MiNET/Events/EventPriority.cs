﻿namespace MiNET.Events
{
	/// <summary>
	/// 	Determines in what order the events should be handled.
	/// 	Highest priority is executed last, lowest is executed first.
	/// </summary>
	public enum EventPriority
	{
		/// <summary>
		/// 	Gets executed 1st
		/// </summary>
		Lowest = 1,
		
		/// <summary>
		/// 	Gets executed 2nd
		/// </summary>
		Low = 2,
		
		/// <summary>
		/// 	Gets executed 3rd
		/// </summary>
		Normal = 3,
		
		/// <summary>
		/// 	Gets executed 4th
		/// </summary>
		High = 4,
		
		/// <summary>
		/// 	Gets executed second to last
		/// </summary>
		Highest = 5,
		
		/// <summary>
		/// 	Gets executed last, always has the final say over the result
		/// </summary>
		Monitor = 6
	}
}
