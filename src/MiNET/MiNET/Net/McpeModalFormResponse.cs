namespace MiNET.Net
{
	public partial class McpeModalFormResponse
	{
		public string data; // = null;
		public byte? cancelReason; // = null;

		partial void AfterDecode()
		{
			if (ReadBool())
			{
				data = ReadString();
			}

			if (ReadBool())
			{
				cancelReason = ReadByte();
			}
		}

		partial void AfterEncode()
		{
			var hasData = !string.IsNullOrEmpty(data);
			Write(hasData);
			if (hasData)
			{
				Write(data);
			}

			Write(cancelReason.HasValue);
			if (cancelReason.HasValue)
			{
				Write(cancelReason.Value);
			}
		}
	}
}
