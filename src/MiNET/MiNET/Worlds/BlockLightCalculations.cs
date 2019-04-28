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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class BlockLightCalculations
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BlockLightCalculations));

		public static void Calculate(Level level, BlockCoordinates blockCoordinates, int from = -1)
		{
			//Interlocked.Add(ref touches, 1);

			ConcurrentDictionary<BlockCoordinates, byte> oldLights = new ConcurrentDictionary<BlockCoordinates, byte>();
			Queue<BlockCoordinates> lightBfsQueue = new Queue<BlockCoordinates>();
			Queue<BlockCoordinates> potentialSource = new Queue<BlockCoordinates>();

			lightBfsQueue.Enqueue(blockCoordinates);
			if (from >= 0)
				oldLights.TryAdd(blockCoordinates, (byte)from);
			while (lightBfsQueue.Count > 0)
			{
				ProcessNode(level, lightBfsQueue.Dequeue(), lightBfsQueue, potentialSource, oldLights);
			}
			while (potentialSource.Count > 0) //relighting
			{
				ProcessNode(level, potentialSource.Dequeue(), potentialSource, new Queue<BlockCoordinates>(), oldLights);
			}
		}

		private static void ProcessNode(Level level, BlockCoordinates coord, Queue<BlockCoordinates> lightBfsQueue, Queue<BlockCoordinates> potentialSource, ConcurrentDictionary<BlockCoordinates, byte> oldLights)
		{
			//Log.Debug($"Setting light on block {block.Id} with LightLevel={block.LightLevel} and BlockLight={block.Blocklight}");
			ChunkColumn chunk = GetChunk(level, coord);
			if (chunk == null) return;

			byte lightLevel = chunk.GetBlocklight(coord.X & 0x0f, coord.Y & 0xff, coord.Z & 0x0f);
			int bid = chunk.GetBlock(coord.X & 0x0f, coord.Y & 0xff, coord.Z & 0x0f);
			//if (bid != 0)
			//	lightLevel = (byte)Math.Max(lightLevel, BlockFactory.GetBlockById(bid).LightLevel);
			byte oldLightLevel = lightLevel;
			if (oldLights.ContainsKey(coord))
				oldLightLevel = oldLights[coord];

			Test(level, coord, coord.BlockUp(), lightBfsQueue, potentialSource, oldLights, chunk, lightLevel, oldLightLevel);
			Test(level, coord, coord.BlockDown(), lightBfsQueue, potentialSource, oldLights, chunk, lightLevel, oldLightLevel);
			Test(level, coord, coord.BlockWest(), lightBfsQueue, potentialSource, oldLights, chunk, lightLevel, oldLightLevel);
			Test(level, coord, coord.BlockEast(), lightBfsQueue, potentialSource, oldLights, chunk, lightLevel, oldLightLevel);
			Test(level, coord, coord.BlockSouth(), lightBfsQueue, potentialSource, oldLights, chunk, lightLevel, oldLightLevel);
			Test(level, coord, coord.BlockNorth(), lightBfsQueue, potentialSource, oldLights, chunk, lightLevel, oldLightLevel);
			oldLights.TryRemove(coord, out oldLightLevel);

			//SetLightLevel(level, lightBfsQueue, level.GetBlock(coord + BlockCoordinates.Down, chunk), lightLevel);
			//SetLightLevel(level, lightBfsQueue, level.GetBlock(coord + BlockCoordinates.West, chunk), lightLevel);
			//SetLightLevel(level, lightBfsQueue, level.GetBlock(coord + BlockCoordinates.East, chunk), lightLevel);
			//SetLightLevel(level, lightBfsQueue, level.GetBlock(coord + BlockCoordinates.South, chunk), lightLevel);
			//SetLightLevel(level, lightBfsQueue, level.GetBlock(coord + BlockCoordinates.North, chunk), lightLevel);
		}

		private static ChunkColumn GetChunk(Level level, BlockCoordinates blockCoordinates)
		{
			AnvilWorldProvider provider = level.WorldProvider as AnvilWorldProvider;
			if (provider != null)
			{
				ChunkColumn chunk;
				provider._chunkCache.TryGetValue((ChunkCoordinates) blockCoordinates, out chunk);

				return chunk;
			}

			return null;
		}

		public static long touches = 0;

		private static void Test(Level level, BlockCoordinates coord, BlockCoordinates newCoord, Queue<BlockCoordinates> lightBfsQueue, Queue<BlockCoordinates> potentialSource, ConcurrentDictionary<BlockCoordinates, byte> oldLights, ChunkColumn chunk, int lightLevel, byte oldLightLevel)
		{
			//Interlocked.Add(ref touches, 1);

			var newChunkCoord = (ChunkCoordinates) newCoord;
			if (chunk.x != newChunkCoord.X || chunk.z != newChunkCoord.Z)
			{
				chunk = GetChunk(level, newCoord);
			}

			if (chunk == null) return;

			oldLights.TryAdd(newCoord, chunk.GetBlocklight(newCoord.X & 0x0f, newCoord.Y & 0xff, newCoord.Z & 0x0f));
			
			if (chunk.GetBlock(newCoord.X & 0x0f, newCoord.Y & 0xff, newCoord.Z & 0x0f) == 0)
			{
				SetLightLevel(chunk, lightBfsQueue, potentialSource, newCoord, lightLevel, oldLightLevel);
			}
			else
			{
				//if (BlockFactory.LuminousBlocks.ContainsKey(chunk.GetBlocklight(newCoord.X & 0x0f, newCoord.Y & 0xff, newCoord.Z & 0x0f)))
				//{
				//}
				//else
				{
					SetLightLevel(chunk, lightBfsQueue, potentialSource, level.GetBlock(newCoord, chunk), lightLevel, oldLightLevel);
				}
			}
		}

		private static void SetLightLevel(ChunkColumn chunk, Queue<BlockCoordinates> lightBfsQueue, Queue<BlockCoordinates> potentialSource, Block b1, int lightLevel, byte oldLightLevel)
		{
			if (b1.LightLevel > 0)
			{
				if (b1.LightLevel >= lightLevel)
				{
					if (lightLevel == 0 && !potentialSource.Contains(b1.Coordinates))
						potentialSource.Enqueue(b1.Coordinates);
					return;
				}

				b1.BlockLight = (byte) Math.Max(b1.LightLevel, lightLevel - 1);
				chunk.SetBlocklight(b1.Coordinates.X & 0x0f, b1.Coordinates.Y & 0xff, b1.Coordinates.Z & 0x0f, (byte) b1.BlockLight);
			}

			if ((!b1.IsSolid || b1.IsTransparent))
			{
				if (lightLevel < oldLightLevel && b1.BlockLight >= oldLightLevel)
				{
					potentialSource.Enqueue(b1.Coordinates);
					return;
				}
				if (b1.BlockLight + 2 <= lightLevel)
				{
					b1.BlockLight = (byte) (lightLevel - 1);
					chunk.SetBlocklight(b1.Coordinates.X & 0x0f, b1.Coordinates.Y & 0xff, b1.Coordinates.Z & 0x0f, (byte) b1.BlockLight);
					lightBfsQueue.Enqueue(b1.Coordinates);
				}
				else if(lightLevel == 0 && b1.BlockLight > 0)
				{
					b1.BlockLight = 0;
					chunk.SetBlocklight(b1.Coordinates.X & 0x0f, b1.Coordinates.Y & 0xff, b1.Coordinates.Z & 0x0f, 0); //reset
					lightBfsQueue.Enqueue(b1.Coordinates);
				}
			}
		}

		private static void SetLightLevel(ChunkColumn chunk, Queue<BlockCoordinates> lightBfsQueue, Queue<BlockCoordinates> potentialSource, BlockCoordinates coord, int lightLevel, byte oldLightLevel)
		{
			var _lightLevel = chunk.GetBlocklight(coord.X & 0x0f, coord.Y & 0xff, coord.Z & 0x0f);
			if (lightLevel < oldLightLevel && _lightLevel >= oldLightLevel)
			{
				potentialSource.Enqueue(coord);
				return;
			}
			if (_lightLevel + 2 <= lightLevel)
			{
				chunk.SetBlocklight(coord.X & 0x0f, coord.Y & 0xff, coord.Z & 0x0f, (byte) (lightLevel - 1));
				lightBfsQueue.Enqueue(coord);
			}
			else if(lightLevel == 0 && _lightLevel > 0)
			{
				chunk.SetBlocklight(coord.X & 0x0f, coord.Y & 0xff, coord.Z & 0x0f, 0); //reset
				lightBfsQueue.Enqueue(coord);
			}
		}
	}
}