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
using System.IO;
using BenchmarkDotNet.Attributes;
using fNbt;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Benchmarks
{
	/// <summary>
	///     What a stored section costs to turn into a usable <see cref="SubChunk" />: the palette NBT,
	///     the runtime id resolution and the block index unpacking. Nothing reads a file or a database
	///     here. The record is built in memory from the real block palette, so the only thing being
	///     timed is what happens to the bytes after a provider has them.
	///     <para>
	///         PaletteSize is the axis that matters: the NBT work scales with the number of palette
	///         entries, while the index unpacking scales with the bits per block those entries force.
	///     </para>
	/// </summary>
	[MemoryDiagnoser]
	public class ChunkParseBenchmarks
	{
		[Params(1, 16, 64, 256)] public int PaletteSize;

		private LevelDbProvider _provider;
		private byte[] _section;
		private byte[] _paletteBytes;
		private List<BlockStateContainer> _containers;
		private List<BlockStateContainer> _queries;
		private readonly BlockStateContainer _scratch = new();
		private readonly StatePool _pool = new();

		[GlobalSetup]
		public void Setup()
		{
			// No database: ParseSection never touches one, and the provider is only the owner of
			// the method under test.
			_provider = new LevelDbProvider();

			_containers = TakePaletteEntries(PaletteSize);
			_section = BuildSection(_containers);
			_paletteBytes = BuildPalette(_containers);

			// Read back rather than reused: querying with the palette's own instances lets Equals
			// short-circuit on reference identity, which never happens against a stored world.
			_queries = new List<BlockStateContainer>(PaletteSize);
			int position = 0;
			var span = new ReadOnlySpan<byte>(_paletteBytes);
			for (int i = 0; i < PaletteSize; i++)
			{
				_queries.Add(SpanPaletteReader.Read(span, ref position));
			}
		}

		/// <summary>The whole conversion, once per invocation, over a section built in setup.</summary>
		[Benchmark(Baseline = true)]
		public int ParseSection()
		{
			SubChunk section = SubChunk.CreateObject();
			try
			{
				_provider.ParseSection(section, _section);
				return section.RuntimeIds.Count;
			}
			finally
			{
				section.PutPool();
			}
		}

		/// <summary>
		///     The tag half alone: fNbt reads every palette entry of the same section and the result is
		///     thrown away. The gap between this and the baseline is what resolution and unpacking cost.
		/// </summary>
		[Benchmark]
		public int PaletteNbtOnly()
		{
			var reader = new MemoryStream(_paletteBytes);
			var file = new NbtFile
			{
				BigEndian = false,
				UseVarInt = false
			};

			int tags = 0;
			for (int i = 0; i < PaletteSize; i++)
			{
				file.LoadFromStream(reader, NbtCompression.None);
				tags += ((NbtCompound) file.RootTag).Count;
			}

			return tags;
		}

		/// <summary>
		///     The same entries read straight off the bytes into the container the resolver wants, with
		///     no tag tree built. Everything the parse produces that is actually used is produced here,
		///     so the gap against <see cref="PaletteNbtOnly" /> is what the tag model costs.
		/// </summary>
		[Benchmark]
		public int PaletteSpanReader()
		{
			var span = new ReadOnlySpan<byte>(_paletteBytes);
			int position = 0;
			int states = 0;

			for (int i = 0; i < PaletteSize; i++)
			{
				BlockStateContainer container = SpanPaletteReader.Read(span, ref position);
				states += container.States.Count;
			}

			return states;
		}

		/// <summary>
		///     The structure walk with nothing materialized: no strings, no state objects, no
		///     container. The gap against <see cref="PaletteSpanReader" /> is what materializing costs
		///     rather than what reading costs.
		/// </summary>
		[Benchmark]
		public int PaletteSkipWalk()
		{
			var span = new ReadOnlySpan<byte>(_paletteBytes);
			int position = 0;

			for (int i = 0; i < PaletteSize; i++)
			{
				SpanPaletteReader.Skip(span, ref position);
			}

			return position;
		}

		/// <summary>
		///     The walk plus only the strings it must make. Against the skip walk this is what
		///     transcoding costs; against the scratch read it is what object construction costs.
		/// </summary>
		[Benchmark]
		public int PaletteStringsOnly()
		{
			var span = new ReadOnlySpan<byte>(_paletteBytes);
			int position = 0;
			int chars = 0;

			for (int i = 0; i < PaletteSize; i++)
			{
				chars += SpanPaletteReader.ReadStringsOnly(span, ref position);
			}

			return chars;
		}

		/// <summary>
		///     The same read as <see cref="PaletteSpanReader" /> but into a reused container with
		///     pooled state objects. The gap between the two is what pooling is worth, with resolution
		///     kept out of it.
		/// </summary>
		[Benchmark]
		public int ScratchReadOnly()
		{
			var span = new ReadOnlySpan<byte>(_paletteBytes);
			int position = 0;
			int states = 0;

			for (int i = 0; i < PaletteSize; i++)
			{
				SpanPaletteReader.ReadInto(span, ref position, _scratch, _pool);
				states += _scratch.States.Count;
			}

			return states;
		}

		/// <summary>
		///     The candidate: walk into a reused container with pooled state objects, then resolve
		///     through the palette exactly as the provider does today. Nothing allocated but the
		///     strings.
		/// </summary>
		[Benchmark]
		public int ScratchReadAndResolve()
		{
			var span = new ReadOnlySpan<byte>(_paletteBytes);
			int position = 0;
			int found = 0;

			for (int i = 0; i < PaletteSize; i++)
			{
				SpanPaletteReader.ReadInto(span, ref position, _scratch, _pool);
				if (BlockFactory.BlockStates.TryGetValue(_scratch, out BlockStateContainer match)) found += match.RuntimeId;
			}

			return found;
		}

		/// <summary>
		///     Resolution alone, over containers built once in setup: the palette dictionary lookup and
		///     the hashing its key does. Whatever a faster parser cannot remove.
		/// </summary>
		[Benchmark]
		public int ResolveOnly()
		{
			int found = 0;
			foreach (BlockStateContainer container in _queries)
			{
				if (BlockFactory.BlockStates.TryGetValue(container, out BlockStateContainer match)) found += match.RuntimeId;
			}

			return found;
		}

		/// <summary>
		///     A version-9 section record as LevelDB stores it: version, storage count, section index,
		///     then one storage of packed block indices followed by its NBT palette. The entries are
		///     taken from the real block palette, so the names and state shapes are the ones the
		///     resolver actually meets.
		/// </summary>
		private static byte[] BuildSection(List<BlockStateContainer> entries)
		{
			int bitsPerBlock = BitsFor(entries.Count);
			int blocksPerWord = bitsPerBlock == 0 ? 0 : 32 / bitsPerBlock;
			int wordCount = bitsPerBlock == 0 ? 0 : (int) Math.Ceiling(4096d / blocksPerWord);

			using var stream = new MemoryStream();

			stream.WriteByte(9); // version
			stream.WriteByte(1); // one storage
			stream.WriteByte(0); // section index

			stream.WriteByte((byte) (bitsPerBlock << 1)); // persistent storage: low bit clear

			// Indices spread evenly over the palette so no entry is favoured and the packing is
			// exercised across every word.
			var words = new uint[wordCount];
			if (bitsPerBlock > 0)
			{
				for (int i = 0; i < 4096; i++)
				{
					int value = i % entries.Count;
					int word = i / blocksPerWord;
					int offset = (i % blocksPerWord) * bitsPerBlock;
					words[word] |= (uint) value << offset;
				}
			}

			foreach (uint word in words)
			{
				stream.Write(BitConverter.GetBytes(word));
			}

			// A single-entry palette states no count, exactly as vanilla writes it.
			if (bitsPerBlock > 0) stream.Write(BitConverter.GetBytes(entries.Count));

			stream.Write(BuildPalette(entries));

			return stream.ToArray();
		}

		/// <summary>Just the palette entries, back to back, as they sit at the tail of a section.</summary>
		private static byte[] BuildPalette(List<BlockStateContainer> entries)
		{
			using var stream = new MemoryStream();

			foreach (BlockStateContainer entry in entries)
			{
				var file = new NbtFile(WritePaletteEntry(entry))
				{
					BigEndian = false,
					UseVarInt = false
				};
				file.SaveToStream(stream, NbtCompression.None);
			}

			return stream.ToArray();
		}

		/// <summary>Distinct entries off the live palette, so the resolver's dictionary lookup meets
		/// real names and real state combinations rather than invented ones.</summary>
		private static List<BlockStateContainer> TakePaletteEntries(int count)
		{
			var entries = new List<BlockStateContainer>(count);
			var seen = new HashSet<string>();

			foreach (BlockStateContainer state in BlockFactory.BlockStates)
			{
				if (!seen.Add(state.Name)) continue; // one per block, so the names vary rather than the states

				entries.Add(state);
				if (entries.Count == count) break;
			}

			if (entries.Count < count) throw new InvalidOperationException($"The palette holds {entries.Count} distinct names, fewer than the {count} asked for.");

			return entries;
		}

		private static NbtCompound WritePaletteEntry(BlockStateContainer container)
		{
			var states = new NbtCompound("states");
			foreach (IBlockState state in container.States)
			{
				switch (state)
				{
					case BlockStateByte stateByte:
						states.Add(new NbtByte(stateByte.Name, stateByte.Value));
						break;
					case BlockStateInt stateInt:
						states.Add(new NbtInt(stateInt.Name, stateInt.Value));
						break;
					case BlockStateString stateString:
						states.Add(new NbtString(stateString.Name, stateString.Value));
						break;
				}
			}

			return new NbtCompound("")
			{
				new NbtString("name", container.Name),
				states,
				new NbtInt("version", BlockPaletteData.BlockStateVersion)
			};
		}

		/// <summary>Bits needed to index the palette, in the sizes the format allows.</summary>
		private static int BitsFor(int paletteSize)
		{
			if (paletteSize <= 1) return 0;

			foreach (int bits in new[] {1, 2, 3, 4, 5, 6, 8, 16})
			{
				if (paletteSize <= 1 << bits) return bits;
			}

			throw new ArgumentOutOfRangeException(nameof(paletteSize));
		}
	}
}
