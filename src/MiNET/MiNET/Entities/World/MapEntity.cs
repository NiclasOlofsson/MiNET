using System;
using log4net;
using MiNET.Entities.ImageProviders;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class MapEntity : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MapEntity));

		public MapInfo MapInfo { get; set; }
		public IMapImageProvider ImageProvider { get; set; }

		public MapEntity(Level level, long mapId = EntityManager.EntityIdUndefined) : base(-1, level)
		{
			if (mapId != EntityManager.EntityIdUndefined)
			{
				EntityId = mapId;
			}
			else
			{
				EntityId = level.EntityManager.AddEntity(this) + 0xFFFF;
			}

			ImageProvider = new MapImageProvider();
			//ImageProvider = new RandomColorMapImageProvider();

			MapInfo mapInfo = new MapInfo
			{
				MapId = EntityId,
				UpdateType = 6,
				Scale = 0,
				X = 0,
				Z = 0,
				Col = 128,
				Row = 128,
				XOffset = 0,
				ZOffset = 0
			};

			MapInfo = mapInfo;
		}

		public override void SpawnToPlayers(Player[] players)
		{
			// This is a server-side only entity
		}

		public override void DespawnFromPlayers(Player[] players)
		{
			// This is a server-side only entity
		}

		public override void OnTick()
		{
			if (Level.TickTime%2 != 0) return;

			// if no image provider, do nothing
			if (ImageProvider == null) return;

			MapInfo.Decorators = new MapDecorator[1];
			for (int i = 0; i < MapInfo.Decorators.Length; i++)
			{
				var decorator = new MapDecorator
				{
					Rotation = (byte) (Level.TickTime%16),
					Icon = (byte) 1,
					X = (byte) (Level.TickTime%255),
					Z = (byte) (Level.TickTime%255),
					Label = "",
					Color = BitConverter.ToUInt32(new byte[] { 0xff, 0xff, 0xff, 0xff }, 0),
				};

				MapInfo.Decorators[i] = decorator;
			}

			var data = ImageProvider.GetData(MapInfo, false);
			if (data != null)
			{
				MapInfo.Data = data;
				var mapInfo = (MapInfo) MapInfo.Clone();

				McpeClientboundMapItemData msg = McpeClientboundMapItemData.CreateObject();
				msg.mapinfo = mapInfo;
				Level.RelayBroadcast(msg);

				return;
			}

			var packet = ImageProvider.GetClientboundMapItemData(MapInfo);
			if (packet != null)
			{
				Level.RelayBroadcast(packet);

				return;
			}

			var batchPacket = ImageProvider.GetBatch(MapInfo, false);
			if (batchPacket != null)
			{
				Level.RelayBroadcast(batchPacket);
			}
		}

		public virtual void AddToMapListeners(Player player, long mapId)
		{
			if (mapId == EntityId)
			{
				if (ImageProvider == null) return;

				var data = ImageProvider.GetData(MapInfo, true);
				if (data != null)
				{
					MapInfo.Data = data;
					var mapInfo = (MapInfo)MapInfo.Clone();

					McpeClientboundMapItemData msg = McpeClientboundMapItemData.CreateObject();
					msg.mapinfo = mapInfo;
					player.SendPackage(msg);

					return;
				}

				var packet = ImageProvider.GetClientboundMapItemData(MapInfo);
				if (packet != null)
				{
					player.SendPackage(packet);

					return;
				}

				var batchPacket = ImageProvider.GetBatch(MapInfo, true);
				if (batchPacket != null)
				{
					player.SendPackage(batchPacket);
				}
			}
		}
	}
}