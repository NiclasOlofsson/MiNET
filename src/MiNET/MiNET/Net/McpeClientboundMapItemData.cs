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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using MiNET.Utils;

namespace MiNET.Net
{
	public partial class McpeClientboundMapItemData : Packet<McpeClientboundMapItemData>
	{
		// MapInfo's UpdateType bitmask is the server-side model and no longer travels on the wire.
		// Since 2168 each payload is a presence-flagged optional and the client derives the type
		// bits from which ones arrived, so these decide what to populate rather than what to write.
		private const int MapUpdateFlagTexture = 0x02;
		private const int MapUpdateFlagDecoration = 0x04;
		private const int MapUpdateFlagInitialisation = 0x08;

		/// <summary>
		///     Builds the packet from the map model the image providers fill in. Every payload is
		///     copied out, so the caller's MapInfo can keep changing while this is queued.
		/// </summary>
		public static McpeClientboundMapItemData FromMapInfo(MapInfo map)
		{
			McpeClientboundMapItemData packet = CreateObject();
			packet.mapId = map.MapId;
			packet.dimension = 0;
			packet.isLocked = false;
			packet.mapOrigin = map.Origin;

			if ((map.UpdateType & MapUpdateFlagInitialisation) != 0)
			{
				packet.creationMapIds = new List<long> {map.MapId};
			}

			if ((map.UpdateType & (MapUpdateFlagInitialisation | MapUpdateFlagDecoration | MapUpdateFlagTexture)) != 0)
			{
				packet.scale = (sbyte) map.Scale;
			}

			if ((map.UpdateType & MapUpdateFlagDecoration) != 0)
			{
				packet.trackedActorIds = new List<MapItemTrackedActorUniqueId>(map.Decorators.Length);
				packet.decorations = new List<MapDecoration>(map.Decorators.Length);

				foreach (MapDecorator decorator in map.Decorators)
				{
					var tracked = new MapItemTrackedActorUniqueId();
					switch (decorator)
					{
						case EntityMapDecorator entity:
							tracked.type = MapItemTrackedActorUniqueId.MapItemTrackedActorType.Entity;
							tracked.entityId = entity.EntityId;
							break;
						case BlockMapDecorator block:
							tracked.type = MapItemTrackedActorUniqueId.MapItemTrackedActorType.BlockEntity;
							tracked.blockPosition = block.Coordinates;
							break;
						default:
							tracked.type = MapItemTrackedActorUniqueId.MapItemTrackedActorType.Other;
							break;
					}
					packet.trackedActorIds.Add(tracked);

					packet.decorations.Add(new MapDecoration
					{
						imageType = (MapDecoration.Type) decorator.Icon,
						rotation = decorator.Rotation,
						x = decorator.X,
						y = decorator.Z,
						label = decorator.Label,
						color = (int) decorator.Color,
					});
				}
			}

			if ((map.UpdateType & MapUpdateFlagTexture) != 0)
			{
				packet.width = map.Col;
				packet.height = map.Row;
				packet.startX = map.XOffset;
				packet.startY = map.ZOffset;

				// The providers hand us RGBA bytes; the wire carries one packed colour per pixel,
				// alpha forced opaque the way the old codec did.
				packet.pixels = new List<uint>(map.Col * map.Row);
				int i = 0;
				for (int pixel = 0; pixel < map.Col * map.Row; pixel++)
				{
					byte red = map.Data[i++];
					byte green = map.Data[i++];
					byte blue = map.Data[i++];
					i++; // alpha, replaced below
					packet.pixels.Add((uint) (red | green << 8 | blue << 16 | 0xff << 24));
				}
			}

			return packet;
		}
	}
}
