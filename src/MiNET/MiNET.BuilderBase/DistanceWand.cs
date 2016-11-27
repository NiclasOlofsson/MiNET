using System.Numerics;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	public class DistanceWand : ItemWoodenAxe
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (DistanceWand));

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			SetPosition2(player, blockCoordinates);
		}

		public override bool BreakBlock(Level world, Player player, Block block, BlockEntity blockEntity)
		{
			SetPosition1(player, block.Coordinates);

			return false; // Will revert block break;
		}

		public void SetPosition1(Player player, BlockCoordinates pos)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			selector.SelectPrimary(pos);

			player.SendMessage($"First position set to {pos}");
		}

		public void SetPosition2(Player player, BlockCoordinates pos)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			selector.SelectSecondary(pos);

			player.SendMessage($"Second position set to {pos}");
		}
	}
}