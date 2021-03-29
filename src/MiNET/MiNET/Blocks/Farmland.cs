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
using System.Collections.Generic;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Farmland : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Farmland));

		public Farmland() : base(60)
		{
			IsTransparent = true; // Partial - blocks light.
			IsBlockingSkylight = false; // Partial - blocks light.
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {new ItemBlock(new Dirt(), 0) {Count = 1}}; // Drop dirt block
		}

		public override void OnTick(Level level, bool isRandom)
		{
			var data = MoisturizedAmount;
			if (FindWater(level, Coordinates, new List<BlockCoordinates>(), 0))
			{
				MoisturizedAmount = 7;
			}
			else
			{
				MoisturizedAmount = Math.Max(0, MoisturizedAmount - 1);
			}
			if (data != MoisturizedAmount)
			{
				level.SetBlock(this);
			}
		}

		public bool FindWater(Level level, BlockCoordinates coord, List<BlockCoordinates> visited, int distance)
		{
			if (visited.Contains(coord)) return false;

			var block = level.GetBlock(coord);
			if (block is Water || block is FlowingWater) return true;

			visited.Add(coord);

			if (distance >= 4) return false;

			// check down
			//if (FindWater(level, coord + BlockCoordinates.Down, visited, distance + 1)) return true;
			// check west
			if (FindWater(level, coord.BlockWest(), visited, distance + 1)) return true;
			// check east
			if (FindWater(level, coord.BlockEast(), visited, distance + 1)) return true;
			// check south
			if (FindWater(level, coord.BlockSouth(), visited, distance + 1)) return true;
			// check north
			if (FindWater(level, coord.BlockNorth(), visited, distance + 1)) return true;
			// check up
			//if (FindWater(level, coord + BlockCoordinates.Up, visited, distance + 1)) return true;

			return false;
		}
	}
}