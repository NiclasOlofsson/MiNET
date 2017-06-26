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
			Log.Debug($"Starting HighPrecisionTimer with {interval} ms interval");

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
						nextStop = (long)(interval * (watch.ElapsedMilliseconds / (float)interval + 1f));

						continue;
					}
					if (msLeft < 5)
					{
						Spins++;
						var stop = nextStop;

						if (useSignaling)
						{
							if (!AutoReset.WaitOne(500))
							{
								Log.Error("No signal received");
							}
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
					Thread.Sleep(Math.Max(1, (int)(msLeft - 16)));
					{
						//long t = nextStop - watch.ElapsedMilliseconds;
						//if (t < -5) Log.Warn($"We overslept {t}ms in thread sleep");
					}
				}
			}, CancelSource.Token, TaskCreationOptions.LongRunning);

			task.Start();
		}

		public void Dispose()
		{
			CancelSource.Cancel();
		}
	}
}