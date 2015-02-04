using System;
using System.Threading;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Entity
	{
		private Timer _entityTicker;

		public Level Level { get; set; }

		public int EntityTypeId { get; private set; }
		public int EntityId { get; set; }

		public DateTime LastUpdatedTime { get; set; }
		public PlayerPosition3D KnownPosition { get; set; }


		public Entity(int entityTypeId, Level level)
		{
			EntityId = EntityManager.EntityIdUndefined;
			Level = level;
			EntityTypeId = entityTypeId;
			KnownPosition = new PlayerPosition3D();
		}

		public virtual MetadataDictionary GetMetadata()
		{
			return new MetadataDictionary();
		}

		protected virtual void OnTick()
		{
		}

		public virtual void SpawnEntity()
		{
			Level.AddEntity(this);
			_entityTicker = new Timer(OnUpdate, null, 0, 50);
		}

		protected virtual void OnUpdate(object state)
		{
			OnTick();
		}

		protected virtual void DespawnEntity()
		{
			_entityTicker.Dispose();
			Level.RemoveEntity(this);
		}
	}
}