namespace MiNET.Network
{
	public class UnknownPackage : Package
	{
		public UnknownPackage(byte id, byte[] message)
		{
			Message = message;
			Id = id;
		}

		public byte[] Message { get; private set; }
	}
}