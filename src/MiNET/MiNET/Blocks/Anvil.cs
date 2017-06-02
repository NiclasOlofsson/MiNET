using System.Numerics;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Anvil : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Anvil));

		public const int Normal = 0;
		public const int SlightlyDamaged = 4;
		public const int VeryDamaged = 8;

		public Anvil() : base(145)
		{
			IsTransparent = true;
			BlastResistance = 6000;
			Hardness = 5;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();
			Metadata = (byte) ((Metadata & 0x0c) | (direction & 0x03));

			world.SetBlock(this);

			return true;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			var containerOpen = McpeContainerOpen.CreateObject();
			containerOpen.windowId = 5 + 9;
			containerOpen.type = 5;
			containerOpen.coordinates = blockCoordinates;
			containerOpen.unknownRuntimeEntityId = 1;
			player.SendPackage(containerOpen);

			return true;
		}
	}
}