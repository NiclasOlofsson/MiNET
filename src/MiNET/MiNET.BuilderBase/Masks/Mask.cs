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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using log4net;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Masks
{
	public class Mask : IParameterSerializer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Mask));

		private class BlockDataEntry
		{
			public int Id { get; set; }
			public byte Metadata { get; set; }
			public bool IgnoreMetadata { get; set; } = true;
		}

		private class MaskEntry
		{
			public bool AboveOnly { get; set; }
			public bool BelowOnly { get; set; }
			public bool Inverted { get; set; }

			public List<BlockDataEntry> BlockList = new List<BlockDataEntry>();
		}

		public Level Level { get; set; }
		public string OriginalMask { get; set; }

		MaskEntry[] _masks = new MaskEntry[0];

		// Used by command handler
		public Mask()
		{
		}

		public Mask(Level level, List<Block> blocks, bool ignoreMetadata)
		{
			Level = level;

			MaskEntry entry = new MaskEntry();

			foreach (var block in blocks)
			{
				entry.BlockList.Add(new BlockDataEntry() {Id = block.Id, Metadata = block.Metadata, IgnoreMetadata = ignoreMetadata});
			}

			_masks = new[] {entry};
		}

		public virtual bool Test(BlockCoordinates coordinates)
		{
			foreach (var mask in _masks)
			{
				if (!Test(coordinates, mask)) return false;
			}

			return true;
		}

		private bool Test(BlockCoordinates coordinates, MaskEntry mask)
		{
			if (Level == null) return true;

			if (mask.AboveOnly)
			{
				coordinates += BlockCoordinates.Down;
			}
			else if (mask.BelowOnly)
			{
				coordinates += BlockCoordinates.Up;
			}

			Block block = Level.GetBlock(coordinates);

			var matches = mask.BlockList.Exists(entry => entry.Id == block.Id && (entry.IgnoreMetadata || block.Metadata == entry.Metadata));

			if (mask.Inverted)
			{
				return !matches;
			}

			return matches;
		}

		public virtual void Deserialize(Player player, string input)
		{
			Level = player.Level;

			// x1:0,air,log:12
			// x<blockId>:<blockData>,<blockId>:<blockData>, .. <blockId>:<blockData>

			// TODO: !, #existing, #region, <, >, 

			if (input.StartsWith("x")) input = input.Remove(0, 1); // remove starting x

			OriginalMask = input;

			string[] inputs = input.Split(' ');

			_masks = new MaskEntry[inputs.Length];
			for (int i = 0; i < inputs.Length; i++)
			{
				MaskEntry entry = new MaskEntry();
				_masks[i] = entry;

				string currentPattern = inputs[i];

				if (currentPattern.StartsWith(">")) // Only place above certain blocks
				{
					entry.AboveOnly = true;
					currentPattern = currentPattern.Remove(0, 1); // remove starting x
				}
				else if (currentPattern.StartsWith("<")) // Only place below certain blocks
				{
					entry.BelowOnly = true;
					currentPattern = currentPattern.Remove(0, 1); // remove starting x
				}
				else if (currentPattern.StartsWith("!")) // Only place if NOT certain blocks
				{
					entry.Inverted = true;
					currentPattern = currentPattern.Remove(0, 1); // remove starting x
				}

				var patterns = currentPattern.Split(',');

				foreach (var pattern in patterns)
				{
					var blockInfos = pattern.Split(':');

					var dataEntry = new BlockDataEntry();

					int id;

					string binfo = blockInfos[0];
					if (!int.TryParse(binfo, out id))
					{
						id = BlockFactory.GetBlockIdByName(binfo);
					}

					dataEntry.Id = id;

					if (blockInfos.Length == 2)
					{
						byte metadata;

						byte.TryParse(blockInfos[1], out metadata);
						dataEntry.Metadata = metadata;
						dataEntry.IgnoreMetadata = false;
					}

					entry.BlockList.Add(dataEntry);
				}
			}
		}
	}
}