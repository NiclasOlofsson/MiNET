using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Commands
{
	public class AllBlocksMask
	{
		private readonly Level _level;

		public AllBlocksMask(Level level)
		{
			_level = level;
		}

		public bool Test(BlockCoordinates coordinates)
		{
			return !_level.IsAir(coordinates);
		}
	}
}