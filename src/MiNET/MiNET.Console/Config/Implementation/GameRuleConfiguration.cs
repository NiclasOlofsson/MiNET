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

using MiNET.Config.Contracts;

namespace MiNET.Console.Config.Implementation
{
	internal class GameRuleConfiguration: IGameRuleConfiguration
	{
		private readonly ConfigParser _configParser;

		public GameRuleConfiguration(ConfigParser configParser)
		{
			_configParser = configParser;
		}

		public bool DrowningDamage => _configParser.GetProperty("GameRule.DrowningDamage", true);
		public bool CommandBlockOutput => _configParser.GetProperty("GameRule.CommandblockOutput", true);
		public bool DoTiledrops => _configParser.GetProperty("GameRule.DoTiledrops", true);
		public bool DoMobloot => _configParser.GetProperty("GameRule.DoMobloot", true);
		public bool KeepInventory => _configParser.GetProperty("GameRule.KeepInventory", true);
		public bool DoDaylightcycle => _configParser.GetProperty("GameRule.DoDaylightcycle", true);
		public bool DoMobSpawning => _configParser.GetProperty("GameRule.DoMobspawning", true);
		public bool DoEntitydrops => _configParser.GetProperty("GameRule.DoEntitydrops", true);
		public bool DoFiretick => _configParser.GetProperty("GameRule.DoFiretick", true);
		public bool DoWeathercycle => _configParser.GetProperty("GameRule.DoWeathercycle", true);
		public bool Pvp => _configParser.GetProperty("GameRule.Pvp", true);
		public bool Falldamage => _configParser.GetProperty("GameRule.Falldamage", true);
		public bool Firedamage => _configParser.GetProperty("GameRule.Firedamage", true);
		public bool Mobgriefing => _configParser.GetProperty("GameRule.Mobgriefing", true);
		public bool ShowCoordinates => _configParser.GetProperty("GameRule.ShowCoordinates", true);
		public bool NaturalRegeneration => _configParser.GetProperty("GameRule.NaturalRegeneration", true);
		public bool TntExplodes => _configParser.GetProperty("GameRule.TntExplodes", true);
		public bool SendCommandfeedback => _configParser.GetProperty("GameRule.SendCommandfeedback", true);
		public int RandomTickSpeed => _configParser.GetProperty("GameRule.RandomTickSpeed", 3);
		public bool PathFinderPrintPath => _configParser.GetProperty("Pathfinder.PrintPath", false);
	}
}