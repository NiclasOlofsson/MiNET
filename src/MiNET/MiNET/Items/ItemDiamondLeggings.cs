using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class ItemDiamondLeggings : Item
	{
		public ItemDiamondLeggings(short metadata) : base(312, metadata)
		{
			ItemType = ItemType.Leggings;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}
