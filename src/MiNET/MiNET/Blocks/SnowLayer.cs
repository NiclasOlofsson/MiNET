using System.Numerics;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class SnowLayer : Block
	{
		public SnowLayer() : base(78)
		{
			IsTransparent = true;
			BlastResistance = 0.5f;
			Hardness = 0.1f;
		}

		public override Item[] GetDrops(Item tool)
		{
			// One per layer, plus one.
			return new[] { ItemFactory.GetItem(332, 0, (byte)(Metadata + 2)) };
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var existingBlock = world.GetBlock(blockCoordinates) as SnowLayer;
			if (existingBlock != null)
			{
				if (existingBlock.Metadata < 7)
				{
					existingBlock.Metadata++;
					world.SetBlock(existingBlock);
					return true;
				}
			}

			return false;
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (BlockLight > 12)
			{
				level.SetAir(Coordinates);
			}
		}
	}
}