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
			IsTransparent = true;
			IsBlockingSkylight = false;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0];
		}
	}
}