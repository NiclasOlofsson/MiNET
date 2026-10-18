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
	///     Reads the section payloads out of real McpeSubChunkPacket frames captured off BDS, to
	///     answer what vanilla actually puts on the wire: which record version, whether the section
	///     index is embedded, and whether the palette is runtime or persistent. Those three
	///     decisions are what SubChunkBlob has to match.
	/// </summary>
	[TestClass]
	public class SubChunkWireFormatTests
	{
		/// <summary>
		///     What vanilla actually puts inside a blob. This is the format SubChunkBlob has to
		///     produce: record version, whether the section index is embedded, and whether the
		///     palette is runtime or persistent.
		/// </summary>
		[TestMethod]
		public void Report_bds_blob_contents()
		{
			string directory = Environment.GetEnvironmentVariable("MINET_CHUNK_CAPTURES");
			if (directory == null || !Directory.Exists(directory))
			{
				Assert.Inconclusive($"Set MINET_CHUNK_CAPTURES to a packet dump directory. Got: {directory ?? "(unset)"}");
				return;
			}

			string[] captures = Directory.GetFiles(directory, "*McpeClientCacheMissResponse.bin");
			var shapes = new Dictionary<string, int>();
			var sizes = new List<int>();
			var allHashes = new HashSet<ulong>();
			int blobCount = 0;

			foreach (string path in captures)
			{
				var packet = new McpeClientCacheMissResponse();
				packet.Decode(File.ReadAllBytes(path).AsMemory());
				if (packet.blobs == null) continue;

				foreach (KeyValuePair<ulong, byte[]> blob in packet.blobs)
				{
					blobCount++;
					byte[] data = blob.Value;
					sizes.Add(data.Length);
					allHashes.Add(blob.Key);

					if (data.Length < 4)
					{
						shapes.TryGetValue($"tiny ({data.Length} bytes): {Convert.ToHexString(data)}", out int tiny);
						shapes[$"tiny ({data.Length} bytes): {Convert.ToHexString(data)}"] = tiny + 1;
						continue;
					}

					byte version = data[0];
					byte storages = data[1];
					byte next = data[2];
					string shape = version >= 9
						? $"v{version} storages={storages} sectionIndex={unchecked((sbyte) next)} paletteFlag=0x{data[3]:x2} runtime={(data[3] & 1) == 1}"
						: $"v{version} storages={storages} paletteFlag=0x{next:x2} runtime={(next & 1) == 1}";

					shapes.TryGetValue(shape, out int seen);
					shapes[shape] = seen + 1;
				}
			}

			Console.WriteLine($"{captures.Length} miss responses, {blobCount} blobs, sizes {(sizes.Count == 0 ? 0 : sizes.Min())}..{(sizes.Count == 0 ? 0 : sizes.Max())} bytes.");

			// Dumped so two capture runs can be compared: identical content produces identical
			// hashes, so an empty intersection between runs means the blob bytes changed.
			string hashFile = Path.Combine(directory, "blob-hashes.txt");
			File.WriteAllLines(hashFile, allHashes.OrderBy(h => h).Select(h => h.ToString("X16")));
			Console.WriteLine($"{allHashes.Count} distinct blob hashes written to {hashFile}");
			foreach (KeyValuePair<string, int> shape in shapes.OrderByDescending(s => s.Value))
			{
				Console.WriteLine($"  {shape.Value,5}x  {shape.Key}");
			}

			Assert.AreNotEqual(0, blobCount, "No blobs in the capture. The client has to report misses for the server to send any.");
		}

		[TestMethod]
		public void Report_bds_subchunk_record_headers()
		{
			string directory = Environment.GetEnvironmentVariable("MINET_CHUNK_CAPTURES");
			if (directory == null || !Directory.Exists(directory))
			{
				Assert.Inconclusive($"Set MINET_CHUNK_CAPTURES to a packet dump directory. Got: {directory ?? "(unset)"}");
				return;
			}

			string[] captures = Directory.GetFiles(directory, "*McpeSubChunkPacket.bin");
			if (captures.Length == 0)
			{
				Assert.Inconclusive($"No McpeSubChunkPacket captures in {directory}.");
				return;
			}

			var shapes = new Dictionary<string, int>();
			var results = new Dictionary<string, int>();
			int entryCount = 0;
			int emptyData = 0;
			int roundTripFailures = 0;
			int cachedPackets = 0;
			int withHash = 0;

			foreach (string path in captures)
			{
				byte[] raw = File.ReadAllBytes(path);
				var packet = new McpeSubChunkPacket();
				packet.Decode(raw.AsMemory());

				// Everything below is only meaningful if we read the frame the way BDS wrote it.
				if (!raw.SequenceEqual(packet.Encode())) roundTripFailures++;
				if (packet.cacheEnabled) cachedPackets++;

				foreach (SubChunkPacketData entry in packet.subChunkData)
				{
					entryCount++;

					var requestResult = (SubChunkPacketData.SubChunkRequestResult) entry.subChunkRequestResult;
					string result = requestResult.ToString();
					results.TryGetValue(result, out int seenResult);
					results[result] = seenResult + 1;

					if (entry.blobId is > 0) withHash++;

					byte[] data = entry.serializedSubChunk;
					if (data == null || data.Length == 0)
					{
						emptyData++;
						continue;
					}

					// version, storage count, then either the section index (v9) or straight into
					// the first storage's palette-and-flag byte (v8).
					byte version = data[0];
					byte storages = data[1];
					byte third = data.Length > 2 ? data[2] : (byte) 0;

					string shape = version >= 9
						? $"v{version} storages={storages} sectionIndex={unchecked((sbyte) third)} result={requestResult}"
						: $"v{version} storages={storages} paletteFlag=0x{third:x2} runtime={(third & 1) == 1} result={requestResult}";

					// For v9 the palette flag is one byte further along.
					if (version >= 9 && data.Length > 3)
					{
						byte paletteFlag = data[3];
						shape += $" paletteFlag=0x{paletteFlag:x2} runtime={(paletteFlag & 1) == 1}";
					}

					shapes.TryGetValue(shape, out int seen);
					shapes[shape] = seen + 1;
				}
			}

			Console.WriteLine($"{captures.Length} SubChunk packets ({cachedPackets} cacheEnabled), {entryCount} entries, {emptyData} with no data, {withHash} carrying a blob hash.");
			Console.WriteLine($"Round-trip failures: {roundTripFailures}. Anything above zero means the shapes below are misread.");
			foreach (KeyValuePair<string, int> result in results.OrderByDescending(r => r.Value))
			{
				Console.WriteLine($"  {result.Value,5}x  result={result.Key}");
			}
			foreach (KeyValuePair<string, int> shape in shapes.OrderByDescending(s => s.Value))
			{
				Console.WriteLine($"  {shape.Value,5}x  {shape.Key}");
			}

			Assert.AreNotEqual(0, entryCount);
		}
	}
}
