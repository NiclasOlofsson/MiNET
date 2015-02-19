using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	class ItemDiamondSword : Item
	{
		internal ItemDiamondSword(short metadata) : base(276, metadata)
		{
			ItemType = ItemType.Sword;
			ItemMaterial = ItemMaterial.Diamond;
		}
	}
}
