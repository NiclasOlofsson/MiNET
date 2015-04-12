using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiNET;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin.MobHunt
{
	public class MobHuntLevel : Level
	{
		private Dictionary<Player, Entity> _playerEntities = new Dictionary<Player, Entity>();

		public MobHuntLevel(string levelId, IWorldProvider worldProvider = null) : base(levelId, worldProvider)
		{
		}

		public override void AddPlayer(Player newPlayer, string broadcastText = null, bool spawn = true)
		{
			base.AddPlayer(newPlayer, broadcastText, false);
			Mob entity = new Mob(new Random().Next(10, 16), this) // Passive mobs
				//Mob entity = new Mob(new Random().Next(32, 39), this) // Hostile mobs
			{
				KnownPosition = newPlayer.KnownPosition,
				//Data = -(blockId | 0 << 0x10)
			};
			entity.SpawnEntity();

			// Despawn the new entity from the player himeself
			newPlayer.SendPackage(new McpeRemoveEntity()
			{
				entityId = entity.EntityId,
			});

			_playerEntities.Add(newPlayer, entity);
		}

		public override void RemovePlayer(Player player, bool despawn = true)
		{
			base.RemovePlayer(player, despawn);

			Entity entity;
			if (!_playerEntities.TryGetValue(player, out entity)) return;
			entity.DespawnEntity();
		}

		protected override void BroadCastMovement(Player[] players, Player[] updatedPlayers)
		{
			if (updatedPlayers.Length == 0) return;

			var moveEntity = McpeMoveEntity.CreateObject(players.Count());
			moveEntity.entities = new EntityLocations();

			//var rotateHead = McpeRotateHead.CreateObject(players.Count());
			//rotateHead.entities = new EntityHeadRotations();

			foreach (var player in updatedPlayers)
			{
				Entity entity;
				if (!_playerEntities.TryGetValue(player, out entity)) continue;

				entity.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
				if (entity.EntityTypeId == 10)
				{
					//BUG: Duck has it's value reversed
					entity.KnownPosition.Pitch = -player.KnownPosition.Pitch;
				}
				moveEntity.entities.Add(entity.EntityId, entity.KnownPosition);
				//rotateHead.entities.Add(entity.EntityId, entity.KnownPosition);
			}

			moveEntity.Encode();
			//rotateHead.Encode();

			new Task(() => RelayBroadcast(moveEntity)).Start();
			//new Task(() => RelayBroadcast(rotateHead)).Start();
		}
	}
}