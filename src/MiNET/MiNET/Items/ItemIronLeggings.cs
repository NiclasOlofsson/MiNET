using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class ItemIronLeggings: Item
	{
		internal ItemIronLeggings(short metadata) : base(308, metadata)
		{
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}
