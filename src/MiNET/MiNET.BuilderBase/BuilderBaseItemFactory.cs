using fNbt;
using MiNET.BuilderBase.Tools;
using MiNET.Items;

namespace MiNET.BuilderBase
{
	public class BuilderBaseItemFactory : ICustomItemFactory
	{
		public Item GetItem(short id, short metadata, int count)
		{
			if (id == new BrushTool().Id)
			{
				return new BrushTool();
			}
			else if (id == new DistanceWand().Id)
			{
				return new DistanceWand();
			}
			else if (id == new TeleportTool().Id)
			{
				return new TeleportTool();
			}

			return null;
		}
	}
}