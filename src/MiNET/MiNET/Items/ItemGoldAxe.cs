using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
    public class ItemGoldAxe : Item
    {
        public ItemGoldAxe(short metadata) : base(286, metadata)
        {
            ItemMaterial = ItemMaterial.Gold;
            ItemType = ItemType.Axe;
            FuelEfficiency = 10;
        }
    }
}
