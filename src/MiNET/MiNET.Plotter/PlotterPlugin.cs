#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Linq;
using log4net;
using MiNET.Entities;
using MiNET.Events;
using MiNET.Events.Block;
using MiNET.Events.Level;
using MiNET.Events.Player;
using MiNET.Player;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Plugins.Commands;
using MiNET.Plugins.Commands.Builtin;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Plotter
{
	[Plugin(Name = "Plotter", Description = "Basic plot server plugin for MiNET", Version = "1.0", Author = "MiNET Team")]
	public class PlotterPlugin : Plugin, IStartup, IEventHandler
	{
		private static readonly ILog              Log = LogManager.GetLogger(typeof (PlotterPlugin));
		private                 PlotManager       _plotManager;

		public void Configure(MiNetServer server)
		{
			server.LevelManager = new PlotterLevelManager(server);
			/*server.LevelManager.LevelCreated += (sender, args) =>
			{
				Level level = args.Level;

				level.BlockBreak += LevelOnBlockBreak;
				level.BlockPlace += LevelOnBlockPlace;
				level.PlayerAdded += LevelOnPlayerAdded;
				level.PlayerRemoved += LevelOnPlayerRemoved;
			};*/
		}

		[EventHandler(EventPriority.Monitor)]
		public void OnEntityLevelJoin(LevelEntityAddedEvent e)
		{
			if (e.Entity is Player.Player player)
				SetSpawnPosition(player, e.Level);
		}

		[EventHandler(EventPriority.Monitor)]
		public void OnEntityLevelLeave(LevelEntityRemovedEvent e)
		{
			if (e.Entity is Player.Player player)
			{
				var plotPlayer = _plotManager.GetOrAddPlotPlayer(player);
				plotPlayer.LastPosition = player.KnownPosition;
				_plotManager.UpdatePlotPlayer(plotPlayer);
			}
		}

		[EventHandler]
		public void OnBlockBreak(BlockBreakEvent e)
		{
			PlotCoordinates coords = (PlotCoordinates) e.Block.Coordinates;
			if (coords == null)
			{
				e.SetCancelled(true);
			}
			else
			{
				if (e.Source != null) e.SetCancelled(!_plotManager.CanBuild(coords, e.Source));
			}
		}

		
		[EventHandler]
		public void OnBlockPlace(BlockPlaceEvent e)
		{
			PlotCoordinates coords = (PlotCoordinates) e.Block.Coordinates;
			if (coords == null)
			{
				e.SetCancelled(true); 
			}
			else
			{
				if (e.Player != null) e.SetCancelled(!_plotManager.CanBuild(coords, e.Player));
			}
		}

		[EventHandler]
		public void OnPlayerJoin(PlayerJoinEvent e)
		{
			e.Player.Ticking += OnTicking;
		}

		protected override void OnEnable()
		{
			var server = Context.Server;

			_plotManager = new PlotManager();
			Context.CommandManager.LoadCommands(new VanillaCommands());
			Context.CommandManager.LoadCommands(new PlotCommands(_plotManager));
			Context.EventDispatcher.RegisterEvents(this);
			
			/*server.PlayerFactory.PlayerCreated += (sender, e) =>
			{
				Player player = e.Player;
				player.PlayerJoining += OnPlayerJoin;
				player.PlayerLeave += OnPlayerLeave;
				player.Ticking += OnTicking;
			};*/
		}

		/// <inheritdoc />
		public override void OnDisable()
		{
			base.OnDisable();
			Context.EventDispatcher.UnregisterEvents(this);
		}

		private void OnTicking(object sender, PlayerEventArgs e)
		{
			var player = e.Player;
			var level = player.Level;
			if (level.Dimension == Dimension.Overworld)
			{
				PlotterPluginStore store = player.Store<PlotterPluginStore>();

				if (_plotManager.TryGetPlot((PlotCoordinates) player.KnownPosition, out Plot plot))
				{
					if (store.CurrentPlot != plot)
					{
						player.SendSetTime(plot.Time);
						player.SendSetDownfall(plot.Downfall);

						if (plot.Owner.Equals(player.ClientUuid) || plot.AllowedPlayers.Contains(player.ClientUuid))
						{
							if (player.IsWorldImmutable)
							{
								player.IsWorldImmutable = false;
								player.SendAdventureSettings();
							}
						}
						else
						{
							if (!player.IsWorldImmutable)
							{
								player.IsWorldImmutable = true;
								player.SendAdventureSettings();
							}
						}

						string plotName = string.IsNullOrEmpty(plot.Title) ? $"{plot.Coordinates.X},{plot.Coordinates.Z}" : plot.Title;
						string plotDescription = string.IsNullOrEmpty(plot.Description) ? "" : $"{ChatColors.Aqua}{plot.Description}\n";
						player.SendTitle(null, TitleType.Clear);
						player.SendTitle(null, TitleType.AnimationTimes, 6, 6, 3*10);
						player.SendTitle($"{plotDescription}{ChatColors.White}Owner is {_plotManager.GetPlotPlayer(plot.Owner).Username}", TitleType.SubTitle);
						player.SendTitle($"{ChatColors.Gold}This is plot {plotName}", TitleType.Title);
						store.CurrentPlot = plot;
					}
				}
				else
				{
					store.CurrentPlot = null;
					if (!player.IsWorldImmutable)
					{
						player.IsWorldImmutable = true;
						player.SendSetDownfall(0);
						player.SendAdventureSettings();
						player.SendSetTime();
					}
				}
			}
		}

		private void SetSpawnPosition(Player.Player player, Level level)
		{
			var plotPlayer = _plotManager.GetOrAddPlotPlayer(player);
			var pos = plotPlayer.LastPosition ?? plotPlayer.Home;
			if (pos != null)
			{
				int height = level.GetHeight((BlockCoordinates) pos);
				if (pos.Y < height)
				{
					pos.Y = height;
				}
				else
				{
					player.IsFlying = true;
					player.SendAdventureSettings();
				}

				player.SpawnPosition = pos;
				player.KnownPosition = pos;
			}
		}
	}

	public class PlotterPluginStore
	{
		public Plot CurrentPlot { get; set; }
	}
}