using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Herobrine : HostileMob, IEternal
	{
		public Herobrine(Level level) : base(666, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
			HealthManager.MaxHealth = int.MaxValue;
			HealthManager.ResetHealth();
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(383, 666, 16)
			};
		}

		public void Praise(Player player)
		{
			Level.BroadcastMessage(player.DisplayName + " has become one with Herobrine.");
		}

		public int Minions => int.MaxValue;
	}
}
