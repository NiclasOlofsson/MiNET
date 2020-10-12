namespace MiNET.Events.Player
{
    /// <summary>
    ///     Dispatched whenever a <see cref="OpenPlayer"/> was created by the <see cref="OpenPlayerManager"/>
    /// </summary>
    public class PlayerCreatedEvent : PlayerEvent
    {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="player">The player that got created</param>
        public PlayerCreatedEvent(MiNET.Player.Player player) : base(player)
        {
        }
    }
}
