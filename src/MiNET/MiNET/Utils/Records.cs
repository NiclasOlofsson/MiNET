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

	public abstract class PlayerRecords : List<Player>
	{
		public PlayerRecords()
		{
		}

		public PlayerRecords(IEnumerable<Player> players)
			: base(players)
		{
		}
	}

	public class PlayerAddRecords : PlayerRecords
	{
	}

	public class PlayerRemoveRecords : PlayerRecords
	{
	}
}