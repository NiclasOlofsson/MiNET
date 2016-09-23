namespace MiNET
{
	public class PlayerAttribute
	{
		public string Name { get; set; }
		public float MinValue { get; set; }
		public float MaxValue { get; set; }
		public float Value { get; set; }
		public float Unknown { get; set; }

		public override string ToString()
		{
			return $"Name: {Name}, MinValue: {MinValue}, MaxValue: {MaxValue}, Value: {Value}, Unknown: {Unknown}";
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