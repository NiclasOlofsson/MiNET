using System;
using log4net;
using MiNET.Entities.Behaviors;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Horse : PassiveMob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Horse));

		private int _type = 0;

		public bool IsEating { get; set; }

		public Horse(Level level) : base(EntityType.Horse, level)
		{
			Width = Length = 1.4;
			Height = 1.6;
			var random = new Random((int) DateTime.UtcNow.Ticks);
			_type = random.Next(7);

			Behaviors.Add(new PanicBehavior(60, Speed, 1.2));
			Behaviors.Add(new HorseEatBlockBehavior(100));
			Behaviors.Add(new StrollBehavior(60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior());
			Behaviors.Add(new RandomLookaroundBehavior());
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[2] = new MetadataInt(_type);
			metadata[16] = new MetadataInt(IsEating ? 32 : 0); // 0 or 32?
			return metadata;
		}
	}
}