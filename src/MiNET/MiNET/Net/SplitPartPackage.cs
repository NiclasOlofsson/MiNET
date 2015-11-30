namespace MiNET.Net
{
	public class SplitPartPackage : Package<SplitPartPackage>
	{
		public byte[] Message { get; set; }

		public SplitPartPackage()
		{
		}

		public override void Reset()
		{
			base.Reset();
			Message = null;
		}
	}
}