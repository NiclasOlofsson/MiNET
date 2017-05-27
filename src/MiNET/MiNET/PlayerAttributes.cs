using System;
using System.Collections.Generic;

namespace MiNET
{
	public class PlayerAttributes : Dictionary<string, PlayerAttribute>
	{
	}

	public class EntityAttributes : Dictionary<string, EntityAttribute>
	{
	}

	public class Links : List<Tuple<long, long>>
	{
	}

	public class GameRules : Dictionary<string, GameRule>
	{
	}
}