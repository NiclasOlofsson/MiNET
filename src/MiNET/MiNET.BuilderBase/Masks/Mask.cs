using System.Collections.Generic;
using log4net;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Masks
{
	public class Mask : IParameterSerializer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Mask));

		private class BlockDataEntry
		{
			public byte Id { get; set; }
			public byte Metadata { get; set; }
			public bool IgnoreMetadata { get; set; } = true;
		}

		private List<BlockDataEntry> _blockList = new List<BlockDataEntry>();

		public Level Level { get; set; }

		// Used by command handler
		public Mask()
		{
		}

		public Mask(Level level, List<Block> blocks, bool ignoreMetadata)
		{
			Level = level;

			foreach (var block in blocks)
			{
				_blockList.Add(new BlockDataEntry() {Id = block.Id, Metadata = block.Metadata, IgnoreMetadata = ignoreMetadata});
			}
		}

		public virtual bool Test(BlockCoordinates coordinates)
		{
			if (Level == null) return true;

			Block block = Level.GetBlock(coordinates);
			return _blockList.Exists(entry => entry.Id == block.Id && (entry.IgnoreMetadata || block.Metadata == entry.Metadata));
		}

		public virtual void Deserialize(Player player, string input)
		{
			Level = player.Level;

			// x1:0,air,log:12
			// x<blockId>:<blockData>,<blockId>:<blockData>, .. <blockId>:<blockData>

			// TODO: !, #existing, #region, <, >, 

			if (input.StartsWith("x")) input = input.Remove(0, 1); // remove starting x

			var patterns = input.Split(',');

			foreach (var pattern in patterns)
			{
				var blockInfos = pattern.Split(':');

				var dataEntry = new BlockDataEntry();

				byte id;

				string binfo = blockInfos[0];
				if (!byte.TryParse(binfo, out id))
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

				_blockList.Add(dataEntry);
			}
		}
	}
}