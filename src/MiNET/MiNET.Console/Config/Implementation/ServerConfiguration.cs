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
	internal class ServerConfiguration: IServerConfiguration
	{
		private readonly ConfigParser _configParser;

		public ServerConfiguration(ConfigParser configParser)
		{
			_configParser = configParser;
		}

		public ServerRole ServerRole => _configParser.GetProperty("ServerRole", ServerRole.Full);
		public string Ip => _configParser.GetProperty("ip", "0.0.0.0");
		public int Port => _configParser.GetProperty("port", 19132);
		public int InactivityTimeout => _configParser.GetProperty("InactivityTimeout", 8500);
		public int ResendThreshold => _configParser.GetProperty("ResendThreshold", 10);
		public bool ForceOrderingForAll => _configParser.GetProperty("ForceOrderingForAll", false);
		public int MaxNumberOfPlayers => _configParser.GetProperty("MaxNumberOfPlayers", 1000);
		public int MaxNumberOfConcurrentConnects => _configParser.GetProperty("MaxNumberOfConcurrentConnects", MaxNumberOfPlayers);
		public int MinWorkerThreads => _configParser.GetProperty("MinWorkerThreads", -1);
		public int MinCompletionPortThreads => _configParser.GetProperty("MinCompletionPortThreads", -1);
		public bool EnableQuery => _configParser.GetProperty("EnableQuery", false);
		public string Motd => _configParser.GetProperty("motd", "MiNET: MCPE Server");
		public string MotdSecond => _configParser.GetProperty("motd-2nd", "MiNET");
		public string AuthorizeErrorMessage => _configParser.GetProperty("Authorize.ErrorMessage", "§cInsufficient permissions. Requires {1}, but player had {0}");
	}
}