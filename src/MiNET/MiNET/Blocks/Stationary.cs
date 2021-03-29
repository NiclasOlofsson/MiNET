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

using log4net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Stationary : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Stationary));

		[StateRange(0, 15)] public virtual int LiquidDepth { get; set; } = 0;

		internal Stationary(byte id) : base(id)
		{
			IsSolid = false;
			IsBuildable = false;
			IsReplaceable = true;
			IsTransparent = true;
		}

		public override void DoPhysics(Level level)
		{
			CheckForHarden(level, Coordinates);

			if (level.GetBlock(Coordinates).Id == Id) // Isn't this always true?
			{
				SetToFlowing(level);
			}
		}

		private void SetToFlowing(Level world)
		{
			var flowingBlock = (Flowing) BlockFactory.GetBlockById((byte) (Id - 1));
			flowingBlock.LiquidDepth = LiquidDepth;
			flowingBlock.Coordinates = Coordinates;
			world.SetBlock(flowingBlock, applyPhysics: false);
			world.ScheduleBlockTick(flowingBlock, 5);
		}

		private void CheckForHarden(Level world, BlockCoordinates coord)
		{
			var block = world.GetBlock(coord) as Stationary; // this

			bool harden = false;
			if ( /*block is FlowingLava || */block is Lava) // this is lava, can not be flowing lava
			{
				if (IsWater(world, coord + BlockCoordinates.Backwards))
				{
					harden = true;
				}

				if (harden || IsWater(world, coord + BlockCoordinates.Forwards))
				{
					harden = true;
				}

				if (harden || IsWater(world, coord + BlockCoordinates.Left))
				{
					harden = true;
				}

				if (harden || IsWater(world, coord + BlockCoordinates.Right))
				{
					harden = true;
				}

				if (harden || IsWater(world, coord + BlockCoordinates.Up))
				{
					harden = true;
				}

				if (harden)
				{
					int liquidDepth = block.LiquidDepth;

					if (liquidDepth == 0)
					{
						world.SetBlock(new Obsidian {Coordinates = new BlockCoordinates(coord)}, true, false);
					}
					else if (liquidDepth <= 4)
					{
						world.SetBlock(new Cobblestone {Coordinates = new BlockCoordinates(coord)}, true, false);
					}
				}
			}
		}

		private bool IsWater(Level world, BlockCoordinates coord)
		{
			Block block = world.GetBlock(coord);
			return block is FlowingWater || block is Water;
		}
	}
}