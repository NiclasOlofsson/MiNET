using System.Numerics;
using fNbt;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemMap : Item
	{
		public long MapId
		{
			get
			{
				if (ExtraData == null) return 0;

				return long.Parse(ExtraData["map_uuid"].StringValue);
			}
			set
			{
				ExtraData = new NbtCompound("tag") {new NbtString("map_uuid", value.ToString())};
			}
		}

		public ItemMap(long mapId = 0, byte count = 1) : base(358, 0, count)
		{
			MapId = mapId;
			MaxStackSize = 1;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
		}
	}
}