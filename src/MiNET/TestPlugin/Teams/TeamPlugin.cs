using MiNET;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using TestPlugin.Teams.Blocks;

namespace TestPlugin.Teams
{
	[Plugin(PluginName = "Team", Description = "A simple game engine with teams", PluginVersion = "1.0", Author = "MiNET Team")]
	public class TeamPlugin : Plugin, IStartup
	{
		private MiNetServer _server;
		private PluginContext _context;

		public void Configure(MiNetServer server)
		{
			BlockFactory.CustomBlockFactory = new PluginBlockFactory(new GameManager());
		}


		public void OnEnable()
		{
		}

		public void OnDisable()
		{
		}
	}
}