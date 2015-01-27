using System;
using System.Collections.Generic;

namespace MiNET.Worlds
{
	public class EntityManager
	{
		private List<Player> _entities = new List<Player>();

		public EntityManager()
		{
			_entities.Add(new Player());
		}

		public void AddEntity(Player caller, Player player)
		{
			if (caller != null && player != caller) throw new Exception("Tried to ADD entity for someone else. Should be the player himself adding");

			lock (_entities)
			{
				if (player.EntityId == -1)
				{
					if (_entities.Contains(player))
						throw new Exception("Tried to add entity that already existed.");
					_entities.Add(player);
					player.EntityId = _entities.IndexOf(player);
				}
			}
		}

		public void RemoveEntity(Player caller, Player player)
		{
			if (player == caller) throw new Exception("Tried to REMOVE entity for self");

			lock (_entities)
			{
				int entityId = _entities.IndexOf(player);
				if (entityId == -1)
					throw new Exception("Expected to find entity on remove. A missed ADD perhaps?");
				_entities.Remove(player);
				player.EntityId = -1;
			}
		}

		public int GetEntityId(Player caller, Player player)
		{
			if (player == caller) return 0;

			//lock (_entities)
			//{
			int entityId = player.EntityId;
			if (entityId == -1)
				throw new Exception("Expected to find player in entities, but didn't exist. Need to AddEntity first.");

			return entityId;
			//}
		}

		public Player GetPlayer(int entityId)
		{
			//lock (_entities)
			//{
			Player player = _entities[entityId];
			if (player == null)
				throw new Exception("Expected to find player in entities, but didn't exist. Need to AddEntity first.");

			return player;
			//}
		}
	}
}