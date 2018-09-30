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
using MiNET.Worlds;

namespace MiNET.Console.Config.Implementation
{
	internal class WorldConfiguration: IWorldConfiguration
	{
		private readonly ConfigParser _configParser;

		public WorldConfiguration(ConfigParser configParser)
		{
			_configParser = configParser;
		}

		public GameMode GameMode => _configParser.GetProperty("GameMode", GameMode.Survival);
		public Difficulty Difficulty => _configParser.GetProperty("Difficulty", Difficulty.Normal);
		public string WorldProvider => _configParser.GetProperty("WorldProvider", "anvil");
		public string PCWorldFolder => _configParser.GetProperty("PCWorldFolder", "World");
		public bool SaveEnabled => _configParser.GetProperty("Save.Enabled", false);
		public int SaveInterval => _configParser.GetProperty("Save.Interval", 300);
		public int UnloadInterval => _configParser.GetProperty("Unload.Interval", -1);
		public bool CalculateLights => _configParser.GetProperty("CalculateLights", false);
		public bool CalculateLightsMakeMovie => _configParser.GetProperty("CalculateLights.MakeMovie", false);
		public bool CheckForSafeSpawn => _configParser.GetProperty("CheckForSafeSpawn", false);
		public bool EnableBlockTicking => _configParser.GetProperty("EnableBlockTicking", false);
		public bool EnableChunkTicking => _configParser.GetProperty("EnableChunkTicking", false);
		public int ViewDistance => _configParser.GetProperty("ViewDistance", 11);
		public string Seed => _configParser.GetProperty("seed", "noise");

		public string SuperflatOverworldSeed => _configParser.GetProperty("superflat.overworld",
			"3;minecraft:bedrock,2*minecraft:dirt,minecraft:grass;1;village");

		public string SuperflatNetherSeed => _configParser.GetProperty("superflat.nether",
			"3;minecraft:bedrock,2*minecraft:netherrack,3*minecraft:lava,2*minecraft:netherrack,20*minecraft:air,minecraft:bedrock;1;village");

		public string SuperflatTheEndSeed => _configParser.GetProperty("superflat.theend",
			"3;40*minecraft:air,minecraft:bedrock,7*minecraft:endstone;1;village");
	}
}