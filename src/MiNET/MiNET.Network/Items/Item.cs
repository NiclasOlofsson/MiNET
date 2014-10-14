using Craft.Net.Common;
using MiNET.Network.Worlds;

namespace MiNET.Network.Items
{
	public abstract class Item
	{
		public short Metadata { get; set; }

		public abstract void UseItem(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face);

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
}