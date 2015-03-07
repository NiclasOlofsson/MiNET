using System.Collections.Generic;
using MiNET.Worlds;

namespace MiNET.Plugins
{
	public class PluginContext
	{
		public PluginManager PluginManager { get; private set; }
		public List<Level> Levels { get; private set; }

		public PluginContext(PluginManager pluginManager, List<Level> levels)
		{
			PluginManager = pluginManager;
			Levels = levels;
		}
	}
}