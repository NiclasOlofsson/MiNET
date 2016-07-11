using System;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSign : ItemBlock
	{
		public ItemSign() : base(323, 0)
		{
			MaxStackSize = 1;
		}

		public override Item GetSmelt()
		{
			return null;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);
			if (face == BlockFace.Up) // On top of block
			{
				// Standing Sign
				var sign = new StandingSign();
				sign.Coordinates = coor;
				// metadata for sign is a value 0-15 that signify the orientation of the sign. Same as PC metadata.
				sign.Metadata = (byte) ((int) (Math.Floor((player.KnownPosition.Yaw + 180)*16/360) + 0.5) & 0x0f);
				world.SetBlock(sign);
			}
			else if (face == BlockFace.North) // At the bottom of block
			{
				// Doesn't work, ignore if that happen. 
				return;
			}
			else
			{
				// Wall sign
				var sign = new WallSign();
				sign.Coordinates = coor;
				sign.Metadata = (byte) face;
				world.SetBlock(sign);
			}

			// Then we create and set the sign block entity that has all the intersting data

			var signBlockEntity = new Sign
			{
				Coordinates = coor
			};


			world.SetBlockEntity(signBlockEntity);
		}
	}
}