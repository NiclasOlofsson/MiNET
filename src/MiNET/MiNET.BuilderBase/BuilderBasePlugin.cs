using log4net;
using MiNET.BuilderBase.Commands;
using MiNET.Items;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Plugins.Commands;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	[Plugin(PluginName = "BuilderBase", Description = "Basic builder commands for MiNET", PluginVersion = "1.0", Author = "MiNET Team")]
	public class BuilderBasePlugin : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BuilderBasePlugin));

		protected override void OnEnable()
		{
			Context.PluginManager.LoadCommands(new HelpCommand(Context.PluginManager));
			Context.PluginManager.LoadCommands(new SelectionCommands());
			Context.PluginManager.LoadCommands(new RegionCommands());
			Context.PluginManager.LoadCommands(new HistoryCommands());
			Context.PluginManager.LoadCommands(new ClipboardCommands());

			ItemFactory.CustomItemFactory = new BuilderBaseItemFactory();

			var server = Context.Server;

			server.LevelManager.LevelCreated += (sender, args) =>
			{
				Level level = args.Level;

				//level.BlockBreak += LevelOnBlockBreak;
				//level.BlockPlace += LevelOnBlockPlace;
			};

			server.PlayerFactory.PlayerCreated += (sender, args) =>
			{
				Player player = args.Player;
				player.PlayerJoin += OnPlayerJoin;
				player.PlayerLeave += OnPlayerLeave;
			};


			var tickTimer = new HighPrecisionTimer(50, LevelTick);
		}

		public override void OnDisable()
		{
		}

		private void LevelTick(object o)
		{
			foreach (var kvp in RegionSelector.RegionSelectors)
			{
				Player player = kvp.Key;
				RegionSelector selector = kvp.Value;

				var wand = player.Inventory.GetItemInHand() as DistanceWand;
				if (wand == null) continue;

				selector.DisplaySelection();
			}
		}

		private void OnPlayerJoin(object sender, PlayerEventArgs e)
		{
			RegionSelector.RegionSelectors.TryAdd(e.Player, new RegionSelector(e.Player));
		}

		private void OnPlayerLeave(object sender, PlayerEventArgs e)
		{
			RegionSelector value;
			RegionSelector.RegionSelectors.TryRemove(e.Player, out value);
		}

		//private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
		//{
		//}

		//private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
		//{
		//}


		[Command]
		public void BbVersion()
		{
			CurrentPlayer.SendMessage("BuilderBase loaded in version 1.0");
		}
	}
}