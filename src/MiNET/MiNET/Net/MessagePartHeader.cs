using MiNET.Utils;

namespace MiNET.Net
{
	public class MessagePartHeader
	{
		public Reliability Reliability { get; set; }
		public Int24 ReliableMessageNumber { get; set; }
		public Int24 SequencingIndex { get; set; }
		public Int24 OrderingIndex { get; set; }
		public byte OrderingChannel { get; set; }

		public bool HasSplit { get; set; }
		public int PartCount { get; set; }
		public short PartId { get; set; }
		public int PartIndex { get; set; }

		public void Decode()
		{
		}

		public void Encode()
		{
		}
	}
}