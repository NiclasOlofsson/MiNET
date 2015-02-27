using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	/// <summary>
	///     Blocks are the basic units of structure in Minecraft. Together, they build up the in-game environment and can be
	///     mined and utilized in various fashions.
	/// </summary>
	public class Block
	{
		public BlockCoordinates Coordinates { get; set; }
		public byte Id { get; set; }
		public byte Metadata { get; set; }

		public bool IsReplacible { get; set; }
		public bool IsSolid { get; set; }
		public bool IsBuildable { get; set; }
		public float Durability { get; set; }
		public short FuelEfficiency { get; set; }


		internal Block(byte id)
		{
			Id = id;
			IsSolid = true;
			IsBuildable = true;
			Durability = 0.5f;
			IsReplacible = true;
		}

		public bool CanPlace(Level world)
		{
			return CanPlace(world, Coordinates);
		}

		protected virtual bool CanPlace(Level world, BlockCoordinates blockCoordinates)
		{
			return world.GetBlock(blockCoordinates).IsReplacible;
		}


		public virtual void BreakBlock(Level world)
		{
			world.SetBlock(new Air {Coordinates = Coordinates});
			BlockUpdate(world, Coordinates);
		}

		public virtual bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			// No default placement. Return unhandled.
			return false;
		}

		public virtual bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			// No default interaction. Return unhandled.
			return false;
		}

		public virtual void OnTick(Level level)
		{
		}


		public virtual void BlockUpdate(Level world, BlockCoordinates blockCoordinates)
		{
			BlockCoordinates up = new BlockCoordinates() {X = blockCoordinates.X, Y = blockCoordinates.Y + 1, Z = blockCoordinates.Z};
			/*BlockCoordinates down = new BlockCoordinates() { X = blockCoordinates.X, Y = blockCoordinates.Y - 1, Z = blockCoordinates.Z };
			BlockCoordinates left = new BlockCoordinates() { X = blockCoordinates.X - 1, Y = blockCoordinates.Y, Z = blockCoordinates.Z };
			BlockCoordinates right = new BlockCoordinates() { X = blockCoordinates.X + 1, Y = blockCoordinates.Y, Z = blockCoordinates.Z };
			BlockCoordinates zplus = new BlockCoordinates() { X = blockCoordinates.X, Y = blockCoordinates.Y, Z = blockCoordinates.Z + 1 };
			BlockCoordinates zminus = new BlockCoordinates() { X = blockCoordinates.X, Y = blockCoordinates.Y, Z = blockCoordinates.Z - 1 };
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

		protected BlockCoordinates GetNewCoordinatesFromFace(BlockCoordinates target, BlockFace face)
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

		public virtual ItemStack GetDrops()
		{
			return new ItemStack(Id, 1, 0);
		}

		public virtual Item GetSmelt()
		{
			return null;
		}

		public virtual void DoPhysics(Level level)
		{
		}
	}
}