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

namespace MiNET.Utils.IO
{
	public class CooldownTimer
	{
		public TimeSpan TimeSpan { get; private set; }

		public DateTime ClearingTime { get; private set; }

		public CooldownTimer(long timeSpan) : this(new TimeSpan(timeSpan * TimeSpan.TicksPerMillisecond))
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

		public CooldownTimerAction(long timeSpan, Action<T> callbackAction) : this(new TimeSpan(timeSpan * TimeSpan.TicksPerMillisecond), callbackAction)
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