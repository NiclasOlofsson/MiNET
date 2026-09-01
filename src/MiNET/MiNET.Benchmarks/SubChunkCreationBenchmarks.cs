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
using MiNET.Worlds;

namespace MiNET.Benchmarks
{
	/// <summary>
	///     What one section's buffers cost to bring into existence, on a normal heap with the GC
	///     running, which is where the zeroing cost of new arrays is real: the constructor as it
	///     is today (ArrayPool rents, nothing ever returned), the same buffers as plain
	///     allocations, and the same buffers allocated uninitialized.
	/// </summary>
	[MemoryDiagnoser]
	public class SubChunkCreationBenchmarks
	{
		[Benchmark(Baseline = true)]
		public SubChunk RentAsIs()
		{
			return new SubChunk();
		}

		/// <summary>One block holding all four buffers back to back: blocks 0..8192 (as shorts),
		/// logged 8192..12288, blocklight 12288..14336, skylight 14336..16384. Uninitialized: the
		/// zeroing is deferred to whoever writes the block.</summary>
		[Benchmark]
		public byte[] OneBlockUninitialized()
		{
			var block = GC.AllocateUninitializedArray<byte>(16384);
			return block;
		}

		private static readonly byte[] ZeroTemplate = new byte[16384];

		[Benchmark]
		public byte[] OneBlockUninitializedCopyZeros()
		{
			var block = GC.AllocateUninitializedArray<byte>(16384);
			ZeroTemplate.CopyTo(block, 0);
			return block;
		}

		[Benchmark]
		public byte[] OneBlockUninitializedFastFill()
		{
			var block = GC.AllocateUninitializedArray<byte>(16384);
			ChunkColumn.FastFill(ref block, (byte) 0, 0UL);
			return block;
		}

	}
}
