using System;
using System.Collections.Generic;
using MiNET.Entities;

namespace MiNET.Worlds
{
	public class EntityManager
	{
		private List<Entity> _entities = new List<Entity>();

		public EntityManager()
		{
			_entities.Add(new Entity(-1, null));
		}

		public void AddEntity(Entity caller, Entity entity)
		{
			if (caller != null && entity != caller) throw new Exception("Tried to ADD entity for someone else. Should be the entity himself adding");

			lock (_entities)
			{
				if (entity.EntityId == -1)
				{
					if (_entities.Contains(entity))
						throw new Exception("Tried to add entity that already existed.");
					_entities.Add(entity);
					entity.EntityId = _entities.IndexOf(entity);
				}
			}
		}

		public void RemoveEntity(Entity caller, Entity entity)
		{
			if (entity == caller) throw new Exception("Tried to REMOVE entity for self");

			lock (_entities)
			{
				int entityId = _entities.IndexOf(entity);
				if (entityId == -1)
					throw new Exception("Expected to find entity on remove. A missed ADD perhaps?");
				_entities.Remove(entity);
				entity.EntityId = -1;
			}
		}

		public int GetEntityId(Entity caller, Entity entity)
		{
			if (entity == caller) return 0;

			//lock (_entities)
			//{
			int entityId = entity.EntityId;
			if (entityId == -1)
				throw new Exception("Expected to find entity in entities, but didn't exist. Need to AddEntity first.");

			return entityId;
			//}
		}

		public Entity GetEntity(int entityId)
		{
			//lock (_entities)
			//{
			Entity player = _entities[entityId];
			if (player == null)
				throw new Exception("Expected to find entity in entities, but didn't exist. Need to AddEntity first.");

			return player;
			//}
		}
	}
}