using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Masks
{
	public class NotAirBlocksMask : Mask
	{
		private readonly Level _level;

		public NotAirBlocksMask(Level level)
		{
			_level = level;
		}

		public override bool Test(BlockCoordinates coordinates)
		{
			return !_level.IsAir(coordinates);
		}
	}
}