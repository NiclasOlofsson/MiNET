using System;
using log4net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class SkeletonHorse : HostileMob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (SkeletonHorse));

		private Random _random;

		public SkeletonHorse(Level level) : base(EntityType.SkeletonHorse, level)
		{
			Width = Length = 1.4;
			Height = 1.6;
			_random = new Random((int) DateTime.UtcNow.Ticks);
		}

		public override MetadataDictionary GetMetadata()
		{
			IsInWater = true;
			var metadata = base.GetMetadata();
			metadata[2] = new MetadataInt(_random.Next(7));
			metadata[16] = new MetadataInt(_random.Next(2) == 1 ? 0 : 32); // 0 or 32?
			return metadata;
		}

		public override void OnTick()
		{
			base.OnTick();
			//if (Level.TickTime%20 == 0) BroadcastSetEntityData();
		}
	}
}