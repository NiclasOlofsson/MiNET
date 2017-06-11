using System.Collections.Generic;

namespace MiNET.Utils
{
	public class ResourcePackInfos : List<ResourcePackInfo>
	{
	}

	public class ResourcePackInfo
	{
		public PackIdVersion PackIdVersion { get; set; }
		public ulong Size { get; set; }
	}

	public class ResourcePackIdVersions : List<PackIdVersion>
	{
	}

	public class PackIdVersion
	{
		public string Id { get; set; }
		public string Version { get; set; }
	}

    public class ResourcePackIds : List<string>
    {
    }
}