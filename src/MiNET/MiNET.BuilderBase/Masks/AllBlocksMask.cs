using MiNET.Utils;

namespace MiNET.BuilderBase.Masks
{
	public class AllBlocksMask : Mask
	{
		public AllBlocksMask()
		{
		}

		public override bool Test(BlockCoordinates coordinates)
		{
			return true;
		}
	}
}