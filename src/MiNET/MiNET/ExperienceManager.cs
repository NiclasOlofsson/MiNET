using System;
using System.ComponentModel;
using System.Reflection;
using log4net;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET
{
	public class ExperienceManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ExperienceManager));

		public Entity Entity { get; set; }
		public int Experience { get; set; }

		public ExperienceManager(Entity entity)
		{
			Entity = entity;
			ResetExperience();
		}

		public virtual void Kill()
		{
			Experience = 0;
		}
		
		public int getLevel(){
			return 0;
		}

		public virtual void ResetExperience()
		{
			Experience = 0;
		}

		public static string GetDescription(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

			if (attributes.Length > 0)
				return attributes[0].Description;

			return value.ToString();
		}
	}
}
