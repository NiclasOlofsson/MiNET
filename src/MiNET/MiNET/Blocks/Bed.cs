#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Numerics;
using log4net;
using MiNET.BlockEntities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Bed : Block
	{
		public Bed() : base(26)
		{
			BlastResistance = 1;
			Hardness = 0.2f;
			IsTransparent = true;
			//IsFlammable = true; // It can catch fire from lava, but not other means.
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return world.GetBlock(blockCoordinates).IsReplacible;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();

			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

			// Base block, meta sets orientation

			Bed block = new Bed
			{
				Coordinates = coordinates
			};

			switch (direction)
			{
				case 1:
					block.Metadata = 0;
					break; // West
				case 2:
					block.Metadata = 1;
					break; // North
				case 3:
					block.Metadata = 2;
					break; // East
				case 0:
					block.Metadata = 3;
					break; // South 
			}

			if (!block.CanPlace(world, player, blockCoordinates, face)) return true;

			BlockFace lowerFace = BlockFace.None;
			switch (block.Metadata)
			{
				case 0:
					lowerFace = (BlockFace) 3;
					break; // West
				case 1:
					lowerFace = (BlockFace) 4;
					break; // North
				case 2:
					lowerFace = (BlockFace) 2;
					break; // East
				case 3:
					lowerFace = (BlockFace) 5;
					break; // South 
			}

			Bed blockUpper = new Bed
			{
				Coordinates = GetNewCoordinatesFromFace(coordinates, lowerFace),
				Metadata = (byte) (block.Metadata | 0x08)
			};

			if (!blockUpper.CanPlace(world, player, blockCoordinates, face)) return true;

			//TODO: Check down from both blocks, must be solids

			world.SetBlock(block);
			world.SetBlockEntity(new BedBlockEntity
			{
				Coordinates = block.Coordinates,
				Color = (byte) Metadata
			});

			world.SetBlock(blockUpper);
			world.SetBlockEntity(new BedBlockEntity
			{
				Coordinates = blockUpper.Coordinates,
				Color = (byte) Metadata
			});

			return true;
		}

		public override void BreakBlock(Level level, bool silent = false)
		{
			base.BreakBlock(level, silent);

			var other = GetOtherPart();
			level.SetAir(other);
			level.RemoveBlockEntity(other);
		}

		private BlockCoordinates GetOtherPart()
		{
			BlockCoordinates direction = new BlockCoordinates();
			switch (Metadata & 0x07)
			{
				case 0:
					direction = Level.East;
					break; // West
				case 1:
					direction = Level.South;
					break; // South
				case 2:
					direction = Level.West;
					break; // East
				case 3:
					direction = Level.North;
					break; // North 
			}

			// Remove bed
			if ((Metadata & 0x08) != 0x08)
			{
				direction = direction*-1;
			}

			return Coordinates + direction;
		}

		private static readonly ILog Log = LogManager.GetLogger(typeof (Bed));

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			if ((Metadata & 0x04) == 0x04)
			{
				Log.Debug($"Bed at {Coordinates} is already occupied"); // Send proper message to player
				return true;
			}

			SetOccupied(world, true);
			player.IsSleeping = true;
			player.SpawnPosition = blockCoordinates;
			player.BroadcastSetEntityData();
			return true;
		}

		public void SetOccupied(Level world, bool isOccupied)
		{
			var other = world.GetBlock(GetOtherPart());
			if (isOccupied)
			{
				world.SetData(Coordinates, (byte) (Metadata | 0x04));
				world.SetData(other.Coordinates, (byte) (other.Metadata | 0x04));
			}
			else
			{
				world.SetData(Coordinates, (byte) (Metadata & ~0x04));
				world.SetData(other.Coordinates, (byte) (other.Metadata & ~0x04));
			}
		}
	}
}