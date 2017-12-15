using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBeetrootSeeds : ItemBlock
	{
		public ItemBeetrootSeeds() : base(458)
		{
			Block = BlockFactory.GetBlockById(244);
		}
	}
}