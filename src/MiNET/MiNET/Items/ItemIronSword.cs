using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class ItemIronSword : Item
	{
		public ItemIronSword() : base(267)
		{
			ItemMaterial = ItemMaterial.Iron;
			ItemType = ItemType.Sword;
		}
	}
}
