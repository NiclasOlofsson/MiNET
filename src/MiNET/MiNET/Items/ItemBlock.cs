using Craft.Net.Common;
using MiNET.Blocks;
using MiNET.Worlds;

namespace MiNET.Items
{
	/// <summary>
	///     Generic Item that will simply place the block on use. No interaction or other use supported by the block.
	/// </summary>
	public class ItemBlock : Item
	{
		private readonly Block _block;

		public ItemBlock(Block block) : base(block.Id)
		{
			_block = block;
		}

		public override void UseItem(Level world, Player player, Coordinates3D targetCoordinates, BlockFace face)
		{
			_block.Coordinates = GetNewCoordinatesFromFace(targetCoordinates, face);
			_block.Metadata = (byte) Metadata;

			if (!_block.CanPlace(world)) return;

			if (_block.PlaceBlock(world, player, targetCoordinates, face)) return; // Handled

			world.SetBlock(_block);
		}
	}
}