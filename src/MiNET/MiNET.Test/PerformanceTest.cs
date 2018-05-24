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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Diagnostics;
using NUnit.Framework;

namespace MiNET
{
	[TestFixture]
	public class PerformanceTest
	{
		//[Test]
		//public void LoopTest()
		//{

		//	Stopwatch sw = new Stopwatch();
		//	var t = sw.ElapsedTicks;
		//	long c = 0;

		//	c = 0;
		//	sw.Restart();
		//	for (short i = 0; i < 10; i++)
		//	for (short j = 0; j < short.MaxValue; j++)
		//	for (short k = 0; k < short.MaxValue; k++)
		//	{
		//		c++;
		//	}

		//	long shortTicks = sw.ElapsedTicks;
		//	Console.WriteLine(shortTicks / TimeSpan.TicksPerMillisecond);

		//	c = 0;
		//	sw.Restart();
		//	for (long i = 0; i < 10; i++)
		//	for (long j = 0; j < short.MaxValue; j++)
		//	for (long k = 0; k < short.MaxValue; k++)
		//	{
		//		c++;
		//	}

		//	long longTicks = sw.ElapsedTicks;
		//	Console.WriteLine(longTicks/TimeSpan.TicksPerMillisecond);

		//	c = 0;
		//	sw.Restart();
		//	for (int i = 0; i < 10; i++)
		//	for (int j = 0; j < short.MaxValue; j++)
		//	for (int k = 0; k < short.MaxValue; k++)
		//	{
		//				c++;
		//	}

		//	long intTicks = sw.ElapsedTicks;
		//	Console.WriteLine(intTicks / TimeSpan.TicksPerMillisecond);
		//}

		//[Test]
		//public void LoopTest2()
		//{

		//	Stopwatch sw = new Stopwatch();
		//	var t = sw.ElapsedTicks;

		//	short s = 0;
		//	sw.Restart();
		//	for (int i = 0; i < 10; i++)
		//	for (int j = 0; j < short.MaxValue; j++)
		//	{
		//		s = 0;
		//		for (int k = 0; k < short.MaxValue; k++)
		//		{
		//			s++;
		//		}
		//	}

		//	long shortTicks = sw.ElapsedTicks;
		//	Console.WriteLine(shortTicks / TimeSpan.TicksPerMillisecond);

		//	int integ = 0;
		//	sw.Restart();
		//	for (int i = 0; i < 10; i++)
		//	for (int j = 0; j < short.MaxValue; j++)
		//	{
		//		integ = 0;
		//		for (int k = 0; k < short.MaxValue; k++)
		//		{
		//			integ++;
		//		}
		//	}

		//	long longTicks = sw.ElapsedTicks;
		//	Console.WriteLine(longTicks / TimeSpan.TicksPerMillisecond);

		//	long lon = 0;
		//	sw.Restart();
		//	for (int i = 0; i < 10; i++)
		//	for (int j = 0; j < short.MaxValue; j++)
		//	{
		//		lon = 0;
		//		for (int k = 0; k < short.MaxValue; k++)
		//		{
		//			lon++;
		//		}
		//	}

		//	long intTicks = sw.ElapsedTicks;
		//	Console.WriteLine(intTicks / TimeSpan.TicksPerMillisecond);
		//}

	}
}