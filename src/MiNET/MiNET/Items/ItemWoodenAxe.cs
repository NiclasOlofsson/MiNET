using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	public class ItemWoodenAxe : Item
	{
		public ItemWoodenAxe(short metadata)
			: base(271, metadata, 10)
		{
			ItemMaterial = ItemMaterial.Wood;
			ItemType = ItemType.Axe;
		}
	}
}
