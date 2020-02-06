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

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET.Plotter
{
	public class PlotManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PlotManager));

		private PlotConfig _config = new PlotConfig();
		private ConcurrentDictionary<PlotCoordinates, Plot> _plots = new ConcurrentDictionary<PlotCoordinates, Plot>();
		private ConcurrentDictionary<UUID, PlotPlayer> _plotPlayers;

		public static string GetExecutingDirectoryName()
		{
			var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
			return new FileInfo(location.AbsolutePath).Directory?.FullName;
		}

		public PlotManager()
		{
			LoadConfig();
		}

		private object _configSync = new object();

		public void LoadConfig()
		{
			lock (_configSync)
			{
				string configFile = Path.Combine(GetExecutingDirectoryName(), "plots-config.json");
				var jsonSerializerSettings = GetJsonSettings();

				if (File.Exists(configFile))
				{
					var plots = JsonConvert.DeserializeObject<PlotConfig>(File.ReadAllText(configFile), jsonSerializerSettings);
					_plotPlayers = new ConcurrentDictionary<UUID, PlotPlayer>(plots.PlotPlayers.ToDictionary(plot => plot.Xuid));
					_plots = new ConcurrentDictionary<PlotCoordinates, Plot>(plots.Plots.ToDictionary(plot => plot.Coordinates));
				}
				else
				{
					Log.Warn($"Found no config file for plots at '{configFile}'");
					_plotPlayers = new ConcurrentDictionary<UUID, PlotPlayer>();
					_plots = new ConcurrentDictionary<PlotCoordinates, Plot>();
				}
			}
		}

		public void SaveConfig()
		{
			lock (_configSync)
			{
				_config.Plots = _plots.Values.OrderByDescending(p => p.UpdateDate).ToArray();
				_config.PlotPlayers = _plotPlayers.Values.OrderByDescending(p => p.UpdateDate).ToArray();
				string configFile = Path.Combine(GetExecutingDirectoryName(), "plots-config.json");
				var jsonSerializerSettings = GetJsonSettings();
				File.WriteAllText(configFile, JsonConvert.SerializeObject(_config, jsonSerializerSettings));
			}
		}

		private static JsonSerializerSettings GetJsonSettings()
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
			};

			jsonSerializerSettings.Converters.Add(new UuidConverter());
			return jsonSerializerSettings;
		}

		public bool HasClaim(PlotCoordinates coords, Player player)
		{
			if (player == null) return false;

			Plot plot = null;
			if (_plots.ContainsKey(coords))
			{
				plot = _plots[coords];
			}

			if (plot != null && Equals(plot.Owner, player.ClientUuid)) return true;

			return false;
		}

		public bool CanBuild(PlotCoordinates coords, Player player)
		{
			if (player == null) return false;

			Plot plot = null;
			if (_plots.ContainsKey(coords))
			{
				plot = _plots[coords];
			}

			if (plot == null) return false;

			if (Equals(plot.Owner, player.ClientUuid) || plot.AllowedPlayers.Contains(player.ClientUuid)) return true;

			return false;
		}

		public PlotPlayer GetOrAddPlotPlayer(Player player)
		{
			if (!_plotPlayers.TryGetValue(player.ClientUuid, out var plotPlayer))
			{
				plotPlayer = new PlotPlayer
				{
					Xuid = player.ClientUuid,
					Username = player.Username,
				};

				if (!_plotPlayers.TryAdd(player.ClientUuid, plotPlayer))
				{
					return null;
				}

				SaveConfig();
			}


			return plotPlayer;
		}

		public void UpdatePlotPlayer(PlotPlayer player)
		{
			player.UpdateDate = DateTime.UtcNow;
			_plotPlayers[player.Xuid] = player;

			SaveConfig();
		}

		public PlotPlayer GetPlotPlayer(string username)
		{
			return _plotPlayers.FirstOrDefault(pair => pair.Value.Username == username).Value;
		}

		public PlotPlayer GetPlotPlayer(UUID uuid)
		{
			_plotPlayers.TryGetValue(uuid, out var value);

			return value;
		}

		public bool TryGetPlot(PlotCoordinates coords, out Plot plot)
		{
			plot = null;

			if (coords == null) return false;
			if (coords.X == 0 || coords.Z == 0) return false;

			if (!_plots.ContainsKey(coords))
			{
				return false;
			}

			plot = _plots[coords];

			return true;
		}

		public bool TryClaim(PlotCoordinates coords, Player player, out Plot plot)
		{
			plot = null;

			GetOrAddPlotPlayer(player);

			if (coords.X == 0 || coords.Z == 0) return false;

			if (_plots.ContainsKey(coords))
			{
				plot = _plots[coords];
			}

			if (plot != null) return false;

			plot = new Plot
			{
				Coordinates = coords,
				Owner = player.ClientUuid,
			};

			if (!_plots.TryAdd(coords, plot)) return false;

			SaveConfig();

			var offset = ConvertToBlockCoordinates(coords);

			//int xOffset = offset.X;
			//int zOffset = offset.Z;
			//player.Level.SetBlock(new GoldBlock {Coordinates = new BlockCoordinates(xOffset, PlotWorldGenerator.PlotHeight + 2, zOffset)});
			//player.Level.SetBlock(new EmeraldBlock
			//{
			//	Coordinates = new BlockCoordinates(xOffset, PlotWorldGenerator.PlotHeight + 2, zOffset) + new BlockCoordinates(PlotWorldGenerator.PlotWidth - 1, 1, PlotWorldGenerator.PlotDepth - 1)
			//});


			Vector3 to = (Vector3) offset + (Vector3) new BlockCoordinates(PlotWorldGenerator.PlotWidth - 1, 256, PlotWorldGenerator.PlotDepth - 1);
			BoundingBox bbox = new BoundingBox(offset, to).GetAdjustedBoundingBox();

			for (int x = (int) bbox.Min.X; x <= (int) bbox.Max.X; x++)
			{
				for (int z = (int) bbox.Min.Z; z <= (int) bbox.Max.Z; z++)
				{
					if (player.Level.IsTransparent(new BlockCoordinates(x, PlotWorldGenerator.PlotHeight, z)))
					{
						player.Level.SetAir(x, PlotWorldGenerator.PlotHeight, z);
					}
				}
			}

			return true;
		}

		public bool Delete(Plot plot)
		{
			if (!_plots.ContainsKey(plot.Coordinates))
			{
				return false;
			}

			if (!_plots.TryRemove(plot.Coordinates, out Plot trash)) return false;

			SaveConfig();

			return true;
		}

		public bool UpdatePlot(Plot plot)
		{
			if (!_plots.ContainsKey(plot.Coordinates))
			{
				return false;
			}

			plot.UpdateDate = DateTime.UtcNow;
			_plots[plot.Coordinates] = plot;

			SaveConfig();

			return true;
		}


		public static BoundingBox GetBoundingBoxForPlot(PlotCoordinates coords)
		{
			var offset = ConvertToBlockCoordinates(coords);
			Vector3 to = (Vector3) offset + (Vector3) new BlockCoordinates(PlotWorldGenerator.PlotWidth - 1, 256, PlotWorldGenerator.PlotDepth - 1);
			return new BoundingBox(offset, to).GetAdjustedBoundingBox();
		}

		//public static PlotCoordinates ConvertToPlotCoordinates(PlayerLocation location)
		//{
		//	return ConvertToPlotCoordinates((BlockCoordinates) location);
		//}

		//public static PlotCoordinates ConvertToPlotCoordinates(BlockCoordinates coords)
		//{
		//	if (PlotWorldGenerator.IsXRoad(coords.X, true) || PlotWorldGenerator.IsZRoad(coords.Z, true)) return null;

		//	int plotX = coords.X/PlotWorldGenerator.PlotAreaWidth + (Math.Sign(coords.X));
		//	int plotZ = coords.Z/PlotWorldGenerator.PlotAreaDepth + (Math.Sign(coords.Z));

		//	return new PlotCoordinates(plotX, plotZ);
		//}

		public static BlockCoordinates ConvertToBlockCoordinates(PlotCoordinates plotCoordinates)
		{
			var xFactor = plotCoordinates.X > 0 ? plotCoordinates.X - 1 : Math.Abs(plotCoordinates.X);
			var zFactor = plotCoordinates.Z > 0 ? plotCoordinates.Z - 1 : Math.Abs(plotCoordinates.Z);

			int xOffset = xFactor*PlotWorldGenerator.PlotAreaWidth;
			int zOffset = zFactor*PlotWorldGenerator.PlotAreaDepth;
			xOffset *= Math.Sign(plotCoordinates.X);
			zOffset *= Math.Sign(plotCoordinates.Z);
			/*if (xOffset < 0)*/
			xOffset += PlotWorldGenerator.RoadWidth;
			/*if (zOffset < 0) */
			zOffset += PlotWorldGenerator.RoadWidth;

			return new BlockCoordinates(xOffset, 0, zOffset);
		}
	}
}