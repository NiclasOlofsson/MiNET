namespace MiNET.Net
{
	public class InternalPing : Packet<InternalPing>
	{
		public InternalPing()
		{
			Id = 0xff;
		}

		protected override void EncodePacket()
		{
		}

		protected override void DecodePacket()
		{
		}
	}
}