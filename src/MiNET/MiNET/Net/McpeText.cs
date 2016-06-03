namespace MiNET.Net
{
	public partial class McpeText : Package<McpeText>
	{
		public const byte TypeRaw = 0;
		public const byte TypeChat = 1;
		public const byte TypeTranslation = 2;
		public const byte TypePopup = 3;
		public const byte TypeTip = 4;

		public string source; // = null;
		public string message; // = null;

		partial void AfterEncode()
		{
			switch (type)
			{
				case TypeChat:
					Write(source);
					Write(message);
					break;
				case TypeRaw:
					Write(message);
					break;
				case TypePopup:
				case TypeTip:
					Write(message);
					break;

				case TypeTranslation:
					Write(message);
					// More stuff
					break;
			}
		}

		public override void Reset()
		{
			type = 0;
			source = null;
			message = null;

			base.Reset();
		}

		partial void AfterDecode()
		{
			switch (type)
			{
				case TypeChat:
					source = ReadString();
					message = ReadString();
					break;
				case TypeRaw:
				case TypePopup:
				case TypeTip:
					message = ReadString();
					break;

				case TypeTranslation:
					message = ReadString();
					// More stuff
					break;
			}
		}
	}
}