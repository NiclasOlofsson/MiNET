namespace MiNET.Network
{
	public class ConnectionRequestAcceptedManual : Package
	{
		private readonly short _port;
		private readonly long _sessionId;

		public ConnectionRequestAcceptedManual(short port, long sessionId)
		{
			Id = 0x10;
			_port = port;
			_sessionId = sessionId;
		}

		protected override void EncodePackage()
		{
			Write((byte) 0x10);
			Write(new byte[] { 0x04, 0x3f, 0x57, 0xfe }); //Cookie
			Write((byte) 0xcd); //Security flags
			Write(_port);
			PutDataArray();
			Write(new byte[] { 0x00, 0x00 });
			Write(_sessionId);
			Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x04, 0x44, 0x0b, 0xa9 });
		}

		private void PutDataArray()
		{
			byte[] unknown1 = { 0xf5, 0xff, 0xff, 0xf5 };
			byte[] unknown2 = { 0xff, 0xff, 0xff, 0xff };

			Write(new Int24(unknown1.Length));
			Write(unknown1);

			for (int i = 0; i < 9; i++)
			{
				Write(new Int24(unknown2.Length));
				Write(unknown2);
			}
		}
	}
}
