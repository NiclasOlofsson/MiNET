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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using AStarNavigator;
using NUnit.Framework;

namespace MiNET
{
	[TestFixture]
	public class AStarTest
	{
		[Test]
		public void PerformanceTestOfTile2()
		{
			HashSet<Tile2> list = new HashSet<Tile2>();
			int n = 5000;
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					Tile2 tile2 = new Tile2(i, j);
					list.Add(tile2);
				}
			}

			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					Tile2 tile2 = new Tile2(i, j);
					list.Contains(tile2);
				}
			}
			sw.Stop();
			Console.WriteLine($"Took {sw.ElapsedMilliseconds}");
		}

		[Test]
		public void PerformanceTestOfTile()
		{
			HashSet<Tile> list = new HashSet<Tile>();
			int n = 5000;
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					Tile tile2 = new Tile(i, j);
					list.Add(tile2);
				}
			}

			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					Tile tile2 = new Tile(i, j);
					list.Contains(tile2);
				}
			}
			sw.Stop();
			Console.WriteLine($"Took {sw.ElapsedMilliseconds}");
		}

		[Test]
		public void PerformanceTestOfStarNavigator()
		{
			//Pathfinder pathfinder = new Pathfinder();
			//pathfinder.FindPath()
			Stopwatch sw = Stopwatch.StartNew();
			sw.Stop();
			Console.WriteLine($"Took {sw.ElapsedMilliseconds}");
		}
	}

	public struct Tile2
	{
		public readonly int X;
		public readonly int Y;
		private readonly int _hashCode;

		public Tile2(int x, int y)
		{
			X = x;
			Y = y;
			unchecked
			{
				_hashCode = (X*397) ^ Y;
			}
		}

		public bool Equals(Tile2 other)
		{
			return X == other.X && Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Tile2 && Equals((Tile2) obj);
		}

		public override int GetHashCode()
		{
			return _hashCode;
		}
	}
}