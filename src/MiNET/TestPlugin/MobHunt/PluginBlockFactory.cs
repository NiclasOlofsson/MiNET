using MiNET.Blocks;

namespace TestPlugin.MobHunt
{
	public class PluginBlockFactory : ICustomBlockFactory
	{
		public Block GetBlockById(byte blockId)
		{
			Block block = null;

			if (blockId == 63) block = new CustomStandingSign();

			return block;
		}
	}
}