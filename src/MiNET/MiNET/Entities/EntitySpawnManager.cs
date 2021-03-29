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
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class EntitySpawnManager
	{
		public class SpawnState : IEqualityComparer<SpawnState>
		{
			public int ChunkX { get; set; }
			public int ChunkZ { get; set; }
			public int Seed { get; set; }

			public SpawnState(int chunkX, int chunkZ, int seed)
			{
				ChunkX = chunkX;
				ChunkZ = chunkZ;
				Seed = seed;
			}

			private sealed class ChunkXChunkZEqualityComparer : IEqualityComparer<SpawnState>
			{
				public bool Equals(SpawnState x, SpawnState y)
				{
					if (ReferenceEquals(x, y)) return true;
					if (ReferenceEquals(x, null)) return false;
					if (ReferenceEquals(y, null)) return false;
					if (x.GetType() != y.GetType()) return false;
					return x.ChunkX == y.ChunkX && x.ChunkZ == y.ChunkZ;
				}

				public int GetHashCode(SpawnState obj)
				{
					unchecked
					{
						return (obj.ChunkX * 397) ^ obj.ChunkZ;
					}
				}
			}

			public static IEqualityComparer<SpawnState> ChunkXChunkZComparer { get; } = new ChunkXChunkZEqualityComparer();

			public bool Equals(SpawnState x, SpawnState y)
			{
				return ChunkXChunkZComparer.Equals(x, y);
			}

			public int GetHashCode(SpawnState obj)
			{
				return ChunkXChunkZComparer.GetHashCode(obj);
			}
		}

		private static readonly ILog Log = LogManager.GetLogger(typeof(EntitySpawnManager));

		public const int CapHostile = 70;
		public const int CapPassive = 70;
		public const int CapAmbient = 15;
		public const int CapWater = 5;

		public static readonly EntityType[] PassiveMobs =
		{
			EntityType.Chicken,
			EntityType.Cow,
			EntityType.Pig,
			EntityType.Sheep,
			EntityType.Wolf,
			EntityType.Rabbit,
			EntityType.Horse
		};

		public static readonly EntityType[] HostileMobs =
		{
			EntityType.Spider,
			EntityType.Zombie,
			EntityType.Skeleton,
			EntityType.Creeper,
			EntityType.Enderman
		};

		public Level Level { get; set; }

		public EntitySpawnManager(Level level)
		{
			Level = level;
		}

		public virtual void DespawnMobs(long tickTime)
		{
			//if (tickTime%400 != 0) return;
			if (tickTime % 20 != 0) return;

			foreach (var entity in Level.Entities)
			{
				if (!entity.Value.CanDespawn) continue;

				if (Level.Players.Count(player => player.Value.IsSpawned && Vector3.Distance(entity.Value.KnownPosition, player.Value.KnownPosition) < 128) == 0)
				{
					if (Log.IsDebugEnabled)
						Log.Debug($"Despawned entity because no players within 128 blocks distance");

					entity.Value.DespawnEntity();
				}
			}
		}

		public virtual void AttemptMobSpawn(BlockCoordinates packCoord, Random random, bool canSpawnPassive, bool canSpawnHostile)
		{
			if (Level.Dimension != Dimension.Overworld) return;

			if (Level.Players.Count(player => player.Value.IsSpawned && Vector3.Distance(packCoord, player.Value.KnownPosition) < 24) != 0)
			{
				//if (Log.IsDebugEnabled)
				//	Log.Debug($"Can't spawn entity because players within 24 blocks distance: {blockCoordinates}");
				return;
			}

			if (!canSpawnHostile && !canSpawnPassive)
			{
				// Mob cap reached on all creatures
				return;
			}

			for (int c = 0; c < 2; c++)
			{
				int maxPackSize = int.MaxValue;
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

				for (int j = 0; j < 3; j++)
				{
					int x = packCoord.X;
					int y = packCoord.Y;
					int z = packCoord.Z;

					// The next 3 (k, x and z) random construct heavily weigh the coordinates toward center of pack-origin
					int k = random.Next(1, 5);
					for (int i = 0; i < k && numberOfSpawnedMobs < maxPackSize; i++)
					{
						x += random.Next(6) - random.Next(6);
						z += random.Next(6) - random.Next(6);

						var spawnBlock = Level.GetBlock(x, y - 1, z);
						//FIXME: The following is wrong. It shouldn't be the same for all mobs and need to be moved into some sort of
						// entity-based "CanSpawn()" method. But performance need to be handled too, and this is way faster right now.
						if (spawnBlock is Grass || spawnBlock is Sand || spawnBlock is Gravel || (doSpawnHostile && spawnBlock.IsSolid && !spawnBlock.IsTransparent))
						{
							if (entityType == EntityType.None)
							{
								entityType = SelectEntityType(spawnBlock, random, doSpawnHostile, doSpawnPassive);
								if (entityType == EntityType.None)
								{
									if (Log.IsDebugEnabled && doSpawnHostile)
										Log.Debug($"Failed to spawn because found no proper entity types for biome {BiomeUtils.GetBiome(spawnBlock.BiomeId).Name}");
									break;
								}

								maxPackSize = entityType == EntityType.Wolf ? 8 : entityType == EntityType.Rabbit ? 2 + random.Next(1) : entityType == EntityType.Horse ? 2 + random.Next(5) : entityType == EntityType.Enderman ? 1 + random.Next(4) : 4;
							}

							var firstBlock = Level.GetBlock(x, y, z);
							if (!firstBlock.IsSolid && !(firstBlock is Stationary) && !(firstBlock is Flowing))
							{
								if (doSpawnPassive && PassiveMobs.Contains(entityType)
													&& Level.GetSubtractedLight(firstBlock.Coordinates, 0) >= 9)
								{
									var secondBlock = Level.GetBlock(x, y + 1, z);
									if ((spawnBlock is Grass || (entityType == EntityType.Rabbit && spawnBlock is Sand)) && !secondBlock.IsSolid)
									{
										var yaw = random.Next(360);

										if (Spawn(new PlayerLocation(x + 0.5, y, z + 0.5, yaw + 15, yaw), entityType, random))
										{
											if (Log.IsDebugEnabled)
												Log.Debug($"Spawned {entityType}");
											//Level.StrikeLightning(new PlayerLocation(x + 0.5, y, z + 0.5, yaw + 15, yaw)); 
											//Level.SetBlock(new StainedGlass() { Metadata = (byte)firstBlock.SkyLight, Coordinates = firstBlock.Coordinates + BlockCoordinates.Down });
											++numberOfSpawnedMobs;
										}
										else
										{
											if (Log.IsDebugEnabled)
												Log.Debug($"Failed to spawn {entityType} because area not clear");
										}
									}
								}
								else if (doSpawnHostile && HostileMobs.Contains(entityType) && Level.GetSubtractedLight(firstBlock.Coordinates) < random.Next(8))
								{
									var secondBlock = Level.GetBlock(x, y + 1, z);
									if (!secondBlock.IsSolid)
									{
										var yaw = random.Next(360);

										if (Spawn(new PlayerLocation(x + 0.5, y, z + 0.5, yaw + 15, yaw), entityType, random))
										{
											if (Log.IsDebugEnabled)
												Log.Debug($"Spawned {entityType} at lightlevel={Level.GetSubtractedLight(firstBlock.Coordinates)}");
											//Level.SetBlock(new StainedGlass() { Metadata = (byte) firstBlock.SkyLight, Coordinates = firstBlock.Coordinates + BlockCoordinates.Down });
											//Log.Warn($"Spawned {entityType} at {firstBlock.Coordinates} at light level on bottom={firstBlock.SkyLight} amd top={secondBlock.SkyLight}, world time={Level.CurrentWorldTime}");

											++numberOfSpawnedMobs;
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
		}

		private EntityType SelectEntityType(Block spawnBlock, Random random, bool canSpawnHostile, bool canSpawnPassive)
		{
			if (!canSpawnHostile && !canSpawnPassive) return EntityType.None;

			// Only choose from the ones that fits the location. Need to implement that filtering.
			// For now, just use all general friendly/passive mobs.

			var hostiles = new[] {EntityType.Spider, EntityType.Zombie, EntityType.Skeleton, EntityType.Creeper, EntityType.Enderman};

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

			EntityType entityType = GetWeightedRandom(possibleMobs.ToArray(), random);

			return entityType;
		}

		private EntityType GetWeightedRandom(EntityType[] possiblEntityTypes, Random random)
		{
			Tuple<EntityType, int>[] weightedPassiveMobs =
			{
				new Tuple<EntityType, int>(EntityType.Sheep, 12),
				new Tuple<EntityType, int>(EntityType.Chicken, 10),
				new Tuple<EntityType, int>(EntityType.Pig, 10),
				new Tuple<EntityType, int>(EntityType.Cow, 8),
				new Tuple<EntityType, int>(EntityType.Wolf, 5),
				new Tuple<EntityType, int>(EntityType.Rabbit, 2),
				new Tuple<EntityType, int>(EntityType.Horse, 1)
			};

			Tuple<EntityType, int>[] weightedHostileMobs =
			{
				new Tuple<EntityType, int>(EntityType.Spider, 100),
				new Tuple<EntityType, int>(EntityType.Zombie, 100),
				new Tuple<EntityType, int>(EntityType.Skeleton, 100),
				new Tuple<EntityType, int>(EntityType.Creeper, 100),
				new Tuple<EntityType, int>(EntityType.Enderman, 10)
			};

			List<Tuple<EntityType, int>> coll = new List<Tuple<EntityType, int>>();
			int totalWeight = 0;
			foreach (var possiblEntityType in possiblEntityTypes)
			{
				if (PassiveMobs.Contains(possiblEntityType))
				{
					var tuple = weightedPassiveMobs.First(m => m.Item1 == possiblEntityType);
					totalWeight += tuple.Item2;
					coll.Add(tuple);
				}
				else if (HostileMobs.Contains(possiblEntityType))
				{
					var tuple = weightedHostileMobs.First(m => m.Item1 == possiblEntityType);
					totalWeight += tuple.Item2;
					coll.Add(tuple);
				}
			}

			coll = coll.OrderByDescending(t => t.Item2).ToList();

			int weight = random.Next(totalWeight);

			int i = 0;

			for (int j = coll.Count; i < j; ++i)
			{
				var t = coll[i];
				weight -= t.Item2;

				if (weight < 0)
				{
					return t.Item1;
				}
			}

			//Log.Warn($"Looking for random entit types, found none");
			return EntityType.None;
		}

		private bool Spawn(PlayerLocation position, EntityType entityType, Random random)
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
					mob = new Sheep(world, random);
					mob.NoAi = true;
					break;
				case EntityType.Wolf:
					mob = new Wolf(world);
					mob.NoAi = true;
					break;
				case EntityType.Horse:
					mob = new Horse(world, random.NextDouble() < 0.10, random);
					mob.IsBaby = random.NextDouble() < 0.20;
					mob.NoAi = true;
					break;
				case EntityType.Ocelot:
					mob = new Ocelot(world);
					mob.NoAi = true;
					break;
				case EntityType.Rabbit:
					mob = new Rabbit(world);
					mob.NoAi = true;
					break;
				case EntityType.Spider:
					mob = new Spider(world);
					mob.NoAi = true;
					break;
				case EntityType.Zombie:
					mob = new Zombie(world);
					mob.IsBaby = random.NextDouble() < 0.05;
					mob.NoAi = true;
					break;
				case EntityType.Skeleton:
					mob = new Skeleton(world);
					mob.NoAi = true;
					break;
				case EntityType.Enderman:
					mob = new Enderman(world);
					mob.NoAi = true;
					break;
				case EntityType.Creeper:
					mob = new Creeper(world);
					mob.NoAi = true;
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
				Log.Debug($"Spawn mob {entityType}");
			return true;
		}

		private bool SpawnAreaClear(BoundingBox bbox)
		{
			BlockCoordinates min = bbox.Min;
			BlockCoordinates max = bbox.Max;
			for (int x = min.X; x <= max.X; x++)
			{
				for (int y = min.Y; y <= max.Y; y++)
				{
					for (int z = min.Z; z <= max.Z; z++)
					{
						// Check this again. Might be that we want to check solids instead?
						if (!Level.IsAir(new BlockCoordinates(x, y, z))) return false;
					}
				}
			}

			return true;
		}
	}
}