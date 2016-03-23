using System;
using log4net;
using MiNET.Entities;
using MiNET.Entities.World;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemEmptyMap : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemEmptyMap));

		public ItemEmptyMap(short metadata = 0, byte count = 1) : base(395, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			MapEntity mapEntity = new MapEntity(world);
			mapEntity.SpawnEntity();

			// Initialize a new map and add it.
			ItemMap itemMap = new ItemMap(mapEntity.EntityId);
			player.Inventory.SetFirstEmptySlot(itemMap, true, false);
		}
	}
}