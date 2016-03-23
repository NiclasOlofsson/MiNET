using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
	public class NoteBlock : Block
	{
		public NoteBlock() : base(25)
		{
			BlastResistance = 4;
			Hardness = 0.8f;
			//IsFlammable = true; // It can catch fire from lava, but not other means.
		}
	}
}
