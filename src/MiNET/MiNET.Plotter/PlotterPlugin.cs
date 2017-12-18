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

using log4net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Worlds;

namespace MiNET.Plotter
{
	[Plugin(PluginName = "Plotter", Description = "Basic plot server plugin for MiNET", PluginVersion = "1.0", Author = "MiNET Team")]
	public class PlotterPlugin : Plugin, IStartup
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PlotterPlugin));
		private PlotManager _plotManager;

		public void Configure(MiNetServer server)
		{
			server.LevelManager = new PlotterLevelManager();
			server.LevelManager.LevelCreated += (sender, args) =>
			{
				Level level = args.Level;

				level.BlockBreak += LevelOnBlockBreak;
				level.BlockPlace += LevelOnBlockPlace;
			};
		}

		protected override void OnEnable()
		{
			var server = Context.Server;

			_plotManager = new PlotManager();
			Context.PluginManager.LoadCommands(new PlotCommands(_plotManager));
		}

		private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
		{
			PlotCoordinates coords = (PlotCoordinates) e.Block.Coordinates;
			if (coords == null)
			{
				//e.Cancel = e.Player.GameMode != GameMode.Creative;
				e.Cancel = true;
			}
			else
			{
				if (e.Player != null) e.Cancel = !_plotManager.HasClaim(coords, e.Player);
			}
			Log.Debug($"Cancel build={e.Cancel}");
		}

		private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
		{
			PlotCoordinates coords = (PlotCoordinates) e.TargetBlock.Coordinates;
			if (coords == null)
			{
				//e.Cancel = e.Player.GameMode != GameMode.Creative;
				e.Cancel = true;
			}
			else
			{
				if (e.Player != null) e.Cancel = !_plotManager.HasClaim(coords, e.Player);
			}
			Log.Debug($"Cancel build={e.Cancel}");
		}
	}
}