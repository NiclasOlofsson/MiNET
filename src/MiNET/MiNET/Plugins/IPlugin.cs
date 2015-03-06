using System.Collections.Generic;
using MiNET.Worlds;

namespace MiNET.Plugins
{
	public class RequestContext
	{
		public Player Player { get; private set; }

		public RequestContext(Player player)
		{
			Player = player;
		}
	}

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

	public interface IPlugin
	{
		/// <summary>
		///     This function will be called on plugin initialization.
		/// </summary>
		void OnEnable(PluginContext context);

		/// <summary>
		///     This function will be called when the plugin will be disabled.s
		/// </summary>
		void OnDisable();
	}
}