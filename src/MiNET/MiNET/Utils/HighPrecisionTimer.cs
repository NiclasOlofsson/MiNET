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

		public class TickEventArgs : EventArgs
		{
			public TimeSpan Duration { get; private set; }
			public long TotalTicks { get; private set; }

			public TickEventArgs(TimeSpan totalDuration, long totalTicks)
			{
				Duration = totalDuration;
				TotalTicks = totalTicks;
			}
		}

		public event EventHandler<TickEventArgs> Tick;
		protected CancellationTokenSource CancelSource;

		public HighPrecisionTimer(int interval)
		{
			Log.Debug($"Starting HighPrecisionTimer with {interval} ms interval");

			if (interval < 1)
				throw new ArgumentOutOfRangeException();
			//Trace.Assert(interval >= 10, "Not reliable/tested, may use too much CPU");

			CancelSource = new CancellationTokenSource();

			var watch = Stopwatch.StartNew();
			long durationMs = 0;
			long totalTicks = 0;
			long nextStop = interval;
			long lastReport = 0;

			var task = new Task(() =>
			{
				while (!CancelSource.IsCancellationRequested)
				{
					long msLeft = nextStop - watch.ElapsedMilliseconds;
					if (msLeft <= 0)
					{
						durationMs = watch.ElapsedMilliseconds;
						totalTicks = durationMs/interval;

						if (durationMs - lastReport >= 1000)
						{
							lastReport = durationMs;
						}

						Tick?.Invoke(this, new TickEventArgs(TimeSpan.FromMilliseconds(durationMs), totalTicks));

						// Calculate when the next stop is. If we're too slow on the trigger then we'll skip ticks
						nextStop = interval*(watch.ElapsedMilliseconds/interval + 1);
					}
					else if (msLeft < 16)
					{
						SpinWait.SpinUntil(() => watch.ElapsedMilliseconds >= nextStop);
						continue;
					}

					Thread.Sleep(1);
					//Thread.Sleep(Math.Max(1, (int) (msLeft - 16)));
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