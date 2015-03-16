using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class ItemDiamondBoots : Item
	{
		public ItemDiamondBoots(short metadata) : base(313, metadata)
		{
			ItemType = ItemType.Boots;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}
