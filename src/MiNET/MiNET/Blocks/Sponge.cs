using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class Sponge : Block
	{
		public Sponge() : base(19)
		{
			BlastResistance = 3;
			Hardness = 0.6f;
		}

	}
}
