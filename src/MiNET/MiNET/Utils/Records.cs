using System.Collections.Generic;
using MiNET.Blocks;

namespace MiNET.Utils
{
	public class Records : List<BlockCoordinates>
	{
		public Records()
		{
		}

		public Records(IEnumerable<BlockCoordinates> coordinates) : base(coordinates)
		{
		}
	}

	public class BlockRecords : List<Block>
	{
		public BlockRecords()
		{
		}

		public BlockRecords(IEnumerable<Block> blocks)
			: base(blocks)
		{
		}
	}
}