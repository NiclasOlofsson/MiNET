using MiNET.Net;
using System.Collections.Generic;

namespace MiNET.Utils
{
	public class ScorePacketInfos : List<ScorePacketInfo> { }

	public class ScorePacketInfo
	{
		public UUID uuid { get; set; }
		public string objectiveName { get; set; }
		public short score { get; set; }
	}
}
