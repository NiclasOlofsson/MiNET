using System;
using System.Collections.Generic;
using System.Configuration;

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

}