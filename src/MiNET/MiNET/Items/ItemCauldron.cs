using MiNET.Blocks;

namespace MiNET.Items
{
	public class ItemCauldron : ItemBlock
	{
		public ItemCauldron() : base(380, 0)
		{
			Block = BlockFactory.GetBlockById(118);
		}
	}
}