using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Events.Player
{
    /// <summary>
    ///     Dispatched whenever an <see cref="OpenPlayer"/> interacts with a block or air
    /// </summary>
    public class PlayerInteractEvent : PlayerEvent
    {
        /// <summary>
        ///     The type of interaction
        /// </summary>
        public PlayerInteractType InteractType { get; }
        
        /// <summary>
        ///     The item the player was holding
        /// </summary>
        public Item Item { get; }
        
        /// <summary>
        ///     The coordinates of the block the player interacted with
        /// </summary>
        public BlockCoordinates Coordinates { get; }
        
        /// <summary>
        ///     The face that the player hit
        /// </summary>
        public BlockFace Face { get; }
        
        public PlayerInteractEvent(MiNET.Player.Player player, Item item, BlockCoordinates blockCoordinates, BlockFace face) :
            this(player, item, blockCoordinates, face, PlayerInteractType.RightClickBlock)
        {

        }

        public PlayerInteractEvent(MiNET.Player.Player player, Item item, BlockCoordinates coordinates, BlockFace face,
            PlayerInteractType type) : base(player)
        {
            Item = item;
            Coordinates = coordinates;
            Face = face;
            InteractType = type;
        }

        public enum PlayerInteractType
        {
            LeftClickBlock,
            RightClickBlock,
            LeftClickAir,
            RightClickAir,
            
            /// <summary>
            /// Not used.
            /// </summary>
            Physical
        }
    }
}