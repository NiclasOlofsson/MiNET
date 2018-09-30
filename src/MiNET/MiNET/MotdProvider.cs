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

using System.Net;
using MiNET.Config;
using MiNET.Config.Contracts;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET
{
	public class MotdProvider
	{
		private static readonly IServerConfiguration ServerConfig = ConfigurationProvider.MiNetConfiguration.Server;
		public string Motd { get; set; }

		public string SecondLine { get; set; }

		public int MaxNumberOfPlayers { get; set; }

		public int NumberOfPlayers { get; set; }

		public MotdProvider()
		{
			Motd = ServerConfig.Motd;
			SecondLine = ServerConfig.MotdSecond;
		}

		public virtual string GetMotd(ServerInfo serverInfo, IPEndPoint caller, bool eduMotd = false)
		{
			NumberOfPlayers = serverInfo.NumberOfPlayers;
			MaxNumberOfPlayers = serverInfo.MaxNumberOfPlayers;

			var protocolVersion = McpeProtocolInfo.ProtocolVersion.ToString();
			var clientVersion = McpeProtocolInfo.GameVersion;
			var edition = "MCPE";

			if (eduMotd)
			{
				protocolVersion = "261";
				clientVersion = "1.4";
				edition = "MCEE";
			}

			return string.Format($"{edition};{Motd};{protocolVersion};{clientVersion};{NumberOfPlayers};{MaxNumberOfPlayers};{Motd.GetHashCode() + caller.Address.Address + caller.Port};{SecondLine};Survival;");
		}
	}
}