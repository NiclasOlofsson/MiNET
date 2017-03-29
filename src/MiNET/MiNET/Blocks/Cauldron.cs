using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Cauldron : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Cauldron));

		public Cauldron() : base(118)
		{
			IsTransparent = true;
			BlastResistance = 10;
			Hardness = 2;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			var itemInHand = player.Inventory.GetItemInHand();

			if (itemInHand is ItemBucket)
			{
				if (itemInHand.Metadata == 8)
				{
					if (Metadata < 8)
					{
						world.SetData(Coordinates, 8);
						itemInHand.Metadata = 0;
						player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
					}
				}
				else if (itemInHand.Metadata == 0)
				{
					if (Metadata > 0)
					{
						world.SetData(Coordinates, 0);
						itemInHand.Metadata = 8;
						player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
					}
				}
			}

			return true; // Handled
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(380)};
		}
	}
}