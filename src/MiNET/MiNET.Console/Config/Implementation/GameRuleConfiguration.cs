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

namespace MiNET.Console.Config.Providers
{
	internal class GameRuleConfiguration: IGameRuleConfiguration
	{
		public bool DrowningDamage => ConfigParser.GetProperty("GameRule.DrowningDamage", true);
		public bool CommandBlockOutput => ConfigParser.GetProperty("GameRule.CommandblockOutput", true);
		public bool DoTiledrops => ConfigParser.GetProperty("GameRule.DoTiledrops", true);
		public bool DoMobloot => ConfigParser.GetProperty("GameRule.DoMobloot", true);
		public bool KeepInventory => ConfigParser.GetProperty("GameRule.KeepInventory", true);
		public bool DoDaylightcycle => ConfigParser.GetProperty("GameRule.DoDaylightcycle", true);
		public bool DoMobSpawning => ConfigParser.GetProperty("GameRule.DoMobspawning", true);
		public bool DoEntitydrops => ConfigParser.GetProperty("GameRule.DoEntitydrops", true);
		public bool DoFiretick => ConfigParser.GetProperty("GameRule.DoFiretick", true);
		public bool DoWeathercycle => ConfigParser.GetProperty("GameRule.DoWeathercycle", true);
		public bool Pvp => ConfigParser.GetProperty("GameRule.Pvp", true);
		public bool Falldamage => ConfigParser.GetProperty("GameRule.Falldamage", true);
		public bool Firedamage => ConfigParser.GetProperty("GameRule.Firedamage", true);
		public bool Mobgriefing => ConfigParser.GetProperty("GameRule.Mobgriefing", true);
		public bool ShowCoordinates => ConfigParser.GetProperty("GameRule.ShowCoordinates", true);
		public bool NaturalRegeneration => ConfigParser.GetProperty("GameRule.NaturalRegeneration", true);
		public bool TntExplodes => ConfigParser.GetProperty("GameRule.TntExplodes", true);
		public bool SendCommandfeedback => ConfigParser.GetProperty("GameRule.SendCommandfeedback", true);
		public int RandomTickSpeed => ConfigParser.GetProperty("GameRule.RandomTickSpeed", 3);
		public bool PathFinderPrintPath => ConfigParser.GetProperty("Pathfinder.PrintPath", false);
	}
}