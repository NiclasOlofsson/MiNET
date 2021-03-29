using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Masks
{
	public class NotAirBlocksMask : Mask
	{
		private readonly Level _level;

		public NotAirBlocksMask(Level level)
		{
			_level = level;
			OriginalMask = "Not air";
		}

		public override bool Test(BlockCoordinates coordinates)
		{
			return !_level.IsAir(coordinates);
		}
	}

	public class AirBlocksMask : Mask
	{
		private readonly Level _level;

		public AirBlocksMask(Level level)
		{
			_level = level;
			OriginalMask = "Air";
		}

		public override bool Test(BlockCoordinates coordinates)
		{
			return _level.IsAir(coordinates);
		}
	}
}