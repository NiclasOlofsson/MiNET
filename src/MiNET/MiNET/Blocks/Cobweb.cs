using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Cobweb : Block
	{
		public Cobweb() : base(30)
		{
			IsSolid = false;
			IsTransparent = true; // Partial - diffuses sky light
			BlastResistance = 20;
			Hardness = 4;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(287)};
		}
	}
}
