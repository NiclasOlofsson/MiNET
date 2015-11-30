using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	/// <summary>
	///     Generic Item that will simply place the block on use. No interaction or other use supported by the block.
	/// </summary>
	public class ItemBlock : Item
	{
		protected readonly Block _block;

		public ItemBlock(Block block, short metadata) : base(block.Id, metadata)
		{
			_block = block;
			FuelEfficiency = _block.FuelEfficiency;
		}

		public override Item GetSmelt()
		{
			return _block.GetSmelt();
		}


		public override void UseItem(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			ItemStack itemStackInHand = player.Inventory.GetItemInHand();
			itemStackInHand.Count--;
			if (itemStackInHand.Count <= 0)
			{
				// set empty
				player.Inventory.Slots[player.Inventory.Slots.IndexOf(itemStackInHand)] = new ItemStack();
			}

			_block.Coordinates = GetNewCoordinatesFromFace(targetCoordinates, face);
			_block.Metadata = (byte) Metadata;

			if (player.GetBoundingBox().Intersects(_block.GetBoundingBox())) return;
			if (!_block.CanPlace(world, face)) return;

			if (_block.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return; // Handled

			world.SetBlock(_block);
		}
	}
}