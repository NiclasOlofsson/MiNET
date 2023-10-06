using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public partial class ItemBucket
	{
		public ItemBucket() : base()
		{
			MaxStackSize = 16;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// Pick up water/lava
			var block = world.GetBlock(blockCoordinates);
			switch (block)
			{
				case Stationary fluid:
					if (fluid.LiquidDepth == 0) // Only source blocks
					{
						switch (block)
						{
							case Lava:
								player.Inventory.AddItem(new ItemLavaBucket(), true);
								break;
							case Water:
								player.Inventory.AddItem(new ItemWaterBucket(), true);
								break;

							default: return false;
						}

						world.SetAir(blockCoordinates);
						Count--;
					}
					return true;
				case Flowing fluid:
					if (fluid.LiquidDepth == 0) // Only source blocks
					{
						switch (block)
						{
							case FlowingLava:
								player.Inventory.AddItem(new ItemLavaBucket(), true);
								break;
							case FlowingWater:
								player.Inventory.AddItem(new ItemWaterBucket(), true);
								break;

							default:
								return false;
						}

						world.SetAir(blockCoordinates);
						Count--;
					}
					return true;
			}

			return false;
		}
	}
}