using System.IO;
using System.Security.Cryptography;

namespace MiNET.Utils
{
	public class CryptoContext
	{
		public bool UseEncryption;

		public RijndaelManaged Algorithm { get; set; }

		public ICryptoTransform Decryptor { get; set; }
		public MemoryStream InputStream { get; set; }
		public CryptoStream CryptoStreamIn { get; set; }

		public ICryptoTransform Encryptor { get; set; }
		public MemoryStream OutputStream { get; set; }
		public CryptoStream CryptoStreamOut { get; set; }

		public long SendCounter = -1;

		public CngKey ClientKey { get; set; }

	}
}