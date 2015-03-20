using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class ItemDiamondChestplate : Item
	{
		public ItemDiamondChestplate(short metadata) : base(311, metadata)
		{
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}
