using System.Collections.Generic;
using Craft.Net.Common;

namespace MiNET.Utils
{
	public class Records : List<Coordinates3D>
	{
		public Records()
		{
		}

		public Records(IEnumerable<Coordinates3D> coordinates) : base(coordinates)
		{
		}
	}
}