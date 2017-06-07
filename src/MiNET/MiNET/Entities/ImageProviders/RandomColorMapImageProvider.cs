using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Entities.ImageProviders
{
	public class RandomColorMapImageProvider : IMapImageProvider
	{
		private WannabeRandom _random = new WannabeRandom();

		public RandomColorMapImageProvider()
		{
		}

		public virtual byte[] GetData(MapInfo mapInfo, bool forced)
		{
			return GenerateColors(mapInfo, (byte) _random.NextInt(255));
		}

		public virtual McpeClientboundMapItemData GetClientboundMapItemData(MapInfo mapInfo)
		{
			return null;
		}

		public virtual McpeWrapper GetBatch(MapInfo mapInfo, bool forced)
		{
			return null;
		}

		private byte[] GenerateColors(MapInfo map, byte next)
		{
			byte[] bytes = new byte[map.Col*map.Row*4];

			int i = 0;
			for (byte y = 0; y < map.Col; y++)
			{
				for (byte x = 0; x < map.Row; x++)
				{
					bytes[i++] = (byte) _random.NextInt(255); // R
					bytes[i++] = (byte) _random.NextInt(255); // G
					bytes[i++] = (byte) _random.NextInt(255); // B
					bytes[i++] = 0xff; // A
				}
			}

			return bytes;
		}
	}
}