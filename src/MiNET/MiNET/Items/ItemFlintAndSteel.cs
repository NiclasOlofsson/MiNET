using Craft.Net.Common;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemFlintAndSteel : Item
	{
		public ItemFlintAndSteel() : base(259)
		{
		}

		public override void UseItem(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			var block = world.GetBlock(blockCoordinates);
			if (block.Id != 46)
			{
				var affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.PositiveY));
				if (affectedBlock.Id == 0)
				{
					var fire = new Block(51)
					{
						Coordinates = affectedBlock.Coordinates
					};
					world.SetBlock(fire);
				}
			}
			else
			{
				world.SetBlock(new Air() {Coordinates = block.Coordinates});

				new Explosion().SpawnTNT(blockCoordinates, world);
			}
		}
	}
}