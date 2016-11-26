using MiNET.Items;

namespace MiNET.BuilderBase
{
	public class BuilderBaseItemFactory : ICustomItemFactory
	{
		public Item GetItem(short id, short metadata, byte count)
		{
			if (id == new ItemStick().Id)
			{
				return new DistanceWand();
			}

			return null;
		}
	}
}