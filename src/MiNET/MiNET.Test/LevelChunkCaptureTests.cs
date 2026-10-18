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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net;

namespace MiNET.Test
{
	/// <summary>
	///     Decodes real McpeLevelChunk frames captured off vanilla BDS and proves we read them the
	///     way BDS wrote them. The proof is a byte-identical re-encode: if our decode had skipped,
	///     mis-sized or mis-ordered any field, the bytes we write back could not match.
	///
	///     Groundwork for the client blob cache. The committed captures are all cacheEnabled=false,
	///     because the client that recorded them declined the cache, so they pin the format we
	///     already send. The cached form, where the packet carries blob hashes instead of most of
	///     the chunk data, is the thing we do not have and cannot implement against until it is
	///     captured the same way. See BlobCacheDirectorySweep for how to record it.
	/// </summary>
	[TestClass]
	public class LevelChunkCaptureTests
	{
		private static string CaptureDirectory => Path.Combine(AppContext.BaseDirectory, "Data", "chunks");

		[TestMethod]
		[Ignore("1.26.34-era captures use the pre-Cereal sentinel format; re-capture against BDS 1.26.40 at protocol 2168.")]
		public void Bds_level_chunk_decodes_and_re_encodes_byte_identical()
		{
			string[] captures = Directory.GetFiles(CaptureDirectory, "*.bin");
			Assert.AreNotEqual(0, captures.Length, "No committed captures found. They should be copied to output from Data/chunks.");

			foreach (string path in captures)
			{
				byte[] expected = File.ReadAllBytes(path);

				var packet = new McpeLevelChunk();
				packet.Decode(expected.AsMemory());

				byte[] actual = packet.Encode();

				CollectionAssert.AreEqual(expected, actual,
					$"Re-encode of {Path.GetFileName(path)} does not match the captured bytes. "
					+ $"Decoded as limit={packet.clientRequestSubChunkLimit} subChunkCount={packet.subChunkCount} "
					+ $"cacheEnabled={packet.cacheEnabled} chunkData={packet.chunkData?.Length ?? -1} bytes.");
			}
		}

		/// <summary>
		///     The one capture decoded by hand before this test existed, kept as an explicit
		///     known-answer so a change to the decoder shows up as a named field rather than as a
		///     byte diff. BDS 1.26.34, flat world, the chunk the player spawns in.
		/// </summary>
		[TestMethod]
		[Ignore("1.26.34-era capture uses the pre-Cereal sentinel format; re-capture against BDS 1.26.40 at protocol 2168.")]
		public void Bds_level_chunk_fields_match_the_hand_decode()
		{
			byte[] bytes = File.ReadAllBytes(Path.Combine(CaptureDirectory, "bds-1.26.34-levelchunk-limited.bin"));

			var packet = new McpeLevelChunk();
			packet.Decode(bytes.AsMemory());

			Assert.AreEqual(0, packet.chunkPosition.x);
			Assert.AreEqual(0, packet.chunkPosition.z);
			Assert.IsNotNull(packet.clientRequestSubChunkLimit);
			Assert.AreEqual(13u, packet.subChunkCount);
			Assert.IsFalse(packet.cacheEnabled, "Captured with the client declining the blob cache.");
			Assert.AreEqual(0, packet.cacheMetadata.Count, "No hashes travel when the cache is off.");
			Assert.AreEqual(72, packet.chunkData.Length);
		}

		/// <summary>
		///     Sweeps a whole capture directory instead of the two committed frames. Point
		///     MINET_CHUNK_CAPTURES at a BedrockMessageHandlerBase.PacketDumpDir run to check every
		///     chunk BDS sent in a session, which is how the cached form gets verified once we can
		///     record one: run the client against BDS with UseBlobCache = true, then point this at
		///     the dump. Inconclusive rather than failing when the directory is absent, since the
		///     captures live in temp_auto and are not committed.
		/// </summary>
		[TestMethod]
		public void Blob_cache_directory_sweep()
		{
			string directory = Environment.GetEnvironmentVariable("MINET_CHUNK_CAPTURES");
			if (directory == null || !Directory.Exists(directory))
			{
				Assert.Inconclusive($"Set MINET_CHUNK_CAPTURES to a packet dump directory to run this. Got: {directory ?? "(unset)"}");
				return;
			}

			string[] captures = Directory.GetFiles(directory, "*McpeLevelChunk.bin");
			if (captures.Length == 0)
			{
				Assert.Inconclusive($"No McpeLevelChunk captures in {directory}.");
				return;
			}

			var cached = new List<string>();
			var failures = new List<string>();

			foreach (string path in captures)
			{
				byte[] expected = File.ReadAllBytes(path);
				string name = Path.GetFileName(path);

				var packet = new McpeLevelChunk();
				try
				{
					packet.Decode(expected.AsMemory());
				}
				catch (Exception e)
				{
					failures.Add($"{name}: decode threw {e.GetType().Name}: {e.Message}");
					continue;
				}

				if (packet.cacheEnabled) cached.Add($"{name}: {packet.cacheMetadata?.Count ?? 0} hashes, {packet.chunkData?.Length ?? -1} inline bytes");

				if (!expected.SequenceEqual(packet.Encode()))
				{
					failures.Add($"{name}: re-encode differs (limit={packet.clientRequestSubChunkLimit} count={packet.subChunkCount} cache={packet.cacheEnabled})");
				}
			}

			Console.WriteLine($"Swept {captures.Length} chunk captures from {directory}, {cached.Count} with the cache on.");
			foreach (string line in cached) Console.WriteLine("  " + line);

			Assert.AreEqual(0, failures.Count, string.Join(Environment.NewLine, failures));
		}
	}
}
