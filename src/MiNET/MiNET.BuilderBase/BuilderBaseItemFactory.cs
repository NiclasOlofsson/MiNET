using fNbt;
using MiNET.BuilderBase.Tools;
using MiNET.Items;

namespace MiNET.BuilderBase
{
	public class BuilderBaseItemFactory : ICustomItemFactory
	{
		public Item GetItem(short id, short metadata, int count)
		{
			if (id == new BrushTool().LegacyId)
			{
				return new BrushTool();
			}
			else if (id == new DistanceWand().LegacyId)
			{
				return new DistanceWand();
			}
			else if (id == new TeleportTool().LegacyId)
			{
				return new TeleportTool();
			}

			return null;
		}
	}
}