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
			return new[] {ItemFactory.GetItem(49, 0, 8)};
		}
	}
}
