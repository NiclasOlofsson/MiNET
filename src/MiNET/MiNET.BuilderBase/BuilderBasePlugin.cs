using System.Collections.Generic;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Commands;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.BuilderBase.Tools;
using MiNET.Items;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Plugins.Commands;
using MiNET.Utils;
using MiNET.Utils.IO;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	[Plugin(PluginName = "BuilderBase", Description = "Basic builder commands for MiNET", PluginVersion = "1.0", Author = "MiNET Team")]
	public class BuilderBasePlugin : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BuilderBasePlugin));

		protected override void OnEnable()
		{
			Context.PluginManager.LoadCommands(new MiscCommands());
			Context.PluginManager.LoadCommands(new SelectionCommands());
			Context.PluginManager.LoadCommands(new RegionCommands());
			Context.PluginManager.LoadCommands(new HistoryCommands());
			Context.PluginManager.LoadCommands(new ClipboardCommands());
			Context.PluginManager.LoadCommands(new BrushCommands());
			Context.PluginManager.LoadCommands(new SchematicsCommands());

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

				//if (!(player.Inventory.GetItemInHand() is DistanceWand wand)) continue;

				selector.DisplaySelection(forceHide: !(player.Inventory.GetItemInHand() is DistanceWand wand));
			}
		}

		private void OnPlayerJoin(object sender, PlayerEventArgs e)
		{
			RegionSelector.RegionSelectors.TryAdd(e.Player, new RegionSelector(e.Player));
			SetInventory(e.Player);
		}

		public void SetInventory(Player player)
		{
			int idx = 0;
			player.Inventory.Slots[idx++] = new ItemAir();
			player.Inventory.Slots[idx++] = new DistanceWand();
			player.Inventory.Slots[idx++] = new TeleportTool();
			player.Inventory.Slots[idx++] = new BrushTool {BrushType = 0, Radius = 5, Mask = new Mask(player.Level, new List<Block> {new Air()}, true)};
			player.Inventory.Slots[idx++] = new BrushTool {BrushType = 2, Radius = 5};
			player.Inventory.Slots[idx++] = new BrushTool {BrushType = 3, Radius = 5};
			player.SendPlayerInventory();
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