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
		private readonly Block _block;

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
			_block.Coordinates = GetNewCoordinatesFromFace(targetCoordinates, face);
			_block.Metadata = (byte) Metadata;

			if (!_block.CanPlace(world)) return;

			if (_block.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return; // Handled

			world.SetBlock(_block);
		}
	}
}