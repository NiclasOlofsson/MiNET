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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Linq;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Plotter
{
	public class PlotterLevelManager : LevelManager
	{
		public override Level GetLevel(Player player, string name)
		{
			Level level = Levels.FirstOrDefault(l => l.LevelId.Equals(name, StringComparison.InvariantCultureIgnoreCase));
			if (level == null)
			{
				int viewDistance = Config.GetProperty("ViewDistance", 11);

				var worldProvider = new LevelDbProvider()
				{
					MissingChunkProvider = new PlotWorldGenerator(),
				};

				level = new Level(this, name, worldProvider, EntityManager, GameMode.Creative, Difficulty.Normal, viewDistance)
				{
					EnableBlockTicking = Config.GetProperty("EnableBlockTicking", false),
					EnableChunkTicking = Config.GetProperty("EnableChunkTicking", false),
					SaveInterval = Config.GetProperty("Save.Interval", 300),
					UnloadInterval = Config.GetProperty("Unload.Interval", 0),
					DrowningDamage = Config.GetProperty("GameRule.DrowningDamage", true),
					CommandblockOutput = Config.GetProperty("GameRule.CommandblockOutput", true),
					DoTiledrops = Config.GetProperty("GameRule.DoTiledrops", true),
					DoMobloot = Config.GetProperty("GameRule.DoMobloot", true),
					KeepInventory = Config.GetProperty("GameRule.KeepInventory", true),
					DoDaylightcycle = Config.GetProperty("GameRule.DoDaylightcycle", false),
					DoMobspawning = Config.GetProperty("GameRule.DoMobspawning", true),
					DoEntitydrops = Config.GetProperty("GameRule.DoEntitydrops", true),
					DoFiretick = Config.GetProperty("GameRule.DoFiretick", true),
					DoWeathercycle = Config.GetProperty("GameRule.DoWeathercycle", true),
					Pvp = Config.GetProperty("GameRule.Pvp", true),
					Falldamage = Config.GetProperty("GameRule.Falldamage", true),
					Firedamage = Config.GetProperty("GameRule.Firedamage", true),
					Mobgriefing = Config.GetProperty("GameRule.Mobgriefing", true),
					ShowCoordinates = Config.GetProperty("GameRule.ShowCoordinates", true),
					NaturalRegeneration = Config.GetProperty("GameRule.NaturalRegeneration", true),
					TntExplodes = Config.GetProperty("GameRule.TntExplodes", true),
					SendCommandfeedback = Config.GetProperty("GameRule.SendCommandfeedback", true),
					RandomTickSpeed = Config.GetProperty("GameRule.RandomTickSpeed", 3),
				};
				level.Initialize();

				Levels.Add(level);
				OnLevelCreated(new LevelEventArgs(null, level));
			}


			return level;
		}
	}
}