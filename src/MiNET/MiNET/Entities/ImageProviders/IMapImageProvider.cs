using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Entities.ImageProviders
{
	public interface IMapImageProvider
	{
		byte[] GetData(MapInfo mapInfo, bool forced);
		McpeClientboundMapItemData GetClientboundMapItemData(MapInfo mapInfo);
		McpeWrapper GetBatch(MapInfo mapInfo, bool forced);
	}
}