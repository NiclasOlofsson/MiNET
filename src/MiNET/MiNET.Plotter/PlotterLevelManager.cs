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
using System.Linq;
using MiNET.Config;
using MiNET.Config.Contracts;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Plotter
{
	public class PlotterLevelManager : LevelManager
	{
		private static readonly IMiNETConfiguration Config = ConfigurationProvider.MiNetConfiguration;
		public override Level GetLevel(Player player, string name)
		{
			Level level = Levels.FirstOrDefault(l => l.LevelId.Equals(name, StringComparison.InvariantCultureIgnoreCase));
			if (level == null)
			{
				int viewDistance = Config.World.ViewDistance;

				string basePath = Config.World.PCWorldFolder.Trim();

				var worldProvider = new AnvilWorldProvider(basePath)
				{
					MissingChunkProvider = new PlotWorldGenerator(),
					ReadSkyLight = !Config.World.CalculateLights,
					ReadBlockLight = !Config.World.CalculateLights,
				};

				level = new Level(this, name, worldProvider, EntityManager, GameMode.Creative, Difficulty.Normal, viewDistance)
				{
					EnableBlockTicking = Config.World.EnableBlockTicking,
					EnableChunkTicking = Config.World.EnableChunkTicking,
					SaveInterval = Config.World.SaveInterval,
					UnloadInterval = Config.World.UnloadInterval,

					DrowningDamage = Config.GameRules.DrowningDamage,
					CommandblockOutput = Config.GameRules.CommandBlockOutput,
					DoTiledrops = Config.GameRules.DoTiledrops,
					DoMobloot = Config.GameRules.DoMobloot,
					KeepInventory = Config.GameRules.KeepInventory,
					DoDaylightcycle = Config.GameRules.DoDaylightcycle,
					DoMobspawning = Config.GameRules.DoMobSpawning,
					DoEntitydrops = Config.GameRules.DoEntitydrops,
					DoFiretick = Config.GameRules.DoFiretick,
					DoWeathercycle = Config.GameRules.DoWeathercycle,
					Pvp = Config.GameRules.Pvp,
					Falldamage = Config.GameRules.Falldamage,
					Firedamage = Config.GameRules.Firedamage,
					Mobgriefing = Config.GameRules.Mobgriefing,
					ShowCoordinates = Config.GameRules.ShowCoordinates,
					NaturalRegeneration = Config.GameRules.NaturalRegeneration,
					TntExplodes = Config.GameRules.TntExplodes,
					SendCommandfeedback = Config.GameRules.SendCommandfeedback,
					RandomTickSpeed = Config.GameRules.RandomTickSpeed,
				};
				level.Initialize();

				Levels.Add(level);
				OnLevelCreated(new LevelEventArgs(null, level));
			}


			return level;
		}
	}
}