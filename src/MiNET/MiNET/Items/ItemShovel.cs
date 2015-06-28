using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemShovel : Item
	{
		internal ItemShovel(int id, short metadata) : base(id, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block tile = world.GetBlock(blockCoordinates);
			if (tile.Id == 2)
			{
				PressedDirt pressedDirt = new PressedDirt
				{
					Coordinates = blockCoordinates,
					Metadata = 0
				};
				world.SetBlock(pressedDirt);
			}
		}
	}
}

//TODO: Make shovels destroy blocks faster