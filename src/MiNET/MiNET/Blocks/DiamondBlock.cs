using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class DiamondBlock : Block
	{
		public DiamondBlock() : base(57)
		{
			BlastResistance = 30;
			Hardness = 5;
		}
	}
}
