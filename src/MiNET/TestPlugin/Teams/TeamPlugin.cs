using System.Threading;
using MiNET;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;

namespace TestPlugin.Teams
{
	[Plugin(PluginName = "Team", Description = "A simple game engine with teams", PluginVersion = "1.0", Author = "MiNET Team")]
	public class TeamPlugin : Plugin, IStartup
	{
		private MiNetServer _server;
		private PluginContext _context;
		private GameManager _gameManager;

		public void Configure(MiNetServer server)
		{
			_gameManager = new GameManager();
			//BlockFactory.CustomBlockFactory = new PluginBlockFactory(_gameManager);
		}

		public void OnEnable()
		{
		}

		public void OnDisable()
		{
		}

		[Command]
		public void Join(Player player, string world)
		{
			if (player.Level.LevelId.Equals(world)) return;

			_gameManager.Join(player, world);
		}
	}
}