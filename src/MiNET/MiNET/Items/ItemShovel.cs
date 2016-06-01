using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemShovel : Item
	{
		internal ItemShovel(short id) : base(id)
		{
			MaxStackSize = 1;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block block = world.GetBlock(blockCoordinates);
			if (block is Grass)
			{
				GrassPath grassPath = new GrassPath
				{
					Coordinates = blockCoordinates,
				};
				world.SetBlock(grassPath);
			}
		}
	}
}