namespace MiNET.Net
{
	public class UnknownPacket : Packet<UnknownPacket>
	{
		public byte[] Message { get; private set; }

		public UnknownPacket() : this(0, null)
		{
		}

		public UnknownPacket(byte id, byte[] message)
		{
			Message = message;
			Id = id;
		}
	}
}