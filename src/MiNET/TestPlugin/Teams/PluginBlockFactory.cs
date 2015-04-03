using MiNET.Blocks;

namespace TestPlugin.Teams
{
	public class PluginBlockFactory : ICustomBlockFactory
	{
		private GameManager _gameManager;

		public PluginBlockFactory(GameManager gameManager)
		{
			_gameManager = gameManager;
		}

		public Block GetBlockById(byte blockId)
		{
			Block block = null;

			if (blockId == 63) block = new CustomStandingSign(_gameManager);

			return block;
		}
	}
}