using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemCake : Item
	{
		public ItemCake() : base(354)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			Block newBlock = world.GetBlock(coordinates);
			if (newBlock.IsReplacible)
			{
				world.SetBlock(new Cake()
				{
					Coordinates = coordinates
				});
			}
		}
	}
}
