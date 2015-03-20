using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class ItemIronChestplate : Item
	{
		internal ItemIronChestplate(short metadata) : base(307, metadata)
		{
			ItemType = ItemType.Chestplate;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}
