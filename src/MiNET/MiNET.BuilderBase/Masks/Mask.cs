using MiNET.Utils;

namespace MiNET.BuilderBase.Masks
{
	public abstract class Mask
	{
		public abstract bool Test(BlockCoordinates coordinates);
	}
}