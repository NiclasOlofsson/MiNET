using System;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemMobHead : Item
	{
		public ItemMobHead(short metadata) : base(397, metadata)
		{
			MaxStackSize = 1;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);
			if (face == BlockFace.Up) // On top of block
			{
				Block skull = new Block(144);
				skull.Coordinates = coor;
				skull.Metadata = 1; // Skull on floor, rotation in block entity
				world.SetBlock(skull);
			}
			else if (face == BlockFace.Down) // At the bottom of block
			{
				// Doesn't work, ignore if that happen. 
				return;
			}
			else
			{
				Block skull = new Block(144);
				skull.Coordinates = coor;
				skull.Metadata = (byte) face; // Skull on floor, rotation in block entity
				world.SetBlock(skull);
			}

			// Then we create and set the sign block entity that has all the intersting data

			var skullBlockEntity = new SkullBlockEntity
			{
				Coordinates = coor,
				Rotation = (byte)((int)(Math.Floor(((player.KnownPosition.Yaw)) * 16 / 360) + 0.5) & 0x0f),
				SkullType = (byte) Metadata
			};


			world.SetBlockEntity(skullBlockEntity);
		}
	}
}