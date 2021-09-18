using System.Collections.Generic;

namespace MiNET.Net
{
	public class Experiments : List<Experiments.Experiment>
	{
		
		public class Experiment
		{
			public string Name { get; }
			public bool Enabled { get; }

			public Experiment(string name, bool enabled)
			{
				Name = name;
				Enabled = enabled;
			}
		}
	}
}