using System.Collections.Generic;

namespace MiNET.Utils
{
	public class EntityMotions : Dictionary<int, Vector3>
	{
	}

	public class EntityHeadRotations : EntityLocations
	{
	}

	public class EntityLocations : Dictionary<int, PlayerLocation>
	{
	}
}