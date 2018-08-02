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
	internal class ServerConfiguration: IServerConfiguration
	{
		public ServerRole ServerRole => ConfigParser.GetProperty("ServerRole", ServerRole.Full);
		public string Ip => ConfigParser.GetProperty("ip", "0.0.0.0");
		public int Port => ConfigParser.GetProperty("port", 19132);
		public int InactivityTimeout => ConfigParser.GetProperty("InactivityTimeout", 8500);
		public int ResendThreshold => ConfigParser.GetProperty("ResendThreshold", 10);
		public bool ForceOrderingForAll => ConfigParser.GetProperty("ForceOrderingForAll", false);
		public int MaxNumberOfPlayers => ConfigParser.GetProperty("MaxNumberOfPlayers", 1000);
		public int MaxNumberOfConcurrentConnects => ConfigParser.GetProperty("MaxNumberOfConcurrentConnects", MaxNumberOfPlayers);
		public int MinWorkerThreads => ConfigParser.GetProperty("MinWorkerThreads", -1);
		public int MinCompletionPortThreads => ConfigParser.GetProperty("MinCompletionPortThreads", -1);
		public bool EnableQuery => ConfigParser.GetProperty("EnableQuery", false);
		public string Motd => ConfigParser.GetProperty("motd", "MiNET: MCPE Server");
		public string MotdSecond => ConfigParser.GetProperty("motd-2nd", "MiNET");
		public string AuthorizeErrorMessage => ConfigParser.GetProperty("Authorize.ErrorMessage", "§cInsufficient permissions. Requires {1}, but player had {0}");
	}
}