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

		public void Reset()
		{
			Reliability = Reliability.Unreliable;
			ReliableMessageNumber = 0;
			SequencingIndex = 0;
			OrderingIndex = 0;
			OrderingChannel = 0;

			HasSplit = false;
			PartCount = 0;
			PartId = 0;
			PartIndex = 0;
		}

		public void Decode()
		{
		}

		public void Encode()
		{
		}
	}
}