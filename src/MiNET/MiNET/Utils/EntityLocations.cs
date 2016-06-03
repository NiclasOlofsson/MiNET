using System.Collections.Generic;
using System.Numerics;

namespace MiNET.Utils
{
	public class EntityMotions : Dictionary<long, Vector3>
	{
	}

	public class EntityHeadRotations : EntityLocations
	{
	}

	public class EntityLocations : Dictionary<long, PlayerLocation>
	{
	}
}