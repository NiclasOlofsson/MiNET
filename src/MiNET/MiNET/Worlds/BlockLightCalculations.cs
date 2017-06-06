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

using System.Collections.Generic;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class BlockLightCalculations
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BlockLightCalculations));

		public static void Calculate(Level level, Block block)
		{
			Queue<Block> lightBfsQueue = new Queue<Block>();

			lightBfsQueue.Enqueue(block);
			while (lightBfsQueue.Count > 0)
			{
				ProcessNode(level, lightBfsQueue.Dequeue(), lightBfsQueue);
			}
		}

		private static void ProcessNode(Level level, Block block, Queue<Block> lightBfsQueue)
		{
			//Log.Debug($"Setting light on block {block.Id} with LightLevel={block.LightLevel} and BlockLight={block.Blocklight}");
			int lightLevel = block.BlockLight;

			SetLightLevel(level, lightBfsQueue, level.GetBlock(block.Coordinates + BlockCoordinates.Up), lightLevel);
			SetLightLevel(level, lightBfsQueue, level.GetBlock(block.Coordinates + BlockCoordinates.Down), lightLevel);
			SetLightLevel(level, lightBfsQueue, level.GetBlock(block.Coordinates + BlockCoordinates.West), lightLevel);
			SetLightLevel(level, lightBfsQueue, level.GetBlock(block.Coordinates + BlockCoordinates.East), lightLevel);
			SetLightLevel(level, lightBfsQueue, level.GetBlock(block.Coordinates + BlockCoordinates.South), lightLevel);
			SetLightLevel(level, lightBfsQueue, level.GetBlock(block.Coordinates + BlockCoordinates.North), lightLevel);
		}

		private static void SetLightLevel(Level level, Queue<Block> lightBfsQueue, Block b1, int lightLevel)
		{
			if (b1.LightLevel > 0)
			{
				b1.BlockLight = (byte) b1.LightLevel;
				level.SetBlockLight(b1);
			}

			if ((!b1.IsSolid || b1.IsTransparent) && b1.BlockLight + 2 <= lightLevel)
			{
				b1.BlockLight = (byte) (lightLevel - 1);
				level.SetBlockLight(b1);
				lightBfsQueue.Enqueue(b1);
			}
		}
	}
}