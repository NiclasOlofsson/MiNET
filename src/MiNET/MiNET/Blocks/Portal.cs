using MiNET.Items;

namespace MiNET.Blocks
{
	public class Portal : Block
	{
		public Portal() : base(90)
		{
			IsTransparent = true;
			IsSolid = false;
			LightLevel = 11;
			Hardness = 60000;
		}

		public override Item GetDrops()
		{
			return null;
		}
	}
}