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

#region Contact

/*
 * Leslie Sanford
 * Email: jabberdabber@hotmail.com
 */

#endregion

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MiNET.Utils.IO
{
	/// <summary>
	///     Defines constants for the multimedia Timer's event types.
	/// </summary>
	public enum TimerMode
	{
		/// <summary>
		///     Timer event occurs once.
		/// </summary>
		OneShot,

		/// <summary>
		///     Timer event occurs periodically.
		/// </summary>
		Periodic
	};

	/// <summary>
	///     Represents information about the multimedia Timer's capabilities.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct TimerCaps
	{
		/// <summary>
		///     Minimum supported period in milliseconds.
		/// </summary>
		public int periodMin;

		/// <summary>
		///     Maximum supported period in milliseconds.
		/// </summary>
		public int periodMax;
	}

	/// <summary>
	///     Represents the Windows multimedia timer.
	/// </summary>
	public sealed class MultiMediaTimer : IComponent
	{
		#region Timer Members

		#region Delegates

		// Represents the method that is called by Windows when a timer event occurs.
		private delegate void TimeProc(int id, int msg, int user, int param1, int param2);

		// Represents methods that raise events.
		private delegate void EventRaiser(EventArgs e);

		#endregion

		#region Win32 Multimedia Timer Functions

		// Gets timer capabilities.
		[DllImport("winmm.dll")]
		private static extern int timeGetDevCaps(ref TimerCaps caps, int sizeOfTimerCaps);

		// Creates and starts the timer.
		[DllImport("winmm.dll")]
		private static extern int timeSetEvent(int delay, int resolution, TimeProc proc, int user, int mode);

		// Stops and destroys the timer.
		[DllImport("winmm.dll")]
		private static extern int timeKillEvent(int id);

		// Indicates that the operation was successful.
		private const int TIMERR_NOERROR = 0;

		#endregion

		#region Fields

		// Timer identifier.
		private int _timerId;

		// Timer mode.
		private volatile TimerMode _mode;

		// Period between timer events in milliseconds.
		private volatile int _period;

		// Timer resolution in milliseconds.
		private volatile int _resolution;

		// Called by Windows when a timer periodic event occurs.
		private TimeProc _timeProcPeriodic;

		// Called by Windows when a timer one shot event occurs.
		private TimeProc _timeProcOneShot;

		// Represents the method that raises the Tick event.
		private EventRaiser _tickRaiser;

		// Indicates whether or not the timer is running.
		private bool _running = false;

		// Indicates whether or not the timer has been disposed.
		private volatile bool _disposed = false;

		// The ISynchronizeInvoke object to use for marshaling events.
		private ISynchronizeInvoke _synchronizingObject = null;

		// For implementing IComponent.
		private ISite _site = null;

		// Multimedia timer capabilities.
		private static TimerCaps _caps;

		#endregion

		#region Events

		/// <summary>
		///     Occurs when the Timer has started;
		/// </summary>
		public event EventHandler Started;

		/// <summary>
		///     Occurs when the Timer has stopped;
		/// </summary>
		public event EventHandler Stopped;

		/// <summary>
		///     Occurs when the time period has elapsed.
		/// </summary>
		public event EventHandler Tick;

		#endregion

		#region Construction

		/// <summary>
		///     Initialize class.
		/// </summary>
		static MultiMediaTimer()
		{
			// Get multimedia timer capabilities.
			timeGetDevCaps(ref _caps, Marshal.SizeOf(_caps));
		}

		/// <summary>
		///     Initializes a new instance of the Timer class.
		/// </summary>
		public MultiMediaTimer()
		{
			Initialize();
		}

		~MultiMediaTimer()
		{
			if (IsRunning)
			{
				// Stop and destroy timer.
				timeKillEvent(_timerId);
			}
		}

		// Initialize timer with default values.
		private void Initialize()
		{
			this._mode = TimerMode.Periodic;
			this._period = Capabilities.periodMin;
			this._resolution = 1;

			_running = false;

			_timeProcPeriodic = new TimeProc(TimerPeriodicEventCallback);
			_timeProcOneShot = new TimeProc(TimerOneShotEventCallback);
			_tickRaiser = new EventRaiser(OnTick);
		}

		#endregion

		#region Methods

		/// <summary>
		///     Starts the timer.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		///     The timer has already been disposed.
		/// </exception>
		/// <exception cref="TimerStartException">
		///     The timer failed to start.
		/// </exception>
		public void Start()
		{
			#region Require

			if (_disposed)
			{
				throw new ObjectDisposedException("Timer");
			}

			#endregion

			#region Guard

			if (IsRunning)
			{
				return;
			}

			#endregion

			// If the periodic event callback should be used.
			if (Mode == TimerMode.Periodic)
			{
				// Create and start timer.
				_timerId = timeSetEvent(Period, Resolution, _timeProcPeriodic, 0, (int) Mode);
			}
			// Else the one shot event callback should be used.
			else
			{
				// Create and start timer.
				_timerId = timeSetEvent(Period, Resolution, _timeProcOneShot, 0, (int) Mode);
			}

			// If the timer was created successfully.
			if (_timerId != 0)
			{
				_running = true;

				if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
				{
					SynchronizingObject.BeginInvoke(
						new EventRaiser(OnStarted),
						new object[] {EventArgs.Empty});
				}
				else
				{
					OnStarted(EventArgs.Empty);
				}
			}
			else
			{
				throw new TimerStartException("Unable to start multimedia Timer.");
			}
		}

		/// <summary>
		///     Stops timer.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		///     If the timer has already been disposed.
		/// </exception>
		public void Stop()
		{
			#region Require

			if (_disposed)
			{
				throw new ObjectDisposedException("Timer");
			}

			#endregion

			#region Guard

			if (!_running)
			{
				return;
			}

			#endregion

			// Stop and destroy timer.
			int result = timeKillEvent(_timerId);

			Debug.Assert(result == TIMERR_NOERROR);

			_running = false;

			if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
			{
				SynchronizingObject.BeginInvoke(
					new EventRaiser(OnStopped),
					new object[] {EventArgs.Empty});
			}
			else
			{
				OnStopped(EventArgs.Empty);
			}
		}

		#region Callbacks

		// Callback method called by the Win32 multimedia timer when a timer
		// periodic event occurs.
		private void TimerPeriodicEventCallback(int id, int msg, int user, int param1, int param2)
		{
			if (_synchronizingObject != null)
			{
				_synchronizingObject.BeginInvoke(_tickRaiser, new object[] {EventArgs.Empty});
			}
			else
			{
				OnTick(EventArgs.Empty);
			}
		}

		// Callback method called by the Win32 multimedia timer when a timer
		// one shot event occurs.
		private void TimerOneShotEventCallback(int id, int msg, int user, int param1, int param2)
		{
			if (_synchronizingObject != null)
			{
				_synchronizingObject.BeginInvoke(_tickRaiser, new object[] {EventArgs.Empty});
				Stop();
			}
			else
			{
				OnTick(EventArgs.Empty);
				Stop();
			}
		}

		#endregion

		#region Event Raiser Methods

		// Raises the Disposed event.
		private void OnDisposed(EventArgs e)
		{
			EventHandler handler = Disposed;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		// Raises the Started event.
		private void OnStarted(EventArgs e)
		{
			EventHandler handler = Started;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		// Raises the Stopped event.
		private void OnStopped(EventArgs e)
		{
			EventHandler handler = Stopped;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		// Raises the Tick event.
		private void OnTick(EventArgs e)
		{
			EventHandler handler = Tick;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#endregion

		#region Properties

		/// <summary>
		///     Gets or sets the object used to marshal event-handler calls.
		/// </summary>
		public ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				#region Require

				if (_disposed)
				{
					throw new ObjectDisposedException("Timer");
				}

				#endregion

				return _synchronizingObject;
			}
			set
			{
				#region Require

				if (_disposed)
				{
					throw new ObjectDisposedException("Timer");
				}

				#endregion

				_synchronizingObject = value;
			}
		}

		/// <summary>
		///     Gets or sets the time between Tick events.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		///     If the timer has already been disposed.
		/// </exception>
		public int Period
		{
			get
			{
				#region Require

				if (_disposed)
				{
					throw new ObjectDisposedException("Timer");
				}

				#endregion

				return _period;
			}
			set
			{
				#region Require

				if (_disposed)
				{
					throw new ObjectDisposedException("Timer");
				}
				else if (value < Capabilities.periodMin || value > Capabilities.periodMax)
				{
					throw new ArgumentOutOfRangeException("Period", value,
						"Multimedia Timer period out of range.");
				}

				#endregion

				_period = value;

				if (IsRunning)
				{
					Stop();
					Start();
				}
			}
		}

		/// <summary>
		///     Gets or sets the timer resolution.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		///     If the timer has already been disposed.
		/// </exception>
		/// <remarks>
		///     The resolution is in milliseconds. The resolution increases
		///     with smaller values; a resolution of 0 indicates periodic events
		///     should occur with the greatest possible accuracy. To reduce system
		///     overhead, however, you should use the maximum value appropriate
		///     for your application.
		/// </remarks>
		public int Resolution
		{
			get
			{
				#region Require

				if (_disposed)
				{
					throw new ObjectDisposedException("Timer");
				}

				#endregion

				return _resolution;
			}
			set
			{
				#region Require

				if (_disposed)
				{
					throw new ObjectDisposedException("Timer");
				}
				else if (value < 0)
				{
					throw new ArgumentOutOfRangeException("Resolution", value,
						"Multimedia timer resolution out of range.");
				}

				#endregion

				_resolution = value;

				if (IsRunning)
				{
					Stop();
					Start();
				}
			}
		}

		/// <summary>
		///     Gets the timer mode.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		///     If the timer has already been disposed.
		/// </exception>
		public TimerMode Mode
		{
			get
			{
				#region Require

				if (_disposed)
				{
					throw new ObjectDisposedException("Timer");
				}

				#endregion

				return _mode;
			}
			set
			{
				#region Require

				if (_disposed)
				{
					throw new ObjectDisposedException("Timer");
				}

				#endregion

				_mode = value;

				if (IsRunning)
				{
					Stop();
					Start();
				}
			}
		}

		/// <summary>
		///     Gets a value indicating whether the Timer is running.
		/// </summary>
		public bool IsRunning
		{
			get { return _running; }
		}

		/// <summary>
		///     Gets the timer capabilities.
		/// </summary>
		public static TimerCaps Capabilities
		{
			get { return _caps; }
		}

		#endregion

		#endregion

		#region IComponent Members

		public event EventHandler Disposed;

		public ISite Site
		{
			get { return _site; }
			set { _site = value; }
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		///     Frees timer resources.
		/// </summary>
		public void Dispose()
		{
			#region Guard

			if (_disposed)
			{
				return;
			}

			#endregion

			if (IsRunning)
			{
				Stop();
			}

			_disposed = true;

			OnDisposed(EventArgs.Empty);
		}

		#endregion
	}

	/// <summary>
	///     The exception that is thrown when a timer fails to start.
	/// </summary>
	public class TimerStartException : ApplicationException
	{
		/// <summary>
		///     Initializes a new instance of the TimerStartException class.
		/// </summary>
		/// <param name="message">
		///     The error message that explains the reason for the exception.
		/// </param>
		public TimerStartException(string message) : base(message)
		{
		}
	}
}