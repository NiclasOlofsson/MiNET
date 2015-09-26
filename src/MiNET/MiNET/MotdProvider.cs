using MiNET.Utils;

namespace MiNET
{
	public class MotdProvider
	{
		public string Motd { get; set; }

		public MotdProvider()
		{
			Motd = Config.GetProperty("motd", "MiNET: MCPE Server");
		}

		public virtual string GetMotd(ServerInfo serverInfo)
		{
			return string.Format(@"MCPE;{0};34;0.12.1;{1};{2}", Motd, serverInfo.NumberOfPlayers, serverInfo.MaxNumberOfPlayers);
		}
	}
}
