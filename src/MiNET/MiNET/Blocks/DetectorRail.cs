using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class DetectorRail : Block
	{
		public DetectorRail() : base(28)
		{
			IsSolid = false;
			IsTransparent = true;
			BlastResistance = 3.5f;
			Hardness = 0.7f;
		}
	}
}
