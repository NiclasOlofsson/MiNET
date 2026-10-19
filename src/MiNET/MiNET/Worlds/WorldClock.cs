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

using System.Collections.Generic;
using System.Text;
using MiNET.Net;

namespace MiNET.Worlds
{
	/// <summary>
	///     A named instant on the clock's cycle, like sunset. Content refers to these by name
	///     instead of comparing raw tick numbers.
	/// </summary>
	public class WorldClockMarker
	{
		public WorldClockMarker(string name, int time, int period = WorldClock.DefaultTicksPerDay)
		{
			Name = name;
			Time = time;
			Period = period;
		}

		public string Name { get; }

		/// <summary>Tick within the cycle at which this marker occurs.</summary>
		public int Time { get; }

		/// <summary>Length of the cycle this marker repeats on.</summary>
		public int Period { get; }

		public long Id => WorldClock.IdOf(Name);
	}

	/// <summary>
	///     The level's clock. One per level: a level has one time, and multiple clocks per level buys
	///     nothing today.
	///
	///     This owns the time. <see cref="Level.WorldTime" /> and <see cref="Level.CurrentWorldCycleTime" />
	///     forward to it, the world provider seeds it in Level.Initialize, and the tick loop advances
	///     it, so there is one number and one cycle length rather than several that can drift.
	/// </summary>
	public class WorldClock
	{
		public const int DefaultTicksPerDay = 24000;

		public const string Overworld = "minecraft:overworld";

		/// <summary>Vanilla's overworld markers: the tick each one falls on within the day.</summary>
		public static readonly IReadOnlyList<WorldClockMarker> OverworldMarkers = new List<WorldClockMarker>
		{
			new WorldClockMarker("minecraft:sunrise", 23000),
			new WorldClockMarker("minecraft:night", 13000),
			new WorldClockMarker("minecraft:noon", 6000),
			new WorldClockMarker("minecraft:midnight", 18000),
			new WorldClockMarker("minecraft:day", 1000),
			new WorldClockMarker("minecraft:sunset", 12000)
		};

		private readonly Level _level;

		public WorldClock(Level level, string name = Overworld)
		{
			_level = level;
			Name = name;
			Markers = new List<WorldClockMarker>(OverworldMarkers);
		}

		public string Name { get; }

		public long Id => IdOf(Name);

		public List<WorldClockMarker> Markers { get; }

		/// <summary>Length of one cycle. Vanilla's day is <see cref="DefaultTicksPerDay" /> ticks.</summary>
		public int TicksPerDay { get; set; } = DefaultTicksPerDay;

		/// <summary>
		///     Ticks elapsed on this clock, seeded from the world provider and advanced by the level
		///     tick. Keeps counting past one day; use <see cref="TimeOfDay" /> for the position
		///     within the current one.
		/// </summary>
		public long Time { get; set; }

		/// <summary>
		///     Whether the cycle is halted. This is the doDaylightCycle gamerule, which lives on the
		///     level because commands and rule broadcasts already read it there.
		/// </summary>
		public bool Paused
		{
			get => !_level.DoDaylightcycle;
			set => _level.DoDaylightcycle = !value;
		}

		/// <summary>Where the clock sits within the current cycle, 0 to <see cref="TicksPerDay" />.</summary>
		public long TimeOfDay => Time % TicksPerDay;

		/// <summary>Advances one tick, unless paused.</summary>
		public void Tick()
		{
			if (!Paused) Time++;
		}

		public WorldClockMarker GetMarker(string name)
		{
			return Markers.Find(m => m.Name == name);
		}

		/// <summary>
		///     Clock and marker ids are FNV-1 64 of the name, multiply then xor. Verified against a
		///     BDS 1.26.34 capture.
		/// </summary>
		public static long IdOf(string name)
		{
			ulong hash = 14695981039346656037;
			foreach (byte b in Encoding.UTF8.GetBytes(name))
			{
				hash = unchecked(hash * 1099511628211);
				hash ^= b;
			}

			return unchecked((long) hash);
		}

		/// <summary>Declares the clock and its markers. Must reach a client before any state update.</summary>
		public McpeSyncWorldClocks CreateRegistryPacket()
		{
			var clock = new WorldClockData
			{
				id = Id,
				name = Name,
				time = (int) Time,
				isPaused = Paused,
				timeMarkers = new List<TimeMarkerData>()
			};

			foreach (WorldClockMarker marker in Markers)
			{
				clock.timeMarkers.Add(new TimeMarkerData
				{
					id = marker.Id,
					name = marker.Name,
					time = marker.Time,
					period = marker.Period
				});
			}

			McpeSyncWorldClocks packet = McpeSyncWorldClocks.CreateObject();
			packet.data = new InitializeRegistryData {clockData = new List<WorldClockData> {clock}};
			return packet;
		}

		/// <summary>The current tick, for a clock the client already knows about.</summary>
		public McpeSyncWorldClocks CreateStatePacket()
		{
			McpeSyncWorldClocks packet = McpeSyncWorldClocks.CreateObject();
			packet.data = new SyncStateData
			{
				clockData = new List<SyncWorldClockStateData>
				{
					new SyncWorldClockStateData
					{
						clockId = Id,
						time = (int) Time,
						isPaused = Paused
					}
				}
			};
			return packet;
		}

		public void SendRegistryTo(Player player)
		{
			player.SendPacket(CreateRegistryPacket());
		}

		public void SendStateTo(Player player)
		{
			player.SendPacket(CreateStatePacket());
		}

		/// <summary>Pushes the current tick to everyone in the level.</summary>
		public void BroadcastState()
		{
			_level.RelayBroadcast(CreateStatePacket());
		}
	}
}
