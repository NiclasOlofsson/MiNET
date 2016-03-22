using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class Wool : Block
	{
		public Wool() : base(35)
		{
			BlastResistance = 4;
			Hardness = 0.8f;
			IsFlammable = true;
		}
	}
}
