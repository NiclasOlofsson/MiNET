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
using BenchmarkDotNet.Attributes;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Benchmarks
{
	/// <summary>
	///     Where the time goes when a column is read off disk. The same set of stored columns is
	///     loaded three ways: the whole thing, only the database reads, and only the parse over bytes
	///     read in setup. Total minus the two says what the rest of the column costs, which is the
	///     biomes, the block entities and building the object.
	///     <para>
	///         The world is a copy, because a running server holds the LevelDB lock on the original.
	///     </para>
	/// </summary>
	[MemoryDiagnoser]
	public class ChunkLoadBenchmarks
	{
		/// <summary>The Bedrock world to read: the folder holding level.dat and db.</summary>
		private const string World = @"C:\Development\github\MiNET\temp_auto\bench-world";

		private const byte KeyVersion = 0x2c;
		private const byte KeySubChunk = 0x2f;
		private const byte KeyHeightAndBiomes3D = 0x2b;
		private const byte KeyBlockEntity = 0x31;
		private const int SubChunkIndexOffset = 4;

		/// <summary>Columns per invocation, walked outward from the origin so the sample is a
		/// contiguous patch rather than whatever the key order puts first.</summary>
		[Params(32)] public int Columns;

		private LevelDbProvider _provider;
		private readonly List<ChunkCoordinates> _coordinates = new();
		private readonly List<byte[]> _sections = new();

		[GlobalSetup]
		public void Setup()
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

			// The section records themselves, read once, so the parse can be timed without the
			// database underneath it.
			foreach (ChunkCoordinates coordinates in _coordinates)
			{
				byte[] key = Combine(Index(coordinates.X, coordinates.Z), new byte[] {KeySubChunk, 0});
				for (int i = 0; i < ChunkColumn.WorldHeight / 16; i++)
				{
					key[^1] = unchecked((byte) (sbyte) (i - SubChunkIndexOffset));
					byte[] bytes = _provider.Db.Get(key);
					if (bytes != null) _sections.Add(bytes);
				}
			}
		}

		/// <summary>Everything: the reads, the parse, the biomes, the block entities.</summary>
		[Benchmark(Baseline = true)]
		public int LoadColumns()
		{
			int sections = 0;
			foreach (ChunkCoordinates coordinates in _coordinates)
			{
				ChunkColumn column = _provider.GetChunk(coordinates, null);
				if (column != null) sections++;
			}

			return sections;
		}

		/// <summary>Only the database: the same keys a column load asks for, results discarded.</summary>
		[Benchmark]
		public int DatabaseReads()
		{
			int bytes = 0;
			foreach (ChunkCoordinates coordinates in _coordinates)
			{
				byte[] index = Index(coordinates.X, coordinates.Z);

				bytes += _provider.Db.Get(Combine(index, KeyVersion))?.Length ?? 0;

				byte[] key = Combine(index, new byte[] {KeySubChunk, 0});
				for (int i = 0; i < ChunkColumn.WorldHeight / 16; i++)
				{
					key[^1] = unchecked((byte) (sbyte) (i - SubChunkIndexOffset));
					bytes += _provider.Db.Get(key)?.Length ?? 0;
				}

				bytes += _provider.Db.Get(Combine(index, KeyHeightAndBiomes3D))?.Length ?? 0;
				bytes += _provider.Db.Get(Combine(index, KeyBlockEntity))?.Length ?? 0;
			}

			return bytes;
		}

		/// <summary>Only the parse, over the section bytes read in setup.</summary>
		[Benchmark]
		public int ParseSections()
		{
			int blocks = 0;
			foreach (byte[] bytes in _sections)
			{
				// No PutPool: the load path never returns a section's buffers, and returning them
				// here restocks the ArrayPool so every Rent succeeds, which measured a pool state
				// GetChunk cannot be in.
				SubChunk section = SubChunk.CreateObject();
				_provider.ParseSection(section, bytes);
				blocks += section.RuntimeIds.Count;
			}

			return blocks;
		}

		[GlobalCleanup]
		public void Cleanup()
		{
			_provider?.Db?.Close();
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
