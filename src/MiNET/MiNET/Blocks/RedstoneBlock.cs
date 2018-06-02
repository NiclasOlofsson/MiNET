using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class RedstoneBlock : Block
	{
		public RedstoneBlock() : base(152)
		{
			BlastResistance = 30;
			Hardness = 5;
            Power = 15;
        }

        public override void DoPhysics(Level level)
        {
            base.DoPhysics(level);
            //OnPower(level, Coordinates, 15, PowerAction.PowerBlock);
        }

        public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
        {
            base.BlockUpdate(level, blockCoordinates);
            //OnPower(level, Coordinates, 15, PowerAction.PowerBlock);
        }

        public override void BreakBlock(Level world, bool silent = false)
        {
            base.BreakBlock(world, silent);
            //OnPower(world, Coordinates, 0, PowerAction.UnPower);
        }

        public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
        {
            //OnPower(world, Coordinates, 15, PowerAction.PowerBlock);
            return base.PlaceBlock(world, player, targetCoordinates, face, faceCoords);
        }
    }
}