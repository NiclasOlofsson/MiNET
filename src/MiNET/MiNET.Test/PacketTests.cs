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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net;
using MiNET.Test.Streaming;
using MiNET.Worlds;

namespace MiNET.Test
{
	[TestClass, DoNotParallelize]
	public class PacketTests
	{
		[TestMethod]
		public void McpePlayStatus_encode_decode_test()
		{
			var input = new McpePlayStatus();
			input.status = 10;
			var bytes = input.Encode();

			var packet = new McpePlayStatus();
			packet.Decode(bytes.AsMemory());

			Assert.AreEqual(10, packet.status);
		}

		private const int _loops = 10_000_000;

		[TestMethod]
		public void McpePlayStatus_encode_perf_test()
		{
			long totalAllocatedBytes = GC.GetTotalMemory(true);
			var watch = Stopwatch.StartNew();
			byte[] result = new byte[0];
			for (int i = 0; i < _loops; i++)
			{
				var input = new McpeMovePlayer();
				result = input.Encode();
				input.Reset();
			}
			if (result.Length < 0) return;

			long watchElapsedMilliseconds = watch.ElapsedMilliseconds;
			Console.WriteLine($"Time: {watchElapsedMilliseconds:N}ms, mem: {(GC.GetTotalMemory(true) - totalAllocatedBytes) / 1024f:N}KB");
		}

		[TestMethod]
		public void McpePlayStatus_pooled_encode_perf_test()
		{
			long totalAllocatedBytes = GC.GetTotalMemory(true);
			var watch = Stopwatch.StartNew();
			byte[] result = new byte[0];
			for (int i = 0; i < _loops; i++)
			{
				var input = McpeMovePlayer.CreateObject();
				result = input.Encode();
				input.PutPool();
			}
			if (result.Length < 0) return;
			long watchElapsedMilliseconds = watch.ElapsedMilliseconds;
			Console.WriteLine($"Time: {watchElapsedMilliseconds:N}ms, mem: {(GC.GetTotalMemory(true) - totalAllocatedBytes) / 1024f:N}KB");
		}

		[TestMethod]
		public void McpePlayStatus_parallel_encode_perf_test()
		{
			byte[] result = new byte[0];

			void Body(int i)
			{
				var input = new McpeMovePlayer();
				result = input.Encode();
			}

			long totalAllocatedBytes = GC.GetTotalMemory(true);
			var watch = Stopwatch.StartNew();
			Parallel.For(0, _loops, i =>
			{
				var input = new McpeMovePlayer();
				result = input.Encode();
			});
			if (result.Length < 0) return;

			long watchElapsedMilliseconds = watch.ElapsedMilliseconds;
			Console.WriteLine($"Time: {watchElapsedMilliseconds:N}ms, mem: {(GC.GetTotalMemory(true) - totalAllocatedBytes) / 1024f:N}KB");
		}

		[TestMethod]
		public void Collections_perf_test()
		{
			var streamManager = new RecyclableMemoryStreamManager();


			RecyclableMemoryStream stream = null;
			int j = 0;
			var watch = Stopwatch.StartNew();
			for (int i = 0; i < 10_000_000; i++)
			{
				j++;
				using var local = new RecyclableMemoryStream(streamManager, Guid.NewGuid(), null, 100);
				stream = local;
			}

			if (stream == null) return;

			Console.WriteLine($"test {j}, {watch.ElapsedMilliseconds:N}ms");
		}

		[TestMethod]
		public void McpeFullChunk_worst_case_encode_test()
		{
			int numberOfSubChunks = 16;
			var column = new ChunkColumn();
			column.DisableCache = true;
			int block = 0;
			var watch = Stopwatch.StartNew();
			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (int y = 0; y < numberOfSubChunks * 16; y++)
					{
						column.SetBlockByRuntimeId(x, y, z, block++);
					}
				}
			}
			foreach (SubChunk subChunk in column)
			{
				subChunk.DisableCache = true;
			}

			Console.WriteLine($"Setup chunks: {watch.ElapsedMilliseconds}ms");


			// 319761
			int topEmpty = column.GetTopEmpty();
			Console.WriteLine($"Top {topEmpty}");

			var bytes = column.GetBytes(topEmpty);
			Console.WriteLine($"Bytes: Size={bytes.Length} bytes");
			//Assert.AreEqual(319761, bytes.Length);

			// warmup
			for (int i = 0; i < 100; i++)
			{
				column.GetBytes(topEmpty);
				column.GetBatch();
			}

			double count = 1500;

			watch.Restart();
			for (int i = 0; i < count; i++)
			{
				column.GetBytes(topEmpty);
			}
			Console.WriteLine($"Bytes: Avg {watch.ElapsedTicks / count:F0} ticks");

			var packet = column.GetBatch();
			Console.WriteLine($"Batch: Size={packet.payload.Length} bytes");

			watch.Restart();
			for (int i = 0; i < count; i++)
			{
				column.GetBatch();
			}

			Console.WriteLine($"Batch: Avg {watch.ElapsedTicks / count:F0} ticks");

		}

	}
}