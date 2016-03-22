using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class IronBlock : Block
	{
		public IronBlock() : base(42)
		{
			BlastResistance = 30;
			Hardness = 5;
		}
	}
}
