﻿namespace MiNET.Events.Player
{
    /// <summary>
    ///     Dispatched whenever a <see cref="OpenPlayer"/>'s food level changes
    /// </summary>
    public class FoodLevelChangeEvent : PlayerEvent
    {
        /// <summary>
        ///     The players old food level
        /// </summary>
        public int OldLevel { get; set; }
        
        /// <summary>
        ///     The players new food level
        /// </summary>
        public int NewLevel { get; set; }
        
        /// <summary>
        ///     The players old exhaustion value
        /// </summary>
        public double OldExhaustion { get; set; }
        
        /// <summary>
        ///     The players new exhaustion value
        /// </summary>
        public double NewExhaustion { get; set; }
        
        /// <summary>
        ///     The players old saturation level
        /// </summary>
        public double OldSaturation { get; set; }
        
        /// <summary>
        ///     The players new saturation level
        /// </summary>
        public double NewSaturation { get; set; }
        
        public FoodLevelChangeEvent(MiNET.Player player, int oldLevel, int newLevel, double oldExhaustion, double newExhaustion, double oldSaturation, double newSaturation) : base(player)
        {
            OldLevel = oldLevel;
            NewLevel = newLevel;
            OldSaturation = oldSaturation;
            NewSaturation = newSaturation;
            OldExhaustion = oldExhaustion;
            NewExhaustion = newExhaustion;
        }
    }
}
