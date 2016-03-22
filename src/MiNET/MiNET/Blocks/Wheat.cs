using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Wheat : Block
	{
		public Wheat() : base(59)
		{
			IsSolid = false;
			IsTransparent = true;
		}

		public override Item GetDrops()
		{
			if (Metadata == 0x7)
			{
				// Can also return 0-3 seeds at random.
				return ItemFactory.GetItem(296, 0, 1);
			}

			return ItemFactory.GetItem(295, 0, 1);
		}
	}
}