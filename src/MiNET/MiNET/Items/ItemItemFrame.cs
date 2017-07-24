using System.Numerics;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemItemFrame : ItemBlock
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemItemFrame));

		public ItemItemFrame() : base(199, 0)
		{
		}

		public override Item GetSmelt()
		{
			return null;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);

			ItemFrame itemFrame = new ItemFrame
			{
				Coordinates = coor,
			};

			if (!itemFrame.CanPlace(world, blockCoordinates, face)) return;

			itemFrame.PlaceBlock(world, player, coor, face, faceCoords);

			// Then we create and set the sign block entity that has all the intersting data

			ItemFrameBlockEntity itemFrameBlockEntity = new ItemFrameBlockEntity
			{
				Coordinates = coor
			};

			world.SetBlockEntity(itemFrameBlockEntity);
		}
	}
}