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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using MiNET;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace TestPlugin.SurivalCraft
{
	[Plugin(PluginName = "SurvivalCraft", Description = "", PluginVersion = "1.0", Author = "MiNET Team")]
	public class SurvivalCraftPlugin : Plugin
	{
		protected override void OnEnable()
		{
			var server = Context.Server;


			server.LevelManager.LevelCreated += (sender, args) =>
			{
				Level level = args.Level;

				//BossBar bossBar = new BossBar(level)
				//{
				//	Animate = false,
				//	MaxProgress = 10,
				//	Progress = 10,
				//	NameTag = $"{ChatColors.Gold}You are playing on a {ChatColors.Gold}MiNET{ChatColors.Gold} server"
				//};
				//bossBar.SpawnEntity();

				//level.AllowBuild = false;
				//level.AllowBreak = false;

				level.BlockBreak += LevelOnBlockBreak;
				level.BlockPlace += LevelOnBlockPlace;
			};

			server.PlayerFactory.PlayerCreated += (sender, args) =>
			{
				Player player = args.Player;
				player.PlayerJoin += OnPlayerJoin;
				player.PlayerLeave += OnPlayerLeave;
			};
		}

		private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
		{
			if (e.Block.Coordinates.DistanceTo((BlockCoordinates) e.Player.SpawnPosition) < 16 * 4)
			{
				e.Cancel = e.Player.GameMode != GameMode.Creative;
			}
		}

		private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
		{
			if (e.ExistingBlock.Coordinates.DistanceTo((BlockCoordinates) e.Player.SpawnPosition) < 16 * 4)
			{
				e.Cancel = e.Player.GameMode != GameMode.Creative;
			}
		}


		private void OnPlayerLeave(object sender, PlayerEventArgs e)
		{
		}

		private void OnPlayerJoin(object sender, PlayerEventArgs e)
		{
		}
	}
}