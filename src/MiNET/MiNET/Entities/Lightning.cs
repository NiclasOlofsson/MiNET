﻿using System.Runtime.Remoting.Messaging;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Lightning : Entity
	{
		public Lightning(Level level) : base(93, level)
		{
		}

		public override void OnTick()
		{
			base.OnTick();

			if (!IsSpawned) return;

			if (Age > 40)
			{
				DespawnEntity();
			}
		}
	}
}