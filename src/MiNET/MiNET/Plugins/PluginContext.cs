using System.Collections.Generic;
using MiNET.Worlds;

namespace MiNET.Plugins
{
	public class PluginContext
	{
		public MiNetServer Server { get; private set; }
		public PluginManager PluginManager { get; private set; }
		public List<Level> Levels { get; private set; }

		public PluginContext(MiNetServer server, PluginManager pluginManager, List<Level> levels)
		{
			Server = server;
			PluginManager = pluginManager;
			Levels = levels;
		}
	}
}