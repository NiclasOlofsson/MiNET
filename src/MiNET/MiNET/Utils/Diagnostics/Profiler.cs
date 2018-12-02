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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using log4net;

namespace MiNET.Utils.Diagnostics
{
	/// <summary>
	///     A general purpose profiler. Profiler is threadsafe in order to be possible to use everywhere independent
	///     of thread knowledge. Measurements can be collected in a hierarchy, so sub-timers can measure individual sections
	///     of code, while still getting an overall view of the whole.
	///     When using measurements, do make sure to check for null in the case profiler is disabled. Check world tick for
	///     examples of proper use.
	/// </summary>
	public class Profiler
	{
		public bool Enabled { get; set; }

		ConcurrentBag<ProfilerResult> _results = new ConcurrentBag<ProfilerResult>();

		public Profiler()
		{
			Enabled = Config.GetProperty("Profiler.Enabled", false);
		}

		public Measurement Begin(string name)
		{
			if (!Enabled) return null;

			return new Measurement(name, this);
		}

		public void End(Measurement measurement)
		{
			measurement.Timer.Stop();

			if (!Enabled) return;

			_results.Add(new ProfilerResult(measurement.Name, measurement.Timer.ElapsedMilliseconds, Stopwatch.GetTimestamp()));
		}

		public void Reset()
		{
			if (!Enabled) _results = new ConcurrentBag<ProfilerResult>();
		}

		public string GetResults(long timespan = 10000)
		{
			long fromTime = (long) (Stopwatch.GetTimestamp() - ((TimeSpan.FromMilliseconds(timespan).TotalSeconds) * Stopwatch.Frequency));

			ProfilerResult[] filtered = _results.ToArray().Where(o => o.TimeStamp > fromTime).ToArray();

			Dictionary<string, long> totalTime = new Dictionary<string, long>();
			foreach (var profilerResult in filtered)
			{
				if (!totalTime.ContainsKey(profilerResult.Name))
				{
					totalTime.Add(profilerResult.Name, 0);
				}

				long val = totalTime[profilerResult.Name];
				totalTime[profilerResult.Name] = val + profilerResult.Time;
			}

			Dictionary<string, long> avarageTime = new Dictionary<string, long>();
			foreach (var counter in totalTime)
			{
				long count = filtered.Count(a => a.Name == counter.Key);
				long avg = counter.Value / count;
				avarageTime.Add(counter.Key, avg);
			}

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("** Profiler result **");

			foreach (var counter in avarageTime.OrderByDescending(a => a.Value))
			{
				long count = filtered.Count(a => a.Name == counter.Key);
				long min = filtered.Where(a => a.Name == counter.Key).Min(a => a.Time);
				long max = filtered.Where(a => a.Name == counter.Key).Max(a => a.Time);
				long violations = filtered.Where(a => a.Name == counter.Key).Count(a => a.Time > 50);
				sb.AppendLine($"{counter.Key} Avarage={counter.Value}ms, Count={count}, Min={min}, Max={max}, Violations={violations}");
			}

			sb.AppendLine("** End results **");

			return sb.ToString();
		}
	}

	public class Measurement
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Measurement));

		public string Name;
		public Stopwatch Timer;
		private Profiler _profiler = null;
		private Measurement _parent = null;
		ConcurrentBag<ProfilerResult> Results = new ConcurrentBag<ProfilerResult>();

		public Measurement(string name, Profiler profiler, Measurement parent = null)
		{
			Name = parent == null ? name : parent.Name + "->" + name;
			_parent = parent;
			_profiler = profiler;

			Timer = Stopwatch.StartNew();
		}

		public Measurement Begin(string name)
		{
			return new Measurement(name, _profiler, this);
		}

		public void End()
		{
			Timer.Stop();

			_profiler.End(this);
			_parent?.Results.Add(new ProfilerResult(Name, Timer.ElapsedMilliseconds, Stopwatch.GetTimestamp()));
		}

		~Measurement()
		{
			if (Timer != null && Timer.IsRunning) Log.Warn($"Timer for {Name} is still running during call to destructor. Please call End() for accurate measurements");
		}
	}

	public struct ProfilerResult
	{
		public string Name;
		public long Time;
		public long TimeStamp;

		public ProfilerResult(string name, long time, long timeStamp)
		{
			Name = name;
			Time = time;
			TimeStamp = timeStamp;
		}
	}
}