using System.Numerics;
using log4net;
using MiNET.BlockEntities;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class ItemFrame : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemFrame));

		public ItemFrame() : base(199)
		{
			IsSolid = false;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();

			switch (face)
			{
				case BlockFace.South: // ok
					Metadata = 0;
					break;
				case BlockFace.North:
					Metadata = 1;
					break;
				case BlockFace.West:
					Metadata = 2;
					break;
				case BlockFace.East: // ok
					Metadata = 3;
					break;
			}

			Log.Warn($"Direction={direction}, face={face}, metadata={Metadata}");

			world.SetBlock(this);

			return true;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			Item itemInHande = player.Inventory.GetItemInHand();

			ItemFrameBlockEntity blockEntity = world.GetBlockEntity(blockCoordinates) as ItemFrameBlockEntity;
			if (blockEntity != null)
			{
				blockEntity.SetItem(itemInHande);
				world.SetBlockEntity(blockEntity);
			}

			return true;
		}
	}
}