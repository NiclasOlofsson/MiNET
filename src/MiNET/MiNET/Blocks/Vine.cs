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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Vine : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Vine));

		public Vine() : base(106)
		{
			IsSolid = false;
			IsTransparent = true;
			BlastResistance = 1;
			Hardness = 0.2f;
			IsFlammable = true;
			IsReplacible = true;
		}

		private const byte North = 0x01;
		private const byte East = 0x02;
		private const byte South = 0x04;
		private const byte West = 0x08;

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			bool canPlace = base.CanPlace(world, player, blockCoordinates, targetCoordinates, face);
			if (!canPlace) return false;

			var block = world.GetBlock(Coordinates);
			if (block is Vine) return false;

			return true;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block block = world.GetBlock(Coordinates);

			if (block is Vine)
			{
				Metadata = block.Metadata;
			}

			byte direction;
			switch (face)
			{
				case BlockFace.North:
					direction = North;
					break;
				case BlockFace.East:
					direction = East;
					break;
				case BlockFace.South:
					direction = South;
					break;
				case BlockFace.West:
					direction = West;
					break;
				default:
					return true; // Do nothing
			}

			if ((Metadata & direction) == direction) return true; // Already have this face covered

			Metadata = direction;

			world.SetBlock(this);
			return true;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			//Block block = level.GetBlock(blockCoordinates);
			//if (!(block is Air)) return;

			Block onTop = level.GetBlock(Coordinates + Level.Up);

			bool hasNorth = (Metadata & North) == North;
			bool hasEast = (Metadata & East) == East;
			bool hasSouth = (Metadata & South) == South;
			bool hasWest = (Metadata & West) == West;

			bool hasNorthTop = (onTop.Metadata & North) == North;
			bool hasEastTop = (onTop.Metadata & East) == East;
			bool hasSouthTop = (onTop.Metadata & South) == South;
			bool hasWestTop = (onTop.Metadata & West) == West;

			bool haveFaceBlock = false;

			if (hasNorth && level.GetBlock(Coordinates + Level.South).IsSolid)
			{
				haveFaceBlock = true;
			}
			else if (hasEast && level.GetBlock(Coordinates + Level.West).IsSolid)
			{
				haveFaceBlock = true;
			}
			else if (hasSouth && level.GetBlock(Coordinates + Level.North).IsSolid)
			{
				haveFaceBlock = true;
			}
			else if (hasWest && level.GetBlock(Coordinates + Level.East).IsSolid)
			{
				haveFaceBlock = true;
			}

			bool hasVineTop = false;
			if (hasNorth && hasNorthTop)
			{
				hasVineTop = true;
			}
			else if (hasEast && hasEastTop)
			{
				hasVineTop = true;
			}
			else if (hasSouth && hasSouthTop)
			{
				hasVineTop = true;
			}
			else if (hasWest && hasWestTop)
			{
				hasVineTop = true;
			}

			if (haveFaceBlock || hasVineTop) return;

			level.BreakBlock(null, this);
		}

		public override Item[] GetDrops(Item tool)
		{
			if (tool.Id != 359) return new Item[0];

			return base.GetDrops(tool);
		}

	}
}