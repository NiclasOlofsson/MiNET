namespace MiNET.Plugins
{
	public enum CommandParameterType
	{
		Bool = -3,
		Enum = -1,

		Unknown = 0,

		Int = 1,
		Float = 3,
		Value = 4,
		WildcardInt = 5,
		Operator = 6,
		CompareOperator = 7,
		Target = 8,

		WildcardTarget = 10,

		Filepath = 17,

		FullIntegerRange = 23,

		EquipmentSlot = 43,
		String = 44,

		IntPosition = 52,
		Position = 53,

		Message = 55,

		Rawtext = 58,

		Json = 62,

		BlockStates = 71,

		Command = 74,


		EnumFlag = 0x200000,
		PostfixFlag = 0x1000000,
		SoftEnumFlag = 0x4000000
	}
}
