using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	public class ItemWoodenShovel : Item
	{
		public ItemWoodenShovel(short metadata)
			: base(269, metadata, 10)
		{
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Shovel;
		}
	}
}
