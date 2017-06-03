using System;
using System.Net;
using MiNET.Utils;

namespace MiNET
{
	public class MotdProvider
	{
		public string Motd { get; set; }

		public string SecondLine { get; set; }

		public int MaxNumberOfPlayers { get; set; }

		public int NumberOfPlayers { get; set; }

		public MotdProvider()
		{
			Motd = Config.GetProperty("motd", "MiNET: MCPE Server");
			SecondLine = Config.GetProperty("motd-2nd", "MiNET");
		}

		public virtual string GetMotd(ServerInfo serverInfo, IPEndPoint caller)
		{
			NumberOfPlayers = serverInfo.NumberOfPlayers;
			MaxNumberOfPlayers = serverInfo.MaxNumberOfPlayers;

			return string.Format($"MCPE;{Motd};113;1.1.0;{NumberOfPlayers};{MaxNumberOfPlayers};{Motd.GetHashCode() + caller.Address.Address + caller.Port};{SecondLine};Survival;");
		}
	}
}