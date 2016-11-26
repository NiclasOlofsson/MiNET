using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.BuilderBase.Commands
{
	public class Pattern
	{
		private readonly int _blockId;
		private readonly int _metadata;

		public Pattern(int blockId, int metadata)
		{
			_blockId = blockId;
			_metadata = metadata;
		}

		public Block Next(BlockCoordinates position)
		{
			Block block = BlockFactory.GetBlockById((byte) _blockId);
			block.Metadata = (byte) _metadata;
			block.Coordinates = position;

			return block;
		}
	}
}