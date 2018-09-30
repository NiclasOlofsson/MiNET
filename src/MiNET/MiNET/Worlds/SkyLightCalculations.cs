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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using log4net;
using MiNET.Blocks;
using MiNET.Config;
using MiNET.Config.Contracts;
using MiNET.Utils;
using SharpAvi;
using SharpAvi.Output;

namespace MiNET.Worlds
{
	public class SkyLightBlockAccess : IBlockAccess
	{
		private readonly IWorldProvider _worldProvider;
		private readonly int _heightForUnloadedChunk;
		private readonly ChunkCoordinates _coord = ChunkCoordinates.None;
		private readonly ChunkColumn _chunk = null;

		public SkyLightBlockAccess(IWorldProvider worldProvider, int heightForUnloadedChunk = 255)
		{
			_worldProvider = worldProvider;
			_heightForUnloadedChunk = heightForUnloadedChunk;
		}

		public SkyLightBlockAccess(IWorldProvider worldProvider, ChunkColumn chunk) : this(worldProvider, -1)
		{
			_chunk = chunk;
			_coord = new ChunkCoordinates(chunk.x, chunk.z);
		}

		public ChunkColumn GetChunk(BlockCoordinates coordinates, bool cacheOnly = false)
		{
			return GetChunk((ChunkCoordinates) coordinates, cacheOnly);
		}

		public ChunkColumn GetChunk(ChunkCoordinates coordinates, bool cacheOnly = false)
		{
			if (coordinates == _coord) return _chunk;

			if (_coord != ChunkCoordinates.None)
				if (coordinates != _coord)
					if (coordinates != _coord + ChunkCoordinates.Backward)
						if (coordinates != _coord + ChunkCoordinates.Forward)
							if (coordinates != _coord + ChunkCoordinates.Left)
								if (coordinates != _coord + ChunkCoordinates.Right)
									if (coordinates != _coord + ChunkCoordinates.Backward + ChunkCoordinates.Left)
										if (coordinates != _coord + ChunkCoordinates.Backward + ChunkCoordinates.Right)
											if (coordinates != _coord + ChunkCoordinates.Forward + ChunkCoordinates.Left)
												if (coordinates != _coord + ChunkCoordinates.Forward + ChunkCoordinates.Right)
													return null;
			return _worldProvider.GenerateChunkColumn(coordinates, true);
		}

		public void SetSkyLight(BlockCoordinates coordinates, byte skyLight)
		{
			ChunkColumn chunk = GetChunk(coordinates, true);
			chunk?.SetSkyLight(coordinates.X & 0x0f, coordinates.Y & 0xff, coordinates.Z & 0x0f, skyLight);
		}

		public int GetHeight(BlockCoordinates coordinates)
		{
			ChunkColumn chunk = GetChunk(coordinates, true);
			if (chunk == null) return _heightForUnloadedChunk;

			return chunk.GetHeight(coordinates.X & 0x0f, coordinates.Z & 0x0f);
		}

		public Block GetBlock(BlockCoordinates coord, ChunkColumn tryChunk = null)
		{
			return null;
		}

		public void SetBlock(int x, int y, int z, int blockId, int metadata = 0, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true)
		{
		}

		public void SetBlock(Block block, bool broadcast = true, bool applyPhysics = true, bool calculateLight = true, ChunkColumn possibleChunk = null)
		{
		}
	}

	public class SkyLightCalculations
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (SkyLightCalculations));
		private static readonly IWorldConfiguration WorldConfig = ConfigurationProvider.MiNetConfiguration.World;

		// Debug tracking, don't enable unless you really need to "see it".

		public bool TrackResults { get; }
		public ConcurrentDictionary<BlockCoordinates, int> Visits { get; } = new ConcurrentDictionary<BlockCoordinates, int>();
		public long StartTimeInMilliseconds { get; set; }

		ConcurrentDictionary<ChunkColumn, bool> _visitedColumns = new ConcurrentDictionary<ChunkColumn, bool>();

		public long visits = 0;

		public SkyLightCalculations(bool trackResults = false)
		{
			TrackResults = trackResults;
		}

		public static void Calculate(Level level)
		{
			var chunks = level.GetLoadedChunks().OrderBy(column => column.x).ThenBy(column => column.z);
			SkyLightBlockAccess blockAccess = new SkyLightBlockAccess(level.WorldProvider);

			_chunkCount = chunks.Count();

			if (_chunkCount == 0) return;

			CheckIfSpawnIsMiddle(chunks, level.SpawnPoint.GetCoordinates3D());

			Stopwatch sw = new Stopwatch();
			sw.Start();

			//Parallel.ForEach(chunks, chunk => chunk.RecalcHeight());

			//Log.Debug($"Recalc height level {level.LevelName}({level.LevelId}) for {_chunkCount} chunks, {_chunkCount*16*16*256} blocks. Time {sw.ElapsedMilliseconds}ms");

			SkyLightCalculations calculator = new SkyLightCalculations(WorldConfig.CalculateLightsMakeMovie);

			int midX = calculator.GetMidX(chunks.ToArray());
			//int width = calculator.GetWidth(chunks.ToArray());

			sw.Restart();

			HighPrecisionTimer tickerHighPrecisionTimer = null;
			if (calculator.TrackResults) tickerHighPrecisionTimer = new HighPrecisionTimer(100, _ => calculator.SnapshotVisits());

			calculator.StartTimeInMilliseconds = Environment.TickCount;

			var t0 = Task.Run(() =>
			{
				var pairs = chunks.OrderBy(pair => pair.x).ThenBy(pair => pair.z).Where(chunk => chunk.x <= midX).OrderByDescending(pair => pair.x).ThenBy(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(blockAccess, pairs);
			});

			var t5 = Task.Run(() =>
			{
				var pairs = chunks.OrderByDescending(pair => pair.x).ThenBy(pair => pair.z).Where(chunk => chunk.x > midX).OrderBy(pair => pair.x).ThenByDescending(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(blockAccess, pairs);
			});

			var t1 = Task.Run(() =>
			{
				var pairs = chunks.OrderBy(pair => pair.x).ThenBy(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(blockAccess, pairs);
			});

			var t2 = Task.Run(() =>
			{
				var pairs = chunks.OrderByDescending(pair => pair.x).ThenByDescending(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(blockAccess, pairs);
			});

			var t3 = Task.Run(() =>
			{
				var pairs = chunks.OrderByDescending(pair => pair.x).ThenBy(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(blockAccess, pairs);
			});

			var t4 = Task.Run(() =>
			{
				var pairs = chunks.OrderBy(pair => pair.x).ThenByDescending(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(blockAccess, pairs);
			});

			Task.WaitAll(t0, t1, t2, t3, t4, t5);

			Log.Debug($"Recalc skylight for {_chunkCount:N0} chunks, {_chunkCount*16*16*256:N0} blocks. Touches={calculator.visits:N0} Time {sw.ElapsedMilliseconds:N0}ms");

			if (calculator.TrackResults)
			{
				Task.Run(() =>
				{
					tickerHighPrecisionTimer?.Dispose();
					calculator.SnapshotVisits();
					calculator.SnapshotVisits();

					if (calculator.RenderingTasks.Count == 0) return;

					// Start with an end-frame (twitter thumbs)
					var last = calculator.RenderingTasks.Last();
					calculator.RenderingTasks.Remove(last);
					calculator.RenderingTasks.Insert(0, last);

					calculator.RenderVideo();

					Log.Debug($"Movie rendered.");
				});
			}

			//foreach (var chunk in chunks)
			//{
			//	calculator.ShowHeights(chunk);
			//}

			//var chunkColumn = chunks.First(column => column.x == -1 && column.z == 0 );
			//if (chunkColumn != null)
			//{
			//	Log.Debug($"Heights:\n{Package.HexDump(chunkColumn.height)}");
			//	Log.Debug($"skylight.Data:\n{Package.HexDump(chunkColumn.skyLight.Data, 64)}");
			//}
		}

		public int CalculateSkyLights(IBlockAccess level, ChunkColumn[] chunks)
		{
			int calcCount = 0;
			Stopwatch calcTime = new Stopwatch();
			int lastCount = 0;

			foreach (var chunk in chunks)
			{
				if (!_visitedColumns.TryAdd(chunk, true)) continue;

				if (chunk.isAllAir) continue;

				calcTime.Restart();
				if (RecalcSkyLight(chunk, level))
				{
					//calcCount++;

					//var elapsedMilliseconds = calcTime.ElapsedMilliseconds;
					//var c = Visits.Sum(pair => pair.Value);
					//if (elapsedMilliseconds > 0) Log.Debug($"Recalc skylight chunk {chunk.x}, {chunk.z}, count #{calcCount} (air={chunk.isAllAir}) chunks. Time {elapsedMilliseconds}ms and {c - lastCount} visits");
					//lastCount = c;
					//PrintVisits();
				}
			}

			//Log.Debug($"Recalc skylight for #{calcCount} chunk. Made {lastCount} visits.");

			return calcCount;
		}

		public bool RecalcSkyLight(ChunkColumn chunk, IBlockAccess level)
		{
			if (chunk == null) return false;

			var lightBfQueue = new Queue<BlockCoordinates>();
			var lightBfSet = new HashSet<BlockCoordinates>();
			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					if (chunk.isAllAir && !IsOnChunkBorder(x, z))
					{
						continue;
					}

					int height = GetHigestSurrounding(x, z, chunk, level);
					if (height == 0)
					{
						continue;
					}

					//var skyLight = chunk.GetSkyLight(x, height, z);
					//if (skyLight == 15)
					{
						//Block block = level.GetBlock(new BlockCoordinates(x + (chunk.x*16), height, z + (chunk.z*16)), chunk);
						//Calculate(level, block);
						//Calculate(level, new BlockCoordinates(x + (chunk.x*16), height, z + (chunk.z*16)), lightBfQueue);
						var coordinates = new BlockCoordinates(x + (chunk.x*16), height, z + (chunk.z*16));
						lightBfQueue.Enqueue(coordinates);
						lightBfSet.Add(coordinates);
					}
					//else
					//{
					//	Log.Error($"Block with wrong light level. Expected 15 but was {skyLight}");
					//}
				}
			}

			Calculate(level, lightBfQueue, lightBfSet);

			return true;
		}

		public void Calculate(Level level, BlockCoordinates coordinates)
		{
			int currentLight = level.GetSkyLight(coordinates);

			var chunk = level.GetChunk(coordinates);
			var height = chunk.GetRecalatedHeight(coordinates.X & 0x0f, coordinates.Z & 0x0f);

			Queue<BlockCoordinates> sourceQueue = new Queue<BlockCoordinates>();
			sourceQueue.Enqueue(coordinates);
			if (currentLight != 0)
			{
				Queue<BlockCoordinates> resetQueue = new Queue<BlockCoordinates>();
				HashSet<BlockCoordinates> visits = new HashSet<BlockCoordinates>();

				// Reset all lights that potentially derive from this
				resetQueue.Enqueue(coordinates);

				Queue<BlockCoordinates> deleteQueue = new Queue<BlockCoordinates>();
				while (resetQueue.Count > 0)
				{
					var coord = resetQueue.Dequeue();
					if (visits.Contains(coord))
					{
						continue;
					}

					visits.Add(coord);

					if (coord.DistanceTo(coordinates) > 16) continue;

					ResetLight(level, resetQueue, sourceQueue, coord);
					if (!sourceQueue.Contains(coord))
					{
						deleteQueue.Enqueue(coord);
					}
				}

				level.SetSkyLight(coordinates, 0);

				foreach (var delete in deleteQueue)
				{
					level.SetSkyLight(delete, 0);
				}
			}
			else
			{
				sourceQueue.Enqueue(coordinates);
				sourceQueue.Enqueue(coordinates + BlockCoordinates.Up);
				sourceQueue.Enqueue(coordinates + BlockCoordinates.Down);
				sourceQueue.Enqueue(coordinates + BlockCoordinates.West);
				sourceQueue.Enqueue(coordinates + BlockCoordinates.East);
				sourceQueue.Enqueue(coordinates + BlockCoordinates.North);
				sourceQueue.Enqueue(coordinates + BlockCoordinates.South);
			}

			chunk.SetHeight(coordinates.X & 0x0f, coordinates.Z & 0x0f, (short) height);

			// Recalc
			Queue<BlockCoordinates> lightBfQueue = new Queue<BlockCoordinates>(sourceQueue);
			HashSet<BlockCoordinates> lightBfSet = new HashSet<BlockCoordinates>(sourceQueue);

			SkyLightBlockAccess blockAccess = new SkyLightBlockAccess(level.WorldProvider);
			Calculate(blockAccess, lightBfQueue, lightBfSet);
		}

		public void ResetLight(Level level, Queue<BlockCoordinates> resetQueue, Queue<BlockCoordinates> sourceQueue, BlockCoordinates coordinates)
		{
			int currentLight = level.GetSkyLight(coordinates);

			if (coordinates.Y < 255)
				TestForSource(level, resetQueue, sourceQueue, coordinates + BlockCoordinates.Up, currentLight);
			if (coordinates.Y > 0)
				TestForSource(level, resetQueue, sourceQueue, coordinates + BlockCoordinates.Down, currentLight, true);
			TestForSource(level, resetQueue, sourceQueue, coordinates + BlockCoordinates.West, currentLight);
			TestForSource(level, resetQueue, sourceQueue, coordinates + BlockCoordinates.East, currentLight);
			TestForSource(level, resetQueue, sourceQueue, coordinates + BlockCoordinates.North, currentLight);
			TestForSource(level, resetQueue, sourceQueue, coordinates + BlockCoordinates.South, currentLight);
		}

		private void TestForSource(Level level, Queue<BlockCoordinates> resetQueue, Queue<BlockCoordinates> sourceQueue, BlockCoordinates coordinates, int currentLight, bool down = false)
		{
			int light = level.GetSkyLight(coordinates);
			if (light == 0) return;

			if (light > currentLight || (light == 15 && !down))
			{
				if (!sourceQueue.Contains(coordinates)) sourceQueue.Enqueue(coordinates);
				return;
			}

			if (!resetQueue.Contains(coordinates)) resetQueue.Enqueue(coordinates);
		}

		public void Calculate(IBlockAccess level, Queue<BlockCoordinates> lightBfQueue, HashSet<BlockCoordinates> lightBfSet)
		{
			try
			{
				//if (block.SkyLight != 15)
				//{
				//	Log.Error($"Block at {block.Coordinates} had unexpected light level. Expected 15 but was {block.SkyLight}");
				//}

				while (lightBfQueue.Count > 0)
				{
					var coordinates = lightBfQueue.Dequeue();
					lightBfSet.Remove(coordinates);
					if (coordinates.Y < 0 || coordinates.Y > 255) continue;

					ChunkColumn chunk = level.GetChunk(coordinates);
					if(chunk == null) continue;

					var newChunkCoord = (ChunkCoordinates) coordinates;
					if (chunk.x != newChunkCoord.X || chunk.z != newChunkCoord.Z)
					{
						chunk = level.GetChunk(newChunkCoord);
						if (chunk == null) continue;
					}

					ProcessNode(level, chunk, coordinates, lightBfQueue, lightBfSet);
				}
			}
			catch (Exception e)
			{
				Log.Error("Calculation", e);
			}
		}

		private void ProcessNode(IBlockAccess level, ChunkColumn chunk, BlockCoordinates coordinates, Queue<BlockCoordinates> lightBfsQueue, HashSet<BlockCoordinates> lightBfSet)
		{
			//if (section.IsAllAir())

			byte currentSkyLight = GetSkyLight(coordinates, chunk);

			int sectionIdx = coordinates.Y >> 4;
			ChunkBase section = chunk.GetChunk(coordinates.Y);

			byte maxSkyLight = currentSkyLight;
			if (coordinates.Y < 255)
			{
				var up = coordinates + BlockCoordinates.Up;
				maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, section, sectionIdx, lightBfsQueue, lightBfSet, up, currentSkyLight, up: true));
			}

			if (coordinates.Y > 0)
			{
				var down = coordinates + BlockCoordinates.Down;
				maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, section, sectionIdx, lightBfsQueue, lightBfSet, down, currentSkyLight, down: true));
			}

			var west = coordinates + BlockCoordinates.West;
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, section, sectionIdx, lightBfsQueue, lightBfSet, west, currentSkyLight));


			var east = coordinates + BlockCoordinates.East;
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, section, sectionIdx, lightBfsQueue, lightBfSet, east, currentSkyLight));


			var south = coordinates + BlockCoordinates.South;
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, section, sectionIdx, lightBfsQueue, lightBfSet, south, currentSkyLight));

			var north = coordinates + BlockCoordinates.North;
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, chunk, section, sectionIdx, lightBfsQueue, lightBfSet, north, currentSkyLight));

			if (IsTransparent(coordinates, section) && currentSkyLight != 15)
			{
				int diffuseLevel = GetDiffuseLevel(coordinates, section);
				maxSkyLight = (byte) Math.Max(currentSkyLight, maxSkyLight - diffuseLevel);

				if (maxSkyLight > currentSkyLight)
				{
					level.SetSkyLight(coordinates, maxSkyLight);

					if (!lightBfSet.Contains(coordinates))
					{
						lightBfsQueue.Enqueue(coordinates);
						lightBfSet.Add(coordinates);
					}
				}
			}
		}

		private byte SetLightLevel(IBlockAccess level, ChunkColumn chunk, ChunkBase section, int sectionIdx, Queue<BlockCoordinates> lightBfsQueue, HashSet<BlockCoordinates> lightBfSet, BlockCoordinates coordinates, byte lightLevel, bool down = false, bool up = false)
		{
			//Interlocked.Add(ref visits, 1);

			if (TrackResults) MakeVisit(coordinates);

			if (!(up || down) && (chunk.x != coordinates.X >> 4 || chunk.z != coordinates.Z >> 4))
			{
				chunk = level.GetChunk((ChunkCoordinates) coordinates);
				section = null;
			}
			else
			{
				if ((up || down) && coordinates.Y >> 4 != sectionIdx)
				{
					section = null;
				}
			}

			if (chunk == null /* || chunk.chunks == null*/) return lightLevel;

			if (!down && !up && coordinates.Y >= GetHeight(coordinates, chunk))
			{
				if (GetSkyLight(coordinates, section) != 15)
				{
					SetSkyLight(coordinates, 15, chunk);

					if (!lightBfSet.Contains(coordinates))
					{
						lightBfsQueue.Enqueue(coordinates);
						lightBfSet.Add(coordinates);
					}
				}

				return 15;
			}

			if (section == null) section = chunk.GetChunk(coordinates.Y);

			bool isTransparent = IsTransparent(coordinates, section);
			byte skyLight = GetSkyLight(coordinates, section);

			if (down && isTransparent && lightLevel == 15)
			{
				if (IsNotBlockingSkylight(coordinates, chunk))
				{
					if (skyLight != 15)
					{
						SetSkyLight(coordinates, 15, chunk);
					}

					if (!lightBfSet.Contains(coordinates))
					{
						lightBfsQueue.Enqueue(coordinates);
						lightBfSet.Add(coordinates);
					}

					return 15;
				}
			}

			if (isTransparent)
			{
				int diffuseLevel = GetDiffuseLevel(coordinates, section);
				if (skyLight + 1 + diffuseLevel <= lightLevel)
				{
					byte newLevel = (byte) (lightLevel - diffuseLevel);
					SetSkyLight(coordinates, newLevel, chunk);

					if (!lightBfSet.Contains(coordinates))
					{
						lightBfsQueue.Enqueue(coordinates);
						lightBfSet.Add(coordinates);
					}

					return newLevel;
				}
			}

			return skyLight;
		}

		public static void SetSkyLight(BlockCoordinates coordinates, byte skyLight, ChunkColumn chunk)
		{
			chunk?.SetSkyLight(coordinates.X & 0x0f, coordinates.Y & 0xff, coordinates.Z & 0x0f, skyLight);
		}

		public static bool IsNotBlockingSkylight(BlockCoordinates blockCoordinates, ChunkColumn chunk)
		{
			if (chunk == null) return true;

			int bid = chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
			return bid == 0 || (BlockFactory.TransparentBlocks[bid] == 1 && bid != 18 && bid != 30 && bid != 8 && bid != 9);
		}

		public static int GetDiffuseLevel(BlockCoordinates blockCoordinates, ChunkBase chunk)
		{
			if (chunk == null) return 15;

			int bx = blockCoordinates.X & 0x0f;
			int by = blockCoordinates.Y & 0xff;
			int bz = blockCoordinates.Z & 0x0f;

			int bid = chunk.GetBlock(bx, by - 16*(by >> 4), bz);
			return bid == 8 || bid == 9 ? 3 : bid == 18 && bid == 30 ? 2 : 1;
		}

		public static bool IsTransparent(BlockCoordinates blockCoordinates, ChunkBase chunk)
		{
			if (chunk == null) return true;

			int bx = blockCoordinates.X & 0x0f;
			int by = blockCoordinates.Y & 0xff;
			int bz = blockCoordinates.Z & 0x0f;

			int bid = chunk.GetBlock(bx, by - 16*(by >> 4), bz);
			return bid == 0 || BlockFactory.TransparentBlocks[bid] == 1;
		}

		public static byte GetSkyLight(BlockCoordinates blockCoordinates, ChunkBase chunk)
		{
			if (chunk == null) return 15;

			int bx = blockCoordinates.X & 0x0f;
			int by = blockCoordinates.Y & 0xff;
			int bz = blockCoordinates.Z & 0x0f;

			return chunk.GetSkylight(bx, by - 16*(by >> 4), bz);
		}

		public static byte GetSkyLight(BlockCoordinates blockCoordinates, ChunkColumn chunk)
		{
			if (chunk == null) return 15;

			return chunk.GetSkylight(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0xff, blockCoordinates.Z & 0x0f);
		}

		public static int GetHeight(BlockCoordinates blockCoordinates, ChunkColumn chunk)
		{
			if (chunk == null) return 256;

			return chunk.GetHeight(blockCoordinates.X & 0x0f, blockCoordinates.Z & 0x0f);
		}

		private void MakeVisit(BlockCoordinates inc)
		{
			BlockCoordinates coordinates = new BlockCoordinates(inc.X, 0, inc.Z);
			if (Visits.ContainsKey(coordinates))
			{
				Visits[coordinates] = Visits[coordinates] + 1;
			}
			else
			{
				Visits.TryAdd(coordinates, 1);
			}
		}

		public static void CheckIfSpawnIsMiddle(IOrderedEnumerable<ChunkColumn> chunks, Vector3 spawnPoint)
		{
			int xMin = chunks.OrderBy(kvp => kvp.x).First().x;
			int xMax = chunks.OrderByDescending(kvp => kvp.x).First().x;
			int xd = Math.Abs(xMax - xMin);

			int zMin = chunks.OrderBy(kvp => kvp.z).First().z;
			int zMax = chunks.OrderByDescending(kvp => kvp.z).First().z;
			int zd = Math.Abs(zMax - zMin);

			int xm = (int) ((xd/2f) + xMin);
			int zm = (int) ((zd/2f) + zMin);

			if (xm != (int) spawnPoint.X >> 4) Log.Warn($"Wrong spawn X={xm}, {(int) spawnPoint.X >> 4}");
			if (zm != (int) spawnPoint.Z >> 4) Log.Warn($"Wrong spawn Z={zm}, {(int) spawnPoint.Z >> 4}");

			if (zm == (int) spawnPoint.Z >> 4 && xm == (int) spawnPoint.X >> 4) Log.Warn($"Spawn correct {xm}, {zm} and {(int) spawnPoint.X >> 4}, {(int) spawnPoint.Z >> 4}");
		}

		private object _imageSync = new object();
		private static int _chunkCount;

		public List<Task<Bitmap>> RenderingTasks { get; } = new List<Task<Bitmap>>();

		public void SnapshotVisits()
		{
			lock (_imageSync)
			{
				if (!TrackResults) return;

				var visits1 = Visits.ToArray();

				if (visits1.Length == 0) return;

				long time = Environment.TickCount;

				Task<Bitmap> t = new Task<Bitmap>(v =>
				{
					var fileId = time;

					try
					{
						var visits = v as KeyValuePair<BlockCoordinates, int>[];

						int valMax = Visits.OrderByDescending(kvp => kvp.Value).First().Value;
						int valMin = visits.OrderBy(kvp => kvp.Value).First().Value;

						int xMin = visits.OrderBy(kvp => kvp.Key.X).First().Key.X;
						int xMax = visits.OrderByDescending(kvp => kvp.Key.X).First().Key.X;
						int xd = Math.Abs(xMax - xMin);

						int zMin = visits.OrderBy(kvp => kvp.Key.Z).First().Key.Z;
						int zMax = visits.OrderByDescending(kvp => kvp.Key.Z).First().Key.Z;
						int zd = Math.Abs(zMax - zMin);

						int zMov = zMin < 0 ? Math.Abs(zMin) : zMin*-1;
						int xMov = xMin < 0 ? Math.Abs(xMin) : xMin*-1;

						//Bitmap bitmap = new Bitmap(xd + 1, zd + 1, PixelFormat.Format32bppArgb);
						Bitmap bitmap = new Bitmap(GetWidth(), GetHeight(), PixelFormat.Format32bppArgb);

						foreach (var visit in visits)
						{
							try
							{
								double logBase = 4;
								double min = Math.Abs(Math.Ceiling(Math.Log(1, logBase)));
								if (visit.Value == 0) continue;
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, new ColorHeatMap().GetColorForValue(visit.Value, valMax));
								bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, new ColorHeatMap().GetColorForValue(Math.Log(visit.Value, logBase) + min, Math.Log(valMax, logBase) + min));
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, CreateHeatColor(Math.Log(visit.Value, logBase) + min, Math.Log(valMax, logBase) + min));
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, CreateHeatColor(Math.Log(visit.Value + 3), Math.Log(valMax + 3)));
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, CreateHeatColor(Math.Pow(visit.Value, 10), Math.Pow(valMax, 10)));
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, CreateHeatColor(visit.Value, valMax));
							}
							catch (Exception e)
							{
								Log.Error($"{xd}, {zd}, {xMin}, {zMin}, {xMax}, {zMax}, X={visit.Key.X}, Z={visit.Key.Z}, {xMov}, {zMov}", e);
								break;
							}
						}
						//byte[] bytes = new byte[xd*zd*4];
						//int i = 0;
						//for (int x = 0; x < xd; x++)
						//{
						//    for (int z = 0; z < zd; z++)
						//    {
						//        bytes[i++*4] = image[x, z];
						//    }
						//}


						//var interval = valMax/zd;
						//for (int i = 0; i < zd; i++)
						//{
						//    //var value = (i * interval) + 3;
						//    //var max = valMax;
						//    var value = Math.Log((i*interval) + 3);
						//    var max = Math.Log(valMax);

						//    bitmap.SetPixel(0, i, CreateHeatColor((int) value, (decimal) max));
						//    bitmap.SetPixel(1, i, CreateHeatColor((int) value, (decimal) max));
						//}


						//using (Graphics g = Graphics.FromImage(bitmap))
						//{
						//    int tz = 0;
						//    for (int i = 10 - 1; i >= 0; i--)
						//    {
						//        var d = i*(valMax/10);
						//        g.DrawString($"{d}={Math.Log(d) :##.00}", new Font("Arial", 8), new SolidBrush(Color.White), 2, (tz++)*zd/10f); // requires font, brush etc
						//    }
						//}

						using (Graphics g = Graphics.FromImage(bitmap))
						{
							g.DrawString($"MiNET skylight calculation\nTime (ms): {fileId - StartTimeInMilliseconds:N0}\n{_chunkCount:N0} chunks with {(_chunkCount*16*16*256):N0} blocks\n{visits.Sum(pair => pair.Value):N0} visits", new Font("Arial", 8), new SolidBrush(Color.White), 1, 0); // requires font, brush etc
						}

						//Directory.CreateDirectory(@"D:\Temp\Light\");

						//lock (_imageSync)
						//{
						//	bitmap.Save(@"D:\Temp\Light\test-" + $"{fileId :00000}.bmp", ImageFormat.Bmp);
						//}

						//bitmap.Dispose();
						//GC.Collect();

						//foreach (var visit in visits)
						//{
						//	Log.Debug($"Visit {visit.Key} {visit.Value} times");
						//}

						Log.Debug($"Made a total of {visits.Sum(pair => pair.Value):N0} visits");
						return bitmap;
					}
					catch (Exception e)
					{
						Log.Error("Rendering", e);
					}

					return null;
				}, visits1);
				RenderingTasks.Add(t);
			}
		}

		private int GetMidX(ChunkColumn[] chunks)
		{
			if (!TrackResults) return 0;

			var visits = chunks.ToArray();

			int xMin = visits.OrderBy(kvp => kvp.x).First().x;
			int xMax = visits.OrderByDescending(kvp => kvp.x).First().x;
			int xd = Math.Abs(xMax - xMin);

			return xMin + xd/2;
		}

		private int GetWidth(ChunkColumn[] chunks)
		{
			if (!TrackResults) return 0;

			var visits = chunks.ToArray();

			int xMin = visits.OrderBy(kvp => kvp.x).First().x;
			int xMax = visits.OrderByDescending(kvp => kvp.x).First().x;
			int xd = Math.Abs(xMax - xMin);

			return xd;
		}

		private int GetWidth()
		{
			if (!TrackResults) return 0;

			var visits = Visits.ToArray();

			int xMin = visits.OrderBy(kvp => kvp.Key.X).First().Key.X;
			int xMax = visits.OrderByDescending(kvp => kvp.Key.X).First().Key.X;
			int xd = Math.Abs(xMax - xMin);

			return xd + 1;
		}

		private int GetHeight()
		{
			if (!TrackResults) return 0;

			var visits = Visits.ToArray();

			int zMin = visits.OrderBy(kvp => kvp.Key.Z).First().Key.Z;
			int zMax = visits.OrderByDescending(kvp => kvp.Key.Z).First().Key.Z;
			int zd = Math.Abs(zMax - zMin);

			return zd + 1;
		}

		private void RenderVideo()
		{
			try
			{
				if (!TrackResults) return;


				var moviePath = @"D:\Temp\Light\test.avi";
				Log.Debug($"Generated all images, now rendering movie to {moviePath}");

				//var files = Directory.EnumerateFiles(@"D:\Temp\Light\", "*.bmp");
				//files = files.OrderBy(s => s);

				//int fps = (int) (RenderingTasks.Count()/10f); // Movie should last 5 seconds
				int fps = 10;

				var writer = new AviWriter(moviePath)
				{
					FramesPerSecond = fps,
					// Emitting AVI v1 index in addition to OpenDML index (AVI v2)
					// improves compatibility with some software, including 
					// standard Windows programs like Media Player and File Explorer
					EmitIndex1 = true
				};

				var stream = writer.AddVideoStream();
				stream.Width = GetWidth();
				stream.Height = GetHeight();
				stream.Codec = KnownFourCCs.Codecs.Uncompressed;

				stream.BitsPerPixel = BitsPerPixel.Bpp32;

				Log.Debug($"Waiting for image rendering of {RenderingTasks.Count} images to complete");
				foreach (var renderingTask in RenderingTasks)
				{
					renderingTask.RunSynchronously();
					Bitmap image = renderingTask.Result;
					//}

					//foreach (var file in files)
					//{
					lock (_imageSync)
					{
						//Bitmap image = (Bitmap) Image.FromFile(file);
						//image = new Bitmap(image, stream.Width, stream.Height);

						byte[] imageData = (byte[]) ToByteArray(image, ImageFormat.Bmp);

						if (imageData == null)
						{
							Log.Warn($"No image data for file.");
							continue;
						}

						if (imageData.Length != stream.Height*stream.Width*4)
						{
							imageData = imageData.Skip(imageData.Length - (stream.Height*stream.Width*4)).ToArray();
						}

						// fill frameData with image

						// write data to a frame
						stream.WriteFrame(true, // is key frame? (many codecs use concept of key frames, for others - all frames are keys)
							imageData, // array with frame data
							0, // starting index in the array
							imageData.Length // length of the data
						);
					}
				}

				writer.Close();
			}
			catch (Exception e)
			{
				Log.Error("Rendering movie", e);
			}
		}

		public static byte[] ToByteArray(Image image, ImageFormat format)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, format);
				return ms.ToArray();
			}
		}

		static Color CreateHeatColor(double value, double max)
		{
			//if (value < 0) value = 0;

			//Log.Debug($"Before Value={value}, min={min}, max={max}");

			double pct = value/max;
			if (pct < 0) pct = 0;

			//Log.Debug($"Value={v :F2}, max={m:F2}, pct={pct:F2}");

			return Color.FromArgb(255, (byte) (255.0f*pct), (byte) (255.0f*(1 - pct)), 0);

			//int A = 255;
			//int R;
			//int G;
			//int B;
			//if (pct < 0.34d)
			//{
			//	R = 255;
			//	G = (byte) (255*Math.Min(3*(pct - 0.333333d), 1d));
			//	B = 0;
			//}
			//else if (pct < 0.67d)
			//{
			//	R = (byte) (255*Math.Min(3*(1d - pct), 1d));
			//	G = 255;
			//	B = 0;
			//}
			//else
			//{
			//	R = (byte) (128 + (127*Math.Min(3*pct, 1d)));
			//	G = 0;
			//	B = 0;
			//}

			//return Color.FromArgb(A, R, G, B);
		}

		private static bool IsOnChunkBorder(int x, int z)
		{
			return !(x > 0 && x < 15 && z > 0 && z < 15);
		}

		private static int GetHigestSurrounding(int x, int z, ChunkColumn chunk, IBlockAccess level)
		{
			int h = chunk.GetHeight(x, z);
			if (h == 255) return h;

			if (x == 0 || x == 15 || z == 0 || z == 15)
			{
				var coords = new BlockCoordinates(x + (chunk.x*16), h, z + (chunk.z*16));

				//h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.Up));
				h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.West));
				h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.East));
				h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.North));
				h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.South));
				if (h > 255) h = 255;
				if (h < 0) h = 0;
				return h;
			}

			//if (z < 15) h = Math.Max(h, chunk.GetHeight(x, z + 1));
			//if (z > 0) h = Math.Max(h, chunk.GetHeight(x, z - 1));
			//if (x < 15) h = Math.Max(h, chunk.GetHeight(x + 1, z));
			//if (x < 15 && z > 0) h = Math.Max(h, chunk.GetHeight(x + 1, z - 1));
			//if (x < 15 && z < 15) h = Math.Max(h, chunk.GetHeight(x + 1, z + 1));
			//if (x > 0) h = Math.Max(h, chunk.GetHeight(x - 1, z));
			//if (x > 0 && z > 0) h = Math.Max(h, chunk.GetHeight(x - 1, z - 1));
			//if (x > 0 && z < 15) h = Math.Max(h, chunk.GetHeight(x - 1, z + 1));

			h = Math.Max(h, chunk.GetHeight(x, z + 1));
			h = Math.Max(h, chunk.GetHeight(x, z - 1));
			h = Math.Max(h, chunk.GetHeight(x + 1, z));
			//h = Math.Max(h, chunk.GetHeight(x + 1, z - 1));
			//h = Math.Max(h, chunk.GetHeight(x + 1, z + 1));
			h = Math.Max(h, chunk.GetHeight(x - 1, z));
			//h = Math.Max(h, chunk.GetHeight(x - 1, z - 1));
			//h = Math.Max(h, chunk.GetHeight(x - 1, z + 1));

			return h;
		}

		public void ShowHeights(ChunkColumn chunk)
		{
			if (chunk == null) return;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					var y = chunk.GetHeight(x, z);
					chunk.SetBlock(x, y, z, 41);
					//for (byte y = 255; y > 0; y--)
					//{
					//	if (chunk.GetSkylight(x, y, z) == 0)
					//	{
					//		chunk.SetBlock(x, y, z, 41);
					//		break;
					//	}
					//}
				}
			}
		}
	}

	public class ColorHeatMap
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ColorHeatMap));

		public ColorHeatMap()
		{
			InitColorsBlocks();
		}

		public ColorHeatMap(byte alpha)
		{
			this.Alpha = alpha;
			InitColorsBlocks();
		}

		private void InitColorsBlocks()
		{
			ColorsOfMap.AddRange(new Color[]
			{
				Color.FromArgb(Alpha, 0, 0, 0), //Black
				Color.FromArgb(Alpha, 0, 0, 0xFF), //Blue
				Color.FromArgb(Alpha, 0, 0xFF, 0xFF), //Cyan
				Color.FromArgb(Alpha, 0, 0xFF, 0), //Green
				Color.FromArgb(Alpha, 0xFF, 0xFF, 0), //Yellow
				Color.FromArgb(Alpha, 0xFF, 0, 0), //Red
				Color.FromArgb(Alpha, 0xFF, 0xFF, 0xFF) // White
			});
		}

		public Color GetColorForValue(double val, double maxVal)
		{
			double valPerc = val/maxVal; // value%
			if (valPerc < 0) valPerc = 0.1;
			if (valPerc > 1.0) valPerc = 1;
			double colorPerc = 1d/(ColorsOfMap.Count - 2); // % of each block of color. the last is the "100% Color"
			double blockOfColor = valPerc/colorPerc; // the integer part repersents how many block to skip
			int blockIdx = (int) Math.Truncate(blockOfColor); // Idx of 
			double valPercResidual = valPerc - (blockIdx*colorPerc); //remove the part represented of block 
			double percOfColor = valPercResidual/colorPerc; // % of color of this block that will be filled

			Color cTarget = ColorsOfMap[blockIdx];
			Color cNext = ColorsOfMap[blockIdx + 1];

			var deltaR = cNext.R - cTarget.R;
			var deltaG = cNext.G - cTarget.G;
			var deltaB = cNext.B - cTarget.B;

			var R = cTarget.R + (deltaR*percOfColor);
			var G = cTarget.G + (deltaG*percOfColor);
			var B = cTarget.B + (deltaB*percOfColor);

			Color c = ColorsOfMap[0];
			try
			{
				c = Color.FromArgb(Alpha, (byte) R, (byte) G, (byte) B);
			}
			catch (Exception ex)
			{
			}
			return c;
		}

		public byte Alpha = 0xff;
		public List<Color> ColorsOfMap = new List<Color>();
	}
}