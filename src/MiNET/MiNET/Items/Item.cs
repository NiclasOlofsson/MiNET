using Craft.Net.Common;
using MiNET.Worlds;

namespace MiNET.Items
{
	/// <summary>
	///     Items are objects which only exist within the player's inventory and hands - which means, they cannot be placed in
	///     the game world. Some items simply place blocks or entities into the game world when used. They are thus an item
	///     when in the inventory and a block when placed. Some examples of objects which exhibit these properties are item
	///     frames, which turn into an entity when placed, and beds, which turn into a group of blocks when placed. When
	///     equipped, items (and blocks) briefly display their names above the HUD.
	/// </summary>
	public class Item
	{
		public int Id { get; set; }
        public bool IsTool { get; set; }
        public ItemMaterial ItemMaterial { get; set; }

		internal Item(int id)
		{
			Id = id;
            ItemMaterial = ItemMaterial.None;
		}

		public short Metadata { get; set; }

		public virtual void UseItem(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
		}

		protected Coordinates3D GetNewCoordinatesFromFace(Coordinates3D target, BlockFace face)
		{
			switch (face)
			{
				case BlockFace.NegativeY:
					return target + Level.Down;
				case BlockFace.PositiveY:
					return target + Level.Up;
				case BlockFace.NegativeZ:
					return target + Level.East;
				case BlockFace.PositiveZ:
					return target + Level.West;
				case BlockFace.NegativeX:
					return target + Level.South;
				case BlockFace.PositiveX:
					return target + Level.North;
				default:
					return target;
			}
		}
	}
    public enum ItemMaterial
    {
        None = 1,
        Wood = 2,
        Stone = 4,
        Iron = 6,
        Diamond = 8,
        Gold = 12
    }
}