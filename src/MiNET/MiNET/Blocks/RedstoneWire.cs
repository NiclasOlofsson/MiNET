using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class RedstoneWire : Block
	{
		public RedstoneWire() : base(55)
		{
			IsTransparent = true;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(331)};
		}
	}
}
