using MiNET.Items;

namespace MiNET.Blocks
{
	public class Air : Block
	{
		public Air() : base(0)
		{
			IsReplacible = true;
			IsSolid = false;
			IsBuildable = false;
		}

		public override Item GetDrops()
		{
			return null;
		}
	}
}