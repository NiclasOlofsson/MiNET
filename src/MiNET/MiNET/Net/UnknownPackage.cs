namespace MiNET.Net
{
	public class UnknownPackage : Package<UnknownPackage>
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

	public class SplitPartPackage : Package<UnknownPackage>
	{
		public SplitPartPackage(byte id, byte[] message)
		{
			Message = message;
			Id = id;
		}

		public byte[] Message { get; private set; }
	}
}