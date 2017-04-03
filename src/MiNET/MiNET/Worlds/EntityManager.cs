using System;
using System.Threading;
using MiNET.Entities;

namespace MiNET.Worlds
{
	public class EntityManager
	{
		public const long EntityIdUndefined = -1;
		public const long EntityIdSelf = 2;

		private long _entityId = EntityIdSelf + 1;

		public long AddEntity(Entity entity)
		{
			if (entity.EntityId == EntityIdUndefined)
			{
				entity.EntityId = Interlocked.Increment(ref _entityId);
			}

			return entity.EntityId;
		}

		public void RemoveEntity(Entity caller, Entity entity)
		{
			if (entity == caller) throw new Exception("Tried to REMOVE entity for self");
			if (entity.EntityId != EntityIdUndefined) entity.EntityId = EntityIdUndefined;
		}
	}
}