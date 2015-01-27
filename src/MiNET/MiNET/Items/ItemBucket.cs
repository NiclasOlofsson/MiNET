using MiNET.Worlds;
using Craft.Net.Common;
using MiNET.Blocks;

namespace MiNET.Items
{
	public class ItemBucket : Item
	{
		internal ItemBucket() : base(325)
		{

		}

		public override void UseItem(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			short Meta = this.Metadata;
			var coordinates = GetNewCoordinatesFromFace (blockCoordinates, face);
			Block targetblock;
			if (Meta == 8 || Meta == 10) //Prevent some kind of cheating...
			{
				targetblock = new Block ((byte)Meta);
				targetblock.Coordinates = coordinates;
				player.Level.SetBlock (targetblock);
			} 
			/*else if (Meta == 0) 
			{
				Block target = player.Level.GetBlock (coordinates);
				if (target.Id == 8 || target.Id == 9)
				{
					player.Level.BreakBlock (world, player, coordinates); //Destroy the picked up water block
				} 
				else if (target.Id == 10 || target.Id == 11)
				{
					player.Level.BreakBlock (world, player, coordinates); //Destroy the picked up lava block
				}
			}*/

			//Commented out above: Using the buckets to remove a liquid source. (Water / Lava)
			//Reason: I did't see a way for me to change a players inventory.

		}
	}
}

