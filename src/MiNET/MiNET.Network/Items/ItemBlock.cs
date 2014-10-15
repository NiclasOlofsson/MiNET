using Craft.Net.Common;
using MiNET.Network.Blocks;
using MiNET.Network.Worlds;

namespace MiNET.Network.Items
{
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

			world.SetBlock(_block);
		}
	}
}