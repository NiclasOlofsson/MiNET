using Craft.Net.Common;
using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	/// <summary>
	///     Blocks are the basic units of structure in Minecraft. Together, they build up the in-game environment and can be
	///     mined and utilized in various fashions.
	/// </summary>
	public class Block
	{
		public Coordinates3D Coordinates { get; set; }
		public byte Id { get; set; }
		public byte Metadata { get; set; }
		public bool IsReplacible { get; set; }
		public bool IsSolid { get; set; }
		public float Durability { get; set; }

		internal Block(byte id)
		{
			Id = id;
			IsSolid = true;
			Durability = 0.5f;
		}

		public bool CanPlace(Level world)
		{
			return CanPlace(world, Coordinates);
		}

		protected virtual bool CanPlace(Level world, Coordinates3D blockCoordinates)
		{
			return world.GetBlock(blockCoordinates).IsReplacible;
		}


		public virtual void BreakBlock(Level world)
		{
			world.SetBlock(new BlockAir {Coordinates = Coordinates});
		}

		public virtual bool PlaceBlock(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			// No default placement. Return unhandled.
			return false;
		}

		public virtual bool Interact(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			// No default interaction. Return unhandled.
			return false;
		}

		public float GetHardness()
		{
			return Durability/5.0F;
		}

		public double GetMineTime(Item miningTool)
		{
			int multiplier = (int) miningTool.ItemMaterial;
			return Durability*(1.5*multiplier);
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