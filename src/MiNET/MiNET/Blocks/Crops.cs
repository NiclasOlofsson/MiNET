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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class Crops : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Crops));

		protected byte MaxGrowth { get; set; } = 7;

		protected Crops(byte id) : base(id)
		{
			IsSolid = false;
			IsTransparent = true;
		}

		public override bool Interact(Level level, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			var itemInHand = player.Inventory.GetItemInHand();
			if (Metadata < MaxGrowth && itemInHand is ItemDye && itemInHand.Metadata == 15)
			{
				Metadata += (byte) new Random().Next(2, 6);
				if (Metadata > MaxGrowth) Metadata = MaxGrowth;

				level.SetBlock(this);

				return true;
			}

			return false;
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (!isRandom) return;

			if (Metadata < MaxGrowth && CalculateGrowthChance(level, this))
			{
				Metadata++;
				level.SetBlock(this);
			}
		}

		private static bool CalculateGrowthChance(Level level, Block target)
		{
			//1 / (floor(25 / points) + 1)
			double points = 0;

			// The farmland block the crop is planted in gives 2 points if dry or 4 if hydrated.
			Block under = level.GetBlock(target.Coordinates + BlockCoordinates.Down);
			points += under.Metadata == 0 ? 2 : 4;

			Block west = level.GetBlock(target.Coordinates + BlockCoordinates.West);
			Block east = level.GetBlock(target.Coordinates + BlockCoordinates.East);
			Block south = level.GetBlock(target.Coordinates + BlockCoordinates.North);
			Block north = level.GetBlock(target.Coordinates + BlockCoordinates.South);
			Block southWest = level.GetBlock(target.Coordinates + BlockCoordinates.West + BlockCoordinates.South);
			Block southEast = level.GetBlock(target.Coordinates + BlockCoordinates.East + BlockCoordinates.South);
			Block northWest = level.GetBlock(target.Coordinates + BlockCoordinates.North + BlockCoordinates.North);
			Block northEast = level.GetBlock(target.Coordinates + BlockCoordinates.South + BlockCoordinates.North);

			// For each of the 8 blocks around the block in which the crop is planted, dry farmland gives 0.25 points, and hydrated farmland gives 0.75
			points += west is Farmland ? west.Metadata == 0 ? 0.25 : 0.75 : 0;
			points += east is Farmland ? east.Metadata == 0 ? 0.25 : 0.75 : 0;
			points += south is Farmland ? south.Metadata == 0 ? 0.25 : 0.75 : 0;
			points += north is Farmland ? north.Metadata == 0 ? 0.25 : 0.75 : 0;
			points += northWest is Farmland ? northWest.Metadata == 0 ? 0.25 : 0.75 : 0;
			points += northEast is Farmland ? northEast.Metadata == 0 ? 0.25 : 0.75 : 0;
			points += southWest is Farmland ? southWest.Metadata == 0 ? 0.25 : 0.75 : 0;
			points += southEast is Farmland ? southEast.Metadata == 0 ? 0.25 : 0.75 : 0;

			// If any plants of the same type are growing in the eight surrounding blocks, the point total is cut in half unless the crops are arranged in rows.
			// TODO: Check rows .. muuhhhuu
			bool cutHalf = false;
			cutHalf |= west.GetType() == target.GetType();
			cutHalf |= east.GetType() == target.GetType();
			cutHalf |= south.GetType() == target.GetType();
			cutHalf |= north.GetType() == target.GetType();
			cutHalf |= northEast.GetType() == target.GetType();
			cutHalf |= northWest.GetType() == target.GetType();
			cutHalf |= southEast.GetType() == target.GetType();
			cutHalf |= southWest.GetType() == target.GetType();
			points /= cutHalf ? 2 : 1;

			//1 / (floor(25 / points) + 1)

			double chance = 1/(Math.Floor(25/points) + 1);

			var calculateGrowthChance = level.Random.NextDouble() <= chance;
			//Log.Debug($"Calculated growth chance. Will grow={calculateGrowthChance} on a chance score of {chance}");
			return calculateGrowthChance;
		}

		protected override bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			if (base.CanPlace(world, blockCoordinates, face))
			{
				Block under = world.GetBlock(Coordinates + BlockCoordinates.Down);
				return under is Farmland;
			}

			return false;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			if (Coordinates + BlockCoordinates.Down == blockCoordinates)
			{
				level.BreakBlock(this);
			}
		}
	}
}