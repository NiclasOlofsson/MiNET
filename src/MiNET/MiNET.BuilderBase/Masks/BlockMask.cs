using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Masks
{
	public class BlockMask : Mask
	{
		private readonly Level _level;
		private readonly Block _from;

		public BlockMask(Level level, Block from)
		{
			_level = level;
			_from = @from;
		}

		public override bool Test(BlockCoordinates coordinates)
		{
			var to = _level.GetBlock(coordinates);

			return to.Id == _from.Id && to.Metadata == _from.Metadata;
		}
	}
}