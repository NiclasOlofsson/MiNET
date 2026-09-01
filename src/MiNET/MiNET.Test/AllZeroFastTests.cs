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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using MiNET.Worlds;

namespace MiNET.Test
{
	/// <summary>
	///     AllZeroFast decides whether a subchunk is entirely air, and that decision decides whether
	///     the section is sent to every client at all (ChunkColumn.GetTopEmpty trims trailing empty
	///     sections off the chunk). A false negative therefore costs real bandwidth on every chunk
	///     for every player, silently.
	///
	///     The pre-existing coverage only asserted the negative case, a buffer containing a non-zero
	///     value returning false, which an implementation that always returns false passes. These
	///     assert the direction that actually matters.
	/// </summary>
	[TestClass]
	public class AllZeroFastTests
	{
		/// <summary>The real subchunk size: 16x16x16 block indices.</summary>
		private const int SubChunkBlocks = 4096;

		[TestMethod]
		public void Empty_subchunk_sized_buffer_is_all_zero()
		{
			Assert.IsTrue(SubChunk.AllZeroFast(MemoryMarshal.AsBytes<short>(new short[SubChunkBlocks])));
		}

		[TestMethod]
		public void Non_zero_is_found_wherever_it_sits()
		{
			foreach (int position in new[] {0, 1, 15, 127, 128, 2048, SubChunkBlocks - 1})
			{
				var buffer = new short[SubChunkBlocks];
				buffer[position] = 1;

				Assert.IsFalse(SubChunkBlocksAllZero(buffer), $"Missed a non-zero block at index {position}.");
			}
		}

		/// <summary>
		///     Lengths whose byte size is not a multiple of the 128-byte block the vectorised loop
		///     steps in, so the leftover elements go through the tail scan. The tail was indexing a
		///     T-array with a byte offset and comparing the wrong way round, both invisible at 4096
		///     shorts because 8192 bytes divides evenly and the tail never ran.
		/// </summary>
		[TestMethod]
		public void Buffers_with_a_remainder_are_scanned_to_the_end()
		{
			foreach (int length in new[] {1, 7, 63, 65, 100, 1000, 4095})
			{
				Assert.IsTrue(SubChunk.AllZeroFast(MemoryMarshal.AsBytes<short>(new short[length])), $"Empty buffer of {length} reported non-zero.");

				var buffer = new short[length];
				buffer[length - 1] = 1;
				Assert.IsFalse(SubChunk.AllZeroFast(MemoryMarshal.AsBytes<short>(buffer)), $"Missed a non-zero value in the tail of a {length} buffer.");
			}
		}

		[TestMethod]
		public void Works_for_byte_buffers_too()
		{
			Assert.IsTrue(SubChunk.AllZeroFast(new byte[SubChunkBlocks]));

			var buffer = new byte[SubChunkBlocks];
			buffer[SubChunkBlocks - 1] = 1;
			Assert.IsFalse(SubChunk.AllZeroFast(buffer));
		}

		/// <summary>
		///     The behaviour the chunk sender actually depends on. A fresh section is air and must
		///     be trimmable; one block makes it real and it has to be sent.
		/// </summary>
		[TestMethod]
		public void Fresh_subchunk_is_air_and_one_block_makes_it_solid()
		{
			var subChunk = new SubChunk();
			Assert.IsTrue(subChunk.IsAllAir(), "A new section holds nothing but air.");

			subChunk.SetBlock(0, 0, 0, new Stone());
			Assert.IsFalse(subChunk.IsAllAir());
		}

		private static bool SubChunkBlocksAllZero(short[] buffer)
		{
			return SubChunk.AllZeroFast(MemoryMarshal.AsBytes<short>(buffer));
		}
	}
}
