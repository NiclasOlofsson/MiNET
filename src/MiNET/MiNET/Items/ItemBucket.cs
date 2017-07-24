using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBucket : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemBucket));

		public ItemBucket(short metadata) : base(325, metadata)
		{
			MaxStackSize = 1;
			FuelEfficiency = (short) (Metadata == 10 ? 1000 : 0);
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			if (Metadata == 8 || Metadata == 10) //Prevent some kind of cheating...
			{
				Block block = BlockFactory.GetBlockById((byte) Metadata);
				block.Coordinates = coordinates;
				if (!block.CanPlace(world, blockCoordinates, face)) return;
				world.SetBlock(block);
				block.PlaceBlock(world, player, block.Coordinates, face, faceCoords);
			}
			else if(Metadata == 0) // Empty bucket
			{
				// Pick up water/lava
				var block = world.GetBlock(blockCoordinates);
				if (block is Stationary || block is Flowing)
				{
					if (block.Metadata == 0) // Only source blocks
					{
						world.SetAir(blockCoordinates);
					}
				}
			}

			FuelEfficiency = (short) (Metadata == 10 ? 1000 : 0);
		}
	}
}