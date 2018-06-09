using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Torch : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Torch));

		public Torch() : this(50)
		{
			LightLevel = 14;
		}
        protected Torch(byte id) : base(id)
        {
            IsTransparent = true;
            IsSolid = false;
        }

        protected bool CanPlaceTorch(Level world, BlockCoordinates targetCoordinates, BlockFace face)
        {
            if (face == BlockFace.Down)
                return false;
            Block block = world.GetBlock(targetCoordinates);

            if (block is Farmland)
                return false;
            if (block is Glass)
                return true;

            if (block is Fence || block is CobblestoneWall)
                return face == BlockFace.Up;
            
            else if (block is BlockStairs)
            {

            }
            if (block is WoodenSlab || block is StoneSlab || block is StoneSlab2)
                return block.Metadata > 7;

            return !block.IsTransparent;
        }

        protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
        {
            return true;
        }

        public override void DoPhysics(Level level)
        {
            var face = GetFaceByMetadata(Metadata);
            if (!CanPlaceTorch(level, GetNewCoordinatesFromFace(Coordinates, GetOppositeFace(face)), face))
                level.BreakBlock(this);
        }

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
            if (!world.GetBlock(Coordinates).IsReplacible)
                return true;

            bool canPlace = CanPlaceTorch(world, blockCoordinates, face);
            if (canPlace)
                Metadata = GetMetadataByFace(face);
            else
            {
                for (var i = face != BlockFace.Up ? 2 : 1; i < 6; i++)
                    if (CanPlaceTorch(world, GetNewCoordinatesFromFace(Coordinates, GetOppositeFace((BlockFace)i)), (BlockFace)i))
                    {
                        Metadata = GetMetadataByFace((BlockFace)i);
                        canPlace = true;
                        break;
                    }
                if(!canPlace && face > BlockFace.Up)
                    if (CanPlaceTorch(world, GetNewCoordinatesFromFace(Coordinates, BlockFace.Down), BlockFace.Up))
                    {
                        Metadata = GetMetadataByFace(BlockFace.Up);
                        canPlace = true;
                    }
            }
            if (!canPlace)
                return true;

            return false;
		}

        protected BlockFace GetOppositeFace(BlockFace face)
        {
            switch (face)
            {
                case BlockFace.Down:
                    return BlockFace.Up;
                case BlockFace.Up:
                    return BlockFace.Down;
                case BlockFace.East:
                    return BlockFace.West;
                case BlockFace.West:
                    return BlockFace.East;
                case BlockFace.North:
                    return BlockFace.South;
                case BlockFace.South:
                    return BlockFace.North;
            }
            return BlockFace.None;
        }

        protected byte GetMetadataByFace(BlockFace face)
        {
            byte metadata = 0;
            switch (face)
            {
                case BlockFace.Up:
                    metadata = 5;
                    break;
                case BlockFace.East:
                    metadata = 4;
                    break;
                case BlockFace.West:
                    metadata = 3;
                    break;
                case BlockFace.North:
                    metadata = 2;
                    break;
                case BlockFace.South:
                    metadata = 1;
                    break;
            }
            return metadata;
        }

        protected BlockFace GetFaceByMetadata(byte metadata)
        {
            BlockFace face = BlockFace.Down;
            switch (metadata)
            {
                case 5:
                    face = BlockFace.Up;
                    break;
                case 4:
                    face = BlockFace.East;
                    break;
                case 3:
                    face = BlockFace.West;
                    break;
                case 2:
                    face = BlockFace.North;
                    break;
                case 1:
                    face = BlockFace.South;
                    break;
            }
            return face;
        }

        public override Item[] GetDrops(Item tool)
		{
			Metadata = 0;
			return base.GetDrops(tool);
		}
	}
}