using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Paddings;

namespace MiNET.Utils
{
	public class CryptoContext
	{
		public bool UseEncryption;

		//public RijndaelManaged Algorithm { get; set; }

		public IBufferedCipher Decryptor { get; set; }
		//public MemoryStream InputStream { get; set; }
		//public CryptoStream CryptoStreamIn { get; set; }

		public IBufferedCipher Encryptor { get; set; }
		//public MemoryStream OutputStream { get; set; }
		//public CryptoStream CryptoStreamOut { get; set; }

		public long SendCounter = -1;

		public AsymmetricCipherKeyPair ClientKey { get; set; }
		public byte[] Key { get; set; }
	}
}