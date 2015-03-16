using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class ItemDiamondHelmet : Item
	{
		public ItemDiamondHelmet(short metadata) : base(310, metadata)
		{
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}
