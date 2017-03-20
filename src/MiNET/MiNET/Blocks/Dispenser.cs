using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Dispenser : Block
	{
		public Dispenser() : base(23)
		{
			BlastResistance = 17.5f;
			Hardness = 3.5f;
		}

		public override Item[] GetDrops(Item tool)
		{
			// TODO: Needs Dispenser TileEntity.
			return base.GetDrops(tool);
		}
	}
}
