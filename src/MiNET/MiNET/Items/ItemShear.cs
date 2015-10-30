using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemShovel : Item
	{
		internal ItemShovel(int id, short metadata) : base(id, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block block = world.GetBlock(blockCoordinates);
			//metadata blockmeta = GetMeta(block);
			
			Entity entity = world.GetEntity(blockCoordinates);
			//Add a entity location handler
			
			//Add a block handler
			
			if (block is 18) //leaves
			{
			/**
				switch(blockmeta)
				{
					case 0:
						//give player an oak leave
						break;
					case 1:
						//give player a spruce leave
						break;
					case 2:
						//give player a birch leave
						break;
					case 3:
						//give player a jungle leave
						break;
					case 4:
						//give player an acacia leave
						break;
					case 5:
						//give player a dark oak leave
						break;
				}
				**/
			
				Air air = new Air
				{
					Coordinates = blockCoordinates,
					Metadata = 0
				};
				world.SetBlock(air);
			}
			
			if (block is 30) //cobweb
			{
				Air air = new Air
				{
					Coordinates = blockCoordinates,
					Metadata = 0
				};
				world.SetBlock(air);
				
				//give player a cobweb (or string)
			}
			
			if (block is 31) //tall grass | fern
			{
				Air air = new Air
				{
					Coordinates = blockCoordinates,
					Metadata = 0
				};
				world.SetBlock(air);
				
				//give player a grass
			}
			
			if (block is 32) //dead bush
			{
				Air air = new Air
				{
					Coordinates = blockCoordinates,
					Metadata = 0
				};
				world.SetBlock(air);
				
				//give player a dead bush
			}
			
			if (block is 35) //wool
			{
			/**
				switch(blockmeta)
				{
					case 0:
						//give player wool with meta
						break;
					case 1:
						//give player wool with meta
						break;
					case 2:
						//give player wool with meta
						break;
					case 3:
						//give player wool with meta
						break;
					case 4:
						//give player wool with meta
						break;
					case 5:
						//give player wool with meta
						break;
					case 6:
						//give player wool with meta
						break;
					case 7:
						//give player wool with meta
						break;
					case 8:
						//give player wool with meta
						break;
					case 9:
						//give player wool with meta
						break;
					case 10:
						//give player wool with meta
						break;
					case 11:
						//give player wool with meta
						break;
					case 12:
						//give player wool with meta
						break;
					case 13:
						//give player wool with meta
						break;
					case 14:
						//give player wool with meta
						break;
					case 15:
						//give player wool with meta
						break;
				}
				**/
				
				Air air = new Air
				{
					Coordinates = blockCoordinates,
					Metadata = 0
				};
				world.SetBlock(air);
			}
			
			if (block is 106) //vines
			{
			
			}
			
			//Add an entity handler
			
			if (entity is Sheep)
			{
				
			}
			
			if (entity is Mooshroom)
			{
			
			}
		}
	}
}
