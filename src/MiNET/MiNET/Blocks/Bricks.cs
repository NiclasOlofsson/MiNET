using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class Bricks : Block
	{
		public Bricks() : base(45)
		{
			BlastResistance = 30;
			Hardness = 2;
		}
	}
}
