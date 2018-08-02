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

namespace MiNET.Console.Config.Providers
{
	internal class WorldConfiguration: IWorldConfiguration
	{
		public GameMode GameMode => ConfigParser.GetProperty("GameMode", GameMode.Survival);
		public Difficulty Difficulty => ConfigParser.GetProperty("Difficulty", Difficulty.Normal);
		public string WorldProvider => ConfigParser.GetProperty("WorldProvider", "anvil");
		public string PCWorldFolder => ConfigParser.GetProperty("PCWorldFolder", "World");
		public bool SaveEnabled => ConfigParser.GetProperty("Save.Enabled", false);
		public int SaveInterval => ConfigParser.GetProperty("Save.Interval", 300);
		public int UnloadInterval => ConfigParser.GetProperty("Unload.Interval", -1);
		public bool CalculateLights => ConfigParser.GetProperty("CalculateLights", false);
		public bool CalculateLightsMakeMovie => ConfigParser.GetProperty("CalculateLights.MakeMovie", false);
		public bool CheckForSafeSpawn => ConfigParser.GetProperty("CheckForSafeSpawn", false);
		public bool EnableBlockTicking => ConfigParser.GetProperty("EnableBlockTicking", false);
		public bool EnableChunkTicking => ConfigParser.GetProperty("EnableChunkTicking", false);
		public int ViewDistance => ConfigParser.GetProperty("ViewDistance", 11);
		public string Seed => ConfigParser.GetProperty("seed", "noise");

		public string SuperflatOverworldSeed => ConfigParser.GetProperty("superflat.overworld",
			"3;minecraft:bedrock,2*minecraft:dirt,minecraft:grass;1;village");

		public string SuperflatNetherSeed => ConfigParser.GetProperty("superflat.nether",
			"3;minecraft:bedrock,2*minecraft:netherrack,3*minecraft:lava,2*minecraft:netherrack,20*minecraft:air,minecraft:bedrock;1;village");

		public string SuperflatTheEndSeed => ConfigParser.GetProperty("superflat.theend",
			"3;40*minecraft:air,minecraft:bedrock,7*minecraft:endstone;1;village");
	}
}