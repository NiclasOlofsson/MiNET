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

using System;
using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class Crops : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Crops));

		[StateRange(0, 7)] public virtual int Growth { get; set; } = 0;

		protected byte MaxGrowth { get; set; } = 7;

		protected Crops(byte id) : base(id)
		{
			IsSolid = false;
			IsTransparent = true;
		}

		public override bool Interact(Level level, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			var itemInHand = player.Inventory.GetItemInHand();
			if (Growth < MaxGrowth && itemInHand is ItemDye && itemInHand.Metadata == 15)
			{
				Growth += (byte) new Random().Next(2, 6);
				if (Growth > MaxGrowth) Growth = MaxGrowth;

				level.SetBlock(this);

				return true;
			}

			return false;
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (!isRandom) return;

			if (Growth < MaxGrowth && CalculateGrowthChance(level, this))
			{
				Growth++;
				level.SetBlock(this);
			}
		}

		private static bool CalculateGrowthChance(Level level, Block target)
		{
			//1 / (floor(25 / points) + 1)
			double points = 0;

			// The farmland block the crop is planted in gives 2 points if dry or 4 if hydrated.
			var under = level.GetBlock(target.Coordinates.BlockDown()) as Farmland;
			if (under == null) return false;

			points += under.MoisturizedAmount == 0 ? 2 : 4;

			{
				var west = level.GetBlock(under.Coordinates.BlockWest()) as Farmland;
				var east = level.GetBlock(under.Coordinates.BlockEast()) as Farmland;
				var south = level.GetBlock(under.Coordinates.BlockNorth()) as Farmland;
				var north = level.GetBlock(under.Coordinates.BlockSouth()) as Farmland;
				var southWest = level.GetBlock(under.Coordinates.BlockSouthWest()) as Farmland;
				var southEast = level.GetBlock(under.Coordinates.BlockSouthEast()) as Farmland;
				var northWest = level.GetBlock(under.Coordinates.BlockNorthWest()) as Farmland;
				var northEast = level.GetBlock(under.Coordinates.BlockNorthEast()) as Farmland;

				// For each of the 8 blocks around the block in which the crop is planted, dry farmland gives 0.25 points, and hydrated farmland gives 0.75
				points += west != null ? west.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
				points += east != null ? east.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
				points += south != null ? south.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
				points += north != null ? north.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
				points += northWest != null ? northWest.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
				points += northEast != null ? northEast.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
				points += southWest != null ? southWest.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
				points += southEast != null ? southEast.MoisturizedAmount == 0 ? 0.25 : 0.75 : 0;
			}

			{
				// If any plants of the same type are growing in the eight surrounding blocks, the point total is cut in half unless the crops are arranged in rows.
				// TODO: Check rows .. muuhhhuu

				var west = level.GetBlock(target.Coordinates.BlockWest()) as Crops;
				var east = level.GetBlock(target.Coordinates.BlockEast()) as Crops;
				var south = level.GetBlock(target.Coordinates.BlockNorth()) as Crops;
				var north = level.GetBlock(target.Coordinates.BlockSouth()) as Crops;
				var southWest = level.GetBlock(target.Coordinates.BlockSouthWest()) as Crops;
				var southEast = level.GetBlock(target.Coordinates.BlockSouthEast()) as Crops;
				var northWest = level.GetBlock(target.Coordinates.BlockNorthWest()) as Crops;
				var northEast = level.GetBlock(target.Coordinates.BlockNorthEast()) as Crops;

				bool cutHalf = false;
				cutHalf |= west != null && west.GetType() == target.GetType();
				cutHalf |= east != null && east.GetType() == target.GetType();
				cutHalf |= south != null && south.GetType() == target.GetType();
				cutHalf |= north != null && north.GetType() == target.GetType();
				cutHalf |= northEast != null && northEast.GetType() == target.GetType();
				cutHalf |= northWest != null && northWest.GetType() == target.GetType();
				cutHalf |= southEast != null && southEast.GetType() == target.GetType();
				cutHalf |= southWest != null && southWest.GetType() == target.GetType();
				points /= cutHalf ? 2 : 1;
			}

			//1 / (floor(25 / points) + 1)

			double chance = 1 / (Math.Floor(25 / points) + 1);

			var calculateGrowthChance = level.Random.NextDouble() <= chance;
			//Log.Debug($"Calculated growth chance. Will grow={calculateGrowthChance} on a chance score of {chance}");
			return calculateGrowthChance;
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			if (base.CanPlace(world, player, blockCoordinates, targetCoordinates, face))
			{
				Block under = world.GetBlock(Coordinates.BlockDown());
				return under is Farmland;
			}

			return false;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			if (Coordinates.BlockDown() == blockCoordinates)
			{
				level.BreakBlock(null, this);
			}
		}
	}
}