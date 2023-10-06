using MiNET.Items;

namespace MiNET.Blocks
{
	public partial class CopperOre
	{
		public CopperOre() : base()
		{
			BlastResistance = 15;
			Hardness = 3;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (tool.ItemMaterial < ItemMaterial.Stone) return new Item[0];

			return new[] { new ItemRawCopper() }; 
		}
	}
}
