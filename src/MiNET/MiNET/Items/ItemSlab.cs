using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSlab : ItemBlock
	{
		public ItemSlab(short id, short metadata) : base(id, metadata)
		{
		}

		public override Item GetSmelt()
		{
			return null;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// 8 = 1000
			byte upperBit = 0x08;
			// 7 = 0111
			byte materialMask = 0x07;

			Block existingBlock = world.GetBlock(blockCoordinates);

			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			Block newBlock = world.GetBlock(coordinates);

			if (face == BlockFace.Up && faceCoords.Y == 0.5 && existingBlock.Id == Id && (existingBlock.Metadata & materialMask) == Metadata)
			{
				// Replace with double block
				SetDoubleSlab(world, blockCoordinates);
				return;
			}

			if (face == BlockFace.Down && faceCoords.Y == 0.5 && (existingBlock.Metadata & materialMask) == Metadata)
			{
				// Replace with double block
				SetDoubleSlab(world, blockCoordinates);
				return;
			}

			if (newBlock.Id != Id || (newBlock.Metadata & materialMask) != Metadata)
			{
				Block slab = BlockFactory.GetBlockById((byte) (Id));
				slab.Coordinates = coordinates;
				slab.Metadata = (byte) Metadata;
				if (face != BlockFace.Up && faceCoords.Y > 0.5 || (face == BlockFace.Down && faceCoords.Y == 0.0))
				{
					slab.Metadata |= upperBit;
				}
				world.SetBlock(slab);
				return;
			}

			// Same material in existing block, make double slab

			{
				// Create double slab, replace existing
				SetDoubleSlab(world, coordinates);
			}
		}

		private void SetDoubleSlab(Level world, BlockCoordinates coordinates)
		{
			Block slab = BlockFactory.GetBlockById((byte) (Id - 1));
			slab.Coordinates = coordinates;
			slab.Metadata = (byte) Metadata;
			world.SetBlock(slab);
		}
	}
}