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
using System.Linq;
using log4net;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Utils;

namespace MiNET.BuilderBase.Patterns
{
	public class Pattern : IParameterSerializer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Pattern));

		private class BlockDataEntry
		{
			public int Id { get; set; }
			public byte Metadata { get; set; }
			public int Weight { get; set; } = 100;
			public int Accumulated { get; set; } = 100;
		}

		private List<BlockDataEntry> _blockList = new List<BlockDataEntry>();
		private Random _random;
		public string OriginalPattern { get; private set; }

		// Used by command handler
		public Pattern()
		{
			_random = new Random();
		}

		public Pattern(int blockId, int metadata)
		{
			_blockList.Add(new BlockDataEntry()
			{
				Id = (byte) blockId,
				Metadata = (byte) metadata
			});
			OriginalPattern = $"{blockId}:{metadata}";
		}

		BlockDataEntry GetRandomBlock(Random random, List<BlockDataEntry> blocksa)
		{
			var blocks = blocksa.OrderBy(entry => entry.Accumulated).ToList();

			if (blocks.Count == 1) return blocks[0];

			double value = random.Next(blocks.Last().Accumulated + 1);

			Log.Debug($"Random value {value:F2}, lenght={blocks.Count}, high={blocks.Last().Accumulated}");

			return blocks.First(entry => value <= entry.Accumulated);
		}

		public Block Next(BlockCoordinates position)
		{
			var blockEntry = GetRandomBlock(_random, _blockList);

			Block block = BlockFactory.GetBlockById(blockEntry.Id);
			block.Metadata = blockEntry.Metadata;
			block.Coordinates = position;

			return block;
		}

		public virtual void Deserialize(Player player, string currentPattern)
		{
			// x20%1:0
			// x<weight>%<blockId>:<blockData>,<weight>%<blockId>:<blockData> .. <weight>%<blockId>:<blockData>

			if (currentPattern.StartsWith("x")) currentPattern = currentPattern.Remove(0, 1); // remove starting x

			OriginalPattern = currentPattern;

			var patterns = currentPattern.Split(',');

			foreach (var pattern in patterns)
			{
				var blockInfos = pattern.Split(':');

				int id;
				byte metadata = 0;
				int weight = 100;

				var weightedIn = blockInfos[0].Split('%');
				if (weightedIn.Length == 2)
				{
					if (!int.TryParse(weightedIn[0], out weight))
					{
						weight = 100;
					}
				}

				string binfo = weightedIn[weightedIn.Length - 1];
				if (!int.TryParse(binfo, out id))
				{
					id = BlockFactory.GetBlockIdByName(binfo);
				}

				if (blockInfos.Length == 2)
				{
					byte.TryParse(blockInfos[1], out metadata);
				}

				Log.Debug($"Parsed x{weight}%{id}:{metadata}");
				_blockList.Add(new BlockDataEntry()
				{
					Id = id,
					Metadata = metadata,
					Weight = weight
				});
			}

			int acc = 0;
			foreach (var entry in _blockList.OrderBy(entry => entry.Weight))
			{
				acc += entry.Weight;
				entry.Accumulated = acc;
			}

			_blockList = _blockList.OrderBy(entry => entry.Accumulated).ToList();
		}
	}
}