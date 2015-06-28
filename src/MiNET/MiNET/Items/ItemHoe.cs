using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemHoe : Item
	{
		internal ItemHoe(int id, short metadata) : base(id, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block tile = world.GetBlock(blockCoordinates);
			if (tile.Id == 2 || tile.Id == 3 || tile.Id == 198)
			{
				Farmland farm = new Farmland
				{
					Coordinates = blockCoordinates,
					Metadata = 0
				};
				world.SetBlock(farm);
			}
		}
	}
}