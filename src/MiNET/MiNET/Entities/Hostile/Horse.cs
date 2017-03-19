using System;
using log4net;
using MiNET.Entities.Passive;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Horse : PassiveMob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Horse));

		private Random _random;
		private int _type = 0;

		public Horse(Level level) : base(EntityType.Horse, level)
		{
			Width = Length = 1.4;
			Height = 1.6;
			_random = new Random((int) DateTime.UtcNow.Ticks);
			_type = _random.Next(7);
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[2] = new MetadataInt(_type);
			metadata[16] = new MetadataInt(_random.Next(2) == 1 ? 0 : 32); // 0 or 32?
			return metadata;
		}
	}
}