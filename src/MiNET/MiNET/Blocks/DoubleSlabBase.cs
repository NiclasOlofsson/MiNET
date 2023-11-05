using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class DoubleSlabBase : SlabBase
	{
		public override Item GetItem(Level world, bool blockItem = false)
		{
			return ItemFactory.GetItem(DoubleSlabToSlabMap[Id], GetGlobalState().Data);
		}

		public override Item[] GetDrops(Level world, Item tool)
		{
			var item = GetItem(world);

			if (item == null) return new Item[0];

			item.Count = 2;

			return new[] { item }; 
		}
	}
}
