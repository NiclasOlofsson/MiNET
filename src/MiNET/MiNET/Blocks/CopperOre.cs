using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class CopperOre
	{
		public CopperOre() : base()
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item[] GetDrops(Level world, Item tool)
		{
			if (tool.ItemMaterial < ItemMaterial.Stone) return new Item[0];

			return new[] { new ItemRawCopper() }; 
		}
	}
}
