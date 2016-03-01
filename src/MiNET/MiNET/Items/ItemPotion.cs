using log4net;
using MiNET.Effects;

namespace MiNET.Items
{
	public class ItemPotion : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemPotion));

		public ItemPotion(short metadata) : base(373, metadata)
		{
		}

		public virtual void Consume(Player player)
		{
			Effect e = null;
			switch (Metadata)
			{
				case 5:
					e = new NightVision { Duration = 3600, Level = 0 };
					break;
				case 6:
					e = new NightVision { Duration = 9600, Level = 0 };
					break;
				case 7:
					e = new Invisibility {Duration = 3600, Level = 0};
					break;
				case 8:
					e = new Invisibility {Duration = 9600, Level = 0};
					break;
				case 9:
					e = new JumpBoost {Duration = 3600, Level = 0};
					break;
				case 10:
					e = new JumpBoost {Duration = 9600, Level = 0};
					break;
				case 11:
					e = new JumpBoost {Duration = 1800, Level = 1};
					break;
				case 12:
					e = new FireResistance {Duration = 3600, Level = 0};
					break;
				case 13:
					e = new FireResistance {Duration = 9600, Level = 0};
					break;
				case 14:
					e = new Speed {Duration = 3600, Level = 0};
					break;
				case 15:
					e = new Speed {Duration = 9600, Level = 0};
					break;
				case 16:
					e = new Speed {Duration = 1800, Level = 1};
					break;
				case 17:
					e = new Slowness {Duration = 3600, Level = 0};
					break;
				case 18:
					e = new Slowness {Duration = 4800, Level = 0};
					break;
				case 19:
					e = new WaterBreathing {Duration = 3600, Level = 0};
					break;
				case 20:
					e = new WaterBreathing {Duration = 9600, Level = 0};
					break;
				case 21:
					e = new InstantHealth {Duration = 0, Level = 0};
					break;
				case 22:
					e = new InstantHealth {Duration = 0, Level = 1};
					break;
				case 23:
					e = new InstantDamage {Duration = 0, Level = 0};
					break;
				case 24:
					e = new InstantDamage {Duration = 0, Level = 1};
					break;
				case 25:
					e = new Poison {Duration = 900, Level = 0};
					break;
				case 26:
					e = new Poison {Duration = 2400, Level = 0};
					break;
				case 27:
					e = new Poison {Duration = 440, Level = 1};
					break;
				case 28:
					e = new Regeneration {Duration = 900, Level = 0};
					break;
				case 29:
					e = new Regeneration {Duration = 2400, Level = 0};
					break;
				case 30:
					e = new Regeneration {Duration = 440, Level = 1};
					break;
				case 31:
					e = new Strength {Duration = 3600, Level = 0};
					break;
				case 32:
					e = new Strength {Duration = 9600, Level = 0};
					break;
				case 33:
					e = new Strength {Duration = 1800, Level = 1};
					break;
				case 34:
					e = new Weakness {Duration = 1800, Level = 0};
					break;
				case 35:
					e = new Weakness {Duration = 4800, Level = 0};
					break;
			}

			if (e != null)
			{
				player.SetEffect(e);
			}
		}
	}
}