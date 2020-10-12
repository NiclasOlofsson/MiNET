using MiNET.Net;

namespace MiNET.Events.Player
{
	public class PlayerSettingsRequestEvent : PlayerEvent
	{
		public PlayerSettingsRequestEvent(MiNET.Player.Player player, McpeServerSettingsRequest request) : base(player)
		{
		}
	}
}
