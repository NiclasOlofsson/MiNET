using System;
using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Sheep : PassiveMob, IAgeable
	{
		public Sheep(Level level) : base(EntityType.Sheep, level)
		{
			Width = Length = 0.9;
			Height = 1.3;
			HealthManager.MaxHealth = 80;
			HealthManager.ResetHealth();

			Behaviors.Add(new PanicBehavior(60, Speed, 1.25));
			Behaviors.Add(new EatBlockBehavior());
			Behaviors.Add(new StrollBehavior(60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior());
			Behaviors.Add(new RandomLookaroundBehavior());
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[16] = new MetadataInt(32);
			return metadata;
		}

		public override Item[] GetDrops()
		{
			Random random = new Random();
			return new[]
			{
				ItemFactory.GetItem(35, 0, 1),
				ItemFactory.GetItem(423, 0, random.Next(1, 3)),
			};
		}
	}

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