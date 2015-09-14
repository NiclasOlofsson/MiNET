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

	public class SplitPartPackage : Package<SplitPartPackage>
	{
		public SplitPartPackage()
		{
		}

		public override void Reset()
		{
			base.Reset();
			Message = null;
		}

		public byte[] Message { get; set; }
	}
}