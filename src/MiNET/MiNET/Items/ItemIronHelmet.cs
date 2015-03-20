using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
	public class ItemIronHelmet : Item
	{
		public ItemIronHelmet(short metadata) : base(306, metadata)
		{
			ItemType = ItemType.Helmet;
			ItemMaterial = ItemMaterial.Iron;
		}
	}
}
