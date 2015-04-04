using MiNET;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;

namespace TestPlugin.WorldEdit
{
	[Plugin(PluginName = "WorldEdit")]
	public class WorldEditPlugin : IStartup, IPlugin
	{
		private MiNetServer _server;
		private PluginContext _context;

		public void Configure(MiNetServer server)
		{
			_server = server;
		}


		public void OnEnable(PluginContext context)
		{
			_context = context;
		}

		public void OnDisable()
		{
		}
	}
}