using System;

namespace MiNET.Utils
{
	public class CooldownTimer
	{
		public TimeSpan TimeSpan { get; private set; }

		public DateTime ClearingTime { get; private set; }

		public CooldownTimer(long timeSpan) : this(new TimeSpan(timeSpan*TimeSpan.TicksPerMillisecond))
		{
		}

		public CooldownTimer(TimeSpan timeSpan)
		{
			TimeSpan = timeSpan;
			Reset();
		}

		public void Reset()
		{
			ClearingTime = DateTime.UtcNow.Add(TimeSpan);
		}

		public bool CanExecute()
		{
			return ClearingTime <= DateTime.UtcNow;
		}


		public bool Execute()
		{
			if (!CanExecute())
			{
				return false;
			}

			Reset();

			return true;
		}
	}

	public class CooldownTimerAction<T> : CooldownTimer where T : class
	{
		public Action<T> CallBackAction { get; set; }

		public CooldownTimerAction(long timeSpan, Action<T> callbackAction) : this(new TimeSpan(timeSpan*TimeSpan.TicksPerMillisecond), callbackAction)
		{
		}

		public CooldownTimerAction(TimeSpan timeSpan, Action<T> callbackAction) : base(timeSpan)
		{
			CallBackAction = callbackAction;
		}

		public bool Execute(T param)
		{
			if (!CanExecute())
			{
				return false;
			}

			CallBackAction?.Invoke(param);

			Reset();

			return true;
		}
	}
}