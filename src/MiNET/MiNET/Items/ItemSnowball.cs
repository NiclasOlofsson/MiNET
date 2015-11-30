using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSnowball : Item
	{
		public ItemSnowball(short metadata) : base(332, metadata)
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

			Snowball snowBall = new Snowball(null, world);
			snowBall.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			snowBall.KnownPosition.Y += 1.62f;
			snowBall.Velocity = snowBall.KnownPosition.GetDirection() * (force);
			snowBall.BroadcastMovement = false;
			snowBall.DespawnOnImpact = true;
			snowBall.SpawnEntity();
		}
	}
}