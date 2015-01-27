namespace MiNET.Net
{
	public class UnknownPackage : Package
	{
		public UnknownPackage() : this(0, null)
		{
		}

		public UnknownPackage(byte id, byte[] message)
		{
			Message = message;
			Id = id;
		}

		public byte[] Message { get; private set; }

	}
}