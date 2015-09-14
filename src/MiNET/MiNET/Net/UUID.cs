using System;

namespace MiNET.Net
{
	public class UUID
	{
		public UUID()
		{
			Id = Guid.NewGuid();
		}

		public UUID(byte[] rfc4122Bytes)
		{
			//Array.Reverse(rfc4122Bytes, 0, 4);
			//Array.Reverse(rfc4122Bytes, 4, 2);
			//Array.Reverse(rfc4122Bytes, 6, 2);
			Id = new Guid(rfc4122Bytes);
		}

		public Guid Id { get; private set; }

		public byte[] GetBytes()
		{
			var bytes = Id.ToByteArray();

			//Array.Reverse(bytes, 0, 4);
			//Array.Reverse(bytes, 4, 2);
			//Array.Reverse(bytes, 6, 2);

			return bytes;
		}

		public override string ToString()
		{
			//xxxxxxxx-xxxx-Mxxx-Nxxx-xxxxxxxxxxxx 8-4-4-12
			return Id.ToString();
		}
	}
}