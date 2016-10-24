using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class Piston : Block
	{
		public Piston() : this(33)
		{
		}

		public Piston(byte id) : base(id)
		{
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}
	}
}
