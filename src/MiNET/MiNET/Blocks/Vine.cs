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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Vine : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Vine));

		public Vine() : base(106)
		{
			IsSolid = false;
			IsTransparent = true;
			BlastResistance = 1;
			Hardness = 0.2f;
			IsFlammable = true;
			IsReplaceable = true;
		}

		private const byte North = 0x01;
		private const byte East = 0x02;
		private const byte South = 0x04;
		private const byte West = 0x08;

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			if (!base.CanPlace(world, player, blockCoordinates, targetCoordinates, face)) return false;

			var block = world.GetBlock(Coordinates);
			return !(block is Vine);
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (world.GetBlock(Coordinates) is Vine block)
			{
				VineDirectionBits = block.VineDirectionBits;
			}

			int direction;
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

			if ((VineDirectionBits & direction) == direction)
			{
				return true; // Already have this face covered
			}

			VineDirectionBits |= direction;

			return false;
		}

		//public override void BreakBlock(Level level, BlockFace face, bool silent = false)
		//{
		//	Log.Debug($"Breaking vine face {face}, have direction: {VineDirectionBits}");
		//	int newValue = GetDirectionBits(level, this);
		//	switch (face)
		//	{
		//		case BlockFace.North:
		//			newValue &= ~North;
		//			break;
		//		case BlockFace.East:
		//			newValue &= ~East;
		//			break;
		//		case BlockFace.South:
		//			newValue &= ~South;
		//			break;
		//		case BlockFace.West:
		//			newValue &= ~West;
		//			break;
		//	}
		//	Log.Debug($"Breaking vine, new value: {newValue}, old {VineDirectionBits}");
		//	if (newValue != VineDirectionBits)
		//	{
		//		VineDirectionBits = newValue;
		//		if (VineDirectionBits != 0)
		//		{
		//			level.SetBlock(this);
		//		}
		//		else
		//		{
		//			base.BreakBlock(level, face, silent);
		//		}
		//	}
		//}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			int newValue = GetDirectionBits(level, this);

			if (newValue != VineDirectionBits)
			{
				VineDirectionBits = newValue;

				if (VineDirectionBits != 0)
				{
					level.SetBlock(this);
				}
				else
				{
					level.BreakBlock(null, this);
				}
			}
		}

		private static int GetDirectionBits(Level level, Vine vine)
		{
			bool hasNorth = (vine.VineDirectionBits & North) == North;
			bool hasEast = (vine.VineDirectionBits & East) == East;
			bool hasSouth = (vine.VineDirectionBits & South) == South;
			bool hasWest = (vine.VineDirectionBits & West) == West;

			var onTop = level.GetBlock(vine.Coordinates + Level.Up) as Vine;
			bool hasNorthTop = onTop != null && (onTop.VineDirectionBits & North) == North;
			bool hasEastTop = onTop != null && (onTop.VineDirectionBits & East) == East;
			bool hasSouthTop = onTop != null && (onTop.VineDirectionBits & South) == South;
			bool hasWestTop = onTop != null && (onTop.VineDirectionBits & West) == West;

			bool hasFaceBlockNorth = hasNorth && level.GetBlock(vine.Coordinates + Level.South).IsSolid;
			bool hasFaceBlockEast = hasEast && level.GetBlock(vine.Coordinates + Level.West).IsSolid;
			bool hasFaceBlockSouth = hasSouth && level.GetBlock(vine.Coordinates + Level.North).IsSolid;
			bool hasFaceBlockWest = hasWest && level.GetBlock(vine.Coordinates + Level.East).IsSolid;

			int newVineDirectionBits = 0;
			if (hasNorth && (hasNorthTop || hasFaceBlockNorth)) newVineDirectionBits |= North;
			if (hasEast && (hasEastTop || hasFaceBlockEast)) newVineDirectionBits |= East;
			if (hasSouth && (hasSouthTop || hasFaceBlockSouth)) newVineDirectionBits |= South;
			if (hasWest && (hasWestTop || hasFaceBlockWest)) newVineDirectionBits |= West;

			return newVineDirectionBits;
		}

		public override Item[] GetDrops(Item tool)
		{
			if (tool.Id != 359) return new Item[0];

			return base.GetDrops(tool);
		}
	}
}