namespace MiNET.Net
{
	public class UnknownPackage : Package<UnknownPackage>
	{
		public byte[] Message { get; private set; }

		public UnknownPackage() : this(0, null)
		{
		}

		public UnknownPackage(byte id, byte[] message)
		{
			Message = message;
			Id = id;
		}
	}
}