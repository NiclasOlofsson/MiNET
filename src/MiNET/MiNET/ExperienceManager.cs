using MiNET.Net;
using MiNET.Worlds;
using System;

namespace MiNET
{
	public class ExperienceManager
	{
		public Player Player { get; set; }
		public float ExperienceLevel { get; set; } = 0f;
		public float Experience { get; set; } = 0f;

		public ExperienceManager(Player player)
		{
			Player = player;
		}

		public void AddExperience(float xp, bool send = true)
		{
			var xpToNextLevel = GetXpToNextLevel();

			if (xp + Experience < xpToNextLevel)
			{
				Experience += xp;
			}
			else
			{
				var expDiff = Experience + xp - xpToNextLevel;
				ExperienceLevel++;
				Experience = 0;
				AddExperience(expDiff, false);
			}

			if (send)
			{
				SendAttributes();
			}
		}

		public void RemoveExperienceLevels(float levels)
		{
			var currentXp = CalculateXp();
			ExperienceLevel = Experience - Math.Abs(levels);
			Experience = GetXpToNextLevel() * currentXp;
		}

		protected virtual float GetXpToNextLevel()
		{
			float xpToNextLevel = 0;
			if (ExperienceLevel >= 0 && ExperienceLevel <= 15)
			{
				xpToNextLevel = 2 * ExperienceLevel + 7;
			}
			else if (ExperienceLevel > 15 && ExperienceLevel <= 30)
			{
				xpToNextLevel = 5 * ExperienceLevel - 38;
			}
			else if (ExperienceLevel > 30)
			{
				xpToNextLevel = 9 * ExperienceLevel - 158;
			}

			return xpToNextLevel;
		}

		protected virtual float CalculateXp()
		{
			return Experience / GetXpToNextLevel();
		}

		public virtual PlayerAttributes AddExperienceAttributes(PlayerAttributes attributes)
		{
			attributes["minecraft:player.experience"] = new PlayerAttribute
			{
				Name = "minecraft:player.experience",
				MinValue = 0,
				MaxValue = 1,
				Value = CalculateXp(),
				Default = 0,
			};
			attributes["minecraft:player.level"] = new PlayerAttribute
			{
				Name = "minecraft:player.level",
				MinValue = 0,
				MaxValue = 24791,
				Value = ExperienceLevel,
				Default = 0,
			};
			return attributes;
		}

		public virtual void SendAttributes()
		{
			var attributes = new PlayerAttributes();
			attributes = AddExperienceAttributes(attributes);

			McpeUpdateAttributes attributesPackate = McpeUpdateAttributes.CreateObject();
			attributesPackate.runtimeEntityId = EntityManager.EntityIdSelf;
			attributesPackate.attributes = attributes;
			Player.SendPacket(attributesPackate);
		}
	}
}