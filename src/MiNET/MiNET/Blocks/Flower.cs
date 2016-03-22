using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class Flower : Block
	{
		public Flower() : base(38)
		{
			IsSolid = false;
			IsTransparent = true;
		}
	}
}
