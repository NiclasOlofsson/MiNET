using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class DeadBush : Block
	{
		public DeadBush() : base(32)
		{
			IsSolid = false;
			IsTransparent = true;
			IsFlammable = true;
		}
	}
}
