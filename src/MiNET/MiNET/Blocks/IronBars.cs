using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class IronBars : Block
	{
		public IronBars() : base(101)
		{
			IsTransparent = true;
			BlastResistance = 30;
			Hardness = 5;
		}
	}
}
