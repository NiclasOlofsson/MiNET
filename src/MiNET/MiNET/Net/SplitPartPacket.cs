namespace MiNET.Net
{
	public class SplitPartPacket : Packet<SplitPartPacket>
	{
		public byte[] Message { get; set; }
		public int SplitId { get; set; }
		public int SplitCount { get; set; }
		public int SplitIdx { get; set; }

		public SplitPartPacket()
		{
		}

		public override void Reset()
		{
			base.Reset();
			SplitId = -1;
			SplitCount = -1;
			SplitIdx = -1;

			Message = null;
		}
	}
}