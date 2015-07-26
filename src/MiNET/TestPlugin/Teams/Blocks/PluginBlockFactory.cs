using MiNET.Blocks;

namespace TestPlugin.Teams.Blocks
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
			if (blockId == 68) block = new CustomWallSign(_gameManager);

			return block;
		}
	}
}