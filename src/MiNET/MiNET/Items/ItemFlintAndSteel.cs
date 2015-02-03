using System;
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
					var fire = new Fire
					{
						Coordinates = affectedBlock.Coordinates
					};
					world.SetBlock(fire);
				}
			}
			else
			{
				world.SetBlock(new Air() {Coordinates = block.Coordinates});
				new PrimedTnt(world)
				{
					KnownPosition = new PlayerPosition3D(blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z),
					Fuse = (byte) (new Random().Next(0, 20) + 10)
				}.SpawnEntity();
			}
		}
	}
}