using System.Linq;
using MiNET.Items;

namespace MiNET.Blocks
{
    public class EnderChest : Chest
    {
        public EnderChest() : base(130)
        {
            IsTransparent = true;
            BlastResistance = 3000;
            Hardness = 22.5f;
            LightLevel = 6;
        }

        public override Item[] GetDrops()
        {
            return Enumerable.Repeat<Item>(new ItemBlock(new Obsidian(), 0), 8).ToArray();
        }
    }
}
