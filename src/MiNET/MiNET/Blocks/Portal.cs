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

using System.Collections.Generic;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class PortalInfo
	{
		public BlockCoordinates Coordinates { get; set; }
		public bool HasPlatform { get; set; }
		public BoundingBox Size { get; set; }
	}

	public partial class Portal : Block
	{
		public Portal() : base(90)
		{
			IsTransparent = true;
			IsSolid = false;
			LightLevel = 11;
			Hardness = 60000;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			bool shouldKeep = true;
			shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockUp()));
			shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockDown()));

			//if (Metadata < 2)
			if (PortalAxis == "x")
			{
				shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockWest()));
				shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockEast()));
			}
			else
			{
				shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockSouth()));
				shouldKeep &= IsValid(level.GetBlock(Coordinates.BlockNorth()));
			}

			if (!shouldKeep)
			{
				Fill(level, Coordinates);
			}
		}

		private bool IsValid(Block block)
		{
			return block is Obsidian || block is Portal;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0];
		}


		public void Fill(Level level, BlockCoordinates origin)
		{
			Queue<BlockCoordinates> visits = new Queue<BlockCoordinates>();

			visits.Enqueue(origin); // Kick it off with some good stuff

			while (visits.Count > 0)
			{
				var coordinates = visits.Dequeue();

				if (!(level.GetBlock(coordinates) is Portal)) continue;

				level.SetAir(coordinates);

				//if (Metadata == 0)
				if (PortalAxis == "x")
				{
					visits.Enqueue(coordinates + Level.East);
					visits.Enqueue(coordinates + Level.West);
				}
				else
				{
					visits.Enqueue(coordinates + Level.North);
					visits.Enqueue(coordinates + Level.South);
				}

				visits.Enqueue(coordinates + Level.Down);
				visits.Enqueue(coordinates + Level.Up);
			}
		}
	}
}