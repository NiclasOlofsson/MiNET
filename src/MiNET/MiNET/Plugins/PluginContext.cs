using System.Collections.Generic;
using MiNET.Worlds;

namespace MiNET.Plugins
{
	public class PluginContext
	{
		public MiNetServer Server { get; private set; }
		public PluginManager PluginManager { get; private set; }
		public LevelManager LevelManager { get; private set; }

		public PluginContext(MiNetServer server, PluginManager pluginManager, LevelManager levelManager)
		{
			Server = server;
			PluginManager = pluginManager;
			LevelManager = levelManager;
		}
	}
}