using System.Collections.Generic;

namespace MiNET.Utils
{
	public class Records : List<BlockCoordinates>
	{
		public Records()
		{
		}

		public Records(IEnumerable<BlockCoordinates> coordinates) : base(coordinates)
		{
		}
	}
}