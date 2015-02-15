using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBucket : Item
	{
		internal ItemBucket(short metadata) : base(325, metadata)
		{
		}


		public override short GetFuelEfficiency()
		{
			return (short) (Metadata == 10 ? 1000 : 0);
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			if (Metadata == 8 || Metadata == 10) //Prevent some kind of cheating...
			{
				Block targetblock = new Block((byte) Metadata)
				{
					Coordinates = coordinates
				};
				player.Level.SetBlock(targetblock);
			}
		}
	}
}