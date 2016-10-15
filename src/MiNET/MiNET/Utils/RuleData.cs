using System.Collections.Generic;

namespace MiNET.Utils
{
	public class Rules : List<RuleData>
	{
	}

	public class RuleData
	{
		public string Name { get; set; }
		public bool Unknown1 { get; set; }
		public bool Unknown2 { get; set; }

		public override string ToString()
		{
			return $"Name: {Name}, Unknown1: {Unknown1}, Unknown2: {Unknown2}";
		}
	}
}