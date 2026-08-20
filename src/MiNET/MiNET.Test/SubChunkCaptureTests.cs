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
	///     Round-trips captured SubChunkPacket frames from a real server.
	///     <para>
	///         What this exists to catch: a sub-chunk heightmap is a 16x16 grid of int8 heights whose
	///         ROWS are each length-prefixed, so 272 bytes travel for 256 values. Written flat, a real
	///         client reads the first height as a row length and answers with a terminating packet
	///         violation on 0xAE ("too many bytes"), which costs the player the session. Both sides of
	///         MiNET share the codec, so nothing but bytes from another implementation can see it.
	///     </para>
	/// </summary>
	[TestClass]
	public class SubChunkCaptureTests
	{
		/// <summary>
		///     Point MINET_SUBCHUNK_CAPTURES at a BedrockMessageHandlerBase.PacketDumpDir run against
		///     a vanilla server. Inconclusive rather than failing when absent, since the captures live
		///     in temp_auto and are not committed.
		/// </summary>
		[TestMethod]
		public void SubChunk_capture_sweep()
		{
			string directory = Environment.GetEnvironmentVariable("MINET_SUBCHUNK_CAPTURES");
			if (directory == null || !Directory.Exists(directory))
			{
				Assert.Inconclusive($"Set MINET_SUBCHUNK_CAPTURES to a packet dump directory to run this. Got: {directory ?? "(unset)"}");
				return;
			}

			string[] captures = Directory.GetFiles(directory, "*id174.bin");
			if (captures.Length == 0)
			{
				Assert.Inconclusive($"No SubChunkPacket (id174) captures in {directory}.");
				return;
			}

			var failures = new List<string>();
			int withHeightmap = 0;

			foreach (string path in captures)
			{
				byte[] expected = File.ReadAllBytes(path);
				string name = Path.GetFileName(path);

				var packet = new McpeSubChunkPacket();
				try
				{
					packet.Decode(expected.AsMemory());
				}
				catch (Exception e)
				{
					failures.Add($"{name}: decode threw {e.GetType().Name}: {e.Message}");
					continue;
				}

				if (packet.subchunkData.Any(entry => entry.heightMapData?.heights != null)) withHeightmap++;

				if (!expected.SequenceEqual(packet.Encode()))
				{
					failures.Add($"{name}: re-encode differs (cache={packet.cacheEnabled} entries={packet.subchunkData?.Count ?? -1})");
				}
			}

			Console.WriteLine($"Swept {captures.Length} SubChunkPacket captures from {directory}, {withHeightmap} carrying a heightmap.");

			Assert.AreEqual(0, failures.Count, string.Join(Environment.NewLine, failures));
			Assert.AreNotEqual(0, withHeightmap, "No captured frame carried a heightmap, so the length-prefixed rows were never exercised.");
		}
	}
}
