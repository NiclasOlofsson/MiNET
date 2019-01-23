#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

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

			if (MapData.Length != (mapInfo.Col * mapInfo.Row * 4))
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
			byte[] bytes = new byte[map.Col * map.Row * 4];

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