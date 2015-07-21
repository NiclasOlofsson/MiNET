using System;
using System.Threading;
using MiNET.Entities;

namespace MiNET.Worlds
{
	public class EntityManager
	{
		public const long EntityIdUndefined = -1;

		private long _entityId = 1;

		public long AddEntity(Entity caller, Entity entity)
		{
			if (caller != null && entity != caller) throw new Exception("Tried to ADD entity for someone else. Should be the entity himself adding");

			if (entity.EntityId == EntityIdUndefined)
			{
				entity.EntityId = Interlocked.Increment(ref _entityId);
			}

			return entity.EntityId;
		}

		public void RemoveEntity(Entity caller, Entity entity)
		{
			if (entity == caller) throw new Exception("Tried to REMOVE entity for self");
		}
	}
}