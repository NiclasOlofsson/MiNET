using System.Numerics;
using log4net;
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
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemBlock));

		protected Block _block;

		protected ItemBlock(short id, short metadata) : base(id, metadata)
		{
		}

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
			Block block = world.GetBlock(targetCoordinates);
			_block.Coordinates = block.IsReplacible ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);

			_block.Metadata = (byte) Metadata;

			if ((player.GetBoundingBox() - 0.01f).Intersects(_block.GetBoundingBox()))
			{
				Log.Debug("Can't build where you are standing: " + _block.GetBoundingBox());
				return;
			}
			if (!_block.CanPlace(world, face)) return;

			if (_block.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return; // Handled

			world.SetBlock(_block);
		}
	}
}