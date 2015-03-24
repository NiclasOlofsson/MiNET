using log4net;
using MiNET;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;

namespace TestPlugin.MobHunt
{
	[Plugin(PluginName = "MobHunt", Description = "A simple game engine for MobHunt", PluginVersion = "1.0", Author = "MiNET Team")]
	public class MobHunt : IStartup
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MobHunt));

		public void Configure(MiNetServer server)
		{
			BlockFactory.CustomBlockFactory = new PluginBlockFactory();
			//server.LevelFactory = new MobHuntLevelFactory();
		}
	}
}