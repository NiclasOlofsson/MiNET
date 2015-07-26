using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin.Teams.Blocks
{
	public class CustomStandingSign : StandingSign
	{
		private GameManager _gameManager;

		public CustomStandingSign(GameManager gameManager)
		{
			_gameManager = gameManager;
		}

		public override bool Interact(Level currentLevel, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			Sign signEntity = currentLevel.GetBlockEntity(blockCoordinates) as Sign;
			if (signEntity == null) return false;

			string world = signEntity.Text1;

			if (player.Level.LevelId.Equals(world)) return true;

			_gameManager.Join(player, world);
			_gameManager.RegisterSign(signEntity, currentLevel);

			return true;
		}
	}
}