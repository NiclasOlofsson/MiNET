using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class MossStone : Block
	{
		public MossStone() : base(48)
		{
			BlastResistance = 30;
			Hardness = 2;
		}
	}
}
