using MiNET.Utils;

namespace MiNET.BuilderBase.Masks
{
	public class AnyBlockMask : Mask
	{
		public AnyBlockMask()
		{
		}

		public override bool Test(BlockCoordinates coordinates)
		{
			return true;
		}
	}
}