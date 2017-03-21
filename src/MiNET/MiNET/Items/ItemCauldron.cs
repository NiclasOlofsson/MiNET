using MiNET.Blocks;

namespace MiNET.Items
{
	public class ItemCauldron : ItemBlock
	{
		public ItemCauldron() : base(380, 0)
		{
			_block = BlockFactory.GetBlockById(118);
		}
	}
}