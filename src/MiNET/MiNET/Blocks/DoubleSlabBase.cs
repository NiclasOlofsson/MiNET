using System.Collections.Generic;
using MiNET.Items;

namespace MiNET.Blocks
{
	public abstract class DoubleSlabBase : SlabBase
	{
		public override Item GetItem(bool blockItem = false)
		{
			return ItemFactory.GetItem(DoubleSlabToSlabMap[Id], GetGlobalState().Data);
		}

		public override Item[] GetDrops(Item tool)
		{
			var item = GetItem();

			if (item == null) return new Item[0];

			item.Count = 2;

			return new[] { item }; 
		}
	}
}
