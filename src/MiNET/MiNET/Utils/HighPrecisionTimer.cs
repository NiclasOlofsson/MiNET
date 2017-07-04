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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace MiNET.Utils
{
	public class HighPrecisionTimer : IDisposable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (HighPrecisionTimer));

		public Action<object> Action { get; set; }

		protected CancellationTokenSource CancelSource;

		public long Spins = 0;
		public long Sleeps = 0;
		public long Misses = 0;
		public long Yields = 0;

		public AutoResetEvent AutoReset = new AutoResetEvent(true);

		public HighPrecisionTimer(int interval, Action<object> action, bool useSignaling = false)
		{
			Action = action;

			if (interval < 1)
				throw new ArgumentOutOfRangeException();

			CancelSource = new CancellationTokenSource();

			var watch = Stopwatch.StartNew();
			long nextStop = interval;

			var task = new Task(() =>
			{
				Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

				while (!CancelSource.IsCancellationRequested)
				{
					long msLeft = nextStop - watch.ElapsedMilliseconds;
					if (msLeft <= 0)
					{
						if (msLeft < -1) Misses++;

						Action(this);
						AutoReset.Reset();

						// Calculate when the next stop is. If we're too slow on the trigger then we'll skip ticks
						nextStop = (long) (interval*(watch.ElapsedMilliseconds/(float) interval + 1f));

						continue;
					}
					if (msLeft < 5)
					{
						Spins++;
						var stop = nextStop;

						if (useSignaling)
						{
							AutoReset.WaitOne(50);
						}


						if (watch.ElapsedMilliseconds < stop)
						{
							SpinWait.SpinUntil(() => watch.ElapsedMilliseconds >= stop);
						}
						//long t = nextStop - watch.ElapsedMilliseconds;
						//if(t < -5) Log.Warn($"We overslept {t}ms in spin wait");
						continue;
					}

					if (msLeft < 16)
					{
						if (Thread.Yield()) Yields++;

						Sleeps++;
						Thread.Sleep(1);
						continue;
					}

					Sleeps++;
					Thread.Sleep(Math.Max(1, (int) (msLeft - 16)));
					{
						//long t = nextStop - watch.ElapsedMilliseconds;
						//if (t < -5) Log.Warn($"We overslept {t}ms in thread sleep");
					}
				}

				CancelSource.Dispose();
				CancelSource = null;

				AutoReset.Dispose();
				AutoReset = null;
			}, CancelSource.Token, TaskCreationOptions.LongRunning);

			task.Start();
		}

		public void Dispose()
		{
			CancelSource.Cancel();
			AutoReset?.Set();
		}
	}
}