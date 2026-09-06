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

using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;

namespace MiNET.Worlds.Tests
{
	/// <summary>
	///     What serialization owes the world: every block a section holds survives the trip to
	///     bytes and back. The dangerous corner is the single-entry palette, where WriteStore's
	///     uniform case decides whether a storage is written at all, and a wrong skip is silent
	///     data loss: the declared storage count has already gone into the stream, and the save
	///     path simply drops the section's blocks.
	/// </summary>
	// Shares SubChunk's static buffers and pools with AllZeroFastTests; interleaved they read each
	// other's sections.
	[TestClass, DoNotParallelize]
	public class SubChunkWriteStoreTests
	{
		/// <summary>
		///     A section uniformly one block, and specifically the block whose runtime id is 0.
		///     The uniform-skip check compared runtime ids against literal 0 as if 0 meant air, so
		///     exactly this section was dropped on save: written as zero storages, read back as
		///     air. No block may vanish because of the number it happens to hold in the palette.
		/// </summary>
		[TestMethod]
		public void Uniform_section_of_runtime_id_zero_survives_a_save_round_trip()
		{
			var section = new SubChunk();
			section.RuntimeIds.Clear();
			section.RuntimeIds.Add(0);
			// Cells already read index 0 after the constructor's clear; the palette now says
			// index 0 is runtime id 0, making the whole section uniformly that block.
			section.MarkBulkLoaded();

			var provider = new LevelDbProvider();
			using var stream = new MemoryStream();
			provider.Write(section, stream, -4);

			var parsed = new SubChunk();
			provider.ParseSection(parsed, stream.ToArray());

			Assert.AreEqual(0, parsed.GetBlockRuntimeId(0, 0, 0), "the stored block came back as something else");
			Assert.AreEqual(0, parsed.GetBlockRuntimeId(15, 15, 15), "the stored block came back as something else");
			CollectionAssert.AreEqual(new List<int> {0}, parsed.RuntimeIds, "the palette lost its only entry");
		}

		/// <summary>
		///     WriteStore also serializes biome storages, where 0 is a real biome (ocean), not a
		///     sentinel. A uniform value-0 palette must produce a storage; skipping it removes one
		///     of the 24 biome storages a chunk promises the client.
		/// </summary>
		[TestMethod]
		public void Uniform_value_zero_storage_is_written_not_skipped()
		{
			using var stream = new MemoryStream();
			bool wrote = SubChunk.WriteStore(stream, new short[4096], default, new List<int> {0}, isBlockPalette: false);

			Assert.IsTrue(wrote, "a single-entry palette holding value 0 was skipped");
			Assert.AreNotEqual(0, stream.Length, "nothing reached the stream");
		}

		/// <summary>
		///     One waterlogged block means one waterlogged cell. The logged palette must carry air
		///     at slot 0, because every untouched cell holds logged index 0; water landing at slot
		///     0 would read the entire section as waterlogged.
		/// </summary>
		[TestMethod]
		public void One_waterlogged_block_round_trips_as_one_waterlogged_cell()
		{
			int stone = new Stone().GetRuntimeId();
			int water = new Water().GetRuntimeId();

			var section = new SubChunk();
			section.SetBlockByRuntimeId(0, 0, 0, stone);
			section.SetLoggedBlockByRuntimeId(0, 0, 0, water);

			var provider = new LevelDbProvider();
			using var stream = new MemoryStream();
			provider.Write(section, stream, -4);

			var parsed = new SubChunk();
			provider.ParseSection(parsed, stream.ToArray());

			Assert.AreEqual(BlockFactory.AirRuntimeId, parsed.LoggedRuntimeIds[0], "logged palette slot 0 must be air");
			Assert.AreEqual(water, parsed.LoggedRuntimeIds[1]);
			Assert.AreEqual(1, parsed.LoggedBlocks[0], "the waterlogged cell lost its water");
			Assert.AreEqual(0, parsed.LoggedBlocks[1], "an untouched cell became waterlogged");
		}

		/// <summary>
		///     When light calculation is on, a section's light buffers must hold their defaults
		///     from the moment of creation, both creation modes: full skylight, no blocklight.
		///     With it off nothing reads them and they are deliberately left uninitialized.
		/// </summary>
		[TestMethod]
		public void Light_buffers_hold_defaults_when_light_calculation_is_on()
		{
			bool before = SubChunk.InitializeLightBuffers;
			try
			{
				SubChunk.InitializeLightBuffers = true;

				var cleared = new SubChunk();
				Assert.AreEqual(15, cleared.GetSkylight(3, 4, 5));
				Assert.AreEqual(0, cleared.GetBlocklight(3, 4, 5));

				var parseMode = new SubChunk(clearBuffers: false);
				Assert.AreEqual(15, parseMode.GetSkylight(12, 0, 9));
				Assert.AreEqual(0, parseMode.GetBlocklight(12, 0, 9));
			}
			finally
			{
				SubChunk.InitializeLightBuffers = before;
			}
		}

		/// <summary>
		///     An untouched section is air and stays air: it writes as zero storages and parses
		///     back all-air, with no seed entry smuggled into the bytes.
		/// </summary>
		[TestMethod]
		public void Untouched_section_round_trips_as_air()
		{
			var section = new SubChunk();

			var provider = new LevelDbProvider();
			using var stream = new MemoryStream();
			provider.Write(section, stream, -4);

			var parsed = new SubChunk();
			provider.ParseSection(parsed, stream.ToArray());

			Assert.IsTrue(parsed.IsAllAir());
			Assert.AreEqual(BlockFactory.AirRuntimeId, parsed.GetBlockRuntimeId(7, 7, 7));
		}
	}
}
