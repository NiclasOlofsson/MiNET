using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Jose;
using log4net;
using MiNET.Net;

namespace MiNET.Utils
{
	public static class CryptoUtils
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (CryptoUtils));

		public static byte[] GetDerEncoded(this ECDiffieHellmanPublicKey key)
		{
			byte[] asn = new byte[24]
			{
				0x30, 0x76, 0x30, 0x10, 0x6, 0x7, 0x2a, 0x86, 0x48, 0xce, 0x3d, 0x2,
				0x1, 0x6, 0x5, 0x2b, 0x81, 0x4, 0x0, 0x22, 0x3, 0x62, 0x0, 0x4
			};

			return asn.Concat(key.ToByteArray().Skip(8)).ToArray();
		}

		public static ECDiffieHellmanPublicKey CreateEcDiffieHellmanPublicKey(string clientPubKeyString)
		{
			byte[] clientPublicKeyBlob = Base64Url.Decode(clientPubKeyString);
			clientPublicKeyBlob = FixPublicKey(clientPublicKeyBlob.Skip(23).ToArray());

			ECDiffieHellmanPublicKey clientKey = ECDiffieHellmanCngPublicKey.FromByteArray(clientPublicKeyBlob, CngKeyBlobFormat.EccPublicBlob);
			return clientKey;
		}

		private static byte[] FixPublicKey(byte[] publicKeyBlob)
		{
			var keyType = new byte[] {0x45, 0x43, 0x4b, 0x33};
			var keyLength = new byte[] {0x30, 0x00, 0x00, 0x00};

			return keyType.Concat(keyLength).Concat(publicKeyBlob.Skip(1)).ToArray();
		}

		public static ECDiffieHellmanPublicKey ImportEccPublicKeyFromCertificate(X509Certificate2 cert)
		{
			var keyType = new byte[] {0x45, 0x43, 0x4b, 0x33};
			var keyLength = new byte[] {0x30, 0x00, 0x00, 0x00};
			var key = cert.PublicKey.EncodedKeyValue.RawData.Skip(1);
			var keyImport = keyType.Concat(keyLength).Concat(key).ToArray();

			//Assert.AreEqual(privateKey, keyImport);

			return ECDiffieHellmanCngPublicKey.FromByteArray(keyImport, CngKeyBlobFormat.EccPublicBlob);
		}

		/// <summary>
		/// Used to create a CngKey that can be used to verify JWT content.
		/// </summary>
		/// <param name="clientPubKeyString"></param>
		/// <returns></returns>
		public static CngKey ImportECDsaCngKeyFromString(string clientPubKeyString)
		{
			byte[] clientPublicKeyBlob = Base64Url.Decode(clientPubKeyString);
			byte[] key = clientPublicKeyBlob.Skip(23).ToArray();

			var keyType = new byte[] {0x45, 0x43, 0x53, 0x33};
			var keyLength = new byte[] {0x30, 0x00, 0x00, 0x00};

			var keyImport = keyType.Concat(keyLength).Concat(key.Skip(1)).ToArray();

			var cngKey = CngKey.Import(keyImport, CngKeyBlobFormat.EccPublicBlob);
			var crypto = new ECDsaCng(cngKey);

			return crypto.Key;
		}

		public static byte[] Encrypt(byte[] payload, CryptoContext cryptoContext)
		{
			var csEncrypt = cryptoContext.CryptoStreamOut;
			var output = cryptoContext.OutputStream;
			output.Position = 0;
			output.SetLength(0);

			using (MemoryStream hashStream = new MemoryStream())
			{
				// hash

				SHA256Managed crypt = new SHA256Managed();

				hashStream.Write(BitConverter.GetBytes(Interlocked.Increment(ref cryptoContext.SendCounter)), 0, 8);
				hashStream.Write(payload, 0, payload.Length);
				hashStream.Write(cryptoContext.Algorithm.Key, 0, cryptoContext.Algorithm.Key.Length);
				var hashBuffer = hashStream.ToArray();

				byte[] validationCheckSum = crypt.ComputeHash(hashBuffer, 0, hashBuffer.Length);

				byte[] content = payload.Concat(validationCheckSum.Take(8)).ToArray();

				csEncrypt.Write(content, 0, content.Length);
				csEncrypt.Flush();
			}

			return output.ToArray();
		}

		public static byte[] Decrypt(byte[] payload, CryptoContext cryptoContext)
		{
			byte[] checksum;
			byte[] clearBytes;

			using (MemoryStream clearBuffer = new MemoryStream())
			{
				//if (Log.IsDebugEnabled)
				//	Log.Debug($"Full payload\n{Package.HexDump(payload)}");

				var input = cryptoContext.InputStream;
				var csDecrypt = cryptoContext.CryptoStreamIn;

				input.Position = 0;
				input.SetLength(0);
				input.Write(payload, 0, payload.Length);
				input.Position = 0;

				var buffer = new byte[payload.Length];
				var read = csDecrypt.Read(buffer, 0, buffer.Length);
				if (read <= 0) Log.Warn("Read 0 lenght from crypto stream");
				clearBuffer.Write(buffer, 0, read);
				csDecrypt.Flush();

				var fullResult = clearBuffer.ToArray();

				//if (Log.IsDebugEnabled)
				//	Log.Debug($"Full content\n{Package.HexDump(fullResult)}");

				clearBytes = (byte[]) fullResult.Take(fullResult.Length - 8).ToArray();
				checksum = fullResult.Skip(fullResult.Length - 8).ToArray();
			}

			return clearBytes;
		}
	}
}