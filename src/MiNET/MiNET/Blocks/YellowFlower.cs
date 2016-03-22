using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class YellowFlower : Block
	{
		public YellowFlower() : base(37)
		{
			IsSolid = false;
			IsTransparent = true;
		}
	}
}
