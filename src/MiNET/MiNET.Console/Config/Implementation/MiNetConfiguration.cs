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
	internal class MiNetConfiguration: IMiNETConfiguration
	{
		public MiNetConfiguration(IServerConfiguration server, IWorldConfiguration world,
			ISecurityConfiguration security, IPlayerConfiguration player,
			IPluginConfiguration plugin, IDebugConfiguration debug,
			IGameRuleConfiguration gameRules)
		{
			Server = server;
			World = world;
			Security = security;
			Player = player;
			Plugin = plugin;
			Debug = debug;
			GameRules = gameRules;
		}

		public IServerConfiguration Server { get; }
		public IWorldConfiguration World { get; }
		public ISecurityConfiguration Security { get; }
		public IPlayerConfiguration Player { get; }
		public IPluginConfiguration Plugin { get; }
		public IDebugConfiguration Debug { get; }
		public IGameRuleConfiguration GameRules { get; }
	}
}