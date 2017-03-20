using System.Collections.Generic;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemHoe : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemHoe));

		internal ItemHoe(short id) : base(id)
		{
			MaxStackSize = 1;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block block = world.GetBlock(blockCoordinates);
			if (block is Grass || block is Dirt || block is GrassPath)
			{
				Farmland farmland = new Farmland
				{
					Coordinates = blockCoordinates,
					Metadata = 0
				};

				if (farmland.FindWater(world, blockCoordinates, new List<BlockCoordinates>(), 0))
				{
					Log.Warn("Found water source");
					farmland.Metadata = 7;
				}

				world.SetBlock(farmland);
			}
		}
	}
}