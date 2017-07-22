using System.Numerics;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Cake : Block
	{
		public Cake() : base(92)
		{
			IsTransparent = true;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (Metadata == 0) return new Item[] { ItemFactory.GetItem(354, 0, 1) };
			return new Item[0];
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			if (Metadata < 6)
			{
				Metadata++;
				world.SetBlock(this);
			}
			else if (Metadata >= 6)
			{
				world.BreakBlock(this);
			}

			if (player.GameMode != GameMode.Creative)
				player.HungerManager.IncreaseFoodAndSaturation(null, 2, 0.4);

			return true;
		}
	}
}