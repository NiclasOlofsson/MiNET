using System.Numerics;
using log4net;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	public class BrushTool : ItemWoodenShovel
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BrushTool));

		public int Radius { get; set; } = 4;
		public int Range { get; set; } = 300;
		public Pattern Pattern { get; set; } = new Pattern(1, 0);
		public Mask Mask { get; set; } = new AllBlocksMask();
		public bool Filled { get; set; } = true;

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			var target = new EditHelper(world).GetBlockInLineOfSight(player.Level, player.KnownPosition);
			if (target == null)
			{
				player.SendMessage("No block in range");
				return;
			}

			new EditHelper(player.Level, Mask).MakeSphere(target.Coordinates, Pattern, Radius, Radius, Radius, Filled);
		}
	}
}