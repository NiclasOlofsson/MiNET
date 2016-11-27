using fNbt;
using MiNET.Items;

namespace MiNET.BuilderBase
{
	public class BuilderBaseItemFactory : ICustomItemFactory
	{
		public Item GetItem(short id, short metadata, byte count)
		{
			if (id == new BrushTool().Id)
			{
				return new BrushTool();
			}
			else if (id == new DistanceWand().Id)
			{
				return new DistanceWand();
			}

			return null;
		}
	}
}