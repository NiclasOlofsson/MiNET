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
			if (world.GetBlock(blockCoordinates).Id != 46)
			{
				if (world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.PositiveY)).Id == 0)
				{
					var fire = new Block(51)
					{
						Coordinates = GetNewCoordinatesFromFace(blockCoordinates, BlockFace.PositiveY)
					};
					world.SetBlock(fire);
				}
			}
			else
			{
				var primedTnt = new PrimedTnt(world);
				primedTnt.KnownPosition = new PlayerPosition3D()
				{
					X = blockCoordinates.X,
					Y = blockCoordinates.Y,
					Z = blockCoordinates.Z,
				};
				primedTnt.EntityId = 10065;
				primedTnt.Fuse = 80;

				primedTnt.SpawnEntity();
			}
		}
	}
}