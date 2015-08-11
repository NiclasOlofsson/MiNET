using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Items
{
    public class ItemDiamondAxe : Item
    {
        public ItemDiamondAxe(short metadata) : base(279, metadata)
        {
            ItemMaterial = ItemMaterial.Diamond;
            ItemType = ItemType.Axe;
            FuelEfficiency = 10;
        }
    }
}
