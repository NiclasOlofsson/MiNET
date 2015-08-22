using log4net;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSpawnEgg : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemSpawnEgg));

		public ItemSpawnEgg(short metadata) : base(383, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

			Mob entity = new Mob(Metadata, world)
			{
				KnownPosition = new PlayerLocation(coordinates.X, coordinates.Y, coordinates.Z),
				//Data = -(blockId | 0 << 0x10)
			};
			entity.SpawnEntity();

			Log.DebugFormat("Player {0} spawned Mob #{1}.", player.Username, Metadata);
			//world.BroadcastTextMessage(string.Format("Player {0} spawned Mob #{1}.", player.Username, Metadata));
		}
	}
}