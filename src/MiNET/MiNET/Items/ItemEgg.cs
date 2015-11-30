using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemEgg : Item
	{
		public ItemEgg(short metadata) : base(344, metadata)
		{
			MaxStackSize = 16;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			var inventory = player.Inventory;
			var itemInHand = inventory.GetItemInHand();

			itemInHand.Count--;
			if (itemInHand.Count <= 0)
			{
				// set empty
				inventory.Slots[inventory.Slots.IndexOf(itemInHand)] = new ItemStack();
			}

			float force = 1.5f;

			Egg egg = new Egg(null, world);
			egg.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			egg.KnownPosition.Y += 1.62f;
			egg.Velocity = egg.KnownPosition.GetDirection()*(force);
			egg.BroadcastMovement = false;
			egg.DespawnOnImpact = true;
			egg.SpawnEntity();
		}
	}
}