using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class ItemIronBoots: Item
	{
		internal ItemIronBoots(short metadata) : base(309, metadata)
		{
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}
