using System.Numerics;
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
		public byte Id { get; protected set; }
		public byte Metadata { get; set; }

		public float Hardness { get; protected set; }
		public float BlastResistance { get; protected set; }
		public short FuelEfficiency { get; protected set; }
		public float FrictionFactor { get; protected set; }
		public int LightLevel { get; protected set; }

		public bool IsReplacible { get; protected set; }
		public bool IsSolid { get; protected set; }
		public bool IsBuildable { get; protected set; }
		public bool IsTransparent { get; protected set; }
		public bool IsFlammable { get; protected set; }

		public Block(byte id)
		{
			Id = id;

			IsSolid = true;
			IsBuildable = true;
			IsReplacible = false;
			IsTransparent = false;
			IsFlammable = false;

			Hardness = 0;
			BlastResistance = 0;
			FuelEfficiency = 0;
			FrictionFactor = 0.6f;
			LightLevel = 0;
		}

		public bool CanPlace(Level world, BlockFace face)
		{
			return CanPlace(world, Coordinates, face);
		}

		protected virtual bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockFace face)
		{
			return world.GetBlock(blockCoordinates).IsReplacible;
		}

		public virtual void BreakBlock(Level world)
		{
			world.SetBlock(new Air {Coordinates = Coordinates});
			BlockUpdate(world, Coordinates);
		}

		public virtual bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// No default placement. Return unhandled.
			return false;
		}

		public virtual bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			// No default interaction. Return unhandled.
			return false;
		}

		public virtual bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			// No default interaction. Return unhandled.
			return Interact(world, player, blockCoordinates, face);
		}

		public virtual void OnTick(Level level)
		{
		}


		public virtual void BlockUpdate(Level world, BlockCoordinates blockCoordinates)
		{
			/*BlockCoordinates up = new BlockCoordinates() {X = blockCoordinates.X, Y = blockCoordinates.Y + 1, Z = blockCoordinates.Z};
			/*BlockCoordinates down = new BlockCoordinates() { X = blockCoordinates.X, Y = blockCoordinates.Y - 1, Z = blockCoordinates.Z };
			BlockCoordinates left = new BlockCoordinates() { X = blockCoordinates.X - 1, Y = blockCoordinates.Y, Z = blockCoordinates.Z };
			BlockCoordinates right = new BlockCoordinates() { X = blockCoordinates.X + 1, Y = blockCoordinates.Y, Z = blockCoordinates.Z };
			BlockCoordinates zplus = new BlockCoordinates() { X = blockCoordinates.X, Y = blockCoordinates.Y, Z = blockCoordinates.Z + 1 };
			BlockCoordinates zminus = new BlockCoordinates() { X = blockCoordinates.X, Y = blockCoordinates.Y, Z = blockCoordinates.Z - 1 };
			
			//All other directions are in here too, however currently we only use this to update fire so we only check the block above.

			if (world.GetBlock(up).Id == 51)
			{
				world.SetBlock(new Air {Coordinates = up});
			}*/


			//This code is really not something we wanna keep :-(
		}

		public float GetHardness()
		{
			return Hardness/5.0F;
		}

		public double GetMineTime(Item miningTool)
		{
			int multiplier = (int) miningTool.ItemMaterial;
			return Hardness*(1.5*multiplier);
		}

		protected BlockCoordinates GetNewCoordinatesFromFace(BlockCoordinates target, BlockFace face)
		{
			switch (face)
			{
				case BlockFace.Down:
					return target + Level.Down;
				case BlockFace.Up:
					return target + Level.Up;
				case BlockFace.East:
					return target + Level.East;
				case BlockFace.West:
					return target + Level.West;
				case BlockFace.North:
					return target + Level.South;
				case BlockFace.South:
					return target + Level.North;
				default:
					return target;
			}
		}

		public virtual Item[] GetDrops()
		{
			return new Item[] { new ItemBlock(this, Metadata) {Count = 1} };
		}

		public virtual Item GetSmelt()
		{
			return null;
		}

		public virtual void DoPhysics(Level level)
		{
		}

		public virtual BoundingBox GetBoundingBox()
		{
			return new BoundingBox(Coordinates, Coordinates + 1);
		}
	}
}