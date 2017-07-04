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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using AStarNavigator;
using AStarNavigator.Algorithms;
using AStarNavigator.Providers;
using fNbt;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Utils;
using NUnit.Framework;

namespace MiNET
{
	[TestFixture, Ignore("")]
	public class MinetServerTest
	{
		[Test, Ignore("")]
		public void HighPrecTimerLoadTest()
		{
			Stopwatch sw = new Stopwatch();
			List<HighPrecisionTimer> timers = new List<HighPrecisionTimer>();
			for (int i = 0; i < 100; i++)
			{
				timers.Add(new HighPrecisionTimer(10, SendTick, false));
			}

			Console.WriteLine($"Created {timers.Count} timers, sleeping");

			Thread.Sleep(10000);

			Console.WriteLine($"Done with {timers.Count} timers. Disposing");

			long spins = 0;
			long sleeps = 0;
			long misses = 0;
			long yields = 0;
			foreach (var timer in timers)
			{
				spins += timer.Spins;
				sleeps += timer.Sleeps;
				misses += timer.Misses;
				yields += timer.Yields;
				timer.Dispose();
			}

			Console.WriteLine($"End {timers.Count} timers. " +
			                  $"\nSpins/timer={spins/timers.Count}, " +
			                  $"\nSleeps/timer={sleeps/timers.Count}, " +
			                  $"\nMisses/timer={misses/timers.Count}, " +
			                  $"\nYields/timer={yields/timers.Count} ");
		}

		[Test, Ignore("")]
		public void HighPrecTimerSignalingLoadTest()
		{
			List<Thread> threads = new List<Thread>();
			for (int i = 0; i < 1000; i++)
			{
				threads.Add(new Thread(Runner));
			}

			threads.ForEach(t => t.Start());

			var timer = new HighPrecisionTimer(TIME/2, Interrupt, false);
		}

		private const int TIME = 200;

		ManualResetEvent signal = new ManualResetEvent(false);
		public CancellationTokenSource cancel = new CancellationTokenSource();


		int _count = 0;
		int _interrupts = 0;
		long _timeWaiting = 0;
		long _errors = 0;

		public void PrintResults()
		{
			signal.Set();
			Thread.Sleep(4000);
			Console.WriteLine($"Interrupted {_interrupts} times. ");
			Console.WriteLine($"Ticked {_count} times. ");
			Console.WriteLine($"Errors {_errors}. ");
			Console.WriteLine($"Avg {_timeWaiting/_count} wait. ");
		}

		private void Runner()
		{
			Stopwatch sw = new Stopwatch();
			int count = 0;
			int errors = 0;
			long timeWaiting = 0;
			while (!cancel.IsCancellationRequested)
			{
				sw.Restart();
				signal.WaitOne();
				var elapsedMilliseconds = sw.ElapsedMilliseconds;
				if (elapsedMilliseconds < TIME - 5) errors++;
				if (elapsedMilliseconds > TIME + 5) errors++;
				timeWaiting += elapsedMilliseconds;
				count++;
				//Console.WriteLine($"Tick. ");
			}

			Interlocked.Add(ref _count, count);
			Interlocked.Add(ref _timeWaiting, timeWaiting);
			Interlocked.Add(ref _errors, errors);
		}

		private void Interrupt(object obj)
		{
			if(signal.WaitOne(0))
			{
				signal.Reset();
			}
			else
			{
				_interrupts++;
				signal.Set();
			}
		}


		private void SendTick(object obj)
		{
		}

		[Test, Ignore("")]
		public void TestPathFinder()
		{
			var navigator = new TileNavigator(
				new EmptyBlockedProvider(), // Instance of: IBockedProvider
				new DiagonalNeighborProvider(), // Instance of: INeighborProvider
				new PythagorasAlgorithm(), // Instance of: IDistanceAlgorithm
				new ManhattanHeuristicAlgorithm() // Instance of: IDistanceAlgorithm
			);

			var from = new Tile(-100.5, -102.5);
			var to = new Tile(120.5, 122.5);

			navigator.Navigate(from, to);
		}

		//[Test]
		//public void TestUuid()
		//{
		//	string uuidString = "4ff749d0-1344-1cea-5929-2c63def056b4";
		//	var uuid = new UUID(new Guid(uuidString));
		//	//var uuid = new UUID(Guid.NewGuid());
		//	var uuidBytes = uuid.GetBytes();
		//	var newUuid = new UUID(uuidBytes);
		//	Assert.AreEqual(uuid.Id, newUuid.Id);
		//	Assert.AreEqual(uuidString, newUuid.ToString());
		//}

		[Test]
		public void TestUuid()
		{
			string uuidString = "a821263b-0df8-44ed-87b7-d57a23fdccfc";
			var inputBytes = new byte[] {0xed, 0x44, 0xf8, 0x0d, 0x3b, 0x26, 0x21, 0xa8, 0xfc, 0xcc, 0xfd, 0x23, 0x7a, 0xd5, 0xb7, 0x87};
			var uuid = new UUID(inputBytes);
			Assert.AreEqual(uuidString, uuid.ToString());
			Assert.AreEqual(inputBytes, uuid.GetBytes());

			uuid = new UUID(uuidString);
			Assert.AreEqual(uuidString, uuid.ToString());
			Assert.AreEqual(inputBytes, uuid.GetBytes());
		}

		[Test]
		public void TestBitArray()
		{
			BitArray bits = new BitArray(64);
			bits[0] = true;
			bits[21] = true;
			byte[] bytes = new byte[8];
			bits.CopyTo(bytes, 0);

			//ulong dataValue = BitConverter.ToUInt64(bytes, 0) << 0;
			ulong dataValue = 1 << 20;

			Console.WriteLine($"{dataValue:x2}");
			Console.WriteLine($"{Convert.ToString((long) dataValue, 2)}");

			var stream = new MemoryStream();

			VarInt.WriteUInt64(stream, dataValue);

			byte[] array = stream.ToArray();
			Console.WriteLine($"{Package.HexDump(array)}");


			//var result = VarInt.ReadUInt64(new MemoryStream(array));
			var result = VarInt.ReadUInt64(new MemoryStream(new byte[] {0x80, 0x80, 0x80, 0x11}));
			Console.WriteLine($"{Convert.ToString((long) result, 2)}");

			//Assert.AreEqual(dataValue, result);
			Console.WriteLine($"{dataValue:x2}");

			//81808080808080808001                    .
		}

		[Test]
		public void TestCustomVarInt()
		{
			{
				var stream = new MemoryStream();
				var bytes = GetBytes("ff ff ff ff 0f");
				stream.Write(bytes, 0, bytes.Length);
				stream.Position = 0;
				int result = VarInt.ReadInt32(stream);
				Assert.AreEqual(-1, result);

				int t = (int) 0x7fffffff;
				t |= 0xff << 1;
				Assert.AreEqual(t | 0x0f << 4*7, result);
				Assert.AreEqual(-1, result);

				MemoryStream outstream = new MemoryStream();
				VarInt.WriteInt32(outstream, -1);
				Assert.AreEqual(bytes, outstream.ToArray());
			}
			{
				var stream = new MemoryStream();
				var bytes = GetBytes("ff ff ff ff ff ff ff ff ff 01");
				stream.Write(bytes, 0, bytes.Length);
				stream.Position = 0;
				long result = VarInt.ReadInt64(stream);
				Assert.AreEqual(-1, result);

				MemoryStream outstream = new MemoryStream();
				VarInt.WriteInt64(outstream, -1);
				Assert.AreEqual(bytes, outstream.ToArray());
			}
		}

		private static byte[] GetBytes(string input)
		{
			var strings = input.Split(' ');
			byte[] bytes = new byte[strings.Length];
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = byte.Parse(strings[i], NumberStyles.AllowHexSpecifier);
			}
			return bytes;
		}

		[Test]
		public void TestForce()
		{
			int j = 20;
			float f = (float) j/20.0F;

			f = (f*f + f*2.0F)/3.0F;
			if ((double) f < 0.1D)
			{
				return;
			}

			if (f > 1.0F)
			{
				f = 1.0F;
			}

			Assert.AreEqual(1, f);
		}

		[Test, Ignore("")]
		public void ChunkLoadTest()
		{
			{
				var chunkPosition = new ChunkCoordinates(0, 0);

				Dictionary<Tuple<int, int>, double> newOrders = new Dictionary<Tuple<int, int>, double>();
				double viewDistance = 250;
				double radiusSquared = viewDistance/Math.PI;
				double radius = Math.Ceiling(Math.Sqrt(radiusSquared));
				int centerX = chunkPosition.X;
				int centerZ = chunkPosition.Z;

				Stopwatch sw = new Stopwatch();
				sw.Start();

				for (double x = -radius; x <= radius; x++)
				{
					for (double z = -radius; z <= radius; z++)
					{
						double distance = (x*x) + (z*z);
						if (distance > radiusSquared)
						{
							continue;
						}

						int chunkX = (int) (x + centerX);
						int chunkZ = (int) (z + centerZ);
						Tuple<int, int> index = new Tuple<int, int>(chunkX, chunkZ);
						newOrders[index] = distance;
					}
				}
			}
			{
				var chunkPosition = new ChunkCoordinates(0, 0);

				// A = pi r^2
				// sqrt(A/pi) = r
				Dictionary<Tuple<int, int>, double> newOrders = new Dictionary<Tuple<int, int>, double>();
				double radius = 9;
				double radiusSqr = radius*radius;
				int centerX = chunkPosition.X;
				int centerZ = chunkPosition.Z;

				Stopwatch sw = new Stopwatch();
				sw.Start();

				for (double x = -radius; x <= radius; x++)
				{
					for (double z = -radius; z <= radius; z++)
					{
						double distanceSqr = (x*x) + (z*z);

						if (distanceSqr > radiusSqr) continue;

						int chunkX = (int) (x + centerX);
						int chunkZ = (int) (z + centerZ);
						Tuple<int, int> index = new Tuple<int, int>(chunkX, chunkZ);
						newOrders[index] = distanceSqr;
					}
				}
			}
		}

		[Test]
		public void AckSeriesTest()
		{
			{
				var ranges = Acks.Slize(new List<int>() {0, 1});
				Assert.AreEqual(1, ranges.Count);
				Assert.AreEqual(0, ranges[0].Item1);
				Assert.AreEqual(1, ranges[0].Item2);
			}

			{
				var ranges = Acks.Slize(new List<int>() {1});
				Assert.AreEqual(1, ranges.Count);
				Assert.AreEqual(1, ranges[0].Item1);
				Assert.AreEqual(1, ranges[0].Item2);
			}
			{
				var ranges = Acks.Slize(new List<int>() {1, 2});
				Assert.AreEqual(1, ranges.Count);
				Assert.AreEqual(1, ranges[0].Item1);
				Assert.AreEqual(2, ranges[0].Item2);
			}
			{
				var ranges = Acks.Slize(new List<int>() {1, 4});
				Assert.AreEqual(1, ranges.Count);
				Assert.AreEqual(1, ranges[0].Item1);
				Assert.AreEqual(4, ranges[0].Item2);
			}
			{
				var ranges = Acks.Slize(new List<int>() {1, 2, 6});
				Assert.AreEqual(2, ranges.Count);
				Assert.AreEqual(1, ranges[0].Item1);
				Assert.AreEqual(2, ranges[0].Item2);
				Assert.AreEqual(6, ranges[1].Item1);
				Assert.AreEqual(6, ranges[1].Item2);
			}
			{
				var ranges = Acks.Slize(new List<int>() {1, 2, 4, 5, 6, 9});
				Assert.AreEqual(3, ranges.Count);
				Assert.AreEqual(1, ranges[0].Item1);
				Assert.AreEqual(2, ranges[0].Item2);
				Assert.AreEqual(4, ranges[1].Item1);
				Assert.AreEqual(6, ranges[1].Item2);
				Assert.AreEqual(9, ranges[2].Item1);
				Assert.AreEqual(9, ranges[2].Item2);
			}
			{
				var ranges = Acks.Slize(new List<int>() {1, 2, 4, 6, 7, 9});
				Assert.AreEqual(4, ranges.Count);
				Assert.AreEqual(1, ranges[0].Item1);
				Assert.AreEqual(2, ranges[0].Item2);
				Assert.AreEqual(4, ranges[1].Item1);
				Assert.AreEqual(4, ranges[1].Item2);
				Assert.AreEqual(6, ranges[2].Item1);
				Assert.AreEqual(7, ranges[2].Item2);
				Assert.AreEqual(9, ranges[3].Item1);
				Assert.AreEqual(9, ranges[3].Item2);
			}
			{
				var ranges = Acks.Slize(new List<int>() {1, 2, 4, 5, 6, 9, 10});
				Assert.AreEqual(3, ranges.Count);
				Assert.AreEqual(1, ranges[0].Item1);
				Assert.AreEqual(2, ranges[0].Item2);
				Assert.AreEqual(4, ranges[1].Item1);
				Assert.AreEqual(6, ranges[1].Item2);
				Assert.AreEqual(9, ranges[2].Item1);
				Assert.AreEqual(10, ranges[2].Item2);
			}
			{
				var ranges = Acks.Slize(new List<int>() {0, 2, 4, 5, 6, 9, 10});
				Assert.AreEqual(4, ranges.Count);
				Assert.AreEqual(0, ranges[0].Item1);
				Assert.AreEqual(0, ranges[0].Item2);
				Assert.AreEqual(2, ranges[1].Item1);
				Assert.AreEqual(2, ranges[1].Item2);
				Assert.AreEqual(4, ranges[2].Item1);
				Assert.AreEqual(6, ranges[2].Item2);
				Assert.AreEqual(9, ranges[3].Item1);
				Assert.AreEqual(10, ranges[3].Item2);
			}
		}


		[Test]
		public void BlockEntityTest()
		{
			NbtFile file = new NbtFile();
			file.BigEndian = false;
			var compound = file.RootTag = new NbtCompound(string.Empty);
			compound.Add(new NbtString("Text1", "first line"));
			compound.Add(new NbtString("Text2", "second line"));
			compound.Add(new NbtString("Text3", "third line"));
			compound.Add(new NbtString("Text4", "forth line"));
			compound.Add(new NbtString("id", "Sign"));
			compound.Add(new NbtInt("x", 6));
			compound.Add(new NbtInt("y", 6));
			compound.Add(new NbtInt("z", 6));

			Console.WriteLine(file.ToString());

			Nbt nbt = new Nbt();
			nbt.NbtFile = file;
			McpeBlockEntityData message = McpeBlockEntityData.CreateObject();
			message.coordinates = new BlockCoordinates(6, 6, 6);
			message.namedtag = nbt;

			Assert.NotNull(message.Encode());
			Console.WriteLine(ByteArrayToString(message.Encode()));

			var b = new byte[]
			{
				0xb8, 0x00, 0x00, 0x00, 0x06, 0x06, 0x00, 0x00, 0x00, 0x06, 0x0a, 0x00, 0x00, 0x08, 0x02,
				0x00, 0x49, 0x64, 0x04, 0x00, 0x53, 0x69, 0x67, 0x6e, 0x03, 0x01, 0x00, 0x78, 0x06, 0x00,
				0x00, 0x00, 0x03, 0x01, 0x00, 0x79, 0x06, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x7a, 0x06,
				0x00, 0x00, 0x00, 0x08, 0x05, 0x00, 0x54, 0x65, 0x78, 0x74, 0x31, 0x0a, 0x00, 0x66, 0x69,
				0x72, 0x73, 0x74, 0x20, 0x6c, 0x69, 0x6e, 0x65, 0x08, 0x05, 0x00, 0x54, 0x65, 0x78, 0x74,
				0x32, 0x0b, 0x00, 0x73, 0x65, 0x63, 0x6f, 0x6e, 0x64, 0x20, 0x6c, 0x69, 0x6e, 0x65, 0x08,
				0x05, 0x00, 0x54, 0x65, 0x78, 0x74, 0x33, 0x0a, 0x00, 0x74, 0x68, 0x69, 0x72, 0x64, 0x20,
				0x6c, 0x69, 0x6e, 0x65, 0x08, 0x05, 0x00, 0x54, 0x65, 0x78, 0x74, 0x34, 0x0a, 0x00, 0x66,
				0x6f, 0x72, 0x74, 0x68, 0x20, 0x6c, 0x69, 0x6e, 0x65, 0x00,
			};
		}


		[Test]
		public void NetworkToAsciiTest()
		{
			IPAddress ip;

			var systemAddress = new byte[] {0x00, 0x00, 0xf5, 0xff};
			ip = new IPAddress(systemAddress);
			Console.WriteLine("ip is " + ByteArrayToString(ip.GetAddressBytes()));
			Console.WriteLine("ip is " + ip.ToString());
			Console.WriteLine("port is " + BitConverter.ToUInt16(new byte[] {0xff, 0xf5,}, 0));
			Console.WriteLine("");

			var ipEndpoint = new IPEndPoint(IPAddress.Loopback, 19132);

			Console.WriteLine("ip is " + ByteArrayToString(ipEndpoint.Address.GetAddressBytes()));

			//long netorder_ip = IPAddress.HostToNetworkOrder(hostorder_ip);

			byte[] unknown1 = {0xf5, 0xff, 0xff, 0xf5};
			long netorder_ip = BitConverter.ToInt32(unknown1, 0);
			ip = new IPAddress(unknown1);
			Console.WriteLine("ip is " + ByteArrayToString(ip.GetAddressBytes()));

			byte[] unknown2 = {0xff, 0xff, 0xff, 0xff};
			ip = new IPAddress(unknown2);
			Console.WriteLine("ip is " + ip.ToString());
		}

		[Test]
		public void EncapsulatedHeaderTest()
		{
			DatagramHeader header = new DatagramHeader(0x8c);
			Assert.AreEqual(true, header.isValid);
			Assert.AreEqual(false, header.isACK);
			Assert.AreEqual(false, header.isNAK);
			Assert.AreEqual(false, header.isPacketPair);
			Assert.AreEqual(false, header.hasBAndAS);
			Assert.AreEqual(true, header.isContinuousSend);
			Assert.AreEqual(true, header.needsBAndAs);
		}

		//[Test]
		//public void LabTest()
		//{
		//	// x = 8, z = 9
		//	string test = "78daedcdc109c0200004c1233e125bb32f1b4a836a170acec0bef74b52576f294f36f2f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f7f73fe10f00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002718974bff9ba43b9b1d578f30";
		//	string test2 = "789ced5dc9721cc711ed8e09c5486341185877872f085c110179df480220099120294ba47ce4cd115ee99dd64aefa6f7332fd09d3cf80374d00f38829f40ff8088b32f7067cfd44cf774d792f5b23bd14065c470d033fdea655657d6cbaa6e80af6459f6bf939393f1789c57acf8340fb6a3a3a39733d0e6fc930991133d833f17e1cfc7444f0ecc3ee93ffe71959f15be007f3eeffff202f0f865fabf320099f422fc4533c6017ef862e33f8a5f26fe053f9b5ee8fa03e10b8f3fae29f34b8f3f1dfe687ac9f94f317e881f76405dfff3790130e3ee9f7fbcd0bfd907e1f452f3ef78c63fc9caf8fbe65fc43fe130cfe9cfc4fc5b51400dfee8f2435d7fb5f9e5eb2f6ef8eaf3ff19e08fa63f23f143f59f80fee7d50a28eb5f7fabf31f73052837fee6facfe0968bdf6c40b0f985d6bf950d0085f8e76d45d0abafbf64f54fe9fa2ff29ff835f47fd15a04bd74fdc1e3971a7fb1035078fe55e257d75f845f447f117ed401b3fe9e5ba6a0ff739b1075fffbdfcb05302bf64ef879f4a2fb5f31fa2fa93f547ff1e985f557833f371ea88dbf65fc4af5472cbfdcfdb799a95cff61eb9f363faebfd8febf84feeaf2afeefff3f8a5f59f1bff19e05f14006c7ed1f9577dfd3d61872fbbfe52ebff45fcbafaa3a5bfa635edfa8b6fa760ff577bfd2f7dffb97f7e75fd8da69ff1830ee8afffb5d75faef9d7ed91ecfaa3853f73778974fccdf07be07726602ffcb9895f5d7f99fc67abfee01bf17f1ee5cf21fd3d03fa87e93fbefe8db7230101c4f517f300ad3f9e3c295e474f80f8eb1e54e680f27b0fbfe8fcb7d2bc8f5e7cfdc976a0077e9703d2fad720f0f00b3fffd5b45ee3b738d039bfcb016d7e8f75b1fe9d4e19f4eaf587bafe4becff02fb1ff8fa934fbf182122facb0fbec63fd7df680146fbdff0c73ae0609f9de07640f6f9eb167e4f0708d4ffc65aeb8feee3cfedeb3faf0392f3efc4a2ffddf32ff63fb8fc92cf1fb65b0ffc3efdefbaff1bebdfaafebae93be167e8bfc40540f7bf859fbfeadd01b0fe13d87f666f40b4e92f107f04ffb4c11fafbf31f5471b7fa40b8eeb3f3bc1e549b7f35f4ffcd6eb9f791d90dc7f6dd15f1fbfe8fd476dfd8fe0efe3fe431ffa6b77a097f8eb365dd1ffbefbbfaeffbefd8fe1afbfa3d9651c80e347f7df65f6bf91f5377fffa15d7fa3f945ea8f2771559067f2e9a1fe57e6470a90b3a0bfb983bf870d7867fc9e0e10baff99c7f24bdc80aed47f2d0499d38191ccf347e6fed3ec6dda2800acfc42cf3fadee3f04172002dbbf3efd73d18bdd7f8d3611fd2bda595f8fa5175aff0af047eb9fbdff7d5b510dfe080f5c97dfc6df76ff61c67f741411bf53ff3c1d2052ffbb1cf0f0775cff7b1c90d43f1b7fc70b2070ff5beef75f22f557ecf9db56fe00fdc3ef7f2af393030715feeaac9305f0c3cf5fadd43f3c7e587f97e38fdbfd65fdb59c7f63e31f3bea3ffffe4787fbbf5e13d1df72fbb5b500f1de0912d25f5b0f4cf9fa7fc4f6c0a9ffecf84b63f263f5a7e6fe9b8cfee93e00a4fefb1f1e077cf58fc4f33ff3fdff567a3277fca8fe78eb0fab0372fc07083fbafe1dfbf4bf537e7ffd67e31fc9ac3ffdfc96f04775fd8fd5dfdcc36f61cf57eb8fb85d007ffd69e7175dff36f923f43f427f4d6366fe595b83f8b91eacaeffd6d6960ef8cca2ffac6180fcfd2919fdd3d65fdd07803cfcdefa5f60fde5e02f4ff13980ea9ff71650d7fa375f8069f1cf1a73ecffdb1c381270a0f2fc85167ff5fecf68340aeeffbafec7df7fa5b60e2625ffa895bf9dbec11fbdff3cce677fffab3d7e0b7b1b7f94fe9af27ba6bf35fa7cddc5dfa63f6c0766fcebeb9386fe97f2cbe58fd1dfbc1a7fbe22bf7c7ebefe56f9fdfabf2c4bacfc8c2d0044fe2af79f587dbe127f34bdd0fd2ff70d0037bddcfac342ef73406cffd1caef70416efe75f33bf4e714f147afff7c8fe074cb5f63afcb0f8f3ffefeeb41b9ff50fcdbaebfaefa4b40ff2afa3b5a71c049dfa2bfb1fa67fefe675bfc16f676fe88f56f957fb5fec85f7238d0ca1fa3bfe38afe33f865f437afe96f117f4d7e1bfcebeb01facb7060b5fe28f86b0e34e20fe167dc0370de7ff7589d9fd1e92df1b75bd608dfce1f39ff7be49fc11f3bff47dfff179a7ffdfadfe5fcefed7fbb030dfe98fd4fe7affff4c01f547f04f203ebbf7ca6c298fe47de7f2d1b2bf71f2cfa1fcc1f7fff3542ffdbe75fa603e3e5ffbfd1a2ffa43f36769bfef23c30f3bf9cfe73d7bfd5fe67f10be9ef32fe207e71fd5dad3f6a2ef4c06fedde9cb7feece2f91337bf8cfec5c72f34ff47ff0a840cbf2f7ebb07c2fae3e0ef52ff42e20fe58f58ff39cbef1ef89d05681ffcb5df7f85f5ff88fdfc53ee5affe6ae2d4011fd5dfeff1badfc0bf2e9b4b1312ca3bf35fe95fdff4afd315ddd1997dd7f6eeeff1bfe08fde7e96f3eef80f24da4fe88d0ff257fad0088e567ff1e40bb6d6c60f8298847f9b5fdd78effbcfbaf3d7ef4f1b9ffa44ef931fc791fbf43c76b8f9f84d7cd7fedf13374fea1e3b5fb4f3fff503c9abf9803faf1ebe2cffbf83beff10f3f7fcf77fe6beb9f365ebbff878f47f3573dff2107f4fb3fe1111bfefc31f8fc831ad8001bf8efa7d9e7a6103f00ce86af7fdafc91f8db5278fdf8d5f317d4cf5ef27707c45b4d3b7f078a17cbbf38fcaddb2fcc7f8af31fc1eface0b9e953c7b3b367bb4c85cafc85e52f8a47f32fe53f8657d27f9dfcdd7eb581e7f19bb18ee0b757f09cf4294ede46f2bf89e7e5ef34cf76eaf3074fbe8babb5534f455efe15f8ed51047e7b6764f9668e8f9e02b4f3571fafb2feace81703bf6d82ad8d7f0ebe25ff99f9d7c4332230644b3c2b7fc8d91d4a8525fe191f1fcd3fa5584bfcc6e2fab1f26fc1bfec7383773730cfff0d9a7f46edf84f03835831edfcc1f1baf74f58fc3bcd328e953e3baf363ee2e117e7c6e9df4633ffb9f9b38ae7e46f891fd5eb0f36ff8afe73f2b7ac352cf91fd2c0023f6dc1bbf37767e9bf2dff5f8c4a24edfc13c85fd5f51f2b7fdaf31fc6070700e2dbc60a33ff1a0d30f3df860fcddf62fea87f1699bf0b3f38fa3bc38f2cf81703f00bfd6fc347e6bf6efef69a7fedfcf0fd9bbef2cf860f0e00c56f34f1bcfcb7ce1f81f96bc787e56ff3b305fe153f7ed15191f95be2b7b35ad270f2b764ddb1f2e7cffc21348cb57eea008fee5f0ecaff96fc3b25f8b0261cf8a006c0fc6b338efe965973cf86f7e6ef2c6bef66d6fc0bc2df8ac65754df92ff117dd8d3fd8baef1d14df48adf6eeee2f69a7f5de223f79f58fa49ffd8f3372c7fee46e397f91b577fcff017ec785ffe4e3bc87f21fd1b46fe39f0b10d88e143ae9d63fe08c2bbf217e50fc9bf96cf58f9333b635a3d31423fa7f42717daf0cf02f11b597bfdeecbbf0a1ec9dfda32a6868f9a834f3c96f0099ff0a7177f3c70ff133ee1133e1e9ff23fe1137eb878347f7d0e9cf6f8133ee1878cd7cedf94ff099ff0f178347fd5f3dfe300489ff009df297ef0f9e769c08f77371080777601187ec2277ca778f5fc05f5d3e74100ded94008de1502e47cc227bcc7069fbf78fe43faeb6b2004efeac47864c227fc39c87fbc7e87f5db154310ded100e2fb79c06baf3fd5f307c57b22f0e371fd44f1b8fec6e79faf8140bcb50f6271a1f8733ffe51bc7afd8ae2f1fc75b9108407ea5f5f0861783c7f6d0d84e26d0d443b1ecaaf8d1f7cfea078547f61fd565dbffa1a08c40f367f7d0d7861d69eeb099ff20fc4c3fbcf285e75fdea6b20101f5d3ffb3c08c6c7e6af278478cfc3f9513cac1f281e1dbff0f8d7c60f3fff06abbf4b43f1ad0ec43333f0eae31f1dbff0f8d7c6a3f9a75fffa2784b03e1f801e72f8cb7747e303ee50f8887f54f1b8feb278a3fcff96b0f60fed5dd7d4e432b7ead6793fb77e2f185138f1f7e17c27ffcc8fff761ac767c927df4701c8f2fec5ff72178f608c43f7e08c13f79fc1284ff37d2fdc94e81dd03f2375bffe4f103207f8f4f9e82f9fbfc3f0f80fc3d3e598f07cbd84718fce9c713553c3a7f648f307832d410fda7f183cc1f94ff083be53f82cfd0fc6715615d18187f81ff0c84c7ae1fcd1f183e196ab75e43d0cfd7eebee03fcb6a70fe3e5fc3f068fe3fc5f019e8bf40feebe2b397417c32d02ed8fe07b12083c72f863f46f110bab0e7285eb7fff0fc07f99369dbf66711b4faf845f1181c36f5f853fe9e7783fe9b9dc1eb1f064f962c1960caf3c731cc0fe293254b166d68fe6ae393254b166fdaf9ab8e479f5f507ffe2159b2e19a7afe6be3c1f9431b9f2c1962f0f8d5cedf84d7c5a7f9eb7cdbc0f5131dff2709af8a4f960c3174fec0c73ff8fc278c87e0eaf864c9104bf90fc1d5f1c9922186d7ffdaf9ab9bff9aebb70b4bbb78e9caebd7ab8797ae5e3bac1eef1e5cbfb13cbeb4bbf7fae18dead77bd70e6f568ef7f6afdda81cefeeed5fbf71ab02dfbf7cfd66e5786ffff2e1e2f8e2c5fd2b07d76ebef1e6fce812f97678ebdb6f19aeddabd76fdc7ce3addbe670efa000bf79fb4e79f6dee52b570f0bf0ed3b6f97715cbe5ab6551cbe4d4ded5e3928da2a0fbfb3f9c75f5e285f7ff8c5b7367ff393af6dbeffc32f6d3eb8f7d5cd77bfff85cdf77ef0c5f2b3dffdec1be5f70f7fbd5b9efbd777f737ff7cffd2e69f7e75b13c36e77cf8e3af2cdee9f5c18fbe5cbe1b2c61e8e72a1f7d46c7bffde9d7cbf3e945bc744cdf932ff4fafdcfbfb938d7bce87b6a97380943af77bef75af939f94f6dd177f432fed2cfd577f283da270c9d6fb0f419f94efd41be5431745cf8b365e2228c899770e483698bdec95783233fe855e04abcf98c7888975ef4b3e1357dfa9777f6ca77ea076a6fde46796cda212e3a3631d3e7744ce7d1cf7f7bef72f9fac70757cb776a93aea56983de4dcce61ad2e7868bcefffbfb57ca9f291e3aa677131b9d6fae17e1e8736ad38c13c29a971947c607331e8cdf669cd08bce31e7198c39e79f1f1e2cf0c617e32bf94ded55fbdff48de977c2527f10c68c6bc29a18cd98a7cfcd982ce2dca297e91b83317d427d6bda32d7cbf46fd1c656c1bf55f4f196191b2646931f86db5c4bea4f3a97ae0b8d71e22ec618f9b0e0a88eadf978d82adad8227fcd18a7b6e6e3abc417afff0309a69f37";
		//	string test3 = "7801EDDC310D00000803B02508C0BF5B828A3DAD904E920900000000000000000000000000000000000000000000000000000000000000F016000000000000000000000000000000A86BFF03000000000000000000000000000000000000000000000000000000000000000040CF01072DC034";

		//	//$ordered = zlib_encode(
		//	//int chunkX) . 
		//	//int chunkZ) . 
		//	//$orderedIds . 
		//	//$orderedData . 
		//	//$orderedSkyLight .
		//	//$orderedLight . 
		//	//$this->biomeIds . 
		//	//$biomeColors . 
		//	//$this->tiles


		//	byte[] val = SoapHexBinary.Parse(test).Value;
		//	Assert.IsNotNull(val);


		//	MemoryStream stream = new MemoryStream(val);
		//	if (stream.ReadByte() != 0x78)
		//	{
		//		throw new InvalidDataException("Incorrect ZLib header. Expected 0x78 0x9C");
		//	}
		//	stream.ReadByte();
		//	using (var defStream2 = new DeflateStream(stream, CompressionMode.Decompress, false))
		//	{
		//		NbtBinaryReader defStream = new NbtBinaryReader(defStream2, true);
		//		ChunkColumn chunk = new ChunkColumn();

		//		chunk.x = IPAddress.NetworkToHostOrder(defStream.ReadInt32());
		//		chunk.z = IPAddress.NetworkToHostOrder(defStream.ReadInt32());

		//		int chunkSize = 16*16*128;
		//		Assert.AreEqual(chunkSize, defStream.Read(chunk.blocks, 0, chunkSize));
		//		Assert.AreEqual(chunkSize/2, defStream.Read(chunk.metadata.Data, 0, chunkSize/2));
		//		Assert.AreEqual(chunkSize/2, defStream.Read(chunk.skylight.Data, 0, chunkSize/2));
		//		Assert.AreEqual(chunkSize/2, defStream.Read(chunk.blocklight.Data, 0, chunkSize/2));

		//		Assert.AreEqual(256, defStream.Read(chunk.biomeId, 0, 256));

		//		byte[] ints = new byte[256*4];
		//		Assert.AreEqual(ints.Length, defStream.Read(ints, 0, ints.Length));
		//		int j = 0;
		//		for (int i = 0; i < ints.Length; i = i + 4)
		//		{
		//			chunk.biomeColor[j++] = BitConverter.ToInt32(new[] {ints[i], ints[i + 1], ints[i + 2], ints[i + 3]}, 0);
		//		}

		//		MemoryStream uncompressed = new MemoryStream();
		//		int b = -1;
		//		do
		//		{
		//			b = defStream2.ReadByte();
		//			if (b != -1) uncompressed.WriteByte((byte) b);
		//		} while (b != -1);

		//		Assert.AreEqual(0, uncompressed.Length);
		//		//Assert.AreEqual(83208, uncompressed.Length);
		//		Assert.AreEqual(8, chunk.x);
		//		Assert.AreEqual(9, chunk.z);
		//		byte[] data = chunk.GetBytes();
		//		Assert.AreEqual(83202, data.Length); // Expected uncompressed length
		//	}
		//}

		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder((ba.Length*2) + 100);
			hex.Append("{");
			foreach (byte b in ba)
				hex.AppendFormat("0x{0:x2},", b);
			hex.Append("}");
			return hex.ToString();
		}

		[Test, Ignore("")]
		public void FlagToStringTest()
		{
			long value = new MetadataLong(8590508032).Value; // 1000000000000010001100000000000000

			BitArray bits = new BitArray(BitConverter.GetBytes(value));

			byte[] bytes = new byte[8];
			bits.CopyTo(bytes, 0);

			long dataValue = BitConverter.ToInt64(bytes, 0);

			Assert.AreEqual(value, dataValue);

			Assert.IsTrue(bits[14]);
			Assert.IsTrue(bits[15]);

			List<Entity.DataFlags> flags = new List<Entity.DataFlags>();
			foreach (var val in Enum.GetValues(typeof (Entity.DataFlags)))
			{
				if (bits[(int) val]) flags.Add((Entity.DataFlags) val);
			}

			Assert.AreEqual(4, flags.Count);

			StringBuilder sb = new StringBuilder();
			sb.Append(string.Join(", ", flags));
			Assert.AreEqual("", sb.ToString());
		}
	}
}