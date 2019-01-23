﻿#region LICENSE

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

using System;
using System.Collections.Generic;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Leaves2 : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Leaves2));

		public Leaves2() : base(161)
		{
			IsTransparent = true;
			BlastResistance = 1;
			Hardness = 0.2f;
			IsFlammable = true;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			// No decay
			if ((Metadata & 0x04) == 0x04) return;

			if ((Metadata & 0x08) == 0x08) return;

			Metadata = (byte) (Metadata | 0x08);
			level.SetBlock(this, false, false, false);
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if ((Metadata & 0x04) == 0x04) return;

			if ((Metadata & 0x08) != 0x08) return;

			//Log.Debug("Checking decay");

			if (FindLog(level, Coordinates, new List<BlockCoordinates>(), 0))
			{
				Metadata = (byte) (Metadata & 0x07);
				level.SetBlock(this, false, false, false);
				return;
			}

			Log.Debug("Decay leaf");

			var drops = GetDrops(null);
			BreakBlock(level, drops.Length == 0);
			foreach (var drop in drops)
			{
				level.DropItem(Coordinates, drop);
			}
		}

		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			if ((Metadata & 0x03) == 1) // Oak and dark oak drops apple
			{
				if (rnd.Next(200) == 0)
				{
					return new Item[] {ItemFactory.GetItem(260, 0, 1)};
				}
			}
			if (rnd.Next(20) == 0)
			{
				// Sapling
				return new Item[] {ItemFactory.GetItem(39, (short) (Metadata + 5), 1)};
			}
			return new Item[0];
		}

		private bool FindLog(Level level, BlockCoordinates coord, List<BlockCoordinates> visited, int distance)
		{
			if (visited.Contains(coord)) return false;

			var block = level.GetBlock(coord);
			if (block is Log2) return true;

			visited.Add(coord);

			if (distance >= 4) return false;

			if (!(block is Leaves2)) return false;
			if ((block.Metadata & 0x07) != (Metadata & 0x07)) return false;

			// check down
			if (FindLog(level, coord.BlockDown(), visited, distance + 1)) return true;
			// check west
			if (FindLog(level, coord.BlockWest(), visited, distance + 1)) return true;
			// check east
			if (FindLog(level, coord.BlockEast(), visited, distance + 1)) return true;
			// check south
			if (FindLog(level, coord.BlockSouth(), visited, distance + 1)) return true;
			// check north
			if (FindLog(level, coord.BlockNorth(), visited, distance + 1)) return true;
			// check up
			if (FindLog(level, coord.BlockUp(), visited, distance + 1)) return true;

			return false;
		}
	}
}