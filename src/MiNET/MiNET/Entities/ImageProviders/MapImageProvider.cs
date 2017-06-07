using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Entities.ImageProviders
{
	public class MapImageProvider : IMapImageProvider
	{
		private bool _alreadySent;

		public byte[] MapData { get; set; }
		public McpeClientboundMapItemData ClientboundMapItemData { get; set; }
		public McpeWrapper Batch { get; set; }

		public MapImageProvider()
		{
		}


		public virtual byte[] GetData(MapInfo mapInfo, bool forced)
		{
			if (ClientboundMapItemData != null || Batch != null) return null;

			byte[] data = null;

			if (MapData == null)
			{
				return null;
			}

			if (MapData.Length != (mapInfo.Col*mapInfo.Row*4))
			{
				return null;
			}

			if (!_alreadySent)
			{
				_alreadySent = true;
				data = MapData;
			}
			else if (forced)
			{
				data = MapData;
			}

			return data;
		}

		public virtual McpeClientboundMapItemData GetClientboundMapItemData(MapInfo mapInfo)
		{
			return null;
		}

		public virtual McpeWrapper GetBatch(MapInfo mapInfo, bool forced)
		{
			return forced ? Batch : null;
		}

		private byte[] GenerateColors(MapInfo map)
		{
			byte[] bytes = new byte[map.Col*map.Row*4];

			int i = 0;
			for (byte y = 0; y < map.Col; y++)
			{
				for (byte x = 0; x < map.Row; x++)
				{
					bytes[i++] = 255; // R
					bytes[i++] = 0; // G
					bytes[i++] = 0; // B
					bytes[i++] = 0xff; // A
				}
			}

			return bytes;
		}
	}
}