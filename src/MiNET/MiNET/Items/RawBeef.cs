using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class RawBeef : Item
	{
		internal RawBeef(short metadata) : base(363, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(364);
		}
	}
}
