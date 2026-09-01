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
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Running;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Benchmarks
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length > 0 && args[0] == "collisions")
			{
				ReportPaletteHashCollisions();
				return;
			}

			if (args.Length > 0 && args[0] == "phases")
			{
				ChunkPhaseMeasurement.Run();
				return;
			}

			if (args.Length > 0 && args[0] == "ctor")
			{
				ChunkPhaseMeasurement.RunCtorSplit();
				return;
			}

			BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);
		}

		/// <summary>
		///     How crowded the palette's 32-bit hash space actually is: entries sharing a hash are the
		///     ones where the lookup has to fall through to Equals to tell them apart.
		/// </summary>
		private static void ReportPaletteHashCollisions()
		{
			var byHash = new Dictionary<int, List<BlockStateContainer>>();

			foreach (BlockStateContainer state in BlockFactory.BlockStates)
			{
				int hash = state.GetHashCode();
				if (!byHash.TryGetValue(hash, out List<BlockStateContainer> bucket)) byHash[hash] = bucket = new List<BlockStateContainer>();
				bucket.Add(state);
			}

			int total = BlockFactory.BlockStates.Count;
			var shared = byHash.Where(pair => pair.Value.Count > 1).ToList();

			Console.WriteLine($"palette entries: {total}");
			Console.WriteLine($"distinct hashes: {byHash.Count}");
			Console.WriteLine($"hashes shared by more than one entry: {shared.Count}");
			Console.WriteLine($"entries involved: {shared.Sum(pair => pair.Value.Count)}");

			foreach ((int hash, List<BlockStateContainer> bucket) in shared.Take(10))
			{
				Console.WriteLine($"  {hash}: {string.Join(" | ", bucket.Select(Describe))}");
			}
		}

		private static string Describe(BlockStateContainer container)
		{
			return $"{container.Name}[{string.Join(",", container.States.Select(state => state.Name))}]";
		}
	}
}
