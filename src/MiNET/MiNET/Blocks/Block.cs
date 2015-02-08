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
			IsReplacible = true;
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
			world.SetBlock(new Air {Coordinates = Coordinates});
			BlockUpdate(world, Coordinates);
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

		public virtual void BlockUpdate(Level world, Coordinates3D blockCoordinates)
		{
			Coordinates3D up = new Coordinates3D() {X = blockCoordinates.X, Y = blockCoordinates.Y + 1, Z = blockCoordinates.Z};
			/*Coordinates3D down = new Coordinates3D() { X = blockCoordinates.X, Y = blockCoordinates.Y - 1, Z = blockCoordinates.Z };
			Coordinates3D left = new Coordinates3D() { X = blockCoordinates.X - 1, Y = blockCoordinates.Y, Z = blockCoordinates.Z };
			Coordinates3D right = new Coordinates3D() { X = blockCoordinates.X + 1, Y = blockCoordinates.Y, Z = blockCoordinates.Z };
			Coordinates3D zplus = new Coordinates3D() { X = blockCoordinates.X, Y = blockCoordinates.Y, Z = blockCoordinates.Z + 1 };
			Coordinates3D zminus = new Coordinates3D() { X = blockCoordinates.X, Y = blockCoordinates.Y, Z = blockCoordinates.Z - 1 };
			*/
			//All other directions are in here too, however currently we only use this to update fire so we only check the block above.

			if (world.GetBlock(up).Id == 51)
			{
				world.SetBlock(new Air {Coordinates = up});
			}
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