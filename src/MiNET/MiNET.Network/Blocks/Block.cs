using Craft.Net.Common;
using MiNET.Network.Worlds;

namespace MiNET.Network.Blocks
{
	public class Block
	{
		public Coordinates3D Coordinates { get; set; }
		public byte Id { get; set; }
		public byte Metadata { get; set; }
		public bool IsReplacible { get; set; }
		public bool IsSolid { get; set; }

		public Block(byte id)
		{
			Id = id;
			IsSolid = true;
		}

		public bool CanPlace(Level world)
		{
			return CanPlace(world, Coordinates);
		}

		public virtual bool CanPlace(Level world, Coordinates3D blockCoordinates)
		{
			return world.GetBlock(blockCoordinates).IsReplacible;
		}


		public virtual void BreakBlock(Level level)
		{
			level.SetBlock(new BlockAir {Coordinates = Coordinates});
		}

		public virtual bool Interact(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			return false;
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
}