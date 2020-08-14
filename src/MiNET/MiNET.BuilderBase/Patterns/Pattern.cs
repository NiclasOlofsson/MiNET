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
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using log4net;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Utils.Vectors;

[assembly: InternalsVisibleTo("MiNET.BuilderBase.Tests")]

namespace MiNET.BuilderBase.Patterns
{
	public class Pattern : IParameterSerializer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Pattern));

		internal class BlockDataEntry
		{
			public BlockDataEntry()
			{
			}

			public int Id { get; set; }
			public byte Metadata { get; set; }
			public bool HasMetadata { get; set; }
			public int Weight { get; set; } = 100;
			public int Accumulated { get; set; } = 100;
			public List<BlockStateEntry> BlockStates { get; set; } = new List<BlockStateEntry>();
			public bool HasBlockStates { get; set; }
		}

		internal class BlockStateEntry
		{
			public string Name { get; set; }
			public string Value { get; set; }
		}

		internal List<BlockDataEntry> BlockList { get; set; } = new List<BlockDataEntry>();
		private Random _random;
		public string OriginalPattern { get; private set; }

		// Used by command handler
		public Pattern()
		{
			_random = new Random();
		}

		public Pattern(int blockId, int metadata)
		{
			BlockList.Add(new BlockDataEntry()
			{
				Id = (byte) blockId,
				Metadata = (byte) metadata,
				HasMetadata = true
			});
			OriginalPattern = $"{blockId}:{metadata}";
		}

		internal BlockDataEntry GetRandomBlock(Random random, List<BlockDataEntry> blockEntries)
		{
			var blocks = blockEntries.OrderBy(entry => entry.Accumulated).ToList();

			if (blocks.Count == 1) return blocks[0];

			double value = random.Next(blocks.Last().Accumulated + 1);

			Log.Debug($"Random value {value:F2}, length={blocks.Count}, high={blocks.Last().Accumulated}");

			return blocks.First(entry => value <= entry.Accumulated);
		}

		public Block Next(BlockCoordinates position)
		{
			BlockDataEntry blockEntry = GetRandomBlock(_random, BlockList);

			Block block;
			if (blockEntry.HasMetadata)
			{
				Log.Debug($"Using block from metadata");
				block = BlockFactory.GetBlockById(blockEntry.Id, blockEntry.Metadata);
			}
			else
			{
				Log.Debug($"Using block with blockstate");
				block = BlockFactory.GetBlockById(blockEntry.Id);
				if (blockEntry.HasBlockStates)
				{
					Log.Debug($"Has block state, setting block");
					BlockStateContainer currentStates = block.GetState();
					foreach (BlockStateEntry stateEntry in blockEntry.BlockStates)
					{
						Log.Debug($"Checking block state for block {stateEntry.Name}");
						IBlockState state = currentStates.States.FirstOrDefault(s => s.Name == stateEntry.Name);
						Log.Debug($"Found state for block {state?.Name}");
						if(state == null) continue;

						switch (state)
						{
							case BlockStateByte s:
							{
								if (byte.TryParse(stateEntry.Value, out byte v)) s.Value = v;
								break;
							}
							case BlockStateInt s:
							{
								if (int.TryParse(stateEntry.Value, out int v)) s.Value = v;
								break;
							}
							case BlockStateString s:
							{
								s.Value = stateEntry.Value;
								break;
							}
						}
					}
					block.SetState(currentStates);
				}
			}

			block ??= new Air();

			block.Coordinates = position;

			return block;
		}

		public virtual void Deserialize(Player player, string currentPattern)
		{
			// See documentation: https://worldedit.enginehub.org/en/latest/usage/general/patterns/

			if (currentPattern.StartsWith("x")) currentPattern = currentPattern.Remove(0, 1); // remove starting x

			OriginalPattern = currentPattern.Trim();

			var patternsEx = new Regex(@",(?![^\[]*])");
			foreach (string pattern in patternsEx.Split(currentPattern.Trim()))
			{
				Log.Debug($"Matching {pattern}");
				var blockDataEntry = new BlockDataEntry();

				var regex = new Regex(@"(?<pattern>((?<weight>\d+)%)?((?<blockId>\d+)|(?<blockName>(minecraft:)?\w+)){1}(:(?<meta>\d+))?(\[(?<states>[a-zA-Z0-9_=,]*)])?)*");
				var stateEx = new Regex(@"(?<name>\w+)\=(?<value>\w+)");

				Match match = regex.Match(pattern.Trim());
				if (match.Success)
				{
					foreach (Group matchGroup in match.Groups)
					{
						if (matchGroup.Name == "weight" && matchGroup.Success)
						{
							Log.Debug($"Matched weight group {matchGroup.Value}");
							if (int.TryParse(matchGroup.Value.Trim(), out int weight)) blockDataEntry.Weight = weight;
						}
						else if (matchGroup.Name == "blockName" && matchGroup.Success)
						{
							Log.Debug($"Matched blockName group {matchGroup.Value}");
							blockDataEntry.Id = BlockFactory.GetBlockIdByName(matchGroup.Value.Trim());
						}
						if (matchGroup.Name == "blockId" && matchGroup.Success)
						{
							Log.Debug($"Matched blockId group {matchGroup.Value}");
							if (int.TryParse(matchGroup.Value.Trim(), out int id)) blockDataEntry.Id = id;
						}
						else if (matchGroup.Name == "meta" && matchGroup.Success)
						{
							Log.Debug($"Matched meta group {matchGroup.Value}");
							if (byte.TryParse(matchGroup.Value.Trim(), out byte metadata))
							{
								blockDataEntry.Metadata = metadata;
								blockDataEntry.HasBlockStates = true;
							}
						}
						else if (matchGroup.Name == "states" && matchGroup.Success)
						{
							Log.Debug($"Matched states group {matchGroup.Value}");

							// Parse block states
							var stateMatches = stateEx.Matches(matchGroup.Value.Trim());
							{
								foreach (Match stateMatch in stateMatches)
								{
									Log.Debug($"State:{stateMatch.Value}");
									blockDataEntry.BlockStates.Add(new BlockStateEntry()
									{
										Name = stateMatch.Groups.Values.First(g => g.Name == "name").Value,
										Value = stateMatch.Groups.Values.First(g => g.Name == "value").Value
									});
									blockDataEntry.HasBlockStates = true;
								}
							}
						}
					}
				}
				else
				{
					throw new Exception("Deprecated code used to be here.");
				}

				BlockList.Add(blockDataEntry);
			}

			int acc = 0;
			foreach (var entry in BlockList.OrderBy(entry => entry.Weight))
			{
				acc += entry.Weight;
				entry.Accumulated = acc;
			}

			BlockList = BlockList.OrderBy(entry => entry.Accumulated).ToList();
		}
	}
}