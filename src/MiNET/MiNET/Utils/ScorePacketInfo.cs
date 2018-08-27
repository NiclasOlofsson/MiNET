using MiNET.Net;
using System.Collections.Generic;

namespace MiNET.Utils
{
	public class ScorePacketInfos : List<ScorePacketInfo> { }

	public class ScorePacketInfo
	{
		public long scoreboardId { get; set; }
		public string objectiveName { get; set; }
		public int score { get; set; }
        public long entityId { get; set; }
        public string fakePlayer { get; set; }
        public byte addType;
	}
}
