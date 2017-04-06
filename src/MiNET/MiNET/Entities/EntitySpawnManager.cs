using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using log4net;
using MiNET.Blocks;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class EntitySpawnManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (EntitySpawnManager));

		public const int CapHostile = 70;
		public const int CapPassive = 10;
		public const int CapAmbient = 15;
		public const int CapWater = 5;

		public static readonly EntityType[] PassiveMobs = {EntityType.Chicken, EntityType.Cow, EntityType.Pig, EntityType.Sheep, EntityType.Wolf, EntityType.Horse};
		public static readonly EntityType[] HostileMobs = {EntityType.Zombie, EntityType.Skeleton, EntityType.Creeper, EntityType.Enderman};

		public Level Level { get; set; }

		public EntitySpawnManager(Level level)
		{
			Level = level;
		}


		public virtual void DespawnMobs(long tickTime)
		{
			if (tickTime%400 != 0) return;

			foreach (var entity in Level.Entities)
			{
				if (Level.Players.Count(player => player.Value.IsSpawned && Vector3.Distance(entity.Value.KnownPosition, player.Value.KnownPosition) < 128) == 0)
				{
					if (Log.IsDebugEnabled)
						Log.Debug($"Despawned entity because no players within 128 blocks distance");

					entity.Value.DespawnEntity();
					return;
				}
			}
		}

		public virtual void AttemptMobSpawn(long tickTime, BlockCoordinates blockCoordinates, int numberOfLoadedChunks)
		{
			bool canSpawnPassive = tickTime%400 == 0;
			bool canSpawnHostile = true;

			var entities = Level.Entities;
			var effectiveChunkCount = Math.Max(17*17, numberOfLoadedChunks);
			if (canSpawnPassive)
			{
				int entityCount = entities.Count(entity => entity.Value is PassiveMob && Vector3.Distance(blockCoordinates, entity.Value.KnownPosition) < effectiveChunkCount);
				canSpawnPassive = (entityCount < CapPassive*effectiveChunkCount/289);
			}

			{
				int entityCount = entities.Count(entity => entity.Value is HostileMob && Vector3.Distance(blockCoordinates, entity.Value.KnownPosition) < effectiveChunkCount);
				canSpawnHostile = (entityCount < CapHostile*effectiveChunkCount/289);
			}

			if (Level.Players.Count(player => player.Value.IsSpawned && Vector3.Distance(blockCoordinates, player.Value.KnownPosition) < 24) != 0)
			{
				//if (Log.IsDebugEnabled)
				//	Log.Debug($"Can't spawn entity because players within 24 blocks distance: {blockCoordinates}");
				return;
			}

			var random = new Random();

			for (int c = 0; c < 2; c++)
			{
				int maxPackSize = 0;
				int numberOfSpawnedMobs = 0;

				EntityType entityType = EntityType.None;

				bool doSpawnHostile = false;
				bool doSpawnPassive = false;
				if (c == 0)
				{
					if (!canSpawnHostile) continue;
					doSpawnHostile = true;
				}
				else if (c == 1)
				{
					if (!canSpawnPassive) continue;
					doSpawnPassive = true;
				}

				for (int i = 0; i < 12; i++)
				{
					int x = random.Next(20) + blockCoordinates.X;
					int y = blockCoordinates.Y;
					int z = random.Next(20) + blockCoordinates.Z;

					var spawnBlock = Level.GetBlock(x, y, z);
					//FIXME: The following is wrong. It shouldn't be the same for all mobs and need to be moved into some sort of
					// entity-based "CanSpawn()" method. But performance need to be handled too, and this is way faster right now.
					if (spawnBlock is Grass || spawnBlock is Sand || spawnBlock is Gravel || (doSpawnHostile && spawnBlock.IsSolid && !spawnBlock.IsTransparent))
					{
						if (entityType == EntityType.None)
						{
							entityType = SelectEntityType(spawnBlock, random, doSpawnHostile, doSpawnPassive);
							if (entityType == EntityType.None)
							{
								break;
							}

							maxPackSize = entityType == EntityType.Wolf ? 8 : entityType == EntityType.Horse ? 2 + random.Next(5) : entityType == EntityType.Enderman ? 1 + random.Next(4) : 4;
						}

						var firstBlock = Level.GetBlock(x, y + 1, z);
						if (!firstBlock.IsSolid)
						{
							if (doSpawnPassive
							    && PassiveMobs.Contains(entityType)
							    && ((firstBlock.BlockLight >= 9 || firstBlock.SkyLight >= 9) || (Level.CurrentWorldTime > 450 && Level.CurrentWorldTime < 11615)))
							{
								var secondBlock = Level.GetBlock(x, y + 2, z);
								if (!secondBlock.IsSolid)
								{
									var yaw = random.Next(360);

									if (Spawn(new PlayerLocation(x, y + 1, z, yaw + 15, yaw), entityType))
									{
										if (++numberOfSpawnedMobs >= maxPackSize) break;
									}
									else
									{
										if (Log.IsDebugEnabled)
											Log.Debug($"Failed to spawn {entityType} because area not clear");
									}
								}
							}
							else if (doSpawnHostile
							         && HostileMobs.Contains(entityType)
							         && firstBlock.BlockLight <= 7
							         && (firstBlock.SkyLight <= 7 || Level.CurrentWorldTime < 450 || Level.CurrentWorldTime > 11615))
							{
								var secondBlock = Level.GetBlock(x, y + 2, z);
								if (!secondBlock.IsSolid)
								{
									var yaw = random.Next(360);

									if (Spawn(new PlayerLocation(x, y + 1, z, yaw + 15, yaw), entityType))
									{
										if (++numberOfSpawnedMobs >= maxPackSize) break;
									}
									else
									{
										if (Log.IsDebugEnabled)
											Log.Debug($"Failed to spawn {entityType} because area not clear");
									}
								}
							}
						}
					}
				}
			}
		}

		private EntityType SelectEntityType(Block spawnBlock, Random random, bool canSpawnHostile, bool canSpawnPassive)
		{
			if (!canSpawnHostile && !canSpawnPassive) return EntityType.None;

			// Only choose from the ones that fits the location. Need to implement that filtering.
			// For now, just use all general friendly/passive mobs.

			var hostiles = new[] {EntityType.Zombie, EntityType.Skeleton, EntityType.Creeper, EntityType.Enderman};

			List<EntityType> possibleMobs = new List<EntityType>();
			if (canSpawnPassive) possibleMobs.AddRange(new[] {EntityType.Chicken, EntityType.Cow, EntityType.Pig, EntityType.Sheep});
			if (canSpawnHostile) possibleMobs.AddRange(hostiles);

			if (new byte[] {5, 19, 30, 31, 32, 33, 133, 158, 160, 161}.Contains(spawnBlock.BiomeId))
			{
				// Forest, taiga, mega taiga, and cold taiga biomes and their variants can also spawn wolves
				if (canSpawnPassive) possibleMobs.Add(EntityType.Wolf);
			}

			if (new byte[] {1, 35, 36, 163, 164}.Contains(spawnBlock.BiomeId))
			{
				// Plains and savanna biomes can also spawn horses, though savannas spawn horses only 1/5 of the time as plains.
				if (canSpawnPassive) possibleMobs.Add(EntityType.Horse);
			}

			if (new byte[] {21, 22, 23, 149, 151}.Contains(spawnBlock.BiomeId))
			{
				// Jungle biomes can also spawn ocelots, and increase the chance to spawn chickens.
				if (canSpawnPassive) possibleMobs.Add(EntityType.Ocelot);
			}

			if (spawnBlock.BiomeId == 2 || BiomeUtils.Biomes.Where(biome => biome.Temperature < 0).Count(biome => biome.Id == spawnBlock.BiomeId) == 1)
			{
				// Desert and snowy biomes (except cold taiga) do not spawn animals other than rabbits.

				// This rule is wrong. Should exempt cold taigas
				possibleMobs.Clear();
				if (canSpawnPassive) possibleMobs.Add(EntityType.Rabbit);
				if (canSpawnHostile) possibleMobs.AddRange(hostiles);
			}

			if (new byte[] {132}.Contains(spawnBlock.BiomeId))
			{
				// Desert and snowy biomes (except cold taiga) do not spawn animals other than rabbits.

				if (canSpawnPassive) possibleMobs.Add(EntityType.Rabbit);
			}

			if (new byte[] {7, 16, 25, 0, 37, 38, 39, 165, 166, 167}.Contains(spawnBlock.BiomeId))
			{
				// Beach, stone beach, river, ocean, and mesa cannot spawn animals; only hostile mobs and squid.

				possibleMobs.Clear();
				if (canSpawnHostile) possibleMobs.AddRange(hostiles);
			}

			if (possibleMobs.Count == 0) return EntityType.None;

			EntityType entityType = possibleMobs[random.Next(possibleMobs.Count)];

			return entityType;
		}

		private bool Spawn(PlayerLocation position, EntityType entityType)
		{
			Level world = Level;
			Mob mob = null;
			switch (entityType)
			{
				case EntityType.Chicken:
					mob = new Chicken(world);
					mob.NoAi = true;
					break;
				case EntityType.Cow:
					mob = new Cow(world);
					mob.NoAi = true;
					break;
				case EntityType.Pig:
					mob = new Pig(world);
					mob.NoAi = true;
					break;
				case EntityType.Sheep:
					mob = new Sheep(world);
					mob.NoAi = true;
					break;
				case EntityType.Wolf:
					mob = new Wolf(world);
					mob.NoAi = true;
					break;
				case EntityType.Horse:
					mob = new Horse(world);
					mob.NoAi = true;
					break;
				case EntityType.Ocelot:
					mob = new Ocelot(world);
					break;
				case EntityType.Rabbit:
					mob = new Rabbit(world);
					break;
				case EntityType.Spider:
					mob = new Spider(world);
					break;
				case EntityType.Zombie:
					mob = new Zombie(world);
					break;
				case EntityType.Skeleton:
					mob = new Skeleton(world);
					break;
				case EntityType.Enderman:
					mob = new Enderman(world);
					break;
				case EntityType.Creeper:
					mob = new Creeper(world);
					break;
			}

			if (mob == null) return false;

			mob.DespawnIfNotSeenPlayer = true;
			mob.KnownPosition = position;
			var bbox = mob.GetBoundingBox();
			if (!SpawnAreaClear(bbox))
			{
				return false;
			}

			ThreadPool.QueueUserWorkItem(state => mob.SpawnEntity());

			if (Log.IsDebugEnabled)
				Log.Warn($"Spawn mob {entityType}");
			return true;
		}

		private bool SpawnAreaClear(BoundingBox bbox)
		{
			BlockCoordinates min = bbox.Min;
			BlockCoordinates max = bbox.Max;
			for (int x = min.X; x < max.X; x++)
			{
				for (int y = min.Y; y < max.Y; y++)
				{
					for (int z = min.Z; z < max.Z; z++)
					{
						if (!Level.IsAir(new BlockCoordinates(x, y, z))) return false;
					}
				}
			}

			return true;
		}
	}
}