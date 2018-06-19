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

	public class GameRules : HashSet<GameRule>
	{
	}

	public class Blockstates : Dictionary<int, Blockstate>
	{
	}

	public class Blockstate
	{
		public int Id { get; set; }
		public int RuntimeId { get; set; }
		public string Name { get; set; }
		public short Data { get; set; }
	}
}