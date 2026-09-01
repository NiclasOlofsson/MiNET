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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;
using fNbt;
using MiNET.BlockEntities.Upgrade;
using MiNET.Utils.IO;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Benchmarks
{
	/// <summary>
	///     Times each phase inside LevelDbProvider.GetChunk by running a line-for-line copy of it
	///     with a stopwatch per phase. The real GetChunk runs over the same columns in the same
	///     process, so the phase sum is validated against the real total instead of subtracted
	///     from it: if the copy's own wall time does not match the real call, the copy has drifted
	///     and the numbers are void.
	/// </summary>
	public static class ChunkPhaseMeasurement
	{
		private const string World = @"C:\Development\github\MiNET\temp_auto\bench-world";
		private const int Columns = 32;
		private const int WarmupPasses = 200;
		private const int MeasuredPasses = 2000;

		private const byte KeyHeightAndBiomes3D = 0x2b;
		private const byte KeyVersion = 0x2c;
		private const byte KeyHeightAndBiomes2D = 0x2d;
		private const byte KeySubChunk = 0x2f;
		private const byte KeyBlockEntity = 0x31;
		private const byte KeyVersionLegacy = 0x76;
		private const int SubChunkIndexOffset = 4;

		private static LevelDbProvider _provider;
		private static readonly List<ChunkCoordinates> _coordinates = new();
		private static long _sectionsCreated;

		// The two private statics the copy needs, bound once so the call itself is a delegate call.
		private static Action<ChunkColumn, byte[]> _parseBiomes3D;
		private static Action<ChunkColumn, ChunkCoordinates, NbtCompound, int, int, int> _inspectBlockEntity;

		private static readonly string[] PhaseNames =
		{
			"keys",
			"version read",
			"column new",
			"section read",
			"section absent",
			"section create",
			"section parse",
			"biome read",
			"biome parse",
			"blockentity read",
			"blockentity parse",
			"lights gate"
		};

		public static void Run()
		{
			Setup();

			// Warm both paths: JIT, database block cache, pools.
			for (int i = 0; i < WarmupPasses; i++)
			{
				RealPass();
				ReplicaPass(new Stopwatch[PhaseNames.Length]);
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();

			// Per-pass samples, so medians and percentiles exist instead of one averaged total.
			// Every measured pass runs inside a no-GC region: no collection can land inside the
			// timers, and the collection debt is paid between passes, untimed.
			int bustedReal = 0, bustedReplica = 0;
			var realSamples = new double[MeasuredPasses];
			var realSw = new Stopwatch();
			for (int i = 0; i < MeasuredPasses; i++)
			{
				StartNoGCPass();
				realSw.Restart();
				RealPass();
				realSw.Stop();
				if (!EndNoGCPass()) bustedReal++;
				realSamples[i] = realSw.Elapsed.TotalMilliseconds;
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();

			_sectionsCreated = 0;
			var phases = new Stopwatch[PhaseNames.Length];
			for (int i = 0; i < phases.Length; i++) phases[i] = new Stopwatch();
			var phaseSamples = new double[PhaseNames.Length][];
			for (int i = 0; i < phases.Length; i++) phaseSamples[i] = new double[MeasuredPasses];
			var wallSamples = new double[MeasuredPasses];
			var pauseSamples = new double[MeasuredPasses];
			var wallSw = new Stopwatch();
			int gen0Before = GC.CollectionCount(0), gen1Before = GC.CollectionCount(1), gen2Before = GC.CollectionCount(2);
			for (int i = 0; i < MeasuredPasses; i++)
			{
				var ticksBefore = new long[phases.Length];
				for (int p = 0; p < phases.Length; p++) ticksBefore[p] = phases[p].ElapsedTicks;
				TimeSpan passPauseBefore = GC.GetTotalPauseDuration();

				StartNoGCPass();
				wallSw.Restart();
				ReplicaPass(phases);
				wallSw.Stop();
				if (!EndNoGCPass()) bustedReplica++;

				wallSamples[i] = wallSw.Elapsed.TotalMilliseconds;
				pauseSamples[i] = (GC.GetTotalPauseDuration() - passPauseBefore).TotalMilliseconds;
				for (int p = 0; p < phases.Length; p++) phaseSamples[p][i] = TimeSpan.FromTicks(phases[p].ElapsedTicks - ticksBefore[p]).TotalMilliseconds;
			}
			int gen0 = GC.CollectionCount(0) - gen0Before, gen1 = GC.CollectionCount(1) - gen1Before, gen2 = GC.CollectionCount(2) - gen2Before;

			double medianSum = 0;
			for (int p = 0; p < phases.Length; p++) medianSum += Percentile(phaseSamples[p], 50);

			Console.WriteLine($"world: {World}, columns per pass: {_coordinates.Count}, warmup passes: {WarmupPasses}, measured passes: {MeasuredPasses}, sections per pass: {_sectionsCreated / MeasuredPasses}");
			Console.WriteLine();
			Console.WriteLine($"{"",-18} {"median",10} {"p10",10} {"p90",10}  ms/pass");
			Console.WriteLine($"{"real GetChunk",-18} {Percentile(realSamples, 50),10:F3} {Percentile(realSamples, 10),10:F3} {Percentile(realSamples, 90),10:F3}");
			Console.WriteLine($"{"replica wall",-18} {Percentile(wallSamples, 50),10:F3} {Percentile(wallSamples, 10),10:F3} {Percentile(wallSamples, 90),10:F3}");
			Console.WriteLine($"{"GC pause",-18} {Percentile(pauseSamples, 50),10:F3} {Percentile(pauseSamples, 10),10:F3} {Percentile(pauseSamples, 90),10:F3}");
			Console.WriteLine($"sum of phase medians: {medianSum:F3} ms; busted no-GC passes: real {bustedReal}, replica {bustedReplica}");
			Console.WriteLine();
			Console.WriteLine($"{"phase",-18} {"median",10} {"p10",10} {"p90",10} {"% of sum",9}");
			for (int i = 0; i < phases.Length; i++)
			{
				double median = Percentile(phaseSamples[i], 50);
				Console.WriteLine($"{PhaseNames[i],-18} {median,10:F3} {Percentile(phaseSamples[i], 10),10:F3} {Percentile(phaseSamples[i], 90),10:F3} {median / medianSum * 100,8:F1}%");
			}

			_provider.Db.Close();
		}

		/// <summary>
		///     The constructor's own work with the GC removed from the loop entirely: a no-GC
		///     region big enough for every allocation the loop makes, so no collection can run.
		/// </summary>
		public static void RunCtorSplit()
		{
			const int iterations = 10_000;
			const int repetitions = 15;

			Measure("new SubChunk() (rent, as-is)", iterations, repetitions, count =>
			{
				for (int i = 0; i < count; i++) new SubChunk();
			});

			Measure("four new arrays + skylight fill", iterations, repetitions, count =>
			{
				for (int i = 0; i < count; i++)
				{
					var blocks = new short[4096];
					var loggedBlocks = new byte[4096];
					var blocklight = new byte[2048];
					var skylight = new byte[2048];
					skylight.AsSpan().Fill(0xff);
					var runtimeIds = new List<int> {0};
					_sink = blocks[i & 4095] + loggedBlocks[i & 4095] + blocklight[i & 2047] + skylight[i & 2047] + runtimeIds[0];
				}
			});

			Measure("four uninitialized + skylight fill", iterations, repetitions, count =>
			{
				for (int i = 0; i < count; i++)
				{
					var blocks = GC.AllocateUninitializedArray<short>(4096);
					var loggedBlocks = GC.AllocateUninitializedArray<byte>(4096);
					var blocklight = GC.AllocateUninitializedArray<byte>(2048);
					var skylight = GC.AllocateUninitializedArray<byte>(2048);
					skylight.AsSpan().Fill(0xff);
					var runtimeIds = new List<int> {0};
					_sink = blocks[i & 4095] + loggedBlocks[i & 4095] + blocklight[i & 2047] + skylight[i & 2047] + runtimeIds[0];
				}
			});

			Measure("uninitialized + fill + explicit clears", iterations, repetitions, count =>
			{
				for (int i = 0; i < count; i++)
				{
					var blocks = GC.AllocateUninitializedArray<short>(4096);
					var loggedBlocks = GC.AllocateUninitializedArray<byte>(4096);
					var blocklight = GC.AllocateUninitializedArray<byte>(2048);
					var skylight = GC.AllocateUninitializedArray<byte>(2048);
					blocks.AsSpan().Clear();
					loggedBlocks.AsSpan().Clear();
					blocklight.AsSpan().Clear();
					skylight.AsSpan().Fill(0xff);
					var runtimeIds = new List<int> {0};
					_sink = blocks[i & 4095] + loggedBlocks[i & 4095] + blocklight[i & 2047] + skylight[i & 2047] + runtimeIds[0];
				}
			});
		}

		/// <summary>Runs the loop once as warmup, then <paramref name="repetitions" /> times inside
		/// per-repetition no-GC regions, and reports the median with p10/p90.</summary>
		private static void Measure(string name, int iterations, int repetitions, Action<int> loop)
		{
			loop(iterations);

			var samples = new double[repetitions];
			var sw = new Stopwatch();
			for (int rep = 0; rep < repetitions; rep++)
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				GC.Collect();

				// ~17KB of arrays plus object and List per iteration, with headroom.
				if (!GC.TryStartNoGCRegion(iterations * 24_000L)) throw new InvalidOperationException("no-GC region refused");
				sw.Restart();
				loop(iterations);
				sw.Stop();
				GC.EndNoGCRegion();

				samples[rep] = sw.Elapsed.TotalNanoseconds / iterations;
			}

			Console.WriteLine($"{name,-42} median {Percentile(samples, 50),6:F0} ns/op  p10 {Percentile(samples, 10),6:F0}  p90 {Percentile(samples, 90),6:F0}");
		}

		private static int _sink;

		/// <summary>One pass allocates ~18MB; 64MB of budget keeps the region alive with headroom.</summary>
		private static void StartNoGCPass()
		{
			if (!GC.TryStartNoGCRegion(64L * 1024 * 1024)) throw new InvalidOperationException("no-GC region refused");
		}

		/// <summary>Ends the pass's region and pays the collection debt, untimed. False when the
		/// budget ran out mid-pass, meaning a collection got inside the timers after all.</summary>
		private static bool EndNoGCPass()
		{
			bool held = GCSettings.LatencyMode == GCLatencyMode.NoGCRegion;
			if (held) GC.EndNoGCRegion();
			GC.Collect();
			return held;
		}

		private static double Percentile(double[] samples, int percentile)
		{
			double[] sorted = (double[]) samples.Clone();
			Array.Sort(sorted);
			int index = (int) Math.Round((sorted.Length - 1) * percentile / 100.0);
			return sorted[index];
		}

		/// <summary>
		///     Splits the section-create phase: the full constructor, the constructor without the
		///     buffer clears, and the constructor when every object is returned to the pool again,
		///     which is the state where ArrayPool actually has buffers to hand back.
		/// </summary>
		private static void MeasureSubChunkCreation()
		{
			const int iterations = 20_000;

			// Warmup.
			for (int i = 0; i < 1_000; i++)
			{
				new SubChunk();
				new SubChunk(false);
				new SubChunk().PutPool();
			}

			GC.Collect();
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++) new SubChunk();
			sw.Stop();
			double fullNs = sw.Elapsed.TotalNanoseconds / iterations;

			GC.Collect();
			sw.Restart();
			for (int i = 0; i < iterations; i++) new SubChunk(false);
			sw.Stop();
			double noClearNs = sw.Elapsed.TotalNanoseconds / iterations;

			GC.Collect();
			sw.Restart();
			for (int i = 0; i < iterations; i++) new SubChunk().PutPool();
			sw.Stop();
			double returnedNs = sw.Elapsed.TotalNanoseconds / iterations;

			// The clears alone, on one instance, so no allocation or GC sits in the loop.
			var subject = new SubChunk();
			for (int i = 0; i < 1_000; i++) subject.ClearBuffers();
			sw.Restart();
			for (int i = 0; i < iterations; i++) subject.ClearBuffers();
			sw.Stop();
			double clearNs = sw.Elapsed.TotalNanoseconds / iterations;

			Console.WriteLine();
			Console.WriteLine($"new SubChunk(), never returned:      {fullNs,8:F0} ns/op");
			Console.WriteLine($"new SubChunk(false), never returned: {noClearNs,8:F0} ns/op  (rent without clears)");
			Console.WriteLine($"new SubChunk() + PutPool:            {returnedNs,8:F0} ns/op  (pool has buffers to reuse)");
			Console.WriteLine($"ClearBuffers alone, one instance:    {clearNs,8:F0} ns/op");
		}

		private static void Setup()
		{
			_provider = new LevelDbProvider(World);
			_provider.Initialize();

			for (int ring = 0; ring < 64 && _coordinates.Count < Columns; ring++)
			{
				for (int x = -ring; x <= ring && _coordinates.Count < Columns; x++)
				{
					for (int z = -ring; z <= ring && _coordinates.Count < Columns; z++)
					{
						if (Math.Max(Math.Abs(x), Math.Abs(z)) != ring) continue;
						if (_provider.Db.Get(Combine(Index(x, z), KeyVersion)) == null) continue;

						_coordinates.Add(new ChunkCoordinates(x, z));
					}
				}
			}

			if (_coordinates.Count < Columns) throw new InvalidOperationException($"{World} holds only {_coordinates.Count} stored columns near the origin, fewer than the {Columns} asked for.");

			MethodInfo parseBiomes = typeof(LevelDbProvider).GetMethod("ParseBiomes3D", BindingFlags.NonPublic | BindingFlags.Static);
			_parseBiomes3D = (Action<ChunkColumn, byte[]>) parseBiomes.CreateDelegate(typeof(Action<ChunkColumn, byte[]>));

			MethodInfo inspect = typeof(LevelDbProvider).GetMethod("InspectBlockEntity", BindingFlags.NonPublic | BindingFlags.Static);
			_inspectBlockEntity = (Action<ChunkColumn, ChunkCoordinates, NbtCompound, int, int, int>) inspect.CreateDelegate(typeof(Action<ChunkColumn, ChunkCoordinates, NbtCompound, int, int, int>));
		}

		private static void RealPass()
		{
			foreach (ChunkCoordinates coordinates in _coordinates)
			{
				_provider.GetChunk(coordinates, null);
			}
		}

		/// <summary>
		///     GetChunk's body, phase-timed. Kept line-for-line with the original apart from the
		///     stopwatches and the two delegate calls; the wall-time check in Run is what proves it
		///     still matches.
		/// </summary>
		private static void ReplicaPass(Stopwatch[] phases)
		{
			foreach (ChunkCoordinates coordinates in _coordinates)
			{
				ReplicaGetChunk(coordinates, phases);
			}
		}

		private static ChunkColumn ReplicaGetChunk(ChunkCoordinates coordinates, Stopwatch[] phases)
		{
			phases[0]?.Start();
			byte[] index = Combine(BitConverter.GetBytes(coordinates.X), BitConverter.GetBytes(coordinates.Z));
			phases[0]?.Stop();

			phases[1]?.Start();
			byte[] version = _provider.Db.Get(Combine(index, KeyVersion)) ?? _provider.Db.Get(Combine(index, KeyVersionLegacy));
			phases[1]?.Stop();

			ChunkColumn chunkColumn = null;
			if (version != null && version.First() >= 7)
			{
				phases[2]?.Start();
				chunkColumn = new ChunkColumn
				{
					X = coordinates.X,
					Z = coordinates.Z
				};
				phases[2]?.Stop();

				byte[] chunkDataKey = Combine(index, new byte[] {KeySubChunk, 0});
				for (int i = 0; i < ChunkColumn.WorldHeight / 16; i++)
				{
					chunkDataKey[^1] = unchecked((byte) (sbyte) (i - SubChunkIndexOffset));
					phases[3]?.Start();
					byte[] sectionBytes = _provider.Db.Get(chunkDataKey);
					phases[3]?.Stop();

					if (sectionBytes == null)
					{
						phases[4]?.Start();
						chunkColumn[i, false]?.PutPool();
						chunkColumn[i] = null;
						phases[4]?.Stop();
						continue;
					}

					phases[5]?.Start();
					SubChunk section = chunkColumn[i];
					phases[5]?.Stop();
					_sectionsCreated++;

					phases[6]?.Start();
					_provider.ParseSection(section, sectionBytes);
					phases[6]?.Stop();
				}

				phases[7]?.Start();
				byte[] biome3DBytes = _provider.Db.Get(Combine(index, KeyHeightAndBiomes3D));
				byte[] flatDataBytes = biome3DBytes == null ? _provider.Db.Get(Combine(index, KeyHeightAndBiomes2D)) : null;
				phases[7]?.Stop();

				phases[8]?.Start();
				if (biome3DBytes != null)
				{
					Buffer.BlockCopy(biome3DBytes.AsSpan().Slice(0, 512).ToArray(), 0, chunkColumn.height, 0, 512);
					_parseBiomes3D(chunkColumn, biome3DBytes.AsSpan().Slice(512).ToArray());
				}
				else if (flatDataBytes != null)
				{
					Buffer.BlockCopy(flatDataBytes.AsSpan().Slice(0, 512).ToArray(), 0, chunkColumn.height, 0, 512);
					chunkColumn.biomeId = flatDataBytes.AsSpan().Slice(512, 256).ToArray();
				}
				phases[8]?.Stop();

				phases[9]?.Start();
				byte[] blockEntityBytes = _provider.Db.Get(Combine(index, KeyBlockEntity));
				phases[9]?.Stop();

				if (blockEntityBytes is {Length: > 0})
				{
					phases[10]?.Start();
					Memory<byte> data = blockEntityBytes.AsMemory();

					var file = new NbtFile
					{
						BigEndian = false,
						UseVarInt = false
					};
					int position = 0;
					while (position < data.Length)
					{
						position += (int) file.LoadFromStream(new MemoryStreamReader(data.Slice(position)), NbtCompression.None);

						NbtTag blockEntityTag = file.RootTag;
						int x = blockEntityTag["x"].IntValue;
						int y = blockEntityTag["y"].IntValue;
						int z = blockEntityTag["z"].IntValue;

						BlockEntityUpgrader.Upgrade((NbtCompound) blockEntityTag);

						_inspectBlockEntity(chunkColumn, coordinates, (NbtCompound) blockEntityTag, x, y, z);

						chunkColumn.SetBlockEntity(new BlockCoordinates(x, y, z), (NbtCompound) blockEntityTag);
					}
					phases[10]?.Stop();
				}
			}

			if (chunkColumn != null)
			{
				phases[11]?.Start();
				if (_provider.Dimension == Dimension.Overworld && MiNET.Utils.Config.GetProperty("CalculateLights", false))
				{
					var blockAccess = new SkyLightBlockAccess(_provider, chunkColumn);
					new SkyLightCalculations().RecalcSkyLight(chunkColumn, blockAccess);
				}
				phases[11]?.Stop();

				chunkColumn.IsDirty = false;
			}

			return chunkColumn;
		}

		private static byte[] Index(int x, int z)
		{
			return Combine(BitConverter.GetBytes(x), BitConverter.GetBytes(z));
		}

		private static byte[] Combine(byte[] first, byte second)
		{
			return Combine(first, new[] {second});
		}

		private static byte[] Combine(byte[] first, byte[] second)
		{
			var result = new byte[first.Length + second.Length];
			Buffer.BlockCopy(first, 0, result, 0, first.Length);
			Buffer.BlockCopy(second, 0, result, first.Length, second.Length);

			return result;
		}
	}
}
