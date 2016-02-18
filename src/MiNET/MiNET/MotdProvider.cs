using MiNET.Utils;

namespace MiNET
{
	public class MotdProvider
	{
		public string Motd { get; set; }

		public int MaxNumberOfPlayers { get; set; }

		public int NumberOfPlayers { get; set; }

		public MotdProvider()
		{
			Motd = Config.GetProperty("motd", "MiNET: MCPE Server");
		}

		public virtual string GetMotd(ServerInfo serverInfo)
		{
			NumberOfPlayers = serverInfo.NumberOfPlayers;
			MaxNumberOfPlayers = serverInfo.MaxNumberOfPlayers;

			return string.Format(@"MCPE;{0};45;0.14;{1};{2}", Motd, NumberOfPlayers, MaxNumberOfPlayers);
		}

	}
}
