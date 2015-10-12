using System;

namespace MiNET.Net
{
	public class UUID
	{
		public UUID(byte[] rfc4122Bytes)
		{
			//Array.Reverse(rfc4122Bytes, 0, 4);
			//Array.Reverse(rfc4122Bytes, 4, 2);
			//Array.Reverse(rfc4122Bytes, 6, 2);
			Id = new Guid(rfc4122Bytes);
		}

		private Guid Id { get; set; }

		public byte[] GetBytes()
		{
			var bytes = Id.ToByteArray();

			//Array.Reverse(bytes, 0, 4);
			//Array.Reverse(bytes, 4, 2);
			//Array.Reverse(bytes, 6, 2);

			return bytes;
		}

		protected bool Equals(UUID other)
		{
			return Id.Equals(other.Id);
		}


		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((UUID) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			//xxxxxxxx-xxxx-Mxxx-Nxxx-xxxxxxxxxxxx 8-4-4-12
			return Id.ToString();
		}
	}
}