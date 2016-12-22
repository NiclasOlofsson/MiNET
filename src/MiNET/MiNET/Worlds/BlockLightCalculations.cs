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