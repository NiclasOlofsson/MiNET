namespace MiNET
{
	public class PlayerAttribute
	{
		public string Name { get; set; }
		public float MinValue { get; set; }
		public float MaxValue { get; set; }
		public float Value { get; set; }
		public float Default { get; set; }

		public override string ToString()
		{
			return $"Name: {Name}, MinValue: {MinValue}, MaxValue: {MaxValue}, Value: {Value}, Default: {Default}";
		}
	}

	public class EntityAttribute
	{
		public string Name { get; set; }
		public float MinValue { get; set; }
		public float MaxValue { get; set; }
		public float Value { get; set; }

		public override string ToString()
		{
			return $"Name: {Name}, MinValue: {MinValue}, MaxValue: {MaxValue}, Value: {Value}";
		}
	}

}