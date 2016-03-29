using System;
using MiNET.Worlds;
using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Entities.Hostile
{
	public class Slime : HostileMob
	{
		public const byte MetadataSize = 16;

		private byte _size = 1;

		public byte Size
		{
			get { return _size; }
			set
			{
				_size = value;
				Width = Height = Length = _size * 0.51000005;
				HealthManager.MaxHealth = (int)Math.Pow(2, _size);
			}
		}

		public Slime(Level level, byte size = 1) : base((int) EntityType.Slime, level)
		{
			Size = size;
			HealthManager.ResetHealth();
		}

		public override MetadataDictionary GetMetadata()
		{
			var md = base.GetMetadata();
			md[MetadataSize] = new MetadataByte(Size);
			return md;
		}
	}
}