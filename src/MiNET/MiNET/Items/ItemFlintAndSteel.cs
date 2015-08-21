using System;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemFlintAndSteel : Item
	{
		public ItemFlintAndSteel(short metadata) : base(259, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var block = world.GetBlock(blockCoordinates);
			if (block is Tnt)
			{
				world.SetBlock(new Air() {Coordinates = block.Coordinates});
				new PrimedTnt(world)
				{
					KnownPosition = new PlayerLocation(blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z),
					Fuse = (byte) (new Random().Next(0, 20) + 10)
				}.SpawnEntity();
			}
			else if(block is Glass || block is Ice || block is Snow || block is BrickStairs || block is Farmland || block is GrassPath || block is WoodSlab || block is StoneSlab){
				return true;
			}
			else if (block.IsSolid)
			{
				var affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Up));
				if (affectedBlock.Id == 0)
				{
					var fire = new Fire
					{
						Coordinates = affectedBlock.Coordinates
					};
					world.SetBlock(fire);
				}
			}
		}
	}
}