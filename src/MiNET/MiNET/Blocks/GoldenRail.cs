using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class GoldenRail : Block
	{
		public GoldenRail() : base(27)
		{
			IsSolid = false;
			IsTransparent = true;
			BlastResistance = 3.5f;
			Hardness = 0.7f;
		}
	}
}
