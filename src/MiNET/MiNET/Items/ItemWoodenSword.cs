using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	public class ItemWoodenSword : Item
	{
		public ItemWoodenSword(short metadata)
			: base(268, metadata, 10)
		{
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Sword;
		}
	}
}
